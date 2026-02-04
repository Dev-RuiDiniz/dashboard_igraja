using System.Globalization;
using System.Text;
using IgrejaSocial.Domain.Enums;
using IgrejaSocial.Domain.Identity;
using IgrejaSocial.Domain.Models;
using IgrejaSocial.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IgrejaSocial.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = RoleNames.Administrador)]
    public class RelatoriosController : ControllerBase
    {
        private readonly IgrejaSocialDbContext _context;

        public RelatoriosController(IgrejaSocialDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retorna o consolidado mensal de entregas e empréstimos.
        /// </summary>
        [HttpGet("mensal")]
        public async Task<ActionResult<RelatorioMensalDto>> GetRelatorioMensal([FromQuery] int? mes, [FromQuery] int? ano)
        {
            var referencia = new DateTime(ano ?? DateTime.Today.Year, mes ?? DateTime.Today.Month, 1);
            var inicio = referencia.Date;
            var fim = inicio.AddMonths(1);

            var totalCestas = await _context.RegistrosAtendimento
                .CountAsync(r => r.TipoAtendimento == TipoAtendimento.CestaBasica
                                 && r.DataEntrega.HasValue
                                 && r.DataEntrega.Value >= inicio
                                 && r.DataEntrega.Value < fim);

            var totalEquipamentos = await _context.RegistrosAtendimento
                .CountAsync(r => r.TipoAtendimento == TipoAtendimento.EmprestimoEquipamento
                                 && r.DataEmprestimo >= inicio
                                 && r.DataEmprestimo < fim);

            return Ok(new RelatorioMensalDto
            {
                Mes = referencia.Month,
                Ano = referencia.Year,
                TotalCestasEntregues = totalCestas,
                TotalEquipamentosEmprestados = totalEquipamentos
            });
        }

        /// <summary>
        /// Exporta em CSV os atendimentos de cesta básica no mês.
        /// </summary>
        [HttpGet("atendimentos-cestas/export")]
        public async Task<IActionResult> ExportarCestasEntregues([FromQuery] int? mes, [FromQuery] int? ano)
        {
            var referencia = new DateTime(ano ?? DateTime.Today.Year, mes ?? DateTime.Today.Month, 1);
            var inicio = referencia.Date;
            var fim = inicio.AddMonths(1);

            var registros = await _context.RegistrosAtendimento
                .Include(r => r.Familia)
                .ThenInclude(f => f.Membros)
                .Where(r => r.TipoAtendimento == TipoAtendimento.CestaBasica
                            && r.DataEntrega.HasValue
                            && r.DataEntrega.Value >= inicio
                            && r.DataEntrega.Value < fim)
                .OrderBy(r => r.DataEntrega)
                .ToListAsync();

            var csv = new StringBuilder();
            csv.AppendLine("DataEntrega;Responsavel;CPF;Endereco;RendaPerCapita;Dependentes;Observacoes");

            foreach (var registro in registros)
            {
                var familia = registro.Familia;
                var renda = familia?.RendaPerCapita.ToString("C", CultureInfo.GetCultureInfo("pt-BR")) ?? "R$ 0,00";
                var dependentes = familia?.Membros.Count ?? 0;
                csv.AppendLine(
                    $"{registro.DataEntrega:dd/MM/yyyy};{familia?.NomeResponsavel};{familia?.CpfResponsavel};{familia?.Endereco};{renda};{dependentes};{registro.Observacoes}");
            }

            var bytes = Encoding.UTF8.GetBytes(csv.ToString());
            var fileName = $"familias-atendidas-{referencia:MM-yyyy}.csv";
            return File(bytes, "text/csv", fileName);
        }
    }
}
