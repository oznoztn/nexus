using System;
using Nexus.Service;
using Nexus.Service.DTOs;

namespace Nexus.Controllers
{
    public class NoteViewModel
    {
        public PagedDtoList<NoteDto> Notes { get; set; }
        public DateTime[] NoteDates { get; set; }
    }
}