using IgrejaSocial.Domain.Entities;
using IgrejaSocial.Domain.Enums;
using IgrejaSocial.Domain.Interfaces;
using IgrejaSocial.Domain.Services;
using System.Threading.Tasks;

namespace IgrejaSocial.Application.Services
{
    public class EquipamentoService
    {
        private readonly IEquipamentoRepository _repository;
        private readonly PatrimonioService _patrimonioService;

        public EquipamentoService(IEquipamentoRepository repository, PatrimonioService patrimonioService)
        {
            _repository = repository;
            _patrimonioService = patrimonioService;
        }

        public async Task<Equipamento> CriarAsync(Equipamento equipamento)
        {
            var proximoNumero = await ObterProximoNumeroAsync(equipamento.Tipo);
            equipamento.CodigoPatrimonio = _patrimonioService.GerarCodigoPatrimonio(equipamento.Tipo, proximoNumero);

            await _repository.AdicionarAsync(equipamento);
            await _repository.SalvarAlteracoesAsync();

            return equipamento;
        }

        private async Task<int> ObterProximoNumeroAsync(TipoEquipamento tipo)
        {
            var count = await _repository.ContarPorTipoAsync(tipo);
            return count + 1;
        }
    }
}
