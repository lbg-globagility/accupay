using System;
using System.IO;

namespace AccuPay.Web.Files.Models
{
    public class VirtualFile : IDisposable
    {
        public string Filename { get; set; }

        public int Size { get; set; }

        public Stream Stream { get; set; }

        public VirtualFile(string filename, Stream stream)
        {
            Filename = filename;
            Stream = stream;
            Size = (int)stream.Length;
        }

        public void Dispose()
        {
            if (Stream is null) return;
            Stream.Close();
            Stream = null;
        }
    }
}
