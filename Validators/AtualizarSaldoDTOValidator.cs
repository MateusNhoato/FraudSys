using FluentValidation;
using FraudSys.DTO;
using FraudSys.Extensions.Validation;
using FraudSys.Repositories.Interfaces;

namespace FraudSys.Validators
{
    public class AtualizarSaldoDTOValidator : AbstractValidator<AtualizarSaldoDTO>
    {

        public AtualizarSaldoDTOValidator(IContaRepository contaRepository)
        {
            RuleFor(atsaldo => atsaldo.Cpf).CpfValido(string.Empty).ContaExiste(contaRepository);
        }
    }
}
