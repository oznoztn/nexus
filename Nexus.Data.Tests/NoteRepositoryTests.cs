using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Moq;
using Nexus.Core.Entities;
using Nexus.Data.Helpers;
using Nexus.Data.Repositories;
using Xunit;

namespace Nexus.Data.Tests
{
    public class NoteRepositoryTests
    {
        [Fact]
        public void FindByCategories_Find()
        {
            var mockContext = new Mock<NexusContext>();
            mockContext.Setup(nex => nex.Set<Category>()).Returns(TestHelpers.MockDbSet(new List<Category>()
            {
                new Category() { Id = 1, Title = "Cat 1"},
                new Category() { Id = 2, Title = "Cat 1.1", ParentId = 1},
                new Category() { Id = 3, Title = "Cat 2"},
                new Category() { Id = 4, Title = "Cat 2.1", ParentId = 3}
            }));

            mockContext.Setup(nex => nex.Set<Note>()).Returns(TestHelpers.MockDbSet(new List<Note>()
            {
                new Note() {Id = 1, Title = "Note 1"},
                new Note() {Id = 2, Title = "Note 2"},
                new Note() {Id = 3, Title = "Note 3"},
                new Note() {Id = 4, Title = "Note 4"},
            }));

            mockContext.Setup(nex => nex.Set<NoteCategory>()).Returns(TestHelpers.MockDbSet(new List<NoteCategory>
            {
                new NoteCategory {NoteId = 1, CategoryId = 1 },
                new NoteCategory {NoteId = 2, CategoryId = 2 },
                new NoteCategory {NoteId = 3, CategoryId = 3 },
                new NoteCategory {NoteId = 4, CategoryId = 4 }
            }));

            var repo = new NoteRepository(mockContext.Object);
            var notes = repo.FindNotesByCategories(new[] { 1, 3, 2 }).ToList();

            Assert.Equal(3, notes.Count());

            Assert.Contains(notes, note => note.Id == 1);
            Assert.Contains(notes, note => note.Id == 2);
            Assert.Contains(notes, note => note.Id == 3);
            Assert.DoesNotContain(notes, note => note.Id == 4);
        }

        [Fact]
        public void FindNotes_SearchTermAndOneCategory_ShouldReturnOneNote()
        {
            var mockContext = new Mock<NexusContext>();
            mockContext.Setup(nex => nex.Set<Category>()).Returns(TestHelpers.MockDbSet(new List<Category>()
            {
                new Category() { Id = 1, Title = "Cat 1"},
                new Category() { Id = 2, Title = "Cat 1.1", ParentId = 1},
                new Category() { Id = 3, Title = "Cat 2"},
                new Category() { Id = 4, Title = "Cat 2.1", ParentId = 3}
            }));

            mockContext.Setup(nex => nex.Set<Note>()).Returns(TestHelpers.MockDbSet(new List<Note>()
            {
                new Note() {Id = 1, Title = "Note A"},
                new Note() {Id = 2, Title = "Note B"},
                new Note() {Id = 3, Title = "Note C"},
                new Note() {Id = 4, Title = "Note D"},
            }));

            mockContext.Setup(nex => nex.Set<NoteCategory>()).Returns(TestHelpers.MockDbSet(new List<NoteCategory>
            {
                new NoteCategory {NoteId = 1, CategoryId = 1 },
                new NoteCategory {NoteId = 2, CategoryId = 2 },
                new NoteCategory {NoteId = 3, CategoryId = 3 },
                new NoteCategory {NoteId = 4, CategoryId = 4 }
            }));

            var repo = new NoteRepository(mockContext.Object);

            string userSearchTerm = "Note";
            int[] categories = { 1 };
            int[] tags = Array.Empty<int>();
            int pageNumber = 1;
            int pageSize = 25;
            var notes = repo.FindNotes(userSearchTerm, categories, tags, pageNumber, pageSize).ToList();

            Assert.Contains(notes, note => note.Title == "Note A");
            Assert.Single(notes);
        }

