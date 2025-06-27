using Amazon.DynamoDBv2.DataModel;
using FraudSys.Constantes;
using FraudSys.Models;
using FraudSys.Repositories.Interfaces;

namespace FraudSys.Repositories
{
    public class ContaRepository : IContaRepository
    {
        private readonly DynamoDBContext _context;

        public ContaRepository(DynamoDBContext context)
        {
            _context = context;
        }

        public async Task<Conta> ObterAsync(string cpf)
        {
            var contas = await _context.QueryAsync<Conta>(FraudSysConstantes.CLIENTE + cpf).GetRemainingAsync();
            return contas.FirstOrDefault();
        }

        public async Task GravarAsync(Conta conta)
        {
            await _context.SaveAsync(conta);
        }

        public async Task GravarAsync(IEnumerable<Conta> contas)
        {
           foreach(var conta in contas)
            {
                await GravarAsync(conta);
            }
        }

        public async Task RemoverAsync(Conta conta)
        {
            await _context.DeleteAsync(conta);
        }

        public async Task<bool> ContaExiste(string cpf)
        {
            if (string.IsNullOrEmpty(cpf)) return false;

            var conta = await ObterAsync(cpf);

            return conta != null; 
        }
    }
}
