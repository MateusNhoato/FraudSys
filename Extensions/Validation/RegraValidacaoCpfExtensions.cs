using FluentValidation;
using FraudSys.Repositories;

namespace FraudSys.Extensions.Validation
{
    public static class RegraValidacaoCpfExtensions
    {

        public static IRuleBuilderOptions<T, string?> CpfValido<T>(
       this IRuleBuilder<T, string?> ruleBuilder, string? nomeCampo)
        {
            return ruleBuilder.NotEmpty().WithMessage("CPF é obrigatório.")
                                         .Matches("/^\\d{3}\\.\\d{3}\\.\\d{3}\\-\\d{2}$/").WithMessage($"Formato de CPF {(string.IsNullOrEmpty(nomeCampo) ? string.Empty : nomeCampo + " ")}inválido.");
        }
    }
}
