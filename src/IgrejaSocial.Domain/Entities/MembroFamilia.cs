using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IgrejaSocial.Domain.Enums;

namespace IgrejaSocial.Domain.Entities
{
    public class MembroFamilia
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "O nome do membro é obrigatório.")]
        [StringLength(150)]
        public string Nome { get; set; }

        [Required(ErrorMessage = "A data de nascimento é obrigatória.")]
        public DateTime DataNascimento { get; set; }

        [Required(ErrorMessage = "O parentesco deve ser informado.")]
        public TipoParentesco Parentesco { get; set; }

        [Required(ErrorMessage = "A escolaridade deve ser informada.")]
        public Escolaridade NivelEscolar { get; set; }

        [Required(ErrorMessage = "O status ocupacional deve ser informado.")]
        public StatusOcupacional SituacaoTrabalho { get; set; }

        public decimal RendaIndividual { get; set; }

        public string Cpf { get; set; }

        public bool PossuiDeficiencia { get; set; }

        [Required]
        public Guid FamiliaId { get; set; }

        [ForeignKey("FamiliaId")]
        public virtual Familia Familia { get; set; }

        // Cálculo dinâmico de idade (Roadmap Tarefa 2)
        public int Idade => DateTime.Today.Year - DataNascimento.Year - 
            (DateTime.Today < DataNascimento.AddYears(DateTime.Today.Year - DataNascimento.Year) ? 1 : 0);
    }
}