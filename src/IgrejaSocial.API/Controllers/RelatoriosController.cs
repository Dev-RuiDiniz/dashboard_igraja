using System.Collections.Generic;
using System.Globalization;
using System.Text;
using IgrejaSocial.Domain.Enums;
using IgrejaSocial.Domain.Identity;
using IgrejaSocial.Domain.Models;
using IgrejaSocial.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

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
            var referencia = new DateTime(ano ?? DateTime.UtcNow.Year, mes ?? DateTime.UtcNow.Month, 1, 0, 0, 0, DateTimeKind.Utc);
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
        /// Retorna o acumulado de cestas entregues por mês no ano.
        /// </summary>
        [HttpGet("cestas/anual")]
        public async Task<ActionResult<IEnumerable<CestasAnuaisDto>>> GetCestasAnuais([FromQuery] int? ano)
        {
            var referenciaAno = ano ?? DateTime.UtcNow.Year;
            var inicio = new DateTime(referenciaAno, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var fim = inicio.AddYears(1);

            var totais = await _context.RegistrosAtendimento
                .Where(r => r.TipoAtendimento == TipoAtendimento.CestaBasica
                            && r.DataEntrega.HasValue
                            && r.DataEntrega.Value >= inicio
                            && r.DataEntrega.Value < fim)
                .GroupBy(r => r.DataEntrega!.Value.Month)
                .Select(g => new { Mes = g.Key, Total = g.Count() })
                .ToListAsync();

            var acumulado = 0;
            var resultado = new List<CestasAnuaisDto>();
            for (var mes = 1; mes <= 12; mes++)
            {
                var totalMes = totais.FirstOrDefault(t => t.Mes == mes)?.Total ?? 0;
                acumulado += totalMes;
                resultado.Add(new CestasAnuaisDto
                {
                    Mes = mes,
                    TotalEntregue = totalMes,
                    TotalAcumulado = acumulado
                });
            }

            return Ok(resultado);
        }

        /// <summary>
        /// Exporta em CSV os atendimentos de cesta básica no mês.
        /// </summary>
        [HttpGet("atendimentos-cestas/export")]
        public async Task<IActionResult> ExportarCestasEntregues([FromQuery] int? mes, [FromQuery] int? ano)
        {
            var referencia = new DateTime(ano ?? DateTime.UtcNow.Year, mes ?? DateTime.UtcNow.Month, 1, 0, 0, 0, DateTimeKind.Utc);
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

        /// <summary>
        /// Gera relatório de KPIs em PDF (giro de estoque, impacto social e atendimentos).
        /// </summary>
        [HttpGet("kpis/pdf")]
        public async Task<IActionResult> GerarRelatorioKpisPdf([FromQuery] int meses = 6)
        {
            var totalEquipamentos = await _context.Equipamentos.CountAsync();
            var totalEmprestimosPeriodo = await _context.RegistrosAtendimento
                .CountAsync(r => r.TipoAtendimento == TipoAtendimento.EmprestimoEquipamento
                                 && r.DataEmprestimo >= DateTime.UtcNow.Date.AddMonths(-meses));

            var totalCestasPeriodo = await _context.RegistrosAtendimento
                .CountAsync(r => r.TipoAtendimento == TipoAtendimento.CestaBasica
                                 && r.DataEntrega.HasValue
                                 && r.DataEntrega.Value >= DateTime.UtcNow.Date.AddMonths(-meses));

            var totalVisitasPeriodo = await _context.RegistrosVisitas
                .CountAsync(v => v.DataConclusao.HasValue
                                 && v.DataConclusao.Value >= DateTime.UtcNow.Date.AddMonths(-meses));

            var totalDoacoesPeriodo = await _context.DoacoesAvulsas
                .CountAsync(d => d.DataRegistro >= DateTime.UtcNow.Date.AddMonths(-meses));

            var giro = totalEquipamentos == 0 ? 0 : (decimal)totalEmprestimosPeriodo / totalEquipamentos;
            var emprestimosConcluidos = await _context.RegistrosAtendimento
                .Where(r => r.TipoAtendimento == TipoAtendimento.EmprestimoEquipamento
                            && r.DataDevolucaoReal.HasValue)
                .ToListAsync();

            var tempoMedioDias = emprestimosConcluidos.Count == 0
                ? 0
                : emprestimosConcluidos.Average(r =>
                    (r.DataDevolucaoReal!.Value.Date - r.DataEmprestimo.Date).TotalDays);

            var periodos = new List<KpiAtendimentoPeriodoDto>();
            for (var i = meses - 1; i >= 0; i--)
            {
                var referencia = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1, 0, 0, 0, DateTimeKind.Utc).AddMonths(-i);
                var inicio = referencia;
                var fim = referencia.AddMonths(1);

                var totalPeriodo = await _context.RegistrosAtendimento
                    .CountAsync(r =>
                        ((r.TipoAtendimento == TipoAtendimento.CestaBasica && r.DataEntrega.HasValue && r.DataEntrega.Value >= inicio && r.DataEntrega.Value < fim)
                        || (r.TipoAtendimento == TipoAtendimento.EmprestimoEquipamento && r.DataEmprestimo >= inicio && r.DataEmprestimo < fim)));

                totalPeriodo += await _context.RegistrosVisitas
                    .CountAsync(v => v.DataConclusao.HasValue && v.DataConclusao.Value >= inicio && v.DataConclusao.Value < fim);

                periodos.Add(new KpiAtendimentoPeriodoDto
                {
                    Periodo = referencia.ToString("MMM/yyyy", CultureInfo.GetCultureInfo("pt-BR")),
                    TotalAtendimentos = totalPeriodo
                });
            }

            var kpis = new KpiReportDto
            {
                GiroEstoque = giro,
                ImpactoSocial = totalCestasPeriodo + totalVisitasPeriodo + totalDoacoesPeriodo,
                AtendimentosPorPeriodo = periodos
            };

            var pdf = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(30);
                    page.Header().Row(row =>
                    {
                        row.RelativeItem().Column(header =>
                        {
                            header.Item().Text("Igreja Social").FontSize(16).SemiBold();
                            header.Item().Text("Relatório de KPIs").FontSize(20).SemiBold();
                        });
                        row.ConstantItem(160).AlignRight().Text($"Emitido em {DateTime.UtcNow:dd/MM/yyyy}");
                    });

                    page.Content().Column(column =>
                    {
                        column.Spacing(12);

                        column.Item().Row(row =>
                        {
                            row.RelativeItem().Border(1).BorderColor(Colors.Grey.Lighten2).Padding(12)
                                .Column(item =>
                                {
                                    item.Item().Text("Giro de Estoque").SemiBold();
                                    item.Item().Text(kpis.GiroEstoque.ToString("P1"));
                                });
                            row.RelativeItem().Border(1).BorderColor(Colors.Grey.Lighten2).Padding(12)
                                .Column(item =>
                                {
                                    item.Item().Text("Impacto Social").SemiBold();
                                    item.Item().Text($"{kpis.ImpactoSocial} atendimentos");
                                });
                            row.RelativeItem().Border(1).BorderColor(Colors.Grey.Lighten2).Padding(12)
                                .Column(item =>
                                {
                                    item.Item().Text("Empréstimos no período").SemiBold();
                                    item.Item().Text(totalEmprestimosPeriodo.ToString());
                                });
                            row.RelativeItem().Border(1).BorderColor(Colors.Grey.Lighten2).Padding(12)
                                .Column(item =>
                                {
                                    item.Item().Text("Tempo Médio de Empréstimo").SemiBold();
                                    item.Item().Text($"{tempoMedioDias:F1} dias");
                                });
                        });

                        column.Item().Text("Atendimentos por período").SemiBold().FontSize(14);

                        column.Item().Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.RelativeColumn();
                                columns.ConstantColumn(120);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Element(CellStyle).Text("Período");
                                header.Cell().Element(CellStyle).AlignRight().Text("Total");
                            });

                            foreach (var periodo in kpis.AtendimentosPorPeriodo)
                            {
                                table.Cell().Element(CellStyle).Text(periodo.Periodo);
                                table.Cell().Element(CellStyle).AlignRight().Text(periodo.TotalAtendimentos.ToString());
                            }

                            static IContainer CellStyle(IContainer container)
                            {
                                return container.BorderBottom(1)
                                    .BorderColor(Colors.Grey.Lighten3)
                                    .PaddingVertical(6);
                            }
                        });
                    });

                    page.Footer().AlignRight().Text(text =>
                    {
                        text.Span("Página ");
                        text.CurrentPageNumber();
                        text.Span(" de ");
                        text.TotalPages();
                    });
                });
            }).GeneratePdf();

            return File(pdf, "application/pdf", $"relatorio-kpis-{DateTime.UtcNow:yyyy-MM-dd}.pdf");
        }
    }
}
