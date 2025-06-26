using FluentValidation.Results;
using FraudSys.DTO;
using FraudSys.Enumerators;

namespace FraudSys.Services
{
    public class ServicoDeMensagens
    {
        private  List<MensagemDTO> Mensagens = new List<MensagemDTO>();

        public bool TemErros => Mensagens.Any(m => m.TipoDeMensagem == TipoDeMensagem.Erro);

        public  void LimparMensagens()
        {
            Mensagens.Clear();
        }

        public  void AdicionarMensagens(IEnumerable<MensagemDTO> mensagens)
        {
            foreach (var mensagem in mensagens)
            {
                AdicionarMensagem(mensagem);
            }
        }

        public void AdicionarMensagens(List<ValidationFailure> mensagensDeErroValidacao)
        {
           foreach(var mensagemDeErroValidacao in mensagensDeErroValidacao)
            {
                AdicionarMensagemErro(mensagemDeErroValidacao.ErrorMessage);
            }
        }

        public  void AdicionarMensagem(MensagemDTO mensagem)
        {
            if (string.IsNullOrEmpty(mensagem.Mensagem))
            {
                throw new ArgumentNullException("Mensagem não foi fornecida.");
            }

            Mensagens.Add(mensagem);
        }

        public  void AdicionarMensagemErro(string mensagem)
        {
            AdicionarMensagem(new MensagemDTO() { Mensagem = mensagem, TipoDeMensagem = TipoDeMensagem.Erro });
        }

        public  void AdicionarMensagemInformacao(string mensagem)
        {
            AdicionarMensagem(new MensagemDTO() { Mensagem = mensagem, TipoDeMensagem = TipoDeMensagem.Informacao });
        }

      
        public IReadOnlyCollection<MensagemDTO> ObterMensagens()
        {
            return Mensagens;
        }
    }
}
