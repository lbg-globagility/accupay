using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface IEmployeePersonalProfilesReportBuilder
    {
        Task CreateReport(int organizationId, string saveFilePath);
    }
}