        [Fact]
        public void FindNotes_GivenSearchTermAndCategories_ShouldQueryNotesByTheirTitleAndGivenCategories()
        {
            var mockContext = new Mock<NexusContext>();
            mockContext.Setup(nex => nex.Set<Category>()).Returns(TestHelpers.MockDbSet(new List<Category>()
            {
                new Category() { Id = 1, Title = "Cat 1"},
                new Category() { Id = 2, Title = "Cat 1.1", ParentId = 1},
                new Category() { Id = 3, Title = "Cat 2"},
                new Category() { Id = 4, Title = "Cat 2.1", ParentId = 3}
            }));

            mockContext.Setup(nex => nex.Set<Note>()).Returns(TestHelpers.MockDbSet(new List<Note>()
            {
                new Note() {Id = 1, Title = "Note A"},
                new Note() {Id = 2, Title = "Note B"},
                new Note() {Id = 3, Title = "Note C"},
                new Note() {Id = 4, Title = "Note D"},
            }));

            mockContext.Setup(nex => nex.Set<NoteCategory>()).Returns(TestHelpers.MockDbSet(new List<NoteCategory>
            {
                new NoteCategory {NoteId = 1, CategoryId = 1 },
                new NoteCategory {NoteId = 2, CategoryId = 2 },
                new NoteCategory {NoteId = 3, CategoryId = 3 },
                new NoteCategory {NoteId = 4, CategoryId = 4 }
            }));

            var repo = new NoteRepository(mockContext.Object);

            string userSearchTerm = "Note";
            int[] categories = { 1, 2 };
            int[] tags = Array.Empty<int>();
            int pageNumber = 1;
            int pageSize = 25;
            var notes = repo.FindNotes(userSearchTerm, categories, tags, pageNumber, pageSize).ToList();

            Assert.Contains(notes, note => note.Title == "Note A");
            Assert.Contains(notes, note => note.Title == "Note B");
            Assert.Equal(2, notes.Count);
        }

        [Fact]
        public void FindNotes_NothingGiven_QueryResultShouldContainEverySingleNotes()
        {
            var mockContext = new Mock<NexusContext>();
            mockContext.Setup(nex => nex.Set<Category>()).Returns(TestHelpers.MockDbSet(new List<Category>()
            {
                new Category() { Id = 1, Title = "Cat 1"},
                new Category() { Id = 2, Title = "Cat 2"},
            }));

            mockContext.Setup(nex => nex.Set<Note>()).Returns(TestHelpers.MockDbSet(new List<Note>()
            {
                new Note() {Id = 1, Title = "Note A"},
                new Note() {Id = 2, Title = "Note B"},
                new Note() {Id = 3, Title = "Note C"},
                new Note() {Id = 4, Title = "Note D"},
            }));

            mockContext.Setup(nex => nex.Set<NoteCategory>()).Returns(TestHelpers.MockDbSet(new List<NoteCategory>
            {
                new NoteCategory {NoteId = 1, CategoryId = 1 },
                new NoteCategory {NoteId = 2, CategoryId = 2 },
                new NoteCategory {NoteId = 3 },
                new NoteCategory {NoteId = 4 }
            }));

            var repo = new NoteRepository(mockContext.Object);

            string userSearchTerm = "";
            int[] categories = Array.Empty<int>();
            int[] tags = Array.Empty<int>();
            int pageNumber = 1;
            int pageSize = 25;
            var notes = repo.FindNotes(userSearchTerm, categories, tags, pageNumber, pageSize).ToList();

            Assert.Contains(notes, note => note.Title == "Note A");
            Assert.Contains(notes, note => note.Title == "Note B");
            Assert.Contains(notes, note => note.Title == "Note C");
            Assert.Contains(notes, note => note.Title == "Note D");
        }

        [Fact]
        public void FindNotes_OnlyCategoriesAreGiven_ShouldQueryOnlyByGivenCategories()
        {
            var mockContext = new Mock<NexusContext>();
            mockContext.Setup(nex => nex.Set<Category>()).Returns(TestHelpers.MockDbSet(new List<Category>()
            {
                new Category() { Id = 1, Title = "Cat 1"},
                new Category() { Id = 2, Title = "Cat 1.1", ParentId = 1},
                new Category() { Id = 3, Title = "Cat 2"},
                new Category() { Id = 4, Title = "Cat 2.1", ParentId = 3}
            }));

            mockContext.Setup(nex => nex.Set<Note>()).Returns(TestHelpers.MockDbSet(new List<Note>()
            {
                new Note() {Id = 1, Title = "Note A"},
                new Note() {Id = 2, Title = "Note B"},
                new Note() {Id = 3, Title = "Note C"},
                new Note() {Id = 4, Title = "Note D"},
            }));

            mockContext.Setup(nex => nex.Set<NoteCategory>()).Returns(TestHelpers.MockDbSet(new List<NoteCategory>
            {
                new NoteCategory {NoteId = 1, CategoryId = 1 },
                new NoteCategory {NoteId = 2, CategoryId = 2 },
                new NoteCategory {NoteId = 3, CategoryId = 3 },
                new NoteCategory {NoteId = 4 }
            }));

            var repo = new NoteRepository(mockContext.Object);

            string userSearchTerm = "";
            int[] categories = { 1, 2 };
            int[] tags = Array.Empty<int>();
            int pageNumber = 1;
            int pageSize = 25;
            var notes = repo.FindNotes(userSearchTerm, categories, tags, pageNumber, pageSize).ToList();

            Assert.Contains(notes, note => note.Title == "Note A");
            Assert.Contains(notes, note => note.Title == "Note B");
            Assert.Equal(2, notes.Count);
        }

