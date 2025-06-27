using FraudSys.DTO;
using FraudSys.Models;
using FraudSys.Repositories.Interfaces;
using FraudSys.Resources;
using FraudSys.Validators;
using FraudSysTest.MockRepositories;

namespace FraudSysTest.Validators
{
    public class TransacaoInDTOValidatorTest
    {
        private readonly TransacaoInDTOValidator _validator;

        public TransacaoInDTOValidatorTest()
        {
            _validator = new TransacaoInDTOValidator(new ContaTestRepository());
        }


        [Fact]
        public async Task DeveRetornarErroDeRemetenteObrigatorio()
        {
            var dto = ObterDtoValido();
            dto.CpfContaRementente = string.Empty;

            var resultado = await _validator.ValidateAsync(dto);

            Assert.False(resultado.IsValid);
            Assert.Contains(resultado.Errors, e => e.ErrorMessage == string.Format(FraudSysResource.CpfEhObrigatorio, FraudSysResource.ContaRemetente));
        }

        [Fact]
        public async Task DeveRetornarErroDeDestinatarioObrigatorio()
        {
            var conta = ObterDtoValido();
            conta.CpfContaDestinatario = string.Empty;

            var resultado = await _validator.ValidateAsync(conta);

            Assert.False(resultado.IsValid);
            Assert.Contains(resultado.Errors, e => e.ErrorMessage == string.Format(FraudSysResource.CpfEhObrigatorio, FraudSysResource.ContaDestinatario));
        }


        [Theory]
        [InlineData("aisejiasej")]
        [InlineData("teste")]
        [InlineData("123.123.444-003")]
        [InlineData("123.123.444-3")]
        [InlineData("123123.444-3")]
        [InlineData("abc.123.444-03")]
        [InlineData("abc.dd.444-03")]
        public async Task DeveRetornarErroDeFormatoDeCpfRemetente(string cpf)
        {
            var conta = ObterDtoValido();
            conta.CpfContaRementente = cpf;

            var resultado = await _validator.ValidateAsync(conta);

            Assert.False(resultado.IsValid);
            Assert.Contains(resultado.Errors, e => e.ErrorMessage == string.Format(FraudSysResource.FormatoCpfInvalido, FraudSysResource.ContaRemetente));
        }

        [Theory]
        [InlineData("aisejiasej")]
        [InlineData("teste")]
        [InlineData("123.123.444-003")]
        [InlineData("123.123.444-3")]
        [InlineData("123123.444-3")]
        [InlineData("abc.123.444-03")]
        [InlineData("abc.dd.444-03")]
        public async Task DeveRetornarErroDeFormatoDeCpfDestinatario(string cpf)
        {
            var conta = ObterDtoValido();
            conta.CpfContaDestinatario = cpf;

            var resultado = await _validator.ValidateAsync(conta);

            Assert.False(resultado.IsValid);
            Assert.Contains(resultado.Errors, e => e.ErrorMessage == string.Format(FraudSysResource.FormatoCpfInvalido, FraudSysResource.ContaDestinatario));
        }

        [Theory]
        [InlineData("123.123.333-01")]
        [InlineData("111.222.555-05")]
        public async Task DeveRetornarErroDeContaRemetenteInexistente(string cpf)
        {
            var dto = ObterDtoValido();
            dto.CpfContaRementente = cpf;

            var resultado = await _validator.ValidateAsync(dto);

            Assert.False(resultado.IsValid);
            Assert.Contains(resultado.Errors, x => x.ErrorMessage == string.Format(FraudSysResource.ContaInformadaNaoExiste, FraudSysResource.ContaRemetente));
        }

        [Theory]
        [InlineData("123.123.333-01")]
        [InlineData("111.222.555-05")]
        public async Task DeveRetornarErroDeContaDestinatarioInexistente(string cpf)
        {
            var dto = ObterDtoValido();
            dto.CpfContaDestinatario = cpf;

            var resultado = await _validator.ValidateAsync(dto);

            Assert.False(resultado.IsValid);
            Assert.Contains(resultado.Errors, x => x.ErrorMessage == string.Format(FraudSysResource.ContaInformadaNaoExiste, FraudSysResource.ContaDestinatario));
        }

        [Theory]
        [InlineData("123.123.123-12", "123.123.123-12")]
        [InlineData("123.123.123-13", "123.123.123-13")]
        public async Task ContasNaoPodemSerIguais(string cpfRemetente, string cpfDestinatario)
        {
            var dto = ObterDtoValido();
            dto.CpfContaDestinatario = cpfDestinatario;
            dto.CpfContaRementente = cpfRemetente;

            var resultado = await _validator.ValidateAsync(dto);

            Assert.False(resultado.IsValid);
            Assert.Contains(resultado.Errors, x => x.ErrorMessage == FraudSysResource.AsContasNaoPodemSerIguais);
        }

  
        [Theory]
        [InlineData(-100d)]
        [InlineData(-1d)]
        [InlineData(-1123123d)]
        [InlineData(0d)]
        public async Task DeveRetornarErroDeValorInvalido(double valorDouble)
        {
            decimal valor = (decimal)valorDouble;
            var dto = ObterDtoValido();
            dto.Valor = valor;

            var resultado = await _validator.ValidateAsync(dto);

            Assert.False(resultado.IsValid);
            Assert.Contains(resultado.Errors, e => e.ErrorMessage == FraudSysResource.NaoEhPossivelDepositarValorMenorQueZero);
        }




        [Fact]
        public async Task NaoDeveRetornarErro()
        {
            var dto = ObterDtoValido();

            var resultado = await _validator.ValidateAsync(dto);

            Assert.True(resultado.IsValid);
        }

        private TransacaoInDTO ObterDtoValido()
        {
            return new TransacaoInDTO()
            {
                CpfContaRementente = "123.123.123-12",
                CpfContaDestinatario = "123.123.123-13",
                Valor = 500
            };
        }
    }
}
