using FluentValidation;
using FraudSys.DTO;
using FraudSys.Extensions.Validation;
using FraudSys.Resources;

namespace FraudSys.Validators
{
    public class ContaInDTOValidator : AbstractValidator<ContaInDTO>
    {
        public ContaInDTOValidator() 
        {

            RuleFor(conta => conta.Cpf).CpfValido(string.Empty);

            RuleFor(conta => conta.Saldo).GreaterThanOrEqualTo(0).When(x => x.Saldo.HasValue).WithMessage(FraudSysResource.NaoEhPossivelCadastrarContaComSaldoNegativo);

            RuleFor(conta => conta.Agencia).NotEmpty().WithMessage(FraudSysResource.AgenciaObrigatoria)
                                           .Matches("^\\d{4}(-\\d{1})?$").WithMessage(FraudSysResource.FormatoInvalidoDeAgencia);

            RuleFor(conta => conta.NumeroConta).NotEmpty().WithMessage(FraudSysResource.NumeroContaObrigatorio)
                                               .Matches("^\\d{5,12}-\\d{1}$").WithMessage(FraudSysResource.FormatoInvalidoNumeroDeConta);

            RuleFor(conta => conta.LimiteTransacoesPix).LimitePixValido().When(c => c.LimiteTransacoesPix.HasValue);
        }
    }
}
