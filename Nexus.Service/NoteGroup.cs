using System.Collections.Generic;
using Nexus.Service.DTOs;

namespace Nexus.Service
{
    public class NoteGroup
    {
        public int Year { get; set; }
        public List<NoteDto> Notes { get; set; }
    }

    public class NoteTagGroup
    {
        public int TagId { get; set; }
        public string TagTitle { get; set; }
        public List<NoteDto> NoteDtos { get; set; }
    }

    public class SimpleNote
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
    }
}