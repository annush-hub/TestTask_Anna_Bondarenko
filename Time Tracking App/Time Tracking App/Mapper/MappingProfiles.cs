using AutoMapper;
using TimeTracking.Application.DTOs;
using TimeTracking.Core;

namespace Time_Tracking_App.Mapper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Activity, ActivityDto>()
                .ForMember(dest => dest.EmployeeFullName,
                           opt => opt.MapFrom(src => $"{src.Employee.FirstName} {src.Employee.LastName}"))
                .ForMember(dest => dest.ProjectName,
                           opt => opt.MapFrom(src => src.Project.Name))
                .ReverseMap();

            CreateMap<CreateActivityDto, Activity>()
                .ForMember(dest => dest.EmployeeId, opt => opt.MapFrom(src => src.EmployeeId))
                .ForMember(dest => dest.ProjectId, opt => opt.MapFrom(src => src.ProjectId))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role))
                .ForMember(dest => dest.ActivityType, opt => opt.MapFrom(src => src.ActivityType))
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
                .ForMember(dest => dest.Hours, opt => opt.MapFrom(src => src.Hours));

            CreateMap<Activity, CreateActivityDto>();

            CreateMap<CreateProjectDto, Project>();
            CreateMap<Project, ProjectDto>();

            CreateMap<Project, ProjectDto>().ReverseMap();
            CreateMap<CreateProjectDto, Project>().ReverseMap();

            CreateMap<Activity, TimeTrackingEntryDto>()
                .ForMember(dest => dest.Project, opt => opt.MapFrom(src => src.Project != null ? src.Project.Name : ""))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()))
                .ForMember(dest => dest.ActivityType, opt => opt.MapFrom(src => src.ActivityType.ToString()));

            CreateMap<Activity, TimeTrackingEntryDto>()
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()))
            .ForMember(dest => dest.Project, opt => opt.MapFrom(src => src.Project.Name))
            .ForMember(dest => dest.ActivityType, opt => opt.MapFrom(src => src.ActivityType.ToString()));

        }
    }
}
