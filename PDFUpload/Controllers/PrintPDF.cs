using Microsoft.AspNetCore.Mvc;


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
            try
            {
                // Getting Path for the file
                string? currentRoute = System.IO.Directory.GetCurrentDirectory();
                _logger.LogInformation($"Path for the file is created");
                // Folder that holds all temporary folders
                string? tempFolderRoute = System.IO.Path.Combine(currentRoute, "TemporaryFolders");
                if(!Directory.Exists(tempFolderRoute))
                {
                    Directory.CreateDirectory(tempFolderRoute);
                    _logger.LogInformation($"Directory is created if doesnot exists in the tempFolderRoute");
                }

                // Creating Object of SaveFile Class that helps us to save files at a given location
                SaveFile saveFile = new SaveFile();
                string files_loc = await saveFile.saveBoth(pdf, json, tempFolderRoute);

                // Get JSON data
                PdfProperties pdfProperties = new PdfProperties(files_loc);

                // Printing Data Extracted to Single Page Pdfs in files_loc folder
                PdfPrinting pdfprint = new PdfPrinting();
                await pdfprint.multiplepdfprinting(pdf, files_loc, pdfProperties);

                //Merging PDFs
                PdfMerging merging = new PdfMerging();
                merging.Mergepdf(files_loc, pdfProperties);

                // Convert pdf file to bytes
            
                var bytes = await System.IO.File.ReadAllBytesAsync(System.IO.Path.Combine(files_loc, "destpdf.pdf"));
                Directory.Delete(files_loc, true);

                return File(bytes, "application/pdf", "Output.pdf");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                _logger.LogError("Error Occured while converting final pdf to bytes");
                return NotFound();
            }
        }
    }
}