        [Fact]
        public void FindNotesPaged_SearchTermIsEmpty_ShouldQueryOnlyByGivenCategories()
        {
            var mockContext = new Mock<NexusContext>();
            mockContext.Setup(nex => nex.Set<Category>()).Returns(TestHelpers.MockDbSet(new List<Category>()
            {
                new Category() { Id = 1, Title = "Cat 1"},
                new Category() { Id = 2, Title = "Cat 1.1", ParentId = 1},
                new Category() { Id = 3, Title = "Cat 2"},
                new Category() { Id = 4, Title = "Cat 2.1", ParentId = 3}
            }));

            mockContext.Setup(nex => nex.Set<Note>()).Returns(TestHelpers.MockDbSet(new List<Note>()
            {
                new Note() {Id = 1, Title = "Note A"},
                new Note() {Id = 2, Title = "Note B"},
                new Note() {Id = 3, Title = "Note C"},
                new Note() {Id = 4, Title = "Note D"},
            }));

            mockContext.Setup(nex => nex.Set<NoteCategory>()).Returns(TestHelpers.MockDbSet(new List<NoteCategory>
            {
                new NoteCategory {NoteId = 1, CategoryId = 1 },
                new NoteCategory {NoteId = 2, CategoryId = 2 },
                new NoteCategory {NoteId = 3, CategoryId = 3 },
                new NoteCategory {NoteId = 4, CategoryId = 4 }
            }));

            var repo = new NoteRepository(mockContext.Object);

            string userSearchTerm = "";
            int[] categories = { 1, 2 };
            int[] tags = Array.Empty<int>();
            int pageNumber = 1;
            int pageSize = 25;

            var notes = repo.FindNotes(userSearchTerm, categories, tags, pageNumber, pageSize).ToList();

            Assert.Contains(notes, note => note.Title == "Note A");
            Assert.Contains(notes, note => note.Title == "Note B");
            Assert.Equal(2, notes.Count);
        }

        [Fact]
        public void UpdateNoteTags_ModifyingExistingNoteTagsByAddingAndRemovingTags()
        {
            var options = SQLiteHelpers.CreateOptions();

            // SEEDING
            using (var context = new NexusContext(options))
            {
                context.Database.EnsureCreated(); // Creates the in-memory SQLite database

                var note = DataProvider.CreateNote().AssignNoteTags(DataProvider.GetAlphabeticalTags());
                context.Notes.Add(note);

                context.SaveChanges();
            }

            using (var context = new NexusContext(options))
            {
                var repo = new NoteRepository(context);
                
                // Adding D, removing C
                repo.UpdateNoteTags(1, new []{"A", "B", "D"});
                repo.UnitOfWork.SaveChanges();
            }

            using (var context = new NexusContext(options))
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
            var options = SQLiteHelpers.CreateOptions();

            // SEEDING
            using (var context = new NexusContext(options))
            {
                context.Database.EnsureCreated(); // Creates the in-memory SQLite database

                var note = DataProvider.CreateNote().AssignNoteTags(DataProvider.GetAlphabeticalTags());
                context.Notes.Add(note);

                context.SaveChanges();
            }

            using (var context = new NexusContext(options))
            {
                var repo = new NoteRepository(context);

                repo.UpdateNoteTags(1, new List<string>());
                repo.UnitOfWork.SaveChanges();
            }

            using (var context = new NexusContext(options))
            {
                var note = context.Notes.Include(n => n.NoteTags).First();

                Assert.False(note.NoteTags.Any());                
            }
        }

