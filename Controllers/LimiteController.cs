using FraudSys.DTO;
using FraudSys.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FraudSys.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LimiteController : BaseController
    {
        private readonly ILimiteService _limiteService;

        public LimiteController(ServicoDeMensagens servicoDeMensagens, ILimiteService limiteService) : base(servicoDeMensagens)
        {
            _limiteService = limiteService;
        }


        [HttpGet("{cpf:string}")]
        public async Task<IActionResult> Get(string cpf)
        {
            var limite = await _limiteService.ObterLimite(cpf);
            return GetBase(limite);
        }

        [HttpPatch]
        public async Task<IActionResult> AtualizarLimite(LimiteDTO dto)
        {
            var atualizou = await _limiteService.AtualizarLimite(dto);
            return PatchBase(atualizou);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string cpf)
        {
            var deletou = await _limiteService.RemoverLimite(cpf);
            return DeleteBase(deletou);
        }
    }
}
