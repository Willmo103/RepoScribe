using System.Collections.Generic;

namespace RepoScribe.Core.Database.Entities
{
    public class RepositoryContentItemEntity : ContentItemEntity
    {
        public string Url { get; set; }
        public string Author { get; set; }
        public string Readme { get; set; }
        public ICollection<ContentItemEntity> Files { get; set; } = new List<ContentItemEntity>();
    }
}
