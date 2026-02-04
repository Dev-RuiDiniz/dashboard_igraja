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
    public class EmprestimoTests : IClassFixture<CustomWebApplicationFactory>
    {
        private readonly CustomWebApplicationFactory _factory;

        public EmprestimoTests(CustomWebApplicationFactory factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task Deve_registrar_emprestimo_equipamento()
        {
            var (familiaId, equipamentoId) = await SeedDadosAsync();
            var client = _factory.CreateClient();

            await AutenticarAsync(client);

            var registro = new RegistroAtendimento
            {
                FamiliaId = familiaId,
                EquipamentoId = equipamentoId,
                DataPrevistaDevolucao = DateTime.Today.AddDays(7),
                Observacoes = "Empréstimo de teste"
            };

            var response = await client.PostAsJsonAsync("api/emprestimos", registro);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        }

        private async Task<(Guid familiaId, Guid equipamentoId)> SeedDadosAsync()
        {
            using var scope = _factory.Services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<IgrejaSocialDbContext>();

            var familia = new Familia
            {
                Id = Guid.NewGuid(),
                NomeResponsavel = "Família Empréstimo",
                CpfResponsavel = "88888888888",
                DocumentacaoApresentada = true,
                Residencia = TipoMoradia.Propria,
                Status = StatusAcompanhamento.Ativo,
                Endereco = "Rua Empréstimo, 10",
                RendaFamiliarTotal = 500.00m,
                DataCadastro = DateTime.Today
            };

            var equipamento = new Equipamento
            {
                Id = Guid.NewGuid(),
                CodigoPatrimonio = $"EQ-{Guid.NewGuid():N}".Substring(0, 8),
                Tipo = TipoEquipamento.CadeiraDeRodas,
                Estado = StatusEquipamento.Bom,
                Descricao = "Equipamento teste",
                IsDisponivel = true,
                ValorEstimado = 200.00m,
                DataAquisicao = DateTime.Today
            };

            context.Familias.Add(familia);
            context.Equipamentos.Add(equipamento);
            await context.SaveChangesAsync();

            return (familia.Id, equipamento.Id);
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
