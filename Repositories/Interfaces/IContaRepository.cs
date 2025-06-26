using FraudSys.Models;

namespace FraudSys.Repositories
{
    public interface IContaRepository
    {
        Task GravarAsync(Conta conta);
        Task RemoverAsync(string cpf);
        Task<Conta> ObterAsync(string cpf);

        Task<bool> ContaExiste(string cpf);
    }
}
