using FluentValidation;
using FraudSys.Repositories.Interfaces;

namespace FraudSys.Extensions.Validation
{
    public static class RegraValidacaoContaExtensions
    {
        public static IRuleBuilderOptions<T, string?> ContaExiste<T>(
        this IRuleBuilder<T, string?> ruleBuilder,
        IContaRepository _contaRepository)
        {
            return ruleBuilder
                .MustAsync(async (cpf, ct) => await _contaRepository.ContaExiste(cpf))
                .WithMessage("Conta informada não existe.");
        }
    }
}
