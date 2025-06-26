using FraudSys.DTO;
using FraudSys.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FraudSys.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ContaController : BaseController
    {
        private readonly IContaService _contaService;


        [HttpGet("{cpf:string}")]
        public async Task<IActionResult> Get(string cpf)
        {
            var conta = await _contaService.ObterAsync(cpf);
            return GetBase(conta);
        }

        [HttpPut]
        public async Task<IActionResult> Put(ContaDTO dto)
        {
            var conta = await _contaService.GravarAsync(dto);
            return PutBase(conta);
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(string cpf)
        {
            var deletou = await _contaService.RemoverAsync(cpf);
            return DeleteBase(deletou);
        }

        [HttpPatch("AtualizarSaldo")]
        public async Task<IActionResult> AtualizarSaldo(AtualizarSaldoDTO dto)
        {
            var atualizou = await _contaService.AtualizarSaldoAsync(dto);

            return PatchBase(atualizou);
        }
    }
}
