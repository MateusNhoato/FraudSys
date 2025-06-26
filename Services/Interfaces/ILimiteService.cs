namespace FraudSys.Services
{
    public interface ILimiteService
    {
        Task<decimal?> ObterLimite(string cpf);

        Task<bool> AtualizarLimite(string cpf, decimal valor);

        Task<bool> RemoverLimite(string cpf);
    }
}
