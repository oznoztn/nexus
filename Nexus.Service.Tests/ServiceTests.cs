using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Nexus.Core.Entities;
using Nexus.Data;
using Moq;
using Nexus.Data.Helpers;
using Nexus.Data.Repositories;
using Nexus.Service.DTOs;
using Nexus.Service.Profiles;
using Nexus.Service.GenericService;
using Nexus.Tests;
using Xunit;

namespace Nexus.Service.Tests
{
    public class ServiceTests : ServiceTestsBase
    {
        [Fact]
        public void Add_DtoShouldHaveSameIdWithTheEntityAfterInsertionToDb_IdShouldNotBeDefaultValue()
        {
            ReInitMapper();
            using (var context = new NexusContext(InMemoryHelpers.CreateOptions("nascence")))
            {
                var service = new Service<Tag,TagDto>(new Repository<Tag>(context));
                var dto = new TagDto { Id = 0, Title = "mir" };
                service.Add(dto);
                Assert.True(dto.Id != default(int));
            }
        }

        [Fact]
        public void Add_DtosShouldHaveSameIdsWithCorrespondingEntitiesAfterInsertionToDb_DtosIdsShouldntBeDefaultValue()
        {
            ReInitMapper();
            var dtos = new List<TagDto>
            {
                new TagDto {Id = 0, Title = "XXX"},
                new TagDto {Id = 0, Title = "YYY"}
            };

            using (var context = new NexusContext(InMemoryHelpers.CreateOptions("luminescent")))
            {
                var service = new Service<Tag, TagDto>(new Repository<Tag>(context));
                service.Add(dtos);

                var idx = context.Tags.Single(t => t.Title == "XXX").Id;
                var idy = context.Tags.Single(t => t.Title == "YYY").Id;

                Assert.Equal(idx, dtos.First().Id);
                Assert.Equal(idy, dtos.Last().Id);
            }
        }

        [Fact]
        public void CanRetrieveBook()
        {
            ReInitMapper();
            Mock<Repository<Book>> mockGenericRepository = new Mock<Repository<Book>>();
            mockGenericRepository
                .Setup(t => t.Get(It.IsAny<int>()))
                .Returns(() => new Book());

            Service<Book, BookDto> genericService = new Service<Book, BookDto>(mockGenericRepository.Object);

            BookDto bookDto = genericService.Get(0);

            Assert.NotNull(bookDto);
        }

        [Fact]
        public async Task CanRetrieveBookAsync()
        {
            ReInitMapper();
            Mock<NexusContext> mockContext = new Mock<NexusContext>();
            mockContext
                .Setup(context => context.Set<Book>())
                .Returns(TestHelpers.MockDbSet<Book>(new List<Book>() { new Book { Id = 1, Author = "adam smith" } }));
            
            var service = new Service<Book, BookDto>(new Repository<Book>(mockContext.Object));

            var book = await service.GetAsync(1);

            Assert.NotNull(book);
        }

        [Fact]
        public void CanDeleteBookById()
        {
            ReInitMapper();
            string databaseName = Guid.NewGuid().ToString();
            int id = 0;

            using (var context = new NexusContext(InMemoryHelpers.CreateOptions(databaseName)))
            {
                var entity = new Book {Author = "adam smith"};
                context.Books.Add(entity);
                context.SaveChanges();
                id = entity.Id;
            }

            using (var context = new NexusContext(InMemoryHelpers.CreateOptions(databaseName)))
            {
                var service = new Service<Book, BookDto>(new Repository<Book>(context));

                var dto = service.Get(id);
                service.Delete(dto);
            }

            using (var context = new NexusContext(InMemoryHelpers.CreateOptions(databaseName)))
            {
                var service = new Service<Book, BookDto>(new Repository<Book>(context));

                var dto = service.Get(id);
                
                Assert.Null(dto);
            }
        }
    }    
}