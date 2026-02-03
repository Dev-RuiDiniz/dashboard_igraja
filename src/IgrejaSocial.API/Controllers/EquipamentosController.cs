using AutoMapper;
using IgrejaSocial.Application.DTOs;
using IgrejaSocial.Domain.Entities;
using IgrejaSocial.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IgrejaSocial.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EquipamentosController : ControllerBase
    {
        private readonly IEquipamentoRepository _repository;
        private readonly IMapper _mapper;

        public EquipamentosController(IEquipamentoRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EquipamentoDto>>> GetTodos()
        {
            var equipamentos = await _repository.ListarTodosAsync();
            return Ok(_mapper.Map<IEnumerable<EquipamentoDto>>(equipamentos));
        }

        [HttpGet("disponiveis")]
        public async Task<ActionResult<IEnumerable<EquipamentoDto>>> GetDisponiveis()
        {
            var equipamentos = await _repository.ListarDisponiveisAsync();
            return Ok(_mapper.Map<IEnumerable<EquipamentoDto>>(equipamentos));
        }

        [HttpPost]
        public async Task<ActionResult<EquipamentoDto>> Criar(Equipamento equipamento)
        {
            // Aqui futuramente chamaremos o PatrimonioService para gerar o código
            await _repository.AdicionarAsync(equipamento);
            await _repository.SalvarAlteracoesAsync();

            var equipamentoDto = _mapper.Map<EquipamentoDto>(equipamento);
            return CreatedAtAction(nameof(GetPorId), new { id = equipamento.Id }, equipamentoDto);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<EquipamentoDto>> GetPorId(Guid id)
        {
            var equipamento = await _repository.ObterPorIdAsync(id);
            if (equipamento is null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<EquipamentoDto>(equipamento));
        }
    }
}
