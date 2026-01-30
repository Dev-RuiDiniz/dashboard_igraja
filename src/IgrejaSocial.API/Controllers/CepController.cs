using IgrejaSocial.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IgrejaSocial.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CepController : ControllerBase
    {
        private readonly ICepService _cepService;

        public CepController(ICepService cepService)
        {
            _cepService = cepService;
        }

        [HttpGet("{cep}")]
        public async Task<IActionResult> GetEndereco(string cep)
        {
            var endereco = await _cepService.BuscarEnderecoPorCepAsync(cep);
            
            if (endereco == null || string.IsNullOrEmpty(endereco.Cep))
                return NotFound("CEP não encontrado.");

            return Ok(endereco);
        }
    }
}