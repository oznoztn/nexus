using System.Collections.Generic;
using System.Linq;
using Moq;
using Nexus.Core.Entities;
using Nexus.Data.Helpers;
using Nexus.Data.Repositories;
using Xunit;

namespace Nexus.Data.Tests
{
    public class TagRepositoryTests
    {
        [Fact]
        public void GetTopNTagsWithUsageInfo()
        {
            var notes = new List<Note>()
            {
                new Note() { Id = 1, IsVisible = true, Title = "N1"},
                new Note() { Id = 2, IsVisible = true, Title = "N2"},
                new Note() { Id = 3, IsVisible = true, Title = "N3"}
            };

            var tags = new List<Tag>()
            {
                new Tag(){ Id = 1, IsHidden = false, Title = "T1"},
                new Tag(){ Id = 2, IsHidden = false, Title = "T2"},
                new Tag(){ Id = 3, IsHidden = false, Title = "T3"},
            };

            var noteTags = new List<NoteTag>()
            {
                new NoteTag(){ NoteId = 1, TagId = 1},
                new NoteTag(){ NoteId = 2, TagId = 2},
                new NoteTag(){ NoteId = 3, TagId = 3},
            };

            var mockContext = new Mock<NexusContext>();
            mockContext.Setup(nex => nex.Set<Note>()).Returns(TestHelpers.MockDbSet(notes));
            mockContext.Setup(nex => nex.Set<Tag>()).Returns(TestHelpers.MockDbSet(tags));
            mockContext.Setup(nex => nex.Set<NoteTag>()).Returns(TestHelpers.MockDbSet(noteTags));

            var tagRepo = new TagRepository(mockContext.Object);

            var result = tagRepo.GetTopNTagsWithUsageInfo(10, true, true).ToList();

            Assert.Equal(3, result.Count);
            Assert.Contains(result, tuple => tuple.Item1.Title == "T1" && tuple.Item2 == 1);
            Assert.Contains(result, tuple => tuple.Item1.Title == "T2" && tuple.Item2 == 1);
            Assert.Contains(result, tuple => tuple.Item1.Title == "T3" && tuple.Item2 == 1);
        }

        [Fact]
        public void GetTopNTagsWithUsageInfo_2()
        {
            var notes = new List<Note>()
            {
                new Note() { Id = 1, IsVisible = true, Title = "N1"},
                new Note() { Id = 2, IsVisible = false, Title = "N2"},
            };

            var tags = new List<Tag>()
            {
                new Tag(){ Id = 1, IsHidden = false, Title = "T1"},
                new Tag(){ Id = 2, IsHidden = false, Title = "T2"},
            };

            var noteTags = new List<NoteTag>()
            {
                new NoteTag(){ NoteId = 1, TagId = 1},
                new NoteTag(){ NoteId = 2, TagId = 2},
            };

            var mockContext = new Mock<NexusContext>();
            mockContext.Setup(nex => nex.Set<Note>()).Returns(TestHelpers.MockDbSet(notes));
            mockContext.Setup(nex => nex.Set<Tag>()).Returns(TestHelpers.MockDbSet(tags));
            mockContext.Setup(nex => nex.Set<NoteTag>()).Returns(TestHelpers.MockDbSet(noteTags));

            var tagRepo = new TagRepository(mockContext.Object);

            var result = tagRepo.GetTopNTagsWithUsageInfo(10, true, false).ToList();

            Assert.Single(result);
            Assert.Contains(result, tuple => tuple.Item1.Title == "T1" && tuple.Item2 == 1);
            Assert.DoesNotContain(result, tuple => tuple.Item1.Title == "T2" && tuple.Item2 == 1);
        }

        [Fact]
        public void GetTopNTagsWithUsageInfo_3()
        {
            var notes = new List<Note>()
            {
                new Note() { Id = 1, IsVisible = true, Title = "N1"},
                new Note() { Id = 2, IsVisible = true, Title = "N2"},
                new Note() { Id = 3, IsVisible = true, Title = "N3"},
                new Note() { Id = 4, IsVisible = true, Title = "N4"},
            };

            var tags = new List<Tag>()
            {
                new Tag(){ Id = 1, IsHidden = false, Title = "T1"},
                new Tag(){ Id = 2, IsHidden = true, Title = "T2"},
            };

            var noteTags = new List<NoteTag>()
            {
                new NoteTag(){ NoteId = 1, TagId = 1},
                new NoteTag(){ NoteId = 2, TagId = 1},
                new NoteTag(){ NoteId = 3, TagId = 2},
                new NoteTag(){ NoteId = 4, TagId = 2},
            };

            var mockContext = new Mock<NexusContext>();
            mockContext.Setup(nex => nex.Set<Note>()).Returns(TestHelpers.MockDbSet(notes));
            mockContext.Setup(nex => nex.Set<Tag>()).Returns(TestHelpers.MockDbSet(tags));
            mockContext.Setup(nex => nex.Set<NoteTag>()).Returns(TestHelpers.MockDbSet(noteTags));

            var tagRepo = new TagRepository(mockContext.Object);

            var result = tagRepo.GetTopNTagsWithUsageInfo(10, false, false);

            Assert.Single(result);
            Assert.Contains(result, tuple => tuple.Item1.Title == "T1" && tuple.Item2 == 2);
            Assert.DoesNotContain(result, tuple => tuple.Item1.Title == "T2");
        }
    }
}