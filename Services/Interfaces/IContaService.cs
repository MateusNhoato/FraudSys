using FraudSys.DTO;

namespace FraudSys.Services
{
    public interface IContaService
    {
        Task<ContaDTO?> ObterAsync(string cpf);
        Task<bool> RemoverAsync(string cpf);
        Task<ContaDTO> GravarAsync(ContaInDTO conta);

        Task<bool> AtualizarSaldoAsync(AtualizarSaldoDTO atualizarSaldoDTO);
    }
}
