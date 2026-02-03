using IgrejaSocial.Domain.Entities;
using IgrejaSocial.Domain.Enums;
using IgrejaSocial.Domain.Interfaces;
using IgrejaSocial.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IgrejaSocial.Infrastructure.Repositories
{
    public class EquipamentoRepository : IEquipamentoRepository
    {
        private readonly IgrejaSocialDbContext _context;

        public EquipamentoRepository(IgrejaSocialDbContext context)
        {
            _context = context;
        }

        public async Task<Equipamento?> ObterPorIdAsync(Guid id) => await _context.Equipamentos.FindAsync(id);

        public async Task<Equipamento?> ObterPorCodigoAsync(string codigo)
        {
            return await _context.Equipamentos
                .FirstOrDefaultAsync(e => e.CodigoPatrimonio == codigo);
        }

        public async Task<IEnumerable<Equipamento>> ListarTodosAsync() => await _context.Equipamentos.ToListAsync();

        public async Task<IEnumerable<Equipamento>> ListarDisponiveisAsync()
        {
            return await _context.Equipamentos
                .Where(e => e.IsDisponivel)
                .ToListAsync();
        }

        public async Task<IEnumerable<Equipamento>> ListarPorStatusAsync(bool disponivel)
        {
            return await _context.Equipamentos
                .Where(e => e.IsDisponivel == disponivel)
                .ToListAsync();
        }

        public async Task<IEnumerable<Equipamento>> ListarPorTipoEEstadoAsync(TipoEquipamento tipo, StatusEquipamento estado)
        {
            return await _context.Equipamentos
                .Where(e => e.Tipo == tipo && e.Estado == estado)
                .ToListAsync();
        }

        public async Task AdicionarAsync(Equipamento equipamento) => await _context.Equipamentos.AddAsync(equipamento);

        public void Atualizar(Equipamento equipamento) => _context.Equipamentos.Update(equipamento);

        public async Task SalvarAlteracoesAsync() => await _context.SaveChangesAsync();
    }
}
