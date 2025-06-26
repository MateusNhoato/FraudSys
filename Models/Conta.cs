using Amazon.DynamoDBv2.DataModel;

namespace FraudSys.Models
{
    [DynamoDBTable("fraud-sys-table")]
    public class Conta
    {
        [DynamoDBHashKey("pk")]
        public string PK => "CLIENTE#" + Cpf;
        [DynamoDBRangeKey("sk")]
        public string SK => "CONTA#" + NumeroConta + "-" + Agencia;

        [DynamoDBProperty]
        public string Cpf { get; set; }

        [DynamoDBProperty]
        public string NumeroConta { get; set; }
        [DynamoDBProperty]
        public string Agencia { get; set; }

        [DynamoDBProperty]
        public decimal Saldo { get; set; }
        [DynamoDBProperty]
        public decimal Limite { get; set; }
    }
}