        [Fact]
        public void UpdateNoteTags_AddingAllTheExistingTagsToAnExistingNoteThatHasNoTags()
        {
            string databaseName = Guid.NewGuid().ToString();
            var options = InMemoryHelpers.CreateOptions(databaseName);

            using (var context = new NexusContext(options))
            {
                var note = DataProvider.CreateNote();
                var tags = DataProvider.GetAlphabeticalTags();

                context.Notes.Add(note);
                context.Tags.AddRange(tags);

                context.SaveChanges();
            }

            using (var context = new NexusContext(options))
            {
                var noteRepo = new NoteRepository(context);

                var note = noteRepo.GetAll().Single();

                noteRepo.UpdateNoteTags(note.Id, DataProvider.GetAlphabeticalTags().Select(t => t.Title));
                noteRepo.UnitOfWork.SaveChanges();
            }

            using (var context = new NexusContext(options))
            {
                var note = context.Notes.Include(n => n.NoteTags).ThenInclude(nt => nt.Tag).First();

                Assert.Equal(3, note.NoteTags.Count);

                Assert.Contains(note.NoteTags, nt => nt.Tag.Title == "A");
                Assert.Contains(note.NoteTags, nt => nt.Tag.Title == "B");
                Assert.Contains(note.NoteTags, nt => nt.Tag.Title == "C");
            }
        }

        [Fact]
        public void UpdateNoteTags_NoteHasNoTags_AddingBrandNewTagsToAnExistingNote()
        {
            string databaseName = Guid.NewGuid().ToString();
            var options = InMemoryHelpers.CreateOptions(databaseName);

            using (var context = new NexusContext(options))
            {
                var note = DataProvider.CreateNote();

                context.Notes.Add(note);
                context.SaveChanges();
            }

            using (var context = new NexusContext(options))
            {
                var repo = new NoteRepository(context);
                var tagsToBeAdded = DataProvider.GetAlphabeticalTags();
                
                var note = repo.GetAll().First();
                repo.UpdateNoteTags(note.Id, tagsToBeAdded.Select(t => t.Title));
                repo.UnitOfWork.SaveChanges();
            }

            using (var context = new NexusContext(options))
            {
                var note = context.Notes.Include(n => n.NoteTags).ThenInclude(nt => nt.Tag).First();

                Assert.Equal(3, note.NoteTags.Count);

                Assert.Contains(note.NoteTags, nt => nt.Tag.Title == "A");
                Assert.Contains(note.NoteTags, nt => nt.Tag.Title == "B");
                Assert.Contains(note.NoteTags, nt => nt.Tag.Title == "C");
            }
        }

        [Fact]
        public void UpdateNoteTags_NoteHasTags_GivenTheSameNoteTagsToUpdateNotesTags()
        {
            var options = SQLiteHelpers.CreateOptions();
            using (var context = new NexusContext(options))
            {
                context.Database.EnsureCreated();
                
                var note = DataProvider.CreateNote();
                var tags = DataProvider.GetAlphabeticalTags();
                note.AssignNoteTags(tags);

                context.Notes.Add(note);
                context.SaveChanges();
            }

            using (var context = new NexusContext(options))
            {
                var noteRepo = new NoteRepository(context);

                var note = noteRepo.GetAll().Single();

                noteRepo.UpdateNoteTags(note.Id, DataProvider.GetAlphabeticalTags().Select(t => t.Title));
                noteRepo.UnitOfWork.SaveChanges();
            }

            using (var context = new NexusContext(options))
            {
                var note = context.Notes.Include(n => n.NoteTags).ThenInclude(nt => nt.Tag).First();

                Assert.Equal(3, note.NoteTags.Count);

                Assert.Contains(note.NoteTags, nt => nt.Tag.Title == "A");
                Assert.Contains(note.NoteTags, nt => nt.Tag.Title == "B");
                Assert.Contains(note.NoteTags, nt => nt.Tag.Title == "C");
            }
        }

        [Fact]
        public void UpdateNoteTags_NoteHasNoTag_AddingNewTagBeforeAnExistingTag()
        {
            var options = SQLiteHelpers.CreateOptions();
            using (var context = new NexusContext(options))
            {
                context.Database.EnsureCreated();

                context.Notes.Add(DataProvider.CreateNote());
                context.Tags.AddRange(DataProvider.GetAlphabeticalTags());
                context.SaveChanges();
            }

            using (var context = new NexusContext(options))
            {
                var repo = new NoteRepository(context);
                var note = repo.GetAll().First();

                repo.UpdateNoteTags(note.Id, new List<string>() { "X", "A" });
                repo.UnitOfWork.SaveChanges();
            }

            using (var context = new NexusContext(options))
            {
                var note = context.Notes.Include(n => n.NoteTags).ThenInclude(nt => nt.Tag).First();

                Assert.True(note.NoteTags.Count == 2);

                Assert.Contains(note.NoteTags, nt => nt.Tag.Title == "A");
                Assert.Contains(note.NoteTags, nt => nt.Tag.Title == "X");

                int totalTagCount = context.Tags.Count();
                Assert.True(totalTagCount == 4);
            }
        }

