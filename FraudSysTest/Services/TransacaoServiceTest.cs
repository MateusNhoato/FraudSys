using FraudSys.DTO;
using FraudSys.Resources;
using FraudSys.Services;
using FraudSys.Services.Interfaces;
using FraudSysTest.MockRepositories;


namespace FraudSysTest.Services
{
    public class TransacaoServiceTest 
    {
        private readonly ITransacaoService _service;
        private readonly ServicoDeMensagens _servicoDeMensagens;

        public TransacaoServiceTest()
        {
            var repositorio = new ContaTestRepository();
            _servicoDeMensagens = new ServicoDeMensagens();
            _service = new TransacaoService(repositorio, _servicoDeMensagens, new FraudSys.Validators.TransacaoInDTOValidator(repositorio));
        }


        [Fact]
        public async Task DeveRetornarErroDeSaldoInsuficiente()
        {
            var dto = ObterDTOValido();
            dto.CpfContaRementente = "123.123.123-14";
            dto.Valor = 600;

            var retorno = await _service.EfetuarTransacao(dto);

            Assert.Null(retorno);
            Assert.Contains(_servicoDeMensagens.ObterMensagens(), x => x.Mensagem == FraudSysResource.SaldoInsuficienteParaTransacao);
        }

        [Fact]
        public async Task DeveRetornarErroDeLimiteTransacaoPix()
        {
            var dto = ObterDTOValido();
            dto.CpfContaRementente = "123.123.123-15";
            dto.Valor = 600;

            var retorno = await _service.EfetuarTransacao(dto);

            Assert.Null(retorno);
            Assert.Contains(_servicoDeMensagens.ObterMensagens(), x => x.Mensagem == FraudSysResource.LimiteContaExcedido);
        }


        [Fact]
        public async Task NaoDeveRetornarErro()
        {
            var dto = ObterDTOValido();
            var retorno = await _service.EfetuarTransacao(dto);

            Assert.NotNull(retorno);
            Assert.DoesNotContain(_servicoDeMensagens.ObterMensagens(), x => x.TipoDeMensagem == FraudSys.Enumerators.TipoDeMensagem.Erro);
        }



        private TransacaoInDTO ObterDTOValido()
        {
            return new TransacaoInDTO()
            {
                CpfContaRementente = "123.123.123-13",
                CpfContaDestinatario = "123.123.123-12",
                Valor = 500
            };
        }
    }
}
