using System.Net.Http.Json;
using IgrejaSocial.Domain.Models; // Puxando do seu projeto Domain

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
                // Chama o endpoint que criaremos na API
                var response = await _httpClient.GetFromJsonAsync<DashboardStatsDto>("api/dashboard/resumo");
                return response ?? new DashboardStatsDto();
            }
            catch (Exception)
            {
                // Em caso de erro (API offline, etc), retorna valores zerados para não quebrar a UI
                return new DashboardStatsDto();
            }
        }
    }
}