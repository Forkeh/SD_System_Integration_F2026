using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Ex_07_To_Do_List.Data;
using Ex_07_To_Do_List.Helpers;
using Ex_07_To_Do_List.Models;
using Ex_07_To_Do_List.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


// SQLite
builder.Services.AddDbContext<AppDbContext>(opt =>
    opt.UseSqlite("Data Source=Data/users.db"));

// Redis
builder.Services.AddSingleton<IConnectionMultiplexer>(
    ConnectionMultiplexer.Connect(builder.Configuration["Redis:Connection"]!));
builder.Services.AddScoped<BlacklistService>();

// JWT
builder.Services.AddScoped<TokenService>();

var jwtSecret = builder.Configuration["Jwt:Secret"]!;
var jwtIssuer = builder.Configuration["Jwt:Issuer"]!;

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtIssuer,
            ValidateAudience = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtSecret))
        };

        // Hook into token validation to check the blacklist
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
            },


            // Custom HTTP response message
            OnChallenge = async ctx =>
            {
                if (ctx.AuthenticateFailure is not null)
                {
                    ctx.HandleResponse(); // suppress the default 401
                    ctx.Response.StatusCode = 401;
                    ctx.Response.ContentType = "application/json";
                    await ctx.Response.WriteAsync(
                        $"{{\"error\": \"{ctx.AuthenticateFailure.Message}\"}}");
                }
            }
        };
    });


builder.Services.AddAuthorization();

var app = builder.Build();

// Seed DB
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
    Seeder.SeedUsers(db);
}

app.UseAuthentication();
app.UseAuthorization();

app.UseHttpsRedirection();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}


// --- Auth endpoints ---
app.MapPost("/auth/login", async (LoginRequest req, AppDbContext db, TokenService tokens) =>
{
    var user = await db.Users.FirstOrDefaultAsync(u => u.Username == req.Username);

    if (user is null || !BCrypt.Net.BCrypt.Verify(req.Password, user.PasswordHash))
    {
        return Results.Unauthorized();
    }

    var token = tokens.CreateToken(user, out _);
    return Results.Ok(new { token });
});

app.MapPost("/auth/logout", async (HttpContext ctx, BlacklistService blacklist) =>
{
    var jti = ctx.User.FindFirstValue(JwtRegisteredClaimNames.Jti);
    var expClaim = ctx.User.FindFirstValue(JwtRegisteredClaimNames.Exp);

    if (jti is null || expClaim is null)
    {
        return Results.BadRequest();
    }

    // Calculate remaining lifetime so TTL isn't longer than needed
    var exp = DateTimeOffset.FromUnixTimeSeconds(long.Parse(expClaim));
    var ttl = exp - DateTimeOffset.UtcNow;
    if (ttl > TimeSpan.Zero)
    {
        await blacklist.BlacklistAsync(jti, ttl);
    }

    return Results.Ok(new { message = "Logged out." });
}).RequireAuthorization();


app.MapGet("/tasks", (int page = 1, int size = 5) =>
    {
        var paged = Tasks.TasksList
            .Skip((page - 1) * size)
            .Take(size)
            .ToList();

        return Results.Ok(new
        {
            Page = page,
            Size = size,
            TotalCount = Tasks.TasksList.Count,
            TotalPages = (int)Math.Ceiling(Tasks.TasksList.Count / (double)size),
            Data = paged
        });
    }).WithName("GetTasks")
    .RequireAuthorization();

app.MapGet("/tasks/{id}", (int id) =>
{
    var task = Tasks.TasksList.FirstOrDefault(t => t.Id == id);
    return task is null ? Results.NotFound() : Results.Ok(task);
}).WithName("GetTaskById");

app.MapPost("/tasks", (TaskDto task) =>
    {
        task.Links = TaskDto.GenerateLinks(task.Id);
        Tasks.TasksList.Add(task);
        return Results.Created($"/tasks/{task.Id}", task);
    }).WithName("CreateTask")
    .RequireAuthorization();
;

app.MapPut("/tasks/{id}", (int id, TaskDto updatedTask) =>
    {
        var index = Tasks.TasksList.FindIndex(t => t.Id == id);
        if (index == -1)
        {
            return Results.NotFound();
        }

        updatedTask.Links = TaskDto.GenerateLinks(id);
        Tasks.TasksList[index] = updatedTask;
        return Results.Ok(updatedTask);
    }).WithName("UpdateTaskById")
    .RequireAuthorization();
;

app.MapDelete("/tasks/{id}", (int id) =>
    {
        var task = Tasks.TasksList.FirstOrDefault(t => t.Id == id);
        if (task is null)
        {
            return Results.NotFound();
        }

        Tasks.TasksList.Remove(task);
        return Results.NoContent();
    }).WithName("DeleteTaskById")
    .RequireAuthorization();
;

app.Run();