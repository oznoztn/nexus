using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Moq;
using Nexus.Core.Entities;
using Nexus.Data.Helpers;
using Xunit;

namespace Nexus.Data.Tests
{
    public class RepositoryTests
    {
        private Repository<T> CreateNewGenericRepositoryWithNewContext<T>(string databaseName)
            where T : class, IEntity
        {
            return new Repository<T>(new NexusContext(InMemoryHelpers.CreateOptions(databaseName)));
        }

        [Fact]
        public void Get_CanRetrieveExistingBook()
        {
            string databaseName = Guid.NewGuid().ToString();
            int bookId;
            using (var context = new NexusContext(InMemoryHelpers.CreateOptions(databaseName)))
            {
                var book = new Book();
                context.Books.Add(book);
                context.SaveChanges();

                bookId = book.Id;
            }

            var genericRepository = CreateNewGenericRepositoryWithNewContext<Book>(databaseName);
            var recentlySavedBook = genericRepository.Get(bookId);

            Assert.NotNull(recentlySavedBook);
        }

        [Fact]
        public void Get_ShouldReturnNullWhenEntityDoesntExist()
        {
            var mockContext = new Mock<DbContext>();
            mockContext.Setup(context => context.Set<Book>()).Returns(TestHelpers.MockDbSet(new List<Book>()));
            var repository = new Repository<Book>(mockContext.Object);

            var entity = repository.Get(default(int));

            Assert.Null(entity);
        }

        [Fact]
        public void CanRetrieveBooksAsync()
        {
            using (var context = new NexusContext(InMemoryHelpers.CreateOptions("dbx")))
            {
                context.Books.AddRange(GetBooks()); // two books
                context.SaveChanges();
            }

            Repository<Book> repository = CreateNewGenericRepositoryWithNewContext<Book>("dbx");

            var books = repository.GetAllAsync().Result;

            Assert.True(books.Count() == 2);
        }

        //[Fact]
        //public void CanRetrievePagedData_Ascending()
        //{
        //    var books = new List<Book>
        //    {
        //        new Book {Id = 1, Title = "A", DisplayOrder = 2},
        //        new Book {Id = 2, Title = "B", DisplayOrder = 3},
        //        new Book {Id = 3, Title = "C", DisplayOrder = 4},
        //        new Book {Id = 4, Title = "D", DisplayOrder = 1},
        //        new Book {Id = 5, Title = "E", DisplayOrder = 5},
        //        new Book {Id = 6, Title = "F", DisplayOrder = 6}
        //    };

        //    var mockContext = new Mock<NexusContext>();
        //    mockContext.Setup(nex => nex.Set<Book>()).Returns(TestHelpers.MockDbSet(books));

        //    var repository = new Repository<Book>(mockContext.Object);

        //    var page1 = repository.GetPaged(t => t.DisplayOrder, 1, 2).ToList();
        //    Assert.Equal(2, page1.Count);
        //    Assert.Equal(page1.First().Id, 4);
        //    Assert.Equal(page1.Last().Id, 1);


        //    var page2 = repository.GetPaged(t => t.DisplayOrder, 2, 2).ToList();
        //    Assert.Equal(2, page1.Count);
        //    Assert.Equal(page2.First().Id, 2);
        //    Assert.Equal(page2.Last().Id, 3);
        //}

        [Fact]
        public void CanInsertBook()
        {
            var bookGenericRepository = CreateNewGenericRepositoryWithNewContext<Book>("a.db");

            Book book = GetBooks(initIdField: false).First();

            bookGenericRepository.Add(book);
            bookGenericRepository.UnitOfWork.SaveChanges();

            using (var c = new NexusContext(InMemoryHelpers.CreateOptions("a.db")))
            {
                var recentlySavedBook = c.Books.FirstOrDefault();

                Assert.True(recentlySavedBook != null);
            }
        }

        [Fact]
        public void CanInsertRangeOfBooks()
        {
            var genericRepository = CreateNewGenericRepositoryWithNewContext<Book>("x.db");
            genericRepository.Add(GetBooks());
            genericRepository.UnitOfWork.SaveChanges();

            var savedBooks = CreateNewGenericRepositoryWithNewContext<Book>("x.db").GetAll();

            Assert.True(savedBooks.Count() != 0);
        }

        [Fact]
        public void CanDeleteBook()
        {
            int bookId;
            string databaseName = Guid.NewGuid().ToString();
            string presudoTitle = Guid.NewGuid().ToString();
            using (var context = new NexusContext(InMemoryHelpers.CreateOptions(databaseName)))
            {
                var entity = new Book {Title = presudoTitle};

                context.Books.Add(entity);
                context.SaveChanges();

                bookId = entity.Id;
            }

            using (var context = new NexusContext(InMemoryHelpers.CreateOptions(databaseName)))
            {
                var repository = new Repository<Book>(context);
                var book = new Book() {Id = bookId};

                repository.Delete(book);
                repository.UnitOfWork.SaveChanges();
            }

            using (var context = new NexusContext(InMemoryHelpers.CreateOptions(databaseName)))
            {
                var repository = new Repository<Book>(context);
                Book b = repository.GetFiltered(book => book.Id == bookId && book.Title == presudoTitle)
                    .FirstOrDefault();

                Assert.Null(b);
            }
        }

