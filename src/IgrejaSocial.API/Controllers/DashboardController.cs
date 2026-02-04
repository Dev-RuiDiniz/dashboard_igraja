using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using IgrejaSocial.Domain.Enums;
using IgrejaSocial.Domain.Identity;
using IgrejaSocial.Domain.Interfaces;
using IgrejaSocial.Domain.Models;
using IgrejaSocial.Infrastructure.Data;

namespace IgrejaSocial.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = RoleNames.Administrador + "," + RoleNames.Voluntario)]
    public class DashboardController : ControllerBase
    {
        private readonly IgrejaSocialDbContext _context;
        private readonly IFamiliaRepository _familiaRepository;

        public DashboardController(IgrejaSocialDbContext context, IFamiliaRepository familiaRepository)
        {
            _context = context;
            _familiaRepository = familiaRepository;
        }

        /// <summary>
        /// Retorna indicadores gerais do dashboard.
        /// </summary>
        [HttpGet("resumo")]
        public async Task<ActionResult<DashboardStatsDto>> GetResumo()
        {
            // 1. Contagem total de famílias
            var totalFamilias = await _context.Familias.CountAsync();

            // 2. Contagem de equipamentos disponíveis
            var equipamentosDisponiveis = await _context.Equipamentos
                .CountAsync(e => e.IsDisponivel);

            var equipamentosEmprestados = await _context.Equipamentos
                .CountAsync(e => !e.IsDisponivel);

            // 3. Contagem de famílias vulneráveis (Renda per capita < 660)
            // Nota: Se você já implementou a lógica de vulnerabilidade no Domain,
            // podemos filtrar aqui.
            var familias = await _context.Familias
                .Include(f => f.Membros)
                .ToListAsync();
            var familiasVulneraveis = familias.Count(f => f.IsVulneravel);

            return Ok(new DashboardStatsDto
            {
                TotalFamilias = totalFamilias,
                EquipamentosDisponiveis = equipamentosDisponiveis,
                EquipamentosEmprestados = equipamentosEmprestados,
                FamiliasVulneraveis = familiasVulneraveis
            });
        }

        /// <summary>
        /// Retorna o ranking das famílias mais vulneráveis.
        /// </summary>
        [HttpGet("ranking-vulnerabilidade")]
        public async Task<ActionResult<IEnumerable<RankingVulnerabilidadeDto>>> GetRankingVulnerabilidade([FromQuery] int limite = 5)
        {
            var familias = await _familiaRepository.ListarRankingVulnerabilidadeAsync(limite);
            var ranking = familias.Select(familia => new RankingVulnerabilidadeDto
            {
                Id = familia.Id,
                NomeResponsavel = familia.NomeResponsavel,
                RendaPerCapita = familia.RendaPerCapita,
                TotalDependentes = familia.Membros.Count
            }).ToList();

            return Ok(ranking);
        }

        /// <summary>
        /// Lista famílias que demandam atenção por atraso em cesta básica.
        /// </summary>
        [HttpGet("atencao")]
        public async Task<ActionResult<IEnumerable<FamiliaAtencaoDto>>> GetFamiliasAtencao()
        {
            var limiteData = DateTime.UtcNow.Date.AddMonths(-2);
            var familias = await _context.Familias.Include(f => f.Membros).ToListAsync();
            var ultimasCestas = await _context.RegistrosAtendimento
                .Where(r => r.TipoAtendimento == TipoAtendimento.CestaBasica && r.DataEntrega.HasValue)
                .GroupBy(r => r.FamiliaId)
                .Select(g => new { FamiliaId = g.Key, UltimaEntrega = g.Max(r => r.DataEntrega) })
                .ToListAsync();

            var ultimaPorFamilia = ultimasCestas
                .ToDictionary(item => item.FamiliaId, item => item.UltimaEntrega);

            var resultado = familias
                .Select(familia =>
                {
                    ultimaPorFamilia.TryGetValue(familia.Id, out var ultimaEntrega);
                    return new FamiliaAtencaoDto
                    {
                        Id = familia.Id,
                        NomeResponsavel = familia.NomeResponsavel,
                        UltimaEntregaCesta = ultimaEntrega,
                        RendaPerCapita = familia.RendaPerCapita,
                        TotalDependentes = familia.Membros.Count
                    };
                })
                .Where(dto => !dto.UltimaEntregaCesta.HasValue || dto.UltimaEntregaCesta.Value.Date < limiteData)
                .OrderBy(dto => dto.UltimaEntregaCesta ?? DateTime.MinValue)
                .ToList();

            return Ok(resultado);
        }

        /// <summary>
        /// Lista famílias com visitas solicitadas ou com atraso de acompanhamento.
        /// </summary>
        [HttpGet("visitas-atrasadas")]
        public async Task<ActionResult<IEnumerable<VisitaAtrasadaDto>>> GetVisitasAtrasadas()
        {
            var limiteDias = 60;
            var limiteData = DateTime.UtcNow.Date.AddDays(-limiteDias);

            var familias = await _context.Familias.AsNoTracking().ToListAsync();
            var visitas = await _context.RegistrosVisitas
                .AsNoTracking()
                .ToListAsync();

            var visitasAbertas = visitas
                .Where(v => !v.DataConclusao.HasValue)
                .GroupBy(v => v.FamiliaId)
                .ToDictionary(g => g.Key, g => g.OrderByDescending(v => v.DataSolicitacao).First());

            var ultimasConclusoes = visitas
                .Where(v => v.DataConclusao.HasValue)
                .GroupBy(v => v.FamiliaId)
                .ToDictionary(g => g.Key, g => g.Max(v => v.DataConclusao));

            var resultado = familias
                .Select(familia =>
                {
                    visitasAbertas.TryGetValue(familia.Id, out var visitaAberta);
                    ultimasConclusoes.TryGetValue(familia.Id, out var ultimaConclusao);
                    var referencia = visitaAberta?.DataSolicitacao ?? ultimaConclusao ?? familia.DataCadastro;
                    var dias = (DateTime.UtcNow.Date - referencia.Date).Days;

                    return new VisitaAtrasadaDto
                    {
                        FamiliaId = familia.Id,
                        NomeResponsavel = familia.NomeResponsavel,
                        DataSolicitacao = visitaAberta?.DataSolicitacao,
                        UltimaVisitaConcluida = ultimaConclusao,
                        DiasEmAberto = dias
                    };
                })
                .Where(dto =>
                    dto.DataSolicitacao.HasValue ||
                    (!dto.UltimaVisitaConcluida.HasValue && dto.DiasEmAberto >= limiteDias) ||
                    (dto.UltimaVisitaConcluida.HasValue && dto.UltimaVisitaConcluida.Value.Date < limiteData))
                .OrderByDescending(dto => dto.DiasEmAberto)
                .ToList();

            return Ok(resultado);
        }

        /// <summary>
        /// Lista tipos de equipamentos com poucas unidades disponíveis.
        /// </summary>
        [HttpGet("estoque-baixo")]
        public async Task<ActionResult<IEnumerable<EquipamentoEstoqueBaixoDto>>> GetEstoqueBaixo([FromQuery] int limite = 2)
        {
            var equipamentos = await _context.Equipamentos.AsNoTracking().ToListAsync();

            var resultado = equipamentos
                .GroupBy(e => e.Tipo)
                .Select(grupo => new EquipamentoEstoqueBaixoDto
                {
                    Tipo = grupo.Key,
                    Disponiveis = grupo.Count(e => e.IsDisponivel),
                    Total = grupo.Count()
                })
                .Where(dto => dto.Disponiveis <= limite)
                .OrderBy(dto => dto.Disponiveis)
                .ToList();

            return Ok(resultado);
        }
    }
}
