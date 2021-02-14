using AccuPay.Core.Entities;
using AccuPay.Core.Enums;
using AccuPay.Core.Helpers;
using AccuPay.Core.Interfaces;
using AccuPay.Core.Services;
using Moq;
using NUnit.Framework;
using System;

namespace AccuPay.Core.UnitTests.PhilHealth
{
    public class PhilHealthCalculatorTest
    {
        private const int OrganizationId = 1;

        private PayPeriod _payPeriod;
        private string _currentSystemOwner;
        private Mock<IPhilHealthPolicy> _policyMock;

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

            _currentSystemOwner = string.Empty;

            _policyMock = new Mock<IPhilHealthPolicy>();

            _policyMock
                .Setup(x => x.OddCentDifference)
                .Returns(true);

            _policyMock
                .Setup(x => x.MinimumContribution)
                .Returns(300);

            _policyMock
                .Setup(x => x.MaximumContribution)
                .Returns(1800);

            _policyMock
                .Setup(x => x.Rate)
                .Returns(3);
        }

        #region BasicSalary

        [TestCaseSource(typeof(PhilHealthTestSource), "TestData")]
        public void ShouldCalculateForMonthlyAndFixed_WithBasicSalaryCalculationBasis(
            decimal salaryBracket,
            decimal expectedPhilHealthEmployeeShare,
            decimal expectedPhilHealthEmployerShare)
        {
            _policyMock
                .Setup(x => x.CalculationBasis(OrganizationId))
                .Returns(PhilHealthCalculationBasis.BasicSalary);

            var calculator = new PhilHealthCalculator(_policyMock.Object);

            var salary = new Salary()
            {
                AutoComputePhilHealthContribution = true,
                BasicSalary = salaryBracket,
                AllowanceSalary = It.IsAny<decimal>(),
            };

            var paystub = new Paystub();
            var previousPaystub = new Paystub();

            // Monthly
            var employeeMonthly = EmployeeMother.Simple(Employee.EmployeeTypeMonthly, OrganizationId);

            calculator.Calculate(salary, paystub, previousPaystub, employeeMonthly, _payPeriod, _currentSystemOwner);

            Assert.AreEqual(expectedPhilHealthEmployeeShare, paystub.PhilHealthEmployeeShare);
            Assert.AreEqual(expectedPhilHealthEmployerShare, paystub.PhilHealthEmployerShare);

            // Fixed
            var employeeFixed = EmployeeMother.Simple(Employee.EmployeeTypeFixed, OrganizationId);

            calculator.Calculate(salary, paystub, previousPaystub, employeeFixed, _payPeriod, _currentSystemOwner);

            Assert.AreEqual(expectedPhilHealthEmployeeShare, paystub.PhilHealthEmployeeShare);
            Assert.AreEqual(expectedPhilHealthEmployerShare, paystub.PhilHealthEmployerShare);
        }

        [TestCaseSource(typeof(PhilHealthTestSource), "TestData")]
        public void ShouldCalculateForDaily_WithBasicSalaryCalculationBasis(
           decimal salaryBracket,
           decimal expectedPhilHealthEmployeeShare,
           decimal expectedPhilHealthEmployerShare)
        {
            _policyMock
                .Setup(x => x.CalculationBasis(OrganizationId))
                .Returns(PhilHealthCalculationBasis.BasicSalary);

            decimal workDaysPerMonth = 20;

            var calculator = new PhilHealthCalculator(_policyMock.Object);

            var salary = new Salary()
            {
                AutoComputePhilHealthContribution = true,
                BasicSalary = salaryBracket / workDaysPerMonth,
                AllowanceSalary = It.IsAny<decimal>(),
            };

            var paystub = new Paystub();
            var previousPaystub = new Paystub();

            // Daily
            var employee = EmployeeMother.Simple(Employee.EmployeeTypeDaily, OrganizationId);
            employee.WorkDaysPerYear = workDaysPerMonth * PayrollTools.MonthsPerYear; //240;

            calculator.Calculate(salary, paystub, previousPaystub, employee, _payPeriod, _currentSystemOwner);

            Assert.AreEqual(expectedPhilHealthEmployeeShare, paystub.PhilHealthEmployeeShare);
            Assert.AreEqual(expectedPhilHealthEmployerShare, paystub.PhilHealthEmployerShare);
        }

        #endregion BasicSalary

        #region Earnings

