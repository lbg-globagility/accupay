using AccuPay.Core.Entities;
using AccuPay.Core.Enums;
using AccuPay.Core.Helpers;
using AccuPay.Core.Interfaces;
using AccuPay.Core.Services;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace AccuPay.Core.UnitTests.SSS
{
    public class SssCalculatorTest_2021
    {
        [TestCaseSource(typeof(SSSData_2021), "Brackets_SalaryBased")]
        public void ShouldCalculate_WithBasicPayCalculationBasis(
            decimal basicSalary,
            decimal expectedSssEmployeeShare,
            decimal expectedSssEmployerShare)
        {
            Mock<IPolicyHelper> policyHelper = new Mock<IPolicyHelper>();
            policyHelper.Setup(x => x.SssCalculationBasis).Returns(SssCalculationBasis.BasicSalary);

            var paystub = new Paystub();
            var previousPaystub = new Paystub();

            BaseShouldCalculate(
                basicSalary: basicSalary,
                paystub: paystub,
                previousPaystub: previousPaystub,
                expectedSssEmployeeShare: expectedSssEmployeeShare,
                expectedSssEmployerShare: expectedSssEmployerShare,
                policyHelper.Object,
                Employee.EmployeeTypeMonthly);

            BaseShouldCalculate(
                basicSalary: basicSalary,
                paystub: paystub,
                previousPaystub: previousPaystub,
                expectedSssEmployeeShare: expectedSssEmployeeShare,
                expectedSssEmployerShare: expectedSssEmployerShare,
                policyHelper.Object,
                Employee.EmployeeTypeFixed);
        }

        [TestCaseSource(typeof(SSSData_2021), "Brackets_PaystubBased")]
        public void ShouldCalculate_WithBasicMinusDeductionsCalculationBasis(
           decimal previousTotalDaysPayWithOutOvertimeAndLeave,
           decimal totalDaysPayWithOutOvertimeAndLeave,
           decimal previousTotalEarnings,
           decimal currentTotalEarnings,
           decimal previousGrossPay,
           decimal currentGrossPay,
           decimal expectedSssEmployeeShare,
           decimal expectedSssEmployerShare)
        {
            var policyHelper = new Mock<IPolicyHelper>();
            policyHelper.Setup(x => x.SssCalculationBasis).Returns(SssCalculationBasis.BasicMinusDeductions);

            var paystubMock = new Mock<Paystub>();
            paystubMock
                .Setup(x => x.TotalDaysPayWithOutOvertimeAndLeave)
                .Returns(totalDaysPayWithOutOvertimeAndLeave);
            Paystub paystub = paystubMock.Object;
            paystub.TotalEarnings = currentTotalEarnings;
            paystub.GrossPay = currentGrossPay;

            var previousPaystubMock = new Mock<Paystub>();
            previousPaystubMock
                .Setup(x => x.TotalDaysPayWithOutOvertimeAndLeave)
                .Returns(previousTotalDaysPayWithOutOvertimeAndLeave);
            Paystub previousPaystub = previousPaystubMock.Object;
            previousPaystub.TotalEarnings = previousTotalEarnings;
            previousPaystub.GrossPay = previousGrossPay;

            decimal basicSalary = 15_000m;

            BaseShouldCalculate(
                basicSalary: basicSalary,
                paystub: paystub,
                previousPaystub: previousPaystub,
                expectedSssEmployeeShare: expectedSssEmployeeShare,
                expectedSssEmployerShare: expectedSssEmployerShare,
                policyHelper.Object,
                Employee.EmployeeTypeMonthly);

            decimal expectedSssEmployeeShareForFixed = 675m;
            decimal expectedSssEmployerShareForFixed = 1_305m;

            BaseShouldCalculate(
                basicSalary: basicSalary,
                paystub: paystub,
                previousPaystub: previousPaystub,
                expectedSssEmployeeShare: expectedSssEmployeeShareForFixed,
                expectedSssEmployerShare: expectedSssEmployerShareForFixed,
                policyHelper.Object,
                Employee.EmployeeTypeFixed);
        }

        [TestCaseSource(typeof(SSSData_2021), "Brackets_PaystubBased")]
        public void ShouldCalculate_WithEarningsCalculationBasis(
            decimal previousTotalDaysPayWithOutOvertimeAndLeave,
            decimal totalDaysPayWithOutOvertimeAndLeave,
            decimal previousTotalEarnings,
            decimal currentTotalEarnings,
            decimal previousGrossPay,
            decimal currentGrossPay,
            decimal expectedSssEmployeeShare,
            decimal expectedSssEmployerShare)
        {
            BaseShouldCalculatePaystubBased(
                SssCalculationBasis.Earnings,
                previousTotalDaysPayWithOutOvertimeAndLeave: previousTotalDaysPayWithOutOvertimeAndLeave,
                totalDaysPayWithOutOvertimeAndLeave: totalDaysPayWithOutOvertimeAndLeave,
                previousTotalEarnings: previousTotalEarnings,
                currentTotalEarnings: currentTotalEarnings,
                previousGrossPay: previousGrossPay,
                currentGrossPay: currentGrossPay,
                expectedSssEmployeeShare: expectedSssEmployeeShare,
                expectedSssEmployerShare: expectedSssEmployerShare);
        }

        [TestCaseSource(typeof(SSSData_2021), "Brackets_PaystubBased")]
        public void ShouldCalculate_WithGrossPayCalculationBasis(
            decimal previousTotalDaysPayWithOutOvertimeAndLeave,
            decimal totalDaysPayWithOutOvertimeAndLeave,
            decimal previousTotalEarnings,
            decimal currentTotalEarnings,
            decimal previousGrossPay,
            decimal currentGrossPay,
            decimal expectedSssEmployeeShare,
            decimal expectedSssEmployerShare)
        {
            BaseShouldCalculatePaystubBased(
                SssCalculationBasis.GrossPay,
                previousTotalDaysPayWithOutOvertimeAndLeave: previousTotalDaysPayWithOutOvertimeAndLeave,
                totalDaysPayWithOutOvertimeAndLeave: totalDaysPayWithOutOvertimeAndLeave,
                previousTotalEarnings: previousTotalEarnings,
                currentTotalEarnings: currentTotalEarnings,
                previousGrossPay: previousGrossPay,
                currentGrossPay: currentGrossPay,
                expectedSssEmployeeShare: expectedSssEmployeeShare,
                expectedSssEmployerShare: expectedSssEmployerShare);
        }

        public void BaseShouldCalculatePaystubBased(
            SssCalculationBasis sssCalculationBasis,
            decimal previousTotalDaysPayWithOutOvertimeAndLeave,
            decimal totalDaysPayWithOutOvertimeAndLeave,
            decimal previousTotalEarnings,
            decimal currentTotalEarnings,
            decimal previousGrossPay,
            decimal currentGrossPay,
            decimal expectedSssEmployeeShare,
            decimal expectedSssEmployerShare)
        {
            var policyHelper = new Mock<IPolicyHelper>();
            policyHelper.Setup(x => x.SssCalculationBasis).Returns(sssCalculationBasis);

            var paystubMock = new Mock<Paystub>();
            paystubMock
                .Setup(x => x.TotalDaysPayWithOutOvertimeAndLeave)
                .Returns(totalDaysPayWithOutOvertimeAndLeave);
            Paystub paystub = paystubMock.Object;
            paystub.TotalEarnings = currentTotalEarnings;
            paystub.GrossPay = currentGrossPay;

            var previousPaystubMock = new Mock<Paystub>();
            previousPaystubMock
                .Setup(x => x.TotalDaysPayWithOutOvertimeAndLeave)
                .Returns(previousTotalDaysPayWithOutOvertimeAndLeave);
            Paystub previousPaystub = previousPaystubMock.Object;
            previousPaystub.TotalEarnings = previousTotalEarnings;
            previousPaystub.GrossPay = previousGrossPay;

            BaseShouldCalculate(
                basicSalary: 0,
                paystub: paystub,
                previousPaystub: previousPaystub,
                expectedSssEmployeeShare: expectedSssEmployeeShare,
                expectedSssEmployerShare: expectedSssEmployerShare,
                policyHelper.Object,
                Employee.EmployeeTypeMonthly);

            BaseShouldCalculate(
                basicSalary: 0,
                paystub: paystub,
                previousPaystub: previousPaystub,
                expectedSssEmployeeShare: expectedSssEmployeeShare,
                expectedSssEmployerShare: expectedSssEmployerShare,
                policyHelper.Object,
                Employee.EmployeeTypeFixed);

            //BaseShouldCalculate(
            //    basicSalary: 0,
            //    paystub: paystub,
            //    previousPaystub: previousPaystub,
            //    expectedSssEmployeeShare: expectedSssEmployeeShare,
            //    expectedSssEmployerShare: expectedSssEmployerShare,
            //    policyHelper.Object,
            //    Employee.EmployeeTypeDaily);
        }

        private static void BaseShouldCalculate(
            decimal basicSalary,
            Paystub paystub,
            Paystub previousPaystub,
            decimal expectedSssEmployeeShare,
            decimal expectedSssEmployerShare,
            IPolicyHelper policy,
            string employeeType)
        {
            List<SocialSecurityBracket> socialSecurityBrackets = MockSocialSecurityBrackets.Get();

            var payPeriod = new PayPeriod()
            {
                PayFromDate = new DateTime(2021, 1, 1),
                PayToDate = new DateTime(2021, 1, 15),
                Half = PayPeriod.FirstHalfValue
            };
            var calculator = new SssCalculator(policy, socialSecurityBrackets, payPeriod);

            var salary = new Salary()
            {
                DoPaySSSContribution = true,
                BasicSalary = basicSalary,
                AllowanceSalary = 0,
            };

            var employee = new Employee()
            {
                EmployeeType = employeeType,
                WorkDaysPerYear = 312,
                Position = new Position()
                {
                    Division = new Division()
                    {
                        SssDeductionSchedule = ContributionSchedule.FIRST_HALF
                    }
                }
            };

            string currentSystemOwner = string.Empty;

            calculator.Calculate(paystub, previousPaystub, salary, employee, currentSystemOwner);

            Assert.AreEqual(expectedSssEmployerShare, paystub.SssEmployerShare);
            Assert.AreEqual(expectedSssEmployeeShare, paystub.SssEmployeeShare);
        }
    }
}
