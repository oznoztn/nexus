using System.Collections.Generic;
using Nexus.Core.Entities;
using Nexus.Service.DTOs;
using Nexus.Service.GenericService;

namespace Nexus.Service.ServiceInterfaces
{
    public interface ICourseService : IService<Course, CourseDto>
    {
        List<CourseGroup> GetCourseGroups();
        int GetNextDisplayOrder();
    }
}