using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Nexus.Core.Entities;
using Nexus.Service.DTOs;

namespace Nexus.Service.Profiles
{
    public class NoteProfile : Profile
    {
        public NoteProfile()
        {
            CreateMap<Note, NoteDto>()
                .AfterMap((note, noteDto) =>
                {
                    if (note.NoteCategories != null)
                    {
                        List<Category> categoryEntities = note.NoteCategories.Select(nt => nt.Category).ToList();
                        List<NoteCategoryDto> categoryDtos = noteDto.NoteCategories.ToList();

                        for (int i = 0; i < categoryEntities.Count; i++)
                        {
                            categoryDtos[i].CategoryTitle = categoryEntities[i].Title;
                            categoryDtos[i].CategorySlug = categoryEntities[i].Slug;
                            categoryDtos[i].IsVisible = categoryEntities[i].IsVisible;
                        }
                    }

                    if (note.NoteTags != null)
                    {
                        List<Tag> tagEntities = note.NoteTags.Select(nt => nt.Tag).ToList();
                        List<NoteTagDto> noteTagDtos = noteDto.NoteTags.ToList();

                        for (int i = 0; i < tagEntities.Count; i++)
                        {
                            noteTagDtos[i].Title = tagEntities[i].Title;
                            noteTagDtos[i].Slug = tagEntities[i].Slug;
                            noteTagDtos[i].IsHidden = tagEntities[i].IsHidden;
                        }
                    }
                });

            CreateMap<NoteDto, Note>()
                .ForMember(dest => dest.NoteTags, o => o.Ignore());
        }
    }
}