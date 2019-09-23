using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using AutoMapper;
using Nexus.Areas.Admin.Models;
using Nexus.Service.DTOs;
using Nexus.Shared;

namespace Nexus.Profiles
{
    public class NoteViewModelProfile : Profile
    {
        public NoteViewModelProfile()
        {
            CreateMap<NoteDto, NoteViewModel>()
                .ForMember(vm => vm.Categories, conf => conf.Ignore())
                .AfterMap((dto, vm) =>
                {
                    vm.Categories = dto.NoteCategories.Select(t => t.CategoryId).ToArray();
                    vm.Tags = string.Join(',', dto.NoteTags.Select(nt => nt.Title));
                });

            CreateMap<NoteViewModel, NoteDto>()
                .ForMember(dest => dest.Slug, o => o.MapFrom(so => Slug.Create(so.Title)))
                .ForMember(dest => dest.Slug, o => o.MapFrom((sourceVm, destDto) =>
                {
                    // Adding
                    switch (sourceVm.Id)
                    {
                        case default(int) when string.IsNullOrWhiteSpace(sourceVm.Slug):
                            return Slug.Create(sourceVm.Title);
                        case default(int):
                            return sourceVm.Slug;
                    }

                    // Editing
                    if (!string.IsNullOrWhiteSpace(sourceVm.Slug))
                        return sourceVm.Slug;
                    else
                        return Slug.Create(sourceVm.Title);

                }))
                .ForMember(dto => dto.NoteCategories, conf => conf.Ignore())
                .AfterMap((vm, dto) =>
                {
                    dto.NoteCategories = vm.Categories.Select(id => new NoteCategoryDto()
                    {
                        CategoryId = id
                    }).ToList();

                    if (!string.IsNullOrWhiteSpace(vm.Tags))
                    {
                        dto.NoteTags = vm.Tags.Split(",").Select(input => new NoteTagDto()
                        {
                            NoteId = dto.Id,
                            Title = input.Trim(),
                            Slug = Slug.Create(input.Trim())
                        }).ToList();
                    }
                });
        }
    }
}