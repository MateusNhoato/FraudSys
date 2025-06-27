using FraudSys.Models;
using FraudSys.Repositories.Interfaces;


namespace FraudSysTest.MockRepositories
{
    internal class ContaTestRepository : IContaRepository
    {
        private readonly List<Conta> _contas;

        public ContaTestRepository()
        {
            _contas = new List<Conta>();
            AdicionarItens();
        }

        public async Task<bool> ContaExiste(string cpf)
        {
            return _contas.Any(c => c.Cpf == cpf);
        }

        public async Task GravarAsync(Conta conta)
        {
            _contas.Add(conta);
        }

        public async Task GravarAsync(IEnumerable<Conta> contas)
        {
            _contas.AddRange(contas);
        }

        public async Task<Conta> ObterAsync(string cpf)
        {
            return _contas.FirstOrDefault(c => c.Cpf == cpf);
        }

        public async Task RemoverAsync(string cpf)
        {
            _contas.RemoveAll(c => c.Cpf == cpf);
        }


        private void AdicionarItens()
        {
            _contas.Add(new Conta()
            {
                Agencia = "0000",
                NumeroConta = "00000-1",
                Cpf = "123.123.123-12",
                Limite = 1000,
                Saldo = 2000
            });

            _contas.Add(new Conta()
            {
                Agencia = "0001",
                NumeroConta = "00000-2",
                Cpf = "123.123.123-13",
                Limite = 1000,
                Saldo = 2000
            });

            _contas.Add(new Conta()
            {
                Agencia = "0001",
                NumeroConta = "00000-2",
                Cpf = "123.123.123-14",
                Limite = 1000,
                Saldo = 100
            });

            _contas.Add(new Conta()
            {
                Agencia = "0001",
                NumeroConta = "00000-2",
                Cpf = "123.123.123-15",
                Limite = 500,
                Saldo = 2000
            });
        }
    }
}
