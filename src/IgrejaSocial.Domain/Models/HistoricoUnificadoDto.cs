using System;

namespace IgrejaSocial.Domain.Models
{
    public class HistoricoUnificadoDto
    {
        public Guid Id { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public DateTime Data { get; set; }
        public string Descricao { get; set; } = string.Empty;
    }
}
