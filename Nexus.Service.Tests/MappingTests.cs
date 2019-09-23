using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Nexus.Core.Entities;
using Nexus.Service.DTOs;
using Nexus.Tests;
using Xunit;

namespace Nexus.Service.Tests
{
    public class MappingTests
    {
        private readonly IRuntimeMapper _mapper;
        public MappingTests()
        {
            _mapper = MapperHelper.CreateNewMapperInstance();
        }

        [Fact]
        public void NoteHasNoteTags()
        {
            var note = new Note { Id = 1, Title = "Note 1" };
            var noteTags = new List<NoteTag>()
            {
                new NoteTag
                {
                    Id = 1,
                    NoteId = 1,
                    TagId = 1,
                    Tag = new Tag
                    {
                        Id = 1,
                        Title = "Tag 1"
                    },
                },
                new NoteTag
                {
                    Id = 1,
                    NoteId = 1,
                    TagId = 2,
                    Tag = new Tag
                    {
                        Id = 2,
                        Title = "Tag 2"
                    },
                }
            };

            note.NoteTags = noteTags;

            NoteDto dto = _mapper.Map<Note, NoteDto>(note);
            Assert.True(dto.NoteTags.Count == 2);

            Assert.Contains(dto.NoteTags, ntd => ntd.NoteId == 1 && ntd.TagId == 1 && ntd.Title == "Tag 1");
            Assert.Contains(dto.NoteTags, ntd => ntd.NoteId == 1 && ntd.TagId == 2 && ntd.Title == "Tag 2");
        }

        [Fact]
        public void NoteHasNoteCategories()
        {
            Note note = new Note { Id = 1, Title = "Note 1" };
            
            var noteCategories = new List<NoteCategory>
            {
                new NoteCategory
                {
                    NoteId = 1,
                    CategoryId = 1,
                    Category = new Category
                    {
                        Id = 1,
                        Title = "Category 1"
                    }
                },
                new NoteCategory
                {
                    NoteId = 1,
                    CategoryId = 2,
                    Category = new Category
                    {
                        Id = 2,
                        Title = "Category 2"
                    }
                }
            };
            note.NoteCategories = noteCategories;

            NoteDto dto = _mapper.Map<Note, NoteDto>(note);

            Assert.True(dto.NoteCategories.Count == 2);
            Assert.Contains(dto.NoteCategories, 
                noteCatDto => noteCatDto.NoteId == 1 && noteCatDto.CategoryId == 1 && noteCatDto.CategoryTitle == "Category 1");
            Assert.Contains(dto.NoteCategories, 
                noteCatDto => noteCatDto.NoteId == 1 && noteCatDto.CategoryId == 2 && noteCatDto.CategoryTitle == "Category 2");
        }
    }
}