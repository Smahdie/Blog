using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Component.FileUpload.Implementations.FileSignatures
{
    internal class PngSignature : FileSignature
    {
        public override List<string> Extensions => new List<string> { "png" };
        public string Signature => "89 50 4E 47 0D 0A 1A 0A";

        public override async Task<bool> Check(IFormFile file)
        {
            var fileContent = await Get(file);

            return fileContent.StartsWith(Signature);
        }
    }
}
