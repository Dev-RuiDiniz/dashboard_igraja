using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using IgrejaSocial.Web;
using MudBlazor.Services;
using IgrejaSocial.Web.Services; // Adicione esta linha para resolver o erro CS0246

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// 1. Configuração do HttpClient com handler de notificação global
builder.Services.AddScoped<SnackbarDelegatingHandler>();
builder.Services.AddHttpClient("Api", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler<SnackbarDelegatingHandler>();
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("Api"));

// 2. Registro do MudBlazor
builder.Services.AddMudServices();

// 3. Injeção de Dependência dos seus Serviços
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<FamiliaService>();

await builder.Build().RunAsync();
