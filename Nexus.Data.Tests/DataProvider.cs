using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nexus.Core.Entities;

namespace Nexus.Data.Tests
{
    public static class DataProvider
    {
        public static Note CreateNote(bool initIdField = false)
        {
            var note = new Note
            {
                Id = initIdField ? 1 : 0,
                Title = "Note 1",
                Slug = "not-1",
                CreationDate = DateTime.Today,
                IsVisible = true
            };

            return note;
        }

        public static Category CreateCategory(CategoryType type = CategoryType.Default, bool initIdField = false)
        {
            return new Category
            {
                Id = initIdField ? 1 : 0,
                Title = "Category 1",
                CategoryTypeId = (int)type,
                ParentId = 0
            };
        }

        public static Course CreateCourse(int? noteCategoryId, int? categoryId, bool initIdField = false)
        {
            return new Course
            {
                Id = initIdField ? 1 : 0,
                Title = "Course 1",
                DisplayOrder = 1,
                CategoryId = categoryId
            };
        }

        public static List<Note> CreateNotes(bool initIdField = false, int count = 6)
        {
            return Enumerable.Range(1, count).Select(noteNumber => new Note
            {
                Id = initIdField ? noteNumber : default(int),
                Title = $"Note {noteNumber}",
                CreationDate = DateTime.Now,
            }).ToList();
        }

        public static List<Tag> CreateTags(bool initField = false, int count = 6)
        {
            return Enumerable.Range(1, count).Select(counter => new Tag
            {
                Id = initField ? counter : default(int),
                Title = $"Tag {counter}"
            }).ToList();
        }

        public static List<Tag> GetAlphabeticalTags()
        {
            var tags = new List<Tag>
            {
                new Tag() {Id = 0, Title = "A"},
                new Tag() {Id = 0, Title = "B"},
                new Tag() {Id = 0, Title = "C"},
            };

            return tags;
        }
    }
}
