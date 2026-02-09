using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMS.BLL.Services.FileServices
{
    public interface IFileService
    {
        public Task<string?> UploadFile(IFormFile file);
    }
}
