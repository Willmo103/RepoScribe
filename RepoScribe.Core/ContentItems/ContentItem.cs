using RepoScribe.Core.Abstractions;
using RepoScribe.Core.DataModels;
using RepoScribe.Core.DataModels.Enums;
using System;
using System.Threading.Tasks;

namespace RepoScribe.Core.ContentItems
{
    public abstract class ContentItem : Metadata
    {
        public Guid Id { get; set; } 
        public Domain Domain { get; set; }
        public ContextualInputSource ContextSource { get; set; }

        // Ingestion and Saving methods
        public virtual void Ingest()
        {
            // Default implementation or throw NotImplementedException
            throw new NotImplementedException();
        }

        public virtual async Task SaveAsync()
        {
            // Default implementation or throw NotImplementedException
            throw new NotImplementedException();
        }

        public virtual void Save()
        {
            // Default implementation or throw NotImplementedException
            throw new NotImplementedException();
        }

        // Rendering methods
        public virtual string RenderAs(IRenderer renderer)
        {
            // Default implementation or throw NotImplementedException
            throw new NotImplementedException();
        }

        public virtual string GetSummary()
        {
            // Default implementation or throw NotImplementedException
            throw new NotImplementedException();
        }
    }
}
