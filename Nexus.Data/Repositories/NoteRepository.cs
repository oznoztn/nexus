using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Nexus.Core;
using Nexus.Core.Entities;
using Nexus.Data.Interfaces;
using Nexus.Shared;
using Nexus.Shared.Enums;

namespace Nexus.Data.Repositories
{
    public static class NoteQueryExtensions
    {
        public static IQueryable<Note> Visible(this IQueryable<Note> query)
        {
            return query.Where(note => note.IsVisible);
        }

        public static IQueryable<Note> Invisible(this IQueryable<Note> query)
        {
            return query.Where(note => !note.IsVisible);
        }
    }

    public class NoteRepository : Repository<Note>, INoteRepository
    {
        public NoteRepository(NexusContext context) : base(context) { }

        public override Note Get(int id)
        {
            return Set
                .AsNoTracking()
                .Include(n => n.NoteCategories).ThenInclude(nc => nc.Category)
                .Include(n => n.NoteTags).ThenInclude(nt => nt.Tag)
                .SingleOrDefault(n => n.Id == id);
        }

        public string GetNoteContent(int noteId)
        {
            return Set.Find(noteId)?.Content ?? "";
        }

        /// <summary>
        /// asdkl aşskdişal ksdişaskiş
        /// </summary>
        /// <param name="updatedEntity"></param>
        public override void Update(Note updatedEntity)
        {
            var entity = Set.Find(updatedEntity.Id);

            entity.Title = updatedEntity.Title;
            entity.Abstract = updatedEntity.Abstract;
            //entity.Content = updatedEntity.Content;
            entity.CreationDate = updatedEntity.CreationDate;
            entity.IsVisible = updatedEntity.IsVisible;
            entity.LastUpdateDate = updatedEntity.LastUpdateDate;
            entity.Slug = updatedEntity.Slug;

            Set.Update(entity);
        }

        public override void Update(IEnumerable<Note> entities)
        {
            foreach (var entity in entities.ToArray())
            {
                this.Update(entity);
            }
        }

        public Note GetPreviousNote(Note comparedNote, bool honorVisibilityRule)
        {
            IQueryable<Note> query = Context.Set<Note>();
            query = honorVisibilityRule ? query.Visible() : query;
            query = query
                .Where(n => n.Id != comparedNote.Id && n.CreationDate < comparedNote.CreationDate)
                .OrderByDescending(n => n.CreationDate);

            return query.FirstOrDefault();
        }

        public Note GetNextNote(Note comparedNote, bool honorVisibilityRule)
        {
            IQueryable<Note> query = Context.Set<Note>();
            query = honorVisibilityRule ? query.Visible() : query;
            query = query
                .Where(n => n.Id != comparedNote.Id && n.CreationDate > comparedNote.CreationDate)
                .OrderBy(n => n.CreationDate);

            return query.FirstOrDefault();
        }

        public void UpdateNoteCategories(int noteId, IEnumerable<NoteCategory> noteCategories)
        {
            var updatedNote = Set.Find(noteId);

            var currentCategories = Context.Set<NoteCategory>().Where(t => t.NoteId == updatedNote.Id).ToList();
            var incomingCategories = noteCategories.ToList();

            // TODO: learn how to implement IEqualityComparer<T>
            var currentIds = currentCategories.Select(t => t.CategoryId).ToList();
            var incomingIds = incomingCategories.Select(t => t.CategoryId).ToList();

            var addedIds = incomingIds.Except(currentIds).ToArray();
            var removedIds = currentIds.Except(incomingIds).ToArray();

            Context.Set<NoteCategory>().AddRange(addedIds.Select(addedCatId => new NoteCategory { CategoryId = addedCatId, NoteId = updatedNote.Id }));
            foreach (var removedId in removedIds)
            {
                var removed = currentCategories.First(t => t.CategoryId == removedId);
                Context.Set<NoteCategory>().Remove(removed);
            }
        }

