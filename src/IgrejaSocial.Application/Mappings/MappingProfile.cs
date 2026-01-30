using AutoMapper;
using IgrejaSocial.Domain.Entities;
using IgrejaSocial.Application.DTOs;

namespace IgrejaSocial.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Mapeamento Família -> FamiliaDto
            CreateMap<Familia, FamiliaDto>();

            // Mapeamento Membro -> MembroFamiliaDto
            // Convertendo Enums para String para facilitar a exibição na UI
            CreateMap<MembroFamilia, MembroFamiliaDto>()
                .ForMember(dest => dest.Parentesco, opt => opt.MapFrom(src => src.Parentesco.ToString()))
                .ForMember(dest => dest.Idade, opt => opt.MapFrom(src => src.Idade));

            // Mapeamento Equipamento -> EquipamentoDto
            CreateMap<Equipamento, EquipamentoDto>()
                .ForMember(dest => dest.Tipo, opt => opt.MapFrom(src => src.Tipo.ToString()))
                .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => src.Estado.ToString()));
        }
    }
}