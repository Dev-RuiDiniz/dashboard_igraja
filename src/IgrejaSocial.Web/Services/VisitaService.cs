using IgrejaSocial.Domain.Models;
using System.Net.Http.Json;

namespace IgrejaSocial.Web.Services
{
    public class VisitaService
    {
        private readonly HttpClient _httpClient;

        public VisitaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<RegistroVisitaDto>> GetVisitasAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<RegistroVisitaDto>>("api/visitas")
                   ?? new List<RegistroVisitaDto>();
        }

        public async Task<bool> SolicitarAsync(RegistroVisitaRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("api/visitas", request);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ConcluirAsync(RegistroVisitaConclusaoRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("api/visitas/concluir", request);
            return response.IsSuccessStatusCode;
        }
    }
}
