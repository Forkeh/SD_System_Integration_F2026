# JWT To-Do List API

A secure ASP.NET Core Minimal API that demonstrates **JWT-based authentication** with a **token blacklisting (revocation) strategy** backed by Redis.

---

## What is a JWT?

A **JSON Web Token (JWT)** is a compact, URL-safe token format used to securely transmit information between parties as a JSON object. It is most commonly used for **stateless authentication**: once a user logs in, the server hands them a signed token. On every subsequent request the client sends that token, and the server validates it — **without needing to look anything up in a database**.

### Structure

A JWT consists of three Base64URL-encoded parts separated by dots:

```
<Header>.<Payload>.<Signature>
```

| Part | Description |
|------|-------------|
| **Header** | Algorithm used to sign the token (e.g. `HS256`) and the token type (`JWT`) |
| **Payload** | Claims — statements about the user and metadata (e.g. `sub`, `jti`, `exp`, `name`) |
| **Signature** | HMAC/RSA signature over `Header.Payload` using a secret key — proves the token has not been tampered with |

### Key Claims Used in This Project

| Claim | Name | Purpose |
|-------|------|---------|
| `sub` | Subject | The user's database ID |
| `jti` | JWT ID | A unique identifier for this specific token (a UUID) |
| `exp` | Expiration | Unix timestamp after which the token is invalid |
| `name` | Name | The user's username |

### Why JWTs are Stateless — and Why That's a Problem

Because the server only needs the **secret key** to verify a JWT, it never has to store tokens anywhere. This scales beautifully — any server in a cluster can validate any token independently.

The downside is the flip side of that same coin: **you cannot invalidate a token before it expires**. If a user logs out, or a token is stolen, the server has no built-in mechanism to reject that specific token — it will keep passing validation until `exp` is reached.

This is the core problem that token revocation strategies solve.

---

## Token Revocation Strategies

### Whitelist Strategy

In a **whitelist** approach, every issued token is stored server-side (e.g. in a database or cache). On every request, the server checks whether the incoming token exists in the whitelist before allowing access.

- ✅ Full control — you always know exactly which tokens are active  
- ❌ **Defeats the purpose of stateless JWTs** — every request now requires a storage lookup  
- ❌ Higher latency and infrastructure overhead  
- ❌ Storage grows with every login  

The whitelist strategy is essentially equivalent to traditional session-based authentication, just with a JWT wrapper around it.

---

### Blacklist Strategy *(used in this project)*

In a **blacklist** approach, tokens are issued and used freely — the server only records a token when it needs to be **revoked** (e.g. on logout). Every request then checks only against this smaller, short-lived list.

- ✅ Stays close to the stateless ideal — the list is empty in the normal case  
- ✅ Storage is minimal — only revoked tokens are tracked  
- ✅ Entries can **auto-expire**: a blacklisted token only needs to be remembered until its own `exp` timestamp, after which it would be invalid anyway  
- ❌ Still requires a storage lookup on every request (but the dataset is tiny)  
- ❌ If the blacklist store goes down, revocations are not enforced  

This project implements the blacklist strategy using **Redis** as the backing store — an ideal fit because Redis supports key-level TTLs, so entries clean themselves up automatically.

---

## How This Project Implements It

### 1. Token Creation — `TokenService`

```csharp
public string CreateToken(User user, out string jti)
{
    jti = Guid.NewGuid().ToString(); // unique token ID

    var claims = new[]
    {
        new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
        new Claim(JwtRegisteredClaimNames.Jti, jti),
        new Claim(ClaimTypes.Name, user.Username)
    };

    var token = new JwtSecurityToken(
        _issuer,
        claims: claims,
        expires: DateTime.UtcNow.AddMinutes(ExpiryMinutes),
        signingCredentials: creds);

    return new JwtSecurityTokenHandler().WriteToken(token);
}
```

Every token is issued with a **`jti` (JWT ID)** — a randomly generated UUID. This is the unique handle used to revoke that specific token later.

---

### 2. Token Blacklisting — `BlacklistService`

```csharp
public Task BlacklistAsync(string jti, TimeSpan ttl)
{
    return _db.StringSetAsync($"blacklist:{jti}", "1", ttl);
}

public async Task<bool> IsBlacklistedAsync(string jti)
{
    return await _db.KeyExistsAsync($"blacklist:{jti}");
}
```

When a token is revoked, its `jti` is written to Redis with a **TTL equal to the token's remaining lifetime**. Once the token would have expired naturally, Redis automatically deletes the key — no manual cleanup needed.

