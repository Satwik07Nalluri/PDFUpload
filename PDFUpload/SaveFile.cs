using Microsoft.AspNetCore.Mvc;

namespace PDFUpload
{
    public class SaveFile
    {
        public async Task<string> saveBoth(IFormFile pdffile, IFormFile jsonfile, string currentRoute)
        {
            string nowString = DateTime.Now.ToString("MM_dd_HH_mm_ss");

            string route = Path.Combine(currentRoute, "TempLoc_" + nowString);

            Directory.CreateDirectory(route);

            // Saving Pdf Files
            //for (var i = 1; i < 4; i++)
            //{
            //   var pdf_loc = Path.Combine(route, $"{i}{Path.GetExtension(pdffile.FileName)}");
            //    await save(pdffile, pdf_loc);
            //}


            // Saving JSON File
            var jsonloc = Path.Combine(route, $"data{Path.GetExtension(jsonfile.FileName)}");
            await save(jsonfile, jsonloc);

            return route;
        }



        public async Task<ActionResult<string>> save(IFormFile file, string fileLoc)
        {

            using (FileStream fileStream = System.IO.File.Create(fileLoc))
            {
                await file.OpenReadStream().CopyToAsync(fileStream);
            }
            return "Success";
        }

        //public async Task<ActionResult<string>> SavePdf(IFormFile file, string currentRoute)
        //{
        //    using (FileStream fileStream = System.IO.File.Create(fileLoc))
        //    {
        //        await file.OpenReadStream().CopyToAsync(fileStream);
        //    }
        //    return "Success";
        //}

        //public async Task<ActionResult<string>> SaveJson(IFormFile file, string currentRoute)
        //{
        //    using (FileStream fileStream = System.IO.File.Create(fileLoc))
        //    {
        //        await file.OpenReadStream().CopyToAsync(fileStream);
        //    }
        //    return "Success";
        //}
    }
}