        public void UpdateNoteTags(int noteId, IEnumerable<string> updatedTags)
        {
            Note note = Set.Find(noteId);

            if (note == null)
                throw new ArgumentException($"No note with given id {noteId}", nameof(note));

            updatedTags = updatedTags.ToArray();

            List<Tag> availableTagEntities = Context.Set<Tag>().ToList();
            List<string> availableTags = availableTagEntities.Select(t => t.Title).ToList();
            
            List<NoteTag> noteTags = Context.Set<NoteTag>().Where(nt => nt.NoteId == noteId).ToList();
            List<string> currentTags =
                (from tag in availableTagEntities join noteTag in noteTags on tag.Id equals noteTag.TagId select tag.Title).ToList();

            var newTags = updatedTags.Except(availableTags).ToArray();
            var removedTags = currentTags.Except(updatedTags).ToArray();
            var existingNewTags = updatedTags.Intersect(availableTags).ToArray();
            
            foreach (var newTag in newTags)
            {
                Tag tag = new Tag { Title = newTag, Slug = Slug.Create(newTag)};
                NoteTag noteTag = new NoteTag { NoteId = note.Id, TagId = tag.Id, Tag = tag, Note = note};

                Context.Set<Tag>().Add(tag);
                Context.Set<NoteTag>().Add(noteTag);
            }

            foreach (var removedTag in removedTags)
            {
                foreach (var noteTag in noteTags)
                {
                    if (noteTag.Tag.Title.Equals(removedTag, StringComparison.InvariantCultureIgnoreCase))
                    {
                        Context.Set<NoteTag>().Remove(noteTag);
                        break;
                    }
                }
            }

            foreach (var existingNewTag in existingNewTags)
            {
                foreach (var availableTag in availableTagEntities)
                {
                    if (existingNewTag.Equals(availableTag.Title, StringComparison.InvariantCultureIgnoreCase))
                    {
                        if (currentTags.Any(existingTag => existingTag == existingNewTag))
                            break;
                            
                        NoteTag noteTag = new NoteTag { NoteId = note.Id, TagId = availableTag.Id, Note = note};
                        note.NoteTags.Add(noteTag);
                        break;
                    }
                }
            }
        }

        public override IPagedList<Note> GetPagedDescending<TS>(Expression<Func<Note, TS>> orderByExpression, int page, int pageSize)
        {
            var query = GetAllInternal(Visibility.All, true, true).OrderByDescending(orderByExpression);
            return new PagedList<Note>(query, page, pageSize);
        }

        //public IPagedList<Note> GetVisibleNotesPaged<TS>(Expression<Func<Note, TS>> orderByExpression, int page, int pageSize, bool isAscending = true)
        //{
        //    IQueryable<Note> query = 
        //        GetAllInternal().Where(note => note.IsVisible && note.CreationDate <= DateTime.Now);
        //    query = isAscending ? query.OrderBy(orderByExpression) : query.OrderByDescending(orderByExpression);

        //    return new PagedList<Note>(query, page, pageSize);
        //}

        public IPagedList<Note> GetNotesByCategorySlug(Visibility noteVisibility, string categorySlug, int page, int pageSize)
        {
            var query =
                from category in Context.Set<Category>()
                join notecat in Context.Set<NoteCategory>() on category.Id equals notecat.CategoryId
                where category.Slug == categorySlug
                join note in GetAllInternal(noteVisibility, true, true) on notecat.NoteId equals note.Id
                orderby note.CreationDate descending
                select note;
            

            return new PagedList<Note>(query, page, pageSize);
        }

        public IPagedList<Note> GetNotes(int year, int month, int page, int pageSize)
        {
            var query =
                from note in GetAllInternal(Visibility.All, true, true)
                where note.CreationDate.Year == year && note.CreationDate.Month == month
                      orderby note.CreationDate descending
                select note;

            return new PagedList<Note>(query, page, pageSize);
        }

        public IPagedList<Note> GetNotes(Visibility noteVisibility, bool includeFuturePosts, int page, int pageSize)
        {
            var query = GetAllInternal(noteVisibility, true, true);

            query = includeFuturePosts ? query : query.Where(n => n.CreationDate <= DateTime.Now);

            query = query.OrderByDescending(n => n.CreationDate);
            
            return new PagedList<Note>(query, page, pageSize);
        }

        public bool UpdateNoteContent(int noteId, string content)
        { 
            Note n = new Note
            {
                Id = noteId,
                Content = content
            };
            Context.Entry(n).Property(np => np.Content).IsModified = true;

            Context.SaveChanges();

            return true;
        }

        public IPagedList<Note> FindNotes(string noteTitle, int[] categories, int[] tags, int pageNumber, int pageSize)
        {
            noteTitle = string.IsNullOrWhiteSpace(noteTitle) ? "" : noteTitle;
            categories = categories ?? Array.Empty<int>();

            var notesByCategoriesQuery = new NotesByCategoriesQuery(Context as NexusContext, categories);
            var notesByTagsQuery = new NotesByTagsQuery(Context as NexusContext, tags);

            IQueryable<Note> notes = Context.Set<Note>().OrderByDescending(n => n.CreationDate);
            notes = categories.Any() ? notesByCategoriesQuery.ExtendQuery(notes) : notes;
            notes = tags.Any() ? notesByTagsQuery.ExtendQuery(notes) : notes;
            notes = string.IsNullOrWhiteSpace(noteTitle) ? notes : from note in notes where note.Title.ToLower().Contains(noteTitle.ToLower()) select note;

            return new PagedList<Note>(notes, pageNumber, pageSize);
        }

