using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.BLL.Services.FileServices
{
    public class FileService :IFileService
    {
        public async Task<string?> UploadFile(IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Images", fileName);

                using (var stream = File.Create(path))
                {
                    await file.CopyToAsync(stream);
                }
                return fileName;
            }
            return null;
        }
    }
}
