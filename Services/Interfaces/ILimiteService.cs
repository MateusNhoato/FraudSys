using FraudSys.DTO;

namespace FraudSys.Services
{
    public interface ILimiteService
    {
        Task<decimal?> ObterLimite(string cpf);

        Task<bool> AtualizarLimite(LimiteDTO dto);

        Task<bool> RemoverLimite(string cpf);
    }
}
