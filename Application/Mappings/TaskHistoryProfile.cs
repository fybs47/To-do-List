using Application.DTOs.Tasks;
using AutoMapper;

namespace Application.Profiles;

public class TaskHistoryProfile : Profile
{
    public TaskHistoryProfile()
    {
        CreateMap<TaskHistory, TaskHistoryDto>()
            .ForMember(dest => dest.ChangedByUserName, opt => opt.MapFrom(src => src.ChangedByUser.Username));
    }
}