using AccuPay.Core.Interfaces;
using AccuPay.Core.ReportModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Infrastructure.Data
{
    public class LaGlobalAlphaListReportDataService : StoredProcedureDataService, ILaGlobalAlphaListReportDataService
    {
        public LaGlobalAlphaListReportDataService(PayrollContext context) : base(context)
        {
        }

        public async Task<List<LaGlobalAlphaListReportModel>> GetData(int organizationId,
            bool actualSwitch,
            int startPeriodId,
            int endPeriodId,
            string saveFilePath)
        {
            int[] periodIds = { startPeriodId, endPeriodId };

            var periods = await _context.PayPeriods.
                Where(p => periodIds.Contains(p.RowID.Value)).
                ToListAsync();

            var startPeriod = periods.FirstOrDefault(p => p.RowID == startPeriodId);
            var endPeriod = periods.FirstOrDefault(p => p.RowID == endPeriodId);

            var paystubs = await _context.Paystubs.
                Include(p => p.Actual).
                Include(p => p.Employee).
                Include(p => p.ThirteenthMonthPay).
                Where(p => p.OrganizationID == organizationId).
                Where(p => p.PayFromDate >= startPeriod.PayFromDate).
                Where(p => p.PayToDate <= endPeriod.PayToDate).
                ToListAsync();

            if (actualSwitch)
            {
                return paystubs.GroupBy(p => p.EmployeeID).
                Select(p => new LaGlobalAlphaListReportModel
                {
                    EmployeeId = p.FirstOrDefault().Employee.EmployeeNo,
                    Name = p.FirstOrDefault().Employee.FullNameLastNameFirst,
                    TIN = p.FirstOrDefault().Employee.TinNo,
                    Address = p.FirstOrDefault().Employee.HomeAddress,
                    BirthDate = p.FirstOrDefault().Employee.BirthDate.ToShortDateString(),
                    ContactNo = p.FirstOrDefault().Employee.MobilePhone,
                    Gross = p.Sum(ps => ps.Actual.GrossPay),
                    SSS = p.Sum(ps => ps.SssEmployeeShare),
                    PhilHealth = p.Sum(ps => ps.PhilHealthEmployeeShare),
                    HDMF = p.Sum(ps => ps.HdmfEmployeeShare),
                    ThirteenthMonthPay = p.Sum(ps => ps.ThirteenthMonthPay.Amount)
                }).
                OrderBy(t => t.Name).
                ToList();
            }

            return paystubs.GroupBy(p => p.EmployeeID).
                Select(p => new LaGlobalAlphaListReportModel
                {
                    EmployeeId = p.FirstOrDefault().Employee.EmployeeNo,
                    Name = p.FirstOrDefault().Employee.FullNameLastNameFirst,
                    TIN = p.FirstOrDefault().Employee.TinNo,
                    Address = p.FirstOrDefault().Employee.HomeAddress,
                    BirthDate = p.FirstOrDefault().Employee.BirthDate.ToShortDateString(),
                    ContactNo = p.FirstOrDefault().Employee.MobilePhone,
                    Gross = p.Sum(ps => ps.GrossPay),
                    SSS = p.Sum(ps => ps.SssEmployeeShare),
                    PhilHealth = p.Sum(ps => ps.PhilHealthEmployeeShare),
                    HDMF = p.Sum(ps => ps.HdmfEmployeeShare),
                    ThirteenthMonthPay = p.Sum(ps => ps.ThirteenthMonthPay.Amount)
                }).
                OrderBy(t => t.Name).
                ToList();
        }
    }
}
