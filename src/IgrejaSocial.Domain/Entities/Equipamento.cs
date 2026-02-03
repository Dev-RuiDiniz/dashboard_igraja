using System;
using System.ComponentModel.DataAnnotations;
using IgrejaSocial.Domain.Enums; //

namespace IgrejaSocial.Domain.Entities
{
    public class Equipamento
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid(); //

        [Required(ErrorMessage = "O código de património é obrigatório.")]
        [StringLength(20)]
        public string CodigoPatrimonio { get; set; } = string.Empty; //

        [Required]
        public TipoEquipamento Tipo { get; set; } //

        [Required]
        public StatusEquipamento Estado { get; set; } //

        [Required(ErrorMessage = "A descrição é necessária para identificação.")]
        [StringLength(200)]
        public string Descricao { get; set; } = string.Empty; //

        public bool IsDisponivel { get; set; } = true; //

        [DataType(DataType.Date)]
        public DateTime DataAquisicao { get; set; } = DateTime.Now; //

        public decimal ValorEstimado { get; set; } //

        public string ObservacoesInternas { get; set; } = string.Empty; //
    }
}
