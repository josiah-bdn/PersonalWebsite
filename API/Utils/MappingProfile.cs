using System;
using Data.Entities;
using Data.DTO;
using AutoMapper;
namespace API.Logic {
    public class MappingProfile : Profile {
        public MappingProfile() {
            CreateMap<AppUser, CompleteProfileDto>();

            CreateMap<EditUserDto, AppUser>()
            .ForMember(dest => dest.ModifiedDate, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.FirstName, opt => opt.Condition(src => src.FirstName != null))
            .ForMember(dest => dest.LastName, opt => opt.Condition(src => src.LastName != null))
            .ForMember(dest => dest.UserName, opt => opt.Condition(src => src.UserName != null));

            CreateMap<UpdateBlogDto, Blog>()
            .ForMember(dest => dest.Modified, opt => opt.MapFrom(src => DateTime.UtcNow))
            .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
            .ForMember(dest => dest.Title, opt => opt.Condition(src => src.Title != null))
            .ForMember(dest => dest.Description, opt => opt.Condition(src => src.Description != null))
            .ForMember(dest => dest.Body, opt => opt.Condition(src => src.Body != null))
            .ForMember(dest => dest.ImageDescription, opt => opt.Condition(src => src.ImageDescription != null))
            .ForMember(dest => dest.ImageUrl, opt => opt.Condition(src => src.ImageUrl != null));


            }
        }
    }

