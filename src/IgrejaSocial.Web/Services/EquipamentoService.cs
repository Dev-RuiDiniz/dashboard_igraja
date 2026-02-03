using System.Net.Http.Json;
using IgrejaSocial.Domain.Entities;
using MudBlazor;

namespace IgrejaSocial.Web.Services
{
    public class EquipamentoService
    {
        private readonly HttpClient _httpClient;
        private readonly ISnackbar _snackbar;

        public EquipamentoService(HttpClient httpClient, ISnackbar snackbar)
        {
            _httpClient = httpClient;
            _snackbar = snackbar;
        }

        public async Task<List<Equipamento>> GetEquipamentosAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<Equipamento>>("api/equipamentos") ?? new();
            }
            catch
            {
                return new();
            }
        }

        public async Task<List<Equipamento>> GetDisponiveisAsync()
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<Equipamento>>("api/equipamentos/disponiveis") ?? new();
            }
            catch
            {
                return new();
            }
        }

        public async Task<bool> CriarEquipamentoAsync(Equipamento equipamento)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/equipamentos", equipamento);
                if (response.IsSuccessStatusCode)
                {
                    _snackbar.Add("Equipamento cadastrado com sucesso!", Severity.Success);
                    return true;
                }
                _snackbar.Add("Não foi possível cadastrar o equipamento.", Severity.Error);
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
