using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AccuPay.Core.Interfaces;
using AccuPay.Core.Interfaces.Reports;
using AccuPay.Core.ReportModels;
using AccuPay.Core.Services;
using AccuPay.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace AccuPay.Infrastructure.Reports
{
    public class PbcomReportBuilder : IPbcomReportBuilder
    {
        private readonly PayrollContext _context;
        public PbcomReportBuilder(PayrollContext context)
        {
            _context = context;
        }

        public async Task<List<PbcomReportModel>> GetData(DateTime startPeriod, DateTime endPeriod, int organizationId)
        {
            var query = _context.Paystubs.Include(x => x.Employee).Where(x=>x.Employee.BankName=="PB com").Where(p => p.PayFromDate >= startPeriod).
                Where(p => p.PayToDate <= endPeriod).Where(x=>x.OrganizationID==organizationId).Select(x=>new PbcomReportModel()
            {
                ATMNo=x.Employee.AtmNo,
                FirstName = x.Employee.FirstName,
                LastName = x.Employee.LastName,
                MiddleName = x.Employee.MiddleName,
                TotalNetSalary = x.TotalNetSalary
            }).ToList();
            return query;
        }
    }
}
