using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

        [DataType(DataType.Date)]
        public DateTime DataCadastro { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "O endereço é obrigatório.")]
        [StringLength(255)]
        public string Endereco { get; set; }

        public string TelefoneContato { get; set; }

        [Range(0, 100000, ErrorMessage = "A renda total deve ser um valor positivo.")]
        public decimal RendaFamiliarTotal { get; set; }

        [Range(1, 20, ErrorMessage = "A família deve ter pelo menos 1 integrante.")]
        public int QuantidadeIntegrantes { get; set; }

        // Propriedade Calculada (Lógica de Negócio)
        public decimal RendaPerCapita => QuantidadeIntegrantes > 0 ? RendaFamiliarTotal / QuantidadeIntegrantes : 0;

        // Regra de Vulnerabilidade Simplificada
        public bool IsVulneravel => RendaPerCapita < 660.00m; // Exemplo: metade de um salário mínimo

        public string Observacoes { get; set; }

        // Relacionamentos (Navegação)
        // public virtual ICollection<Membro> Membros { get; set; }
        // public virtual ICollection<EntregaCesta> HistoricoEntregas { get; set; }
    }
}