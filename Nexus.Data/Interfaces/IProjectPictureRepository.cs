using System.Collections.Generic;
using Nexus.Core;
using Nexus.Core.Entities;

namespace Nexus.Data.Interfaces
{
    public interface IProjectPictureRepository : IRepository<ProjectPicture>
    {
        void UpdatePictureInformation(ProjectPicture projectPicture);
    }
}