using AutoMapper;
using Nexus.Areas.Admin.Models;
using Nexus.Service.DTOs;
using Nexus.Shared;

namespace Nexus.Profiles
{
    public class CategoryViewModelProfile : Profile
    {
        public CategoryViewModelProfile()
        {
            CreateMap<CategoryDto, CategoryViewModel>();

            CreateMap<CategoryViewModel, CategoryDto>()
                .ForMember(dest => dest.Slug, o => o.MapFrom((source, dest) =>
                {
                    if (string.IsNullOrWhiteSpace(source.Slug))
                        return Slug.Create(source.Title);

                    return source.Slug;
                }));
        }
    }
}