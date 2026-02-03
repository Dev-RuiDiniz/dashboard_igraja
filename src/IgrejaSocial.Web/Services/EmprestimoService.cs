using System.Net.Http.Json;
using IgrejaSocial.Domain.Entities;
using IgrejaSocial.Domain.Enums;
using IgrejaSocial.Web.Models;
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

        public async Task<List<HistoricoEmprestimoDto>> ListarHistoricoFamiliaAsync(Guid familiaId)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<List<HistoricoEmprestimoDto>>($"api/emprestimos/familia/{familiaId}") ?? new();
            }
            catch
            {
                return new();
            }
        }

        public async Task<bool> RegistrarDevolucaoAsync(Guid registroId, DateTime dataDevolucao, EstadoConservacao estado)
        {
            var payload = new
            {
                DataDevolucao = dataDevolucao,
                EstadoConservacao = estado
            };

            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/emprestimos/{registroId}/devolucao", payload);
                if (response.IsSuccessStatusCode)
                {
                    _snackbar.Add("Devolução registrada com sucesso!", Severity.Success);
                    return true;
                }

                var erro = await response.Content.ReadAsStringAsync();
                _snackbar.Add(string.IsNullOrWhiteSpace(erro) ? "Não foi possível registrar a devolução." : erro, Severity.Error);
                return false;
            }
            catch
            {
                return false;
            }
        }

        public async Task<HistoricoEmprestimoDto?> ObterEmprestimoAtivoAsync(Guid equipamentoId)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<HistoricoEmprestimoDto>($"api/emprestimos/ativos/equipamento/{equipamentoId}");
            }
            catch
            {
                return null;
            }
        }
    }
}
