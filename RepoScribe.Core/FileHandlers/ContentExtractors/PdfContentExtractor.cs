using RepoScribe.Core.Abstractions;
using RepoScribe.Core.ContentItems;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System.Text;
using RepoScribe.Core.DataModels;

namespace RepoScribe.Core.FileHandlers.ContentExtractors
{
    public class PdfContentExtractor : IContentExtractor
    {
        private readonly List<string> _supportedExtensions = new List<string> { ".pdf" };

        public bool CanExtract(string input)
        {
            var extension = Path.GetExtension(input).ToLower();
            return _supportedExtensions.Contains(extension);
        }

        public ContentItem ExtractContent(string input)
        {
            var fileInfo = new FileInfo(input);
            var owner = fileInfo.GetAccessControl()
                .GetOwner(typeof(System.Security.Principal.NTAccount)).ToString();

            string content = ExtractTextFromPdf(input);

            var lines = content.Split('\n')
                .Select((line, index) => new LineContent { Number = index + 1, Content = line.Trim() })
                .ToList();

            return new PdfContentItem
            {
                Path = input,
                Owner = owner,
                LastModified = fileInfo.LastWriteTime,
                SizeMB = fileInfo.Length / (1024.0 * 1024.0),
                Language = "pdf",
                Content = content,
                Lines = lines
            };
        }

        private string ExtractTextFromPdf(string filePath)
        {
            StringBuilder text = new StringBuilder();
            using (PdfDocument document = PdfReader.Open(filePath, PdfDocumentOpenMode.Import))
            {
                foreach (PdfPage page in document.Pages)
                {
                    // Placeholder for text extraction
                    text.AppendLine($"[Text extraction not implemented for {filePath}]");
                }
            }
            return text.ToString();
        }
    }
}
