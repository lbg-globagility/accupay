using AccuPay.Data.Entities;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AccuPay.Core.UnitTests.Mothers
{
    public class LoanMother
    {
        public static LoanSchedule Simple()
        {
            return new LoanSchedule()
            {
                RowID = It.IsAny<int>(),
                EmployeeID = It.IsAny<int>(),
                OrganizationID = It.IsAny<int>(),
                LoanTypeID = It.IsAny<int>(),
                LoanNumber = It.IsAny<string>(),
                TotalLoanAmount = It.IsAny<decimal>(),
                TotalBalanceLeft = It.IsAny<decimal>(),
                DedEffectiveDateFrom = It.IsAny<DateTime>(),
                DeductionAmount = It.IsAny<decimal>(),
                Status = It.IsAny<string>(),
                DeductionPercentage = It.IsAny<decimal>(),
                DeductionSchedule = It.IsAny<string>(),
                Comments = It.IsAny<string>()
            };
        }

        public static LoanSchedule WithLoanAmountAndDeductionAmount(
            decimal totalLoanAmount,
            decimal originalDeductionAmount,
            List<LoanTransaction> loanTransactions = null)
        {
            var loan = Simple();

            loan.TotalLoanAmount = totalLoanAmount;
            loan.TotalBalanceLeft = loan.TotalLoanAmount;
            loan.OriginalDeductionAmount = originalDeductionAmount;
            loan.DeductionAmount = loan.OriginalDeductionAmount;

            loan.LoanTransactions = loanTransactions;
            loan.TotalBalanceLeft -= loanTransactions?.Sum(l => l.PrincipalAmount) ?? 0;

            return loan;
        }

        public static LoanSchedule ForYearlyLoanInterest(
            decimal totalLoanAmount,
            decimal deductionPercentage,
            decimal basicMonthlySalary,
            decimal originalDeductionAmount,
            List<LoanTransaction> loanTransactions = null)
        {
            var loan = WithLoanAmountAndDeductionAmount(
                totalLoanAmount: totalLoanAmount,
                originalDeductionAmount: originalDeductionAmount,
                loanTransactions: loanTransactions);

            loan.DeductionPercentage = deductionPercentage;
            loan.BasicMonthlySalary = basicMonthlySalary;

            return loan;
        }
    }
}
