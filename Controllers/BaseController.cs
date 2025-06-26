using FraudSys.DTO;
using FraudSys.Services;
using Microsoft.AspNetCore.Mvc;

namespace FraudSys.Controllers
{
    public class BaseController : ControllerBase
    {
        private readonly ServicoDeMensagens _mensagens;

        public BaseController(ServicoDeMensagens mensagens)
        {
            _mensagens = mensagens;
        }

        private bool TemErros => _mensagens.TemErros;
        private IReadOnlyCollection<MensagemDTO> Mensagens => _mensagens.ObterMensagens();
        private IActionResult BadRequestPadrao => BadRequest(Mensagens);
        private IActionResult NotFoundPadrao => NotFound(Mensagens);


        private protected IActionResult GetBase<Entity>(Entity entidade)
        {

            if (entidade == null)
            {
                return NotFoundPadrao;
            }

          
            return RetornoPadrao(entidade);
        }

        private protected IActionResult GetBase<Entity>(IEnumerable<Entity> entidades)
        {

            if (entidades == null || !entidades.Any())
            {
                return NotFoundPadrao;
            }

            return RetornoPadrao(entidades);
        }

        private protected IActionResult PutBase<Entity>(IEnumerable<Entity> entidades)
        {
            return RetornoPadrao(entidades);
        }

        private protected IActionResult PutBase<Entity>(Entity entidade)
        {
            return RetornoPadrao(entidade);
        }

        private protected IActionResult DeleteBase(bool deletou)
        {
            return RetornoComCondicao(deletou);
        }

        private protected IActionResult PatchBase(bool atualizou)
        {
            return RetornoComCondicao(atualizou);
        }

        public IActionResult RetornoPadrao<T>(T resultado)
        {
            if (TemErros)
            {
                return BadRequestPadrao;
            }

            return Ok(new RequestResultDTO<T>
            {
                Result = resultado,
                Mensagens = Mensagens.Select(m => m.Mensagem).ToArray()
            });
        }

        private IActionResult RetornoComCondicao(bool condicao)
        {
            if (TemErros)
            {
                return BadRequestPadrao;
            }

            if (!condicao)
            {
                return NotFoundPadrao;
            }

            return Ok();
        }
    }
}
