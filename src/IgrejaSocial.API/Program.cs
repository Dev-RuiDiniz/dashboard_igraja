using IgrejaSocial.Infrastructure.Data;
using IgrejaSocial.Domain.Interfaces;
using IgrejaSocial.Infrastructure.ExternalServices;
using IgrejaSocial.Infrastructure.Repositories;
using IgrejaSocial.Domain.Services;
using IgrejaSocial.Application.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// --- 1. Configuração do CORS  ---
builder.Services.AddCors(options =>
{
    options.AddPolicy("BlazorPolicy", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});

// --- 2. Configuração do Banco de Dados (SQL Server) ---
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") 
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<IgrejaSocialDbContext>(options =>
    options.UseSqlServer(connectionString));

// --- 3. Registro do Serviço de CEP ---
builder.Services.AddHttpClient<ICepService, ViaCepService>(client =>
{
    client.BaseAddress = new Uri("https://viacep.com.br/ws/");
});

// --- 4. Controladores e Serialização JSON ---
builder.Services.AddControllers()
    .AddJsonOptions(options => 
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// --- 5. Injeção de Dependência (Services e Repositories) ---
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<SocialAnalysisService>();
builder.Services.AddScoped<PatrimonioService>();
builder.Services.AddScoped<EquipamentoService>();
builder.Services.AddScoped<IFamiliaRepository, FamiliaRepository>();
builder.Services.AddScoped<IEquipamentoRepository, EquipamentoRepository>();
builder.Services.AddScoped<IRegistroAtendimentoRepository, RegistroAtendimentoRepository>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// --- 6. Pipeline de Requisições HTTP (A ordem importa aqui!) ---

// O middleware do CORS deve vir antes de MapControllers e HttpsRedirection
app.UseCors("BlazorPolicy"); 

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
