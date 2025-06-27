using FraudSys.DTO;
using FraudSys.Resources;
using FraudSys.Services;
using FraudSysTest.MockRepositories;


namespace FraudSysTest.Services
{
    public class LimiteServiceTest
    {
        private readonly ILimiteService _limiteService;
        private readonly ServicoDeMensagens _servicoDeMensagens;

        public LimiteServiceTest()
        {
            var repositorio = new ContaTestRepository();
            _servicoDeMensagens = new ServicoDeMensagens();
            _limiteService = new LimiteService(repositorio, new FraudSys.Validators.LimiteDTOValidator(repositorio), _servicoDeMensagens);
        }

        [Theory]
        [InlineData("123.123.123-12")]
        [InlineData("123.123.123-13")]
        public async Task DeveRetornarLimiteDeContasExistente(string cpf)
        {
            var retorno = await _limiteService.ObterLimite(cpf);

            Assert.NotNull(retorno);
        }

        [Theory]
        [InlineData("142.333.512-02")]
        [InlineData("123.004.223-05")]
        public async Task NaoDeveRetornarLimiteDeContasInexistente(string cpf)
        {
            var retorno = await _limiteService.ObterLimite(cpf);

            Assert.Null(retorno);
        }

        [Theory]
        [InlineData("123.123.123-12")]
        [InlineData("123.123.123-13")]
        public async Task DeveRemoverLimiteDeContasExistente(string cpf)
        {
            var retorno = await _limiteService.RemoverLimite(cpf);

            Assert.True(retorno);
        }

        [Theory]
        [InlineData("142.333.512-02")]
        [InlineData("123.004.223-05")]
        public async Task NaoDeveRemoverLimiteDeContasInexistente(string cpf)
        {
            var retorno = await _limiteService.RemoverLimite(cpf);

            Assert.False(retorno);
        }

        [Theory]
        [InlineData(-100d)]
        [InlineData(-2000d)]
        public async Task DeveRetornarErroDeLimiteInvalidoAoAtualizar(double? valorDouble)
        {
            var dto = ObterDTOValido();
            dto.Valor = (decimal?)valorDouble;
            var atualizou = await _limiteService.AtualizarLimite(dto);

            Assert.False(atualizou);
            Assert.Contains(_servicoDeMensagens.ObterMensagens(), m => m.Mensagem == FraudSysResource.LimiteDeveSerMaiorQueZero);
        }

        [Theory]
        [InlineData("142.333.512-02")]
        [InlineData("123.004.223-05")]
        public async Task DeveRetornarErroDeContaInvalidaAoAtualizar(string cpf)
        {
            var dto = ObterDTOValido();
            dto.Cpf = cpf;
            var atualizou = await _limiteService.AtualizarLimite(dto);

            Assert.False(atualizou);
            Assert.Contains(_servicoDeMensagens.ObterMensagens(), m => m.Mensagem == FraudSysResource.ContaInformadaNaoExiste);
        }

        [Fact]
        public async Task NaoDeveRetornarErros()
        {
            var dto = ObterDTOValido();
            var atualizou = await _limiteService.AtualizarLimite(dto);

            Assert.True(atualizou);
            Assert.DoesNotContain(_servicoDeMensagens.ObterMensagens(), m => m.TipoDeMensagem == FraudSys.Enumerators.TipoDeMensagem.Erro);
        }


        private LimiteInDTO ObterDTOValido()
        {
            return new LimiteInDTO()
            {
                Cpf = "123.123.123-13",
                Valor = 500
            };
        }
    }
}
