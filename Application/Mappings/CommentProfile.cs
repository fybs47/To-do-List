using Application.DTOs.Comments;
using AutoMapper;
using Domain.Entities;

namespace Application.Profiles;

public class CommentProfile : Profile
{
    public CommentProfile()
    {
        CreateMap<Comment, CommentDto>()
            .ForMember(dest => dest.AuthorName, opt => 
                opt.MapFrom(src => src.Author.Username))
            .ForMember(dest => dest.TaskTitle, opt => 
                opt.MapFrom(src => src.Task.Title));
        
        CreateMap<CreateCommentDto, Comment>()
            .ForMember(dest => dest.CreatedAt, opt => 
                opt.MapFrom(_ => DateTime.UtcNow));
    }
}