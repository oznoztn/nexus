namespace Nexus.Core.Entities
{
    public class ProjectPicture : IEntity
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
        public Project Project { get; set; }
    }
}