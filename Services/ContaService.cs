using FraudSys.DTO;
using FraudSys.Models;
using FraudSys.Repositories;
using FraudSys.Validators;

namespace FraudSys.Services
{
    public class ContaService : IContaService
    {
        private readonly IContaRepository _repository;
        private readonly ContaInDTOValidator _contaInValidator;
        private readonly AtualizarSaldoDTOValidator _atualizarSaldoDTOValidator;
        private readonly ServicoDeMensagens _servicoDeMensagens;

   

        public async Task<bool> AtualizarSaldoAsync(AtualizarSaldoDTO atualizarSaldoDTO)
        {
            var validacao = await _atualizarSaldoDTOValidator.ValidateAsync(atualizarSaldoDTO);
            
            if (!validacao.IsValid)
            {
                _servicoDeMensagens.AdicionarMensagens(validacao.Errors);
                return false;
            }

            var conta = await _repository.ObterAsync(atualizarSaldoDTO.Cpf);
            AtualizarSaldoConta(conta, atualizarSaldoDTO.Valor);
            await GravarConta(conta);
            
            return true;
        }

        public async Task<ContaDTO> GravarAsync(ContaInDTO contaInDTO)
        {
            var validacao = _contaInValidator.Validate(contaInDTO);

            if (!validacao.IsValid)
            {
                _servicoDeMensagens.AdicionarMensagens(validacao.Errors);
                return null;
            }

            var conta = ObterConta(contaInDTO);
            await GravarConta(conta);
            return contaInDTO;
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

        private Conta ObterConta(ContaInDTO contaInDTO)
        {
            return new Conta
            {
                Cpf = contaInDTO.Cpf,
                Agencia = contaInDTO.Agencia,
                Limite = contaInDTO.LimiteTransacoesPix,
                NumeroConta = contaInDTO.NumeroConta,
                Saldo = contaInDTO.Saldo ?? 0
            };
        }

        private void AtualizarSaldoConta(Conta conta, decimal valor)
        {
            conta.Saldo += valor;
        }

        private async Task GravarConta(Conta conta)
        {
            await _repository.GravarAsync(conta);
        }
    }
}
