using System.Net.Http.Json;
using IgrejaSocial.Domain.Entities; // Adicione este using para reconhecer a classe Familia

namespace IgrejaSocial.Web.Services
{
    public class FamiliaService
    {
        private readonly HttpClient _httpClient;

        public FamiliaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Familia>> GetFamiliasAsync() 
        {
            try 
            {
                return await _httpClient.GetFromJsonAsync<List<Familia>>("api/familia") ?? new();
            }
            catch
            {
                return new List<Familia>();
            }
        }
        public async Task<CepResponseDto> ConsultarCepAsync(string cep)
        {
            // Remove caracteres especiais antes de enviar
            var cepLimpo = new string(cep.Where(char.IsDigit).ToArray());
            if (cepLimpo.Length != 8) return null;

            try 
            {
                return await _httpClient.GetFromJsonAsync<CepResponseDto>($"api/cep/{cepLimpo}");
            }
            catch
            {
                return null;
            }
        }
    }
}