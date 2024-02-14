using Microsoft.AspNetCore.Http;
using OnlineSaleSiteAuth.Application.Service.File.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineSaleSiteAuth.Application.Service.File
{
    public interface IFileService
    {
        Task<ServiceResponse<FileUploadResultDto>> UploadFile(IFormFile file, int maxWidth = 200, int maxHeight = 200);
    }
}
