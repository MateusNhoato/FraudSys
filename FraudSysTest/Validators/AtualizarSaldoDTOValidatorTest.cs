using FraudSys.DTO;
using FraudSys.Resources;
using FraudSys.Validators;
using FraudSysTest.MockRepositories;


namespace FraudSysTest.Validators
{
    public class AtualizarSaldoDTOValidatorTest
    {
        private readonly AtualizarSaldoDTOValidator _validator;

        public AtualizarSaldoDTOValidatorTest()
        {
            _validator = new AtualizarSaldoDTOValidator(new ContaTestRepository());
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



        private AtualizarSaldoDTO ObterDtoValido()
        {
            return new AtualizarSaldoDTO()
            {
                Cpf = "123.123.123-12",
                Valor = 500
            };
        }

        
    }
}
