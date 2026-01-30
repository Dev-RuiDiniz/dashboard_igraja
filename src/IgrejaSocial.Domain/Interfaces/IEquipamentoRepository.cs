using IgrejaSocial.Domain.Entities;

namespace IgrejaSocial.Domain.Interfaces
{
    public interface IEquipamentoRepository
    {
        Task<Equipamento?> ObterPorIdAsync(Guid id);
        Task<Equipamento?> ObterPorCodigoAsync(string codigo);
        Task<IEnumerable<Equipamento>> ListarTodosAsync();
        Task<IEnumerable<Equipamento>> ListarDisponiveisAsync();
        Task AdicionarAsync(Equipamento equipamento);
        void Atualizar(Equipamento equipamento);
        Task SalvarAlteracoesAsync();
    }
}