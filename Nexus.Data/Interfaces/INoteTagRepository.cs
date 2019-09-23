using System.Collections.Generic;
using Nexus.Core;
using Nexus.Core.Entities;

namespace Nexus.Data.Interfaces
{
    public interface INoteTagRepository : IRepository<NoteTag>
    {
        IEnumerable<NoteTag> GetNoteTagsByNoteId(int noteId);
    }
}