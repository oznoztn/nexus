using System.Collections.Generic;
using AutoMapper;
using Nexus.Core.Entities;
using Nexus.Data.Interfaces;
using Nexus.Service.DTOs;
using Nexus.Service.GenericService;
using Nexus.Service.Helpers;
using Nexus.Service.ServiceInterfaces;

namespace Nexus.Service
{
    public class NoteTagService : Service<NoteTag, NoteTagDto>, INoteTagService
    {
        private readonly INoteTagRepository _noteRepository;
        private readonly IMapper _mapper;

        public NoteTagService(IMapper mapper, INoteTagRepository noteRepository) : base(noteRepository, mapper)
        {
            _noteRepository = noteRepository;
            _mapper = mapper;
        }

        public IEnumerable<NoteTagDto> GetNoteTagsByNoteId(int noteId)
        {
            var entities = _noteRepository.GetNoteTagsByNoteId(noteId);

            var dtos = _mapper.Map<IEnumerable<NoteTagDto>>(entities);

            return dtos;
        }
    }
}