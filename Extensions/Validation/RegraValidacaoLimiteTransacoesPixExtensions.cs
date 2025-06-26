using FluentValidation;

namespace FraudSys.Extensions.Validation
{
    public static class RegraValidacaoLimiteTransacoesPixExtensions
    {

        public static IRuleBuilderOptions<T, decimal> LimitePixValido<T>(
      this IRuleBuilder<T, decimal> ruleBuilder)
        {
            return ruleBuilder.GreaterThanOrEqualTo(0).WithMessage("O limite de transações pix não pode ser negativo.");
        }
    }
}
