using System;
using System.Collections.Generic;

namespace Nexus.Core.Entities
{
    public class Project : IEntity
    {
        public int Id { get; set; }
        public bool IsAvocational { get; set; }
        public bool IsVisible { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Abstract { get; set; }
        public string Description { get; set; }
        public string URL { get; set; }
        public DateTime DateStarted { get; set; }
        public DateTime? DateFinished { get; set; }
        public ICollection<ProjectPicture> ProjectPictures { get; set; }
    }
}