namespace IgrejaSocial.Domain.Enums
{
    public enum TipoResidencia
    {
        Propria,
        Alugada,
        Cedida,
        Ocupacao,
        SituacaoDeRua
    }

    public enum StatusAcompanhamento
    {
        Ativo,
        Suspenso,
        Arquivado, // Quando a família não precisa mais ou mudou-se
        AguardandoVisita
    }
}