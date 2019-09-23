using System;
using System.Collections.Generic;
using System.Linq;
using Nexus.Core.Entities;

namespace Nexus.Data.Tests.Helpers
{
    public class Seeder
    {
        private readonly NexusContext _context;

        public Seeder(NexusContext context)
        {
            _context = context;
        }

        public void SeedBooks()
        {
            var books = GetBooks();
            _context.Books.AddRange(books);
            _context.SaveChanges();
        }
        public void SeedCategories()
        {
            var categories = new List<Category>()
            {
                new Category { Title = "Machine Learning"},
                new Category { Title = "Deep Learning"},
                new Category { Title = "Data Structures & Algorithms"},
                new Category { Title = "Design Patterns"},
                new Category { Title = "Unit Testing"},
                new Category { Title = "C#" },
                new Category { Title = "JavaScript" },
                new Category { Title = "jQuery" },
                new Category { Title = "ASP.NET Core MVC" },
                new Category { Title = "ASP.NET MVC" },
                new Category { Title = "ASP.NET Web API 2" },
                new Category { Title = "ASP.NET Web API" },
                new Category { Title = "Entity Framework Core 2" },
                new Category { Title = "Entity Framework 6" },
                new Category { Title = "ADO.NET" },
                new Category { Title = "Windows Presentation Foundation" },
                new Category { Title = "Windows Communication Foundation" },
                new Category { Title = "Telerik UI" },
                new Category { Title = "Telerik UI ASP.NET MVC" },                
            };

            int displayOrder = 0;
            foreach (var category in categories)
            {
                category.DisplayOrder = ++displayOrder;
            }

            _context.AddRange(categories);
            _context.SaveChanges();
        }

        public void SeedSampleNotes()
        {
            Category programming = _context.Categories.First(t => t.Title == "C#");
            Category machineLearning = _context.Categories.First(t => t.Title == "Machine Learning");
            Category designPatterns = _context.Categories.First(t => t.Title == "Design Patterns");


            _context.Notes.AddRange(PopulateSampleNotes(programming, 30));
            _context.Notes.AddRange(PopulateSampleNotes(machineLearning, 20));
            _context.Notes.AddRange(PopulateSampleNotes(designPatterns, 10));

            _context.SaveChanges();
        }

        private List<Note> PopulateSampleNotes(Category category, int n)
        {
            var notes = new List<Note>();

            for (int i = 1; i <= n; i++)
            {
                notes.Add(new Note() {
                    Title = $"{category.Title} Sample Note {i}",
                    Slug = $"{category.Slug}-sample-note-{i}",
                    LastUpdateDate = DateTime.Now.AddHours(i),
                    Abstract = "abstract",
                    Content = "content",
                    IsVisible = true,
                    CreationDate = DateTime.Now.AddHours(i),
                    NoteCategories = new List<NoteCategory>()
                    {
                        new NoteCategory()
                        {
                            CategoryId = category.Id
                        }
                    }
                });
            }

            return notes;
        }

        public List<Book> GetBooks()
        {
            Book won = new Book
            {
                Author = "Adam Smith",
                Title = "Wealth of Nations",
                DisplayOrder = 1,
                IsVisible = true,
                Pages = 754,
                ReadingStatusId = (int)ReadingStatus.CurrentlyReading,
                YearFinished = null,
                PublicationYear = 1776
            };

            Book reminiscences = new Book
            {
                Author = "Edwin Lefèvre",
                Title = "Reminiscences of a Stock Operator Book",
                DisplayOrder = 2,
                IsVisible = true,
                Pages = 288,
                ReadingStatusId = (int)ReadingStatus.Read,
                YearFinished = 2017,
                PublicationYear = 1923
            };

            Book codeFirst = new Book
            {
                Author = "Julie Lerman & Rowan Miller",
                Title = "Programming Entity Framework: Code First",
                DisplayOrder = 3,
                IsVisible = true,
                Pages = 192,
                ReadingStatusId = (int)ReadingStatus.Read,
                YearFinished = 2016,
                PublicationYear = 2012
            };

            Book dbContext = new Book
            {
                Author = "Julie Lerman & Rowan Miller",
                Title = "Programming Entity Framework: DbContext",
                DisplayOrder = 4,
                IsVisible = true,
                Pages = 256,
                ReadingStatusId = (int)ReadingStatus.Read,
                YearFinished = 2016,
                PublicationYear = 2012
            };

            Book coreMvc2 = new Book
            {
                Author = "Adam Freeman",
                Title = "Pro ASP.NET Core MVC 2",
                DisplayOrder = 5,
                IsVisible = true,
                Pages = 1024,
                ReadingStatusId = (int)ReadingStatus.CurrentlyReading,
                YearFinished = null,
                PublicationYear = 2017
            };

            Book artOfUnitTesting = new Book
            {
                Author = "Roy Osherove",
                Title = "The Art of Unit Testing",
                DisplayOrder = 6,
                IsVisible = true,
                Pages = 294,
                ReadingStatusId = (int)ReadingStatus.CurrentlyReading,
                YearFinished = 2016,
                PublicationYear = 2014
            };

            Book nutShell = new Book
            {
                Author = "Ben Albahari & Joseph Albahari",
                Title = "C# 6.0 in a Nutshell",
                DisplayOrder = 7,
                IsVisible = true,
                Pages = 1136,
                ReadingStatusId = (int)ReadingStatus.CurrentlyReading,
                YearFinished = null,
                PublicationYear = 2015
            };

            Book cpp = new Book
            {
                Author = "Jesse Liberty & Bradley Jones",
                Title = "Teach Yourself C++ in 21 Days 5th Edition",
                DisplayOrder = 8,
                IsVisible = true,
                Pages = 936,
                ReadingStatusId = (int)ReadingStatus.Read,
                YearFinished = 2014,
                PublicationYear = 2005
            };

            var books = new List<Book>
            {
                won,
                reminiscences,
                codeFirst,
                dbContext,
                coreMvc2,
                artOfUnitTesting,
                nutShell,
                cpp
            };

            return books;
        }

        
    }
}