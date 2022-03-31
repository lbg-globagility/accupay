using AccuPay.Core.Entities;
using AccuPay.Core.Interfaces;
using AccuPay.Core.Interfaces.Reports;
using AccuPay.Core.ReportModels;
using AccuPay.Core.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Infrastructure.Data.Reports
{
    public class AlphaListReportDataService : StoredProcedureDataService, IAlphaListReportDataService
    {
        private readonly IPaystubRepository _paystubRepository;
        private readonly IWithholdingTaxBracketRepository _withholdingTaxBracketRepository;

        public AlphaListReportDataService(PayrollContext context,
            IPaystubRepository paystubRepository,
            IWithholdingTaxBracketRepository withholdingTaxBracketRepository) : base(context)
        {
            _paystubRepository = paystubRepository;
            _withholdingTaxBracketRepository = withholdingTaxBracketRepository;
        }

        public async Task<List<AlphalistModel>> GetData(int organizationId,
            bool actualSwitch,
            PayPeriod startPeriod,
            PayPeriod endPeriod)
        {
            var periodRange = new TimePeriod(startPeriod.PayFromDate,
                endPeriod.PayToDate);

            var paystubs = await _context.Paystubs
                .Include(x => x.Actual)
                .Include(x => x.PayPeriod)
                .Include(x => x.ThirteenthMonthPay)
                .Include(x => x.Employee)
                .Where(x => x.OrganizationID == organizationId)
                .Where(x => x.PayPeriod.PayFromDate >= periodRange.Start)
                .Where(x => x.PayPeriod.PayToDate <= periodRange.End)
                .ToListAsync();

            var latestSalaries = GetLatestSalaries(organizationId);

            var withholdingTaxBrackets = (await _withholdingTaxBracketRepository.GetAllAsync()).
                Where(w => w.IsMonthly).
                Where(w => w.EffectiveDateFrom <= periodRange.Start).
                Where(w => periodRange.Start <= w.EffectiveDateTo).
                ToList();

            return paystubs.GroupBy(p => p.EmployeeID).
                OrderBy(p => p.FirstOrDefault().Employee.FullNameLastNameFirst).
                Select(p => new AlphalistModel(paystubs: p,
                    periodRange: periodRange,
                    latestSalaries: latestSalaries,
                    actualSwitch: actualSwitch,
                    withholdingTaxBrackets: withholdingTaxBrackets)).
                    ToList();
        }

        private List<SalaryModel> GetLatestSalaries(int organizationId)
        {
            string procedureSql = $"CALL `GetLatestSalaries`({organizationId}); SELECT * FROM latestsalaries;";
            var result = CallRawSql(procedureSql);

            return result.Rows.
                OfType<DataRow>().
                Select(r => new SalaryModel(dataRow: r)).
                ToList();
        }
    }
}
