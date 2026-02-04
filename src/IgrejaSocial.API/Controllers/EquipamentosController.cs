using AutoMapper;
using IgrejaSocial.Application.DTOs;
using IgrejaSocial.Application.Services;
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
    public class EquipamentosController : ControllerBase
    {
        private readonly IEquipamentoRepository _repository;
        private readonly EquipamentoService _equipamentoService;
        private readonly IMapper _mapper;

        public EquipamentosController(IEquipamentoRepository repository, EquipamentoService equipamentoService, IMapper mapper)
        {
            _repository = repository;
            _equipamentoService = equipamentoService;
            _mapper = mapper;
        }

        /// <summary>
        /// Lista todos os equipamentos cadastrados.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EquipamentoDto>>> GetTodos()
        {
            var equipamentos = await _repository.ListarTodosAsync();
            return Ok(_mapper.Map<IEnumerable<EquipamentoDto>>(equipamentos));
        }

        /// <summary>
        /// Lista apenas equipamentos disponíveis para empréstimo.
        /// </summary>
        [HttpGet("disponiveis")]
        public async Task<ActionResult<IEnumerable<EquipamentoDto>>> GetDisponiveis()
        {
            var equipamentos = await _repository.ListarDisponiveisAsync();
            return Ok(_mapper.Map<IEnumerable<EquipamentoDto>>(equipamentos));
        }

        /// <summary>
        /// Cria um novo equipamento (somente administradores).
        /// </summary>
        [HttpPost]
        [Authorize(Roles = RoleNames.Administrador)]
        public async Task<ActionResult<EquipamentoDto>> Criar(Equipamento equipamento)
        {
            await _equipamentoService.CriarAsync(equipamento);

            var equipamentoDto = _mapper.Map<EquipamentoDto>(equipamento);
            return CreatedAtAction(nameof(GetPorId), new { id = equipamento.Id }, equipamentoDto);
        }

        /// <summary>
        /// Retorna um equipamento pelo identificador.
        /// </summary>
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
