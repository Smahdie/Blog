using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Component.FileUpload.Implementations.FileSignatures
{
    internal abstract class FileSignature
    {
        public abstract List<string> Extensions { get; }

        public abstract Task<bool> Check(IFormFile file);

        public async Task<string> Get(IFormFile file)
        {
            using var stream = file.OpenReadStream();
            var bufferLen = 4096;
            var buffer = new byte[bufferLen];

            await stream.ReadAsync(buffer, 0, bufferLen);

            var fileByteArray = ByteArrayToString(buffer);

            return fileByteArray;
        }

        private static string ByteArrayToString(byte[] byteArray)
        {
            return BitConverter.ToString(byteArray)
                .Substring(0, 100)
                .Replace("-", " ");
        }
    }
}
