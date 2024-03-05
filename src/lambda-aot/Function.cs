using Amazon.Lambda.Core;
using Amazon.Lambda.RuntimeSupport;
using Amazon.Lambda.Serialization.SystemTextJson;
using Amazon.Lambda.APIGatewayEvents;
using System.Text.Json.Serialization;
using lambda_aot.serializer;

[assembly: LambdaSerializer(typeof(SourceGeneratorLambdaJsonSerializer<lambda_aot.serializer.CustomSerializer>))]
namespace lambda_aot
{
    public class Function
    {
        /// <summary>
        /// The main entry point for the custom runtime.
        /// </summary>
        /// <param name="args"></param>
        private static async Task Main(string[] args)
        {
            Func<APIGatewayHttpApiV2ProxyRequest, ILambdaContext, string> handler = FunctionHandler;
            await LambdaBootstrapBuilder.Create(handler, new SourceGeneratorLambdaJsonSerializer<CustomSerializer>())
                .Build()
                .RunAsync();
        }

        /// <summary>
        /// A simple function that takes a string and does a ToUpper
        ///
        /// To use this handler to respond to an AWS event, reference the appropriate package from 
        /// https://github.com/aws/aws-lambda-dotnet#events
        /// and change the string input parameter to the desired event type.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        public static string FunctionHandler(APIGatewayHttpApiV2ProxyRequest input, ILambdaContext context)
        {

            var architecture = System.Runtime.InteropServices.RuntimeInformation.ProcessArchitecture;
            var dotnetVersion = Environment.Version.ToString();
            return $"Architecture: {architecture}, .NET Version: {dotnetVersion} -- {input?.Body.ToUpper()}";
        }
    }
}