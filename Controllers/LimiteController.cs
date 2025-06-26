using FraudSys.DTO;
using Microsoft.AspNetCore.Mvc;

namespace FraudSys.Controllers
{
    public class LimiteController : ControllerBase
    {
        [HttpGet("{cpf:string}")]
        public IActionResult Get(string cpf)
        {
            return Ok();
        }

        [HttpPatch]
        public IActionResult AtualizarLimite(LimiteDTO dto)
        {
            return Ok();
        }

        [HttpDelete]
        public IActionResult Delete(string cpf)
        {
            return Ok();
        }
    }
}
