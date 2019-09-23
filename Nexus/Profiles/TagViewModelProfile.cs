using AutoMapper;
using Nexus.Areas.Admin.Models;
using Nexus.Service.DTOs;

namespace Nexus.Profiles
{
    public class TagViewModelProfile : Profile
    {
        public TagViewModelProfile()
        {
            CreateMap<TagViewModel, TagDto>();
        }
    }
}