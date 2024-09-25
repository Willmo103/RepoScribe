namespace RepoScribe.Core.DataModels.Markdown
{
    public abstract class MarkdownContent : IOutputTemplating
    {
        public abstract string ToMarkdown();

        public override string ToString()
        {
            return ToMarkdown();
        }

        public abstract string ApplyTemplate(string template);
    }
}
