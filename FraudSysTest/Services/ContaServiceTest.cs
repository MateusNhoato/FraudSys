using FraudSys.DTO;
using FraudSys.Models;
using FraudSys.Repositories.Interfaces;
using FraudSys.Services;
using FraudSysTest.MockRepositories;

namespace FraudSysTest.Services
{
    public class ContaServiceTest
    {
        private readonly IContaService _service;

        public ContaServiceTest()
        {
            var repositorio = new ContaTestRepository();
            _service = new ContaService(repositorio, new FraudSys.Validators.ContaInDTOValidator(), new FraudSys.Validators.AtualizarSaldoDTOValidator(repositorio), new ServicoDeMensagens());
        }


        [Theory]
        [InlineData("423.444.555-01")]
        [InlineData("423.444.555-05")]
        [InlineData("070.422.647-03")]
        public async Task NaoDeveRetornarContasQueNaoExistem(string cpf)
        {
            var conta = await _service.ObterAsync(cpf);

            Assert.Null(conta);
        }

        [Theory]
        [InlineData("123.123.123-12")]
        [InlineData("123.123.123-13")]
        public async Task DeveRetornarContasExistentes(string cpf)
        {
            var conta = await _service.ObterAsync(cpf);

            Assert.NotNull(conta);
        }
        
        
        [Theory]
        [InlineData("123.123.123-12")]
        [InlineData("123.123.123-13")]
        public async Task DeveRemoverContasExistentes(string cpf)
        {
            var removeu = await _service.RemoverAsync(cpf);
            Assert.True(removeu);
        }

        [Theory]
        [InlineData("123.123.123-44")]
        [InlineData("123.123.123-67")]
        public async Task NaoDeveRemoverContasInexistentes(string cpf)
        {
            var removeu = await _service.RemoverAsync(cpf);
            Assert.False(removeu);
        }



        [Fact]
        public async Task NaoDeveAdicionarContaInvalida()
        {
            var contaInDTO = new ContaInDTO()
            {
                Cpf = string.Empty,
                Agencia = "12345",
                NumeroConta = "55555-5",
                LimiteTransacoesPix = 5000,
                Saldo = 60000

            };

            var retorno = await _service.GravarAsync(contaInDTO);
            Assert.Null(retorno);
        }

        [Fact]
        public async Task DeveAdicionarContaValida()
        {
            var contaInDTO = new ContaInDTO()
            {
                Cpf = "123.123.444-03",
                Agencia = "12345",
                NumeroConta = "55555-5",
                LimiteTransacoesPix = 5000,
                Saldo = 60000

            };

            var retorno = await _service.GravarAsync(contaInDTO);
            Assert.Null(retorno);
        }


        [Theory]
        [InlineData("123.123.123-12", 1000d)]
        [InlineData("123.123.123-12", -600d)]
        [InlineData("123.123.123-13", 2000d)]
        [InlineData("123.123.123-13", -200d)]
        public async Task DeveAtualizarSaldoValidoEmUmaContaExistente(string cpfContaExistente, double valor)
        {

            var atualizou = await _service.AtualizarSaldoAsync(new AtualizarSaldoDTO() { Cpf = cpfContaExistente, Valor = (decimal)valor});

            Assert.True(atualizou);
        }

        [Theory]
        [InlineData("123.123.123-12", -5000d)]
        [InlineData("123.123.123-12", -60000d)]
        [InlineData("123.123.123-13", -200000d)]
        [InlineData("123.123.123-13", -50000d)]
        public async Task NaoDeveAtualizarSaldoInvalidoEmUmaContaExistente(string cpfContaExistente, double valor)
        {

            var atualizou = await _service.AtualizarSaldoAsync(new AtualizarSaldoDTO() { Cpf = cpfContaExistente, Valor = (decimal)valor });

            Assert.False(atualizou);
        }


    }
}
