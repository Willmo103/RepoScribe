using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RepoScribe.Core.Database.Entities
{
    public abstract class ContentItemEntity
    {
        [Key]
        public int Id { get; set; }
        public string Path { get; set; }
        public string Owner { get; set; }
        public DateTime LastModified { get; set; }
        public double SizeMB { get; set; }
        public string Language { get; set; }
        public string Content { get; set; }
        public string Domain { get; set; }
        public string ContextSource { get; set; }
    }
}
