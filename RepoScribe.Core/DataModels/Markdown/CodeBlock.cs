namespace RepoScribe.Core.DataModels.Markdown
{
    public class CodeBlock : MarkdownContent
    {
        public string Language { get; set; }
        public string Content { get; set; }

        public CodeBlock(string language, string content)
        {
            Language = language;
            Content = content;
        }

        public override string ToMarkdown()
        {
            return $"```{Language}\n{Content}\n```\n";
        }

        public override string ApplyTemplate(string template)
        {
            return string.Format(template, ToMarkdown());
        }
    }
}
