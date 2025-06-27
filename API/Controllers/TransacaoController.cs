using FraudSys.DTO;
using FraudSys.Services;
using FraudSys.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FraudSys.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TransacaoController : BaseController
    {
        private readonly ITransacaoService _transacaoService;

        public TransacaoController(ServicoDeMensagens mensagens, ITransacaoService transacaoService) : base(mensagens)
        {
            _transacaoService = transacaoService;
        }

        [HttpPost]
        public async Task<IActionResult> Post(TransacaoInDTO dto)
        {
            var retorno = await _transacaoService.EfetuarTransacao(dto);

            return RetornoPadrao(retorno);
        }
    }
}
