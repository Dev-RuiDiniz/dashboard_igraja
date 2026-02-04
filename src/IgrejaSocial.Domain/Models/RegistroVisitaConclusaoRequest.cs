using System;
using System.ComponentModel.DataAnnotations;

namespace IgrejaSocial.Domain.Models
{
    public class RegistroVisitaConclusaoRequest
    {
        [Required]
        public Guid RegistroId { get; set; }

        [StringLength(500)]
        public string? Observacoes { get; set; }
    }
}
