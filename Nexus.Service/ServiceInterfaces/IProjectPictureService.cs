using System.Collections.Generic;
using Nexus.Service.DTOs;
using Nexus.Service.GenericService;

namespace Nexus.Service.ServiceInterfaces
{
    public interface IProjectPictureService : IService<ProjectPictureDto, ProjectPictureDto>
    {
        IEnumerable<ProjectPictureDto> GetProjectPictures(int projectId);
        IEnumerable<ProjectPictureDto> GetProjectPicturesOrdered(int projectId);
    }
}