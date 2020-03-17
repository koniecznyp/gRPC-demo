using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Net.Client;

namespace GrpcMessageServerService
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Starting...");
            var serverAddress = "https://localhost:5001";
            var httpClientHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback =
                    HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };
            var httpClient = new HttpClient(httpClientHandler);
            var channel = GrpcChannel.ForAddress(serverAddress, new GrpcChannelOptions { HttpClient = httpClient });
            var _client = new MessageService.MessageServiceClient(channel);

            // example 1
            var request = new GetMessageRequest { Id = "6523" };
            var response = _client.GetMessage(request);
            Console.WriteLine($"Server response: {response.Message} {response.Value1} {response.Value2}");
   
            // example 2
            var stream = _client.GetMessagesStream(request);
            while (await stream.ResponseStream.MoveNext(CancellationToken.None))
            {
                Console.WriteLine($"{stream.ResponseStream.Current.Message} {stream.ResponseStream.Current.Value1}");
            }

            Console.WriteLine(stream.GetStatus());
        }
    }
}
