using System;
using System.ComponentModel.DataAnnotations;
using IgrejaSocial.Domain.Validations;

namespace IgrejaSocial.Domain.Entities
{
    public class PessoaEmSituacaoRua
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(150, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 150 caracteres.")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O CPF é obrigatório.")]
        [Cpf]
        public string Cpf { get; set; } = string.Empty;

        [StringLength(20, ErrorMessage = "O RG deve ter no máximo 20 caracteres.")]
        public string? Rg { get; set; }

        public bool DocumentacaoApresentada { get; set; } = true;

        [DataType(DataType.Date)]
        public DateTime? DataNascimento { get; set; }

        [DataType(DataType.Date)]
        public DateTime DataCadastro { get; set; } = DateTime.Now;

        [Phone(ErrorMessage = "Telefone em formato inválido.")]
        public string? TelefoneContato { get; set; }

        [StringLength(255)]
        public string? LocalReferencia { get; set; }

        [StringLength(500)]
        public string? Observacoes { get; set; }
    }
}
