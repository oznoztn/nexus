using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Nexus.Core.Entities;
using Nexus.Data.Helpers;
using Nexus.Data.Repositories;
using Xunit;

namespace Nexus.Data.Tests
{
    public class NoteRepositoryIntegrationTests
    {
        private readonly DbContextOptions<NexusContext> _options;
        public NoteRepositoryIntegrationTests()
        {
            _options = PersistenceHelper.BuildOptions("TestDbConnectionString");
            var context = new NexusContext(PersistenceHelper.BuildOptions("TestDbConnectionString"));
            context.Database.EnsureDeleted();
            context.Database.Migrate();
            context.Dispose();
        }

        [Fact]
        public void UpdateNoteTags_ModifyingExistingNoteTagsByAddingAndRemovingTags()
        {
            // SEEDING
            using (var context = new NexusContext(_options))
            {
                var note = DataProvider.CreateNote().AssignNoteTags(DataProvider.GetAlphabeticalTags());
                context.Notes.Add(note);

                context.SaveChanges();
            }

            using (var context = new NexusContext(_options))
            {
                var repo = new NoteRepository(context);

                // Adding D, removing C
                var note = context.Notes.First();
                repo.UpdateNoteTags(note.Id, new[] {"A", "B", "D"});
                repo.UnitOfWork.SaveChanges();
            }

            using (var context = new NexusContext(_options))
            {
                var note = context.Notes.Include(n => n.NoteTags).ThenInclude(nt => nt.Tag).First();

                var tagTitles = note.NoteTags.Select(nt => nt.Tag.Title).ToList();

                Assert.Contains("A", tagTitles);
                Assert.Contains("B", tagTitles);
                Assert.Contains("D", tagTitles);

                Assert.DoesNotContain("C", tagTitles);
            }
        }

        [Fact]
        public void UpdateNoteTags_RemovingExistingNoteTags()
        {
            // SEEDING
            using (var context = new NexusContext(_options))
            {
                context.Database.EnsureCreated(); // Creates the in-memory SQLite database

                var note = DataProvider.CreateNote().AssignNoteTags(DataProvider.GetAlphabeticalTags());
                context.Notes.Add(note);

                context.SaveChanges();
            }

            using (var context = new NexusContext(_options))
            {
                var repo = new NoteRepository(context);
                var note = repo.GetAll().First();
                repo.UpdateNoteTags(note.Id, new List<string>());
                repo.UnitOfWork.SaveChanges();
            }

            using (var context = new NexusContext(_options))
            {
                var note = context.Notes.Include(n => n.NoteTags).First();

                Assert.False(note.NoteTags.Any());
            }
        }

        [Fact]
        public void UpdateNoteTags_AddingAllTheExistingTagsToAnExistingNoteThatHasNoTags()
        {
            using (var context = new NexusContext(_options))
            {
                var note = DataProvider.CreateNote();
                var tags = DataProvider.GetAlphabeticalTags();

                context.Notes.Add(note);
                context.Tags.AddRange(tags);

                context.SaveChanges();
            }

            using (var context = new NexusContext(_options))
            {
                var noteRepo = new NoteRepository(context);

                var note = noteRepo.GetAll().Single();

                noteRepo.UpdateNoteTags(note.Id, DataProvider.GetAlphabeticalTags().Select(t => t.Title));
                noteRepo.UnitOfWork.SaveChanges();
            }

            using (var context = new NexusContext(_options))
            {
                var note = context.Notes.Include(n => n.NoteTags).ThenInclude(nt => nt.Tag).First();

                Assert.Equal(3, note.NoteTags.Count);

                Assert.Contains(note.NoteTags, nt => nt.Tag.Title == "A");
                Assert.Contains(note.NoteTags, nt => nt.Tag.Title == "B");
                Assert.Contains(note.NoteTags, nt => nt.Tag.Title == "C");
            }
        }

        [Fact]
        public void UpdateNoteTags_AddingBrandNewTagsToAnExistingNoteThatHasNoTags()
        {
            using (var context = new NexusContext(_options))
            {
                var note = DataProvider.CreateNote();

                context.Notes.Add(note);
                context.SaveChanges();
            }

            using (var context = new NexusContext(_options))
            {
                var note = context.Notes.First();

                var repo = new NoteRepository(context);
                var tagsToBeAdded = DataProvider.GetAlphabeticalTags();

                repo.UpdateNoteTags(note.Id, tagsToBeAdded.Select(t => t.Title));
                repo.UnitOfWork.SaveChanges();
            }

            using (var context = new NexusContext(_options))
            {
                var note = context.Notes.Include(n => n.NoteTags).ThenInclude(nt => nt.Tag).First();

                Assert.Equal(3, note.NoteTags.Count);

                Assert.Contains(note.NoteTags, nt => nt.Tag.Title == "A");
                Assert.Contains(note.NoteTags, nt => nt.Tag.Title == "B");
                Assert.Contains(note.NoteTags, nt => nt.Tag.Title == "C");
            }
        }

        [Fact]
        public void UpdateNoteTags_GivenTheSameNoteTagsToUpdateNotesTags()
        {
            using (var context = new NexusContext(_options))
            {
                context.Database.EnsureCreated();

                var note = DataProvider.CreateNote();
                var tags = DataProvider.GetAlphabeticalTags();
                note.AssignNoteTags(tags);

                context.Notes.Add(note);
                context.SaveChanges();
            }

            using (var context = new NexusContext(_options))
            {
                var noteRepo = new NoteRepository(context);

                var note = noteRepo.GetAll().Single();

                noteRepo.UpdateNoteTags(note.Id, DataProvider.GetAlphabeticalTags().Select(t => t.Title));
                noteRepo.UnitOfWork.SaveChanges();
            }

            using (var context = new NexusContext(_options))
            {
                var note = context.Notes.Include(n => n.NoteTags).ThenInclude(nt => nt.Tag).First();

                Assert.Equal(3, note.NoteTags.Count);

                Assert.Contains(note.NoteTags, nt => nt.Tag.Title == "A");
                Assert.Contains(note.NoteTags, nt => nt.Tag.Title == "B");
                Assert.Contains(note.NoteTags, nt => nt.Tag.Title == "C");
            }
        }

        [Fact]
        public void UpdateNoteTags_AddingNewTagBeforeAnExistingTag()
        {
            using (var context = new NexusContext(_options))
            {
                context.Notes.Add(DataProvider.CreateNote());
                context.Tags.AddRange(DataProvider.GetAlphabeticalTags());               
                context.SaveChanges();
            }

            using (var context = new NexusContext(_options))
            {
                var repo = new NoteRepository(context);
                var note = repo.GetAll().First();

                repo.UpdateNoteTags(note.Id, new List<string>() {"X", "A" });
                repo.UnitOfWork.SaveChanges();
            }

            using (var context = new NexusContext(_options))
            {
                var note = context.Notes.Include(n => n.NoteTags).ThenInclude(nt => nt.Tag).First();

                Assert.True(note.NoteTags.Count == 2);
                
                Assert.Contains(note.NoteTags, nt => nt.Tag.Title == "A");
                Assert.Contains(note.NoteTags, nt => nt.Tag.Title == "X");
            }
        }
    }
}