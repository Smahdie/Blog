using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Component.FileUpload.Implementations.FileSignatures
{
    internal class JpgSignature : FileSignature
    {
        public override List<string> Extensions => new List<string> { "jpg", "jpeg" };
        public string Signature => "FF D8 FF";

        public override async Task<bool> Check(IFormFile file)
        {
            var fileContent = await Get(file);

            return fileContent.StartsWith(Signature);
        }
    }
}