        [TestCaseSource(typeof(PhilHealthTestSource), "TestData")]
        public void ShouldCalculateForMonthlyAndFixed_WithEarningsCalculationBasis(
            decimal salaryBracket,
            decimal expectedPhilHealthEmployeeShare,
            decimal expectedPhilHealthEmployerShare)
        {
            _policyMock
                .Setup(x => x.CalculationBasis(OrganizationId))
                .Returns(PhilHealthCalculationBasis.Earnings);

            var calculator = new PhilHealthCalculator(_policyMock.Object);

            var paystub = new Paystub() { TotalEarnings = salaryBracket / 2 };
            var previousPaystub = new Paystub() { TotalEarnings = salaryBracket / 2 };

            var salary = new Salary()
            {
                AutoComputePhilHealthContribution = true,
                BasicSalary = It.IsAny<decimal>(),
                AllowanceSalary = It.IsAny<decimal>(),
            };

            // Monthly
            var employeeMonthly = EmployeeMother
                .Simple(Employee.EmployeeTypeMonthly, OrganizationId);

            calculator.Calculate(salary, paystub, previousPaystub, employeeMonthly, _payPeriod, _currentSystemOwner);

            Assert.AreEqual(expectedPhilHealthEmployeeShare, paystub.PhilHealthEmployeeShare);
            Assert.AreEqual(expectedPhilHealthEmployerShare, paystub.PhilHealthEmployerShare);

            // Fixed
            var employeeFixed = EmployeeMother
                .Simple(Employee.EmployeeTypeFixed, OrganizationId);

            calculator.Calculate(salary, paystub, previousPaystub, employeeFixed, _payPeriod, _currentSystemOwner);

            Assert.AreEqual(expectedPhilHealthEmployeeShare, paystub.PhilHealthEmployeeShare);
            Assert.AreEqual(expectedPhilHealthEmployerShare, paystub.PhilHealthEmployerShare);
        }

        [TestCaseSource(typeof(PhilHealthTestSource), "TestData")]
        public void ShouldCalculateForDaily_WithEarningsCalculationBasis(
            decimal salaryBracket,
            decimal expectedPhilHealthEmployeeShare,
            decimal expectedPhilHealthEmployerShare)
        {
            _policyMock
                .Setup(x => x.CalculationBasis(OrganizationId))
                .Returns(PhilHealthCalculationBasis.Earnings);

            var calculator = new PhilHealthCalculator(_policyMock.Object);

            var salary = new Salary()
            {
                AutoComputePhilHealthContribution = true,
                BasicSalary = It.IsAny<decimal>(),
                AllowanceSalary = It.IsAny<decimal>(),
            };

            var paystub = new Paystub() { TotalEarnings = salaryBracket / 2 };
            var previousPaystub = new Paystub() { TotalEarnings = salaryBracket / 2 };

            // Daily
            var employee = EmployeeMother.Simple(Employee.EmployeeTypeDaily, OrganizationId);

            calculator.Calculate(salary, paystub, previousPaystub, employee, _payPeriod, _currentSystemOwner);

            Assert.AreEqual(expectedPhilHealthEmployeeShare, paystub.PhilHealthEmployeeShare);
            Assert.AreEqual(expectedPhilHealthEmployerShare, paystub.PhilHealthEmployerShare);
        }

        #endregion Earnings

        #region GrossPay

        [TestCaseSource(typeof(PhilHealthTestSource), "TestData")]
        public void ShouldCalculateForMonthlyAndFixed_WithGrossPayCalculationBasis(
            decimal salaryBracket,
            decimal expectedPhilHealthEmployeeShare,
            decimal expectedPhilHealthEmployerShare)
        {
            _policyMock
                .Setup(x => x.CalculationBasis(OrganizationId))
                .Returns(PhilHealthCalculationBasis.GrossPay);

            var calculator = new PhilHealthCalculator(_policyMock.Object);

            var paystub = new Paystub() { GrossPay = salaryBracket / 2 };
            var previousPaystub = new Paystub() { GrossPay = salaryBracket / 2 };

            var salary = new Salary()
            {
                AutoComputePhilHealthContribution = true,
                BasicSalary = It.IsAny<decimal>(),
                AllowanceSalary = It.IsAny<decimal>(),
            };

            // Monthly
            var employeeMonthly = EmployeeMother
                .Simple(Employee.EmployeeTypeMonthly, OrganizationId);

            calculator.Calculate(salary, paystub, previousPaystub, employeeMonthly, _payPeriod, _currentSystemOwner);

            Assert.AreEqual(expectedPhilHealthEmployeeShare, paystub.PhilHealthEmployeeShare);
            Assert.AreEqual(expectedPhilHealthEmployerShare, paystub.PhilHealthEmployerShare);

            // Fixed
            var employeeFixed = EmployeeMother
                .Simple(Employee.EmployeeTypeFixed, OrganizationId);

            calculator.Calculate(salary, paystub, previousPaystub, employeeFixed, _payPeriod, _currentSystemOwner);

            Assert.AreEqual(expectedPhilHealthEmployeeShare, paystub.PhilHealthEmployeeShare);
            Assert.AreEqual(expectedPhilHealthEmployerShare, paystub.PhilHealthEmployerShare);
        }

