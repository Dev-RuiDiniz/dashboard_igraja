using System;
using System.Net;
using System.Net.Http.Json;
using IgrejaSocial.Domain.Entities;
using IgrejaSocial.Domain.Enums;
using IgrejaSocial.Domain.Models;
using IgrejaSocial.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace IgrejaSocial.IntegrationTests
{
    public class EntregaCestaTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;

        public EntregaCestaTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Deve_registrar_entrega_cesta_basica()
        {
            var familiaId = await SeedFamiliaAsync();
            var client = _factory.CreateClient();

            await AutenticarAsync(client);

            var request = new CestaBasicaEntregaRequest
            {
                FamiliaId = familiaId,
                DataEntrega = DateTime.Today,
                Observacoes = "Entrega de teste"
            };

            var response = await client.PostAsJsonAsync("api/cestas", request);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        private async Task<Guid> SeedFamiliaAsync()
        {
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<IgrejaSocialDbContext>();

            var familia = new Familia
            {
                Id = Guid.NewGuid(),
                NomeResponsavel = "Família Teste",
                CpfResponsavel = "99999999999",
                DocumentacaoApresentada = true,
                Residencia = TipoMoradia.Alugada,
                Status = StatusAcompanhamento.Ativo,
                Endereco = "Rua Teste, 123",
                RendaFamiliarTotal = 300.00m,
                DataCadastro = DateTime.Today
            };

            context.Familias.Add(familia);
            await context.SaveChangesAsync();

            return familia.Id;
        }

        private static async Task AutenticarAsync(HttpClient client)
        {
            var loginRequest = new LoginRequest
            {
                Email = "admin@igreja.local",
                Password = "Admin@1234"
            };

            var response = await client.PostAsJsonAsync("api/auth/login", loginRequest);
            response.EnsureSuccessStatusCode();
        }
    }
}
