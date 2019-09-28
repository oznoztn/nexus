using Nexus.Core.Entities;
using Nexus.Service.Interfaces;

namespace Nexus.Service.DTOs
{
    public class ProjectPictureDto : IDto, IEntity
    {
        public int Id { get; set; }
        public int ProjectId { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsVisible { get; set; }
        public string Title { get; set; }
        public string Alt { get; set; }
        public string Caption { get; set; }
        public string FileName { get; set; }
        public byte[] Binary { get; set; }
    }    
}