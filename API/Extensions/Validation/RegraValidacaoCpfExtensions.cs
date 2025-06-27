using FluentValidation;
using FraudSys.Repositories;
using FraudSys.Resources;

namespace FraudSys.Extensions.Validation
{
    public static class RegraValidacaoCpfExtensions
    {

        public static IRuleBuilderOptions<T, string?> CpfValido<T>(
       this IRuleBuilder<T, string?> ruleBuilder, string? nomeCampo)
        {
            return ruleBuilder.NotEmpty().WithMessage(string.Format(FraudSysResource.CpfEhObrigatorio, !string.IsNullOrEmpty(nomeCampo) ? nomeCampo : string.Empty))
                                         .Matches(@"^\d{3}\.?\d{3}\.?\d{3}-?\d{2}$").WithMessage( string.Format(FraudSysResource.FormatoCpfInvalido, !string.IsNullOrEmpty(nomeCampo) ? nomeCampo : string.Empty));
        }
    }
}
