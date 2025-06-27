
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
        private readonly CpfDTOValidator _cpfDTOValidator;
        private readonly ServicoDeMensagens _servicoDeMensagens;

        public LimiteService(IContaRepository contaRepository, LimiteDTOValidator limiteDTOValidator, ServicoDeMensagens servicoDeMensagens, CpfDTOValidator cpfDTOValidator)
        {
            _contaRepository = contaRepository;
            _limiteDTOValidator = limiteDTOValidator;
            _servicoDeMensagens = servicoDeMensagens;
            _cpfDTOValidator = cpfDTOValidator;
        }

        public async Task<bool> AtualizarLimite(LimiteInDTO dto)
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

        public async Task<LimiteOutDTO> ObterLimite(string cpf)
        {
            var validacao = await _cpfDTOValidator.ValidateAsync(new CpfDTO() { Cpf = cpf });

            if (!validacao.IsValid)
            {
                _servicoDeMensagens.AdicionarMensagens(validacao.Errors);
                return null;
            }

            var conta = await _contaRepository.ObterAsync(cpf);

            return new LimiteOutDTO
            {
                Limite = conta.Limite
            };
        }

        public async Task<bool> RemoverLimite(string cpf)
        {
            var validacao = await _cpfDTOValidator.ValidateAsync(new CpfDTO() { Cpf = cpf });

            if (!validacao.IsValid)
            {
                _servicoDeMensagens.AdicionarMensagens(validacao.Errors);
                return false;
            }


            var conta = await _contaRepository.ObterAsync(cpf);
            AtualizarLimite(conta, null);
            await _contaRepository.GravarAsync(conta);

            return true;
        }

        private void AtualizarLimite(Conta conta, decimal? valor)
        {
            conta.Limite = valor;
        }
    }
}
