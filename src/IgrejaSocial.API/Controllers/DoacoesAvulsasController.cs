using IgrejaSocial.Domain.Entities;
using IgrejaSocial.Domain.Identity;
using IgrejaSocial.Domain.Models;
using IgrejaSocial.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IgrejaSocial.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = RoleNames.Administrador + "," + RoleNames.Voluntario)]
    public class DoacoesAvulsasController : ControllerBase
    {
        private readonly IgrejaSocialDbContext _context;

        public DoacoesAvulsasController(IgrejaSocialDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DoacaoAvulsa>>> Listar()
        {
            var doacoes = await _context.DoacoesAvulsas
                .Include(d => d.PessoaEmSituacaoRua)
                .AsNoTracking()
                .OrderByDescending(d => d.DataRegistro)
                .ToListAsync();
            return Ok(doacoes);
        }

        [HttpPost]
        public async Task<ActionResult<DoacaoAvulsa>> Criar(DoacaoAvulsaRequest request)
        {
            var pessoa = await _context.PessoasEmSituacaoRua
                .FirstOrDefaultAsync(p => p.Id == request.PessoaEmSituacaoRuaId);
            if (pessoa is null)
            {
                return NotFound("Pessoa em situação de rua não encontrada.");
            }

            var usuario = User?.Identity?.Name ?? "Sistema";
            var doacao = new DoacaoAvulsa
            {
                PessoaEmSituacaoRuaId = request.PessoaEmSituacaoRuaId,
                Item = request.Item,
                Quantidade = request.Quantidade,
                UnidadeMedida = request.UnidadeMedida,
                Observacoes = request.Observacoes,
                UsuarioRegistro = usuario,
                DataRegistro = DateTime.Now
            };

            _context.DoacoesAvulsas.Add(doacao);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Listar), new { id = doacao.Id }, doacao);
        }
    }
}
