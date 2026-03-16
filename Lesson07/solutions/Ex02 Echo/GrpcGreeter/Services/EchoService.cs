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
        return Task.FromResult(new EchoResponse
        {
            Message = "You said: " + request.Name
        });
    }
}
