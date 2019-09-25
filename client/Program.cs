using System;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Net.Client;
using grpc_blog;

namespace client
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Calling SayHello Endpoint with name: 'World'");

            // The gRPC endpoint set up in our service.
            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            // Create a client using the channel
            var client = new Greeter.GreeterClient(channel);

            // CAll the SayHello endpoint
            var helloReply = client.SayHello(new HelloRequest { Name = "World" });
            Console.WriteLine($"Response from server, '{helloReply.Message}'");

            Console.WriteLine("Calling StreamHello endpoint with name: 'John Doe'");
            // Calling the StreamHello ednpoint and constructing the stream
            using (var streamResponse = client.StreamHello(new HelloRequest { Name = "John Doe" })) 
            {
                Console.WriteLine("Response stream listed here with index of receiving order");
                int i = 1;
                // Keep moving next and print the received character untill end of stream
                while(await streamResponse.ResponseStream.MoveNext(cancellationToken: CancellationToken.None))
                {
                    Console.WriteLine($"[{i:D2}] {streamResponse.ResponseStream.Current.Character}");
                    i++;
                }
            }
            Console.WriteLine("Closing stream resposne");

            // Prevent early exit
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
