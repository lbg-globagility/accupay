using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace AccuPay.Web.Core.Emails
{
    public class FileEmailSender : IEmailSender
    {
        private readonly string _rootDirectory;

        public FileEmailSender(string rootDirectory)
        {
            _rootDirectory = rootDirectory;
        }

        public Task Send(Email email)
        {
            var fileName = $"{email.Subject}-{DateTime.UtcNow.ToFileTime()}.html";
            var directory = _rootDirectory;

            Directory.CreateDirectory(directory);

            var fullPath = $"{directory}/{fileName}";
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                stream.Write(Encoding.UTF8.GetBytes(email.Html));
                stream.Close();
            }

            return Task.CompletedTask;
        }
    }
}
