using Amazon.DynamoDBv2.DataModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    [DynamoDBTable("tabela_teste")]
    public class FaturaModel
    {
        [DynamoDBHashKey]
        public string fatura { get; set; }
        [DynamoDBRangeKey]
        public string transacao { get; set; }
    }
}
