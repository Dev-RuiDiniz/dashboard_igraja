using IgrejaSocial.Domain.Entities;
using IgrejaSocial.Domain.Enums; // Adicionado para resolver CS0246
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IgrejaSocial.Domain.Interfaces
{
    public interface IEquipamentoRepository
    {
        Task<Equipamento?> ObterPorIdAsync(Guid id);
        Task<Equipamento?> ObterPorCodigoAsync(string codigo);
        Task<IEnumerable<Equipamento>> ListarTodosAsync();
        Task<IEnumerable<Equipamento>> ListarDisponiveisAsync();
        Task<IEnumerable<Equipamento>> ListarPorStatusAsync(bool disponivel);
        Task<IEnumerable<Equipamento>> ListarPorTipoEEstadoAsync(TipoEquipamento tipo, StatusEquipamento estado);
        Task<int> ContarPorTipoAsync(TipoEquipamento tipo);
        Task AdicionarAsync(Equipamento equipamento);
        void Atualizar(Equipamento equipamento);
        Task SalvarAlteracoesAsync();
    }
}
