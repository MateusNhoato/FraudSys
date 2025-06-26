using Amazon.DynamoDBv2.DataModel;

namespace FraudSys.Models
{
    [DynamoDBTable("fraud-sys-table")]
    public class Transacao
    {
        [DynamoDBHashKey("pk")]
        public string PK => "CONTA#" + ContaRemetente;

        [DynamoDBRangeKey("sk")]
        public string SK => "TRANSACAO#" + Data.ToString("yyyy-MM-ddTHH:mm:ss");

        [DynamoDBProperty]
        public string ContaRemetente { get; set; }
        [DynamoDBProperty]
        public string ContaDestino { get; set; }
        [DynamoDBProperty]
        public DateTime Data { get; set; }
        [DynamoDBProperty]
        public decimal Valor { get; set; }
        [DynamoDBProperty]
        public bool Aprovada { get; set; }
        [DynamoDBProperty]
        public string? MensagemDeErro { get; set; }
    }
}
