using IgrejaSocial.Domain.Entities;
using IgrejaSocial.Domain.Identity;
using IgrejaSocial.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IgrejaSocial.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = RoleNames.Administrador + "," + RoleNames.Voluntario)]
    public class PessoasSituacaoRuaController : ControllerBase
    {
        private readonly IgrejaSocialDbContext _context;

        public PessoasSituacaoRuaController(IgrejaSocialDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PessoaEmSituacaoRua>>> GetTodas()
        {
            var pessoas = await _context.PessoasEmSituacaoRua
                .AsNoTracking()
                .OrderBy(p => p.Nome)
                .ToListAsync();
            return Ok(pessoas);
        }

        [HttpPost]
        public async Task<ActionResult<PessoaEmSituacaoRua>> Criar(PessoaEmSituacaoRua pessoa)
        {
            _context.PessoasEmSituacaoRua.Add(pessoa);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetTodas), new { id = pessoa.Id }, pessoa);
        }
    }
}
