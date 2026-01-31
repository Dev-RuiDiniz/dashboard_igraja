using System.Net.Http.Json;
using IgrejaSocial.Domain.Entities;
using IgrejaSocial.Domain.Models; // Use o novo namespace do Domain

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

        public async Task<CepResponse?> ConsultarCepAsync(string cep)
        {
            var cepLimpo = new string(cep.Where(char.IsDigit).ToArray());
            if (cepLimpo.Length != 8) return null;

            try 
            {
                // A chamada vai para o seu Controller da API
                return await _httpClient.GetFromJsonAsync<CepResponse>($"api/cep/{cepLimpo}");
            }
            catch
            {
                return null;
            }
        }
    }
}