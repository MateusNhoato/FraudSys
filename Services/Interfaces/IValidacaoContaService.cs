using FraudSys.DTO;

namespace FraudSys.Services
{
    public interface IValidacaoContaService
    {
        bool InformacoesContaValida(ContaDTO conta);

        bool ValorSaldoValido(decimal valorSaldo);
    }
}
