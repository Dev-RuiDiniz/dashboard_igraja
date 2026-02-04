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
            
            var result = await _httpClient.GetFromJsonAsync<CepResponse>($"{cepLimpo}/json/");

            if (result is null)
            {
                return new CepResponse { Erro = true };
            }

            if (result.Erro || string.IsNullOrWhiteSpace(result.Cep))
            {
                result.Erro = true;
            }

            return result;
        }
    }
}
