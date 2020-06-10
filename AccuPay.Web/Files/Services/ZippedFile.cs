using System;
using System.IO;

namespace AccuPay.Web.Files.Services
{
    public class ZippedFile
    {
        public string Name { get; set; }

        public string ContentType { get; set; }

        public Stream Stream { get; set; }

        public ZippedFile(Stream stream, string name, string contentType)
        {
            Stream = stream;
            Name = name;
            ContentType = contentType;
        }
    }
}
