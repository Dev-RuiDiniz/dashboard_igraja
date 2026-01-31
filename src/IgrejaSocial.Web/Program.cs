using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using IgrejaSocial.Web;
using MudBlazor.Services;
using IgrejaSocial.Web.Services; // Adicione esta linha para resolver o erro CS0246

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// 1. Configuração do HttpClient (Mova para cima para melhor organização)
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// 2. Registro do MudBlazor
builder.Services.AddMudServices();

// 3. Injeção de Dependência dos seus Serviços
builder.Services.AddScoped<IDashboardService, DashboardService>();

await builder.Build().RunAsync();