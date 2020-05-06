using AccuPay.Data.Entities;
using AccuPay.Data.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccuPay.Data.Services
{
    public class FiledLeaveReportDataService
    {
        private readonly int _organizationId;
        private readonly TimePeriod _selectedPeriod;

        public FiledLeaveReportDataService(int organizationId, TimePeriod selectedPeriod)
        {
            _organizationId = organizationId;
            _selectedPeriod = selectedPeriod;
        }

        public async Task<IEnumerable<LeaveTransaction>> GetData()
        {
            using (var context = new PayrollContext())
            {
                return await context.LeaveTransactions.
                                    Include(x => x.Employee).
                                    Include(x => x.LeaveLedger.Product).
                                    Include(x => x.Leave).
                                    Where(x => x.TransactionDate >= _selectedPeriod.Start).
                                    Where(x => x.TransactionDate <= _selectedPeriod.End).
                                    Where(x => x.OrganizationID == _organizationId).
                                    Where(x => x.Type == LeaveTransactionType.Debit).
                                    Where(x => x.Amount != 0).
                                    OrderBy(x => x.TransactionDate).
                                    ToListAsync();
            }
        }
    }
}