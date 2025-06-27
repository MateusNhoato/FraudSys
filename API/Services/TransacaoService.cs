using FraudSys.DTO;
using FraudSys.Models;
using FraudSys.Repositories.Interfaces;
using FraudSys.Resources;
using FraudSys.Services.Interfaces;
using FraudSys.Validators;
using Microsoft.Extensions.Options;

namespace FraudSys.Services
{
    public class TransacaoService : ITransacaoService
    {
        private readonly IContaRepository _contaRepository;
        private readonly ServicoDeMensagens _servicoDeMensagens;
        private readonly TransacaoInDTOValidator _transacaoValidator;

        public TransacaoService(IContaRepository contaRepository, ServicoDeMensagens servicoDeMensagens, TransacaoInDTOValidator transacaoValidator)
        {
            _contaRepository = contaRepository;
            _servicoDeMensagens = servicoDeMensagens;
            _transacaoValidator = transacaoValidator;
        }

        public async Task<TransacaoOutDTO?> EfetuarTransacao(TransacaoInDTO dto)
        {
            var validacao = await _transacaoValidator.ValidateAsync(dto);

            if (!validacao.IsValid)
            {
                _servicoDeMensagens.AdicionarMensagens(validacao.Errors);
                return null;
            }

            TransacaoOutDTO resultado = null;

            var contaRemetente = await _contaRepository.ObterAsync(dto.CpfContaRementente);

            if (!TemSaldoDisponivelParaTransacao(contaRemetente, dto.Valor))
            {
                _servicoDeMensagens.AdicionarMensagemErro(FraudSysResource.SaldoInsuficienteParaTransacao);
            }

            if(!TemLimiteDisponivelParaTransacao(contaRemetente, dto.Valor))
            {
                _servicoDeMensagens.AdicionarMensagemErro(FraudSysResource.LimiteContaExcedido);
            }

            if (!_servicoDeMensagens.TemErros)
            {
                var contaDestinatario = await _contaRepository.ObterAsync(dto.CpfContaDestinatario);

                AtualizarSaldos(contaRemetente, contaDestinatario, dto.Valor);
                await _contaRepository.GravarAsync(new List<Conta>() { contaDestinatario, contaRemetente });

                resultado = new TransacaoOutDTO()
                {
                    LimiteAtual = contaRemetente.Limite
                };

                _servicoDeMensagens.AdicionarMensagemInformacao(FraudSysResource.TransacaoAprovada);
            }

            return resultado;
        }

        private void AtualizarSaldos(Conta contaRemetente, Conta contaDestinatario, decimal valor)
        {
            AtualizarSaldoRemetente(contaRemetente, valor);
            AtualizarSaldoDestinatario(contaDestinatario, valor);
        }
        private void AtualizarSaldoDestinatario(Conta conta, decimal valor)
        {
            conta.Saldo += valor;
        }

        private void AtualizarSaldoRemetente(Conta conta, decimal valor)
        {
            conta.Limite -= valor;
            conta.Saldo -= valor;
        }

        private bool TemSaldoDisponivelParaTransacao(Conta conta, decimal valor)
        {
            return conta.Saldo >= valor;
        }

        private bool TemLimiteDisponivelParaTransacao(Conta conta, decimal valor)
        {
            return conta.Limite == null ||
                   conta.Limite >= valor;
        }
    }
}
