namespace RepoScribe.Core.DataModels.Markdown
{
    public class Header : MarkdownContent
    {
        public int Level { get; set; }
        public string Text { get; set; }

        public Header(int level, string text)
        {
            Level = level;
            Text = text;
        }

        public override string ToMarkdown()
        {
            return $"{new string('#', Level)} {Text}\n";
        }

        public override string ApplyTemplate(string template)
        {
            return string.Format(template, ToMarkdown());
        }
    }
}
