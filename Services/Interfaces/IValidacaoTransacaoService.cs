using FraudSys.DTO;

namespace FraudSys.Services
{
    public interface IValidacaoTransacaoService
    {
        bool TransacaoValida(TransacaoDTO dto);
    }
}
