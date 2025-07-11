﻿using FraudSys.DTO;
using FraudSys.Models;
using FraudSys.Repositories.Interfaces;
using FraudSys.Resources;
using FraudSys.Validators;

namespace FraudSys.Services
{
    public class ContaService : IContaService
    {
        private readonly IContaRepository _repository;
        private readonly ContaInDTOValidator _contaInValidator;
        private readonly AtualizarSaldoDTOValidator _atualizarSaldoDTOValidator;
        private readonly ServicoDeMensagens _servicoDeMensagens;
        private readonly CpfDTOValidator _cpfDTOValidator;

        public ContaService(IContaRepository repository, ContaInDTOValidator contaInValidator, AtualizarSaldoDTOValidator atualizarSaldoDTOValidator, ServicoDeMensagens servicoDeMensagens, CpfDTOValidator cpfDTOValidator)
        {
            _repository = repository;
            _contaInValidator = contaInValidator;
            _atualizarSaldoDTOValidator = atualizarSaldoDTOValidator;
            _servicoDeMensagens = servicoDeMensagens;
            _cpfDTOValidator = cpfDTOValidator;
        }

        public async Task<bool> AtualizarSaldoAsync(AtualizarSaldoDTO atualizarSaldoDTO)
        {
            var validacao = await _atualizarSaldoDTOValidator.ValidateAsync(atualizarSaldoDTO);
            
            if (!validacao.IsValid)
            {
                _servicoDeMensagens.AdicionarMensagens(validacao.Errors);
                return false;
            }

            var conta = await _repository.ObterAsync(atualizarSaldoDTO.Cpf);
            
            if(!AtualizarSaldoConta(conta, atualizarSaldoDTO.Valor))
            {
                return false;
            }

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
            var validacao = await _cpfDTOValidator.ValidateAsync(new CpfDTO() { Cpf = cpf });

            if (!validacao.IsValid)
            {
                _servicoDeMensagens.AdicionarMensagens(validacao.Errors);
                return null;
            }

            var conta = await _repository.ObterAsync(cpf);

            return ObterDTO(conta);
        }

        public async Task<bool> RemoverAsync(string cpf)
        {
            var validacao = await _cpfDTOValidator.ValidateAsync(new CpfDTO() { Cpf = cpf });

            if (!validacao.IsValid)
            {
                _servicoDeMensagens.AdicionarMensagens(validacao.Errors);
                return false;
            }
            var conta = await _repository.ObterAsync(cpf);
            await _repository.RemoverAsync(conta);

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
            return new Conta(contaInDTO.Cpf, contaInDTO.NumeroConta, contaInDTO.Agencia, contaInDTO.Saldo.Value, contaInDTO.LimiteTransacoesPix);
        }

        private bool AtualizarSaldoConta(Conta conta, decimal valor)
        {
            if (OperacaoVaiNegativarAConta(conta, valor))
            {
                _servicoDeMensagens.AdicionarMensagemErro(FraudSysResource.OperacaoInvalidaNaoEhPossivelNegativarSaldo);
                return false;
            }

            conta.Saldo += valor;
            return true;
        }


        private bool OperacaoVaiNegativarAConta(Conta conta, decimal valor)
        {
            return valor < 0 &&
                conta.Saldo + valor < 0;
        }
        private async Task GravarConta(Conta conta)
        {
            await _repository.GravarAsync(conta);
        }
    }
}
