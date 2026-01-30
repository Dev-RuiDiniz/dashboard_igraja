using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using IgrejaSocial.Domain.Enums;

namespace IgrejaSocial.Domain.Entities
{
    public class Familia
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "O nome do responsável é obrigatório.")]
        [StringLength(150, MinimumLength = 3, ErrorMessage = "O nome deve ter entre 3 e 150 caracteres.")]
        public string NomeResponsavel { get; set; }

        [Required(ErrorMessage = "O CPF é obrigatório.")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "O CPF deve conter apenas os 11 dígitos numéricos.")]
        public string CpfResponsavel { get; set; }

        [Required]
        public TipoResidencia Residencia { get; set; }

        [Required]
        public StatusAcompanhamento Status { get; set; }

        [DataType(DataType.Date)]
        public DateTime DataCadastro { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "O endereço é obrigatório.")]
        [StringLength(255)]
        public string Endereco { get; set; }

        [Phone(ErrorMessage = "Telefone em formato inválido.")]
        public string TelefoneContato { get; set; }

        public decimal RendaFamiliarTotal { get; set; }

        public string Observacoes { get; set; }

        public virtual ICollection<MembroFamilia> Membros { get; set; } = new List<MembroFamilia>();

        // Logística de Negócio Corrigida: Soma a renda de todos os membros para a média per capita
        public int TotalIntegrantes => Membros.Count + 1;

        public decimal RendaPerCapita => TotalIntegrantes > 0 
            ? (RendaFamiliarTotal + Membros.Sum(m => m.RendaIndividual)) / TotalIntegrantes 
            : 0;

        public bool IsVulneravel => RendaPerCapita < 660.00m;

        public bool PossuiCriancas => Membros.Any(m => m.Idade < 12);
    }
}