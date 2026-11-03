using System;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface IRemittanceReportBuilder
    {
        Task CreateReport(int organizationId, DateTime dateMonth, string saveFilePath);
    }
}
