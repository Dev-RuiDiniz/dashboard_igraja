using IgrejaSocial.Domain.Entities;
using IgrejaSocial.Domain.Models;
using System.Net.Http.Json;

namespace IgrejaSocial.Web.Services
{
    public class DoacaoAvulsaService
    {
        private readonly HttpClient _httpClient;

        public DoacaoAvulsaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<DoacaoAvulsa>> GetDoacoesAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<DoacaoAvulsa>>("api/doacoesavulsas")
                   ?? new List<DoacaoAvulsa>();
        }

        public async Task<bool> CriarAsync(DoacaoAvulsaRequest request)
        {
            var response = await _httpClient.PostAsJsonAsync("api/doacoesavulsas", request);
            return response.IsSuccessStatusCode;
        }
    }
}
