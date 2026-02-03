using IgrejaSocial.Domain.Entities;
using IgrejaSocial.Domain.Interfaces;
using IgrejaSocial.Infrastructure.Data;
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

        public async Task SalvarAlteracoesAsync() => await _context.SaveChangesAsync();
    }
}
