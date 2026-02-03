using System.Net.Http.Json;
using IgrejaSocial.Domain.Entities;
using MudBlazor;

namespace IgrejaSocial.Web.Services
{
    public class EmprestimoService
    {
        private readonly HttpClient _httpClient;
        private readonly ISnackbar _snackbar;

        public EmprestimoService(HttpClient httpClient, ISnackbar snackbar)
        {
            _httpClient = httpClient;
            _snackbar = snackbar;
        }

        public async Task<bool> CriarEmprestimoAsync(RegistroAtendimento registro)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/emprestimos", registro);
                if (response.IsSuccessStatusCode)
                {
                    _snackbar.Add("Empréstimo registrado com sucesso!", Severity.Success);
                    return true;
                }

                var erro = await response.Content.ReadAsStringAsync();
                _snackbar.Add(string.IsNullOrWhiteSpace(erro) ? "Não foi possível registrar o empréstimo." : erro, Severity.Error);
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
