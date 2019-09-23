using System.Collections.Generic;
using Nexus.Core;
using Nexus.Core.Entities;

namespace Nexus.Data.Interfaces
{
    public interface ICourseRepository : IRepository<Course>
    {
        IEnumerable<Course> GetCoursesByCategory(int categoryId);
        IEnumerable<Course> GetCoursesByCategories(int[] categoryIds);
        int GetNextDisplayOrder();
    }
}