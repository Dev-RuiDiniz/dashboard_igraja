using System;
using System.ComponentModel.DataAnnotations;
using IgrejaSocial.Domain.Enums;

namespace IgrejaSocial.Domain.Entities
{
    public class RegistroAtendimento
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid FamiliaId { get; set; }

        public Familia? Familia { get; set; }

        public Guid? EquipamentoId { get; set; }

        public Equipamento? Equipamento { get; set; }

        [Required]
        public TipoAtendimento TipoAtendimento { get; set; } = TipoAtendimento.EmprestimoEquipamento;

        [DataType(DataType.Date)]
        public DateTime? DataEntrega { get; set; }

        [StringLength(150)]
        public string? UsuarioEntrega { get; set; }

        [StringLength(450)]
        public string? UsuarioId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DataEmprestimo { get; set; } = DateTime.Now;

        [DataType(DataType.Date)]
        public DateTime? DataPrevistaDevolucao { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DataDevolucaoReal { get; set; }

        [StringLength(500)]
        public string Observacoes { get; set; } = string.Empty;
    }
}
