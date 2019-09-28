using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Nexus.Core.Entities;
using Nexus.Service.DTOs;
using Nexus.Service.Extensions;

namespace Nexus.Service.Profiles
{
    public class ProjectProfile : Profile
    {
        public ProjectProfile()
        {
            CreateMap<Project, ProjectDto>()
                .ForMember(dest => dest.ProjectPictureDtos, o => o.Ignore()).AfterMap((source, dest) =>
                {
                    if (source.ProjectPictures != null)
                    {
                        var projectPictureDtos = dest.ProjectPictureDtos.ToList();
                        foreach (var projectPicture in source.ProjectPictures.ToArray())
                        {
                            ProjectPictureDto projectPictureDto = new ProjectPictureDto()
                            {
                                Id = projectPicture.Id,
                                ProjectId = projectPicture.ProjectId,
                                DisplayOrder = projectPicture.DisplayOrder,
                                FileName = projectPicture.FileName,
                                Title = projectPicture.Title,
                                Caption = projectPicture.Caption,
                                Alt = projectPicture.Alt,
                                Binary = projectPicture.Binary,
                                IsVisible = projectPicture.IsVisible
                            };
                            projectPictureDtos.Add(projectPictureDto);;
                        }
                        dest.ProjectPictureDtos = projectPictureDtos;
                    }
                });

            CreateMap<ProjectDto, Project>();
        }
    }
}