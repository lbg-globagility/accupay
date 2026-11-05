using AccuPay.Core.Entities;
using AccuPay.Core.Interfaces;
using AccuPay.Core.Services.Reports;
using AccuPay.Core.ValueObjects;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Infrastructure.Data
{
    public class RemittanceReportDataService : IRemittanceReportDataService
    {
        private readonly PayrollContext _context;
        private readonly IProjectEmployeeRepository _projectEmployeeRepository;

        public RemittanceReportDataService(
            PayrollContext context,
            IProjectEmployeeRepository projectEmployeeRepository)
        {
            _context = context;
            _projectEmployeeRepository = projectEmployeeRepository;
        }

        public async Task<ICollection<RemittanceReportRow>> GetData(int organizationId, DateTime dateMonth)
        {
            var allPaystubs = await _context.Paystubs
                .AsNoTracking()
                .Include(p => p.Employee)
                .Include(p => p.PayPeriod)
                .Where(p => p.OrganizationID == organizationId)
                .Where(p => p.PayPeriod.Month == dateMonth.Month)
                .Where(p => p.PayPeriod.Year == dateMonth.Year)
                .ToListAsync();

            var paystubs = await ExcludeContractors(
                organizationId,
                allPaystubs,
                p => p.PayPeriod,
                p => p.EmployeeID);

            if (!paystubs.Any())
                return new List<RemittanceReportRow>();

            var paystubIds = paystubs.Select(p => p.RowID).ToArray();

            var loanTransactions = await _context.LoanTransactions
                .AsNoTracking()
                .Include(t => t.Loan.LoanType)
                .Where(t => paystubIds.Contains(t.PaystubID))
                .ToListAsync();

            var monthStart = new DateTime(dateMonth.Year, dateMonth.Month, 1);

            var socialSecurityBrackets = await _context.SocialSecurityBrackets
                .AsNoTracking()
                .Where(s => s.EffectiveDateFrom <= monthStart)
                .Where(s => s.EffectiveDateTo >= monthStart)
                .ToListAsync();

            return paystubs
                .GroupBy(p => p.EmployeeID)
                .Select(g => CreateRow(
                    g.First().Employee,
                    g.ToList(),
                    loanTransactions.Where(t => t.EmployeeID == g.Key).ToList(),
                    socialSecurityBrackets))
                .OrderBy(r => r.EmployeeName)
                .ToList();
        }

        public async Task<RemittanceLoanData> GetSssSalaryLoanData(int organizationId, DateTime dateMonth)
        {
            var previousMonth = dateMonth.AddMonths(-1);

            var loanTransactions = (await _context.LoanTransactions
                    .AsNoTracking()
                    .Include(t => t.Loan.LoanType)
                    .Include(t => t.Loan.Employee)
                    .Include(t => t.PayPeriod)
                    .Where(t => t.OrganizationID == organizationId)
                    .Where(t => t.PaystubID != null)
                    .Where(t =>
                        (t.PayPeriod.Month == dateMonth.Month && t.PayPeriod.Year == dateMonth.Year) ||
                        (t.PayPeriod.Month == previousMonth.Month && t.PayPeriod.Year == previousMonth.Year))
                    .ToListAsync())
                .Where(t => GetLoanCategory(t.Loan?.LoanType) == LoanCategory.SssSalaryLoan)
                .ToList();

            var employeeTransactions = await ExcludeContractors(
                organizationId,
                loanTransactions,
                t => t.PayPeriod,
                t => t.EmployeeID);

            var paid = employeeTransactions
                .Where(t => t.PayPeriod.Month == dateMonth.Month)
                .GroupBy(t => t.LoanID)
                .Select(g => CreateLoanRow(
                    g.First().Loan,
                    totalDeduction: g.Sum(t => t.DeductionAmount),
                    remarks: null))
                .OrderBy(r => r.EmployeeName)
                .ToList();

            // Includes contractors so a loan that is still being paid is not reported as stopped
            // when its employee became a contractor this month.
            var paidLoanIds = loanTransactions
                .Where(t => t.PayPeriod.Month == dateMonth.Month)
                .Select(t => t.LoanID)
                .ToList();

            var stoppedPayment = employeeTransactions
                .Where(t => t.PayPeriod.Month == previousMonth.Month)
                .Where(t => !paidLoanIds.Contains(t.LoanID))
                .GroupBy(t => t.LoanID)
                .Select(g => g.First().Loan)
                .Select(l => CreateLoanRow(l, totalDeduction: null, remarks: GetStopPaymentRemarks(l)))
                .OrderBy(r => r.EmployeeName)
                .ToList();

            return new RemittanceLoanData()
            {
                Paid = paid,
                StoppedPayment = stoppedPayment
            };
        }

        /// <summary>
        /// The remittance report is for regular employees only. Contractor status is checked per
        /// cutoff, the same way the payroll computes it, so a mid-month change keeps the cutoffs
        /// the person was a regular employee.
        /// </summary>
        private async Task<List<T>> ExcludeContractors<T>(
            int organizationId,
            ICollection<T> items,
            Func<T, PayPeriod> getPayPeriod,
            Func<T, int?> getEmployeeId)
        {
            var employeeItems = new List<T>();

            foreach (var cutoff in items.GroupBy(i => getPayPeriod(i).RowID))
            {
                var payPeriod = getPayPeriod(cutoff.First());

                var contractorEmployeeIds = await _projectEmployeeRepository.GetContractorEmployeeIdsAsync(
                    organizationId,
                    new TimePeriod(payPeriod.PayFromDate, payPeriod.PayToDate));

                employeeItems.AddRange(cutoff
                    .Where(i => !contractorEmployeeIds.Contains(getEmployeeId(i).GetValueOrDefault())));
            }

            return employeeItems;
        }

        private RemittanceLoanRow CreateLoanRow(Loan loan, decimal? totalDeduction, string remarks)
        {
            return new RemittanceLoanRow()
            {
                EmployeeName = loan.Employee?.FullNameWithMiddleInitialLastNameFirst,
                SssNumber = loan.Employee?.SssNo,
                LoanDate = loan.DedEffectiveDateFrom,
                LoanAmount = loan.TotalLoanAmount,
                TotalDeduction = totalDeduction,
                Remarks = remarks
            };
        }

        private string GetStopPaymentRemarks(Loan loan)
        {
            var employee = loan.Employee;

            if (employee != null && !employee.IsActive)
                return employee.EmploymentStatus.Trim().ToLower();

            if (loan.Status == Loan.STATUS_COMPLETE)
                return "fully paid";

            return null;
        }

        private RemittanceReportRow CreateRow(
            Employee employee,
            ICollection<Paystub> paystubs,
            ICollection<LoanTransaction> loanTransactions,
            ICollection<SocialSecurityBracket> socialSecurityBrackets)
        {
            var totalSss = paystubs.Sum(p => p.SssEmployeeShare + p.SssEmployerShare);

            var row = new RemittanceReportRow()
            {
                EmployeeNumber = employee.EmployeeNo,
                EmployeeName = employee.FullNameWithMiddleInitialLastNameFirst,
                LastName = employee.LastName,
                FirstName = employee.FirstName,
                MiddleName = employee.MiddleName,
                SssNumber = employee.SssNo,
                PhilHealthNumber = employee.PhilHealthNo,
                PagIbigNumber = employee.HdmfNo,
                TinNumber = employee.TinNo,
                BirthDate = employee.BirthDate,
                TotalBasic = paystubs.Sum(p => p.BasicPay),
                TotalCompensation = paystubs.Sum(p => p.GrossPay),
                TotalSss = totalSss,
                HdmfEmployeeShare = paystubs.Sum(p => p.HdmfEmployeeShare),
                HdmfEmployerShare = paystubs.Sum(p => p.HdmfEmployerShare),
                TotalTax = paystubs.Sum(p => p.WithholdingTax),
                PhilHealthEmployeeShare = paystubs.Sum(p => p.PhilHealthEmployeeShare),
                PhilHealthEmployerShare = paystubs.Sum(p => p.PhilHealthEmployerShare),
            };

            SetSssBreakdown(row, totalSss, socialSecurityBrackets);
            SetLoans(row, loanTransactions);

            return row;
        }

        private void SetSssBreakdown(
            RemittanceReportRow row,
            decimal totalSss,
            ICollection<SocialSecurityBracket> socialSecurityBrackets)
        {
            // The paystub only stores the combined SSS share so the premium, ECC and MPF
            // are taken from the bracket whose total contribution matches the month's total.
            var bracket = socialSecurityBrackets.FirstOrDefault(s =>
                s.EmployeeContributionAmount +
                s.EmployeeMPFAmount +
                s.EmployerContributionAmount +
                s.EmployerECAmount +
                s.EmployerMPFAmount == totalSss);

            if (bracket == null)
            {
                row.SssPremium = totalSss;
                return;
            }

            row.SssPremium = bracket.EmployeeContributionAmount + bracket.EmployerContributionAmount;
            row.Ecc = bracket.EmployerECAmount;
            row.MandatoryProvidentFund = bracket.EmployeeMPFAmount + bracket.EmployerMPFAmount;
        }

        private void SetLoans(RemittanceReportRow row, ICollection<LoanTransaction> loanTransactions)
        {
            foreach (var loanTransaction in loanTransactions)
            {
                var amount = loanTransaction.DeductionAmount;

                switch (GetLoanCategory(loanTransaction.Loan?.LoanType))
                {
                    case LoanCategory.SssSalaryLoan: row.SssSalaryLoan += amount; break;
                    case LoanCategory.SssCalamityLoan: row.SssCalamityLoan += amount; break;
                    case LoanCategory.SssEmergencyLoan: row.SssEmergencyLoan += amount; break;
                    case LoanCategory.HdmfSalaryLoan: row.HdmfSalaryLoan += amount; break;
                    case LoanCategory.HdmfCalamityLoan: row.HdmfCalamityLoan += amount; break;
                    case LoanCategory.HdmfMp2: row.HdmfMp2 += amount; break;
                }
            }
        }

        private LoanCategory GetLoanCategory(Product loanType)
        {
            // Loan types are user defined so they are classified by name.
            var loanTypeName = loanType?.PartNo?.Trim().ToUpper() ?? string.Empty;

            if (loanTypeName.Contains("MP2"))
                return LoanCategory.HdmfMp2;

            if (loanTypeName.Contains("SSS"))
            {
                if (loanTypeName.Contains("CALAMITY"))
                    return LoanCategory.SssCalamityLoan;
                if (loanTypeName.Contains("EMERGENCY"))
                    return LoanCategory.SssEmergencyLoan;

                return LoanCategory.SssSalaryLoan;
            }

            if (loanTypeName.Contains("HDMF") ||
                loanTypeName.Contains("PAGIBIG") ||
                loanTypeName.Contains("PAG-IBIG"))
            {
                if (loanTypeName.Contains("CALAMITY"))
                    return LoanCategory.HdmfCalamityLoan;

                return LoanCategory.HdmfSalaryLoan;
            }

            return LoanCategory.Other;
        }

        private enum LoanCategory
        {
            Other,
            SssSalaryLoan,
            SssCalamityLoan,
            SssEmergencyLoan,
            HdmfSalaryLoan,
            HdmfCalamityLoan,
            HdmfMp2
        }
    }
}
