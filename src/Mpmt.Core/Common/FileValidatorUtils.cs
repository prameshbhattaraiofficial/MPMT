using FileSignatures;
using FileSignatures.Formats;
using Microsoft.AspNetCore.Http;

namespace Mpmt.Core.Common
{
    public static class FileValidatorUtils
    {
        public static async Task<(bool, string matchedFileExtension)> IsValidImageAsync(IFormFile file, params string[] fileExtensions)
        {
            if (file is null)
                return (false, null);

            using var ms = new MemoryStream();
            await file.CopyToAsync(ms);

            var inspector = new FileFormatInspector();
            var format = inspector.DetermineFileFormat(ms);

            if (!fileExtensions.Any())
                return (format is Image, format.Extension);

            return (format is Image && fileExtensions.Contains(format.Extension, StringComparer.OrdinalIgnoreCase), format.Extension);
        }

        public static bool TryValidateImage(IFormFile file, out string matchedFileExtension, params string[] fileExtensions)
        {
            matchedFileExtension = null;

            if (file is null)
                return false;

            bool isImage = false;

            try
            {
                using var ms = new MemoryStream();
                file.CopyTo(ms);

                var inspector = new FileFormatInspector();
                var format = inspector.DetermineFileFormat(ms);

                if (!fileExtensions.Any())
                {
                    isImage = format is Image;
                    matchedFileExtension = isImage ? format.Extension : null;

                    return isImage;
                }

                isImage = format is Image && fileExtensions.Contains(format.Extension, StringComparer.OrdinalIgnoreCase);
                matchedFileExtension = isImage ? format.Extension : null;
                
                return isImage;
            }
            catch (Exception)
            {
                return isImage;
            }
        }

        public static async Task<(bool, string matchedFileExtension)> IsValidDocumentAsync(IFormFile file, params string[] fileExtensions)
        {
            if (file is null)
                return (false, null);

            using var ms = new MemoryStream();
            await file.CopyToAsync(ms);

            var inspector = new FileFormatInspector();
            var format = inspector.DetermineFileFormat(ms);

            if (!fileExtensions.Any())
                return (format is Pdf, format.Extension);

            return (format is Pdf && fileExtensions.Contains(format.Extension, StringComparer.OrdinalIgnoreCase), format.Extension);
        }
    }
}
