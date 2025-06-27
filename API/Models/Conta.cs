using Amazon.DynamoDBv2.DataModel;
using FraudSys.Constantes;

namespace FraudSys.Models
{
    [DynamoDBTable("fraud-sys-table")]
    public class Conta
    {
        [DynamoDBHashKey("pk")]
        public string Pk { get; set; }
        [DynamoDBRangeKey("sk")]
        public string Sk { get; set; }

        [DynamoDBProperty]
        public string Cpf { get; set; }

        [DynamoDBProperty]
        public string NumeroConta { get; set; }
        [DynamoDBProperty]
        public string Agencia { get; set; }

        [DynamoDBProperty]
        public decimal Saldo { get; set; }
        [DynamoDBProperty]
        public decimal? Limite { get; set; }

        public Conta(string cpf, string numeroConta, string agencia, decimal saldo, decimal? limite)
        {
            Cpf = cpf;
            NumeroConta = numeroConta;
            Agencia = agencia;
            Saldo = saldo;
            Limite = limite;
            Pk = FraudSysConstantes.CLIENTE + Cpf;
            Sk = FraudSysConstantes.CONTA+ NumeroConta + FraudSysConstantes.SEPARADOR + Agencia;
        }

        public Conta()
        {
        }
    }
}
