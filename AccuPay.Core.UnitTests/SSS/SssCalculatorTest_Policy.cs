using AccuPay.Core.Entities;
using AccuPay.Core.Enums;
using AccuPay.Core.Helpers;
using AccuPay.Core.Interfaces;
using AccuPay.Core.Services;
using AccuPay.Core.TestData;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace AccuPay.Core.UnitTests.SSS
{
    public class SssCalculatorTest_Policy
    {
        private const int OrganizationId = 1;

        private PayPeriod _payPeriod;
        private List<SocialSecurityBracket> _socialSecurityBrackets;
        private string _currentSystemOwner;

        [SetUp]
        public void SetUp()
        {
            _payPeriod = new PayPeriod()
            {
                PayFromDate = new DateTime(2021, 1, 1),
                PayToDate = new DateTime(2021, 1, 15),
                Half = PayPeriod.FirstHalfValue,
                Year = 2021,
                Month = 1
            };

            _socialSecurityBrackets = MockSocialSecurityBrackets.Get();
            _currentSystemOwner = string.Empty;
        }

        #region BasicSalary

        [TestCaseSource(typeof(SSSTestSource_2021), "Brackets_SalaryBased")]
        public void ShouldCalculateForMonthlyAndFixed_WithBasicSalaryCalculationBasis(
            decimal salaryBracket,
            decimal expectedSssEmployeeShare,
            decimal expectedSssEmployerShare)
        {
            Mock<IPolicyHelper> policy = new Mock<IPolicyHelper>();
            policy
                .Setup(x => x.SssCalculationBasis(OrganizationId))
                .Returns(SssCalculationBasis.BasicSalary);

            var calculator = new SssCalculator(policy.Object, _socialSecurityBrackets, _payPeriod);

            var salary = new Salary()
            {
                DoPaySSSContribution = true,
                BasicSalary = salaryBracket,
                AllowanceSalary = It.IsAny<decimal>(),
            };

            var paystub = new Paystub();
            var previousPaystub = new Paystub();

            // Monthly
            var employeeMonthly = EmployeeMother.Simple(Employee.EmployeeTypeMonthly, OrganizationId);

            calculator.Calculate(paystub, previousPaystub, salary, employeeMonthly, _currentSystemOwner);

            Assert.AreEqual(expectedSssEmployerShare, paystub.SssEmployerShare);
            Assert.AreEqual(expectedSssEmployeeShare, paystub.SssEmployeeShare);

            // Fixed
            var employeeFixed = EmployeeMother.Simple(Employee.EmployeeTypeFixed, OrganizationId);

            calculator.Calculate(paystub, previousPaystub, salary, employeeFixed, _currentSystemOwner);

            Assert.AreEqual(expectedSssEmployeeShare, paystub.SssEmployeeShare);
            Assert.AreEqual(expectedSssEmployerShare, paystub.SssEmployerShare);
        }

        //[Theory]
        //[TestCase(0, 0, 0)]
        //[TestCase(50, 135, 265)]
        [TestCaseSource(typeof(SSSTestSource_2021), "Brackets_SalaryBased")]
        public void ShouldCalculateForDaily_WithBasicSalaryCalculationBasis(
            decimal salaryBracket,
            decimal expectedSssEmployeeShare,
            decimal expectedSssEmployerShare)
        {
            Mock<IPolicyHelper> policy = new Mock<IPolicyHelper>();
            policy
                .Setup(x => x.SssCalculationBasis(OrganizationId))
                .Returns(SssCalculationBasis.BasicSalary);

            decimal workDaysPerMonth = 20;

            var calculator = new SssCalculator(policy.Object, _socialSecurityBrackets, _payPeriod);

            var salary = new Salary()
            {
                DoPaySSSContribution = true,
                BasicSalary = salaryBracket / workDaysPerMonth,
                AllowanceSalary = It.IsAny<decimal>(),
            };

            var paystub = new Paystub();
            var previousPaystub = new Paystub();

            // Daily
            var employee = EmployeeMother.Simple(Employee.EmployeeTypeDaily, OrganizationId);
            employee.WorkDaysPerYear = workDaysPerMonth * PayrollTools.MonthsPerYear; //240;

            calculator.Calculate(paystub, previousPaystub, salary, employee, _currentSystemOwner);

            Assert.AreEqual(expectedSssEmployeeShare, paystub.SssEmployeeShare);
            Assert.AreEqual(expectedSssEmployerShare, paystub.SssEmployerShare);
        }

        #endregion BasicSalary

        #region Earnings

        [TestCaseSource(typeof(SSSTestSource_2021), "Brackets_SalaryBased")]
        public void ShouldCalculateForMonthlyAndFixed_WithEarningsCalculationBasis(
            decimal salaryBracket,
            decimal expectedSssEmployeeShare,
            decimal expectedSssEmployerShare)
        {
            Mock<IPolicyHelper> policy = new Mock<IPolicyHelper>();
            policy
                .Setup(x => x.SssCalculationBasis(OrganizationId))
                .Returns(SssCalculationBasis.Earnings);

            var calculator = new SssCalculator(policy.Object, _socialSecurityBrackets, _payPeriod);

            var paystub = new Paystub() { TotalEarnings = salaryBracket / 2 };
            var previousPaystub = new Paystub() { TotalEarnings = salaryBracket / 2 };

            var salary = new Salary()
            {
                DoPaySSSContribution = true,
                BasicSalary = It.IsAny<decimal>(),
                AllowanceSalary = It.IsAny<decimal>(),
            };

            // Monthly
            var employeeMonthly = EmployeeMother
                .Simple(Employee.EmployeeTypeMonthly, OrganizationId);

            calculator
                .Calculate(paystub, previousPaystub, salary, employeeMonthly, _currentSystemOwner);

            Assert.AreEqual(expectedSssEmployerShare, paystub.SssEmployerShare);
            Assert.AreEqual(expectedSssEmployeeShare, paystub.SssEmployeeShare);

            // Fixed
            var employeeFixed = EmployeeMother
                .Simple(Employee.EmployeeTypeFixed, OrganizationId);

            calculator
                .Calculate(paystub, previousPaystub, salary, employeeFixed, _currentSystemOwner);

            Assert.AreEqual(expectedSssEmployeeShare, paystub.SssEmployeeShare);
            Assert.AreEqual(expectedSssEmployerShare, paystub.SssEmployerShare);
        }

        [TestCaseSource(typeof(SSSTestSource_2021), "Brackets_SalaryBased")]
        public void ShouldCalculateForDaily_WithEarningsCalculationBasis(
            decimal salaryBracket,
            decimal expectedSssEmployeeShare,
            decimal expectedSssEmployerShare)
        {
            Mock<IPolicyHelper> policy = new Mock<IPolicyHelper>();
            policy
                .Setup(x => x.SssCalculationBasis(OrganizationId))
                .Returns(SssCalculationBasis.Earnings);

            var calculator = new SssCalculator(policy.Object, _socialSecurityBrackets, _payPeriod);

            var salary = new Salary()
            {
                DoPaySSSContribution = true,
                BasicSalary = It.IsAny<decimal>(),
                AllowanceSalary = It.IsAny<decimal>(),
            };

            var paystub = new Paystub() { TotalEarnings = salaryBracket / 2 };
            var previousPaystub = new Paystub() { TotalEarnings = salaryBracket / 2 };

            // Daily
            var employee = EmployeeMother.Simple(Employee.EmployeeTypeDaily, OrganizationId);

            calculator.Calculate(paystub, previousPaystub, salary, employee, _currentSystemOwner);

            Assert.AreEqual(expectedSssEmployeeShare, paystub.SssEmployeeShare);
            Assert.AreEqual(expectedSssEmployerShare, paystub.SssEmployerShare);
        }

        #endregion Earnings

        #region GrossPay

        [TestCaseSource(typeof(SSSTestSource_2021), "Brackets_SalaryBased")]
        public void ShouldCalculateForMonthlyAndFixed_WithGrossPayCalculationBasis(
            decimal salaryBracket,
            decimal expectedSssEmployeeShare,
            decimal expectedSssEmployerShare)
        {
            Mock<IPolicyHelper> policy = new Mock<IPolicyHelper>();
            policy
                .Setup(x => x.SssCalculationBasis(OrganizationId))
                .Returns(SssCalculationBasis.GrossPay);

            var calculator = new SssCalculator(policy.Object, _socialSecurityBrackets, _payPeriod);

            var paystub = new Paystub() { GrossPay = salaryBracket / 2 };
            var previousPaystub = new Paystub() { GrossPay = salaryBracket / 2 };

            var salary = new Salary()
            {
                DoPaySSSContribution = true,
                BasicSalary = It.IsAny<decimal>(),
                AllowanceSalary = It.IsAny<decimal>(),
            };

            // Monthly
            var employeeMonthly = EmployeeMother
                .Simple(Employee.EmployeeTypeMonthly, OrganizationId);

            calculator
                .Calculate(paystub, previousPaystub, salary, employeeMonthly, _currentSystemOwner);

            Assert.AreEqual(expectedSssEmployerShare, paystub.SssEmployerShare);
            Assert.AreEqual(expectedSssEmployeeShare, paystub.SssEmployeeShare);

            // Fixed
            var employeeFixed = EmployeeMother
                .Simple(Employee.EmployeeTypeFixed, OrganizationId);

            calculator
                .Calculate(paystub, previousPaystub, salary, employeeFixed, _currentSystemOwner);

            Assert.AreEqual(expectedSssEmployeeShare, paystub.SssEmployeeShare);
            Assert.AreEqual(expectedSssEmployerShare, paystub.SssEmployerShare);
        }

        [TestCaseSource(typeof(SSSTestSource_2021), "Brackets_SalaryBased")]
        public void ShouldCalculateForDaily_WithGrossPayCalculationBasis(
            decimal salaryBracket,
            decimal expectedSssEmployeeShare,
            decimal expectedSssEmployerShare)
        {
            Mock<IPolicyHelper> policy = new Mock<IPolicyHelper>();
            policy
                .Setup(x => x.SssCalculationBasis(OrganizationId))
                .Returns(SssCalculationBasis.GrossPay);

            var calculator = new SssCalculator(policy.Object, _socialSecurityBrackets, _payPeriod);

            var salary = new Salary()
            {
                DoPaySSSContribution = true,
                BasicSalary = It.IsAny<decimal>(),
                AllowanceSalary = It.IsAny<decimal>(),
            };

            var paystub = new Paystub() { GrossPay = salaryBracket / 2 };
            var previousPaystub = new Paystub() { GrossPay = salaryBracket / 2 };

            // Daily
            var employee = EmployeeMother.Simple(Employee.EmployeeTypeDaily, OrganizationId);

            calculator.Calculate(paystub, previousPaystub, salary, employee, _currentSystemOwner);

            Assert.AreEqual(expectedSssEmployeeShare, paystub.SssEmployeeShare);
            Assert.AreEqual(expectedSssEmployerShare, paystub.SssEmployerShare);
        }

        #endregion GrossPay

        #region BasicMinusDeductions

        [TestCaseSource(typeof(SSSTestSource_2021), "Brackets_SalaryBased")]
        public void ShouldCalculateForDailyAndMonthly_WithBasicMinusDeductionsCalculationBasis(
            decimal salaryBracket,
            decimal expectedSssEmployeeShare,
            decimal expectedSssEmployerShare)
        {
            Mock<IPolicyHelper> policy = new Mock<IPolicyHelper>();
            policy
                .Setup(x => x.SssCalculationBasis(OrganizationId))
                .Returns(SssCalculationBasis.BasicMinusDeductions);

            var calculator = new SssCalculator(policy.Object, _socialSecurityBrackets, _payPeriod);

            var paystubMock = new Mock<Paystub>();
            paystubMock
                .Setup(x => x.TotalDaysPayWithOutOvertimeAndLeave)
                .Returns(salaryBracket / 2);
            Paystub paystub = paystubMock.Object;

            var previousPaystubMock = new Mock<Paystub>();
            previousPaystubMock
                .Setup(x => x.TotalDaysPayWithOutOvertimeAndLeave)
                .Returns(salaryBracket / 2);
            Paystub previousPaystub = previousPaystubMock.Object;

            var salary = new Salary()
            {
                DoPaySSSContribution = true,
                BasicSalary = It.IsAny<decimal>(),
                AllowanceSalary = It.IsAny<decimal>(),
            };

            // Monthly
            var employeeMonthly = EmployeeMother
                .Simple(Employee.EmployeeTypeMonthly, OrganizationId);

            calculator
                .Calculate(paystub, previousPaystub, salary, employeeMonthly, _currentSystemOwner);

            Assert.AreEqual(expectedSssEmployerShare, paystub.SssEmployerShare);
            Assert.AreEqual(expectedSssEmployeeShare, paystub.SssEmployeeShare);

            // Daily
            var employeeDaily = EmployeeMother
                .Simple(Employee.EmployeeTypeDaily, OrganizationId);

            calculator
                .Calculate(paystub, previousPaystub, salary, employeeDaily, _currentSystemOwner);

            Assert.AreEqual(expectedSssEmployeeShare, paystub.SssEmployeeShare);
            Assert.AreEqual(expectedSssEmployerShare, paystub.SssEmployerShare);
        }

        [TestCaseSource(typeof(SSSTestSource_2021), "Brackets_SalaryBased")]
        public void ShouldCalculateForFixed_WithBasicMinusDeductionsCalculationBasis(
           decimal salaryBracket,
           decimal expectedSssEmployeeShare,
           decimal expectedSssEmployerShare)
        {
            Mock<IPolicyHelper> policy = new Mock<IPolicyHelper>();
            policy
                .Setup(x => x.SssCalculationBasis(OrganizationId))
                .Returns(SssCalculationBasis.BasicMinusDeductions);

            var calculator = new SssCalculator(policy.Object, _socialSecurityBrackets, _payPeriod);

            var paystub = new Paystub();
            var previousPaystub = new Paystub();

            var salary = new Salary()
            {
                DoPaySSSContribution = true,
                BasicSalary = salaryBracket,
                AllowanceSalary = 0,
            };

            // Fixed
            var employee = EmployeeMother
                .Simple(Employee.EmployeeTypeFixed, OrganizationId);

            calculator
                .Calculate(paystub, previousPaystub, salary, employee, _currentSystemOwner);

            Assert.AreEqual(expectedSssEmployeeShare, paystub.SssEmployeeShare);
            Assert.AreEqual(expectedSssEmployerShare, paystub.SssEmployerShare);
        }

        #endregion BasicMinusDeductions

        #region BasicMinusDeductionsWithoutPremium

        [TestCaseSource(typeof(SSSTestSource_2021), "Brackets_SalaryBased")]
        public void ShouldCalculateForDaily_WithBasicMinusDeductionsWithoutPremiumCalculationBasis(
            decimal salaryBracket,
            decimal expectedSssEmployeeShare,
            decimal expectedSssEmployerShare)
        {
            Mock<IPolicyHelper> policy = new Mock<IPolicyHelper>();
            policy
                .Setup(x => x.SssCalculationBasis(OrganizationId))
                .Returns(SssCalculationBasis.BasicMinusDeductionsWithoutPremium);

            decimal workDaysPerMonth = 20;

            var calculator = new SssCalculator(policy.Object, _socialSecurityBrackets, _payPeriod);

            var paystubMock = new Mock<Paystub>();
            paystubMock
                .Setup(x => x.TotalWorkedHoursWithoutOvertimeAndLeave)
                .Returns(workDaysPerMonth / 2 * PayrollTools.WorkHoursPerDay);
            Paystub paystub = paystubMock.Object;

            var previousPaystubMock = new Mock<Paystub>();
            previousPaystubMock
                .Setup(x => x.TotalWorkedHoursWithoutOvertimeAndLeave)
                .Returns(workDaysPerMonth / 2 * PayrollTools.WorkHoursPerDay);
            Paystub previousPaystub = previousPaystubMock.Object;

            var salary = new Salary()
            {
                DoPaySSSContribution = true,
                BasicSalary = salaryBracket / workDaysPerMonth,
                AllowanceSalary = 0,
            };

            // Daily
            var employee = EmployeeMother
                .Simple(Employee.EmployeeTypeDaily, OrganizationId);
            employee.WorkDaysPerYear = workDaysPerMonth * PayrollTools.MonthsPerYear; // 240

            calculator
                .Calculate(paystub, previousPaystub, salary, employee, _currentSystemOwner);

            Assert.AreEqual(expectedSssEmployeeShare, paystub.SssEmployeeShare);
            Assert.AreEqual(expectedSssEmployerShare, paystub.SssEmployerShare);
        }

        [TestCaseSource(typeof(SSSTestSource_2021), "Brackets_SalaryBased")]
        public void ShouldCalculateForMonthly_WithBasicMinusDeductionsWithoutPremiumCalculationBasis(
            decimal salaryBracket,
            decimal expectedSssEmployeeShare,
            decimal expectedSssEmployerShare)
        {
            Mock<IPolicyHelper> policy = new Mock<IPolicyHelper>();
            policy
                .Setup(x => x.SssCalculationBasis(OrganizationId))
                .Returns(SssCalculationBasis.BasicMinusDeductionsWithoutPremium);

            decimal workDaysPerMonth = 20;

            var calculator = new SssCalculator(policy.Object, _socialSecurityBrackets, _payPeriod);

            var paystubMock = new Mock<Paystub>();
            paystubMock
                .Setup(x => x.TotalWorkedHoursWithoutOvertimeAndLeave)
                .Returns(workDaysPerMonth / 2 * PayrollTools.WorkHoursPerDay);
            Paystub paystub = paystubMock.Object;

            var previousPaystubMock = new Mock<Paystub>();
            previousPaystubMock
                .Setup(x => x.TotalWorkedHoursWithoutOvertimeAndLeave)
                .Returns(workDaysPerMonth / 2 * PayrollTools.WorkHoursPerDay);
            Paystub previousPaystub = previousPaystubMock.Object;

            var salary = new Salary()
            {
                DoPaySSSContribution = true,
                BasicSalary = salaryBracket,
                AllowanceSalary = 0,
            };

            // Monthly
            var employee = EmployeeMother
                .Simple(Employee.EmployeeTypeMonthly, OrganizationId);
            employee.WorkDaysPerYear = workDaysPerMonth * PayrollTools.MonthsPerYear; // 240

            calculator
                .Calculate(paystub, previousPaystub, salary, employee, _currentSystemOwner);

            Assert.AreEqual(expectedSssEmployerShare, paystub.SssEmployerShare);
            Assert.AreEqual(expectedSssEmployeeShare, paystub.SssEmployeeShare);
        }

        [TestCaseSource(typeof(SSSTestSource_2021), "Brackets_SalaryBased")]
        public void ShouldCalculateForFixed_WithBasicMinusDeductionsWithoutPremiumCalculationBasis(
           decimal salaryBracket,
           decimal expectedSssEmployeeShare,
           decimal expectedSssEmployerShare)
        {
            Mock<IPolicyHelper> policy = new Mock<IPolicyHelper>();
            policy
                .Setup(x => x.SssCalculationBasis(OrganizationId))
                .Returns(SssCalculationBasis.BasicMinusDeductions);

            var calculator = new SssCalculator(policy.Object, _socialSecurityBrackets, _payPeriod);

            var paystub = new Paystub();
            var previousPaystub = new Paystub();

            var salary = new Salary()
            {
                DoPaySSSContribution = true,
                BasicSalary = salaryBracket,
                AllowanceSalary = 0,
            };

            // Fixed
            var employee = EmployeeMother
                .Simple(Employee.EmployeeTypeFixed, OrganizationId);

            calculator
                .Calculate(paystub, previousPaystub, salary, employee, _currentSystemOwner);

            Assert.AreEqual(expectedSssEmployeeShare, paystub.SssEmployeeShare);
            Assert.AreEqual(expectedSssEmployerShare, paystub.SssEmployerShare);
        }

        #endregion BasicMinusDeductionsWithoutPremium
    }
}
