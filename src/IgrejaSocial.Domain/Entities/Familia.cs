using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Json.Serialization;
using IgrejaSocial.Domain.Enums;
using IgrejaSocial.Domain.Validations;

namespace IgrejaSocial.Domain.Entities
{
    public class Familia
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "O nome do responsável é obrigatório.")]
        [StringLength(150, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 150 caracteres.")]
        public string NomeResponsavel { get; set; } = string.Empty;

        [Required(ErrorMessage = "O CPF é obrigatório.")]
        [Cpf]
        public string CpfResponsavel { get; set; } = string.Empty;

        [Required]
        public TipoMoradia Residencia { get; set; }

        [Required]
        public StatusAcompanhamento Status { get; set; }

        [DataType(DataType.Date)]
        public DateTime DataCadastro { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "O endereço é obrigatório.")]
        [StringLength(255)]
        public string Endereco { get; set; } = string.Empty;

        [Phone(ErrorMessage = "Telefone em formato inválido.")]
        public string? TelefoneContato { get; set; } // Opcional

        public decimal RendaFamiliarTotal { get; set; }

        public string? Observacoes { get; set; } // Opcional

        public virtual ICollection<MembroFamilia> Membros { get; set; } = new List<MembroFamilia>();

        public int TotalIntegrantes => Membros.Count + 1;

        public decimal RendaPerCapita => TotalIntegrantes > 0 
            ? (RendaFamiliarTotal + Membros.Sum(m => m.RendaIndividual)) / TotalIntegrantes 
            : 0;

        public bool IsVulneravel => RendaPerCapita < 660.00m;

        public bool PossuiCriancas => Membros.Any(m => m.Idade < 12);
    }
}
