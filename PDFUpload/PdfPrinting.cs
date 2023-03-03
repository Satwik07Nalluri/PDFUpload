using ceTe.DynamicPDF.Merger;
using Path = System.IO.Path;

namespace PDFUpload
{
    public class PdfPrinting
    {
        public async Task<string> multiplepdfprinting(IFormFile pdffile,string file_loc, PdfProperties pdfProperties)
        {
            try
            {
                //Creating individual PDFs
                for (int i = 0; i < pdfProperties.Pdfprops.Count; i++)
                {
                    string pdf_loc = Path.Combine(file_loc, $"{i + 1}{Path.GetExtension(pdffile.FileName)}");
                    SaveFile saving = new SaveFile();
                    await saving.save(pdffile, pdf_loc);
                }

                //Writing Data to individual PDFs
                for (int i = 0; i < pdfProperties.Pdfprops.Count; i++)
                {
                    string pdfloc = Path.Combine(file_loc, $"{i + 1}{Path.GetExtension(pdffile.FileName)}");
                    MergeDocument source = new MergeDocument(@pdfloc);
                    foreach (var (k, v) in pdfProperties.GetPdfpropsofPage(i + 1))
                    {
                        source.Form.Fields[k].Value = v;
                    }
                    source.Draw(pdfloc);
                }
                return "Success";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Error Occured while Printing Individual PDFs");
                return "Unsuccessful";
            }
        }
    }
}
