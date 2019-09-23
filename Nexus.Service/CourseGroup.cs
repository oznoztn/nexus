using System.Collections.Generic;
using Nexus.Service.DTOs;

namespace Nexus.Service
{
    public class CourseGroup
    {
        public int CategoryId { get; set; }
        public string CategoryTitle { get; set; }
        public List<CourseDto> Courses { get; set; }
    }
}