using Microsoft.AspNetCore.Http;

namespace Mpmt.Services.Common
{
    public interface IFileProviderService
    {
        Task<(bool isSuccess, string filePath)> UploadFileAsync(string relativefolderPath, IFormFile file);
        bool TryUploadFile(IFormFile file, string dirRelativePath, out string relativeFileUrl);
        void DeleteFile(string filePath);
    }
}
