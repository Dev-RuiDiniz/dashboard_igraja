using IgrejaSocial.Domain.Interfaces;
using IgrejaSocial.Domain.Models;
using System.Net.Http.Json;

namespace IgrejaSocial.Infrastructure.ExternalServices
{
    public class ViaCepService : ICepService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ViaCepService> _logger;

        public ViaCepService(HttpClient httpClient, ILogger<ViaCepService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<CepResponse> BuscarEnderecoPorCepAsync(string cep)
        {
            var cepLimpo = cep.Replace("-", "").Replace(".", "");

            try
            {
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
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Falha ao consultar o CEP {Cep}.", cepLimpo);
                return new CepResponse { Erro = true, ServicoIndisponivel = true };
            }
        }
    }
}
