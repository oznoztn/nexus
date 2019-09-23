using System.Collections.Generic;
using System.Linq;
using Nexus.Core.Entities;

namespace Nexus.Data.Tests
{
    public static class NoteExtensions
    {
        public static Note AssignNoteTags(this Note note, List<Tag> tags)
        {
            note.NoteTags = tags.Select(tag => new NoteTag()
            {
                Tag = tag,
                TagId = tag.Id,
                NoteId = note.Id
            }).ToList();

            return note;
        }
    }
}