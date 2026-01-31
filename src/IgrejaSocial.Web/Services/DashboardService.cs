using System.Net.Http.Json;
using IgrejaSocial.Web.Models;

namespace IgrejaSocial.Web.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly HttpClient _httpClient;

        public DashboardService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<DashboardStatsDto> GetResumoAsync()
        {
            try 
            {
                return await _httpClient.GetFromJsonAsync<DashboardStatsDto>("api/dashboard/resumo") 
                       ?? new DashboardStatsDto();
            }
            catch
            {
                return new DashboardStatsDto(); // Retorna zerado em caso de erro na API
            }
        }
    }
}