using System;
using System.Collections.Generic;
using Nexus.Service.Interfaces;

namespace Nexus.Service.DTOs
{
    public class ProjectDto : IDto
    {
        private IEnumerable<ProjectPictureDto> _projectPictureDtos;
        public int Id { get; set; }
        public bool IsAvocational { get; set; }
        public bool IsVisible { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }
        public string Abstract { get; set; }
        public string Description { get; set; }
        public string URL { get; set; }
        public byte[] LogoBinary { get; set; }
        public DateTime DateStarted { get; set; }
        public DateTime? DateFinished { get; set; }
        public IEnumerable<ProjectPictureDto> ProjectPictureDtos
        {
            get => _projectPictureDtos ?? (_projectPictureDtos = new List<ProjectPictureDto>());
            set => _projectPictureDtos = value;
        }
    }
}