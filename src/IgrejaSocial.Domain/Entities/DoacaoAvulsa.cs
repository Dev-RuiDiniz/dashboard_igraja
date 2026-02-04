using System;
using System.ComponentModel.DataAnnotations;

namespace IgrejaSocial.Domain.Entities
{
    public class DoacaoAvulsa
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid PessoaEmSituacaoRuaId { get; set; }

        public PessoaEmSituacaoRua? PessoaEmSituacaoRua { get; set; }

        [Required]
        [StringLength(150)]
        public string Item { get; set; } = string.Empty;

        [Range(1, int.MaxValue)]
        public int Quantidade { get; set; } = 1;

        [StringLength(50)]
        public string? UnidadeMedida { get; set; }

        [DataType(DataType.Date)]
        public DateTime DataRegistro { get; set; } = DateTime.Now;

        [StringLength(150)]
        public string? UsuarioRegistro { get; set; }

        [StringLength(500)]
        public string? Observacoes { get; set; }
    }
}