        [TestCaseSource(typeof(PhilHealthTestSource), "TestData")]
        public void ShouldCalculateForDaily_WithGrossPayCalculationBasis(
            decimal salaryBracket,
            decimal expectedPhilHealthEmployeeShare,
            decimal expectedPhilHealthEmployerShare)
        {
            _policyMock
                .Setup(x => x.CalculationBasis(OrganizationId))
                .Returns(PhilHealthCalculationBasis.GrossPay);

            var calculator = new PhilHealthCalculator(_policyMock.Object);

            var salary = new Salary()
            {
                AutoComputePhilHealthContribution = true,
                BasicSalary = It.IsAny<decimal>(),
                AllowanceSalary = It.IsAny<decimal>(),
            };

            var paystub = new Paystub() { GrossPay = salaryBracket / 2 };
            var previousPaystub = new Paystub() { GrossPay = salaryBracket / 2 };

            // Daily
            var employee = EmployeeMother.Simple(Employee.EmployeeTypeDaily, OrganizationId);

            calculator.Calculate(salary, paystub, previousPaystub, employee, _payPeriod, _currentSystemOwner);

            Assert.AreEqual(expectedPhilHealthEmployeeShare, paystub.PhilHealthEmployeeShare);
            Assert.AreEqual(expectedPhilHealthEmployerShare, paystub.PhilHealthEmployerShare);
        }

        #endregion GrossPay

        #region BasicMinusDeductions

        [TestCaseSource(typeof(PhilHealthTestSource), "TestData")]
        public void ShouldCalculateForDailyAndMonthly_WithBasicMinusDeductionsCalculationBasis_TotalDaysPayWithOutOvertimeAndLeaveOverriden(
            decimal salaryBracket,
            decimal expectedPhilHealthEmployeeShare,
            decimal expectedPhilHealthEmployerShare)
        {
            _policyMock
                .Setup(x => x.CalculationBasis(OrganizationId))
                .Returns(PhilHealthCalculationBasis.BasicMinusDeductions);

            var calculator = new PhilHealthCalculator(_policyMock.Object);

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
                AutoComputePhilHealthContribution = true,
                BasicSalary = It.IsAny<decimal>(),
                AllowanceSalary = It.IsAny<decimal>(),
            };

            // Monthly
            var employeeMonthly = EmployeeMother
                .Simple(Employee.EmployeeTypeMonthly, OrganizationId);

            calculator.Calculate(salary, paystub, previousPaystub, employeeMonthly, _payPeriod, _currentSystemOwner);

            Assert.AreEqual(expectedPhilHealthEmployeeShare, paystub.PhilHealthEmployeeShare);
            Assert.AreEqual(expectedPhilHealthEmployerShare, paystub.PhilHealthEmployerShare);

            // Daily
            var employeeDaily = EmployeeMother
                .Simple(Employee.EmployeeTypeDaily, OrganizationId);

            calculator.Calculate(salary, paystub, previousPaystub, employeeDaily, _payPeriod, _currentSystemOwner);

