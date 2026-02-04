using IgrejaSocial.Domain.Models;

namespace IgrejaSocial.Web.Services
{
    public interface IDashboardService
    {
        Task<DashboardStatsDto> GetResumoAsync();
        Task<List<FamiliaAtencaoDto>> GetFamiliasAtencaoAsync();
        Task<List<RankingVulnerabilidadeDto>> GetRankingVulnerabilidadeAsync(int limite);
        Task<List<VisitaAtrasadaDto>> GetVisitasAtrasadasAsync();
    }
}
