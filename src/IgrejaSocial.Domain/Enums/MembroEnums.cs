namespace IgrejaSocial.Domain.Enums
{
    public enum TipoParentesco
    {
        Responsavel,
        Conjuge,
        Filho,
        Enteado,
        Neto,
        PaiMae,
        Irmao,
        Outro
    }

    public enum Escolaridade
    {
        NaoAlfabetizado,
        EnsinoFundamentalIncompleto,
        EnsinoFundamentalCompleto,
        EnsinoMedioIncompleto,
        EnsinoMedioCompleto,
        EnsinoSuperior,
        EducacaoInfantil // Para crianças pequenas
    }

    public enum StatusOcupacional
    {
        Desempregado,
        EmpregadoCLT,
        Autonomo,
        Aposentado,
        Pensionista,
        Estudante,
        TrabalhoInfantil_Alerta // Importante para proteção social
    }
}