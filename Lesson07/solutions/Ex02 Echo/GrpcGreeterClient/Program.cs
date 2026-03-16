using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Grpc.Net.Client;
using GrpcEchoClient;

// The port number must match the port of the gRPC server.
AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);

using var channel = GrpcChannel.ForAddress("http://localhost:5174");

var client = new Echo.EchoClient(channel);

Console.Write("Enter your name: ");
string nameInput = Console.ReadLine()!;

Console.Write("Enter timestamp: ");
string timeStampInput = Console.ReadLine()!;


try
{
    Timestamp? timestamp = string.IsNullOrWhiteSpace(timeStampInput)
    ? null
    : Timestamp.FromDateTime(DateTime.Parse(timeStampInput).ToUniversalTime());

    var reply = await client.EchoAsync(
        new EchoRequest { Message = nameInput, TimeStamp = timestamp });

    Console.WriteLine(reply.Message);
    Console.WriteLine(reply.TimeStamp);
    Console.WriteLine("Press any key to exit...");
    Console.ReadKey();
}
catch (RpcException ex)
{
    Console.WriteLine("Status code: " + ex.Status.StatusCode);
    Console.WriteLine("Message: " + ex.Status.Detail);
}


