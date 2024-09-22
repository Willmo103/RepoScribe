using System.Reflection.PortableExecutable;
using System.Text;
using CodeFlattener.Core.DataModels;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;

namespace CodeFlattener.Core.FileHandlers
{
    public class PdfFileHandler : IFileHandler
    {
        private readonly List<string> _supportedExtensions = new List<string> { ".pdf" };

        public bool CanHandle(string extension)
        {
            return _supportedExtensions.Contains(extension.ToLower());
        }

        public FileMetadata ProcessFile(string filePath)
        {
            var fileInfo = new FileInfo(filePath);
            string content = ExtractTextFromPdf(filePath);

            var lines = content.Split('\n')
                .Select((line, index) => new LineContent { Number = index + 1, Content = line.Trim() })
                .ToList();

            return new FileMetadata
            {
                Path = filePath,
                Owner = fileInfo.GetAccessControl()
                    .GetOwner(typeof(System.Security.Principal.NTAccount)).ToString(),
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
                    // Note: PdfSharp doesn't support text extraction out of the box.
                    // You might need to use a different library like PDFBox or iText7 for robust text extraction.
                    // Here, we'll return a placeholder.
                    text.AppendLine($"[Text extraction not implemented for {filePath}]");
                }
            }
            return text.ToString();
        }
    }
}
