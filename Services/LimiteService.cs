
using FraudSys.DTO;
using FraudSys.Models;
using FraudSys.Repositories.Interfaces;
using FraudSys.Validators;

namespace FraudSys.Services
{
    public class LimiteService : ILimiteService
    {

        private readonly IContaRepository _contaRepository;
        private readonly LimiteDTOValidator _limiteDTOValidator;
        private readonly ServicoDeMensagens _servicoDeMensagens;

        public LimiteService(IContaRepository contaRepository, LimiteDTOValidator limiteDTOValidator, ServicoDeMensagens servicoDeMensagens)
        {
            _contaRepository = contaRepository;
            _limiteDTOValidator = limiteDTOValidator;
            _servicoDeMensagens = servicoDeMensagens;
        }

        public async Task<bool> AtualizarLimite(LimiteDTO dto)
        {
            var validacao = await _limiteDTOValidator.ValidateAsync(dto);

            if(!validacao.IsValid)
            {
                _servicoDeMensagens.AdicionarMensagens(validacao.Errors);
                return false;
            }

            var conta = await _contaRepository.ObterAsync(dto.Cpf);
            AtualizarLimite(conta, dto.Valor);
            await _contaRepository.GravarAsync(conta);

            return true;
        }

        public async Task<decimal?> ObterLimite(string cpf)
        {
            if (! await _contaRepository.ContaExiste(cpf))
            {
                return null;
            }

            var conta = await _contaRepository.ObterAsync(cpf);

            return conta?.Limite;
        }

        public async Task<bool> RemoverLimite(string cpf)
        {
            if (!await _contaRepository.ContaExiste(cpf))
            {
                return false;
            }

            var conta = await _contaRepository.ObterAsync(cpf);
            AtualizarLimite(conta, 0);
            await _contaRepository.GravarAsync(conta);

            return true;
        }

        private void AtualizarLimite(Conta conta, decimal valor)
        {
            conta.Limite = valor;
        }
    }
}
