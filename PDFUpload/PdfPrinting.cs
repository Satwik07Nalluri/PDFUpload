using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Newtonsoft.Json.Linq;
using ceTe.DynamicPDF.Merger;
using ceTe.DynamicPDF.PageElements;
using Microsoft.AspNetCore.Routing;
using Path = System.IO.Path;
using Microsoft.AspNetCore.Mvc;

namespace PDFUpload
{
    public class PdfPrinting
    {
        public async void multiplepdfprinting(IFormFile pdffile,string file_loc, PdfProperties pdfProperties)
        {
            for (int i = 0; i < pdfProperties.Pdfprops.Count; i++)
            {
                string pdf_loc = Path.Combine(file_loc, $"{i + 1}{Path.GetExtension(pdffile.FileName)}");
                SaveFile saving = new SaveFile();
                await saving.save(pdffile, pdf_loc);
            }
            for (int i = 0; i < pdfProperties.Pdfprops.Count; i++)
            {
                string pdfloc = Path.Combine(file_loc, $"{i + 1}.pdf");
                MergeDocument source= new MergeDocument(@pdfloc);
                foreach (var (k, v) in pdfProperties.GetPdfpropsofPage(i+1))
                {
                    source.Form.Fields[k].Value = v;
                }
                source.Draw(pdfloc);
            }
        }
    }
}
