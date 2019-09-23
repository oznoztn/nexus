using System;
using System.Collections.Generic;
using Nexus.Core.Entities;
using Nexus.Service.DTOs;
using Nexus.Service.GenericService;
using Nexus.Shared;
using Nexus.Shared.Enums;

namespace Nexus.Service.ServiceInterfaces
{
    public interface INoteService : IService<Note, NoteDto>
    {
        string GetNoteContent(int noteId);
        bool UpdateNoteContent(int noteId, string content);
        DateTime[] GetNoteDates();
        NoteDto Get(int id, bool honorVisibilityRule);
        IEnumerable<NoteCategoryDto> GetNoteCategories(int noteId);
        PagedDtoList<NoteDto> FindNotesByTagSlug(Visibility noteVisibility, string tagTitle, int pageNumber, int pageSize);
        PagedDtoList<NoteDto> FindNotes(string noteTitle, int[] categories, int[] tags, int pageNumber, int pageSize);
        PagedDtoList<NoteDto> GetPaged(int page, int pageSize);
        PagedDtoList<NoteDto> GetPagedDescending(int page, int pageSize);
        PagedDtoList<NoteDto> GetNotes(int year, int month, int page, int pageSize);
        PagedDtoList<NoteDto> GetNotes(Visibility noteVisibility, bool includeFuturePosts, int page, int pageSize);
        PagedDtoList<NoteDto> GetNotesByCategorySlug(Visibility noteVisibility, string categorySlug, int page, int pageSize);
    }
}