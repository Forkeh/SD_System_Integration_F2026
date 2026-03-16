using Grpc.Net.Client;
using GrpcEchoClient;

// The port number must match the port of the gRPC server.
AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

using var channel = GrpcChannel.ForAddress("http://localhost:5174");

var client = new Echo.EchoClient(channel);

Console.Write("Enter your name: ");
string nameInput = Console.ReadLine()!;

var reply = await client.EchoAsync(
    new EchoRequest { Name = nameInput });
    
Console.WriteLine(reply.Message);
Console.WriteLine("Press any key to exit...");
Console.ReadKey();
