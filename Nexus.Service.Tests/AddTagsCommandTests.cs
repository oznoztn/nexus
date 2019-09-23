using System.Collections.Generic;
using System.Linq;
using Nexus.Data;
using Nexus.Data.Helpers;
using Nexus.Service.Commands;
using Xunit;

namespace Nexus.Service.Tests
{
    public class AddTagsCommandTests
    {
        [Fact]
        public void CanWriteMoreThanOneTag()
        {
            var options = InMemoryHelpers.CreateOptions("CanWriteMoreThanOneTag");

            using (var context = new NexusContext(options))
            {
                var tags = GetTags(3);
                AddTagsCommand command = new AddTagsCommand(context, tags);
                command.Execute();
            }

            using (var context = new NexusContext(options))
            {
                var tags = context.Tags.ToList();

                Assert.True(tags.Count == 3);                
            }
        }

        [Fact]
        public void CanWriteProperlyMoreThanOneTag()
        {
            var options = InMemoryHelpers.CreateOptions("CanWriteProperlyMoreThanOneTag");

            using (var context = new NexusContext(options))
            {
                var tags = GetTags(3);
                AddTagsCommand command = new AddTagsCommand(context, tags);
                command.Execute();
            }

            using (var context = new NexusContext(options))
            {
                var tags = context.Tags.ToList();

                Assert.True(tags.Count == 3);
                Assert.Contains(tags, tag => tag.Title == "Tag 1");
                Assert.Contains(tags, tag => tag.Title == "Tag 2");
                Assert.Contains(tags, tag => tag.Title == "Tag 3");
            }
        }

        [Fact]
        public void ShouldntWriteAnythingWhenEmptyTagListIsGiven()
        {
            var options = InMemoryHelpers.CreateOptions("ShouldntWriteAnythingWhenEmptyTagListIsGiven");

            using (var context = new NexusContext(options))
            {
                var tags = Enumerable.Empty<string>();
                AddTagsCommand command = new AddTagsCommand(context, tags);
                command.Execute();
            }

            using (var context = new NexusContext(options))
            {
                var tags = context.Tags.ToList();
                Assert.False(tags.Any());
            }
        }

        private IEnumerable<string> GetTags(int n)
        {
            var tags = new List<string>();
            for (int i = 0; i < n; i++)
            {
                tags.Add($"Tag {i+1}");
            }

            return tags;
        }
    }
}