---

### 3. Logout Endpoint — `POST /auth/logout`

```
POST /auth/logout
Authorization: Bearer <token>
```

```csharp
app.MapPost("/auth/logout", async (HttpContext ctx, BlacklistService blacklist) =>
{
    var jti = ctx.User.FindFirstValue(JwtRegisteredClaimNames.Jti);
    var expClaim = ctx.User.FindFirstValue(JwtRegisteredClaimNames.Exp);

    var exp = DateTimeOffset.FromUnixTimeSeconds(long.Parse(expClaim));
    var ttl = exp - DateTimeOffset.UtcNow;

    if (ttl > TimeSpan.Zero)
    {
        await blacklist.BlacklistAsync(jti, ttl);
    }

    return Results.Ok(new { message = "Logged out." });
}).RequireAuthorization();
```

The TTL is calculated as the **remaining lifetime of the token** (`exp - now`), so Redis never stores a key longer than necessary.

---

### 4. Blacklist Check on Every Request

The check is hooked directly into the JWT authentication middleware via `OnTokenValidated`:

```csharp
options.Events = new JwtBearerEvents
{
    OnTokenValidated = async ctx =>
    {
        var blacklist = ctx.HttpContext.RequestServices
            .GetRequiredService<BlacklistService>();

        var jti = ctx.Principal?.FindFirstValue(JwtRegisteredClaimNames.Jti);

        if (jti is null || await blacklist.IsBlacklistedAsync(jti))
        {
            ctx.Fail("Token has been revoked.");
        }
    }
};
```

This fires **after** the standard JWT validation (signature, issuer, expiry) passes. If the `jti` is found in Redis, the request is rejected with a `401 Unauthorized`.

---

## Architecture Overview

```
Client
  │
  │  POST /auth/login  ──────────────────────────────────────────────────────────┐
  │  { username, password }                                                       │
  │                                                                               ▼
  │                                                                       AppDbContext (SQLite)
  │                                                                       BCrypt.Verify(password)
  │                                                                       TokenService.CreateToken()
  │◄─────────────────────────────────────────────────────────────────────{ token: "eyJ..." }
  │
  │  POST /auth/logout  ─────────────────────────────────────────────────────────┐
  │  Authorization: Bearer <token>                                                │
  │                                                                               ▼
  │                                                                       BlacklistService
  │                                                                       Redis SET blacklist:<jti> TTL
  │◄─────────────────────────────────────────────────────────────────────{ message: "Logged out." }
  │
  │  GET /tasks  ────────────────────────────────────────────────────────────────┐
  │  Authorization: Bearer <token>                                                │
  │                                                         JWT Middleware        │
  │                                                         1. Validate signature │
  │                                                         2. Check expiry       │
  │                                                         3. Check blacklist ◄──┘
  │◄──────────────────────────────────────────────────────── 200 OK  /  401 Unauthorized
```

---

## Tech Stack

| Concern | Technology |
|---------|-----------|
| Framework | ASP.NET Core 9 Minimal API |
| Authentication | `Microsoft.AspNetCore.Authentication.JwtBearer` |
| Token signing | HMAC-SHA256 (`HS256`) |
| User store | SQLite via Entity Framework Core |
| Password hashing | BCrypt (`BCrypt.Net-Next`) |
| Token revocation | Redis via `StackExchange.Redis` |
| Redis runtime | Docker (`redis:7-alpine`) |

---

## Configuration (`appsettings.json`)

```json
{
  "Jwt": {
    "Secret": "your-super-secret-key-min-32-chars!!",
    "Issuer": "jwt-todo-api",
    "ExpiryMinutes": "60"
  },
  "Redis": {
    "Connection": "localhost:6379"
  }
}
```

> ⚠️ **Never commit a real secret to source control.** Use environment variables or a secrets manager in production.

---

## Running the Project

### 1. Start Redis

```bash
docker-compose up -d
```

### 2. Run the API

```bash
dotnet run
```

The database is created and seeded automatically on startup.

**Default seeded user:**

| Username | Password |
|----------|----------|
| `brian`  | `secret123` |

### 3. Authenticate

```http
POST /auth/login
Content-Type: application/json

{
  "username": "brian",
  "password": "secret123"
}
```

Use the returned token as a **Bearer token** on all subsequent requests:

```http
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

### 4. Logout (revoke the token)

```http
POST /auth/logout
Authorization: Bearer <your-token>
```

After this call, the **same token will be rejected** even though it has not expired yet.

