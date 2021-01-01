using AccuPay.Core.Entities;
using AccuPay.Core.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Core.Services
{
    public class FiledLeaveReportDataService
    {
        private readonly PayrollContext _context;

        public FiledLeaveReportDataService(PayrollContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<LeaveTransaction>> GetData(int organizationId,
                                                                TimePeriod selectedPeriod)
        {
            return await _context.LeaveTransactions.
                                Include(x => x.Employee).
                                Include(x => x.LeaveLedger.Product).
                                Include(x => x.Leave).
                                Where(x => x.TransactionDate >= selectedPeriod.Start).
                                Where(x => x.TransactionDate <= selectedPeriod.End).
                                Where(x => x.OrganizationID == organizationId).
                                Where(x => x.Type == LeaveTransactionType.Debit).
                                Where(x => x.Amount != 0).
                                OrderBy(x => x.TransactionDate).
                                ToListAsync();
        }
    }
}