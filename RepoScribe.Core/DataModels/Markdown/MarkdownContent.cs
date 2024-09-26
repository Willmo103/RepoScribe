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
}
/*
 input_type 
imput_context_source
public Enum ContextualInputSources {
    HostFiles,
    GitHub,
    Browsing History,
    Email,
    Local Files,

}

public enum Domains {
    Protected,
    Personal,
    Interpersonal,
    Professional,
    

public Class Cluster

ClusterIndex {
    public string ClusterId { get; set; }
    public GUID ClusterIndexId { get; set; }
    public List<CacheEntry> CacheEntries { get; set; }
    
}
async multi-shot generation based on nearest neighbor search using domaine of previous questions 
the user Might ask and the context of the current question to generate several responses with diferent 
seeds and allow for reflection in the monent to keep generating better and better responses untell one is generated
and the user grades only the last response generated the chain that lwads to the best response is saved and the
chain that leads to the worst response is discarded the data is used in backpropagation to improve the model imeadiatly

input_context_medium



Domains are top level contexual data classes
Domains {
    public string DomainId { get; set; }
    public string DomainName { get; set; }
}


 */