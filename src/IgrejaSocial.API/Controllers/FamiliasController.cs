using AutoMapper;
using IgrejaSocial.Application.DTOs;
using IgrejaSocial.Domain.Entities;
using IgrejaSocial.Domain.Enums;
using IgrejaSocial.Domain.Identity;
using IgrejaSocial.Domain.Interfaces;
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
    public class FamiliasController : ControllerBase
    {
        private readonly IFamiliaRepository _repository;
        private readonly IMapper _mapper;
        private readonly IgrejaSocialDbContext _context;

        public FamiliasController(
            IFamiliaRepository repository,
            IMapper mapper,
            IgrejaSocialDbContext context)
        {
            _repository = repository;
            _mapper = mapper;
            _context = context;
        }

        /// <summary>
        /// Lista todas as famílias cadastradas.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FamiliaDto>>> GetTodas()
        {
            var familias = await _repository.ListarTodasAsync();
            return Ok(_mapper.Map<IEnumerable<FamiliaDto>>(familias));
        }

        /// <summary>
        /// Lista as famílias em condição de vulnerabilidade.
        /// </summary>
        [HttpGet("vulneraveis")]
        public async Task<ActionResult<IEnumerable<FamiliaDto>>> GetVulneraveis()
        {
            var familias = await _repository.ListarVulneraveisAsync();
            return Ok(_mapper.Map<IEnumerable<FamiliaDto>>(familias));
        }

        /// <summary>
        /// Cadastra uma nova família.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<FamiliaDto>> Criar(Familia familia)
        {
            await _repository.AdicionarAsync(familia);
            await _repository.SalvarAlteracoesAsync();

            var familiaDto = _mapper.Map<FamiliaDto>(familia);
            return CreatedAtAction(nameof(GetPorId), new { id = familia.Id }, familiaDto);
        }

        /// <summary>
        /// Retorna os detalhes de uma família.
        /// </summary>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<FamiliaDto>> GetPorId(Guid id)
        {
            var familia = await _repository.ObterPorIdAsync(id);
            if (familia is null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<FamiliaDto>(familia));
        }

        /// <summary>
        /// Retorna a linha do tempo unificada (cestas, empréstimos e visitas).
        /// </summary>
        [HttpGet("{id:guid}/historico-unificado")]
        public async Task<ActionResult<IEnumerable<HistoricoUnificadoDto>>> GetHistoricoUnificado(Guid id)
        {
            var registros = await _context.RegistrosAtendimento
                .Include(r => r.Equipamento)
                .Where(r => r.FamiliaId == id)
                .ToListAsync();

            var visitas = await _context.RegistrosVisitas
                .Where(v => v.FamiliaId == id)
                .ToListAsync();

            var historico = new List<HistoricoUnificadoDto>();

            historico.AddRange(registros.Select(registro => new HistoricoUnificadoDto
            {
                Id = registro.Id,
                Tipo = registro.TipoAtendimento == TipoAtendimento.CestaBasica ? "Cesta básica" : "Empréstimo",
                Data = registro.TipoAtendimento == TipoAtendimento.CestaBasica
                    ? registro.DataEntrega ?? registro.DataEmprestimo
                    : registro.DataEmprestimo,
                Descricao = registro.TipoAtendimento == TipoAtendimento.CestaBasica
                    ? $"Entrega registrada por {registro.UsuarioEntrega ?? "Sistema"}"
                    : $"Equipamento: {registro.Equipamento?.Descricao ?? "Não informado"}"
            }));

            historico.AddRange(visitas.Select(visita => new HistoricoUnificadoDto
            {
                Id = visita.Id,
                Tipo = "Visita domiciliar",
                Data = visita.DataConclusao ?? visita.DataSolicitacao,
                Descricao = visita.DataConclusao.HasValue
                    ? $"Concluída por {visita.Executor ?? "Sistema"}"
                    : $"Solicitada por {visita.Solicitante}"
            }));

            return Ok(historico.OrderByDescending(h => h.Data));
        }
    }
}
