using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using GrpcEcho;

namespace GrpcEchoServer.Services;

public class EchoService : Echo.EchoBase
{
    private readonly ILogger<EchoService> _logger;
    public EchoService(ILogger<EchoService> logger)
    {
        _logger = logger;
    }

    public override Task<EchoResponse> Echo(EchoRequest request, ServerCallContext context)
    {
        if (string.IsNullOrEmpty(request.Message))
        {
            throw new RpcException(new Status(StatusCode.InvalidArgument, "Message is required."));
        }

        Timestamp timestamp = request.TimeStamp ?? Timestamp.FromDateTime(DateTime.UtcNow);

        return Task.FromResult(new EchoResponse
        {
            Message = "You said: " + request.Message,
            TimeStamp = timestamp
        });
    }
}
