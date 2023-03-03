using Newtonsoft.Json;

namespace PDFUpload
{
    public class PdfProperties
    {
        public List<Dictionary<string, string>>? Pdfprops { get; set; }
        public string? Watermark { get; set; }
        public string? Adobeversion { get; set; }

        public PdfProperties()
        {

        }

        public PdfProperties(string folder)
        {
            try
            {
                string json_array = System.IO.Path.Combine(folder, "data.json");
                var file = System.IO.File.ReadAllText(json_array);
                var data = JsonConvert.DeserializeObject<PdfProperties>(file);
                Pdfprops = data.Pdfprops;
                Watermark = data.Watermark;
                Adobeversion = data.Adobeversion;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error Occured while extracting JSON file");
            }
        }

        public List<Dictionary<string, string>> GetPdfprops()
        {
            return Pdfprops;
        }

        public Dictionary<string, string> GetPdfpropsofPage(int page_number)
        {
            return Pdfprops[page_number-1];
        }

        public string GetWatermark()
        {
            return Watermark;
        }

        public string GetAdobeversion()
        {
            return Adobeversion;
        }
    }
}
