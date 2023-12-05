using System;
using Data.Entities;
using Data.DTO;
using AutoMapper;
namespace API.Logic {
    public class MappingProfile : Profile {
        public MappingProfile() {
            CreateMap<AppUser, CreateAppUserDto>();

            CreateMap<EditUserDto, AppUser>()
            .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.FirstName, opt => opt.Condition(src => src.FirstName != null))
            .ForMember(dest => dest.LastName, opt => opt.Condition(src => src.LastName != null))
            .ForMember(dest => dest.UserName, opt => opt.Condition(src => src.UserName != null));

        }
    }
}

