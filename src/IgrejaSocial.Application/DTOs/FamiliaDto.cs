namespace IgrejaSocial.Application.DTOs
{
    public class FamiliaDto
    {
        public Guid Id { get; set; }
        public string NomeResponsavel { get; set; }
        public string CpfResponsavel { get; set; }
        public string Endereco { get; set; }
        public decimal RendaPerCapita { get; set; } // Valor calculado
        public bool IsVulneravel { get; set; }      // Valor calculado
        public int TotalIntegrantes { get; set; }
    }
}