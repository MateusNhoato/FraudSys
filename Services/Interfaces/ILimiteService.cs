using FraudSys.DTO;

namespace FraudSys.Services
{
    public interface ILimiteService
    {
        Task<LimiteOutDTO> ObterLimite(string cpf);

        Task<bool> AtualizarLimite(LimiteInDTO dto);

        Task<bool> RemoverLimite(string cpf);
    }
}
