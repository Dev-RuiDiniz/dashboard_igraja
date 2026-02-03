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
    public class RegistroAtendimentoRepository : IRegistroAtendimentoRepository
    {
        private readonly IgrejaSocialDbContext _context;

        public RegistroAtendimentoRepository(IgrejaSocialDbContext context)
        {
            _context = context;
        }

        public async Task AdicionarAsync(RegistroAtendimento registro) => await _context.RegistrosAtendimento.AddAsync(registro);

        public async Task<RegistroAtendimento?> ObterAtivoPorEquipamentoAsync(Guid equipamentoId)
        {
            return await _context.RegistrosAtendimento
                .Include(r => r.Equipamento)
                .FirstOrDefaultAsync(r => r.EquipamentoId == equipamentoId && r.DataDevolucaoReal == null);
        }

        public async Task<bool> ExisteEmprestimoAtivoPorFamiliaETipoAsync(Guid familiaId, TipoEquipamento tipo)
        {
            return await _context.RegistrosAtendimento
                .Include(r => r.Equipamento)
                .AnyAsync(r => r.FamiliaId == familiaId
                               && r.DataDevolucaoReal == null
                               && r.Equipamento != null
                               && r.Equipamento.Tipo == tipo);
        }

        public async Task<List<RegistroAtendimento>> ListarPorFamiliaAsync(Guid familiaId)
        {
            return await _context.RegistrosAtendimento
                .Include(r => r.Equipamento)
                .Where(r => r.FamiliaId == familiaId)
                .OrderByDescending(r => r.DataEmprestimo)
                .ToListAsync();
        }

        public async Task SalvarAlteracoesAsync() => await _context.SaveChangesAsync();
    }
}
