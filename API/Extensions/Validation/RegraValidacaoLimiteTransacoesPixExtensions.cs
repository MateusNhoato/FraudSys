using FluentValidation;
using FraudSys.Resources;

namespace FraudSys.Extensions.Validation
{
    public static class RegraValidacaoLimiteTransacoesPixExtensions
    {

        public static IRuleBuilderOptions<T, decimal?> LimitePixValido<T>(
      this IRuleBuilder<T, decimal?> ruleBuilder)
        {
            return ruleBuilder.GreaterThan(0).WithMessage(FraudSysResource.LimiteDeveSerMaiorQueZero);
        }
    }
}
