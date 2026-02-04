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
    public class VisitasController : ControllerBase
    {
        private readonly IgrejaSocialDbContext _context;

        public VisitasController(IgrejaSocialDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RegistroVisitaDto>>> Listar()
        {
            var visitas = await _context.RegistrosVisitas
                .Include(v => v.Familia)
                .AsNoTracking()
                .OrderByDescending(v => v.DataSolicitacao)
                .Select(v => new RegistroVisitaDto
                {
                    Id = v.Id,
                    FamiliaId = v.FamiliaId,
                    NomeResponsavel = v.Familia != null ? v.Familia.NomeResponsavel : string.Empty,
                    Solicitante = v.Solicitante,
                    Executor = v.Executor,
                    DataSolicitacao = v.DataSolicitacao,
                    DataConclusao = v.DataConclusao,
                    Observacoes = v.Observacoes
                })
                .ToListAsync();

            return Ok(visitas);
        }

        [HttpPost]
        public async Task<ActionResult<RegistroVisita>> Solicitar(RegistroVisitaRequest request)
        {
            var familia = await _context.Familias.FindAsync(request.FamiliaId);
            if (familia is null)
            {
                return NotFound("Família não encontrada.");
            }

            var solicitante = User?.Identity?.Name ?? "Sistema";
            var visita = new RegistroVisita
            {
                FamiliaId = request.FamiliaId,
                Solicitante = solicitante,
                Observacoes = request.Observacoes,
                DataSolicitacao = DateTime.UtcNow
            };

            _context.RegistrosVisitas.Add(visita);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Listar), new { id = visita.Id }, visita);
        }

        [HttpPost("concluir")]
        public async Task<ActionResult> Concluir(RegistroVisitaConclusaoRequest request)
        {
            var visita = await _context.RegistrosVisitas
                .Include(v => v.Familia)
                .FirstOrDefaultAsync(v => v.Id == request.RegistroId);
            if (visita is null)
            {
                return NotFound("Registro de visita não encontrado.");
            }

            if (visita.DataConclusao.HasValue)
            {
                return BadRequest("Visita já concluída.");
            }

            visita.DataConclusao = DateTime.UtcNow;
            visita.Executor = User?.Identity?.Name ?? "Sistema";
            if (!string.IsNullOrWhiteSpace(request.Observacoes))
            {
                visita.Observacoes = request.Observacoes;
            }

            await _context.SaveChangesAsync();
            return Ok();
        }
    }
}
