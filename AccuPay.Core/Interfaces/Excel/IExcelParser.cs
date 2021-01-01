using System.Collections.Generic;
using System.IO;

namespace AccuPay.Core.Interfaces.Excel
{
    public interface IExcelParser<T> where T : IExcelRowRecord, new()
    {
        string XlsxExtension { get; }

        IList<T> Read(Stream fileStream, string worksheetName = null);

        IList<T> Read(string filePath, string worksheetName = null);
    }
}