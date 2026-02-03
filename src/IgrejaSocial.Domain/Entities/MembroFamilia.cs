using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization; // Necessário para o JsonIgnore
using IgrejaSocial.Domain.Enums;
using IgrejaSocial.Domain.Validations;

namespace IgrejaSocial.Domain.Entities
{
    public class MembroFamilia
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required(ErrorMessage = "O nome do membro é obrigatório.")]
        [StringLength(150)]
        public string Nome { get; set; } = string.Empty; // Inicializado para evitar CS8618

        [Required(ErrorMessage = "A data de nascimento é obrigatória.")]
        public DateTime DataNascimento { get; set; }

        [Required(ErrorMessage = "O parentesco deve ser informado.")]
        public TipoParentesco Parentesco { get; set; }

        [Required(ErrorMessage = "A escolaridade deve ser informada.")]
        public GrauInstrucao NivelEscolar { get; set; }

        [Required(ErrorMessage = "O status ocupacional deve ser informado.")]
        public StatusOcupacional SituacaoTrabalho { get; set; }

        public decimal RendaIndividual { get; set; }

        [Cpf]
        public string? Cpf { get; set; } // Anulável, pois nem todos os membros (crianças) possuem CPF

        public bool PossuiDeficiencia { get; set; }

        [Required]
        public Guid FamiliaId { get; set; }

        [ForeignKey("FamiliaId")]
        [JsonIgnore] // EVITA ERRO 400 NO SWAGGER: O Swagger não tentará validar a Família inteira aqui
        public virtual Familia? Familia { get; set; } // Anulável para facilitar a criação via API

        // Cálculo dinâmico de idade ajustado para o dia atual
        [JsonIgnore] // Não precisa ser enviado no JSON, é calculado no servidor
        public int Idade => DateTime.Today.Year - DataNascimento.Year - 
            (DateTime.Today < DataNascimento.AddYears(DateTime.Today.Year - DataNascimento.Year) ? 1 : 0);
    }
}
