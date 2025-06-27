using FraudSys.Models;

namespace FraudSys.Repositories.Interfaces
{
    public interface IContaRepository
    {
        Task GravarAsync(Conta conta);
        Task GravarAsync(IEnumerable<Conta> contas);
        Task RemoverAsync(string cpf);
        Task<Conta> ObterAsync(string cpf);

        Task<bool> ContaExiste(string cpf);
    }
}
