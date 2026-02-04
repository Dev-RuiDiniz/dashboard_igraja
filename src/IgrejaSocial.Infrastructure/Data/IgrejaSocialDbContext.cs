using IgrejaSocial.Domain.Entities;
using IgrejaSocial.Domain.Enums;
using IgrejaSocial.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace IgrejaSocial.Infrastructure.Data
{
    public class IgrejaSocialDbContext : IdentityDbContext<ApplicationUser>
    {
        public IgrejaSocialDbContext(DbContextOptions<IgrejaSocialDbContext> options) : base(options) { }

        public DbSet<Familia> Familias { get; set; }
        public DbSet<MembroFamilia> Membros { get; set; }
        public DbSet<Equipamento> Equipamentos { get; set; }
        public DbSet<RegistroAtendimento> RegistrosAtendimento { get; set; }
        public DbSet<PessoaEmSituacaoRua> PessoasEmSituacaoRua { get; set; }
        public DbSet<DoacaoAvulsa> DoacoesAvulsas { get; set; }
        public DbSet<RegistroVisita> RegistrosVisitas { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // 1. Configuração: Família
            modelBuilder.Entity<Familia>(entity =>
            {
                entity.HasKey(f => f.Id);
                entity.HasIndex(f => f.CpfResponsavel).IsUnique();
                entity.HasIndex(f => f.RgResponsavel);
                entity.Property(f => f.NomeResponsavel).IsRequired().HasMaxLength(150);
                entity.Property(f => f.Endereco).IsRequired().HasMaxLength(255);
                entity.Property(f => f.RendaFamiliarTotal).HasPrecision(18, 2);
                entity.Property(f => f.Latitude).HasPrecision(9, 6);
                entity.Property(f => f.Longitude).HasPrecision(9, 6);

                entity.HasMany(f => f.Membros)
                      .WithOne(m => m.Familia)
                      .HasForeignKey(m => m.FamiliaId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // 2. Configuração: MembroFamilia
            modelBuilder.Entity<MembroFamilia>(entity =>
            {
                entity.HasKey(m => m.Id);
                entity.Property(m => m.Nome).IsRequired().HasMaxLength(150);
                entity.Property(m => m.Cpf).HasMaxLength(11);
                entity.Property(m => m.RendaIndividual).HasPrecision(18, 2);
                entity.Property(m => m.NumeroCadUnico).HasMaxLength(30);
            });

            // 3. Configuração: Equipamento
            modelBuilder.Entity<Equipamento>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.CodigoPatrimonio).IsUnique();
                entity.Property(e => e.CodigoPatrimonio).IsRequired().HasMaxLength(20);
                entity.Property(e => e.ValorEstimado).HasPrecision(18, 2);
            });

            // 4. Configuração: RegistroAtendimento
            modelBuilder.Entity<RegistroAtendimento>(entity =>
            {
                entity.HasKey(r => r.Id);
                entity.Property(r => r.Observacoes).HasMaxLength(500);
                entity.Property(r => r.UsuarioEntrega).HasMaxLength(150);
                entity.Property(r => r.UsuarioId).HasMaxLength(450);
                entity.Property(r => r.TipoAtendimento).IsRequired();

                entity.HasOne(r => r.Familia)
                    .WithMany()
                    .HasForeignKey(r => r.FamiliaId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(r => r.Equipamento)
                    .WithMany()
                    .HasForeignKey(r => r.EquipamentoId)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // 5. Configuração: Pessoa em Situação de Rua
            modelBuilder.Entity<PessoaEmSituacaoRua>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.HasIndex(p => p.Cpf).IsUnique();
                entity.Property(p => p.Nome).IsRequired().HasMaxLength(150);
                entity.Property(p => p.Rg).HasMaxLength(20);
                entity.Property(p => p.LocalReferencia).HasMaxLength(255);
                entity.Property(p => p.Observacoes).HasMaxLength(500);
            });

            // 6. Configuração: Doações Avulsas
            modelBuilder.Entity<DoacaoAvulsa>(entity =>
            {
                entity.HasKey(d => d.Id);
                entity.Property(d => d.Item).IsRequired().HasMaxLength(150);
                entity.Property(d => d.UnidadeMedida).HasMaxLength(50);
                entity.Property(d => d.UsuarioRegistro).HasMaxLength(150);
                entity.Property(d => d.Observacoes).HasMaxLength(500);
                entity.HasOne(d => d.PessoaEmSituacaoRua)
                    .WithMany()
                    .HasForeignKey(d => d.PessoaEmSituacaoRuaId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // 7. Configuração: Registro de Visitas
            modelBuilder.Entity<RegistroVisita>(entity =>
            {
                entity.HasKey(v => v.Id);
                entity.Property(v => v.Solicitante).IsRequired().HasMaxLength(150);
                entity.Property(v => v.Executor).HasMaxLength(150);
                entity.Property(v => v.Observacoes).HasMaxLength(500);
                entity.HasOne(v => v.Familia)
                    .WithMany()
                    .HasForeignKey(v => v.FamiliaId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // 8. SEED DE DADOS (Tarefa 10)
            SeedDados(modelBuilder);
        }

        private void SeedDados(ModelBuilder modelBuilder)
        {
            // Seed Equipamentos
            modelBuilder.Entity<Equipamento>().HasData(
                new Equipamento
                {
                    Id = Guid.Parse("a1b2c3d4-e5f6-4a5b-8c9d-0e1f2a3b4c5d"),
                    CodigoPatrimonio = "CAD-01",
                    Tipo = TipoEquipamento.CadeiraDeRodas,
                    Estado = StatusEquipamento.Novo,
                    Descricao = "Cadeira de rodas manual dobrável",
                    IsDisponivel = true,
                    ValorEstimado = 850.00m,
                    DataAquisicao = DateTime.Now
                },
                new Equipamento
                {
                    Id = Guid.Parse("b2c3d4e5-f6a7-5b6c-9d0e-1f2a3b4c5d6e"),
                    CodigoPatrimonio = "AND-01",
                    Tipo = TipoEquipamento.Andador,
                    Estado = StatusEquipamento.Bom,
                    Descricao = "Andador de alumínio com rodas",
                    IsDisponivel = true,
                    ValorEstimado = 250.00m,
                    DataAquisicao = DateTime.Now
                }
            );

            // Seed Família Exemplo (Importante para testar Vulnerabilidade)
            var familiaId = Guid.Parse("c3d4e5f6-a7b8-6c7d-0e1f-2a3b4c5d6e7f");
            modelBuilder.Entity<Familia>().HasData(
                new Familia
                {
                    Id = familiaId,
                    NomeResponsavel = "João da Silva (Teste Seed)",
                    CpfResponsavel = "12345678901",
                    DocumentacaoApresentada = true,
                    Residencia = TipoMoradia.Alugada,
                    Status = StatusAcompanhamento.Ativo,
                    Endereco = "Rua das Oliveiras, 50 - Bairro Solidariedade",
                    RendaFamiliarTotal = 400.00m, // Renda baixa para forçar Vulnerabilidade = True
                    DataCadastro = DateTime.Now
                }
            );
        }
    }
}
