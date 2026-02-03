using System;
using IgrejaSocial.Domain.Enums;

namespace IgrejaSocial.Domain.Models
{
    public class DevolucaoEquipamentoRequest
    {
        public Guid EquipamentoId { get; set; }
        public DateTime DataDevolucaoReal { get; set; }
        public StatusEquipamento EstadoConservacao { get; set; }
    }
}
