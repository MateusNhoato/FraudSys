using FluentValidation;
using FluentValidation.Results;
using FraudSys.DTO;
using FraudSys.Extensions.Validation;
using FraudSys.Repositories.Interfaces;
using FraudSys.Resources;

namespace FraudSys.Validators
{
    public class TransacaoInDTOValidator : AbstractValidator<TransacaoInDTO>
    {


        public TransacaoInDTOValidator(IContaRepository _contaRepository)
        {
            RuleFor(tr => tr.CpfContaRementente).Cascade(CascadeMode.Stop).CpfValido(FraudSysResource.ContaRemetente).ContaExiste(_contaRepository);
            RuleFor(tr => tr.CpfContaDestinatario).Cascade(CascadeMode.Stop).CpfValido(FraudSysResource.ContaDestinatario).ContaExiste(_contaRepository);
            RuleFor(tr => tr.Valor).GreaterThan(0).WithMessage(FraudSysResource.NaoEhPossivelDepositarValorMenorQueZero);
            RuleFor(tr => tr.CpfContaRementente).NotEqual(x => x.CpfContaDestinatario).WithMessage(FraudSysResource.AsContasNaoPodemSerIguais);
        }
    }
}
