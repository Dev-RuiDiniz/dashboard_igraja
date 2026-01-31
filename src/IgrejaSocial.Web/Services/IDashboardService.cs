using IgrejaSocial.Domain.Models;

namespace IgrejaSocial.Web.Services
{
    public interface IDashboardService
    {
        Task<DashboardStatsDto> GetResumoAsync();
    }
}