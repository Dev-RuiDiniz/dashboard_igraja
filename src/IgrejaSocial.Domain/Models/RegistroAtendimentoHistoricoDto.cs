using System;
using IgrejaSocial.Domain.Enums;

namespace IgrejaSocial.Domain.Models
{
    public class RegistroAtendimentoHistoricoDto
    {
        public Guid Id { get; set; }
        public Guid EquipamentoId { get; set; }
        public string CodigoPatrimonio { get; set; } = string.Empty;
        public string DescricaoEquipamento { get; set; } = string.Empty;
        public TipoEquipamento TipoEquipamento { get; set; }
        public DateTime DataEmprestimo { get; set; }
        public DateTime DataPrevistaDevolucao { get; set; }
        public DateTime? DataDevolucaoReal { get; set; }
        public StatusEquipamento EstadoConservacao { get; set; }
    }
}
