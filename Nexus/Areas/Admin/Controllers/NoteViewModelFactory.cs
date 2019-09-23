using System;
using System.Linq;
using AutoMapper;
using Nexus.Areas.Admin.Models;
using Nexus.Service.DTOs;
using Nexus.Service.ServiceInterfaces;

namespace Nexus.Areas.Admin.Controllers
{
    public class NoteViewModelFactory
    {
        private readonly IMapper _mapper;
        private readonly INoteService _noteService;
        private readonly ITagService _tagService;

        public NoteViewModelFactory(IMapper mapper, INoteService noteService, ITagService tagService)
        {
            _mapper = mapper;
            _noteService = noteService;
            _tagService = tagService;
        }

        public NoteViewModel Create()
        {
            return new NoteViewModel
            {
                IsVisible = false,
                CreationDate = DateTime.Now,
                LastUpdateDate = null,
                Tags = string.Empty,
                AvailableTags = _tagService.GetAll().Select(t => t.Title).ToArray()
            };
        }

        public NoteViewModel Create(int noteId)
        {
            var noteDto = _noteService.Get(noteId);

            if (noteDto == null)
                return null;

            var vm = _mapper.Map<NoteViewModel>(noteDto);

            vm.AvailableTags = _tagService.GetAll().Select(t => t.Title).ToArray();

            return vm;
        }

        public void PrepareTags(NoteViewModel vm)
        {
            vm.AvailableTags = _tagService.GetAll().Select(t => t.Title).ToArray();
        }
    }
}