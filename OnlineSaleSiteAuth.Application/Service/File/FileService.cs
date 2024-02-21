using Microsoft.AspNetCore.Http;
using OnlineSaleSiteAuth.Application.Service.File.Dtos;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace OnlineSaleSiteAuth.Application.Service.File
{

    public class FileService : IFileService
    {
        public async Task<ServiceResponse<FileUploadResultDto>> UploadFile(IFormFile file, int maxWidth = 200, int maxHeight = 200)
        {
            if (file.Length > 0)
            {
                var newFileName = Guid.NewGuid().ToString().Replace("-", "");
                newFileName += Path.GetExtension(file.FileName);
                var newFilePath = Path.Combine(Path.GetFullPath("wwwroot/images"), newFileName);
                using (var stream = System.IO.File.Create(newFilePath))
                {
                    await file.CopyToAsync(stream).ConfigureAwait(false);
                }

                return new ServiceResponse<FileUploadResultDto>(new FileUploadResultDto { Name = newFileName, Path = $"/images/{newFileName}" }, true, string.Empty);
            }

            return new ServiceResponse<FileUploadResultDto>(null, false, "File empty");
        }
    }
}
    
    //public class FileService : IFileService
    //{
    //    public async Task<ServiceResponse<FileUploadResultDto>> UploadFile(IFormFile file, int maxWidth = 200, int maxHeight = 200)
    //    {
    //        if (file.Length > 0)
    //        {
    //            var newFileName = Guid.NewGuid().ToString().Replace("-", "");
    //            newFileName += Path.GetExtension(file.FileName);
    //            var newFilePath = Path.Combine(Path.GetFullPath("wwwroot/images"), newFileName);

    //            using (var stream = System.IO.File.Create(newFilePath))
    //            {
    //                await file.CopyToAsync(stream).ConfigureAwait(false);
    //            }

    //            using (var image = Image.Load(newFilePath))
    //            {
    //                image.Mutate(x => x.Resize(maxWidth, maxHeight));
    //                image.Save(newFilePath);
    //            }

    //            return new ServiceResponse<FileUploadResultDto>(new FileUploadResultDto { Name = newFileName, Path = $"/images/{newFileName}" }, true, string.Empty);
    //        }

    //        return new ServiceResponse<FileUploadResultDto>(null, false, "File empty");
    //    }
    //}
//}

