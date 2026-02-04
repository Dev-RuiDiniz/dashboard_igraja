using IgrejaSocial.Domain.Enums;

namespace IgrejaSocial.Domain.Models
{
    public class EquipamentoEstoqueBaixoDto
    {
        public TipoEquipamento Tipo { get; set; }
        public int Disponiveis { get; set; }
        public int Total { get; set; }
    }
}
