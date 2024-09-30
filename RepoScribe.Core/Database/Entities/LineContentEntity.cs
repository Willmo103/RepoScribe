using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RepoScribe.Core.Database.Entities
{
    public class LineContentEntity
    {
        [Key]
        public int Id { get; set; }
        public int Number { get; set; }
        public string Content { get; set; }

        // Foreign key to parent ContentItemEntity
        public int ContentItemEntityId { get; set; }
        public ContentItemEntity ContentItem { get; set; }
    }
}
