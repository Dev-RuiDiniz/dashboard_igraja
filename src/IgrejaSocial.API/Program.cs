using IgrejaSocial.Infrastructure.Data;
using IgrejaSocial.Domain.Interfaces;
using IgrejaSocial.Infrastructure.ExternalServices;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// --- 1. Configuração do Banco de Dados (SQLite) ---
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<IgrejaSocialDbContext>(options =>
    options.UseSqlite(connectionString));

// --- 2. Registro do Serviço de CEP ---
// Agora o builder já existe, então podemos registrar o HttpClient
builder.Services.AddHttpClient<ICepService, ViaCepService>(client =>
{
    client.BaseAddress = new Uri("https://viacep.com.br/ws/");
});

// --- 3. Controladores e Serialização JSON ---
builder.Services.AddControllers()
    .AddJsonOptions(options => 
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// --- 4. Swagger/OpenAPI ---
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// --- 5. Pipeline de Requisições HTTP ---
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();