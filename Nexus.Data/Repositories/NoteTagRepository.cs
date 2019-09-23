using System;
using System.Collections.Generic;
using System.Linq;
using Nexus.Core.Entities;
using Nexus.Data.Interfaces;

namespace Nexus.Data.Repositories
{
    public class NoteTagRepository : Repository<NoteTag>, INoteTagRepository
    {
        public IEnumerable<NoteTag> GetNoteTagsByNoteId(int noteId)
        {
            return Set.Where(nt => nt.NoteId == noteId).AsEnumerable();
        }
    }
}