            Assert.AreEqual(expectedPhilHealthEmployeeShare, paystub.PhilHealthEmployeeShare);
            Assert.AreEqual(expectedPhilHealthEmployerShare, paystub.PhilHealthEmployerShare);
        }

        [TestCaseSource(typeof(PhilHealthTestSource), "TestData")]
        public void ShouldCalculateForFixed_WithBasicMinusDeductionsCalculationBasis_TotalDaysPayWithOutOvertimeAndLeaveOverriden(
           decimal salaryBracket,
           decimal expectedPhilHealthEmployeeShare,
           decimal expectedPhilHealthEmployerShare)
        {
            _policyMock
                .Setup(x => x.CalculationBasis(OrganizationId))
                .Returns(PhilHealthCalculationBasis.BasicMinusDeductions);

            var calculator = new PhilHealthCalculator(_policyMock.Object);

            var paystub = new Paystub();
            var previousPaystub = new Paystub();

            var salary = new Salary()
            {
                AutoComputePhilHealthContribution = true,
                BasicSalary = salaryBracket,
                AllowanceSalary = 0,
            };

            // Fixed
            var employee = EmployeeMother
                .Simple(Employee.EmployeeTypeFixed, OrganizationId);

            calculator.Calculate(salary, paystub, previousPaystub, employee, _payPeriod, _currentSystemOwner);

            Assert.AreEqual(expectedPhilHealthEmployeeShare, paystub.PhilHealthEmployeeShare);
            Assert.AreEqual(expectedPhilHealthEmployerShare, paystub.PhilHealthEmployerShare);
        }

        #endregion BasicMinusDeductions

        #region BasicMinusDeductionsWithoutPremium_WithoutDeduction

        [TestCaseSource(typeof(PhilHealthTestSource), "TestData")]
        public void ShouldCalculateForDaily_WithoutDeduction(
            decimal salaryBracket,
            decimal expectedPhilHealthEmployeeShare,
            decimal expectedPhilHealthEmployerShare)
        {
            _policyMock
                .Setup(x => x.CalculationBasis(OrganizationId))
                .Returns(PhilHealthCalculationBasis.BasicMinusDeductionsWithoutPremium);

            decimal workDaysPerMonth = 20;

            var calculator = new PhilHealthCalculator(_policyMock.Object);

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
                AutoComputePhilHealthContribution = true,
                BasicSalary = salaryBracket / workDaysPerMonth,
                AllowanceSalary = 0,
            };

            // Daily
            var employee = EmployeeMother
                .Simple(Employee.EmployeeTypeDaily, OrganizationId);
            employee.WorkDaysPerYear = workDaysPerMonth * PayrollTools.MonthsPerYear; // 240

            calculator.Calculate(salary, paystub, previousPaystub, employee, _payPeriod, _currentSystemOwner);

            Assert.AreEqual(expectedPhilHealthEmployeeShare, paystub.PhilHealthEmployeeShare);
            Assert.AreEqual(expectedPhilHealthEmployerShare, paystub.PhilHealthEmployerShare);
        }

        [TestCaseSource(typeof(PhilHealthTestSource), "TestData")]
        public void ShouldCalculateForMonthly_WithoutDeduction(
            decimal salaryBracket,
            decimal expectedPhilHealthEmployeeShare,
            decimal expectedPhilHealthEmployerShare)
        {
            _policyMock
                .Setup(x => x.CalculationBasis(OrganizationId))
                .Returns(PhilHealthCalculationBasis.BasicMinusDeductionsWithoutPremium);

            decimal workDaysPerMonth = 20;

            var calculator = new PhilHealthCalculator(_policyMock.Object);

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
                AutoComputePhilHealthContribution = true,
                BasicSalary = salaryBracket,
                AllowanceSalary = 0,
            };

            // Monthly
            var employee = EmployeeMother
                .Simple(Employee.EmployeeTypeMonthly, OrganizationId);
            employee.WorkDaysPerYear = workDaysPerMonth * PayrollTools.MonthsPerYear; // 240

            calculator.Calculate(salary, paystub, previousPaystub, employee, _payPeriod, _currentSystemOwner);

            Assert.AreEqual(expectedPhilHealthEmployeeShare, paystub.PhilHealthEmployeeShare);
            Assert.AreEqual(expectedPhilHealthEmployerShare, paystub.PhilHealthEmployerShare);
        }

        [TestCaseSource(typeof(PhilHealthTestSource), "TestData")]
        public void ShouldCalculateForFixed_WithBasicMinusDeductionsWithoutPremiumCalculationBasis(
           decimal salaryBracket,
           decimal expectedPhilHealthEmployeeShare,
           decimal expectedPhilHealthEmployerShare)
        {
            _policyMock
                .Setup(x => x.CalculationBasis(OrganizationId))
                .Returns(PhilHealthCalculationBasis.BasicMinusDeductionsWithoutPremium);

            var calculator = new PhilHealthCalculator(_policyMock.Object);

            var paystub = new Paystub();
            var previousPaystub = new Paystub();

            var salary = new Salary()
            {
                AutoComputePhilHealthContribution = true,
                BasicSalary = salaryBracket,
                AllowanceSalary = 0,
            };

            // Fixed
            var employee = EmployeeMother
                .Simple(Employee.EmployeeTypeFixed, OrganizationId);

            calculator.Calculate(salary, paystub, previousPaystub, employee, _payPeriod, _currentSystemOwner);

            Assert.AreEqual(expectedPhilHealthEmployeeShare, paystub.PhilHealthEmployeeShare);
            Assert.AreEqual(expectedPhilHealthEmployerShare, paystub.PhilHealthEmployerShare);
        }

        #endregion BasicMinusDeductionsWithoutPremium_WithoutDeduction
    }
}
