using System;
using IgrejaSocial.Domain.Enums;

namespace IgrejaSocial.Web.Models
{
    public class HistoricoEmprestimoDto
    {
        public Guid RegistroId { get; set; }
        public string CodigoPatrimonio { get; set; } = string.Empty;
        public TipoEquipamento Tipo { get; set; }
        public DateTime DataEmprestimo { get; set; }
        public DateTime DataPrevistaDevolucao { get; set; }
        public DateTime? DataDevolucao { get; set; }
        public EstadoConservacao? EstadoConservacao { get; set; }
    }
}
