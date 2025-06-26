using FraudSys.DTO;
using FraudSys.Repositories.Interfaces;
using FraudSys.Services.Interfaces;
using FraudSys.Validators;
using Microsoft.Extensions.Options;

namespace FraudSys.Services
{
    public class TransacaoService : ITransacaoService
    {
        private readonly IContaRepository _contaRepository;
        private readonly ServicoDeMensagens _servicoDeMensagens;
        private readonly TransacaoValidator _transacaoValidator;

        public TransacaoService(IContaRepository contaRepository, ServicoDeMensagens servicoDeMensagens, TransacaoValidator transacaoValidator)
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

            if(contaRemetente.Saldo < dto.Valor)
            {
                _servicoDeMensagens.AdicionarMensagemErro("Saldo insuficiente para transação.");
            }

            if(contaRemetente.Limite < dto.Valor)
            {
                _servicoDeMensagens.AdicionarMensagemErro("Limite da conta excedido.");
            }

            if (!_servicoDeMensagens.TemErros)
            {
                contaRemetente.Limite -= dto.Valor;
                contaRemetente.Saldo -= dto.Valor;

                var contaDestinatario = await _contaRepository.ObterAsync(dto.CpfContaRementente);
                contaDestinatario.Saldo += dto.Valor;

                await _contaRepository.GravarAsync(contaRemetente);
                await _contaRepository.GravarAsync(contaDestinatario);


                resultado = new TransacaoOutDTO()
                {
                    LimiteAtual = contaRemetente.Limite
                };

                _servicoDeMensagens.AdicionarMensagemInformacao("Transação aprovada!");
            }



            return resultado;
        }
    }
}
