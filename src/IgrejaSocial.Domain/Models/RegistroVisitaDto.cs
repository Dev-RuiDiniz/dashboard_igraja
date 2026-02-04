using System;

namespace IgrejaSocial.Domain.Models
{
    public class RegistroVisitaDto
    {
        public Guid Id { get; set; }
        public Guid FamiliaId { get; set; }
        public string NomeResponsavel { get; set; } = string.Empty;
        public string Solicitante { get; set; } = string.Empty;
        public string? Executor { get; set; }
        public DateTime DataSolicitacao { get; set; }
        public DateTime? DataConclusao { get; set; }
        public string? Observacoes { get; set; }
    }
}
