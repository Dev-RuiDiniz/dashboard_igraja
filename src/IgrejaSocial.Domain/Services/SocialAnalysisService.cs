using IgrejaSocial.Domain.Entities;

namespace IgrejaSocial.Domain.Services
{
    public class SocialAnalysisService
    {
        /// <summary>
        /// Calcula a renda per capita da família somando a renda de todos os membros.
        /// </summary>
        public decimal CalcularRendaPerCapita(Familia familia)
        {
            if (familia.Membros == null || !familia.Membros.Any())
            {
                // Se não houver membros detalhados, usa a renda total informada na família
                // dividido por 1 (o responsável) ou pelo número total de residentes.
                return familia.RendaFamiliarTotal / 1;
            }

            // Soma a renda individual de todos os membros cadastrados
            decimal somaRendaMembros = familia.Membros.Sum(m => m.RendaIndividual);
            
            // O total de pessoas inclui o responsável (entidade Familia) + membros
            int totalPessoas = familia.Membros.Count + 1;

            return (familia.RendaFamiliarTotal + somaRendaMembros) / totalPessoas;
        }

        /// <summary>
        /// Define o nível de prioridade com base na renda per capita (Exemplo de lógica)
        /// </summary>
        public string DefinirPrioridade(decimal rendaPerCapita)
        {
            if (rendaPerCapita <= 218) return "Extrema Pobreza"; // Referência CadÚnico
            if (rendaPerCapita <= 706) return "Pobreza";
            return "Baixa Renda";
        }
    }
}