using IgrejaSocial.Domain.Entities;
using IgrejaSocial.Domain.Enums;
using IgrejaSocial.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

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

            if (await _registroRepository.ExisteEmprestimoAtivoPorFamiliaETipoAsync(registro.FamiliaId, equipamento.Tipo))
            {
                return BadRequest("A família já possui um empréstimo ativo deste tipo de equipamento.");
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

        [HttpPut("{id:guid}/devolucao")]
        public async Task<ActionResult<RegistroAtendimento>> RegistrarDevolucao(Guid id, DevolucaoRequest request)
        {
            if (request.DataDevolucao.Date < DateTime.Today)
            {
                return BadRequest("A data de devolução não pode ser retroativa.");
            }

            var registro = await _registroRepository.ObterPorIdAsync(id);
            if (registro is null)
            {
                return NotFound();
            }

            if (registro.DataDevolucao.HasValue)
            {
                return BadRequest("Este empréstimo já foi encerrado.");
            }

            var equipamento = await _equipamentoRepository.ObterPorIdAsync(registro.EquipamentoId);
            if (equipamento is null)
            {
                return BadRequest("Equipamento não encontrado.");
            }

            registro.DataDevolucao = request.DataDevolucao;
            registro.EstadoConservacao = request.EstadoConservacao;
            _registroRepository.Atualizar(registro);

            equipamento.IsDisponivel = request.EstadoConservacao != EstadoConservacao.PrecisaReparo;
            equipamento.Estado = request.EstadoConservacao == EstadoConservacao.PrecisaReparo
                ? StatusEquipamento.NecessitaManutencao
                : equipamento.Estado;
            _equipamentoRepository.Atualizar(equipamento);

            await _registroRepository.SalvarAlteracoesAsync();

            return Ok(registro);
        }

        [HttpGet("familia/{familiaId:guid}")]
        public async Task<ActionResult<IEnumerable<HistoricoEmprestimoDto>>> ListarPorFamilia(Guid familiaId)
        {
            var registros = await _registroRepository.ListarPorFamiliaAsync(familiaId);
            var resultado = registros.Select(r => new HistoricoEmprestimoDto
            {
                RegistroId = r.Id,
                CodigoPatrimonio = r.Equipamento?.CodigoPatrimonio ?? string.Empty,
                Tipo = r.Equipamento?.Tipo ?? TipoEquipamento.Outro,
                DataEmprestimo = r.DataEmprestimo,
                DataPrevistaDevolucao = r.DataPrevistaDevolucao,
                DataDevolucao = r.DataDevolucao,
                EstadoConservacao = r.EstadoConservacao
            });

            return Ok(resultado);
        }

        [HttpGet("ativos/equipamento/{equipamentoId:guid}")]
        public async Task<ActionResult<HistoricoEmprestimoDto>> ObterAtivoPorEquipamento(Guid equipamentoId)
        {
            var registro = await _registroRepository.ObterAtivoPorEquipamentoAsync(equipamentoId);
            if (registro is null)
            {
                return NotFound();
            }

            var resultado = new HistoricoEmprestimoDto
            {
                RegistroId = registro.Id,
                CodigoPatrimonio = registro.Equipamento?.CodigoPatrimonio ?? string.Empty,
                Tipo = registro.Equipamento?.Tipo ?? TipoEquipamento.Outro,
                DataEmprestimo = registro.DataEmprestimo,
                DataPrevistaDevolucao = registro.DataPrevistaDevolucao,
                DataDevolucao = registro.DataDevolucao,
                EstadoConservacao = registro.EstadoConservacao
            };

            return Ok(resultado);
        }

        public class DevolucaoRequest
        {
            public DateTime DataDevolucao { get; set; }
            public EstadoConservacao EstadoConservacao { get; set; }
        }

        public class HistoricoEmprestimoDto
        {
            public Guid RegistroId { get; set; }
            public string CodigoPatrimonio { get; set; } = string.Empty;
            public TipoEquipamento Tipo { get; set; }
            public DateTime DataEmprestimo { get; set; }
            public DateTime DataPrevistaDevolucao { get; set; }
            public DateTime? DataDevolucao { get; set; }
            public EstadoConservacao? EstadoConservacao { get; set; }
        }
    }
}
