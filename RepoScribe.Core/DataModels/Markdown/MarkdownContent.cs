namespace RepoScribe.Core.DataModels.Markdown
{
    public abstract class MarkdownContent
    {
        public abstract string ToMarkdown();

        public override string ToString()
        {
            return ToMarkdown();
        }

        public abstract string ApplyTemplate(string template);
    }

    /*
    public enum ContextualInputSource
    {
        HostFiles,
        GitHub,
        BrowsingHistory,
        Email,
        LocalFiles,
        SMS
    }

    public enum Domain
    {
        Protected,
        Personal,
        Interpersonal,
        Professional
    }

    public class ClusterIndex
    {
        public string ClusterId { get; set; }
        public Guid ClusterIndexId { get; set; }
        public List<CacheEntry> CacheEntries { get; set; }
    }

    // Additional classes and enums can be defined here
    */
}
/*
async multi-shot generation based on nearest neighbor search using domaine of previous questions 
the user Might ask and the context of the current question to generate several responses with diferent 
seeds and allow for reflection in the monent to keep generating better and better responses untell one is generated
and the user grades only the last response generated the chain that lwads to the best response is saved and the
chain that leads to the worst response is discarded the data is used in backpropagation to improve the model imeadiatly

 */