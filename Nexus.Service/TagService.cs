using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Nexus.Core;
using Nexus.Core.Entities;
using Nexus.Data;
using Nexus.Data.Interfaces;
using Nexus.Service.DTOs;
using Nexus.Service.Extensions;
using Nexus.Service.GenericService;
using Nexus.Service.ServiceInterfaces;

namespace Nexus.Service
{
    public class TagService : ITagService
    {
        private readonly NexusContext _context;
        private readonly ITagRepository _tagRepository;
        private readonly IMapper _mapper;

        public TagService(IMapper mapper, NexusContext context, ITagRepository tagRepository)
        {
            _context = context;
            _tagRepository = tagRepository;
            _mapper = mapper;
        }

        public TagDto GetById(int id)
        {
            var tag = _context.Tags.FirstOrDefault(t => t.Id == id);
            var dto = _mapper.Map<TagDto>(tag);
            return dto;
        }

        public TagDto GetBySlug(string tagSlug)
        {
            var tag = _tagRepository.GetBySlug(tagSlug);
            var dto = _mapper.Map<TagDto>(tag);
            return dto;
        }

        public TagDto GetByTitle(string title)
        {
            var tag = _tagRepository.GetByTitle(title);

            if (tag != null)
            {
                var tagDto = _mapper.Map<TagDto>(tag);
                return tagDto;
            }

            return null;
        }

        public IEnumerable<TagDto> GetAll()
        {
            var tags = _context.Tags.OrderByDescending(tag => tag.Title).ToList();
            var dtos = _mapper.Map<List<TagDto>>(tags);
            return dtos;
        }

        public IEnumerable<Tuple<TagDto, int>> GetTopNTagsAlongWithUsageInfo(bool includeHiddenTags, bool countInvisibleNotes, int n = int.MaxValue)
        {
            var tuples = _tagRepository.GetTopNTagsWithUsageInfo(n, includeHiddenTags, countInvisibleNotes).ToList();

            var dtuples = new List<Tuple<TagDto, int>>();
            foreach (Tuple<Tag, int> tuple in tuples)
            {
                var tagDto = _mapper.Map<TagDto>(tuple.Item1);
                dtuples.Add(new Tuple<TagDto, int>(tagDto, tuple.Item2));
            }

            //var dtuples = _mapper.Map<IEnumerable<Tuple<Tag, int>>>(tuples);
            return dtuples.AsEnumerable();
        }

        public IEnumerable<TagDto> GetTopNTagsWithAtLeastOneNote(int n, bool includeHiddenTags)
        {
            IEnumerable<Tag> tags = _tagRepository.GetTopNTagsWithAtLeastOneNote(n, includeHiddenTags);
            var dtos = _mapper.Map<IEnumerable<TagDto>>(tags);
            return dtos;
        }

        //public PagedDtoList<TagNoteCountDto> GetTagsWithNoteCount(int page, int pageSize)
        //{
        //    IQueryable<NoteTag> noteTags = _context.NoteTags.AsNoTracking().AsQueryable();
        //    IQueryable<Tag> tags = _context.Tags.AsNoTracking().AsQueryable();

        //    var query =
        //        from ntag in noteTags
        //        join tag in tags on ntag.TagId equals tag.Id
        //        group ntag by new { tag.Id, tag.Title } into grup
        //        select new TagNoteCountDto
        //        {
        //            Id = grup.Key.Id,
        //            Title = grup.Key.Title,
        //            NoteCount = grup.Count()
        //        };

        //    //return new PagedList<TagNoteCountDto>(query, page, pageSize);
        //    return null;
        //}

        public void Update(TagDto tagDto)
        {
            if(tagDto == null)
                throw new ArgumentNullException(nameof(tagDto));

            Tag tag = _context.Tags.Find(tagDto.Id);
            tag = _mapper.Map(tagDto, tag);

            _context.SaveChanges();
        }

        public void Delete(TagDto tagDto)
        {
            if (tagDto == null)
                throw new ArgumentNullException(nameof(tagDto));

            Tag tag = _context.Tags.FirstOrDefault(t => t.Id == tagDto.Id);
            
            if (tag != null)
            {
                _context.Tags.Remove(tag);
                _context.SaveChanges();
            }
        }
    }
}
