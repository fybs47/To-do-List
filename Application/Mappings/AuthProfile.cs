using Application.DTOs.Auth;
using Application.DTOs.Users;
using Application.Helpers;
using AutoMapper;
using Domain.Entities;

namespace Application.Profiles;

public class AuthProfile : Profile
{
    public AuthProfile()
    {
        CreateMap<User, UserDto>();
        CreateMap<RegisterDto, User>()
            .ForMember(dest => dest.PasswordHash, 
                opt => opt.MapFrom(src => PasswordHelper.HashPassword(src.Password)))
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow));
    }
}