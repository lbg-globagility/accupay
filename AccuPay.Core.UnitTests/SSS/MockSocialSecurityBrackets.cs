using AccuPay.Core.Entities;
using System;
using System.Collections.Generic;

namespace AccuPay.Core.UnitTests.SSS
{
    public class MockSocialSecurityBrackets
    {
        public static List<SocialSecurityBracket> Get()
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
                    EmployerECAmount = 10.00M,
                    EffectiveDateFrom = new DateTime(2019, 4, 1),
                    EffectiveDateTo = new DateTime(2020, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 2250.00M,
                    RangeToAmount = 2749.99M,
                    MonthlySalaryCredit = 2500.00M,
                    EmployeeContributionAmount = 100.00M,
                    EmployerContributionAmount = 200.00M,
                    EmployerECAmount = 10.00M,
                    EffectiveDateFrom = new DateTime(2019, 4, 1),
                    EffectiveDateTo = new DateTime(2020, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 2750.00M,
                    RangeToAmount = 3249.99M,
                    MonthlySalaryCredit = 3000.00M,
                    EmployeeContributionAmount = 120.00M,
                    EmployerContributionAmount = 240.00M,
                    EmployerECAmount = 10.00M,
                    EffectiveDateFrom = new DateTime(2019, 4, 1),
                    EffectiveDateTo = new DateTime(2020, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 3250.00M,
                    RangeToAmount = 3749.99M,
                    MonthlySalaryCredit = 3500.00M,
                    EmployeeContributionAmount = 140.00M,
                    EmployerContributionAmount = 280.00M,
                    EmployerECAmount = 10.00M,
                    EffectiveDateFrom = new DateTime(2019, 4, 1),
                    EffectiveDateTo = new DateTime(2020, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 3750.00M,
                    RangeToAmount = 4249.99M,
                    MonthlySalaryCredit = 4000.00M,
                    EmployeeContributionAmount = 160.00M,
                    EmployerContributionAmount = 320.00M,
                    EmployerECAmount = 10.00M,
                    EffectiveDateFrom = new DateTime(2019, 4, 1),
                    EffectiveDateTo = new DateTime(2020, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 4250.00M,
                    RangeToAmount = 4749.99M,
                    MonthlySalaryCredit = 4500.00M,
                    EmployeeContributionAmount = 180.00M,
                    EmployerContributionAmount = 360.00M,
                    EmployerECAmount = 10.00M,
                    EffectiveDateFrom = new DateTime(2019, 4, 1),
                    EffectiveDateTo = new DateTime(2020, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 4750.00M,
                    RangeToAmount = 5249.99M,
                    MonthlySalaryCredit = 5000.00M,
                    EmployeeContributionAmount = 200.00M,
                    EmployerContributionAmount = 400.00M,
                    EmployerECAmount = 10.00M,
                    EffectiveDateFrom = new DateTime(2019, 4, 1),
                    EffectiveDateTo = new DateTime(2020, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 5250.00M,
                    RangeToAmount = 5749.99M,
                    MonthlySalaryCredit = 5500.00M,
                    EmployeeContributionAmount = 220.00M,
                    EmployerContributionAmount = 440.00M,
                    EmployerECAmount = 10.00M,
                    EffectiveDateFrom = new DateTime(2019, 4, 1),
                    EffectiveDateTo = new DateTime(2020, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 5750.00M,
                    RangeToAmount = 6249.99M,
                    MonthlySalaryCredit = 6000.00M,
                    EmployeeContributionAmount = 240.00M,
                    EmployerContributionAmount = 480.00M,
                    EmployerECAmount = 10.00M,
                    EffectiveDateFrom = new DateTime(2019, 4, 1),
                    EffectiveDateTo = new DateTime(2020, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 6250.00M,
                    RangeToAmount = 6749.99M,
                    MonthlySalaryCredit = 6500.00M,
                    EmployeeContributionAmount = 260.00M,
                    EmployerContributionAmount = 520.00M,
                    EmployerECAmount = 10.00M,
                    EffectiveDateFrom = new DateTime(2019, 4, 1),
                    EffectiveDateTo = new DateTime(2020, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 6750.00M,
                    RangeToAmount = 7249.99M,
                    MonthlySalaryCredit = 7000.00M,
                    EmployeeContributionAmount = 280.00M,
                    EmployerContributionAmount = 560.00M,
                    EmployerECAmount = 10.00M,
                    EffectiveDateFrom = new DateTime(2019, 4, 1),
                    EffectiveDateTo = new DateTime(2020, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 7250.00M,
                    RangeToAmount = 7749.99M,
                    MonthlySalaryCredit = 7500.00M,
                    EmployeeContributionAmount = 300.00M,
                    EmployerContributionAmount = 600.00M,
                    EmployerECAmount = 10.00M,
                    EffectiveDateFrom = new DateTime(2019, 4, 1),
                    EffectiveDateTo = new DateTime(2020, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 7750.00M,
                    RangeToAmount = 8249.99M,
                    MonthlySalaryCredit = 8000.00M,
                    EmployeeContributionAmount = 320.00M,
                    EmployerContributionAmount = 640.00M,
                    EmployerECAmount = 10.00M,
                    EffectiveDateFrom = new DateTime(2019, 4, 1),
                    EffectiveDateTo = new DateTime(2020, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 8250.00M,
                    RangeToAmount = 8749.99M,
                    MonthlySalaryCredit = 8500.00M,
                    EmployeeContributionAmount = 340.00M,
                    EmployerContributionAmount = 680.00M,
                    EmployerECAmount = 10.00M,
                    EffectiveDateFrom = new DateTime(2019, 4, 1),
                    EffectiveDateTo = new DateTime(2020, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 8750.00M,
                    RangeToAmount = 9249.99M,
                    MonthlySalaryCredit = 9000.00M,
                    EmployeeContributionAmount = 360.00M,
                    EmployerContributionAmount = 720.00M,
                    EmployerECAmount = 10.00M,
                    EffectiveDateFrom = new DateTime(2019, 4, 1),
                    EffectiveDateTo = new DateTime(2020, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 9250.00M,
                    RangeToAmount = 9749.99M,
                    MonthlySalaryCredit = 9500.00M,
                    EmployeeContributionAmount = 380.00M,
                    EmployerContributionAmount = 760.00M,
                    EmployerECAmount = 10.00M,
                    EffectiveDateFrom = new DateTime(2019, 4, 1),
                    EffectiveDateTo = new DateTime(2020, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 9750.00M,
                    RangeToAmount = 10249.99M,
                    MonthlySalaryCredit = 10000.00M,
                    EmployeeContributionAmount = 400.00M,
                    EmployerContributionAmount = 800.00M,
                    EmployerECAmount = 10.00M,
                    EffectiveDateFrom = new DateTime(2019, 4, 1),
                    EffectiveDateTo = new DateTime(2020, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 10250.00M,
                    RangeToAmount = 10749.99M,
                    MonthlySalaryCredit = 10500.00M,
                    EmployeeContributionAmount = 420.00M,
                    EmployerContributionAmount = 840.00M,
                    EmployerECAmount = 10.00M,
                    EffectiveDateFrom = new DateTime(2019, 4, 1),
                    EffectiveDateTo = new DateTime(2020, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 10750.00M,
                    RangeToAmount = 11249.99M,
                    MonthlySalaryCredit = 11000.00M,
                    EmployeeContributionAmount = 440.00M,
                    EmployerContributionAmount = 880.00M,
                    EmployerECAmount = 10.00M,
                    EffectiveDateFrom = new DateTime(2019, 4, 1),
                    EffectiveDateTo = new DateTime(2020, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 11250.00M,
                    RangeToAmount = 11749.99M,
                    MonthlySalaryCredit = 11500.00M,
                    EmployeeContributionAmount = 460.00M,
                    EmployerContributionAmount = 920.00M,
                    EmployerECAmount = 10.00M,
                    EffectiveDateFrom = new DateTime(2019, 4, 1),
                    EffectiveDateTo = new DateTime(2020, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 11750.00M,
                    RangeToAmount = 12249.99M,
                    MonthlySalaryCredit = 12000.00M,
                    EmployeeContributionAmount = 480.00M,
                    EmployerContributionAmount = 960.00M,
                    EmployerECAmount = 10.00M,
                    EffectiveDateFrom = new DateTime(2019, 4, 1),
                    EffectiveDateTo = new DateTime(2020, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 12250.00M,
                    RangeToAmount = 12749.99M,
                    MonthlySalaryCredit = 12500.00M,
                    EmployeeContributionAmount = 500.00M,
                    EmployerContributionAmount = 1000.00M,
                    EmployerECAmount = 10.00M,
                    EffectiveDateFrom = new DateTime(2019, 4, 1),
                    EffectiveDateTo = new DateTime(2020, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 12750.00M,
                    RangeToAmount = 13249.99M,
                    MonthlySalaryCredit = 13000.00M,
                    EmployeeContributionAmount = 520.00M,
                    EmployerContributionAmount = 1040.00M,
                    EmployerECAmount = 10.00M,
                    EffectiveDateFrom = new DateTime(2019, 4, 1),
                    EffectiveDateTo = new DateTime(2020, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 13250.00M,
                    RangeToAmount = 13749.99M,
                    MonthlySalaryCredit = 13500.00M,
                    EmployeeContributionAmount = 540.00M,
                    EmployerContributionAmount = 1080.00M,
                    EmployerECAmount = 10.00M,
                    EffectiveDateFrom = new DateTime(2019, 4, 1),
                    EffectiveDateTo = new DateTime(2020, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 13750.00M,
                    RangeToAmount = 14249.99M,
                    MonthlySalaryCredit = 14000.00M,
                    EmployeeContributionAmount = 560.00M,
                    EmployerContributionAmount = 1120.00M,
                    EmployerECAmount = 10.00M,
                    EffectiveDateFrom = new DateTime(2019, 4, 1),
                    EffectiveDateTo = new DateTime(2020, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 14250.00M,
                    RangeToAmount = 14749.99M,
                    MonthlySalaryCredit = 14500.00M,
                    EmployeeContributionAmount = 580.00M,
                    EmployerContributionAmount = 1160.00M,
                    EmployerECAmount = 10.00M,
                    EffectiveDateFrom = new DateTime(2019, 4, 1),
                    EffectiveDateTo = new DateTime(2020, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 14750.00M,
                    RangeToAmount = 15249.99M,
                    MonthlySalaryCredit = 15000.00M,
                    EmployeeContributionAmount = 600.00M,
                    EmployerContributionAmount = 1200.00M,
                    EmployerECAmount = 30.00M,
                    EffectiveDateFrom = new DateTime(2019, 4, 1),
                    EffectiveDateTo = new DateTime(2020, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 15250.00M,
                    RangeToAmount = 15749.99M,
                    MonthlySalaryCredit = 15500.00M,
                    EmployeeContributionAmount = 620.00M,
                    EmployerContributionAmount = 1240.00M,
                    EmployerECAmount = 30.00M,
                    EffectiveDateFrom = new DateTime(2019, 4, 1),
                    EffectiveDateTo = new DateTime(2020, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 15750.00M,
                    RangeToAmount = 16249.99M,
                    MonthlySalaryCredit = 16000.00M,
                    EmployeeContributionAmount = 640.00M,
                    EmployerContributionAmount = 1280.00M,
                    EmployerECAmount = 30.00M,
                    EffectiveDateFrom = new DateTime(2019, 4, 1),
                    EffectiveDateTo = new DateTime(2020, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 16250.00M,
                    RangeToAmount = 16749.99M,
                    MonthlySalaryCredit = 16500.00M,
                    EmployeeContributionAmount = 660.00M,
                    EmployerContributionAmount = 1320.00M,
                    EmployerECAmount = 30.00M,
                    EffectiveDateFrom = new DateTime(2019, 4, 1),
                    EffectiveDateTo = new DateTime(2020, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 16750.00M,
                    RangeToAmount = 17249.99M,
                    MonthlySalaryCredit = 17000.00M,
                    EmployeeContributionAmount = 680.00M,
                    EmployerContributionAmount = 1360.00M,
                    EmployerECAmount = 30.00M,
                    EffectiveDateFrom = new DateTime(2019, 4, 1),
                    EffectiveDateTo = new DateTime(2020, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 17250.00M,
                    RangeToAmount = 17749.99M,
                    MonthlySalaryCredit = 17500.00M,
                    EmployeeContributionAmount = 700.00M,
                    EmployerContributionAmount = 1400.00M,
                    EmployerECAmount = 30.00M,
                    EffectiveDateFrom = new DateTime(2019, 4, 1),
                    EffectiveDateTo = new DateTime(2020, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 17750.00M,
                    RangeToAmount = 18249.99M,
                    MonthlySalaryCredit = 18000.00M,
                    EmployeeContributionAmount = 720.00M,
                    EmployerContributionAmount = 1440.00M,
                    EmployerECAmount = 30.00M,
                    EffectiveDateFrom = new DateTime(2019, 4, 1),
                    EffectiveDateTo = new DateTime(2020, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 18250.00M,
                    RangeToAmount = 18749.99M,
                    MonthlySalaryCredit = 18500.00M,
                    EmployeeContributionAmount = 740.00M,
                    EmployerContributionAmount = 1480.00M,
                    EmployerECAmount = 30.00M,
                    EffectiveDateFrom = new DateTime(2019, 4, 1),
                    EffectiveDateTo = new DateTime(2020, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 18750.00M,
                    RangeToAmount = 19249.99M,
                    MonthlySalaryCredit = 19000.00M,
                    EmployeeContributionAmount = 760.00M,
                    EmployerContributionAmount = 1520.00M,
                    EmployerECAmount = 30.00M,
                    EffectiveDateFrom = new DateTime(2019, 4, 1),
                    EffectiveDateTo = new DateTime(2020, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 19250.00M,
                    RangeToAmount = 19749.99M,
                    MonthlySalaryCredit = 19500.00M,
                    EmployeeContributionAmount = 780.00M,
                    EmployerContributionAmount = 1560.00M,
                    EmployerECAmount = 30.00M,
                    EffectiveDateFrom = new DateTime(2019, 4, 1),
                    EffectiveDateTo = new DateTime(2020, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 19750.00M,
                    RangeToAmount = 999999.00M,
                    MonthlySalaryCredit = 20000.00M,
                    EmployeeContributionAmount = 800.00M,
                    EmployerContributionAmount = 1600.00M,
                    EmployerECAmount = 30.00M,
                    EffectiveDateFrom = new DateTime(2019, 4, 1),
                    EffectiveDateTo = new DateTime(2020, 12, 1)
                },

                new SocialSecurityBracket()
                {
                    RangeFromAmount = 1_000m,
                    RangeToAmount = 3_249.99m,
                    MonthlySalaryCredit = 3_000,
                    EmployeeContributionAmount = 135.0m,
                    EmployerContributionAmount = 255m,
                    EmployerECAmount = 10m,
                    EmployerMPFAmount = 0.0m,
                    EmployeeMPFAmount = 0.0m,
                    EffectiveDateFrom = new DateTime(2021, 1, 1),
                    EffectiveDateTo = new DateTime(2022, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 3_250m,
                    RangeToAmount = 3_749.99m,
                    MonthlySalaryCredit = 3_500,
                    EmployeeContributionAmount = 157.5m,
                    EmployerContributionAmount = 297.5m,
                    EmployerECAmount = 10m,
                    EmployerMPFAmount = 0.0m,
                    EmployeeMPFAmount = 0.0m,
                    EffectiveDateFrom = new DateTime(2021, 1, 1),
                    EffectiveDateTo = new DateTime(2022, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 3_750m,
                    RangeToAmount = 4_249.99m,
                    MonthlySalaryCredit = 4_000,
                    EmployeeContributionAmount = 180.0m,
                    EmployerContributionAmount = 340m,
                    EmployerECAmount = 10m,
                    EmployerMPFAmount = 0.0m,
                    EmployeeMPFAmount = 0.0m,
                    EffectiveDateFrom = new DateTime(2021, 1, 1),
                    EffectiveDateTo = new DateTime(2022, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 4_250m,
                    RangeToAmount = 4_749.99m,
                    MonthlySalaryCredit = 4_500,
                    EmployeeContributionAmount = 202.5m,
                    EmployerContributionAmount = 382.5m,
                    EmployerECAmount = 10m,
                    EmployerMPFAmount = 0.0m,
                    EmployeeMPFAmount = 0.0m,
                    EffectiveDateFrom = new DateTime(2021, 1, 1),
                    EffectiveDateTo = new DateTime(2022, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 4_750m,
                    RangeToAmount = 5_249.99m,
                    MonthlySalaryCredit = 5_000,
                    EmployeeContributionAmount = 225.0m,
                    EmployerContributionAmount = 425m,
                    EmployerECAmount = 10m,
                    EmployerMPFAmount = 0.0m,
                    EmployeeMPFAmount = 0.0m,
                    EffectiveDateFrom = new DateTime(2021, 1, 1),
                    EffectiveDateTo = new DateTime(2022, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 5_250m,
                    RangeToAmount = 5_749.99m,
                    MonthlySalaryCredit = 5_500,
                    EmployeeContributionAmount = 247.5m,
                    EmployerContributionAmount = 467.5m,
                    EmployerECAmount = 10m,
                    EmployerMPFAmount = 0.0m,
                    EmployeeMPFAmount = 0.0m,
                    EffectiveDateFrom = new DateTime(2021, 1, 1),
                    EffectiveDateTo = new DateTime(2022, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 5_750m,
                    RangeToAmount = 6_249.99m,
                    MonthlySalaryCredit = 6_000,
                    EmployeeContributionAmount = 270.0m,
                    EmployerContributionAmount = 510m,
                    EmployerECAmount = 10m,
                    EmployerMPFAmount = 0.0m,
                    EmployeeMPFAmount = 0.0m,
                    EffectiveDateFrom = new DateTime(2021, 1, 1),
                    EffectiveDateTo = new DateTime(2022, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 6_250m,
                    RangeToAmount = 6_749.99m,
                    MonthlySalaryCredit = 6_500,
                    EmployeeContributionAmount = 292.5m,
                    EmployerContributionAmount = 552.5m,
                    EmployerECAmount = 10m,
                    EmployerMPFAmount = 0.0m,
                    EmployeeMPFAmount = 0.0m,
                    EffectiveDateFrom = new DateTime(2021, 1, 1),
                    EffectiveDateTo = new DateTime(2022, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 6_750m,
                    RangeToAmount = 7_249.99m,
                    MonthlySalaryCredit = 7_000,
                    EmployeeContributionAmount = 315.0m,
                    EmployerContributionAmount = 595m,
                    EmployerECAmount = 10m,
                    EmployerMPFAmount = 0.0m,
                    EmployeeMPFAmount = 0.0m,
                    EffectiveDateFrom = new DateTime(2021, 1, 1),
                    EffectiveDateTo = new DateTime(2022, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 7_250m,
                    RangeToAmount = 7_749.99m,
                    MonthlySalaryCredit = 7_500,
                    EmployeeContributionAmount = 337.5m,
                    EmployerContributionAmount = 637.5m,
                    EmployerECAmount = 10m,
                    EmployerMPFAmount = 0.0m,
                    EmployeeMPFAmount = 0.0m,
                    EffectiveDateFrom = new DateTime(2021, 1, 1),
                    EffectiveDateTo = new DateTime(2022, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 7_750m,
                    RangeToAmount = 8_249.99m,
                    MonthlySalaryCredit = 8_000,
                    EmployeeContributionAmount = 360.0m,
                    EmployerContributionAmount = 680m,
                    EmployerECAmount = 10m,
                    EmployerMPFAmount = 0.0m,
                    EmployeeMPFAmount = 0.0m,
                    EffectiveDateFrom = new DateTime(2021, 1, 1),
                    EffectiveDateTo = new DateTime(2022, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 8_250m,
                    RangeToAmount = 8_749.99m,
                    MonthlySalaryCredit = 8_500,
                    EmployeeContributionAmount = 382.5m,
                    EmployerContributionAmount = 722.5m,
                    EmployerECAmount = 10m,
                    EmployerMPFAmount = 0.0m,
                    EmployeeMPFAmount = 0.0m,
                    EffectiveDateFrom = new DateTime(2021, 1, 1),
                    EffectiveDateTo = new DateTime(2022, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 8_750m,
                    RangeToAmount = 9_249.99m,
                    MonthlySalaryCredit = 9_000,
                    EmployeeContributionAmount = 405.0m,
                    EmployerContributionAmount = 765m,
                    EmployerECAmount = 10m,
                    EmployerMPFAmount = 0.0m,
                    EmployeeMPFAmount = 0.0m,
                    EffectiveDateFrom = new DateTime(2021, 1, 1),
                    EffectiveDateTo = new DateTime(2022, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 9_250m,
                    RangeToAmount = 9_749.99m,
                    MonthlySalaryCredit = 9_500,
                    EmployeeContributionAmount = 427.5m,
                    EmployerContributionAmount = 807.5m,
                    EmployerECAmount = 10m,
                    EmployerMPFAmount = 0.0m,
                    EmployeeMPFAmount = 0.0m,
                    EffectiveDateFrom = new DateTime(2021, 1, 1),
                    EffectiveDateTo = new DateTime(2022, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 9_750m,
                    RangeToAmount = 10_249.99m,
                    MonthlySalaryCredit = 10_000,
                    EmployeeContributionAmount = 450.0m,
                    EmployerContributionAmount = 850m,
                    EmployerECAmount = 10m,
                    EmployerMPFAmount = 0.0m,
                    EmployeeMPFAmount = 0.0m,
                    EffectiveDateFrom = new DateTime(2021, 1, 1),
                    EffectiveDateTo = new DateTime(2022, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 10_250m,
                    RangeToAmount = 10_749.99m,
                    MonthlySalaryCredit = 10_500,
                    EmployeeContributionAmount = 472.5m,
                    EmployerContributionAmount = 892.5m,
                    EmployerECAmount = 10m,
                    EmployerMPFAmount = 0.0m,
                    EmployeeMPFAmount = 0.0m,
                    EffectiveDateFrom = new DateTime(2021, 1, 1),
                    EffectiveDateTo = new DateTime(2022, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 10_750m,
                    RangeToAmount = 11_249.99m,
                    MonthlySalaryCredit = 11_000,
                    EmployeeContributionAmount = 495.0m,
                    EmployerContributionAmount = 935m,
                    EmployerECAmount = 10m,
                    EmployerMPFAmount = 0.0m,
                    EmployeeMPFAmount = 0.0m,
                    EffectiveDateFrom = new DateTime(2021, 1, 1),
                    EffectiveDateTo = new DateTime(2022, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 11_250m,
                    RangeToAmount = 11_749.99m,
                    MonthlySalaryCredit = 11_500,
                    EmployeeContributionAmount = 517.5m,
                    EmployerContributionAmount = 977.5m,
                    EmployerECAmount = 10m,
                    EmployerMPFAmount = 0.0m,
                    EmployeeMPFAmount = 0.0m,
                    EffectiveDateFrom = new DateTime(2021, 1, 1),
                    EffectiveDateTo = new DateTime(2022, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 11_750m,
                    RangeToAmount = 12_249.99m,
                    MonthlySalaryCredit = 12_000,
                    EmployeeContributionAmount = 540.0m,
                    EmployerContributionAmount = 1_020m,
                    EmployerECAmount = 10m,
                    EmployerMPFAmount = 0.0m,
                    EmployeeMPFAmount = 0.0m,
                    EffectiveDateFrom = new DateTime(2021, 1, 1),
                    EffectiveDateTo = new DateTime(2022, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 12_250m,
                    RangeToAmount = 12_749.99m,
                    MonthlySalaryCredit = 12_500,
                    EmployeeContributionAmount = 562.5m,
                    EmployerContributionAmount = 1_062.5m,
                    EmployerECAmount = 10m,
                    EmployerMPFAmount = 0.0m,
                    EmployeeMPFAmount = 0.0m,
                    EffectiveDateFrom = new DateTime(2021, 1, 1),
                    EffectiveDateTo = new DateTime(2022, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 12_750m,
                    RangeToAmount = 13_249.99m,
                    MonthlySalaryCredit = 13_000,
                    EmployeeContributionAmount = 585.0m,
                    EmployerContributionAmount = 1_105m,
                    EmployerECAmount = 10m,
                    EmployerMPFAmount = 0.0m,
                    EmployeeMPFAmount = 0.0m,
                    EffectiveDateFrom = new DateTime(2021, 1, 1),
                    EffectiveDateTo = new DateTime(2022, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 13_250m,
                    RangeToAmount = 13_749.99m,
                    MonthlySalaryCredit = 13_500,
                    EmployeeContributionAmount = 607.5m,
                    EmployerContributionAmount = 1_147.5m,
                    EmployerECAmount = 10m,
                    EmployerMPFAmount = 0.0m,
                    EmployeeMPFAmount = 0.0m,
                    EffectiveDateFrom = new DateTime(2021, 1, 1),
                    EffectiveDateTo = new DateTime(2022, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 13_750m,
                    RangeToAmount = 14_249.99m,
                    MonthlySalaryCredit = 14_000,
                    EmployeeContributionAmount = 630.0m,
                    EmployerContributionAmount = 1_190m,
                    EmployerECAmount = 10m,
                    EmployerMPFAmount = 0.0m,
                    EmployeeMPFAmount = 0.0m,
                    EffectiveDateFrom = new DateTime(2021, 1, 1),
                    EffectiveDateTo = new DateTime(2022, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 14_250m,
                    RangeToAmount = 14_749.99m,
                    MonthlySalaryCredit = 14_500,
                    EmployeeContributionAmount = 652.5m,
                    EmployerContributionAmount = 1_232.5m,
                    EmployerECAmount = 10m,
                    EmployerMPFAmount = 0.0m,
                    EmployeeMPFAmount = 0.0m,
                    EffectiveDateFrom = new DateTime(2021, 1, 1),
                    EffectiveDateTo = new DateTime(2022, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 14_750m,
                    RangeToAmount = 15_249.99m,
                    MonthlySalaryCredit = 15_000,
                    EmployeeContributionAmount = 675.0m,
                    EmployerContributionAmount = 1_275m,
                    EmployerECAmount = 30m,
                    EmployerMPFAmount = 0.0m,
                    EmployeeMPFAmount = 0.0m,
                    EffectiveDateFrom = new DateTime(2021, 1, 1),
                    EffectiveDateTo = new DateTime(2022, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 15_250m,
                    RangeToAmount = 15_749.99m,
                    MonthlySalaryCredit = 15_500,
                    EmployeeContributionAmount = 697.5m,
                    EmployerContributionAmount = 1_317.5m,
                    EmployerECAmount = 30m,
                    EmployerMPFAmount = 0.0m,
                    EmployeeMPFAmount = 0.0m,
                    EffectiveDateFrom = new DateTime(2021, 1, 1),
                    EffectiveDateTo = new DateTime(2022, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 15_750m,
                    RangeToAmount = 16_249.99m,
                    MonthlySalaryCredit = 16_000,
                    EmployeeContributionAmount = 720.0m,
                    EmployerContributionAmount = 1_360m,
                    EmployerECAmount = 30m,
                    EmployerMPFAmount = 0.0m,
                    EmployeeMPFAmount = 0.0m,
                    EffectiveDateFrom = new DateTime(2021, 1, 1),
                    EffectiveDateTo = new DateTime(2022, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 16_250m,
                    RangeToAmount = 16_749.99m,
                    MonthlySalaryCredit = 16_500,
                    EmployeeContributionAmount = 742.5m,
                    EmployerContributionAmount = 1_402.5m,
                    EmployerECAmount = 30m,
                    EmployerMPFAmount = 0.0m,
                    EmployeeMPFAmount = 0.0m,
                    EffectiveDateFrom = new DateTime(2021, 1, 1),
                    EffectiveDateTo = new DateTime(2022, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 16_750m,
                    RangeToAmount = 17_249.99m,
                    MonthlySalaryCredit = 17_000,
                    EmployeeContributionAmount = 765.0m,
                    EmployerContributionAmount = 1_445m,
                    EmployerECAmount = 30m,
                    EmployerMPFAmount = 0.0m,
                    EmployeeMPFAmount = 0.0m,
                    EffectiveDateFrom = new DateTime(2021, 1, 1),
                    EffectiveDateTo = new DateTime(2022, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 17_250m,
                    RangeToAmount = 17_749.99m,
                    MonthlySalaryCredit = 17_500,
                    EmployeeContributionAmount = 787.5m,
                    EmployerContributionAmount = 1_487.5m,
                    EmployerECAmount = 30m,
                    EmployerMPFAmount = 0.0m,
                    EmployeeMPFAmount = 0.0m,
                    EffectiveDateFrom = new DateTime(2021, 1, 1),
                    EffectiveDateTo = new DateTime(2022, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 17_750m,
                    RangeToAmount = 18_249.99m,
                    MonthlySalaryCredit = 18_000,
                    EmployeeContributionAmount = 810.0m,
                    EmployerContributionAmount = 1_530m,
                    EmployerECAmount = 30m,
                    EmployerMPFAmount = 0.0m,
                    EmployeeMPFAmount = 0.0m,
                    EffectiveDateFrom = new DateTime(2021, 1, 1),
                    EffectiveDateTo = new DateTime(2022, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 18_250m,
                    RangeToAmount = 18_749.99m,
                    MonthlySalaryCredit = 18_500,
                    EmployeeContributionAmount = 832.5m,
                    EmployerContributionAmount = 1_572.5m,
                    EmployerECAmount = 30m,
                    EmployerMPFAmount = 0.0m,
                    EmployeeMPFAmount = 0.0m,
                    EffectiveDateFrom = new DateTime(2021, 1, 1),
                    EffectiveDateTo = new DateTime(2022, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 18_750m,
                    RangeToAmount = 19_249.99m,
                    MonthlySalaryCredit = 19_000,
                    EmployeeContributionAmount = 855.5m,
                    EmployerContributionAmount = 1_615m,
                    EmployerECAmount = 30m,
                    EmployerMPFAmount = 0.0m,
                    EmployeeMPFAmount = 0.0m,
                    EffectiveDateFrom = new DateTime(2021, 1, 1),
                    EffectiveDateTo = new DateTime(2022, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 19_250m,
                    RangeToAmount = 19_749.99m,
                    MonthlySalaryCredit = 19_500,
                    EmployeeContributionAmount = 877.5m,
                    EmployerContributionAmount = 1_657.5m,
                    EmployerECAmount = 30m,
                    EmployerMPFAmount = 0.0m,
                    EmployeeMPFAmount = 0.0m,
                    EffectiveDateFrom = new DateTime(2021, 1, 1),
                    EffectiveDateTo = new DateTime(2022, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 19_750m,
                    RangeToAmount = 20_249.99m,
                    MonthlySalaryCredit = 20_000,
                    EmployeeContributionAmount = 900.0m,
                    EmployerContributionAmount = 1_700m,
                    EmployerECAmount = 30m,
                    EmployerMPFAmount = 0.0m,
                    EmployeeMPFAmount = 0.0m,
                    EffectiveDateFrom = new DateTime(2021, 1, 1),
                    EffectiveDateTo = new DateTime(2022, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 20_250m,
                    RangeToAmount = 20_749.99m,
                    MonthlySalaryCredit = 20_500,
                    EmployeeContributionAmount = 900.0m,
                    EmployerContributionAmount = 1_700.0m,
                    EmployerECAmount = 30m,
                    EmployerMPFAmount = 42.5m,
                    EmployeeMPFAmount = 22.5m,
                    EffectiveDateFrom = new DateTime(2021, 1, 1),
                    EffectiveDateTo = new DateTime(2022, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 20_750m,
                    RangeToAmount = 21_249.99m,
                    MonthlySalaryCredit = 21_000,
                    EmployeeContributionAmount = 900.0m,
                    EmployerContributionAmount = 1_700.0m,
                    EmployerECAmount = 30m,
                    EmployerMPFAmount = 85.0m,
                    EmployeeMPFAmount = 45.0m,
                    EffectiveDateFrom = new DateTime(2021, 1, 1),
                    EffectiveDateTo = new DateTime(2022, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 21_250m,
                    RangeToAmount = 21_749.99m,
                    MonthlySalaryCredit = 21_500,
                    EmployeeContributionAmount = 900.0m,
                    EmployerContributionAmount = 1_700.0m,
                    EmployerECAmount = 30m,
                    EmployerMPFAmount = 127.5m,
                    EmployeeMPFAmount = 67.5m,
                    EffectiveDateFrom = new DateTime(2021, 1, 1),
                    EffectiveDateTo = new DateTime(2022, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 21_750m,
                    RangeToAmount = 22_249.99m,
                    MonthlySalaryCredit = 22_000,
                    EmployeeContributionAmount = 900.0m,
                    EmployerContributionAmount = 1_700.0m,
                    EmployerECAmount = 30m,
                    EmployerMPFAmount = 170.0m,
                    EmployeeMPFAmount = 90.0m,
                    EffectiveDateFrom = new DateTime(2021, 1, 1),
                    EffectiveDateTo = new DateTime(2022, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 22_250m,
                    RangeToAmount = 22_749.99m,
                    MonthlySalaryCredit = 22_500,
                    EmployeeContributionAmount = 900.0m,
                    EmployerContributionAmount = 1_700.0m,
                    EmployerECAmount = 30m,
                    EmployerMPFAmount = 212.5m,
                    EmployeeMPFAmount = 112.5m,
                    EffectiveDateFrom = new DateTime(2021, 1, 1),
                    EffectiveDateTo = new DateTime(2022, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 22_750m,
                    RangeToAmount = 23_249.99m,
                    MonthlySalaryCredit = 23_000,
                    EmployeeContributionAmount = 900.0m,
                    EmployerContributionAmount = 1_700.0m,
                    EmployerECAmount = 30m,
                    EmployerMPFAmount = 255.0m,
                    EmployeeMPFAmount = 135.0m,
                    EffectiveDateFrom = new DateTime(2021, 1, 1),
                    EffectiveDateTo = new DateTime(2022, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 23_250m,
                    RangeToAmount = 23_749.99m,
                    MonthlySalaryCredit = 23_500,
                    EmployeeContributionAmount = 900.0m,
                    EmployerContributionAmount = 1_700.0m,
                    EmployerECAmount = 30m,
                    EmployerMPFAmount = 297.5m,
                    EmployeeMPFAmount = 157.5m,
                    EffectiveDateFrom = new DateTime(2021, 1, 1),
                    EffectiveDateTo = new DateTime(2022, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 23_750m,
                    RangeToAmount = 24_249.99m,
                    MonthlySalaryCredit = 24_000,
                    EmployeeContributionAmount = 900.0m,
                    EmployerContributionAmount = 1_700.0m,
                    EmployerECAmount = 30m,
                    EmployerMPFAmount = 340.0m,
                    EmployeeMPFAmount = 180.0m,
                    EffectiveDateFrom = new DateTime(2021, 1, 1),
                    EffectiveDateTo = new DateTime(2022, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 24_250m,
                    RangeToAmount = 24_749.99m,
                    MonthlySalaryCredit = 24_500,
                    EmployeeContributionAmount = 900.0m,
                    EmployerContributionAmount = 1_700.0m,
                    EmployerECAmount = 30m,
                    EmployerMPFAmount = 382.5m,
                    EmployeeMPFAmount = 202.5m,
                    EffectiveDateFrom = new DateTime(2021, 1, 1),
                    EffectiveDateTo = new DateTime(2022, 12, 1)
                },
                new SocialSecurityBracket()
                {
                    RangeFromAmount = 24_750m,
                    RangeToAmount = 9999_999.99m,
                    MonthlySalaryCredit = 25_000,
                    EmployeeContributionAmount = 900.0m,
                    EmployerContributionAmount = 1_700.0m,
                    EmployerECAmount = 30m,
                    EmployerMPFAmount = 425.0m,
                    EmployeeMPFAmount = 225.0m,
                    EffectiveDateFrom = new DateTime(2021, 1, 1),
                    EffectiveDateTo = new DateTime(2022, 12, 1)
                }
            };
        }
    }
}
