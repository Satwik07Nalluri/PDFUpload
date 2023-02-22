using Microsoft.AspNetCore.Mvc;

namespace PDFUpload
{
    public class SaveFile
    {
        public async Task<ActionResult<string>> save(IFormFile file, string fileLoc)
        {
            
            using (FileStream fileStream = System.IO.File.Create(fileLoc))
            {
                await file.OpenReadStream().CopyToAsync(fileStream);
            }
            return "Success";
        }
    }
}
