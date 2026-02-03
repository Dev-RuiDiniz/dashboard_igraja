using System;

namespace IgrejaSocial.Domain.Models
{
    public class RankingVulnerabilidadeDto
    {
        public Guid Id { get; set; }
        public string NomeResponsavel { get; set; } = string.Empty;
        public decimal RendaPerCapita { get; set; }
        public int TotalDependentes { get; set; }
    }
}
