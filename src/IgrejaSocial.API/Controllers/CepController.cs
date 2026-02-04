using IgrejaSocial.Domain.Identity;
using IgrejaSocial.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IgrejaSocial.Domain.Models;

namespace IgrejaSocial.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = RoleNames.Administrador + "," + RoleNames.Voluntario)]
    public class CepController : ControllerBase
    {
        private readonly ICepService _cepService;

        public CepController(ICepService cepService)
        {
            _cepService = cepService;
        }

        /// <summary>
        /// Busca endereço pelo CEP informado.
        /// </summary>
        [HttpGet("{cep}")]
        public async Task<IActionResult> GetEndereco(string cep)
        {
            var endereco = await _cepService.BuscarEnderecoPorCepAsync(cep);
            
            if (endereco.ServicoIndisponivel)
            {
                return StatusCode(StatusCodes.Status503ServiceUnavailable, "Serviço de CEP indisponível.");
            }

            if (string.IsNullOrEmpty(endereco.Cep) || endereco.Erro)
                return NotFound("CEP não encontrado.");

            return Ok(endereco);
        }
    }
}
