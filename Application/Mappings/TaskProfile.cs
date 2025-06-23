using Application.DTOs.Comments;
using Application.DTOs.Tasks;
using AutoMapper;
using Domain.Entities;

namespace Application.Profiles;

public class TaskProfile : Profile
{
    public TaskProfile()
    {
        CreateMap<TaskItem, TaskDto>()
            .ForMember(dest => dest.CreatorName, opt => opt.MapFrom(src => src.Creator.Username))
            .ForMember(dest => dest.AssigneeName, opt => opt.MapFrom(src => src.Assignee != null ? src.Assignee.Username : string.Empty));
        
        CreateMap<CreateTaskDto, TaskItem>();
        CreateMap<Comment, CommentDto>()
            .ForMember(dest => dest.AuthorName, opt => opt.MapFrom(src => src.Author.Username));
        CreateMap<TaskHistory, TaskHistoryDto>()
            .ForMember(dest => dest.ChangedByUserName, opt => opt.MapFrom(src => src.ChangedByUser.Username));
    }
}