using Application.Catalog;
using AutoMapper;
using Domain.Catalog.Entities;

namespace Application.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Department, DepartmentDto>();
        CreateMap<Position, PositionDto>();
        CreateMap<Speaker, SpeakerDto>();
        CreateMap<OnlineEvent, OnlineEventDto>();
    }
}