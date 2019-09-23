using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Nexus.Core.Entities;
using Nexus.Data.Interfaces;
using Nexus.Service.DTOs;
using Nexus.Service.Extensions;
using Nexus.Service.GenericService;
using Nexus.Service.ServiceInterfaces;

namespace Nexus.Service
{
    public class ProjectService : Service<Project, ProjectDto>, IProjectService
    {
        private readonly IMapper _mapper;
        private readonly IProjectRepository _projectRepository;

        public ProjectService(IMapper mapper, IProjectRepository projectRepository) : base(projectRepository, mapper)
        {
            _mapper = mapper;
            _projectRepository = projectRepository;
        }

        public ProjectDto GetBySlug(string slug)
        {
            var project = _projectRepository.GetBySlug(slug);

            if (project?.ProjectPictures != null)
            {
                project.ProjectPictures = project.ProjectPictures
                    .OrderBy(pic => pic.DisplayOrder)
                    .ThenByDescending(p => p.IsVisible)
                    .ThenBy(pic => pic.Id).ToArray();
            }

            var projectDto = _mapper.Map<ProjectDto>(project);

            return projectDto;
        }

        public IEnumerable<ProjectDto> GetAllWithFirstPictureIncluded()
        {
            var projects = _projectRepository.GetAllWithFirstPictureIncluded();
            var dtos = _mapper.Map<IEnumerable<ProjectDto>>(projects);

            return dtos;
        }
    }
}