using FluentValidation;
using FraudSys.DTO;
using FraudSys.Extensions.Validation;
using FraudSys.Repositories;
using FraudSys.Repositories.Interfaces;

namespace FraudSys.Validators
{
    public class CpfDTOValidator : AbstractValidator<CpfDTO>
    {
        public CpfDTOValidator(IContaRepository repository) 
        {
            RuleFor(c => c.Cpf).Cascade(CascadeMode.Stop).CpfValido(string.Empty).ContaExiste(repository);
        }
    }
}
