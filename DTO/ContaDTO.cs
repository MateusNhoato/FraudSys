namespace FraudSys.DTO
{
    public class ContaDTO
    {
        public string? Cpf { get; set; }
        public int NumeroConta { get; set; }
        public int Agencia { get; set; }
        public decimal? LimiteTransacoesPix { get; set; }
    }

    public class ContaInDTO : ContaDTO
    {
        public decimal? Saldo { get; set; }
    }
}
