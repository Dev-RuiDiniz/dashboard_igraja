using IgrejaSocial.Domain.Entities;
using IgrejaSocial.Domain.Enums;
using IgrejaSocial.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;

namespace IgrejaSocial.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmprestimosController : ControllerBase
    {
        private readonly IEquipamentoRepository _equipamentoRepository;
        private readonly IRegistroAtendimentoRepository _registroRepository;

        public EmprestimosController(
            IEquipamentoRepository equipamentoRepository,
            IRegistroAtendimentoRepository registroRepository)
        {
            _equipamentoRepository = equipamentoRepository;
            _registroRepository = registroRepository;
        }

        [HttpPost]
        public async Task<ActionResult<RegistroAtendimento>> Criar(RegistroAtendimento registro)
        {
            if (registro.DataPrevistaDevolucao.Date < DateTime.Today)
            {
                return BadRequest("A data de previsão não pode ser retroativa.");
            }

            var equipamento = await _equipamentoRepository.ObterPorIdAsync(registro.EquipamentoId);
            if (equipamento is null)
            {
                return BadRequest("Equipamento não encontrado.");
            }

            if (!equipamento.IsDisponivel || equipamento.Estado == StatusEquipamento.NecessitaManutencao)
            {
                return BadRequest("Equipamento indisponível para empréstimo.");
            }

            equipamento.IsDisponivel = false;
            _equipamentoRepository.Atualizar(equipamento);

            registro.DataEmprestimo = DateTime.Now;
            await _registroRepository.AdicionarAsync(registro);
            await _registroRepository.SalvarAlteracoesAsync();

            return CreatedAtAction(nameof(Criar), registro);
        }
    }
}
