using System;
using System.IO;
using System.Text;
using RepoScribe.Core.Abstractions;
using RepoScribe.Core.ContentItems;
using RepoScribe.Core.DataModels.Markdown;

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
            if (contentItem == null)
                throw new ArgumentNullException(nameof(contentItem));

            var markdown = new StringBuilder();

            if (contentItem is CodeContentItem codeContent)
            {
                var codeBlock = new CodeBlock(codeContent.Language, codeContent.Content);
                if (!string.IsNullOrEmpty(template))
                {
                    markdown.AppendLine(codeBlock.ApplyTemplate(template));
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
                    markdown.AppendLine(codeBlock.ToMarkdown());
                }
            }
            else if (contentItem is ImageContentItem imageContent)
            {
                // TODO: Implement rendering for ImageContentItem
            }
            else if (contentItem is PdfContentItem pdfContent)
            {
                // TODO: Implement rendering for PdfContentItem
            }
            else if (contentItem is RepositoryContentItem repoContent)
            {
                // TODO: Implement rendering for RepositoryContentItem
            }
            else
            {
                // Handle other content types or throw exception
            }

            return markdown.ToString();
        }
    }
}
