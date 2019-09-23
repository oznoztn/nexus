using System;
using System.Collections.Generic;
using System.IO;
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
    public class ProjectPictureService : Service<ProjectPicture, ProjectPictureDto>, IProjectPictureService
    {
        private readonly IProjectPictureRepository _projectPictureRepository;
        private readonly IMapper _mapper;

        public ProjectPictureService(IMapper mapper, IProjectPictureRepository projectPictureRepository) : base(projectPictureRepository, mapper)
        {
            _mapper = mapper;
            _projectPictureRepository = projectPictureRepository;
        }

        public IEnumerable<ProjectPictureDto> GetProjectPictures(int projectId)
        {
            var projectPictures = _projectPictureRepository.GetAll().Where(pp => pp.ProjectId == projectId).ToList();
            var dtos = _mapper.Map<IEnumerable<ProjectPictureDto>>(projectPictures);

            return dtos;
        }

        public IEnumerable<ProjectPictureDto> GetProjectPicturesOrdered(int projectId)
        {
            var projectPictures = GetProjectPictures(projectId)
                .OrderByDescending(p => p.IsVisible)
                .ThenBy(pic => pic.DisplayOrder)
                .ThenBy(pic => pic.Id)
                .ToList();

            var dtos = _mapper.Map<IEnumerable<ProjectPictureDto>>(projectPictures);
            return dtos;
        }

        public void UpdatePictureInformation(ProjectPictureDto projectPictureDto)
        {
            ProjectPicture entity = projectPictureDto.MapTo<ProjectPicture>();

            _projectPictureRepository.UpdatePictureInformation(entity);
            _projectPictureRepository.UnitOfWork.SaveChanges();

            projectPictureDto = _mapper.Map<ProjectPictureDto>(entity);
        }
    }
}