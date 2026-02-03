using System.Net.Http;
using MudBlazor;

namespace IgrejaSocial.Web.Services
{
    public class SnackbarDelegatingHandler : DelegatingHandler
    {
        private readonly ISnackbar _snackbar;

        public SnackbarDelegatingHandler(ISnackbar snackbar)
        {
            _snackbar = snackbar;
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                var response = await base.SendAsync(request, cancellationToken);

                if (!response.IsSuccessStatusCode)
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
