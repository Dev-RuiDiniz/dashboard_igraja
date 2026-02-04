using System;

namespace IgrejaSocial.Domain.Models
{
    public class VisitaAtrasadaDto
    {
        public Guid FamiliaId { get; set; }
        public string NomeResponsavel { get; set; } = string.Empty;
        public DateTime? DataSolicitacao { get; set; }
        public DateTime? UltimaVisitaConcluida { get; set; }
        public int DiasEmAberto { get; set; }
    }
}
