using ceTe.DynamicPDF.Merger;

namespace PDFUpload
{
    public class PdfMerging
    {
        public void Mergepdf(string file_loc, PdfProperties pdfProperties)
        {
            string pdfloc = Path.Combine(file_loc, "1.pdf");
            string destpdf = Path.Combine(file_loc, $"destpdf.pdf");
            MergeDocument source = new MergeDocument(@pdfloc);
            for (int i = 1; i < pdfProperties.Pdfprops.Count; i++)
            {
                source.Append(Path.Combine(file_loc, $"{i+1}.pdf"));
            }
            source.Draw(@destpdf);
        }
    }
}
