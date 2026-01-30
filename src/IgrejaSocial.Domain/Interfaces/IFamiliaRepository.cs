using IgrejaSocial.Domain.Entities;

namespace IgrejaSocial.Domain.Interfaces
{
    public interface IFamiliaRepository
    {
        Task<Familia?> ObterPorIdAsync(Guid id);
        Task<IEnumerable<Familia>> ListarTodasAsync();
        Task<IEnumerable<Familia>> ListarVulneraveisAsync(); // Consulta complexa
        Task AdicionarAsync(Familia familia);
        void Atualizar(Familia familia);
        Task SalvarAlteracoesAsync();
    }
}