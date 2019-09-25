using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.Extensions.Logging;

namespace grpc_blog
{
    public class GreeterService : Greeter.GreeterBase
    {
        private readonly ILogger<GreeterService> _logger;
        public GreeterService(ILogger<GreeterService> logger)
        {
            _logger = logger;
        }

        public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            return Task.FromResult(new HelloReply
            {
                Message = "Hello " + request.Name
            });
        }

        public override async Task StreamHello(HelloRequest request, IServerStreamWriter<HelloStreamReply> responseStream, ServerCallContext context)
        {
            var message = $"Hello {request.Name}";
            foreach (var c in message)
            {
                var reply = new HelloStreamReply { Character = c.ToString() };
                await responseStream.WriteAsync(reply);
            }
        }
    }
}
