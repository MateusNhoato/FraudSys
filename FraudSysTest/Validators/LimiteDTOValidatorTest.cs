using FraudSys.DTO;
using FraudSys.Models;
using FraudSys.Repositories.Interfaces;
using FraudSys.Resources;
using FraudSys.Validators;
using FraudSysTest.MockRepositories;


namespace FraudSysTest.Validators
{
    public class LimiteDTOValidatorTest
    {
        private readonly LimiteDTOValidator _validator;

        public LimiteDTOValidatorTest()
        {
            _validator = new LimiteDTOValidator(new ContaTestRepository());
        }



        [Fact]
        public async Task DeveRetornarErroDeCpfObrigatorio()
        {
            var conta = ObterDtoValido();
            conta.Cpf = string.Empty;

            var resultado = await _validator.ValidateAsync(conta);

            Assert.False(resultado.IsValid);
            Assert.Contains(resultado.Errors, e => e.ErrorMessage == string.Format(FraudSysResource.CpfEhObrigatorio, string.Empty));
        }


        [Theory]
        [InlineData("aisejiasej")]
        [InlineData("teste")]
        [InlineData("123.123.444-003")]
        [InlineData("123.123.444-3")]
        [InlineData("123123.444-3")]
        [InlineData("abc.123.444-03")]
        [InlineData("abc.dd.444-03")]
        public async Task DeveRetornarErroDeFormatoDeCpf(string cpf)
        {
            var conta = ObterDtoValido();
            conta.Cpf = cpf;

            var resultado = await _validator.ValidateAsync(conta);

            Assert.False(resultado.IsValid);
            Assert.Contains(resultado.Errors, e => e.PropertyName == "Cpf");
        }


        [Theory]
        [InlineData("123.123.333-01")]
        [InlineData("111.222.555-05")]
        public async Task DeveRetornarErroDeContaInexistente(string cpf)
        {
            var dto = ObterDtoValido();
            dto.Cpf = cpf;

            var resultado = await _validator.ValidateAsync(dto);

            Assert.False(resultado.IsValid);
            Assert.Contains(resultado.Errors, x => x.ErrorMessage == FraudSysResource.ContaInformadaNaoExiste);
        }


        [Theory]
        [InlineData("123.123.123-12")]
        [InlineData("123.123.123-13")]
        public async Task NaoDeveRetornarErroDeConta(string cpf)
        {
            var dto = ObterDtoValido();
            dto.Cpf = cpf;

            var resultado = await _validator.ValidateAsync(dto);

            Assert.True(resultado.IsValid);
        }


        [Theory]
        [InlineData(-100d)]
        [InlineData(0d)]
        [InlineData(-1d)]
        [InlineData(-1123123d)]
        public async Task DeveRetornarErroDeLimiteTransacaoPixInvalido(double valorDouble)
        {
            var valor = (decimal)valorDouble;

            var dto = ObterDtoValido();
            dto.Valor = valor;

            var resultado = await _validator.ValidateAsync(dto);

            Assert.False(resultado.IsValid);
            Assert.Contains(resultado.Errors, e => e.ErrorMessage == FraudSysResource.LimiteDeveSerMaiorQueZero);
        }

        [Theory]
        [InlineData(100d)]
        [InlineData(null)]
        [InlineData(100000d)]
        [InlineData(25000d)]
        public async Task NaoDeveRetornarErroDeLimiteTransacaoPixInvalido(double? valorDouble)
        {
            var valor = (decimal?)valorDouble;
            var dto = ObterDtoValido();
            dto.Valor = valor;

            var resultado = await _validator.ValidateAsync(dto);

            Assert.True(resultado.IsValid);
            Assert.DoesNotContain(resultado.Errors, e => e.PropertyName == "Valor");
        }


        private LimiteInDTO ObterDtoValido()
        {
            return new LimiteInDTO()
            {
                Cpf = "123.123.123-12",
                Valor = 500
            };
        }

    }
}
