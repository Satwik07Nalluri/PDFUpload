using ceTe.DynamicPDF.Merger;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IO;
namespace PDFUpload
{
    public class PdfMerging
    {
        public async Task<ActionResult> Mergepdf(string file_loc, PdfProperties pdfProperties)
        {
            string pdfloc = Path.Combine(file_loc, "1.pdf");
            MergeDocument source = new MergeDocument(@pdfloc);
            for (int i = 1; i < pdfProperties.Pdfprops.Count; i++)
            {
                source.Append($"{i + 1}.pdf");
            }
            source.Draw(pdfloc);
            var bytes = await File.ReadAllBytesAsync(pdfloc);
            var returnValue = ControllerBase.File(bytes, "application/pdf", Path.GetFileName(pdfloc));
            Directory.Delete(file_loc, true);
            return returnValue;

        }
    }
}
