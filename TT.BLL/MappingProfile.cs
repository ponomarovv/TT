using AutoMapper;
using TT.BLL.Models;
using TT.DAL.Entities;

namespace TT.BLL
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Project, ProjectModel>()
                .ForMember(dest => dest.TotalTimeSpent, opt => opt.Ignore());
        }
    }
}
