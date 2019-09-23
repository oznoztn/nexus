using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Nexus.Core;
using Nexus.Core.Entities;
using Nexus.Shared;
using Nexus.Shared.Enums;

namespace Nexus.Data.Interfaces
{
    public interface INoteRepository : IRepository<Note>
    {
        string GetNoteContent(int noteId);
        Note GetPreviousNote(Note comparedNote, bool honorVisibilityRule);
        Note GetNextNote(Note comparedNote, bool honorVisibilityRule);
        void UpdateNoteCategories(int noteId, IEnumerable<NoteCategory> noteCategories);
        void UpdateNoteTags(int noteId, IEnumerable<string> updatedTags);
        bool UpdateNoteContent(int noteId, string content);
        IEnumerable<Note> FindNotesByCategories(int[] categories);
        IEnumerable<DateTime> GetNoteDates();
        IPagedList<Note> FindNotes(string noteTitle, int[] categories, int[] tags, int pageNumber, int pageSize);
        IPagedList<Note> GetNotesByCategorySlug(Visibility noteVisibility, string categorySlug, int page, int pageSize);
        IPagedList<Note> GetNotes(int year, int month, int page, int pageSize);
        IPagedList<Note> GetNotes(Visibility noteVisibility, bool includeFuturePosts, int page, int pageSize);
        IPagedList<Note> FindNotesByTag(Visibility noteVisibility, string tagTitle, int pageNumber, int pageSize);
    }
}