using AutoMapper;
using IgrejaSocial.Application.DTOs;
using IgrejaSocial.Domain.Entities;
using IgrejaSocial.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IgrejaSocial.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FamiliasController : ControllerBase
    {
        private readonly IFamiliaRepository _repository;
        private readonly IMapper _mapper;

        public FamiliasController(IFamiliaRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<FamiliaDto>>> GetTodas()
        {
            var familias = await _repository.ListarTodasAsync();
            return Ok(_mapper.Map<IEnumerable<FamiliaDto>>(familias));
        }

        [HttpGet("vulneraveis")]
        public async Task<ActionResult<IEnumerable<FamiliaDto>>> GetVulneraveis()
        {
            var familias = await _repository.ListarVulneraveisAsync();
            return Ok(_mapper.Map<IEnumerable<FamiliaDto>>(familias));
        }

        [HttpPost]
        public async Task<ActionResult<FamiliaDto>> Criar(Familia familia)
        {
            await _repository.AdicionarAsync(familia);
            await _repository.SalvarAlteracoesAsync();

            var familiaDto = _mapper.Map<FamiliaDto>(familia);
            return CreatedAtAction(nameof(GetTodas), new { id = familia.Id }, familiaDto);
        }
    }
}