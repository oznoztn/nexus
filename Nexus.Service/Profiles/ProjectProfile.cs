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
                        List<ProjectPictureDto> pictureDtos = dest.ProjectPictureDtos.ToList();
                        foreach (var projectPicture in source.ProjectPictures.ToArray())
                        {
                            var pictureDto = projectPicture.MapTo<ProjectPictureDto>();
                            pictureDtos.Add(pictureDto);
                        }
                        dest.ProjectPictureDtos = pictureDtos;
                    }
                });

            CreateMap<ProjectDto, Project>();
        }
    }
}