using AccuPay.Core.Entities;
using AccuPay.Core.Enums;
using AccuPay.Core.Interfaces;
using AccuPay.Core.Services;
using AccuPay.Core.TestData;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AccuPay.Core.IntegrationTests.SSS
{
    public class SssCalculatorTest_Bracket2019 : DatabaseTest
    {
        private const int OrganizationId = 1;

        [TestCaseSource(typeof(SSSTestSource_2019), "Brackets_SalaryBased")]
        public void ShouldCalculate_WithBasicSalaryCalculationBasis(
        decimal basicSalary,
        decimal expectedSssEmployeeShare,
        decimal expectedSssEmployerShare)
        {
            Mock<IPolicyHelper> policyHelper = new Mock<IPolicyHelper>();
            policyHelper
                .Setup(x => x.SssCalculationBasis(OrganizationId))
                .Returns(SssCalculationBasis.BasicSalary);

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

        [TestCaseSource(typeof(SSSTestSource_2019), "Brackets_PaystubBased")]
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
            policyHelper
                .Setup(x => x.SssCalculationBasis(OrganizationId))
                .Returns(SssCalculationBasis.BasicMinusDeductions);

            var paystubMock = new Mock<Paystub>();
            paystubMock
                .Setup(x => x.TotalDaysPayWithOutOvertimeAndLeave(true))
                .Returns(totalDaysPayWithOutOvertimeAndLeave);
            Paystub paystub = paystubMock.Object;
            paystub.TotalEarnings = currentTotalEarnings;
            paystub.GrossPay = currentGrossPay;

            var previousPaystubMock = new Mock<Paystub>();
            previousPaystubMock
                .Setup(x => x.TotalDaysPayWithOutOvertimeAndLeave(true))
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

            decimal expectedSssEmployeeShareForFixed = 600m;
            decimal expectedSssEmployerShareForFixed = 1_230m;

            BaseShouldCalculate(
                basicSalary: basicSalary,
                paystub: paystub,
                previousPaystub: previousPaystub,
                expectedSssEmployeeShare: expectedSssEmployeeShareForFixed,
                expectedSssEmployerShare: expectedSssEmployerShareForFixed,
                policyHelper.Object,
                Employee.EmployeeTypeFixed);
        }

        [TestCaseSource(typeof(SSSTestSource_2019), "Brackets_PaystubBased")]
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

        [TestCaseSource(typeof(SSSTestSource_2019), "Brackets_PaystubBased")]
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
            policyHelper
                .Setup(x => x.SssCalculationBasis(OrganizationId))
                .Returns(sssCalculationBasis);

            var paystubMock = new Mock<Paystub>();
            paystubMock
                .Setup(x => x.TotalDaysPayWithOutOvertimeAndLeave(true))
                .Returns(totalDaysPayWithOutOvertimeAndLeave);
            Paystub paystub = paystubMock.Object;
            paystub.TotalEarnings = currentTotalEarnings;
            paystub.GrossPay = currentGrossPay;

            var previousPaystubMock = new Mock<Paystub>();
            previousPaystubMock
                .Setup(x => x.TotalDaysPayWithOutOvertimeAndLeave(true))
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

        [TestCaseSource(typeof(SSSTestSource_2019), "Brackets_SalaryBased")]
        public void ShouldCalculate__WithIrregularPayPeriodAndBasicPayCalculationBasis(
            decimal basicSalary,
            decimal expectedSssEmployeeShare,
            decimal expectedSssEmployerShare)
        {
            BaseShouldCalculate_WithIrregularPayPeriod(
                basicSalary: basicSalary,
                SssCalculationBasis.BasicSalary,
                previousTotalDaysPayWithOutOvertimeAndLeave: 0,
                totalDaysPayWithOutOvertimeAndLeave: 0,
                previousTotalEarnings: 0,
                currentTotalEarnings: 0,
                previousGrossPay: 0,
                currentGrossPay: 0,
                expectedSssEmployeeShare: expectedSssEmployeeShare,
                expectedSssEmployerShare: expectedSssEmployerShare);
        }

        [TestCaseSource(typeof(SSSTestSource_2019), "Brackets_PaystubBased")]
        public void ShouldCalculate_WithIrregularPayPeriodAndBasicMinusDeductionsCalculationBasis(
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
            policyHelper
                .Setup(x => x.SssCalculationBasis(OrganizationId))
                .Returns(SssCalculationBasis.BasicMinusDeductions);

            var paystubMock = new Mock<Paystub>();
            paystubMock
                .Setup(x => x.TotalDaysPayWithOutOvertimeAndLeave(true))
                .Returns(totalDaysPayWithOutOvertimeAndLeave);
            Paystub paystub = paystubMock.Object;
            paystub.TotalEarnings = currentTotalEarnings;
            paystub.GrossPay = currentGrossPay;

            var previousPaystubMock = new Mock<Paystub>();
            previousPaystubMock
                .Setup(x => x.TotalDaysPayWithOutOvertimeAndLeave(true))
                .Returns(previousTotalDaysPayWithOutOvertimeAndLeave);
            Paystub previousPaystub = previousPaystubMock.Object;
            previousPaystub.TotalEarnings = previousTotalEarnings;
            previousPaystub.GrossPay = previousGrossPay;

            var payPeriod = new PayPeriod()
            {
                PayFromDate = new DateTime(2019, 3, 16),
                PayToDate = new DateTime(2019, 3, 31),
                Half = PayPeriod.FirstHalfValue,
                Year = 2019,
                Month = 4
            };

            decimal basicSalary = 15_000m;

            BaseShouldCalculate(
                basicSalary: basicSalary,
                paystub: paystub,
                previousPaystub: previousPaystub,
                expectedSssEmployeeShare: expectedSssEmployeeShare,
                expectedSssEmployerShare: expectedSssEmployerShare,
                policyHelper.Object,
                Employee.EmployeeTypeMonthly,
                payPeriod);

            decimal expectedSssEmployeeShareForFixed = 600m;
            decimal expectedSssEmployerShareForFixed = 1_230m;

            BaseShouldCalculate(
                basicSalary: basicSalary,
                paystub: paystub,
                previousPaystub: previousPaystub,
                expectedSssEmployeeShare: expectedSssEmployeeShareForFixed,
                expectedSssEmployerShare: expectedSssEmployerShareForFixed,
                policyHelper.Object,
                Employee.EmployeeTypeFixed,
                payPeriod);
        }

        [TestCaseSource(typeof(SSSTestSource_2019), "Brackets_PaystubBased")]
        public void ShouldCalculate_WithIrregularPayPeriodAndGrossPayCalculationBasis(
           decimal previousTotalDaysPayWithOutOvertimeAndLeave,
           decimal totalDaysPayWithOutOvertimeAndLeave,
           decimal previousTotalEarnings,
           decimal currentTotalEarnings,
           decimal previousGrossPay,
           decimal currentGrossPay,
           decimal expectedSssEmployeeShare,
           decimal expectedSssEmployerShare)
        {
            BaseShouldCalculate_WithIrregularPayPeriod(
                basicSalary: 0,
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

        [TestCaseSource(typeof(SSSTestSource_2019), "Brackets_PaystubBased")]
        public void ShouldCalculate_WithIrregularPayPeriodAndEarningsCalculationBasis(
           decimal previousTotalDaysPayWithOutOvertimeAndLeave,
           decimal totalDaysPayWithOutOvertimeAndLeave,
           decimal previousTotalEarnings,
           decimal currentTotalEarnings,
           decimal previousGrossPay,
           decimal currentGrossPay,
           decimal expectedSssEmployeeShare,
           decimal expectedSssEmployerShare)
        {
            BaseShouldCalculate_WithIrregularPayPeriod(
                basicSalary: 0,
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

        /// <summary>
        /// For scenarios like in Cinema 2000 and goldwings where the
        /// payroll time period is different from attendance time period.
        /// </summary>
        private void BaseShouldCalculate_WithIrregularPayPeriod(
            decimal basicSalary,
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
            policyHelper
                .Setup(x => x.SssCalculationBasis(OrganizationId))
                .Returns(sssCalculationBasis);

            var paystubMock = new Mock<Paystub>();
            paystubMock
                .Setup(x => x.TotalDaysPayWithOutOvertimeAndLeave(true))
                .Returns(totalDaysPayWithOutOvertimeAndLeave);
            Paystub paystub = paystubMock.Object;
            paystub.TotalEarnings = currentTotalEarnings;
            paystub.GrossPay = currentGrossPay;

            var previousPaystubMock = new Mock<Paystub>();
            previousPaystubMock
                .Setup(x => x.TotalDaysPayWithOutOvertimeAndLeave(true))
                .Returns(previousTotalDaysPayWithOutOvertimeAndLeave);
            Paystub previousPaystub = previousPaystubMock.Object;
            previousPaystub.TotalEarnings = previousTotalEarnings;
            previousPaystub.GrossPay = previousGrossPay;

            var payPeriod = new PayPeriod()
            {
                PayFromDate = new DateTime(2019, 3, 16),
                PayToDate = new DateTime(2019, 3, 31),
                Half = PayPeriod.FirstHalfValue,
                Year = 2019,
                Month = 4
            };

            BaseShouldCalculate(
                basicSalary: basicSalary,
                paystub: paystub,
                previousPaystub: previousPaystub,
                expectedSssEmployeeShare: expectedSssEmployeeShare,
                expectedSssEmployerShare: expectedSssEmployerShare,
                policyHelper.Object,
                Employee.EmployeeTypeFixed,
                payPeriod);

            BaseShouldCalculate(
                basicSalary: basicSalary,
                paystub: paystub,
                previousPaystub: previousPaystub,
                expectedSssEmployeeShare: expectedSssEmployeeShare,
                expectedSssEmployerShare: expectedSssEmployerShare,
                policyHelper.Object,
                Employee.EmployeeTypeMonthly,
                payPeriod);
        }

        private void BaseShouldCalculate(
            decimal basicSalary,
            Paystub paystub,
            Paystub previousPaystub,
            decimal expectedSssEmployeeShare,
            decimal expectedSssEmployerShare,
            IPolicyHelper policy,
            string employeeType)
        {
            var payPeriod = new PayPeriod()
            {
                PayFromDate = new DateTime(2019, 4, 1),
                PayToDate = new DateTime(2019, 4, 15),
                Half = PayPeriod.FirstHalfValue,
                Year = 2019,
                Month = 4
            };

            BaseShouldCalculate(
                basicSalary: basicSalary,
                paystub: paystub,
                previousPaystub: previousPaystub,
                expectedSssEmployeeShare: expectedSssEmployeeShare,
                expectedSssEmployerShare: expectedSssEmployerShare,
                policy,
                employeeType,
                payPeriod);
        }

        private void BaseShouldCalculate(
            decimal basicSalary,
            Paystub paystub,
            Paystub previousPaystub,
            decimal expectedSssEmployeeShare,
            decimal expectedSssEmployerShare,
            IPolicyHelper policy,
            string employeeType,
            PayPeriod payPeriod)
        {
            var sssRepository = MainServiceProvider.GetRequiredService<ISocialSecurityBracketRepository>();
            IEnumerable<SocialSecurityBracket> socialSecurityBrackets = sssRepository.GetAll();

            if (socialSecurityBrackets.Max(x => x.EffectiveDateFrom) < new DateTime(2020, 4, 1))
                throw new Exception("SSS brackets for 2019-2020 are not created in the database yet");

            var calculator = new SssCalculator(policy, socialSecurityBrackets.ToList(), payPeriod);

            var salary = new Salary()
            {
                DoPaySSSContribution = true,
                BasicSalary = basicSalary,
                AllowanceSalary = 0,
            };

            var employee = EmployeeMother.Simple(employeeType, OrganizationId);

            string currentSystemOwner = string.Empty;

            calculator.Calculate(paystub, previousPaystub, salary, employee, currentSystemOwner);

            Assert.AreEqual(expectedSssEmployerShare, paystub.SssEmployerShare);
            Assert.AreEqual(expectedSssEmployeeShare, paystub.SssEmployeeShare);
        }
    }
}
