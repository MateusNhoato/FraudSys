using FluentValidation;
using FraudSys.DTO;
using FraudSys.Extensions.Validation;
using System.Drawing;

namespace FraudSys.Validators
{
    public class ContaInDTOValidator : AbstractValidator<ContaInDTO>
    {
        public ContaInDTOValidator() 
        {

            RuleFor(conta => conta.Cpf).CpfValido(string.Empty);

            RuleFor(conta => conta.Saldo).GreaterThanOrEqualTo(0).When(x => x.Saldo.HasValue).WithMessage("Não é possível cadastrar uma conta com saldo negativo.");

            RuleFor(conta => conta.Agencia).NotEmpty().WithMessage("Agência é obrigatória.")
                                           .Matches("^\\d{4}(-\\d{1})?$").WithMessage("Formato inválido de agência. Formatos válidos: \"1234\" ou \"1234-5\".");

            RuleFor(conta => conta.NumeroConta).NotEmpty().WithMessage("Número da conta é obrigatório.")
                                               .Matches("^\\d{5,12}-\\d{1}$").WithMessage("Formato inválido de número de conta. A conta deve ter de 5 a 12 dígitos, com dígito verificador (Ex: 123456-7, 12345-6).");

            RuleFor(conta => conta.LimiteTransacoesPix).LimitePixValido();
        }
    }
}
