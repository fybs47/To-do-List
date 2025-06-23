using Application.DTOs.Users;
using Application.Helpers;
using AutoMapper;
using Domain.Entities;

namespace Application.Profiles;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>();
        CreateMap<UpdateUserDto, User>()
            .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.PasswordHash, 
                opt => opt.MapFrom(src => PasswordHelper.HashPassword(src.Password)))
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}