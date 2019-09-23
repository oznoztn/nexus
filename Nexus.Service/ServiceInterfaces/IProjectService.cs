using System.Collections.Generic;
using Nexus.Core.Entities;
using Nexus.Service.DTOs;
using Nexus.Service.GenericService;

namespace Nexus.Service.ServiceInterfaces
{
    public interface IProjectService : IService<Project, ProjectDto>
    {
        ProjectDto GetBySlug(string slug);
        IEnumerable<ProjectDto> GetAllWithFirstPictureIncluded();
    }
}
