using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

namespace AccuPay.Web.Core.Files
{
    public class LocalFilesystem : IFilesystem
    {
        private readonly string _rootPath;

        public LocalFilesystem(string rootPath)
        {
            _rootPath = rootPath;
        }

        public async Task<string> Move(IFormFile file, string path)
        {
            var directoryPath = Path.GetDirectoryName(path);

            var fullDirectoryPath = $"{_rootPath}/{directoryPath}";
            if (!Directory.Exists(fullDirectoryPath))
            {
                Directory.CreateDirectory(fullDirectoryPath);
            }

            var filename = Path.GetFileName(path);

            var fullPath = $"{_rootPath}/{directoryPath}/{filename}";
            using (var destination = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(destination);
            }

            return $"{directoryPath}/{filename}";
        }

        public async Task<string> Move(Stream stream, string path)
        {
            var directoryPath = Path.GetDirectoryName(path);
            var fullDirectoryPath = $"{_rootPath}/{directoryPath}";

            if (!Directory.Exists(fullDirectoryPath))
            {
                Directory.CreateDirectory(fullDirectoryPath);
            }

            var filename = Path.GetFileName(path);

            var fullPath = $"{fullDirectoryPath}/{filename}";
            using (var destination = new FileStream(fullPath, FileMode.Create))
            {
                await stream.CopyToAsync(destination);
            }

            return $"{directoryPath}/{filename}";
        }

        public Task<Stream> Get(string path)
        {
            var fullPath = $"{_rootPath}/{path}";

            return Task.FromResult<Stream>(new FileStream(fullPath, FileMode.Open, FileAccess.Read, FileShare.Read));
        }

        public Task Remove(string path)
        {
            var fullPath = $"{_rootPath}/{path}";

            return Task.Run(() =>
            {
                File.Delete(fullPath);
            });
        }
    }
}
