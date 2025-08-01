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

        #region BasicMinusDeductions_WithoutDeduction

        [TestCaseSource(typeof(PhilHealthTestSource), "TestData")]
        public void ShouldCalculateForDaily_WithBasicMinusDeductionsCalculationBasis_WithoutDeduction(
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
                .Setup(x => x.TotalDaysPayWithOutOvertimeAndLeave(false))
                .Returns(salaryBracket / 2);
            Paystub paystub = paystubMock.Object;

            var previousPaystubMock = new Mock<Paystub>();
            previousPaystubMock
                .Setup(x => x.TotalDaysPayWithOutOvertimeAndLeave(false))
                .Returns(salaryBracket / 2);
            Paystub previousPaystub = previousPaystubMock.Object;

            var salary = new Salary()
            {
                AutoComputePhilHealthContribution = true,
                BasicSalary = It.IsAny<decimal>(),
                AllowanceSalary = It.IsAny<decimal>(),
            };

            // Daily
            var employee = EmployeeMother
                .Simple(Employee.EmployeeTypeDaily, OrganizationId);

            calculator.Calculate(salary, paystub, previousPaystub, employee, _payPeriod, _currentSystemOwner);

            Assert.AreEqual(expectedPhilHealthEmployeeShare, paystub.PhilHealthEmployeeShare);
            Assert.AreEqual(expectedPhilHealthEmployerShare, paystub.PhilHealthEmployerShare);
        }

        [TestCaseSource(typeof(PhilHealthTestSource), "TestData")]
        public void ShouldCalculateForMonthly_WithBasicMinusDeductionsCalculationBasis_WithoutDeduction(
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
                .Setup(x => x.TotalDaysPayWithOutOvertimeAndLeave(true))
                .Returns(salaryBracket / 2);
            Paystub paystub = paystubMock.Object;

            var previousPaystubMock = new Mock<Paystub>();
            previousPaystubMock
                .Setup(x => x.TotalDaysPayWithOutOvertimeAndLeave(true))
                .Returns(salaryBracket / 2);
            Paystub previousPaystub = previousPaystubMock.Object;

            var salary = new Salary()
            {
                AutoComputePhilHealthContribution = true,
                BasicSalary = It.IsAny<decimal>(),
                AllowanceSalary = It.IsAny<decimal>(),
            };

            // Monthly
            var employee = EmployeeMother
                .Simple(Employee.EmployeeTypeMonthly, OrganizationId);

            calculator.Calculate(salary, paystub, previousPaystub, employee, _payPeriod, _currentSystemOwner);

            Assert.AreEqual(expectedPhilHealthEmployeeShare, paystub.PhilHealthEmployeeShare);
            Assert.AreEqual(expectedPhilHealthEmployerShare, paystub.PhilHealthEmployerShare);
        }

        [TestCaseSource(typeof(PhilHealthTestSource), "TestData")]
        public void ShouldCalculateForFixed_WithBasicMinusDeductionsCalculationBasis_WithoutDeduction(
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

        #endregion BasicMinusDeductions_WithoutDeduction

        #region BasicMinusDeductions_WithDeduction

        [TestCaseSource(typeof(PhilHealthTestSource), "TestData")]
        public void ShouldCalculateForDaily_WithBasicMinusDeductionsCalculationBasis_WithDeduction(
            decimal salaryBracket,
            decimal expectedPhilHealthEmployeeShare,
            decimal expectedPhilHealthEmployerShare)
        {
            _policyMock
                .Setup(x => x.CalculationBasis(OrganizationId))
                .Returns(PhilHealthCalculationBasis.BasicMinusDeductions);

            decimal workPayPerCutOff = salaryBracket / 2;

            var calculator = new PhilHealthCalculator(_policyMock.Object);

            var paystub = new Paystub();
            PaystubHelper.SetTotalWorkedPayWithoutOvertimeAndLeaveValue(
                workPayPerCutOff, paystub);

            PaystubHelper.SetPayDeductionsValue(paystub);

            var previousPaystub = new Paystub();
            PaystubHelper.SetTotalWorkedPayWithoutOvertimeAndLeaveValue(
                workPayPerCutOff, previousPaystub);

            PaystubHelper.SetPayDeductionsValue(previousPaystub);

            var salary = new Salary()
            {
                AutoComputePhilHealthContribution = true,
                BasicSalary = It.IsAny<decimal>(),
                AllowanceSalary = It.IsAny<decimal>(),
            };

            // Daily
            var employee = EmployeeMother
                .Simple(Employee.EmployeeTypeDaily, OrganizationId);

            calculator.Calculate(salary, paystub, previousPaystub, employee, _payPeriod, _currentSystemOwner);

            Assert.AreEqual(expectedPhilHealthEmployeeShare, paystub.PhilHealthEmployeeShare);
            Assert.AreEqual(expectedPhilHealthEmployerShare, paystub.PhilHealthEmployerShare);
        }

        [TestCaseSource(typeof(PhilHealthTestSource), "TestData")]
        public void ShouldCalculateForMonthly_WithBasicMinusDeductionsCalculationBasis_WithDeduction(
            decimal salaryBracket,
            decimal expectedPhilHealthEmployeeShare,
            decimal expectedPhilHealthEmployerShare)
        {
            _policyMock
                .Setup(x => x.CalculationBasis(OrganizationId))
                .Returns(PhilHealthCalculationBasis.BasicMinusDeductions);

            decimal workPayPerCutOff = salaryBracket / 2;

            var calculator = new PhilHealthCalculator(_policyMock.Object);

            var paystub = new Paystub();
            PaystubHelper.SetTotalWorkedPayWithoutOvertimeAndLeaveValue(
                workPayPerCutOff, paystub);

            PaystubHelper.SetPayDeductionsValue(paystub);

            // In monthly, RegularPay still has AbsenceDeduction, LateDeduction, and UndertimeDeduction
            paystub.RegularPay +=
                paystub.AbsenceDeduction +
                paystub.LateDeduction +
                paystub.UndertimeDeduction;

            var previousPaystub = new Paystub();
            PaystubHelper.SetTotalWorkedPayWithoutOvertimeAndLeaveValue(
                workPayPerCutOff, previousPaystub);

            PaystubHelper.SetPayDeductionsValue(previousPaystub);

            // In monthly, RegularPay still has AbsenceDeduction, LateDeduction, and UndertimeDeduction
            previousPaystub.RegularPay +=
                previousPaystub.AbsenceDeduction +
                previousPaystub.LateDeduction +
                previousPaystub.UndertimeDeduction;

            var salary = new Salary()
            {
                AutoComputePhilHealthContribution = true,
                BasicSalary = It.IsAny<decimal>(),
                AllowanceSalary = It.IsAny<decimal>(),
            };

            // Monthly
            var employee = EmployeeMother
                .Simple(Employee.EmployeeTypeMonthly, OrganizationId);

            calculator.Calculate(salary, paystub, previousPaystub, employee, _payPeriod, _currentSystemOwner);

            Assert.AreEqual(expectedPhilHealthEmployeeShare, paystub.PhilHealthEmployeeShare);
            Assert.AreEqual(expectedPhilHealthEmployerShare, paystub.PhilHealthEmployerShare);
        }

        #endregion BasicMinusDeductions_WithDeduction

        #region BasicMinusDeductionsWithoutPremium_WithoutDeduction

        [TestCaseSource(typeof(PhilHealthTestSource), "TestData")]
        public void ShouldCalculateForDaily_WithBasicMinusDeductionsWithoutPremiumCalculationBasis_WithoutDeduction(
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
                .Setup(x => x.TotalWorkedHoursWithoutOvertimeAndLeave(false))
                .Returns(workDaysPerMonth / 2 * PayrollTools.WorkHoursPerDay);
            Paystub paystub = paystubMock.Object;

            var previousPaystubMock = new Mock<Paystub>();
            previousPaystubMock
                .Setup(x => x.TotalWorkedHoursWithoutOvertimeAndLeave(false))
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
        public void ShouldCalculateForMonthly_WithBasicMinusDeductionsWithoutPremiumCalculationBasis_WithoutDeduction(
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
                .Setup(x => x.TotalWorkedHoursWithoutOvertimeAndLeave(true))
                .Returns(workDaysPerMonth / 2 * PayrollTools.WorkHoursPerDay);
            Paystub paystub = paystubMock.Object;

            var previousPaystubMock = new Mock<Paystub>();
            previousPaystubMock
                .Setup(x => x.TotalWorkedHoursWithoutOvertimeAndLeave(true))
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

        #region BasicMinusDeductionsWithoutPremium_WithDeduction

        [TestCaseSource(typeof(PhilHealthTestSource), "TestData")]
        public void ShouldCalculateForDaily_WithBasicMinusDeductionsWithoutPremiumCalculationBasis_WithDeduction(
            decimal salaryBracket,
            decimal expectedPhilHealthEmployeeShare,
            decimal expectedPhilHealthEmployerShare)
        {
            _policyMock
                .Setup(x => x.CalculationBasis(OrganizationId))
                .Returns(PhilHealthCalculationBasis.BasicMinusDeductionsWithoutPremium);

            decimal workDaysPerMonth = 20;
            decimal workHoursPerCutOff = workDaysPerMonth / 2 * PayrollTools.WorkHoursPerDay;

            var calculator = new PhilHealthCalculator(_policyMock.Object);

            var paystub = new Paystub();
            PaystubHelper.SetTotalWorkedHoursWithoutOvertimeAndLeaveValue(
                workHoursPerCutOff, paystub);

            PaystubHelper.SetHourDeductionsValue(paystub);

            var previousPaystub = new Paystub();
            PaystubHelper.SetTotalWorkedHoursWithoutOvertimeAndLeaveValue(
                workHoursPerCutOff, previousPaystub);

            PaystubHelper.SetHourDeductionsValue(previousPaystub);

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
        public void ShouldCalculateForMonthly_WithBasicMinusDeductionsWithoutPremiumCalculationBasis_WithDeduction(
            decimal salaryBracket,
            decimal expectedPhilHealthEmployeeShare,
            decimal expectedPhilHealthEmployerShare)
        {
            _policyMock
                .Setup(x => x.CalculationBasis(OrganizationId))
                .Returns(PhilHealthCalculationBasis.BasicMinusDeductionsWithoutPremium);

            decimal workDaysPerMonth = 20;
            decimal workHoursPerCutOff = workDaysPerMonth / 2 * PayrollTools.WorkHoursPerDay;

            var calculator = new PhilHealthCalculator(_policyMock.Object);

            var paystub = new Paystub();
            PaystubHelper.SetTotalWorkedHoursWithoutOvertimeAndLeaveValue(
                workHoursPerCutOff, paystub);

            PaystubHelper.SetHourDeductionsValue(paystub);

            // In monthly, RegularHours still has AbsentHours, LateHours, and UndertimeHours
            paystub.RegularHours +=
                paystub.AbsentHours +
                paystub.LateHours +
                paystub.UndertimeHours;

            var previousPaystub = new Paystub();
            PaystubHelper.SetTotalWorkedHoursWithoutOvertimeAndLeaveValue(
                workHoursPerCutOff, previousPaystub);

            PaystubHelper.SetHourDeductionsValue(previousPaystub);

            // In monthly, RegularHours still has AbsentHours, LateHours, and UndertimeHours
            previousPaystub.RegularHours +=
                previousPaystub.AbsentHours +
                previousPaystub.LateHours +
                previousPaystub.UndertimeHours;

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

        #endregion BasicMinusDeductionsWithoutPremium_WithDeduction
    }
}
