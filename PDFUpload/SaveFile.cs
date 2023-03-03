using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


namespace PDFUpload
{
    public class SaveFile
    {
        public async Task<string> saveBoth(IFormFile pdffile, IFormFile jsonfile, string currentRoute)
        {
            try
            {
                string nowString = DateTime.Now.ToString("MM_dd_HH_mm_ss");

                string route = Path.Combine(currentRoute, "TempLoc_" + nowString);

                Directory.CreateDirectory(route);

                // Saving JSON File
                var jsonloc = Path.Combine(route, $"data{Path.GetExtension(jsonfile.FileName)}");
                await save(jsonfile, jsonloc);

                return route;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Error Occured while creating Temporary folder");
                return "Unsuccessful";
            }
        }



        public async Task<ActionResult<string>> save(IFormFile file, string fileLoc)
        {
            try
            {
                using (FileStream fileStream = System.IO.File.Create(fileLoc))
                {
                    await file.OpenReadStream().CopyToAsync(fileStream);
                }
                return "Success";
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine("Error Occured while creating File Stream");
                return "Unsuccessful";
            }
        }
    }
}
