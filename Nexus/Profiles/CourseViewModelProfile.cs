using System.Linq;
using AutoMapper;
using Nexus.Areas.Admin.Models;
using Nexus.Service.DTOs;

namespace Nexus.Profiles
{
    public class CourseViewModelProfile : Profile
    {
        public CourseViewModelProfile()
        {
            CreateMap<CourseDto, CourseViewModel>()
                .ForMember(vm => vm.CategoryId, os => os.Condition(dto => dto.CategoryId.HasValue))
                .ForMember(vm => vm.Categories, os => os.MapFrom((dto, vm) =>
                {
                    return dto.CourseCategories.Select(bc => bc.CategoryId).ToArray();
                }));

            CreateMap<CourseViewModel, CourseDto>()
                .ForMember(dto => dto.CategoryId, os => os.Condition(vm => vm.CategoryId.HasValue && vm.CategoryId.Value != 0))
                .ForMember(dto => dto.CourseCategories, os => os.MapFrom((vm, dto) =>
                {
                    return vm.Categories.Select(id => new CourseCategoryDto() { CategoryId = id }).ToList();
                }));
        }
    }
}