# Exercise 02 – gRPC Echo

This exercise demonstrates a basic gRPC request/reply pattern using .NET 9. It consists of two projects:

- **GrpcEchoServer** – an ASP.NET Core gRPC server that echoes messages back to the caller
- **GrpcEchoClient** – a .NET console application that sends a message (and optional timestamp) to the server and prints the response

---

## Project Structure

```
Ex02 Echo/
├── GrpcEchoServer/
│   ├── Program.cs               # Server entry point & middleware setup
│   ├── Protos/echo.proto        # Protobuf service & message definitions
│   └── Services/EchoService.cs  # gRPC service implementation
└── GrpcEchoClient/
    ├── Program.cs               # Client entry point & call logic
    └── Protos/echo.proto        # Same proto file (client-side code generation)
```

---

## The Protobuf Contract (`echo.proto`)

Both projects share the same service contract defined in their respective `Protos/echo.proto` files:

```protobuf
syntax = "proto3";

option csharp_namespace = "GrpcEcho";

package echo;

import "google/protobuf/timestamp.proto";

service Echo {
  rpc Echo (EchoRequest) returns (EchoResponse);
}

message EchoRequest {
  string message   = 1;
  google.protobuf.Timestamp time_stamp = 2;
}

message EchoResponse {
  string message   = 1;
  google.protobuf.Timestamp time_stamp = 2;
}
```

Key points:
- A single **unary RPC** method `Echo` takes an `EchoRequest` and returns an `EchoResponse`.
- Both request and response carry a `string message` and an optional `google.protobuf.Timestamp`.
- The `csharp_namespace` option means the generated C# classes live in the `GrpcEcho` namespace.
- At build time, `Grpc.Tools` (via the `<Protobuf>` MSBuild item) compiles the `.proto` file into strongly-typed C# classes — server-side stubs for the server project and client-side stubs for the client project.

---

## GrpcEchoServer

### Dependencies (`GrpcEchoServer.csproj`)

| Package | Role |
|---|---|
| `Grpc.AspNetCore` | Integrates gRPC into ASP.NET Core (includes Kestrel HTTP/2 support, routing, DI helpers, and Protobuf tooling) |

The `.csproj` includes:
```xml
<Protobuf Include="Protos\echo.proto" GrpcServices="Server" />
```
`GrpcServices="Server"` tells `Grpc.Tools` to generate only the **server-side abstract base class** (`Echo.EchoBase`) from the proto file.

### Entry Point (`Program.cs`)

```csharp
var builder = WebApplication.CreateBuilder(args);
builder.Services.AddGrpc();          // registers gRPC middleware

var app = builder.Build();
app.MapGrpcService<EchoService>();   // maps the Echo service to its route
app.MapGet("/", () => "...");        // fallback for plain HTTP requests
app.Run();
```

- `AddGrpc()` registers the gRPC framework with the ASP.NET Core dependency injection container.
- `MapGrpcService<EchoService>()` registers the service on an HTTP/2 route that gRPC clients can reach. The route is automatically derived from the proto package and service name (`/echo.Echo/Echo`).
- The `MapGet("/")` fallback returns a plain-text hint because browsers (or tools like `curl`) cannot speak the gRPC protocol.
- The server listens on `http://localhost:5174` (defined in `Properties/launchSettings.json`).

### Service Implementation (`Services/EchoService.cs`)

```csharp
public class EchoService : Echo.EchoBase
{
    public override Task<EchoResponse> Echo(EchoRequest request, ServerCallContext context)
    {
        if (string.IsNullOrEmpty(request.Message))
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Message is required."));

        Timestamp timestamp = request.TimeStamp ?? Timestamp.FromDateTime(DateTime.UtcNow);

        return Task.FromResult(new EchoResponse
        {
            Message = "You said: " + request.Message,
            TimeStamp = timestamp
        });
    }
}
```

Step-by-step logic:
1. **Inherits** from `Echo.EchoBase` — the auto-generated abstract class that maps the `Echo` RPC method to a C# virtual method.
2. **Validates input** — if `Message` is empty or null, a gRPC `INVALID_ARGUMENT` error is thrown. This surfaces to the client as an `RpcException` with `StatusCode.InvalidArgument`.
3. **Resolves the timestamp** — if the client provided a timestamp it is used as-is; otherwise the server substitutes the current UTC time.
4. **Returns the response** — prepends `"You said: "` to the original message and includes the timestamp.

---

## GrpcEchoClient

### Dependencies (`GrpcEchoClient.csproj`)

