using ceTe.DynamicPDF.Merger;
using ceTe.DynamicPDF.PageElements;
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
        public async Task<ActionResult> PrintPdf(IFormFile pdf, IFormFile json)
        {
            // Getting Path for the file
            string? currentRoute = System.IO.Directory.GetCurrentDirectory();

            // Folder that holds all temporary folders
            string? tempFolderRoute = System.IO.Path.Combine(currentRoute, "TemporaryFodlers");
            if(!Directory.Exists(tempFolderRoute))
            {
                Directory.CreateDirectory(tempFolderRoute);
            }

            // Creating Object of SaveFile Class that helps us to save files at a given location
            SaveFile saveFile = new SaveFile();
            string files_loc = await saveFile.saveBoth(pdf, json, tempFolderRoute);

            // Get JSON data
            PdfProperties pdfProperties = new PdfProperties(files_loc);



            // Printing Data Extracted to Single Page Pdfs in files_loc folder
            //for(int i=1; i <= 3;i++)
            //{
                //foreach(var (k,v) in pdfProperties.GetPdfpropsofPage(i))
                //{
                    //Console.WriteLine($"{k} is {v}");
                //}
            //}

            // Merging Pdfs and Deleting Temp Folder and get File as return value
            PdfPrinting pdfprint= new PdfPrinting();
            pdfprint.multiplepdfprinting(pdf, files_loc, pdfProperties);

            //return File
            return Ok("Success");
            //MergeDocument sourcepdf = new MergeDocument(@pdf_new);
            //var file = System.IO.File.ReadAllText(@json_new);
            //var data = JsonConvert.DeserializeObject<Dictionary<string,string>>(file);

            //var bytes = await System.IO.File.ReadAllBytesAsync(pdf_new);
            //var returnValue = File(bytes, "application/pdf", destination_filename + ".pdf");

            //if (data is not null)
            //{
            //    foreach (var (key, value) in data)
            //    {
            //        sourcepdf.Form.Fields[key].Value = value;

            //    }                string destpdf = Path.Combine(route, destination_filename + ".pdf");
            //    sourcepdf.Draw(@destpdf);
            //    bytes = await System.IO.File.ReadAllBytesAsync(destpdf);
            //    returnValue = File(bytes, "application/pdf", Path.GetFileName(destpdf));
            //    System.IO.File.Delete(@pdf_new);
            //    System.IO.File.Delete(@json_new);
            //    System.IO.File.Delete(destpdf);
            //}
            //return returnValue;

        }
    }
}