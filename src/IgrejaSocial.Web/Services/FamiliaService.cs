using System.Net.Http.Json;
using IgrejaSocial.Domain.Entities;
using IgrejaSocial.Domain.Models;
using MudBlazor;

namespace IgrejaSocial.Web.Services
{
    public class FamiliaService
    {
        private readonly HttpClient _httpClient;
        private readonly ISnackbar _snackbar;

        public FamiliaService(HttpClient httpClient, ISnackbar snackbar)
        {
            _httpClient = httpClient;
            _snackbar = snackbar;
        }

        public async Task<List<Familia>> GetFamiliasAsync() 
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<Familia>>("api/familia") ?? new();
            }
            catch (Exception)
            {
                return new();
            }
        }

        public async Task<bool> CriarFamiliaAsync(Familia familia)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/familia", familia);
                
                if (response.IsSuccessStatusCode)
                {
                    _snackbar.Add("Cadastro realizado com sucesso!", Severity.Success);
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public async Task<CepResponse?> ConsultarCepAsync(string cep)
        {
            var cepLimpo = new string(cep.Where(char.IsDigit).ToArray());
            if (cepLimpo.Length != 8) return null;

            try 
            {
                var response = await _httpClient.GetAsync($"api/cep/{cepLimpo}");
                if (!response.IsSuccessStatusCode)
                {
                    _snackbar.Add("CEP não encontrado ou inválido.", Severity.Warning);
                    return null;
                }

                var resultado = await response.Content.ReadFromJsonAsync<CepResponse>();
                if (resultado == null || resultado.Erro)
                {
                    _snackbar.Add("CEP não encontrado ou inválido.", Severity.Warning);
                    return null;
                }

                return resultado;
            }
            catch
            {
                _snackbar.Add("Não foi possível consultar o CEP no momento.", Severity.Error);
                return null;
            }
        }

        public async Task<List<HistoricoUnificadoDto>> GetHistoricoUnificadoAsync(Guid familiaId)
        {
            try
            {
                return await _httpClient
                           .GetFromJsonAsync<List<HistoricoUnificadoDto>>($"api/familia/{familiaId}/historico-unificado")
                       ?? new List<HistoricoUnificadoDto>();
            }
            catch
            {
                return new List<HistoricoUnificadoDto>();
            }
        }
    }
}
