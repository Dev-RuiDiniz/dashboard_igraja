using System.Net.Http.Json;
using IgrejaSocial.Domain.Models;
using Microsoft.JSInterop;

namespace IgrejaSocial.Web.Services
{
    public class RelatorioService
    {
        private readonly HttpClient _httpClient;
        private readonly IJSRuntime _jsRuntime;

        public RelatorioService(HttpClient httpClient, IJSRuntime jsRuntime)
        {
            _httpClient = httpClient;
            _jsRuntime = jsRuntime;
        }

        public async Task<RelatorioMensalDto> GetRelatorioMensalAsync(int mes, int ano)
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<RelatorioMensalDto>($"api/relatorios/mensal?mes={mes}&ano={ano}");
                return response ?? new RelatorioMensalDto { Mes = mes, Ano = ano };
            }
            catch
            {
                return new RelatorioMensalDto { Mes = mes, Ano = ano };
            }
        }

        public async Task<bool> DownloadCestasAtendidasCsvAsync(int mes, int ano)
        {
            try
            {
                var response = await _httpClient.GetAsync($"api/relatorios/atendimentos-cestas/export?mes={mes}&ano={ano}");
                if (!response.IsSuccessStatusCode)
                {
                    return false;
                }

                var bytes = await response.Content.ReadAsByteArrayAsync();
                var fileName = $"familias-atendidas-{mes:D2}-{ano}.csv";
                var base64 = Convert.ToBase64String(bytes);

                await _jsRuntime.InvokeVoidAsync("downloadFileFromBytes", fileName, "text/csv", base64);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
