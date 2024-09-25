using System.Text;

namespace RepoScribe.Core.DataModels.Markdown
{
    public class MarkdownDocument
    {
        private readonly List<MarkdownContent> _contents = new();

        public void AddContent(MarkdownContent content)
        {
            _contents.Add(content);
        }

        public override string ToString()
        {
            var sb = new StringBuilder();
            foreach (var content in _contents)
            {
                sb.Append(content.ToString());
            }
            return sb.ToString();
        }
    }
}
