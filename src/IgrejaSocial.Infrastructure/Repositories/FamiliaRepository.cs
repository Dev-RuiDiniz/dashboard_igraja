using IgrejaSocial.Domain.Entities;
using IgrejaSocial.Domain.Interfaces;
using IgrejaSocial.Domain.Enums;
using IgrejaSocial.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IgrejaSocial.Infrastructure.Repositories
{
    public class FamiliaRepository : IFamiliaRepository
    {
        private readonly IgrejaSocialDbContext _context;

        public FamiliaRepository(IgrejaSocialDbContext context)
        {
            _context = context;
        }

        public async Task<Familia?> ObterPorIdAsync(Guid id)
        {
            return await _context.Familias
                .Include(f => f.Membros)
                .FirstOrDefaultAsync(f => f.Id == id);
        }

        public async Task<Familia?> ObterPorCpfAsync(string cpf)
        {
            return await _context.Familias
                .Include(f => f.Membros)
                .FirstOrDefaultAsync(f => f.CpfResponsavel == cpf);
        }

        public async Task<IEnumerable<Familia>> ListarTodasAsync()
        {
            return await _context.Familias
                .Include(f => f.Membros)
                .ToListAsync();
        }

        public async Task<IEnumerable<Familia>> ListarPorBairroAsync(string bairro)
        {
            return await _context.Familias
                .Include(f => f.Membros)
                .Where(f => f.Endereco.Contains(bairro))
                .ToListAsync();
        }

        public async Task<IEnumerable<Familia>> ListarVulneraveisAsync()
        {
            // Busca os dados incluindo membros para que a propriedade computada IsVulneravel funcione
            var familias = await _context.Familias.Include(f => f.Membros).ToListAsync();
            return familias.Where(f => f.IsVulneravel);
        }

        public async Task<IEnumerable<Familia>> ListarRankingVulnerabilidadeAsync(int limite)
        {
            var familias = await _context.Familias.Include(f => f.Membros).ToListAsync();

            return familias
                .OrderBy(f => f.RendaPerCapita)
                .ThenByDescending(f => f.Membros.Count)
                .Take(limite);
        }

        public async Task<bool> JaRecebeuBeneficioNoMesAtualAsync(Guid familiaId)
        {
            var inicioDoMes = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1);
            
            // Lógica de validação temporal para bloqueio na UI (Tarefa 6 do Roadmap)
            return await _context.Familias
                .AnyAsync(f => f.Id == familiaId && 
                               f.Status == StatusAcompanhamento.Ativo && 
                               f.DataCadastro >= inicioDoMes); 
        }

        public async Task AdicionarAsync(Familia familia) => await _context.Familias.AddAsync(familia);

        public void Atualizar(Familia familia) => _context.Familias.Update(familia);

        public async Task SalvarAlteracoesAsync() => await _context.SaveChangesAsync();
    }
}
