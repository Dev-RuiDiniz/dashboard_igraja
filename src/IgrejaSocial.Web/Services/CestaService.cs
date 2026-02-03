using System.Net.Http.Json;
using IgrejaSocial.Domain.Models;
using MudBlazor;

namespace IgrejaSocial.Web.Services
{
    public class CestaService
    {
        private readonly HttpClient _httpClient;
        private readonly ISnackbar _snackbar;

        public CestaService(HttpClient httpClient, ISnackbar snackbar)
        {
            _httpClient = httpClient;
            _snackbar = snackbar;
        }

        public async Task<bool> RegistrarEntregaAsync(CestaBasicaEntregaRequest request)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/cestas", request);
                if (response.IsSuccessStatusCode)
                {
                    _snackbar.Add("Entrega de cesta registrada com sucesso!", Severity.Success);
                    return true;
                }

                var erro = await response.Content.ReadAsStringAsync();
                _snackbar.Add(string.IsNullOrWhiteSpace(erro) ? "Não foi possível registrar a entrega." : erro, Severity.Error);
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
