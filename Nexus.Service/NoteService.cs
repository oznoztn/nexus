using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Nexus.Core.Entities;
using Nexus.Data;
using Nexus.Data.Interfaces;
using Nexus.Data.Repositories;
using Nexus.Service.DTOs;
using Nexus.Service.Extensions;
using Nexus.Service.GenericService;
using Nexus.Service.Helpers;
using Nexus.Service.ServiceInterfaces;
using Nexus.Shared;
using Nexus.Shared.Enums;

namespace Nexus.Service
{
    public class NoteService : Service<Note,NoteDto>, INoteService
    {
        private readonly IMapper _mapper;
        private readonly INoteRepository _noteRepository;
        private readonly ICategoryRepository _categoryRepository;

        public NoteService(
            IMapper mapper, 
            INoteRepository noteRepository, 
            ICategoryRepository categoryRepository) : base(noteRepository, mapper)
        {
            _mapper = mapper;
            _noteRepository = noteRepository;
            _categoryRepository = categoryRepository;
        }

        public override NoteDto Get(int id)
        {
            return Get(id, true);
        }

        public NoteDto Get(int id, bool honorVisibilityRule)
        {
            var note = _noteRepository.Get(id);

            if (note == null)
                return null;

            var previousNote = _noteRepository.GetPreviousNote(note, honorVisibilityRule);
            var nextNote = _noteRepository.GetNextNote(note, honorVisibilityRule);

            var dto = _mapper.Map<NoteDto>(note);
            dto.PreviousNote = _mapper.Map<SlimNoteDto>(previousNote);
            dto.NextNote = _mapper.Map<SlimNoteDto>(nextNote);

            return dto;
        }

        public IEnumerable<NoteCategoryDto> GetNoteCategories(int noteId)
        {
            var notes = _categoryRepository.GetNoteCategories(noteId);
            var noteDtos = _mapper.Map<IEnumerable<NoteCategoryDto>>(notes);
            return noteDtos;
        }

        public DateTime[] GetNoteDates()
        {
            return _noteRepository.GetNoteDates().ToArray();
        }

        public string GetNoteContent(int noteId)
        {
            return _noteRepository.GetNoteContent(noteId);
        }

        public bool UpdateNoteContent(int noteId, string content)
        {
            if (noteId == default(int))
                return false;

            return _noteRepository.UpdateNoteContent(noteId, content);
        }

        public PagedDtoList<NoteDto> FindNotesByTagSlug(Visibility noteVisibility, string tagSlug, int pageNumber, int pageSize)
        {
            var pagedNotes = _noteRepository.FindNotesByTagSlug(noteVisibility, tagSlug, pageNumber, pageSize);

            return new PagedDtoList<NoteDto>(_mapper, pagedNotes);
        }

        public PagedDtoList<NoteDto> FindNotes(string noteTitle, int[] categories, int[] tags, int pageNumber, int pageSize)
        {
            var pagedNotes = _noteRepository.FindNotes(noteTitle, categories, tags, pageNumber, pageSize);

            return new PagedDtoList<NoteDto>(_mapper, pagedNotes);
        }

        public PagedDtoList<NoteDto> GetPaged(int page, int pageSize)
        {
            var pagedNotes = _noteRepository.GetPaged(note => note.CreationDate, page, pageSize);

            return new PagedDtoList<NoteDto>(_mapper, pagedNotes);
        }

        public PagedDtoList<NoteDto> GetPagedDescending(int page, int pageSize)
        {
            var pagedNotes = _noteRepository.GetPagedDescending(note => note.CreationDate, page, pageSize);

            return new PagedDtoList<NoteDto>(_mapper, pagedNotes);
        }

        public PagedDtoList<NoteDto> GetNotes(int year, int month, int page, int pageSize)
        {
            var pagedNotes = _noteRepository.GetNotes(year, month, page, pageSize);

            return new PagedDtoList<NoteDto>(_mapper, pagedNotes);
        }

        public PagedDtoList<NoteDto> GetNotes(Visibility noteVisibility, bool includeFuturePosts, int page, int pageSize)
        {
            var pagedNotes = _noteRepository.GetNotes(noteVisibility, includeFuturePosts, page, pageSize);
            var pagedNoteDtos = new PagedDtoList<NoteDto>(_mapper, pagedNotes);
            return pagedNoteDtos;
        }

        public PagedDtoList<NoteDto> GetNotesByCategorySlug(Visibility noteVisibility, string categorySlug, int page, int pageSize)
        {
            var pagedNotes = _noteRepository.GetNotesByCategorySlug(noteVisibility, categorySlug, page, pageSize);

            return new PagedDtoList<NoteDto>(_mapper, pagedNotes);
        }
        
        public override NoteDto Update(NoteDto dto)
        {
            var note = _mapper.Map<Note>(dto);
            _noteRepository.Update(note);
            _noteRepository.UpdateNoteCategories(note.Id, _mapper.Map<IEnumerable<NoteCategory>>(dto.NoteCategories));
            _noteRepository.UpdateNoteTags(note.Id, dto.NoteTags.Select(nt => nt.Title));
            _noteRepository.UnitOfWork.SaveChanges();
            return dto;
        }

        public override void Add(NoteDto dto)
        {
            var note = _mapper.Map<Note>(dto);

            _noteRepository.Add(note);
            _noteRepository.UnitOfWork.SaveChanges();

            _noteRepository.UpdateNoteTags(note.Id, dto.NoteTags.Select(ntDto => ntDto.Title));
            _noteRepository.UnitOfWork.SaveChanges();

            dto.Id = note.Id;
        }
    }
}