using System;
using System.ComponentModel.DataAnnotations;

namespace IgrejaSocial.Domain.Entities
{
    public class RegistroAtendimento
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid FamiliaId { get; set; }

        public Familia? Familia { get; set; }

        [Required]
        public Guid EquipamentoId { get; set; }

        public Equipamento? Equipamento { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DataEmprestimo { get; set; } = DateTime.Now;

        [Required]
        [DataType(DataType.Date)]
        public DateTime DataPrevistaDevolucao { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DataDevolucaoReal { get; set; }

        [StringLength(500)]
        public string Observacoes { get; set; } = string.Empty;
    }
}
