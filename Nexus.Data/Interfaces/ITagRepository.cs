using System;
using System.Collections.Generic;
using Nexus.Core.Entities;

namespace Nexus.Data.Interfaces
{
    public interface ITagRepository
    {
        Tag GetBySlug(string slug);
        Tag GetByTitle(string title);
        IEnumerable<Tuple<Tag, int>> GetTopNTagsWithUsageInfo(int n, bool includeHiddenTags, bool countInvisibleNotes);
        IEnumerable<Tag> GetTopNTagsWithAtLeastOneNote(int n, bool includeHiddenTags);
    }
}