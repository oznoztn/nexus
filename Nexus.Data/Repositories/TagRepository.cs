using System;
using System.Collections.Generic;
using System.Linq;
using Nexus.Core.Entities;
using Nexus.Data.Interfaces;

namespace Nexus.Data.Repositories
{
    public class TagRepository : Repository<Tag>, ITagRepository
    {
        public TagRepository(NexusContext context) : base(context)
        {
        }
        public Tag GetBySlug(string slug)
        {
            var tag = Set.FirstOrDefault(t => t.Slug == slug);
            return tag;
        }

        public Tag GetByTitle(string title)
        {
            return Set.FirstOrDefault(tag =>
                string.Equals(title, tag.Title, StringComparison.InvariantCultureIgnoreCase));
        }

        public IEnumerable<Tuple<Tag, int>> GetTopNTagsWithUsageInfo(int n, bool includeHiddenTags, bool countInvisibleNotes)
        {
            var tagsQuery = includeHiddenTags ? Context.Set<Tag>() : Context.Set<Tag>().Where(t => !t.IsHidden);
            var notesQuery = countInvisibleNotes ? Context.Set<Note>() : Context.Set<Note>().Where(no => no.IsVisible);

            // LEFT JOIN -silme belki işe yarar
            //var query =
            //    from tag in tagsQuery
            //    join noteTag in _context.Set<NoteTag>() on tag.Id equals noteTag.TagId
            //    join nt in notesQuery on noteTag.NoteId equals nt.Id into jtable
            //    from jrow in jtable.DefaultIfEmpty()
            //    group jrow by tag into grup
            //    orderby grup.Key.Title
            //    select new Tuple<Tag, int>(grup.Key, grup.Count(nt => nt != null));

            var query =
                from tag in tagsQuery
                join noteTag in Context.Set<NoteTag>() on tag.Id equals noteTag.TagId
                join nt in notesQuery on noteTag.NoteId equals nt.Id
                group nt by tag into grup
                orderby grup.Key.Title
                select new Tuple<Tag, int>(grup.Key, grup.Count(nt => nt != null));

            query = query.OrderByDescending(tuple => tuple.Item2).Take(n);

            return query.AsEnumerable();
        }

        public IEnumerable<Tag> GetTopNTagsWithAtLeastOneNote(int n, bool includeHiddenTags)
        {
            var tagsQuery = includeHiddenTags ? Context.Set<Tag>() : Context.Set<Tag>().Where(t => !t.IsHidden);
            var notesQuery = includeHiddenTags ? Context.Set<Note>() : Context.Set<Note>().Where(no => no.IsVisible);

            var query =
                (from tag in tagsQuery
                join noteTag in Context.Set<NoteTag>() on tag.Id equals noteTag.TagId
                join nt in notesQuery on noteTag.NoteId equals nt.Id
                orderby tag.Title
                select tag).Distinct();

            if (n != 0)
                query = query.Take(n);

            return query.AsEnumerable();
        }

        public void GetTagGroup()
        {
            var tags = Context.Set<Tag>();
            var noteTags = Context.Set<NoteTag>();
            var notes = Context.Set<Note>();

            var query =
                from tag in tags join noteTag in noteTags on tag.Id equals noteTag.TagId
                join note in notes on noteTag.NoteId equals note.Id
                group noteTag by noteTag.Tag.Title into tagGroup
                select new
                {
                    TagTitle = tagGroup.Key,
                    NumberOfNotes = tagGroup.Count(),
                    Notes = tagGroup.Select(nt => new Note
                    {
                        Id = nt.NoteId,
                        Title = nt.Note.Title,
                        Slug = nt.Note.Slug
                    }).ToList()
                };

            var result = query.ToList();
        }
    }
}
