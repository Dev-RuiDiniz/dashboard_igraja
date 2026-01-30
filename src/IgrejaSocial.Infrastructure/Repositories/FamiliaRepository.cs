using IgrejaSocial.Domain.Entities;
using IgrejaSocial.Domain.Interfaces;
using IgrejaSocial.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

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

        public async Task<IEnumerable<Familia>> ListarPorBairroAsync(string bairro)
        {
            return await _context.Familias
                .Include(f => f.Membros)
                .Where(f => f.Endereco.Contains(bairro))
                .ToListAsync();
        }

        public async Task<IEnumerable<Familia>> ListarVulneraveisAsync()
        {
            var familias = await _context.Familias.Include(f => f.Membros).ToListAsync();
            return familias.Where(f => f.IsVulneravel);
        }

        public async Task AdicionarAsync(Familia familia) => await _context.Familias.AddAsync(familia);
        public void Atualizar(Familia familia) => _context.Familias.Update(familia);
        public async Task SalvarAlteracoesAsync() => await _context.SaveChangesAsync();
    }
}