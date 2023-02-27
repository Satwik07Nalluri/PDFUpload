using ceTe.DynamicPDF.Merger;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;


namespace PDFUpload.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PrintPdfController : ControllerBase
    {
        private readonly ILogger<PrintPdfController> _logger;

        public PrintPdfController(ILogger<PrintPdfController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult> PrintPdf(IFormFile pdf, IFormFile json, string destination_filename)
        {
            var currentRoute = System.IO.Directory.GetCurrentDirectory();
            string route = Path.Combine(currentRoute, "files");
            var pdf_new = Path.Combine(route, $"{Guid.NewGuid()}{Path.GetExtension(pdf.FileName)}");
            var json_new = Path.Combine(route, $"{Guid.NewGuid()}{Path.GetExtension(json.FileName)}");


            SaveFile saveFile = new SaveFile();
            await saveFile.save(pdf, pdf_new);
            await saveFile.save(json, json_new);
            MergeDocument sourcepdf = new MergeDocument(@pdf_new);
            for(var i = 0;i<=2;i++)
            {
                sourcepdf.Append(pdf_new);
            }
            var file = System.IO.File.ReadAllText(@json_new);
            var data = JsonConvert.DeserializeObject<Dictionary<string,string>>(file);

            var bytes = await System.IO.File.ReadAllBytesAsync(pdf_new);
            var returnValue = File(bytes, "application/pdf", destination_filename + ".pdf");

            if (data is not null)
            {
                foreach (var (key, value) in data)
                {
                    sourcepdf.Form.Fields[key].Value = value;

                }                string destpdf = Path.Combine(route, destination_filename + ".pdf");
                sourcepdf.Draw(@destpdf);
                bytes = await System.IO.File.ReadAllBytesAsync(destpdf);
                returnValue = File(bytes, "application/pdf", Path.GetFileName(destpdf));
                System.IO.File.Delete(@pdf_new);
                System.IO.File.Delete(@json_new);
                System.IO.File.Delete(destpdf);
            }
            return returnValue;

        }
    }
}