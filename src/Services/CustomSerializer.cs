using Amazon.Lambda.APIGatewayEvents;
using lambda_api.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Services
{

    [JsonSerializable(typeof(APIGatewayProxyRequest))]
    [JsonSerializable(typeof(APIGatewayProxyResponse))]
    [JsonSerializable(typeof(testemodel))]
    public partial class CustomSerializer : JsonSerializerContext
    {
    }

}