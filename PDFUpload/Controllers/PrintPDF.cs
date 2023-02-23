using ceTe.DynamicPDF.Merger;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace PDFUpload.Controllers
{
    [ApiController]
    [Route("/api/printpdf")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public async Task<ActionResult<string>> PrintPdf(IFormFile pdf,IFormFile json,string destination_filename)
        {
            var currentRoute = System.IO.Directory.GetCurrentDirectory();
            string route = Path.Combine(currentRoute, "files");
            var pdf_new = Path.Combine(route,$"{Guid.NewGuid()}{Path.GetExtension(pdf.FileName)}");
            var json_new = Path.Combine(route, $"{Guid.NewGuid()}{Path.GetExtension(json.FileName)}");
            

            SaveFile saveFile = new SaveFile();
            saveFile.save(pdf,pdf_new);
            saveFile.save(json,json_new);
            MergeDocument sourcepdf = new MergeDocument(@pdf_new);
            var file = System.IO.File.ReadAllText(@json_new);
            var data = JsonSerializer.Deserialize<Dictionary<string, string>>(file);

            foreach (var (key, value) in data)
            {
                sourcepdf.Form.Fields[key].Value = value;
            }
            //sourcepdf.Form.Fields["CheckBox1"].Value = "1";
            string destpdf = Path.Combine(route, destination_filename + ".pdf");
            sourcepdf.Draw(@destpdf);
            System.IO.File.Delete(@pdf_new);
            System.IO.File.Delete(@json_new);
            return "Success";

        }
    }
}