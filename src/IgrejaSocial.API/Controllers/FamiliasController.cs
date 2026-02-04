using AutoMapper;
using IgrejaSocial.Application.DTOs;
using IgrejaSocial.Domain.Entities;
using IgrejaSocial.Domain.Identity;
using IgrejaSocial.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IgrejaSocial.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = RoleNames.Administrador + "," + RoleNames.Voluntario)]
    public class FamiliasController : ControllerBase
    {
        private readonly IFamiliaRepository _repository;
        private readonly IMapper _mapper;

        public FamiliasController(IFamiliaRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
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
    }
}
