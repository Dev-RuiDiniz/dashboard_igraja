using System;
using System.ComponentModel.DataAnnotations;

namespace IgrejaSocial.Domain.Entities
{
    public class RegistroVisita
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid FamiliaId { get; set; }

        public Familia? Familia { get; set; }

        [Required]
        public string Solicitante { get; set; } = string.Empty;

        public string? Executor { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime DataSolicitacao { get; set; } = DateTime.UtcNow;

        [DataType(DataType.DateTime)]
        public DateTime? DataConclusao { get; set; }

        [StringLength(500)]
        public string? Observacoes { get; set; }
    }
}
