using FraudSys.DTO;
using FraudSys.Models;
using FraudSys.Repositories;

namespace FraudSys.Services
{
    public class ContaService : IContaService
    {
        private readonly IContaRepository _repository;
        private readonly IValidacaoContaService _validacaoContaService;


        public async Task<bool> AtualizarSaldoAsync(AtualizarSaldoDTO atualizarSaldoDTO)
        {
            var conta = await _repository.ObterAsync(atualizarSaldoDTO.Cpf);

            if(conta is null)
            {
                return false;
            }

            if (!_validacaoContaService.ValorSaldoValido(atualizarSaldoDTO.Valor))
            {
                return false;
            }

            await AtualizarSaldoConta(conta, atualizarSaldoDTO.Valor);

            return true;
        }

        public async Task<ContaDTO> GravarAsync(ContaInDTO conta)
        {
            if (!_validacaoContaService.InformacoesContaValida(conta))
            {
                return null;
            }

            await _repository.GravarAsync(new Conta
            {
                Cpf = conta.Cpf,
                Agencia = conta.Agencia,
                Limite = conta.LimiteTransacoesPix,
                NumeroConta = conta.NumeroConta,
                Saldo = conta.Saldo ?? 0
            });

            return conta;
        }

        public async Task<ContaDTO?> ObterAsync(string cpf)
        {
            var conta = await _repository.ObterAsync(cpf);

            return ObterDTO(conta);
        }

        public async Task<bool> RemoverAsync(string cpf)
        {
            var conta = await _repository.ObterAsync(cpf);

            if (conta is null)
            {
                return false;
            }
            await _repository.RemoverAsync(cpf);

            return true;
        }

        private ContaDTO? ObterDTO(Conta conta) 
        {
            return conta is null ? null : new ContaDTO()
            {
                Agencia = conta.Agencia,
                Cpf = conta.Cpf,
                NumeroConta = conta.NumeroConta,
                LimiteTransacoesPix = conta.Limite
            };
        }

        private async Task AtualizarSaldoConta(Conta conta, decimal valor)
        {
            conta.Saldo += valor;

            await _repository.GravarAsync(conta);
        }
    }
}
