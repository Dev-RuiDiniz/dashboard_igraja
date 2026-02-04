using IgrejaSocial.Domain.Entities;
using IgrejaSocial.Domain.Enums;
using IgrejaSocial.Domain.Identity;
using IgrejaSocial.Domain.Interfaces;
using IgrejaSocial.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IgrejaSocial.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = RoleNames.Administrador + "," + RoleNames.Voluntario)]
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

        /// <summary>
        /// Registra um empréstimo de equipamento.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<RegistroAtendimento>> Criar(RegistroAtendimento registro)
        {
            if (!registro.EquipamentoId.HasValue || registro.EquipamentoId == Guid.Empty)
            {
                return BadRequest("Equipamento não informado para empréstimo.");
            }

            if (!registro.DataPrevistaDevolucao.HasValue)
            {
                return BadRequest("A data de previsão de devolução é obrigatória.");
            }

            if (registro.DataPrevistaDevolucao.Value.Date < DateTime.Today)
            {
                return BadRequest("A data de previsão não pode ser retroativa.");
            }

            var equipamento = await _equipamentoRepository.ObterPorIdAsync(registro.EquipamentoId.Value);
            if (equipamento is null)
            {
                return BadRequest("Equipamento não encontrado.");
            }

            if (!equipamento.IsDisponivel || equipamento.Estado == StatusEquipamento.NecessitaManutencao)
            {
                return BadRequest("Equipamento indisponível para empréstimo.");
            }

            var possuiEmprestimoAtivo = await _registroRepository
                .ExisteEmprestimoAtivoPorFamiliaETipoAsync(registro.FamiliaId, equipamento.Tipo);
            if (possuiEmprestimoAtivo)
            {
                return BadRequest("A família já possui um empréstimo ativo deste tipo de equipamento.");
            }

            equipamento.IsDisponivel = false;
            _equipamentoRepository.Atualizar(equipamento);

            registro.TipoAtendimento = TipoAtendimento.EmprestimoEquipamento;
            registro.DataEmprestimo = DateTime.Now;
            await _registroRepository.AdicionarAsync(registro);
            await _registroRepository.SalvarAlteracoesAsync();

            return CreatedAtAction(nameof(Criar), registro);
        }

        /// <summary>
        /// Registra a devolução de um equipamento emprestado.
        /// </summary>
        [HttpPost("devolucao")]
        public async Task<ActionResult> RegistrarDevolucao(DevolucaoEquipamentoRequest request)
        {
            if (request.DataDevolucaoReal.Date > DateTime.Today)
            {
                return BadRequest("A data de devolução não pode ser futura.");
            }

            var registro = await _registroRepository.ObterAtivoPorEquipamentoAsync(request.EquipamentoId);
            if (registro is null)
            {
                return BadRequest("Nenhum empréstimo ativo encontrado para este equipamento.");
            }

            var equipamento = await _equipamentoRepository.ObterPorIdAsync(request.EquipamentoId);
            if (equipamento is null)
            {
                return BadRequest("Equipamento não encontrado.");
            }

            registro.DataDevolucaoReal = request.DataDevolucaoReal.Date;

            equipamento.Estado = request.EstadoConservacao;
            equipamento.IsDisponivel = request.EstadoConservacao != StatusEquipamento.NecessitaManutencao;

            _equipamentoRepository.Atualizar(equipamento);
            await _registroRepository.SalvarAlteracoesAsync();

            return Ok();
        }

        /// <summary>
        /// Lista o histórico de empréstimos de uma família.
        /// </summary>
        [HttpGet("familia/{familiaId:guid}")]
        public async Task<ActionResult<IEnumerable<RegistroAtendimentoHistoricoDto>>> ListarPorFamilia(Guid familiaId)
        {
            var registros = await _registroRepository.ListarPorFamiliaAsync(familiaId);
            var historico = registros.Select(registro => new RegistroAtendimentoHistoricoDto
            {
                Id = registro.Id,
                EquipamentoId = registro.EquipamentoId ?? Guid.Empty,
                CodigoPatrimonio = registro.Equipamento?.CodigoPatrimonio ?? string.Empty,
                DescricaoEquipamento = registro.Equipamento?.Descricao ?? string.Empty,
                TipoEquipamento = registro.Equipamento?.Tipo ?? TipoEquipamento.Outro,
                DataEmprestimo = registro.DataEmprestimo,
                DataPrevistaDevolucao = registro.DataPrevistaDevolucao.GetValueOrDefault(),
                DataDevolucaoReal = registro.DataDevolucaoReal,
                EstadoConservacao = registro.Equipamento?.Estado ?? StatusEquipamento.Bom
            }).ToList();

            return Ok(historico);
        }
    }
}
