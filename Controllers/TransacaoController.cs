using FraudSys.DTO;
using Microsoft.AspNetCore.Mvc;

namespace FraudSys.Controllers
{
    public class TransacaoController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Post(TransacaoDTO dto)
        {
            return Ok();
        }
    }
}