        [Fact]
        public void UpdateNoteTags_NoteHasTags_AddingNewANDExistingTag()
        {
            var options = SQLiteHelpers.CreateOptions();
            using (var context = new NexusContext(options))
            {
                context.Database.EnsureCreated();

                context.Notes.Add(DataProvider.CreateNote().AssignNoteTags(DataProvider.GetAlphabeticalTags()));
                context.Tags.Add(new Tag() {Title = "D"});
                context.SaveChanges();
            }

            using (var context = new NexusContext(options))
            {
                var repo = new NoteRepository(context);
                var note = repo.GetAll().First();

                repo.UpdateNoteTags(note.Id, new List<string>() { "A", "B", "X", "D" });
                repo.UnitOfWork.SaveChanges();
            }

            using (var context = new NexusContext(options))
            {
                var note = context.Notes.Include(n => n.NoteTags).ThenInclude(nt => nt.Tag).First();

                Assert.True(note.NoteTags.Count == 4);

                Assert.Contains(note.NoteTags, nt => nt.Tag.Title == "A");
                Assert.Contains(note.NoteTags, nt => nt.Tag.Title == "B");
                Assert.Contains(note.NoteTags, nt => nt.Tag.Title == "X");
                Assert.Contains(note.NoteTags, nt => nt.Tag.Title == "D");

                int totalTagCount = context.Tags.Count();
                Assert.True(totalTagCount == 5);
            }
        }

        #region GetPreviousNote & GetNextNote Tests
        [Fact]
        public void GetNextNode_hasnextnode()
        {
            var mockContext = new Mock<NexusContext>();
            mockContext.Setup(nex => nex.Set<Note>()).Returns(TestHelpers.MockDbSet(new List<Note>
            {
                new Note {Id = 1, IsVisible = true, CreationDate = DateTime.Now},
                new Note {Id = 2, IsVisible = true, CreationDate = DateTime.Now.AddDays(1)}
            }));

            var repository = new NoteRepository(mockContext.Object);

            var comparedNote = repository.Get(1);
            var nextNote = repository.GetNextNote(comparedNote, honorVisibilityRule: false);
            
            Assert.NotNull(nextNote);
            Assert.Equal(2, nextNote.Id);
        }

        [Fact]
        public void GetPreviousNotes_haspreviousnote()
        {
            var mockContext = new Mock<NexusContext>();
            mockContext.Setup(nex => nex.Set<Note>()).Returns(TestHelpers.MockDbSet(new List<Note>
            {
                new Note {Id = 1, IsVisible = true, CreationDate = DateTime.Now},
                new Note {Id = 2, IsVisible = true, CreationDate = DateTime.Now.AddDays(1)}
            }));

            var repository = new NoteRepository(mockContext.Object);

            var comparedNote = repository.Get(2);
            var previosNote = repository.GetPreviousNote(comparedNote, false);

            Assert.NotNull(previosNote);
            Assert.Equal(1, previosNote.Id);
        }

        [Fact]
        public void GetNextNode_VisibleOnly_HasNextNotesButTheImmediateNextNoteIsHidden_ShouldOmitTheHiddenOne()
        {
            var mockContext = new Mock<NexusContext>();
            mockContext.Setup(nex => nex.Set<Note>()).Returns(TestHelpers.MockDbSet(new List<Note>
            {
                new Note {Id = 1, IsVisible = true, CreationDate = DateTime.Now},
                new Note {Id = 2, IsVisible = false, CreationDate = DateTime.Now.AddDays(1)},
                new Note {Id = 3, IsVisible = true, CreationDate = DateTime.Now.AddDays(3)}
            }));

            var repository = new NoteRepository(mockContext.Object);

            var comparedNote = repository.Get(1);
            var nextNote = repository.GetNextNote(comparedNote, honorVisibilityRule: true);

            Assert.NotNull(nextNote);
            Assert.Equal(3, nextNote.Id);
        }

