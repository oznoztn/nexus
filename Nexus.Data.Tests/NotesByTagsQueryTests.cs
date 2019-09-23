using System.Collections.Generic;
using System.Linq;
using Moq;
using Nexus.Core.Entities;
using Nexus.Data.Helpers;
using Nexus.Data.Repositories;
using Xunit;

namespace Nexus.Data.Tests
{
    public class NotesByTagsQueryTests
    {
        private NotesByTagsQuery CreateQuery(NexusContext context, int[] tags)
        {
            return new NotesByTagsQuery(context, tags);
        }

        [Fact]
        public void Query_CanQueryWithSingleTagId_OneNoteAssignedToGivenTagId()
        {
            var options = InMemoryHelpers.CreateOptions();
            using (var context = new NexusContext(options))
            {
                List<Note> notes = DataProvider.CreateNotes(initIdField: true, count: 3);
                List<Tag> tags = DataProvider.CreateTags(initField: true, count: 3);
                List<NoteTag> noteTags = new List<NoteTag> { new NoteTag() { Id = 1, TagId = 1, NoteId = 1 } };

                context.Notes.AddRange(notes);
                context.Tags.AddRange(tags);
                context.NoteTags.AddRange(noteTags);

                context.SaveChanges();
            }

            using (var context = new NexusContext(options))
            {
                int[] tagIds = { 1 };
                NotesByTagsQuery query = CreateQuery(context, tagIds);

                List<Note> resultList = query.Query().ToList();

                Assert.Single(resultList);
                Assert.Contains(resultList, note => note.Id == 1 && note.Title == "Note 1");
            }
        }

        [Fact]
        public void Query_CanQueryWithSingleTagId_NoNoteAssignedToGivenTagId()
        {
            var options = InMemoryHelpers.CreateOptions();
            using (var context = new NexusContext(options))
            {
                List<Note> notes = DataProvider.CreateNotes(initIdField: true, count: 3);
                List<Tag> tags = DataProvider.CreateTags(initField: true, count: 3);
                List<NoteTag> noteTags = new List<NoteTag> { new NoteTag { Id = 1, TagId = 1, NoteId = 1 } };

                context.Notes.AddRange(notes);
                context.Tags.AddRange(tags);
                context.NoteTags.AddRange(noteTags);

                context.SaveChanges();
            }

            using (var context = new NexusContext(options))
            {
                int[] tagIds = { 3 };
                NotesByTagsQuery query = CreateQuery(context, tagIds);

                List<Note> resultList = query.Query().ToList();

                Assert.Empty(resultList);
            }
        }

        [Fact]
        public void Query_CanQueryWithMultipleTagIds_()
        {
            var options = InMemoryHelpers.CreateOptions();
            using (var context = new NexusContext(options))
            {
                List<Note> notes = DataProvider.CreateNotes(initIdField: true, count: 3);
                List<Tag> tags = DataProvider.CreateTags(initField: true, count: 3);
                List<NoteTag> noteTags = new List<NoteTag>
                {
                    new NoteTag { TagId = 2, NoteId = 2 },
                    new NoteTag { TagId = 3, NoteId = 3 }
                };

                context.Notes.AddRange(notes);
                context.Tags.AddRange(tags);
                context.NoteTags.AddRange(noteTags);

                context.SaveChanges();
            }

            using (var context = new NexusContext(options))
            {
                int[] tagIds = { 2, 3 };
                NotesByTagsQuery query = CreateQuery(context, tagIds);

                List<Note> resultList = query.Query().ToList();

                Assert.Equal(2, resultList.Count);

                Assert.Contains(resultList, note => note.Id == 2 && note.Title == "Note 2");
                Assert.Contains(resultList, note => note.Id == 3 && note.Title == "Note 3");

                Assert.DoesNotContain(resultList, note => note.Id == 1 && note.Title == "Note 1");
            }
        }

        [Fact]
        public void Query_QueryingWithEmptyTagsList_ShouldReturnAnEmptyListOfNotes()
        {
            var options = InMemoryHelpers.CreateOptions();
            using (var context = new NexusContext(options))
            {
                List<Note> notes = DataProvider.CreateNotes(initIdField: true, count: 3);
                List<Tag> tags = DataProvider.CreateTags(initField: true, count: 3);
                List<NoteTag> noteTags = new List<NoteTag>()
                {
                    new NoteTag() { Id = 1, TagId = 1, NoteId = 1 },
                    new NoteTag() { Id = 2, TagId = 2, NoteId = 2 },
                    new NoteTag() { Id = 3, TagId = 3, NoteId = 3 }
                };

                context.Notes.AddRange(notes);
                context.Tags.AddRange(tags);
                context.NoteTags.AddRange(noteTags);

                context.SaveChanges();
            }

            using (var context = new NexusContext(options))
            {
                int[] tagIds = { };
                NotesByTagsQuery query = CreateQuery(context, tagIds);
                List<Note> resultList = query.Query().ToList();
                Assert.Empty(resultList);
            }
        }

        [Fact]
        public void ExtendQuery_ExtendingQueryWithEmptyTagsList_ShouldReturnAnEmptyListOfNotes()
        {
            var options = InMemoryHelpers.CreateOptions();
            using (var context = new NexusContext(options))
            {
                List<Note> notes = DataProvider.CreateNotes(initIdField: true, count: 3);
                List<Tag> tags = DataProvider.CreateTags(initField: true, count: 3);
                List<NoteTag> noteTags = new List<NoteTag>()
                {
                    new NoteTag() { Id = 1, TagId = 1, NoteId = 1 },
                    new NoteTag() { Id = 2, TagId = 2, NoteId = 2 },
                    new NoteTag() { Id = 3, TagId = 3, NoteId = 3 }
                };

                context.Notes.AddRange(notes);
                context.Tags.AddRange(tags);
                context.NoteTags.AddRange(noteTags);

                context.SaveChanges();
            }

            using (var context = new NexusContext(options))
            {
                int[] tagIds = { };
                NotesByTagsQuery query = CreateQuery(context, tagIds);

                List<Note> resultList = query.ExtendQuery(context.Notes.AsQueryable()).ToList();

                Assert.Empty(resultList);
            }
        }
    }
}