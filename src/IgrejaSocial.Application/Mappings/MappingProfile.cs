using AutoMapper;
using IgrejaSocial.Domain.Entities;
using IgrejaSocial.Application.DTOs;

namespace IgrejaSocial.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Mapeamento de Família
            CreateMap<Familia, FamiliaDto>();

            // Mapeamento de Membro (Corrigido)
            CreateMap<MembroFamilia, MembroFamiliaDto>();

            // Mapeamento de Equipamento (Corrigido)
            CreateMap<Equipamento, EquipamentoDto>();
        }
    }
}