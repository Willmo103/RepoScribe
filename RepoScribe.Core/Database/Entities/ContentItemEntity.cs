using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using RepoScribe.Core.DataModels.Enums;

namespace RepoScribe.Core.Database.Entities
{
    public abstract class ContentItemEntity
    {
        [Key]
        public Guid Id { get; set; }
        public string Path { get; set; }
        public string Owner { get; set; }
        public DateTime LastModified { get; set; }
        public double SizeMB { get; set; }
        public string Language { get; set; }
        public string Content { get; set; }
        public Domain Domain { get; set; }
        public ContextualInputSource ContextSource { get; set; }

        // Navigation property
        public ICollection<LineContentEntity> Lines { get; set; } = new List<LineContentEntity>();
    }
}
