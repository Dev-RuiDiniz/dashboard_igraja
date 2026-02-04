using System.Net;
using System.Net.Http;
using MudBlazor;
using Microsoft.AspNetCore.Components;

namespace IgrejaSocial.Web.Services
{
    public class SnackbarDelegatingHandler : DelegatingHandler
    {
        private readonly ISnackbar _snackbar;
        private readonly NavigationManager _navigationManager;

        public SnackbarDelegatingHandler(ISnackbar snackbar, NavigationManager navigationManager)
        {
            _snackbar = snackbar;
            _navigationManager = navigationManager;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                var response = await base.SendAsync(request, cancellationToken);

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    _snackbar.Add("Sessão expirada. Faça login novamente.", Severity.Warning);
                    _navigationManager.NavigateTo("/login");
                }
                else if (response.StatusCode == HttpStatusCode.Forbidden)
                {
                    _snackbar.Add("Você não tem permissão para essa ação.", Severity.Warning);
                }
                else if (!response.IsSuccessStatusCode)
                {
                    var content = response.Content is null
                        ? string.Empty
                        : await response.Content.ReadAsStringAsync(cancellationToken);
                    var message = string.IsNullOrWhiteSpace(content)
                        ? $"Erro ao chamar {request.RequestUri} (HTTP {(int)response.StatusCode})."
                        : content;

                    _snackbar.Add(message, Severity.Error);
                }

                return response;
            }
            catch (Exception ex)
            {
                _snackbar.Add($"Falha de comunicação: {ex.Message}", Severity.Error);
                throw;
            }
        }
    }
}
