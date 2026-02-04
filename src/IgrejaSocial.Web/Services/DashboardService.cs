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

        public async Task<List<FamiliaAtencaoDto>> GetFamiliasAtencaoAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<FamiliaAtencaoDto>>("api/dashboard/atencao");
                return response ?? new List<FamiliaAtencaoDto>();
            }
            catch
            {
                return new List<FamiliaAtencaoDto>();
            }
        }

        public async Task<List<RankingVulnerabilidadeDto>> GetRankingVulnerabilidadeAsync(int limite)
        {
            try
            {
                var response = await _httpClient
                    .GetFromJsonAsync<List<RankingVulnerabilidadeDto>>($"api/dashboard/ranking-vulnerabilidade?limite={limite}");
                return response ?? new List<RankingVulnerabilidadeDto>();
            }
            catch
            {
                return new List<RankingVulnerabilidadeDto>();
            }
        }

        public async Task<List<VisitaAtrasadaDto>> GetVisitasAtrasadasAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<VisitaAtrasadaDto>>("api/dashboard/visitas-atrasadas");
                return response ?? new List<VisitaAtrasadaDto>();
            }
            catch
            {
                return new List<VisitaAtrasadaDto>();
            }
        }
    }
}
