using System;
using System.Collections.Generic;
using Microsoft.Extensions.Caching.Memory;

namespace RepoScribe.Core.DataModels
{
    public class ClusterIndex
    {
        public string ClusterId { get; set; }
        public Guid ClusterIndexId { get; set; }
        public List<Guid> MemberEmbeddings { get; set; }
    }
}
