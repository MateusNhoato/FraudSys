using FluentAssertions;
using FraudSys.DTO;
using FraudSys.Validators;
using FraudSys.Resources;

namespace FraudSysTest.Validators
{
    public class ContaInDTOValidatorTest
    {
        private readonly ContaInDTOValidator _validator;

        public ContaInDTOValidatorTest()
        {
            _validator = new ContaInDTOValidator();
        }

        [Fact]
        public void DeveRetornarErroDeCpfObrigatorio()
        {
            var conta = ObterContaInDTOValido();
            conta.Cpf = string.Empty;

            var resultado = _validator.Validate(conta);

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
        public void DeveRetornarErroDeFormatoDeCpf(string cpf)
        {
            var conta = ObterContaInDTOValido();
            conta.Cpf = cpf;

            var resultado = _validator.Validate(conta);

            Assert.False(resultado.IsValid);
            Assert.Contains(resultado.Errors, e => e.PropertyName == "Cpf");
        }

        [Theory]
        [InlineData("123.123.123-02")]
        [InlineData("070.321.442-05")]
        [InlineData("873.525.122-06")]
        public void NaoDeveRetornarErroDeCpf(string cpf)
        {
            var conta = ObterContaInDTOValido();
            conta.Cpf = cpf;

            var resultado = _validator.Validate(conta);

            Assert.True(resultado.IsValid);
            Assert.DoesNotContain(resultado.Errors, e => e.PropertyName == "Cpf");
        }


        [Fact]
        public void DeveRetornarErroDeAgenciaObrigatoria()
        {
            var conta= ObterContaInDTOValido();
            conta.Agencia = string.Empty;

            var resultado = _validator.Validate(conta);

            Assert.False(resultado.IsValid);
            Assert.Contains(resultado.Errors, e => e.ErrorMessage == FraudSysResource.AgenciaObrigatoria);
        }

        [Theory]
        [InlineData("0000-02")]
        [InlineData("aaaa-aa")]
        [InlineData("aaaa-1")]
        [InlineData("4412-a")]
        [InlineData("aaaa-a")]
        [InlineData("000-3")]
        [InlineData("0000--1")]
        [InlineData("00000-1")]
        [InlineData("000000")]
        [InlineData("aksdpaosk")]
        public void DeveRetornarErroDeFormatoDeAgencia(string agencia)
        {
            var conta = ObterContaInDTOValido();
            conta.Agencia = agencia;

            var resultado = _validator.Validate(conta);

            Assert.False(resultado.IsValid);
            Assert.Contains(resultado.Errors, e => e.ErrorMessage == FraudSysResource.FormatoInvalidoDeAgencia);
        }

        [Theory]
        [InlineData("0000-2")]
        [InlineData("1234-1")]
        [InlineData("0000")]
        [InlineData("1234")]
        public void NaoDeveRetornarErroDeAgencia(string agencia)
        {
            var conta = ObterContaInDTOValido();
            conta.Agencia = agencia;

            var resultado = _validator.Validate(conta);

            Assert.True(resultado.IsValid);
            Assert.DoesNotContain(resultado.Errors, e => e.PropertyName == "Agencia");
        }

        [Fact]
        public void DeveRetornarErroDeNumeroContaObrigatorio()
        {
            var conta = ObterContaInDTOValido();
            conta.NumeroConta = string.Empty;

            var resultado = _validator.Validate(conta);

            Assert.False(resultado.IsValid);
            Assert.Contains(resultado.Errors, e => e.ErrorMessage == FraudSysResource.NumeroContaObrigatorio);
        }


        [Theory]
        [InlineData("aaaaa-a")]
        [InlineData("00000-a")]
        [InlineData("0000-1")]
        [InlineData("00A03-1")]
        [InlineData("1293129301290393021")]
        [InlineData("12askdoas-1")]
        public void DeveRetornarErroDeFormatoDeNumeroDeConta(string numeroConta)
        {
            var conta = ObterContaInDTOValido();
            conta.NumeroConta = numeroConta;

            var resultado = _validator.Validate(conta);

            Assert.False(resultado.IsValid);
            Assert.Contains(resultado.Errors, e => e.ErrorMessage == FraudSysResource.FormatoInvalidoNumeroDeConta);
        }

        [Theory]
        [InlineData("00000-1")]
        [InlineData("123455-2")]
        [InlineData("1234557-2")]
        [InlineData("12345575-2")]
        [InlineData("123455759-2")]
        [InlineData("1234557591-2")]
        [InlineData("12345575912-2")]
        [InlineData("123455759123-2")]

        public void NaoDeveRetornarErroDeNumeroDeConta(string numeroConta)
        {
            var conta = ObterContaInDTOValido();
            conta.NumeroConta = numeroConta;

            var resultado = _validator.Validate(conta);

            Assert.True(resultado.IsValid);
            Assert.DoesNotContain(resultado.Errors, e => e.PropertyName == "NumeroConta");
        }

        [Theory]
        [InlineData(-100)]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(-1123123)]
        public void DeveRetornarErroDeLimiteTransacaoPixInvalido(decimal valor)
        {
            var conta = ObterContaInDTOValido();
            conta.LimiteTransacoesPix = valor;

            var resultado = _validator.Validate(conta);

            Assert.False(resultado.IsValid);
            Assert.Contains(resultado.Errors, e => e.ErrorMessage == FraudSysResource.LimiteDeveSerMaiorQueZero);
        }

        [Theory]
        [InlineData(100d)]
        [InlineData(null)]
        [InlineData(100000d)]
        [InlineData(25000d)]
        public void NaoDeveRetornarErroDeLimiteTransacaoPixInvalido(double? valorDouble)
        {
            decimal? valor = (decimal?)valorDouble;
            var conta = ObterContaInDTOValido();
            conta.LimiteTransacoesPix = valor;

            var resultado = _validator.Validate(conta);

            Assert.True(resultado.IsValid);
            Assert.DoesNotContain(resultado.Errors, e => e.PropertyName == "LimiteTransacoesPix");
        }




        [Theory]
        [InlineData(-100d)]
        [InlineData(-1d)]
        [InlineData(-1123123d)]
        public void DeveRetornarErroDeSaldoInvalido(double? valorDouble)
        {
            decimal? valor = (decimal?)valorDouble;
            var conta = ObterContaInDTOValido();
            conta.Saldo = valor;

            var resultado = _validator.Validate(conta);

            Assert.False(resultado.IsValid);
            Assert.Contains(resultado.Errors, e => e.ErrorMessage == FraudSysResource.NaoEhPossivelCadastrarContaComSaldoNegativo);
        }


        [Theory]
        [InlineData(0)]
        [InlineData(10000)]
        [InlineData(200000)]
        public void NaoDeveRetornarErroDeSaldo(decimal valor)
        {
            var conta = ObterContaInDTOValido();
            conta.Saldo = valor;

            var resultado = _validator.Validate(conta);

            Assert.True(resultado.IsValid);
            Assert.DoesNotContain(resultado.Errors, e => e.PropertyName == "Saldo");
        }

        [Fact]
        public void NaoDeveRetornarErros()
        {
            var conta = ObterContaInDTOValido();
            var resultado = _validator.Validate(conta);
            Assert.True(resultado.IsValid);
        }


        private ContaInDTO ObterContaInDTOValido()
        {
            return new ContaInDTO() { NumeroConta = "55555-5",
                                      Cpf = "123.123.123-12",
                                      Agencia = "0000", 
                                      LimiteTransacoesPix = 1000,
                                      Saldo = 10000 };
        }
    }
}