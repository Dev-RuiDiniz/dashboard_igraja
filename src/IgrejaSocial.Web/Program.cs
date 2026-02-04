using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using IgrejaSocial.Web;
using IgrejaSocial.Web.Services;
using MudBlazor.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// ✅ URL DA API (troque pela porta real da sua API)
var apiBaseUrl = "https://localhost:7250/"; // EXEMPLO

// 1) HttpClient apontando para a API (não para o Web)
builder.Services.AddScoped<SnackbarDelegatingHandler>();

builder.Services.AddHttpClient("Api", client =>
{
    client.BaseAddress = new Uri("http://localhost:5099/");
})
.AddHttpMessageHandler<SnackbarDelegatingHandler>();

builder.Services.AddScoped(sp =>
    sp.GetRequiredService<IHttpClientFactory>().CreateClient("Api"));

// 2) MudBlazor
builder.Services.AddMudServices();

// 3) Auth / Authorization
builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<AuthService>();

// ✅ Registre o tipo concreto também (resolve erro do MainLayout)
builder.Services.AddScoped<ApiAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp =>
    sp.GetRequiredService<ApiAuthenticationStateProvider>());

// 4) Serviços
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<FamiliaService>();
builder.Services.AddScoped<EquipamentoService>();
builder.Services.AddScoped<EmprestimoService>();
builder.Services.AddScoped<CestaService>();
builder.Services.AddScoped<RelatorioService>();
builder.Services.AddScoped<PessoaSituacaoRuaService>();
builder.Services.AddScoped<DoacaoAvulsaService>();
builder.Services.AddScoped<VisitaService>();

await builder.Build().RunAsync();
