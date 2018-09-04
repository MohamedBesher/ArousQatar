using System.Net.Http;
using System.Net.Http.Headers;

namespace Saned.ArousQatar.Api.Infrastructure.Core
{
    public class FileUploadResult
    {
        public string LocalFilePath { get; set; }
        public string FileName { get; set; }
        public long FileLength { get; set; }
    }

    public class UploadMultipartFormProvider : MultipartFormDataStreamProvider
    {
        public UploadMultipartFormProvider(string rootPath) : base(rootPath)
        {
        }

        public override string GetLocalFileName(HttpContentHeaders headers)
        {
            if (headers != null && headers.ContentDisposition != null)
            {
                return headers.ContentDisposition
                    .FileName
                    .TrimEnd('"')
                    .TrimStart('"');
            }

            return base.GetLocalFileName(headers);
        }
    }
}