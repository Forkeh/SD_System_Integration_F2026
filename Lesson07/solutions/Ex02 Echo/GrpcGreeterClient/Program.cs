using Grpc.Net.Client;
using GrpcGreeterClient;

// The port number must match the port of the gRPC server.
AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
using var channel = GrpcChannel.ForAddress("http://localhost:5174");
var client = new Greeter.GreeterClient(channel);
Console.Write("Enter your name: ");
string nameInput = Console.ReadLine()!;

var reply = await client.SayHelloAsync(
    new HelloRequest { Name = nameInput });
Console.WriteLine("Greeting: " + reply.Message);
Console.WriteLine("Press any key to exit...");
Console.ReadKey();
