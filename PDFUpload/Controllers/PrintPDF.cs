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
            // Getting Path for the file
            string? currentRoute = System.IO.Directory.GetCurrentDirectory();

            // Folder that holds all temporary folders
            string? tempFolderRoute = System.IO.Path.Combine(currentRoute, "TemporaryFolders");
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
            PdfPrinting pdfprint = new PdfPrinting();
            await pdfprint.multiplepdfprinting(pdf, files_loc, pdfProperties);

            //Merging PDFs
            PdfMerging merging = new PdfMerging();
            merging.Mergepdf(files_loc, pdfProperties);

            // COnvert pdf file to bytes
            var bytes = await System.IO.File.ReadAllBytesAsync(System.IO.Path.Combine(files_loc, "destpdf.pdf"));
            Directory.Delete(files_loc, true);

            return File(bytes, "application/pdf", "Output.pdf");
        }
    }
}