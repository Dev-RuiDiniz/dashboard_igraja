using System;
using System.ComponentModel.DataAnnotations;

namespace IgrejaSocial.Domain.Models
{
    public class RegistroVisitaRequest
    {
        [Required]
        public Guid FamiliaId { get; set; }

        [StringLength(500)]
        public string? Observacoes { get; set; }
    }
}
