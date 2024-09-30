using System.IO;
using System.Text;
using RepoScribe.Core.Abstractions;
using RepoScribe.Core.ContentItems;

namespace RepoScribe.Core.Renderers
{
    public class MarkdownRenderer : ITemplateRenderer
    {
        public string Render(ContentItem contentItem)
        {
            return Render(contentItem, null);
        }

        public string Render(ContentItem contentItem, string template)
        {
            var markdown = new StringBuilder();

            if (contentItem is CodeContentItem codeContent)
            {
                if (template != null)
                {
                    // Apply template rendering
                    markdown.AppendLine(string.Format(template, codeContent.Language, codeContent.Content));
                }
                else
                {
                    markdown.Append("---\n")
                            .AppendLine($"title: {Path.GetFileNameWithoutExtension(codeContent.Path)}")
                            .AppendLine($"language: {codeContent.Language}")
                            .AppendLine($"owner: {codeContent.Owner}")
                            .AppendLine($"lastModified: {codeContent.LastModified}")
                            .AppendLine($"sizeMB: {codeContent.SizeMB}")
                            .AppendLine("---");
                    markdown.AppendLine($"```{codeContent.Language}");
                    markdown.AppendLine(codeContent.Content);
                    markdown.AppendLine("```");
                }
            }
            else if (contentItem is ImageContentItem imageContent)
            {
                if (template != null)
                {
                    // TODO: Apply template rendering for images
                }
                else
                {
                    markdown.AppendLine($"![Image]({imageContent.Path})");
                }
            }
            // TODO: Handle other content types

            return markdown.ToString();
        }
    }
}
