using IgrejaSocial.Domain.Entities;
using System.Net.Http.Json;

namespace IgrejaSocial.Web.Services
{
    public class PessoaSituacaoRuaService
    {
        private readonly HttpClient _httpClient;

        public PessoaSituacaoRuaService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<PessoaEmSituacaoRua>> GetPessoasAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<PessoaEmSituacaoRua>>("api/pessoassituacaorua")
                   ?? new List<PessoaEmSituacaoRua>();
        }

        public async Task<bool> CriarAsync(PessoaEmSituacaoRua pessoa)
        {
            var response = await _httpClient.PostAsJsonAsync("api/pessoassituacaorua", pessoa);
            return response.IsSuccessStatusCode;
        }
    }
}
