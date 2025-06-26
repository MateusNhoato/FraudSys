using FraudSys.DTO;
using FraudSys.Services;
using Microsoft.AspNetCore.Mvc;

namespace FraudSys.Controllers
{
    public class BaseController : ControllerBase
    {
        private readonly ServicoDeMensagens _mensagens;


        private bool TemErros => _mensagens.TemErros;
        private IReadOnlyCollection<MensagemDTO> Mensagens => _mensagens.ObterMensagens();
        private IActionResult BadRequestPadrao => BadRequest(Mensagens);
        private IActionResult NotFoundPadrao => NotFound(Mensagens);


        private protected IActionResult GetBase<Entity>(Entity entity)
        {

            if (entity == null)
            {
                return NotFoundPadrao;
            }

            if (TemErros)
            {
                return BadRequestPadrao;
            }

            return Ok(entity);
        }

        private protected IActionResult GetBase<Entity>(IEnumerable<Entity> entidades)
        {

            if (entidades == null || !entidades.Any())
            {
                return NotFoundPadrao;
            }

            if (TemErros)
            {
                return BadRequestPadrao;
            }

            return Ok(entidades);
        }

        private protected IActionResult PutBase<Entity>(IEnumerable<Entity> entidades)
        {
            if (TemErros)
            {
                return BadRequestPadrao;
            }

            return Ok(entidades);
        }

        private protected IActionResult PutBase<Entity>(Entity entidade)
        {
            if (TemErros)
            {
                return BadRequestPadrao;
            }

            return Ok(entidade);
        }

        private protected IActionResult DeleteBase(bool deletou)
        {
            if (TemErros)
            {
                return BadRequestPadrao;
            }

            if (!deletou)
            {
                return NotFoundPadrao;
            }

            return Ok();
        }

        private protected IActionResult PatchBase(bool atualizou)
        {
            if (TemErros)
            {
                return BadRequestPadrao;
            }

            if (!atualizou)
            {
                return NotFoundPadrao;
            }

            return Ok();
        }
    }
}
