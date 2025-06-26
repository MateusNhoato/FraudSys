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
            Mensagens.AddRange(mensagens);
        }

        public  void AdicionarMensagem(MensagemDTO mensagem)
        {
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
