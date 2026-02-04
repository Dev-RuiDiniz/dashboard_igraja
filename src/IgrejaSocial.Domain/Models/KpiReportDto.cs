using System.Collections.Generic;

namespace IgrejaSocial.Domain.Models
{
    public class KpiReportDto
    {
        public decimal GiroEstoque { get; set; }
        public int ImpactoSocial { get; set; }
        public List<KpiAtendimentoPeriodoDto> AtendimentosPorPeriodo { get; set; } = new();
    }

    public class KpiAtendimentoPeriodoDto
    {
        public string Periodo { get; set; } = string.Empty;
        public int TotalAtendimentos { get; set; }
    }
}
