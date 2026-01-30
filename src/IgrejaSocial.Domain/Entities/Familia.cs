using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

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

        [Phone(ErrorMessage = "Telefone em formato inválido.")]
        public string TelefoneContato { get; set; }

        [Range(0, 100000, ErrorMessage = "A renda total deve ser um valor positivo.")]
        public decimal RendaFamiliarTotal { get; set; }

        public string Observacoes { get; set; }

        // --- Relacionamentos ---
        
        /// <summary>
        /// Coleção de dependentes da família.
        /// </summary>
        public virtual ICollection<MembroFamilia> Membros { get; set; } = new List<MembroFamilia>();

        // --- Propriedades Somente Leitura (Lógica de Negócio / DDD) ---

        /// <summary>
        /// Total de pessoas considerando o Responsável + Dependentes.
        /// </summary>
        public int TotalIntegrantes => Membros.Count + 1;

        /// <summary>
        /// Cálculo dinâmico da renda por pessoa.
        /// </summary>
        public decimal RendaPerCapita => TotalIntegrantes > 0 ? RendaFamiliarTotal / TotalIntegrantes : 0;

        /// <summary>
        /// Define se a família está abaixo da linha de pobreza (Ex: R$ 660,00 per capita).
        /// </summary>
        public bool IsVulneravel => RendaPerCapita < 660.00m;

        /// <summary>
        /// Atalho para verificar se há crianças (menores de 12 anos) na composição familiar.
        /// </summary>
        public bool PossuiCriancas => Membros.Any(m => m.Idade < 12);
    }
}