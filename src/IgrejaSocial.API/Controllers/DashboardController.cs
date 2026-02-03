using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IgrejaSocial.Infrastructure.Data;
using IgrejaSocial.Domain.Models;

namespace IgrejaSocial.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DashboardController : ControllerBase
    {
        private readonly IgrejaSocialDbContext _context;

        public DashboardController(IgrejaSocialDbContext context)
        {
            _context = context;
        }

        [HttpGet("resumo")]
        public async Task<ActionResult<DashboardStatsDto>> GetResumo()
        {
            // 1. Contagem total de famílias
            var totalFamilias = await _context.Familias.CountAsync();

            // 2. Contagem de equipamentos disponíveis
            var equipamentosDisponiveis = await _context.Equipamentos
                .CountAsync(e => e.IsDisponivel);

            // 2.1 Contagem de equipamentos em uso
            var equipamentosEmUso = await _context.Equipamentos
                .CountAsync(e => !e.IsDisponivel);

            // 3. Contagem de famílias vulneráveis (Renda per capita < 660)
            // Nota: Se você já implementou a lógica de vulnerabilidade no Domain,
            // podemos filtrar aqui.
            var familiasVulneraveis = await _context.Familias
                .CountAsync(f => f.IsVulneravel);

            return Ok(new DashboardStatsDto
            {
                TotalFamilias = totalFamilias,
                EquipamentosDisponiveis = equipamentosDisponiveis,
                EquipamentosEmUso = equipamentosEmUso,
                FamiliasVulneraveis = familiasVulneraveis
            });
        }
    }
}
