namespace IgrejaSocial.Application.DTOs
{
    public class FamiliaDto
    {
        public Guid Id { get; set; }
        public string NomeResponsavel { get; set; }
        public string CpfResponsavel { get; set; }
        public string? RgResponsavel { get; set; }
        public string Endereco { get; set; }
        public bool DocumentacaoApresentada { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public int? IdadeResponsavel { get; set; }
        public decimal RendaPerCapita { get; set; } // Valor calculado
        public bool IsVulneravel { get; set; }      // Valor calculado
        public int TotalIntegrantes { get; set; }
        public int TotalCriancasDependentes { get; set; }
        public int TotalAdultosDependentes { get; set; }
    }
}
