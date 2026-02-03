using IgrejaSocial.Domain.Entities;
using IgrejaSocial.Domain.Enums;
using IgrejaSocial.Domain.Interfaces;
using IgrejaSocial.Domain.Services;
using System;
using System.Linq;
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
            var prefixo = ObterPrefixo(tipo);
            var equipamentos = await _repository.ListarTodosAsync();

            var maiorNumero = equipamentos
                .Select(e => ExtrairNumeroDoCodigo(e.CodigoPatrimonio, prefixo))
                .DefaultIfEmpty(0)
                .Max();

            return maiorNumero + 1;
        }

        private static string ObterPrefixo(TipoEquipamento tipo)
        {
            var texto = tipo.ToString();
            return texto.Length >= 3 ? texto[..3].ToUpperInvariant() : texto.ToUpperInvariant();
        }

        private static int ExtrairNumeroDoCodigo(string? codigo, string prefixo)
        {
            if (string.IsNullOrWhiteSpace(codigo))
            {
                return 0;
            }

            var partes = codigo.Split('-', StringSplitOptions.RemoveEmptyEntries);
            if (partes.Length < 2 || !partes[0].Equals(prefixo, StringComparison.OrdinalIgnoreCase))
            {
                return 0;
            }

            return int.TryParse(partes[1], out var numero) ? numero : 0;
        }
    }
}