        public IEnumerable<Note> FindNotesByCategories(int[] categories)
        {
            return FindNotesByCategoriesPrivate(categories).AsEnumerable();
        }

        private IQueryable<Note> FindNotesByCategoriesPrivate(int[] categories)
        {
            var notes =
                from note in Context.Set<Note>()
                join ncat in Context.Set<NoteCategory>().Where(nc => categories.Any(id => id == nc.CategoryId)) on note.Id equals ncat.NoteId
                orderby note.CreationDate descending
                select note;

            return notes;
        }

        private IQueryable<Note> GetAllInternal(Visibility noteVisibilityRule, bool includeTags, bool includeCategories)
        {
            IQueryable<Note> query;
            switch (noteVisibilityRule)
            {
                case Visibility.Visible:
                    query = Context.Set<Note>().Visible();
                    break;
                case Visibility.Hidden:
                    query = Context.Set<Note>().Invisible();
                    break;
                default:
                    query = Context.Set<Note>();
                    break;
            }

            if (includeTags)
                query = query.Include(n => n.NoteCategories).ThenInclude(nc => nc.Category);

            if (includeCategories)
                query = query.Include(n => n.NoteTags).ThenInclude(nc => nc.Tag);

            return query;
        }

        public IEnumerable<DateTime> GetNoteDates()
        {
            return
                Context.Set<Note>()
                    .OrderByDescending(note => note.CreationDate)
                    .Select(note => note.CreationDate);
        }

        public IPagedList<Note> FindNotesByTag(Visibility noteVisibility, string tagSlug, int pageNumber, int pageSize)
        {
            var query =
                from tag in Context.Set<Tag>()
                join ntag in Context.Set<NoteTag>() on tag.Id equals ntag.TagId
                join note in GetAllInternal(noteVisibility, true, true) on ntag.NoteId equals note.Id
                where String.Equals(tag.Slug, tagSlug, StringComparison.InvariantCultureIgnoreCase)
                orderby note.CreationDate descending 
                select note;

            return new PagedList<Note>(query, pageNumber, pageSize);
        }
    }

    public class NotesByCategoriesQuery
    {
        private readonly NexusContext _context;
        private readonly int[] _categories;

        public NotesByCategoriesQuery(NexusContext context, int[] categories)
        {
            _context = context;
            _categories = categories;
        }

        public IQueryable<Note> Query()
        {
            var query =
                from note in _context.Set<Note>()
                join ncat in _context.Set<NoteCategory>()
                    .Where(nc => _categories.Any(id => id == nc.CategoryId)) on note.Id equals ncat.NoteId
                orderby note.CreationDate descending
                select note;

            return query;
        }

        public IQueryable<Note> ExtendQuery(IQueryable<Note> notesQuery)
        {
            var extended =
                from note in notesQuery
                join ncat in _context.Set<NoteCategory>()
                    .Where(nc => _categories.Any(id => id == nc.CategoryId)) on note.Id equals ncat.NoteId
                orderby note.CreationDate descending
                select note;

            return extended;
        }

    }

    public class NotesByTagsQuery
    {
        private readonly NexusContext _context;
        private readonly int[] _tags;

        public NotesByTagsQuery(NexusContext context, int[] tags)
        {
            _context = context;
            _tags = tags;
        }

        public IQueryable<Note> Query()
        {
            var query = ExtendQuery(_context.Notes.AsQueryable());

            return query;
        }

        public IQueryable<Note> ExtendQuery(IQueryable<Note> notesQuery)
        {
            var qextended =
                from note in notesQuery
                join nt in _context.Set<NoteTag>() on note.Id equals nt.NoteId
                where _tags.Contains(nt.TagId)
                select note;

            return qextended;
        }
    }

    public class PostReferenceQuery
    {
        private readonly NexusContext _context;
        private readonly Note _note;
        private readonly bool _isNext;

        public PostReferenceQuery(NexusContext context, Note note, bool isNext)
        {
            _context = context;
            _note = note;
            _isNext = isNext;
        }

        public Note[] Execute()
        {
            Note previous = null;
            Note next = null;

            IQueryable<Note> query = _context.Set<Note>().Visible();
            if (_isNext)
            {
                query = query
                    .Where(n => n.Id != _note.Id && n.CreationDate >= _note.CreationDate)
                    .OrderBy(n => n.CreationDate);

                next = query.FirstOrDefault();
            }
            else
            {
                query = query
                    .Where(n => n.Id != _note.Id && n.CreationDate <= _note.CreationDate)
                    .OrderByDescending(n => n.CreationDate);

                previous = query.FirstOrDefault();
            }

            return new Note[] { previous, next};
        }
    }
}