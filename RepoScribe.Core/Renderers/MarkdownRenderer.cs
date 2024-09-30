using System.Text;
using RepoScribe.Core.Abstractions;
using RepoScribe.Core.ContentItems;
using RepoScribe.Core.DataModels.Markdown;

namespace RepoScribe.Core.Renderers
{
    public class MarkdownRenderer : IRenderer
    {
        //public void HandleRenderRequest (ContentItem contentItem)
        //{
        //    // Determine the type of content item and render accordingly'
        //    if (contentItem.CanRender("Markdown"))
        //    {
        //        var template = contentItem.FormatTemplate("Markdown");
        //    }
        //    else
        //    {
        //        throw new UnrenderableContent("Content item cannot be rendered as Markdown");
        //    }
        //}

        public string Render(ContentItem contentItem)
        {
            var markdown = new StringBuilder();

            if (contentItem is CodeContentItem codeContent)
            {
                markdown.Append("---\n")
                        // This is a place holder. This will be a section defined and constructed 
                        // in the code item itself. This is just a simple example.
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
            else if (contentItem is ImageContentItem imageContent)
            {
                markdown.AppendLine($"![Image]({imageContent.Path})");
            }
            else if (contentItem is PdfContentItem pdfContent)
            {
                markdown.AppendLine($"## PDF Document: {Path.GetFileName(pdfContent.Path)}");
                markdown.AppendLine(pdfContent.Content);
            }
            else if (contentItem is RepositoryContentItem repoContent)
            {
                markdown.AppendLine($"# Repository: {Path.GetFileName(repoContent.Path)}");
                markdown.AppendLine($"**URL**: {repoContent.Url}");
                markdown.AppendLine($"**Author**: {repoContent.Author}");
                markdown.AppendLine($"**Readme**:\n{repoContent.Readme}");
                markdown.AppendLine("## Files");
                foreach (var file in repoContent.Files)
                {
                    markdown.AppendLine($"- {file.Path} ({file.Language})");
                }
            }
            // Add more content types as needed

            return markdown.ToString();
        }
    }
}
