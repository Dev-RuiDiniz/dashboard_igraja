using System;

namespace IgrejaSocial.Domain.Models
{
    public class FamiliaAtencaoDto
    {
        public Guid Id { get; set; }
        public string NomeResponsavel { get; set; } = string.Empty;
        public DateTime? UltimaEntregaCesta { get; set; }
        public decimal RendaPerCapita { get; set; }
        public int TotalDependentes { get; set; }
    }
}
