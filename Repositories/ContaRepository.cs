using Amazon.DynamoDBv2.DataModel;
using FraudSys.Models;
using Microsoft.Extensions.Localization;

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
            var contas = await _context.QueryAsync<Conta>(cpf).GetRemainingAsync();
            return contas.FirstOrDefault();
        }

        public async Task GravarAsync(Conta conta)
        {
            await _context.SaveAsync(conta);
        }

        public async Task RemoverAsync(string cpf)
        {
            await _context.DeleteAsync<Conta>(cpf);
        }

        public async Task<bool> ContaExiste(string cpf)
        {
            if (string.IsNullOrEmpty(cpf)) return false;

            var conta = await ObterAsync(cpf);

            return conta != null; 
        }
    }
}
