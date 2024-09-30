using System.Collections.Generic;

namespace RepoScribe.Core.Database.Entities
{
    public class CodeContentItemEntity : ContentItemEntity
    {
        public ICollection<LineContentEntity> Lines { get; set; } = new List<LineContentEntity>();
    }
}
