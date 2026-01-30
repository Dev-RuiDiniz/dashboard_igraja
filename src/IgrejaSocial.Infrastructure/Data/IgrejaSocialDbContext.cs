using IgrejaSocial.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace IgrejaSocial.Infrastructure.Data
{
    public class IgrejaSocialDbContext : DbContext
    {
        public IgrejaSocialDbContext(DbContextOptions<IgrejaSocialDbContext> options) : base(options) { }

        public DbSet<Familia> Familias { get; set; }
        public DbSet<MembroFamilia> Membros { get; set; }
        public DbSet<Equipamento> Equipamentos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração: Família
            modelBuilder.Entity<Familia>(entity =>
            {
                entity.HasKey(f => f.Id);
                
                // Índice Único para o CPF do Responsável
                entity.HasIndex(f => f.CpfResponsavel).IsUnique();
                
                entity.Property(f => f.NomeResponsavel).IsRequired().HasMaxLength(150);
                entity.Property(f => f.Endereco).IsRequired().HasMaxLength(255);
                
                // Configuração de Precisão Decimal para Valores Financeiros
                entity.Property(f => f.RendaFamiliarTotal).HasPrecision(18, 2);

                // Relacionamento 1:N (Uma Família tem muitos Membros)
                entity.HasMany(f => f.Membros)
                      .WithOne(m => m.Familia)
                      .HasForeignKey(m => m.FamiliaId)
                      .OnDelete(DeleteBehavior.Cascade); // Se a família for excluída, apaga os membros
            });

            // Configuração: MembroFamilia
            modelBuilder.Entity<MembroFamilia>(entity =>
            {
                entity.HasKey(m => m.Id);
                entity.Property(m => m.Nome).IsRequired().HasMaxLength(150);
                
                // CPF de membro é opcional mas, se houver, deve ser mapeado adequadamente
                entity.Property(m => m.Cpf).HasMaxLength(11);
            });

            // Configuração: Equipamento
            modelBuilder.Entity<Equipamento>(entity =>
            {
                entity.HasKey(e => e.Id);
                
                // Índice Único para o Código de Patrimônio
                entity.HasIndex(e => e.CodigoPatrimonio).IsUnique();
                
                entity.Property(e => e.CodigoPatrimonio).IsRequired().HasMaxLength(20);
                entity.Property(e => e.ValorEstimado).HasPrecision(18, 2);
            });
        }
    }
}