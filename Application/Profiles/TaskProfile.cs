using Application.DTOs;
using AutoMapper;
using Domain.Entities;

namespace Application.Profiles;

public class TaskProfile : Profile
{
    public TaskProfile()
    {
        CreateMap<TaskItem, TaskItemDto>().ReverseMap();
    }
}