using FraudSys.DTO;

namespace FraudSys.Services.Interfaces
{
    public interface ITransacaoService
    {
        Task<TransacaoOutDTO?> EfetuarTransacao(TransacaoInDTO dto);
    }
}
