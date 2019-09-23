using System.Collections.Generic;
using Nexus.Core.Entities;
using Nexus.Service.DTOs;
using Nexus.Service.GenericService;

namespace Nexus.Service.ServiceInterfaces
{
    public interface INoteTagService : IService<NoteTag, NoteTagDto>
    {
        IEnumerable<NoteTagDto> GetNoteTagsByNoteId(int noteId);
    }
}