        [Fact]
        public void CanDeleteRangeOfBooks()
        {
            var dbName = Guid.NewGuid().ToString();
            var genericRepository = CreateNewGenericRepositoryWithNewContext<Book>(dbName);

            genericRepository.Add(GetBooks());
            genericRepository.UnitOfWork.SaveChanges();

            var newRepo = CreateNewGenericRepositoryWithNewContext<Book>(dbName);

            var recentlyInsertedBooks = newRepo.GetAll();

            newRepo.Delete(recentlyInsertedBooks);
            newRepo.UnitOfWork.SaveChanges();

            Assert.Empty(newRepo.GetAll().ToArray());
        }

        [Fact]
        public void CanUpdateBook()
        {
            string databaseName = Guid.NewGuid().ToString();
            using (var context = new NexusContext(InMemoryHelpers.CreateOptions(databaseName)))
            {
                context.Books.AddRange(GetBooks());
                context.SaveChanges();
            }

            var repository = CreateNewGenericRepositoryWithNewContext<Book>(databaseName);
            ;

            Book book = repository.Get(1);
            book.Title = "UPDATED";
            repository.Update(book);
            repository.UnitOfWork.SaveChanges();

            Assert.Equal("UPDATED", repository.Get(book.Id).Title);
        }

        [Fact]
        public void CanUpdateRangeOfBooks()
        {
            string databaseName = Guid.NewGuid().ToString();
            using (var context = new NexusContext(InMemoryHelpers.CreateOptions(databaseName)))
            {
                context.Books.AddRange(GetBooks());
                context.SaveChanges();
            }

            var updateRepository = CreateNewGenericRepositoryWithNewContext<Book>(databaseName);
            
            var savedBooks = updateRepository.GetAll().ToList();
            foreach (var savedBook in savedBooks)
            {
                savedBook.Title = "MODIFIED";
            }

            updateRepository.Update(savedBooks);
            updateRepository.UnitOfWork.SaveChanges();

            using (var context = new NexusContext(InMemoryHelpers.CreateOptions(databaseName)))
            {
                foreach (var book in context.Books.ToList())
                {
                    Assert.Equal("MODIFIED", book.Title);
                }
            }
        }

        [Fact]
        public void GetFiltered_CanQueryWithSinglePredicate()
        {
            NexusContext context = new NexusContext(InMemoryHelpers.CreateOptions(Guid.NewGuid().ToString()));
            context.Books.Add(new Book() {Author = "auth 1"});
            context.SaveChanges();

            Repository<Book> bookGenericRepository = new Repository<Book>(context);
            var books = bookGenericRepository.GetFiltered(b => b.Author.Contains("auth")).ToList();

            Assert.True(books.Any());
        }

        [Fact]
        public void GetFiltered_CanQueryWithMultiplePredicate()
        {
            NexusContext context = new NexusContext(InMemoryHelpers.CreateOptions(Guid.NewGuid().ToString()));

            var b1 = new Book() {Author = "auth", Title = "book 1"};
            var b2 = new Book() {Author = "auth", Title = "book 2"};
            context.Books.Add(b1);
            context.Books.Add(b2);
            context.SaveChanges();

            Repository<Book> bookGenericRepository = new Repository<Book>(context);
            var books = bookGenericRepository.GetFiltered(b => b.Author.Contains("auth") && b.Title == "book 1")
                .ToList();

            Assert.True(books.Count == 1);
        }

        //[Fact]
        //public void CanInclude()
        //{
        //    var data = new List<Work>();
        //    data.Add(new Work 
        //    {
        //        Skills = new List<Skill> {
        //            new Skill(), new Skill()
        //        }
        //    });

        //    var mockContext = new Mock<NexusContext>();
        //    mockContext
        //        .Setup(context => context.Set<Work>())
        //        .Returns(TestHelpers.MockDbSet<Work>(data));

        //    var repository = new Repository<Work>(mockContext.Object);

        //    var result = repository.AllInclude(t => t.Skills);

        //    Assert.Equal(2, result.First().Skills.Count);           
        //}

        //[Fact]
        //public void CanIncludeAndFilter()
        //{
        //    var data = new List<Work>()
        //    {
        //        new Work
        //        {
        //            Id = 1,
        //            Skills = new List<Skill>
        //            {
        //                new Skill(),
        //                new Skill()
        //            }
        //        },
        //        new Work {Id = 2},
        //        new Work {Id = 3}
        //    };

        //    var mockContext = new Mock<NexusContext>();
        //    mockContext
        //        .Setup(context => context.Set<Work>())
        //        .Returns(TestHelpers.MockDbSet<Work>(data));

        //    var repository = new Repository<Work>(mockContext.Object);

        //    var result = repository.AllIncludeFiltered(t => t.Id == 1, t => t.Skills);

        //    Assert.IsNotNull(result.FirstOrDefault());
        //    Assert.Equal(2, result.First().Skills.Count);
        //}

        public static List<Book> GetBooks(bool initIdField = true)
        {
            var booksInMemory = new List<Book>
            {
                new Book
                {
                    Id = 1, Author = "author 1", Title = "book 1", Pages = 1, IsVisible = true,
                    DisplayOrder = 1, YearFinished = 2001, ReadingStatus = ReadingStatus.Read,
                    CoverImage = null, PublicationYear = 2001,
                    ReadingStatusId = (int) ReadingStatus.Read
                },
                new Book
                {
                    Id = 2, Author = "author 2", Title = "book 2", Pages = 2, IsVisible = true,
                    DisplayOrder = 2, YearFinished = 2002, ReadingStatus = ReadingStatus.Read,
                    CoverImage = null, PublicationYear = 2002,
                    ReadingStatusId = (int) ReadingStatus.Read
                }
            };
            return booksInMemory;
        }
    }    
}