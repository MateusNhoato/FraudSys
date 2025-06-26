namespace FraudSys.DTO
{
    public class ContaDTO
    {
        public string? Cpf { get; set; }
        public string NumeroConta { get; set; }
        public string Agencia { get; set; }
        public decimal LimiteTransacoesPix { get; set; }
    }

    public class ContaInDTO : ContaDTO
    {
        public decimal? Saldo { get; set; }
    }
}