        // the immediate prev note is hidden
        [Fact]
        public void GetPreviousNote_VisibleOnly_HasPreviousNotesButTheImmediatePrevNoteIsHidden_ShouldOmitTheHiddenOne()
        {
            var mockContext = new Mock<NexusContext>();
            mockContext.Setup(nex => nex.Set<Note>()).Returns(TestHelpers.MockDbSet(new List<Note>
            {
                new Note {Id = 1, IsVisible = true, CreationDate = DateTime.Now},
                new Note {Id = 2, IsVisible = false, CreationDate = DateTime.Now.AddDays(1)},
                new Note {Id = 3, IsVisible = true, CreationDate = DateTime.Now.AddDays(2)},
            }));

            var repository = new NoteRepository(mockContext.Object);

            var comparedNote = repository.Get(3);
            var previosNote = repository.GetPreviousNote(comparedNote, honorVisibilityRule: true);

            Assert.NotNull(previosNote);
            Assert.Equal(1, previosNote.Id);
        }

        [Fact]
        public void GetNextNote_VisAndInvisNotes_HasNextNote_ShouldTakeTheHiddenNoteIntoAccount()
        {
            var mockContext = new Mock<NexusContext>();
            mockContext.Setup(nex => nex.Set<Note>()).Returns(TestHelpers.MockDbSet(new List<Note>
            {
                new Note {Id = 1, IsVisible = true, CreationDate = DateTime.Now},
                new Note {Id = 2, IsVisible = false, CreationDate = DateTime.Now.AddDays(1)},
                new Note {Id = 3, IsVisible = true, CreationDate = DateTime.Now.AddDays(3)}
            }));

            var repository = new NoteRepository(mockContext.Object);

            var comparedNote = repository.Get(1);
            var nextNote = repository.GetNextNote(comparedNote, honorVisibilityRule: false);

            Assert.Equal(2, nextNote.Id);
        }

        [Fact]
        public void GetPreviousNote_VisAndInvisNotes_HasPrevNote_ShouldTakeTheHiddenNoteIntoAccount()
        {
            var mockContext = new Mock<NexusContext>();
            mockContext.Setup(nex => nex.Set<Note>()).Returns(TestHelpers.MockDbSet(new List<Note>
            {
                new Note {Id = 1, IsVisible = true, CreationDate = DateTime.Now},
                new Note {Id = 2, IsVisible = false, CreationDate = DateTime.Now.AddDays(1)},
                new Note {Id = 3, IsVisible = true, CreationDate = DateTime.Now.AddDays(2)},
            }));

            var repository = new NoteRepository(mockContext.Object);

            var comparedNote = repository.Get(3);
            var previosNote = repository.GetPreviousNote(comparedNote, honorVisibilityRule: false);

            Assert.NotNull(previosNote);
            Assert.Equal(2, previosNote.Id);
            Assert.NotEqual(1, previosNote.Id);
        }

        [Fact]
        public void Test1_VisibleOnly_HasPrevNoteAndNextNoteButTheyreBothHidden_PrevNoteAndNextNodeShouldBeNull()
        {
            var mock = new Mock<NexusContext>();
            mock.Setup(nex => nex.Set<Note>()).Returns(TestHelpers.MockDbSet(new List<Note>
            {
                new Note {Id = 1, IsVisible = false, CreationDate = DateTime.Now},
                new Note {Id = 2, IsVisible = true, CreationDate = DateTime.Now.AddDays(1)},
                new Note {Id = 3, IsVisible = false, CreationDate = DateTime.Now.AddDays(2)},
            }));

            var repository = new NoteRepository(mock.Object);
            var comparedNote = repository.Get(id: 2);

            var prevNote = repository.GetPreviousNote(comparedNote, honorVisibilityRule: true);
            var nextNode = repository.GetNextNote(comparedNote, honorVisibilityRule: true);

            Assert.Null(prevNote);
            Assert.Null(nextNode);
        }
        
        [Fact]
        public void Test1_VisAndInvisNotes_HasNoPrevNoteAndNextNote_PrevNoteAndNextNodeShouldBeNull()
        {
            var mock = new Mock<NexusContext>();
            mock.Setup(nex => nex.Set<Note>()).Returns(TestHelpers.MockDbSet(new List<Note>
            {
                new Note {Id = 1, IsVisible = true, CreationDate = DateTime.Now},
            }));

            var repository = new NoteRepository(mock.Object);

            var comparedNote = repository.Get(id: 1);
            var prevNote = repository.GetPreviousNote(comparedNote, false);
            var nextNote = repository.GetNextNote(comparedNote, false);

            Assert.Null(prevNote);
            Assert.Null(nextNote);
        }
        #endregion
    }
}
