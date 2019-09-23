using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Moq;
using Nexus.Core.Entities;
using Nexus.Data.Repositories;
using Nexus.Service.Profiles;
using Nexus.Tests;
using Xunit;

namespace Nexus.Service.Tests
{
    public class CategoryServiceTests : ServiceTestsBase
    {
        [Fact]
        public void TestingZeroCategoryScenario()
        {
            ReInitMapper();
            var categories = new List<Category>();
            var categoryRepositoryMock = new Mock<CategoryRepository>();
            categoryRepositoryMock.Setup(repo => repo.GetAll()).Returns(() => categories.AsEnumerable());

            var categoryService = new CategoryService(categoryRepositoryMock.Object, null);
            var result = categoryService.GetCategoriesTree(false); 

            Assert.Empty(result);
        }

        [Fact]
        public void TestingOneCategoryScenario()
        {
            ReInitMapper();
            var categories = new List<Category>
            {
                new Category {Id = 1, ParentId = 0, Title = "Programming", DisplayOrder = 1}
            };

            var categoryRepositoryMock = new Mock<CategoryRepository>();
            categoryRepositoryMock.Setup(repo => repo.GetAll()).Returns(() => categories.AsEnumerable());

            var categoryService = new CategoryService(categoryRepositoryMock.Object, null);

            var result = categoryService.GetCategoriesTree();

            Assert.Equal("Programming", result.Single(t => t.Id == 1).Title);
        }

        [Fact]
        public void NoChildElements_TitlesShouldBeAsTheyAre()
        {
            ReInitMapper();
            var categories = new List<Category>
            {
                new Category {Id = 1, ParentId = 0, Title = "Programming", DisplayOrder = 1},
                new Category {Id = 2, ParentId = 0, Title = "Trading", DisplayOrder = 2}
            };

            var categoryRepositoryMock = new Mock<CategoryRepository>();
            categoryRepositoryMock.Setup(repo => repo.GetAll()).Returns(() => categories.AsEnumerable());

            var categoryService = new CategoryService(categoryRepositoryMock.Object, null);

            var result = categoryService.GetCategoriesTree().ToArray();

            Assert.Equal("Programming", result.Single(t => t.Id == 1).Title);
            Assert.Equal("Trading", result.Single(t => t.Id == 2).Title);
        }

        [Fact]
        public void Level2_OneParentWithOneChild_TitlesShouldBeModifiedAccordingly()
        {
            ReInitMapper();
            var categories = new List<Category>
            {
                new Category {Id = 1, ParentId = 0, Title = "Programming", DisplayOrder = 1},
                new Category {Id = 2, ParentId = 1, Title = "C#", DisplayOrder = 2}
            };

            var categoryRepositoryMock = new Mock<CategoryRepository>();
            categoryRepositoryMock.Setup(repo => repo.GetAll()).Returns(() => categories.AsEnumerable());

            var categoryService = new CategoryService(categoryRepositoryMock.Object, null);

            var result = categoryService.GetCategoriesTree().ToArray();

            Assert.Equal("Programming", result.Single(t => t.Id == 1).Title);
            Assert.Equal("Programming  >>  C#", result.Single(t => t.Id == 2).Title);
        }

        [Fact]
        public void Level3_TitlesShouldBeModifiedAccordingly()
        {
            ReInitMapper();
            var categories = new List<Category>
            {
                new Category {Id = 1, ParentId = 0, Title = "Programming", DisplayOrder = 1},
                new Category {Id = 2, ParentId = 1, Title = "C#", DisplayOrder = 2},
                new Category {Id = 3, ParentId = 2, Title = "TPL", DisplayOrder = 3}
            };

            var categoryRepositoryMock = new Mock<CategoryRepository>();
            categoryRepositoryMock.Setup(repo => repo.GetAll()).Returns(() => categories.AsEnumerable());

            var categoryService = new CategoryService(categoryRepositoryMock.Object, null);

            var result = categoryService.GetCategoriesTree().ToArray();

            Assert.Equal("Programming", result.Single(t => t.Id == 1).Title);
            Assert.Equal("Programming  >>  C#", result.Single(t => t.Id == 2).Title);
            Assert.Equal("Programming  >>  C#  >>  TPL", result.Single(t => t.Id == 3).Title);
        }

        [Fact]
        public void ChildElementHasTwoChildren_TitlesShouldBeModifiedAsExpected()
        {
            ReInitMapper();
            var categories = new List<Category>();
            categories.Add(new Category { Id = 1, ParentId = 0, Title = "Programming", DisplayOrder = 1 });
            categories.Add(new Category { Id = 2, ParentId = 1, Title = "C#", DisplayOrder = 2 });
            categories.Add(new Category { Id = 3, ParentId = 2, Title = "TPL", DisplayOrder = 3 });
            categories.Add(new Category { Id = 4, ParentId = 2, Title = "ASYNC", DisplayOrder = 4 });

            var categoryRepositoryMock = new Mock<CategoryRepository>();
            categoryRepositoryMock.Setup(repo => repo.GetAll()).Returns(() => categories.AsEnumerable());

            var categoryService = new CategoryService(categoryRepositoryMock.Object, null);

            var result = categoryService.GetCategoriesTree().ToArray();

            Assert.Equal("Programming", result.Single(t => t.Id == 1).Title);
            Assert.Equal("Programming  >>  C#", result.Single(t => t.Id == 2).Title);
            Assert.Equal("Programming  >>  C#  >>  TPL", result.Single(t => t.Id == 3).Title);
            Assert.Equal("Programming  >>  C#  >>  ASYNC", result.Single(t => t.Id == 4).Title);
        }
    }
}
