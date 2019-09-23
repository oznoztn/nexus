using System.Collections.Generic;
using System.Linq;
using Moq;
using Nexus.Core.Entities;
using Nexus.Data.Helpers;
using Nexus.Data.Repositories;
using Xunit;

namespace Nexus.Data.Tests
{
    public class CourseRepositoryTests
    {
        [Fact]
        public void GetCoursesByCategory_ShouldReturnOneCourse()
        {
            var categories = new List<Category>
            {
                new Category {Id = 1, Title = "cat1"}
            };
            var courses = new List<Course>
            {
                new Course {Id = 1, Title = "co1", CategoryId = 1},
                new Course {Id = 2, Title = "co2", CategoryId = 10}
            };

            var mockContext = new Mock<NexusContext>();
            mockContext.Setup(nex => nex.Set<Category>()).Returns(TestHelpers.MockDbSet(categories));
            mockContext.Setup(nex => nex.Set<Course>()).Returns(TestHelpers.MockDbSet(courses));

            int categoryId = 1;
            CourseRepository repository = new CourseRepository(mockContext.Object);
            List<Course> result = repository.GetCoursesByCategory(categoryId).ToList();

            Assert.Single(result);
        }

        [Fact]
        public void GetCoursesByCategory_ShouldReturnEmptyList()
        {
            var categories = new List<Category> {new Category {Id = 1, Title = "cat1"}};
            var courses = new List<Course>
            {
                new Course {Id = 1, Title = "co1", CategoryId = 1},
                new Course {Id = 2, Title = "co2", CategoryId = 10}
            };

            var mockContext = new Mock<NexusContext>();
            mockContext.Setup(nex => nex.Set<Category>()).Returns(TestHelpers.MockDbSet(categories));
            mockContext.Setup(nex => nex.Set<Course>()).Returns(TestHelpers.MockDbSet(courses));

            int categoryId = 2;
            CourseRepository repository = new CourseRepository(mockContext.Object);
            List<Course> result = repository.GetCoursesByCategory(categoryId).ToList();

            Assert.Empty(result);
        }

        [Fact]
        public void GetCoursesByCategories_ShouldQueryProperly()
        {
            var categories = new List<Category>
            {
                new Category {Id = 1, Title = "cat1"},
                new Category {Id = 2, Title = "cat2"},
                new Category {Id = 3, Title = "cat3"}
            };
            var courses = new List<Course>
            {
                new Course {Id = 1, Title = "x", CategoryId = 1},
                new Course {Id = 2, Title = "y", CategoryId = 2},
                new Course {Id = 3, Title = "z", CategoryId = 10},
            };

            var mockContext = new Mock<NexusContext>();
            mockContext.Setup(nex => nex.Set<Category>()).Returns(TestHelpers.MockDbSet(categories));
            mockContext.Setup(nex => nex.Set<Course>()).Returns(TestHelpers.MockDbSet(courses));

            int[] categoryIds = {1, 2};
            CourseRepository repository = new CourseRepository(mockContext.Object);
            List<Course> result = repository.GetCoursesByCategories(categoryIds).ToList();

            Assert.Equal(2, result.Count);
            Assert.Contains(result, c => c.Title == "x");
            Assert.Contains(result, c => c.Title == "y");
            Assert.DoesNotContain(result, c => c.Title == "z");
        }
    }
}