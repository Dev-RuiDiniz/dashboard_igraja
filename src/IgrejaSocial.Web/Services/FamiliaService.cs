using System.Net.Http.Json;
using IgrejaSocial.Domain.Models; // Onde reside o seu DTO de listagem

namespace IgrejaSocial.Web.Services
{
    public class FamiliaService
    {
        private readonly HttpClient _httpClient;

        public FamiliaService(HttpClient httpClient) => _httpClient = httpClient;

        public async Task<List<Familia>> GetFamiliasAsync() 
        {
            return await _httpClient.GetFromJsonAsync<List<Familia>>("api/familia") ?? new();
        }
    }
}