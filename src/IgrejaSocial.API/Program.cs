using IgrejaSocial.Infrastructure.Data;
using IgrejaSocial.Domain.Interfaces;
using IgrejaSocial.Infrastructure.ExternalServices;
using IgrejaSocial.Infrastructure.Repositories;
using IgrejaSocial.Domain.Services;
using IgrejaSocial.Application.Services;
using IgrejaSocial.Domain.Identity;
using IgrejaSocial.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Reflection;
using System.Text.Json.Serialization;
using QuestPDF.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

QuestPDF.Settings.License = LicenseType.Community;

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

// --- 3. Configuração do Identity ---
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequireUppercase = true;
        options.Password.RequiredLength = 8;
        options.User.RequireUniqueEmail = true;
    })
    .AddEntityFrameworkStores<IgrejaSocialDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(RoleNames.Administrador, policy => policy.RequireRole(RoleNames.Administrador));
    options.AddPolicy(RoleNames.Voluntario, policy => policy.RequireRole(RoleNames.Administrador, RoleNames.Voluntario));
});

// --- 4. Registro do Serviço de CEP ---
var cepBaseUrl = builder.Configuration.GetValue<string>("CepService:BaseUrl")
    ?? throw new InvalidOperationException("CepService:BaseUrl not configured.");
builder.Services.AddHttpClient<ICepService, ViaCepService>(client =>
{
    client.BaseAddress = new Uri(cepBaseUrl);
});

// --- 5. Controladores e Serialização JSON ---
builder.Services.AddControllers()
    .AddJsonOptions(options => 
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });

// --- 6. Injeção de Dependência (Services e Repositories) ---
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    if (File.Exists(xmlPath))
    {
        options.IncludeXmlComments(xmlPath);
    }
});
builder.Services.AddScoped<SocialAnalysisService>();
builder.Services.AddScoped<PatrimonioService>();
builder.Services.AddScoped<EquipamentoService>();
builder.Services.AddScoped<IFamiliaRepository, FamiliaRepository>();
builder.Services.AddScoped<IEquipamentoRepository, EquipamentoRepository>();
builder.Services.AddScoped<IRegistroAtendimentoRepository, RegistroAtendimentoRepository>();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

// --- 7. Pipeline de Requisições HTTP (A ordem importa aqui!) ---

// O middleware do CORS deve vir antes de MapControllers e HttpsRedirection
app.UseCors("BlazorPolicy"); 

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapFallbackToFile("index.html");

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    var roles = new[] { RoleNames.Administrador, RoleNames.Voluntario };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    var adminEmail = builder.Configuration.GetValue<string>("AdminSeed:Email");
    var adminPassword = builder.Configuration.GetValue<string>("AdminSeed:Password");
    if (!string.IsNullOrWhiteSpace(adminEmail) && !string.IsNullOrWhiteSpace(adminPassword))
    {
        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser is null)
        {
            adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(adminUser, adminPassword);
            if (result.Succeeded)
            {
                await userManager.AddToRolesAsync(adminUser, roles);
            }
        }
    }
}

app.Run();

public partial class Program { }