| Package | Role |
|---|---|
| `Google.Protobuf` | Runtime serialisation/deserialisation of Protobuf messages |
| `Grpc.Net.Client` | Managed .NET gRPC client (`GrpcChannel`, typed stubs) |
| `Grpc.Tools` | Build-time code generation from `.proto` files |

The `.csproj` includes:
```xml
<Protobuf Include="Protos\echo.proto" GrpcServices="Client" />
```
`GrpcServices="Client"` generates only the **client-side stub** (`Echo.EchoClient`) from the proto file.

### Entry Point (`Program.cs`)

```csharp
AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

using var channel = GrpcChannel.ForAddress("http://localhost:5174");
var client = new Echo.EchoClient(channel);
```

- The `AppContext.SetSwitch` call **enables unencrypted HTTP/2**. By default, .NET's `HttpClient` only allows HTTP/2 over TLS; this switch lifts that restriction so the client can talk to a plain `http://` gRPC server during development.
- `GrpcChannel.ForAddress(...)` creates a reusable channel (an HTTP/2 connection pool) to the server.
- `Echo.EchoClient` is the generated stub — a thin wrapper around the channel that exposes typed `EchoAsync(...)` / `Echo(...)` methods.

#### User Interaction

```csharp
Console.Write("Enter your name: ");
string nameInput = Console.ReadLine()!;

Console.Write("Enter timestamp: ");
string timeStampInput = Console.ReadLine()!;
```

The user is prompted for:
1. **A message** (labelled "name" in the prompt) — sent as `EchoRequest.Message`.
2. **An optional timestamp** — if left blank, `null` is sent and the server uses its current time instead.

#### Making the Call

```csharp
Timestamp? timestamp = string.IsNullOrWhiteSpace(timeStampInput)
    ? null
    : Timestamp.FromDateTime(DateTime.Parse(timeStampInput).ToUniversalTime());

var reply = await client.EchoAsync(
    new EchoRequest { Message = nameInput, TimeStamp = timestamp });

Console.WriteLine(reply.Message);
Console.WriteLine(reply.TimeStamp);
```

- If the user provided a timestamp string it is parsed, converted to UTC, and wrapped in a `google.protobuf.Timestamp`.
- `EchoAsync` sends the request asynchronously over HTTP/2 and awaits the `EchoResponse`.
- The echoed message and timestamp are printed to the console.

#### Error Handling

```csharp
catch (RpcException ex)
{
    Console.WriteLine("Status code: " + ex.Status.StatusCode);
    Console.WriteLine("Message: " + ex.Status.Detail);
}
```

Any gRPC-level error (e.g. `INVALID_ARGUMENT` when the message is empty, or `UNAVAILABLE` when the server is not running) is caught and its status code and detail message are printed.

---

## How to Run

> Both projects must be run simultaneously. Start the server first.

### 1. Start the server

```bash
cd GrpcEchoServer
dotnet run
```

The server starts on `http://localhost:5174`.

### 2. Start the client

Open a second terminal:

```bash
cd GrpcEchoClient
dotnet run
```

You will be prompted to enter a message and an optional timestamp:

```
Enter your name: Alice
Enter timestamp:          <- leave blank to use server time
You said: Alice
{ seconds: 1742130000 nanos: 0 }
Press any key to exit...
```

---

## Request / Response Flow

```
Client                                    Server
  |                                          |
  |-- EchoRequest { message, timestamp } -->|
  |                                          | validate message
  |                                          | resolve timestamp
  |<- EchoResponse { message, timestamp } --|
  |                                          |
```

1. The client serialises an `EchoRequest` protobuf message and sends it over an HTTP/2 POST to `/echo.Echo/Echo`.
2. Kestrel on the server receives the request, deserialises it, and dispatches it to `EchoService.Echo(...)`.
3. The service validates the message, constructs the response, and returns it.
4. The client deserialises the `EchoResponse` and prints the fields.

---

## Key Concepts Illustrated

| Concept | Where |
|---|---|
| Protobuf IDL as a shared contract | `Protos/echo.proto` in both projects |
| Server-side code generation | `GrpcServices="Server"` in server `.csproj` |
| Client-side code generation | `GrpcServices="Client"` in client `.csproj` |
| gRPC unary RPC | Single request → single response |
| `google.protobuf.Timestamp` well-known type | Optional timestamp field, server fallback |
| gRPC error handling via `RpcException` | `INVALID_ARGUMENT` on server, caught on client |
| Unencrypted HTTP/2 development flag | `Http2UnencryptedSupport` switch in client |
