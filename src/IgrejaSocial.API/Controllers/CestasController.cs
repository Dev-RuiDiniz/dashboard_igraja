using IgrejaSocial.Domain.Entities;
using IgrejaSocial.Domain.Enums;
using IgrejaSocial.Domain.Identity;
using IgrejaSocial.Domain.Interfaces;
using IgrejaSocial.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IgrejaSocial.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = RoleNames.Administrador + "," + RoleNames.Voluntario)]
    public class CestasController : ControllerBase
    {
        private readonly IFamiliaRepository _familiaRepository;
        private readonly IRegistroAtendimentoRepository _registroRepository;

        public CestasController(
            IFamiliaRepository familiaRepository,
            IRegistroAtendimentoRepository registroRepository)
        {
            _familiaRepository = familiaRepository;
            _registroRepository = registroRepository;
        }

        /// <summary>
        /// Registra a entrega de cesta básica para uma família.
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<RegistroAtendimento>> RegistrarEntrega(CestaBasicaEntregaRequest request)
        {
            var familia = await _familiaRepository.ObterPorIdAsync(request.FamiliaId);
            if (familia is null)
            {
                return NotFound("Família não encontrada.");
            }

            var dataEntrega = request.DataEntrega.Date;
            var jaRecebeu = await _registroRepository.ExisteCestaBasicaNoMesAsync(
                request.FamiliaId,
                dataEntrega.Month,
                dataEntrega.Year);

            if (jaRecebeu)
            {
                return BadRequest("A família já recebeu cesta básica neste mês.");
            }

            var registro = new RegistroAtendimento
            {
                FamiliaId = request.FamiliaId,
                TipoAtendimento = TipoAtendimento.CestaBasica,
                DataEntrega = dataEntrega,
                DataEmprestimo = dataEntrega,
                Observacoes = request.Observacoes ?? string.Empty
            };

            await _registroRepository.AdicionarAsync(registro);
            await _registroRepository.SalvarAlteracoesAsync();

            return CreatedAtAction(nameof(RegistrarEntrega), registro);
        }
    }
}
