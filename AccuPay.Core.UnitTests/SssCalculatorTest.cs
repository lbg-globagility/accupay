using AccuPay.Core.Entities;
using AccuPay.Core.Enums;
using AccuPay.Core.Helpers;
using AccuPay.Core.Interfaces;
using AccuPay.Core.Services;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace AccuPay.Core.UnitTests
{
    public class SssCalculatorTest
    {
        [TestCaseSource(typeof(SSSData), "Brackets_2019_SalaryBased")]
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

        [TestCaseSource(typeof(SSSData), "Brackets_2019_PaystubBased")]
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

        [TestCaseSource(typeof(SSSData), "Brackets_2019_PaystubBased")]
        public void ShouldCalculate_WithEarningsCalculationBasis(
            decimal previousTotalDaysPayWithOutOvertimeAndLeave,
            decimal totalDaysPayWithOutOvertimeAndLeave,
            decimal previousTotalEarnings,
            decimal currentTotalEarnings,
            decimal previousGrossPay,
            decimal currentGrossPay,
            decimal sssEmployeeShare,
            decimal sssEmployerShare)
        {
            BaseShouldCalculatePaystubBased(
                SssCalculationBasis.Earnings,
                previousTotalDaysPayWithOutOvertimeAndLeave: previousTotalDaysPayWithOutOvertimeAndLeave,
                totalDaysPayWithOutOvertimeAndLeave: totalDaysPayWithOutOvertimeAndLeave,
                previousTotalEarnings: previousTotalEarnings,
                currentTotalEarnings: currentTotalEarnings,
                previousGrossPay: previousGrossPay,
                currentGrossPay: currentGrossPay,
                expectedSssEmployeeShare: sssEmployeeShare,
                expectedSssEmployerShare: sssEmployerShare);
        }

        [TestCaseSource(typeof(SSSData), "Brackets_2019_PaystubBased")]
        public void ShouldCalculate_WithGrossPayCalculationBasis(
            decimal previousTotalDaysPayWithOutOvertimeAndLeave,
            decimal totalDaysPayWithOutOvertimeAndLeave,
            decimal previousTotalEarnings,
            decimal currentTotalEarnings,
            decimal previousGrossPay,
            decimal currentGrossPay,
            decimal sssEmployeeShare,
            decimal sssEmployerShare)
        {
            BaseShouldCalculatePaystubBased(
                SssCalculationBasis.GrossPay,
                previousTotalDaysPayWithOutOvertimeAndLeave: previousTotalDaysPayWithOutOvertimeAndLeave,
                totalDaysPayWithOutOvertimeAndLeave: totalDaysPayWithOutOvertimeAndLeave,
                previousTotalEarnings: previousTotalEarnings,
                currentTotalEarnings: currentTotalEarnings,
                previousGrossPay: previousGrossPay,
                currentGrossPay: currentGrossPay,
                expectedSssEmployeeShare: sssEmployeeShare,
                expectedSssEmployerShare: sssEmployerShare);
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
            List<SocialSecurityBracket> socialSecurityBrackets = SSSBrackets_2019();

            var calculator = new SssCalculator(policy, socialSecurityBrackets);

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

            var payPeriod = new PayPeriod()
            {
                Half = PayPeriod.FirstHalfValue
            };

            string currentSystemOwner = string.Empty;

            calculator.Calculate(paystub, previousPaystub, salary, employee, payPeriod, currentSystemOwner);

            Assert.AreEqual(expectedSssEmployerShare, paystub.SssEmployerShare);
            Assert.AreEqual(expectedSssEmployeeShare, paystub.SssEmployeeShare);
        }

        private static List<SocialSecurityBracket> SSSBrackets_2019()
        {
            return new List<SocialSecurityBracket>()
            {
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 0.00M,
                    RangeToAmount = 2249.99M,
                    MonthlySalaryCredit = 2000.00M,
                    EmployeeContributionAmount = 80.00M,
                    EmployerContributionAmount = 160.00M,
                    EmployeeECAmount = 10.00M,
                    EffectiveDateFrom = new DateTime(2019, 4, 1),
                    EffectiveDateTo = new DateTime(2022, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 2250.00M,
                    RangeToAmount = 2749.99M,
                    MonthlySalaryCredit = 2500.00M,
                    EmployeeContributionAmount = 100.00M,
                    EmployerContributionAmount = 200.00M,
                    EmployeeECAmount = 10.00M,
                    EffectiveDateFrom = new DateTime(2019, 4, 1),
                    EffectiveDateTo = new DateTime(2022, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 2750.00M,
                    RangeToAmount = 3249.99M,
                    MonthlySalaryCredit = 3000.00M,
                    EmployeeContributionAmount = 120.00M,
                    EmployerContributionAmount = 240.00M,
                    EmployeeECAmount = 10.00M,
                    EffectiveDateFrom = new DateTime(2019, 4, 1),
                    EffectiveDateTo = new DateTime(2022, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 3250.00M,
                    RangeToAmount = 3749.99M,
                    MonthlySalaryCredit = 3500.00M,
                    EmployeeContributionAmount = 140.00M,
                    EmployerContributionAmount = 280.00M,
                    EmployeeECAmount = 10.00M,
                    EffectiveDateFrom = new DateTime(2019, 4, 1),
                    EffectiveDateTo = new DateTime(2022, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 3750.00M,
                    RangeToAmount = 4249.99M,
                    MonthlySalaryCredit = 4000.00M,
                    EmployeeContributionAmount = 160.00M,
                    EmployerContributionAmount = 320.00M,
                    EmployeeECAmount = 10.00M,
                    EffectiveDateFrom = new DateTime(2019, 4, 1),
                    EffectiveDateTo = new DateTime(2022, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 4250.00M,
                    RangeToAmount = 4749.99M,
                    MonthlySalaryCredit = 4500.00M,
                    EmployeeContributionAmount = 180.00M,
                    EmployerContributionAmount = 360.00M,
                    EmployeeECAmount = 10.00M,
                    EffectiveDateFrom = new DateTime(2019, 4, 1),
                    EffectiveDateTo = new DateTime(2022, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 4750.00M,
                    RangeToAmount = 5249.99M,
                    MonthlySalaryCredit = 5000.00M,
                    EmployeeContributionAmount = 200.00M,
                    EmployerContributionAmount = 400.00M,
                    EmployeeECAmount = 10.00M,
                    EffectiveDateFrom = new DateTime(2019, 4, 1),
                    EffectiveDateTo = new DateTime(2022, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 5250.00M,
                    RangeToAmount = 5749.99M,
                    MonthlySalaryCredit = 5500.00M,
                    EmployeeContributionAmount = 220.00M,
                    EmployerContributionAmount = 440.00M,
                    EmployeeECAmount = 10.00M,
                    EffectiveDateFrom = new DateTime(2019, 4, 1),
                    EffectiveDateTo = new DateTime(2022, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 5750.00M,
                    RangeToAmount = 6249.99M,
                    MonthlySalaryCredit = 6000.00M,
                    EmployeeContributionAmount = 240.00M,
                    EmployerContributionAmount = 480.00M,
                    EmployeeECAmount = 10.00M,
                    EffectiveDateFrom = new DateTime(2019, 4, 1),
                    EffectiveDateTo = new DateTime(2022, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 6250.00M,
                    RangeToAmount = 6749.99M,
                    MonthlySalaryCredit = 6500.00M,
                    EmployeeContributionAmount = 260.00M,
                    EmployerContributionAmount = 520.00M,
                    EmployeeECAmount = 10.00M,
                    EffectiveDateFrom = new DateTime(2019, 4, 1),
                    EffectiveDateTo = new DateTime(2022, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 6750.00M,
                    RangeToAmount = 7249.99M,
                    MonthlySalaryCredit = 7000.00M,
                    EmployeeContributionAmount = 280.00M,
                    EmployerContributionAmount = 560.00M,
                    EmployeeECAmount = 10.00M,
                    EffectiveDateFrom = new DateTime(2019, 4, 1),
                    EffectiveDateTo = new DateTime(2022, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 7250.00M,
                    RangeToAmount = 7749.99M,
                    MonthlySalaryCredit = 7500.00M,
                    EmployeeContributionAmount = 300.00M,
                    EmployerContributionAmount = 600.00M,
                    EmployeeECAmount = 10.00M,
                    EffectiveDateFrom = new DateTime(2019, 4, 1),
                    EffectiveDateTo = new DateTime(2022, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 7750.00M,
                    RangeToAmount = 8249.99M,
                    MonthlySalaryCredit = 8000.00M,
                    EmployeeContributionAmount = 320.00M,
                    EmployerContributionAmount = 640.00M,
                    EmployeeECAmount = 10.00M,
                    EffectiveDateFrom = new DateTime(2019, 4, 1),
                    EffectiveDateTo = new DateTime(2022, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 8250.00M,
                    RangeToAmount = 8749.99M,
                    MonthlySalaryCredit = 8500.00M,
                    EmployeeContributionAmount = 340.00M,
                    EmployerContributionAmount = 680.00M,
                    EmployeeECAmount = 10.00M,
                    EffectiveDateFrom = new DateTime(2019, 4, 1),
                    EffectiveDateTo = new DateTime(2022, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 8750.00M,
                    RangeToAmount = 9249.99M,
                    MonthlySalaryCredit = 9000.00M,
                    EmployeeContributionAmount = 360.00M,
                    EmployerContributionAmount = 720.00M,
                    EmployeeECAmount = 10.00M,
                    EffectiveDateFrom = new DateTime(2019, 4, 1),
                    EffectiveDateTo = new DateTime(2022, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 9250.00M,
                    RangeToAmount = 9749.99M,
                    MonthlySalaryCredit = 9500.00M,
                    EmployeeContributionAmount = 380.00M,
                    EmployerContributionAmount = 760.00M,
                    EmployeeECAmount = 10.00M,
                    EffectiveDateFrom = new DateTime(2019, 4, 1),
                    EffectiveDateTo = new DateTime(2022, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 9750.00M,
                    RangeToAmount = 10249.99M,
                    MonthlySalaryCredit = 10000.00M,
                    EmployeeContributionAmount = 400.00M,
                    EmployerContributionAmount = 800.00M,
                    EmployeeECAmount = 10.00M,
                    EffectiveDateFrom = new DateTime(2019, 4, 1),
                    EffectiveDateTo = new DateTime(2022, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 10250.00M,
                    RangeToAmount = 10749.99M,
                    MonthlySalaryCredit = 10500.00M,
                    EmployeeContributionAmount = 420.00M,
                    EmployerContributionAmount = 840.00M,
                    EmployeeECAmount = 10.00M,
                    EffectiveDateFrom = new DateTime(2019, 4, 1),
                    EffectiveDateTo = new DateTime(2022, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 10750.00M,
                    RangeToAmount = 11249.99M,
                    MonthlySalaryCredit = 11000.00M,
                    EmployeeContributionAmount = 440.00M,
                    EmployerContributionAmount = 880.00M,
                    EmployeeECAmount = 10.00M,
                    EffectiveDateFrom = new DateTime(2019, 4, 1),
                    EffectiveDateTo = new DateTime(2022, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 11250.00M,
                    RangeToAmount = 11749.99M,
                    MonthlySalaryCredit = 11500.00M,
                    EmployeeContributionAmount = 460.00M,
                    EmployerContributionAmount = 920.00M,
                    EmployeeECAmount = 10.00M,
                    EffectiveDateFrom = new DateTime(2019, 4, 1),
                    EffectiveDateTo = new DateTime(2022, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 11750.00M,
                    RangeToAmount = 12249.99M,
                    MonthlySalaryCredit = 12000.00M,
                    EmployeeContributionAmount = 480.00M,
                    EmployerContributionAmount = 960.00M,
                    EmployeeECAmount = 10.00M,
                    EffectiveDateFrom = new DateTime(2019, 4, 1),
                    EffectiveDateTo = new DateTime(2022, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 12250.00M,
                    RangeToAmount = 12749.99M,
                    MonthlySalaryCredit = 12500.00M,
                    EmployeeContributionAmount = 500.00M,
                    EmployerContributionAmount = 1000.00M,
                    EmployeeECAmount = 10.00M,
                    EffectiveDateFrom = new DateTime(2019, 4, 1),
                    EffectiveDateTo = new DateTime(2022, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 12750.00M,
                    RangeToAmount = 13249.99M,
                    MonthlySalaryCredit = 13000.00M,
                    EmployeeContributionAmount = 520.00M,
                    EmployerContributionAmount = 1040.00M,
                    EmployeeECAmount = 10.00M,
                    EffectiveDateFrom = new DateTime(2019, 4, 1),
                    EffectiveDateTo = new DateTime(2022, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 13250.00M,
                    RangeToAmount = 13749.99M,
                    MonthlySalaryCredit = 13500.00M,
                    EmployeeContributionAmount = 540.00M,
                    EmployerContributionAmount = 1080.00M,
                    EmployeeECAmount = 10.00M,
                    EffectiveDateFrom = new DateTime(2019, 4, 1),
                    EffectiveDateTo = new DateTime(2022, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 13750.00M,
                    RangeToAmount = 14249.99M,
                    MonthlySalaryCredit = 14000.00M,
                    EmployeeContributionAmount = 560.00M,
                    EmployerContributionAmount = 1120.00M,
                    EmployeeECAmount = 10.00M,
                    EffectiveDateFrom = new DateTime(2019, 4, 1),
                    EffectiveDateTo = new DateTime(2022, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 14250.00M,
                    RangeToAmount = 14749.99M,
                    MonthlySalaryCredit = 14500.00M,
                    EmployeeContributionAmount = 580.00M,
                    EmployerContributionAmount = 1160.00M,
                    EmployeeECAmount = 10.00M,
                    EffectiveDateFrom = new DateTime(2019, 4, 1),
                    EffectiveDateTo = new DateTime(2022, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 14750.00M,
                    RangeToAmount = 15249.99M,
                    MonthlySalaryCredit = 15000.00M,
                    EmployeeContributionAmount = 600.00M,
                    EmployerContributionAmount = 1200.00M,
                    EmployeeECAmount = 30.00M,
                    EffectiveDateFrom = new DateTime(2019, 4, 1),
                    EffectiveDateTo = new DateTime(2022, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 15250.00M,
                    RangeToAmount = 15749.99M,
                    MonthlySalaryCredit = 15500.00M,
                    EmployeeContributionAmount = 620.00M,
                    EmployerContributionAmount = 1240.00M,
                    EmployeeECAmount = 30.00M,
                    EffectiveDateFrom = new DateTime(2019, 4, 1),
                    EffectiveDateTo = new DateTime(2022, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 15750.00M,
                    RangeToAmount = 16249.99M,
                    MonthlySalaryCredit = 16000.00M,
                    EmployeeContributionAmount = 640.00M,
                    EmployerContributionAmount = 1280.00M,
                    EmployeeECAmount = 30.00M,
                    EffectiveDateFrom = new DateTime(2019, 4, 1),
                    EffectiveDateTo = new DateTime(2022, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 16250.00M,
                    RangeToAmount = 16749.99M,
                    MonthlySalaryCredit = 16500.00M,
                    EmployeeContributionAmount = 660.00M,
                    EmployerContributionAmount = 1320.00M,
                    EmployeeECAmount = 30.00M,
                    EffectiveDateFrom = new DateTime(2019, 4, 1),
                    EffectiveDateTo = new DateTime(2022, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 16750.00M,
                    RangeToAmount = 17249.99M,
                    MonthlySalaryCredit = 17000.00M,
                    EmployeeContributionAmount = 680.00M,
                    EmployerContributionAmount = 1360.00M,
                    EmployeeECAmount = 30.00M,
                    EffectiveDateFrom = new DateTime(2019, 4, 1),
                    EffectiveDateTo = new DateTime(2022, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 17250.00M,
                    RangeToAmount = 17749.99M,
                    MonthlySalaryCredit = 17500.00M,
                    EmployeeContributionAmount = 700.00M,
                    EmployerContributionAmount = 1400.00M,
                    EmployeeECAmount = 30.00M,
                    EffectiveDateFrom = new DateTime(2019, 4, 1),
                    EffectiveDateTo = new DateTime(2022, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 17750.00M,
                    RangeToAmount = 18249.99M,
                    MonthlySalaryCredit = 18000.00M,
                    EmployeeContributionAmount = 720.00M,
                    EmployerContributionAmount = 1440.00M,
                    EmployeeECAmount = 30.00M,
                    EffectiveDateFrom = new DateTime(2019, 4, 1),
                    EffectiveDateTo = new DateTime(2022, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 18250.00M,
                    RangeToAmount = 18749.99M,
                    MonthlySalaryCredit = 18500.00M,
                    EmployeeContributionAmount = 740.00M,
                    EmployerContributionAmount = 1480.00M,
                    EmployeeECAmount = 30.00M,
                    EffectiveDateFrom = new DateTime(2019, 4, 1),
                    EffectiveDateTo = new DateTime(2022, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 18750.00M,
                    RangeToAmount = 19249.99M,
                    MonthlySalaryCredit = 19000.00M,
                    EmployeeContributionAmount = 760.00M,
                    EmployerContributionAmount = 1520.00M,
                    EmployeeECAmount = 30.00M,
                    EffectiveDateFrom = new DateTime(2019, 4, 1),
                    EffectiveDateTo = new DateTime(2022, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 19250.00M,
                    RangeToAmount = 19749.99M,
                    MonthlySalaryCredit = 19500.00M,
                    EmployeeContributionAmount = 780.00M,
                    EmployerContributionAmount = 1560.00M,
                    EmployeeECAmount = 30.00M,
                    EffectiveDateFrom = new DateTime(2019, 4, 1),
                    EffectiveDateTo = new DateTime(2022, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 19750.00M,
                    RangeToAmount = 999999.00M,
                    MonthlySalaryCredit = 20000.00M,
                    EmployeeContributionAmount = 800.00M,
                    EmployerContributionAmount = 1600.00M,
                    EmployeeECAmount = 30.00M,
                    EffectiveDateFrom = new DateTime(2019, 4, 1),
                    EffectiveDateTo = new DateTime(2022, 12, 1)
                }
            };
        }
    }
}
