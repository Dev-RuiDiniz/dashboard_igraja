using System;
using System.ComponentModel.DataAnnotations;

namespace IgrejaSocial.Domain.Models
{
    public class CestaBasicaEntregaRequest
    {
        [Required]
        public Guid FamiliaId { get; set; }

        [Required]
        public DateTime DataEntrega { get; set; } = DateTime.Today;

        public string? Observacoes { get; set; }
    }
}
