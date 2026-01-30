using IgrejaSocial.Domain.Entities;

namespace IgrejaSocial.Domain.Interfaces
{
    public interface IEquipamentoRepository
    {
        Task<Equipamento?> ObterPorCodigoAsync(string codigo);
        Task<IEnumerable<Equipamento>> ListarDisponiveisAsync();
        Task AdicionarAsync(Equipamento equipamento);
        Task SalvarAlteracoesAsync();
    }
}