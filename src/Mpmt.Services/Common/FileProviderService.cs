using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Mpmt.Core.Configuration;
using Mpmt.Services.Logging;

namespace Mpmt.Services.Common
{
    public class FileProviderService : IFileProviderService
    {
        private readonly IOptions<StaticContentConfig> _staticContentConfig;
        private readonly IExceptionLogger _exceptionLogger;

        public FileProviderService(
            IOptions<StaticContentConfig> staticContentConfig,
            IExceptionLogger exceptionLogger)
        {
            _staticContentConfig = staticContentConfig;
            _exceptionLogger = exceptionLogger;
        }

        public async Task<(bool isSuccess, string filePath)> UploadFileAsync(string dirRelativePath, IFormFile file)
        {
            if (string.IsNullOrWhiteSpace(dirRelativePath) || file is null)
                return (false, default);

            try
            {
                var baseDir = Path.TrimEndingDirectorySeparator(_staticContentConfig.Value.UserDataDirectory);
                string uploadDirPath = $"{baseDir}\\{dirRelativePath}";

                if (!Directory.Exists(uploadDirPath))
                    Directory.CreateDirectory(uploadDirPath);

                string fileName = $"{DateTime.UtcNow:yyyyMMddHHmmssffff}_{Guid.NewGuid():N}.{(Path.GetExtension(file.FileName) ?? string.Empty).Trim('.')}";

                var fileFullPath = Path.Combine(uploadDirPath, fileName);
                await using (var fileStream = new FileStream(fileFullPath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                    fileStream.Flush();
                }

                string returnPath = $"/{Uri.UnescapeDataString(Path.Combine(dirRelativePath, fileName).Replace('\\', '/')).TrimStart('/')}";
                return (true, returnPath);
            }
            catch (Exception ex)
            {
                await _exceptionLogger.LogAsync(ex);
                return (false, default);
            }
        }

        public bool TryUploadFile(IFormFile file, string dirRelativePath, out string relativeFileUrl)
        {
            relativeFileUrl = null;

            if (string.IsNullOrWhiteSpace(dirRelativePath) || file is null)
                return false;

            try
            {
                var baseDir = Path.TrimEndingDirectorySeparator(_staticContentConfig.Value.UserDataDirectory);
                string uploadDirPath = $"{baseDir}\\{dirRelativePath}";

                if (!Directory.Exists(uploadDirPath))
                    Directory.CreateDirectory(uploadDirPath);

                string fileName = $"{DateTime.UtcNow:yyyyMMddHHmmssffff}_{Guid.NewGuid():N}.{(Path.GetExtension(file.FileName) ?? "jpg").Trim('.')}";
                var fileFullPath = Path.Combine(uploadDirPath, fileName);

                using var fileStream = new FileStream(fileFullPath, FileMode.Create);
                file.CopyTo(fileStream);
                fileStream.Flush();

                relativeFileUrl = $"/{Uri.UnescapeDataString(Path.Combine(dirRelativePath, fileName).Replace('\\', '/')).TrimStart('/')}";
                return true;
            }
            catch (Exception ex)
            {
                _exceptionLogger.LogAsync(ex).Wait();
                return false;
            }
        }

        public void DeleteFile(string filePath)
        {
            string fileFullPath = filePath;
            if (!IsAbsoluteUrl(fileFullPath))
            {
                var baseDir = Path.TrimEndingDirectorySeparator(_staticContentConfig.Value.UserDataDirectory);
                fileFullPath = $"{baseDir}\\{fileFullPath?.Trim('/')}";
            }

            if (!File.Exists(fileFullPath))
                return;

            File.Delete(fileFullPath);
        }

        public bool IsAbsoluteUrl(string url)
        {
            return Uri.IsWellFormedUriString(url, UriKind.Absolute);
        }
    }
}
