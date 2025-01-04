using Newtonsoft.Json;
using System.Text;

namespace Mpmt.Services.Common.Extensions
{
    public static class StreamExtensions
    {
        public static void SerializeToJsonAndWrite<T>(this Stream stream, T inputDataObj)
        {
            if (stream is null) throw new ArgumentNullException(nameof(stream));

            if (!stream.CanWrite) throw new NotSupportedException("Can't write to this stream.");

            using var streamWriter = new StreamWriter(stream, new UTF8Encoding(), 1024, true);

            using var jsonTextWriter = new JsonTextWriter(streamWriter);
            var jsonSerializer = new JsonSerializer();
            jsonSerializer.Serialize(jsonTextWriter, inputDataObj);
            jsonTextWriter.Flush();
        }

        public static async Task<string> ReadAsStringAsync(this Stream stream)
        {
            if (stream is null)
                return null;

            stream.Position = 0;
            using var reader = new StreamReader(stream, Encoding.UTF8);
            
            return await reader.ReadToEndAsync();
        }
    }
}
