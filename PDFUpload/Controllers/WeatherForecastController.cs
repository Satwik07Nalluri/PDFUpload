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

        [HttpGet("{pdfloc}/{jsonloc}")]
        public async Task<ActionResult<string>> PrintPdf(string pdfloc,string jsonloc)
        {
            MergeDocument sourcepdf = new MergeDocument(@pdfloc);
            var file = System.IO.File.ReadAllText(@jsonloc);
            var data = JsonSerializer.Deserialize<Dictionary<string, string>>(file);

            foreach (var (key, value) in data)
            {
                sourcepdf.Form.Fields[key].Value = value;
            }
            //sourcepdf.Form.Fields["CheckBox1"].Value = "1";
            sourcepdf.Draw(@"C:\Sai_Charan_Work\PrintPDF\Destination.pdf");
            return "Success";
        }
    }
}