using System.Collections.Generic;

namespace Nexus.Core.Entities
{
    public class Category : IEntity
    {
        public Category()
        {
            IsVisible = true;
        }

        public int Id { get; set; }
        public string Slug { get; set; }
        public int CategoryTypeId { get; set; }
        public int ParentId { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsVisible { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public CategoryType CategoryType { get; set; }
    }

    public enum CategoryType
    {
        Default = 0,
        Book = 1,
        Course = 2,
        Any = 3
    }
}