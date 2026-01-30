using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        public string Parentesco { get; set; } // Ex: Filho, Cônjuge, Avô

        public string Cpf { get; set; }

        public bool PossuiDeficiencia { get; set; }

        // Foreign Key
        [Required]
        public Guid FamiliaId { get; set; }

        // Propriedade de Navegação
        [ForeignKey("FamiliaId")]
        public virtual Familia Familia { get; set; }

        // Lógica de Domínio
        public int Idade => DateTime.Today.Year - DataNascimento.Year - 
            (DateTime.Today < DataNascimento.AddYears(DateTime.Today.Year - DataNascimento.Year) ? 1 : 0);
    }
}