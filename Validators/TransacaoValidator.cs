using FluentValidation;
using FluentValidation.Results;
using FraudSys.DTO;
using FraudSys.Extensions.Validation;
using FraudSys.Repositories.Interfaces;

namespace FraudSys.Validators
{
    public class TransacaoValidator : AbstractValidator<TransacaoInDTO>
    {


        public TransacaoValidator(IContaRepository _contaRepository)
        {
            RuleFor(tr => tr.CpfContaRementente).CpfValido("conta do remetente").ContaExiste(_contaRepository);
            RuleFor(tr => tr.CpfContaDestinatario).CpfValido("conta do destinatário").ContaExiste(_contaRepository);
            RuleFor(tr => tr.Valor).GreaterThan(0).WithMessage("Não é possível depositar valores negativos.");
        }
    }
}
