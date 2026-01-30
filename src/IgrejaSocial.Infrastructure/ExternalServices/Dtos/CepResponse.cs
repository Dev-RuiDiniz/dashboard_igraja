namespace IgrejaSocial.Infrastructure.ExternalServices.Dtos
{
    public class CepResponse
    {
        public string Cep { get; set; }
        public string Logradouro { get; set; }
        public string Bairro { get; set; }
        public string Localidade { get; set; } // Cidade
        public string Uf { get; set; }
    }
}