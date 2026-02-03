namespace IgrejaSocial.Domain.Models
{
    public class RelatorioMensalDto
    {
        public int Mes { get; set; }
        public int Ano { get; set; }
        public int TotalCestasEntregues { get; set; }
        public int TotalEquipamentosEmprestados { get; set; }
    }
}
