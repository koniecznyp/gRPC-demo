using System;
using System.Threading.Tasks;
using Grpc.Core;

namespace GrpcMessageServerService
{
    public class GrpcMessageService : MessageService.MessageServiceBase
    {
        public override Task<GetMessageResponse> GetMessage(GetMessageRequest request, ServerCallContext context)
            => Task.FromResult(new GetMessageResponse
            {
                Message = $"Some message from server",
                Value1 = 256,
                Value2 = 214.35
            });

        public override async Task GetMessagesStream(GetMessageRequest request, IServerStreamWriter<GetMessageResponse> responseStream, ServerCallContext context)
        {
            var random = new Random();
            for(int i = 1; i <= 10; ++i)
            {
                // simulate some work
                var delay = random.Next(100, 2000);
                await Task.Delay(delay);
                await responseStream.WriteAsync(new GetMessageResponse { Message = $"Step: {i}", Value1 = delay, Value2 = delay * delay});
            }
        }
    }
}