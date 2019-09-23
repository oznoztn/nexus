using System.Collections.Generic;
using Nexus.Core.Entities;

namespace Nexus.Data.Interfaces
{
    public interface IProjectRepository : IRepository<Project>
    {
        Project GetBySlug(string slug);
        IEnumerable<Project> GetAllWithFirstPictureIncluded();
    }
}