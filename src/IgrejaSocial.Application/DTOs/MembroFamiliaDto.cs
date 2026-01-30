namespace IgrejaSocial.Application.DTOs
{
    public class MembroFamiliaDto
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Parentesco { get; set; } // Pode ser string para o usuário ler fácil
        public int Idade { get; set; }         // Usaremos a propriedade computada
        public decimal RendaIndividual { get; set; }
    }
}