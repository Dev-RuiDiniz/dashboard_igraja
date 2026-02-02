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
                _snackbar.Add("Erro ao carregar a lista de famílias.", Severity.Error);
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

                _snackbar.Add("Erro ao salvar cadastro. Verifique os dados.", Severity.Warning);
                return false;
            }
            catch
            {
                _snackbar.Add("Falha na comunicação com o servidor.", Severity.Error);
                return false;
            }
        }

        public async Task<CepResponse?> ConsultarCepAsync(string cep)
        {
            var cepLimpo = new string(cep.Where(char.IsDigit).ToArray());
            if (cepLimpo.Length != 8) return null;

            try 
            {
                var resultado = await _httpClient.GetFromJsonAsync<CepResponse>($"api/cep/{cepLimpo}");
                
                if (resultado == null || resultado.Erro)
                {
                    _snackbar.Add("CEP não encontrado ou inválido.", Severity.Warning);
                    return null;
                }

                return resultado;
            }
            catch
            {
                _snackbar.Add("Erro ao consultar o serviço de CEP.", Severity.Error);
                return null;
            }
        }
    }
}