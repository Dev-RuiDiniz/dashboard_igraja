using System;
using System.ComponentModel.DataAnnotations;

namespace IgrejaSocial.Domain.Models
{
    public class DoacaoAvulsaRequest
    {
        [Required]
        public Guid PessoaEmSituacaoRuaId { get; set; }

        [Required]
        [StringLength(150)]
        public string Item { get; set; } = string.Empty;

        [Range(1, int.MaxValue)]
        public int Quantidade { get; set; } = 1;

        [StringLength(50)]
        public string? UnidadeMedida { get; set; }

        [StringLength(500)]
        public string? Observacoes { get; set; }
    }
}
