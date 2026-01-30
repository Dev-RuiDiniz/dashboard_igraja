using AutoMapper;
using IgrejaSocial.Domain.Entities;
using IgrejaSocial.Application.DTOs;

namespace IgrejaSocial.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Mapeamento simples: As propriedades com nomes iguais são automáticas
            CreateMap<Familia, FamiliaDto>();
            
            // Se precisar de mapeamento para Membros
            CreateMap<MembroFamilia, MembroFamiliaDto>();
            
            // Mapeamento para Equipamentos
            CreateMap<Equipamento, EquipamentoDto>();
        }
    }
}