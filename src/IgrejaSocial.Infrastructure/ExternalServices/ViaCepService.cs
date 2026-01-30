using IgrejaSocial.Domain.Interfaces;
using IgrejaSocial.Domain.Models;
using System.Net.Http.Json;

namespace IgrejaSocial.Infrastructure.ExternalServices
{
    public class ViaCepService : ICepService
    {
        private readonly HttpClient _httpClient;

        public ViaCepService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CepResponse> BuscarEnderecoPorCepAsync(string cep)
        {
            var cepLimpo = cep.Replace("-", "").Replace(".", "");
            
            // Usamos o operador de coalescência nula (??) para resolver o erro CS8603
            var result = await _httpClient.GetFromJsonAsync<CepResponse>($"{cepLimpo}/json/");
            
            return result ?? new CepResponse(); 
        }
    }
}