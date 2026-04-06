using DocumentFormat.OpenXml.Packaging;
using System.Text;
using UglyToad.PdfPig;

namespace AI.ResumeMatcher.Helpers
{
    public static class FileParser
    {
        public static async Task<string> ExtractText(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return string.Empty;

            var extension = Path.GetExtension(file.FileName).ToLower();

            using var stream = file.OpenReadStream();

            return extension switch
            {
                ".pdf" => ExtractPdfText(stream),
                ".docx" => ExtractDocxText(stream),
                ".txt" => await ExtractPlainText(stream),
                _ => await ExtractPlainText(stream)
            };
        }

        // ================= PDF =================
        private static string ExtractPdfText(Stream stream)
        {
            var text = new StringBuilder();

            using (var document = PdfDocument.Open(stream))
            {
                foreach (var page in document.GetPages())
                {
                    text.AppendLine(page.Text);
                }
            }

            return text.ToString();
        }

        // ================= DOCX =================
        private static string ExtractDocxText(Stream stream)
        {
            var text = new StringBuilder();

            using (var wordDoc = WordprocessingDocument.Open(stream, false))
            {
                var body = wordDoc.MainDocumentPart?.Document?.Body;

                if (body != null)
                {
                    text.AppendLine(body.InnerText);
                }
            }

            return text.ToString();
        }

        // ================= TXT =================
        private static async Task<string> ExtractPlainText(Stream stream)
        {
            using var reader = new StreamReader(stream);
            return await reader.ReadToEndAsync();
        }
    }
}
