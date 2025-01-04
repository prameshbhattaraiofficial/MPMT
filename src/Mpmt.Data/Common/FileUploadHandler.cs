using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace Mpmt.Data.Common
{
    /// <summary>
    /// The file upload handler.
    /// </summary>
    public class FileUploadHandler
    {
        /// <summary>
        /// Uploads the file.
        /// </summary>
        /// <param name="hostEnv">The host env.</param>
        /// <param name="folderPath">The folder path.</param>
        /// <param name="file">The file.</param>
        /// <returns>A Task.</returns>
        public static async Task<string> UploadFile(IWebHostEnvironment hostEnv, string folderPath, IFormFile file)
        {
            if (!Directory.Exists("" + hostEnv.WebRootPath + "\\" + folderPath))
                Directory.CreateDirectory("" + hostEnv.WebRootPath + "\\" + folderPath);

            if (file == null) return "/" + folderPath;

            //folderPath += Guid.NewGuid().ToString() + "_" + file.FileName;
            var fileNameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(file.FileName);
            var fileExtension = System.IO.Path.GetExtension(file.FileName);
            folderPath += fileNameWithoutExtension + "_" + DateTime.UtcNow.ToString("yyyy_MM_dd_HH_mm_ss_ffff") + fileExtension;

            var serverFolder = Path.Combine(hostEnv.WebRootPath, folderPath);
            await using (var fileStream = new FileStream(serverFolder, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
                fileStream.Flush();
            }
            return "/" + folderPath;
        }
    }
}
