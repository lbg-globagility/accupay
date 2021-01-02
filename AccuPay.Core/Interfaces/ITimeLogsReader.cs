using System.IO;
using static AccuPay.Core.Services.TimeLogsReader;

namespace AccuPay.Core.Interfaces
{
    public interface ITimeLogsReader
    {
        ImportOutput Read(FileStream fileStream);

        ImportOutput Read(string filename);
    }
}
