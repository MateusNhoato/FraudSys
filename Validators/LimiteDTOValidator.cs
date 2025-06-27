using FluentValidation;
using FraudSys.DTO;
using FraudSys.Extensions.Validation;
using FraudSys.Repositories.Interfaces;

namespace FraudSys.Validators
{
    public class LimiteDTOValidator : AbstractValidator<LimiteInDTO>
    {
        public LimiteDTOValidator(IContaRepository contaRepository) 
        {
            RuleFor(lim=> lim.Cpf).CpfValido(string.Empty).ContaExiste(contaRepository);

            RuleFor(lim => lim.Valor).LimitePixValido();
        }
    }
}
