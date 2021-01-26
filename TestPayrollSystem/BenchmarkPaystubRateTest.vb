Option Strict On

Imports AccuPay
Imports AccuPay.Benchmark
Imports AccuPay.Core.Entities
Imports AccuPay.Core.ValueObjects
Imports AccuPay.Utilities

<TestFixture>
Public Class BenchmarkPaystubRateTest

    Private _overtimeRate As OvertimeRate

    Private _dailyBenchmarkPaystubRate As BenchmarkPaystubRate
    Private _dailyEmployee As Employee
    Private _dailySalary As Salary

    Private _monthlyBenchmarkPaystubRate As BenchmarkPaystubRate
    Private _monthlyEmployee As Employee
    Private _monthlySalary As Salary

    <SetUp>
    Public Sub SetUp()

        'daily employee
        _dailyEmployee = New Employee() With {
            .RowID = 1,
            .EmployeeType = "Daily",
            .WorkDaysPerYear = 312
        }

        _dailySalary = New Salary() With {
            .EmployeeID = 1,
            .BasicSalary = 537,
            .AllowanceSalary = 63
        }

        _dailyBenchmarkPaystubRate = New BenchmarkPaystubRate(_dailyEmployee, _dailySalary)

        'monthly employee
        _monthlyEmployee = New Employee() With {
            .RowID = 1,
            .EmployeeType = "Monthly",
            .WorkDaysPerYear = 312
        }

        _monthlySalary = New Salary() With {
            .EmployeeID = 1,
            .BasicSalary = 13962,
            .AllowanceSalary = 1638
        }

        _monthlyBenchmarkPaystubRate = New BenchmarkPaystubRate(_monthlyEmployee, _monthlySalary)

        InitializeOvertimeRate()

    End Sub

    Private Sub InitializeOvertimeRate()
        Dim basePay As Decimal = 1D
        Dim overtime As Decimal = 1.25D
        Dim nightDifferential As Decimal = 1.1D
        Dim nightDifferentialOvertime As Decimal = 1.375D
        Dim restDay As Decimal = 1.3D
        Dim restDayOvertime As Decimal = 1.69D
        Dim restDayNightDifferential As Decimal = 1.43D
        Dim restDayNightDifferentialOvertime As Decimal = 1.859D
        Dim specialHoliday As Decimal = 1.3D
        Dim specialHolidayOvertime As Decimal = 1.69D
        Dim specialHolidayNightDifferential As Decimal = 1.43D
        Dim specialHolidayNightDifferentialOvertime As Decimal = 1.859D
        Dim specialHolidayRestDay As Decimal = 1.5D
        Dim specialHolidayRestDayOvertime As Decimal = 1.95D
        Dim specialHolidayRestDayNightDifferential As Decimal = 1.65D
        Dim specialHolidayRestDayNightDifferentialOvertime As Decimal = 2.145D
        Dim regularHoliday As Decimal = 2D
        Dim regularHolidayOvertime As Decimal = 2.6D
        Dim regularHolidayNightDifferential As Decimal = 2.2D
        Dim regularHolidayNightDifferentialOvertime As Decimal = 2.86D
        Dim regularHolidayRestDay As Decimal = 2.6D
        Dim regularHolidayRestDayOvertime As Decimal = 3.38D
        Dim regularHolidayRestDayNightDifferential As Decimal = 2.86D
        Dim regularHolidayRestDayNightDifferentialOvertime As Decimal = 3.718D
        Dim doubleHoliday As Decimal = 3D
        Dim doubleHolidayOvertime As Decimal = 3.9D
        Dim doubleHolidayNightDifferential As Decimal = 3.3D
        Dim doubleHolidayNightDifferentialOvertime As Decimal = 4.29D
        Dim doubleHolidayRestDay As Decimal = 3.9D
        Dim doubleHolidayRestDayOvertime As Decimal = 5.07D
        Dim doubleHolidayRestDayNightDifferential As Decimal = 4.29D
        Dim doubleHolidayRestDayNightDifferentialOvertime As Decimal = 5.577D

        _overtimeRate = New OvertimeRate(basePay:=basePay,
                    overtime:=overtime,
                    nightDifferential:=nightDifferential,
                    nightDifferentialOvertime:=nightDifferentialOvertime,
                    restDay:=restDay,
                    restDayOvertime:=restDayOvertime,
                    restDayNightDifferential:=restDayNightDifferential,
                    restDayNightDifferentialOvertime:=restDayNightDifferentialOvertime,
                    specialHoliday:=specialHoliday,
                    specialHolidayOvertime:=specialHolidayOvertime,
                    specialHolidayNightDifferential:=specialHolidayNightDifferential,
                    specialHolidayNightDifferentialOvertime:=specialHolidayNightDifferentialOvertime,
                    specialHolidayRestDay:=specialHolidayRestDay,
                    specialHolidayRestDayOvertime:=specialHolidayRestDayOvertime,
                    specialHolidayRestDayNightDifferential:=specialHolidayRestDayNightDifferential,
                    specialHolidayRestDayNightDifferentialOvertime:=specialHolidayRestDayNightDifferentialOvertime,
                    regularHoliday:=regularHoliday,
                    regularHolidayOvertime:=regularHolidayOvertime,
                    regularHolidayNightDifferential:=regularHolidayNightDifferential,
                    regularHolidayNightDifferentialOvertime:=regularHolidayNightDifferentialOvertime,
                    regularHolidayRestDay:=regularHolidayRestDay,
                    regularHolidayRestDayOvertime:=regularHolidayRestDayOvertime,
                    regularHolidayRestDayNightDifferential:=regularHolidayRestDayNightDifferential,
                    regularHolidayRestDayNightDifferentialOvertime:=regularHolidayRestDayNightDifferentialOvertime,
                    doubleHoliday:=doubleHoliday,
                    doubleHolidayOvertime:=doubleHolidayOvertime,
                    doubleHolidayNightDifferential:=doubleHolidayNightDifferential,
                    doubleHolidayNightDifferentialOvertime:=doubleHolidayNightDifferentialOvertime,
                    doubleHolidayRestDay:=doubleHolidayRestDay,
                    doubleHolidayRestDayOvertime:=doubleHolidayRestDayOvertime,
                    doubleHolidayRestDayNightDifferential:=doubleHolidayRestDayNightDifferential,
                    doubleHolidayRestDayNightDifferentialOvertime:=doubleHolidayRestDayNightDifferentialOvertime)
    End Sub

    <Test>
    Public Sub ShouldReadEmployeeAndSalary()
        DefaultOvertimeInitialization()

        Assert.AreEqual(_dailyEmployee, _dailyBenchmarkPaystubRate.Employee)
        Assert.AreEqual(_dailySalary, _dailyBenchmarkPaystubRate.Salary)

        Assert.AreEqual(False, _dailyBenchmarkPaystubRate.IsInvalid)

        Assert.AreEqual(_monthlyEmployee, _monthlyBenchmarkPaystubRate.Employee)
        Assert.AreEqual(_monthlySalary, _monthlyBenchmarkPaystubRate.Salary)

        Assert.AreEqual(False, _monthlyBenchmarkPaystubRate.IsInvalid)

    End Sub

    <Test>
    Public Sub ShouldComputeCorrectRates()
        DefaultOvertimeInitialization()

        Dim dailyRate As Decimal = 537
        Dim monthlyRate As Decimal = 13962 '537 * 26 days
        Dim hourlyRate As Decimal = CDec(67.125) '537 / 8 hours

        Dim actualDailyRate As Decimal = 600 '537 + 63
        Dim actualMonthlyRate As Decimal = 15600 '600 * 26 days
        Dim actualHourlyRate As Decimal = 75 '600 / 8 hours

        Assert.AreEqual(dailyRate, _dailyBenchmarkPaystubRate.DailyRate)
        Assert.AreEqual(monthlyRate, _dailyBenchmarkPaystubRate.MonthlyRate)
        Assert.AreEqual(hourlyRate, _dailyBenchmarkPaystubRate.HourlyRate)
        Assert.AreEqual(actualDailyRate, _dailyBenchmarkPaystubRate.ActualDailyRate)
        Assert.AreEqual(actualMonthlyRate, _dailyBenchmarkPaystubRate.ActualMonthlyRate)
        Assert.AreEqual(actualHourlyRate, _dailyBenchmarkPaystubRate.ActualHourlyRate)

        Assert.AreEqual(_dailySalary.AllowanceSalary, _dailyBenchmarkPaystubRate.AllowanceSalary)

        'Monthly
        Assert.AreEqual(dailyRate, _monthlyBenchmarkPaystubRate.DailyRate)
        Assert.AreEqual(monthlyRate, _monthlyBenchmarkPaystubRate.MonthlyRate)
        Assert.AreEqual(hourlyRate, _monthlyBenchmarkPaystubRate.HourlyRate)
        Assert.AreEqual(actualDailyRate, _monthlyBenchmarkPaystubRate.ActualDailyRate)
        Assert.AreEqual(actualMonthlyRate, _monthlyBenchmarkPaystubRate.ActualMonthlyRate)
        Assert.AreEqual(actualHourlyRate, _monthlyBenchmarkPaystubRate.ActualHourlyRate)

        Assert.AreEqual(_monthlySalary.AllowanceSalary, _monthlyBenchmarkPaystubRate.AllowanceSalary)

    End Sub

    <Test>
    Public Sub ShouldPassCorrectPolicyAndHours()
        Dim employeeEntitledForSpecialHolidayPay As Boolean = True
        Dim employeeEntitledForRegularHolidayPay As Boolean = True
        Dim employeeEntitledForNightDifferentialPay As Boolean = True
        Dim allowanceForOvertimePolicy As Boolean = True
        Dim allowanceForNightDiffPolicy As Boolean = False
        Dim allowanceForNightDiffOTPolicy As Boolean = True
        Dim allowanceForHolidayPolicy As Boolean = False
        Dim allowanceForRestDayPolicy As Boolean = True
        Dim allowanceForRestDayOTPolicy As Boolean = False
        Dim employeeEntitledForRestDayPay As Boolean = True

        Dim regularHours As Decimal = 1
        Dim overtimeHours As Decimal = 2

        Dim nightDiffHours As Decimal = 3
        Dim nightDiffOvertimeHours As Decimal = 4
        Dim restDayHours As Decimal = 5
        Dim restDayOTHours As Decimal = 6
        Dim specialHolidayHours As Decimal = 7
        Dim specialHolidayOTHours As Decimal = 8
        Dim regularHolidayHours As Decimal = 9
        Dim regularHolidayOTHours As Decimal = 10

        Dim leaveHours As Decimal = 11
        Dim lateHours As Decimal = 12
        Dim undertimeHours As Decimal = 13
        Dim absentHours As Decimal = 14

        Dim restDayNightDiffHours As Decimal = 15
        Dim restDayNightDiffOTHours As Decimal = 16
        Dim specialHolidayNightDiffHours As Decimal = 17
        Dim specialHolidayNightDiffOTHours As Decimal = 18
        Dim specialHolidayRestDayHours As Decimal = 19
        Dim specialHolidayRestDayOTHours As Decimal = 20
        Dim specialHolidayRestDayNightDiffHours As Decimal = 21
        Dim specialHolidayRestDayNightDiffOTHours As Decimal = 22
        Dim regularHolidayNightDiffHours As Decimal = 23
        Dim regularHolidayNightDiffOTHours As Decimal = 24
        Dim regularHolidayRestDayHours As Decimal = 25
        Dim regularHolidayRestDayOTHours As Decimal = 26
        Dim regularHolidayRestDayNightDiffHours As Decimal = 27
        Dim regularHolidayRestDayNightDiffOTHours As Decimal = 28

        'Daily
        _dailyBenchmarkPaystubRate.Compute(
            _overtimeRate,
            employeeEntitledForNightDifferentialPay:=employeeEntitledForNightDifferentialPay,
            employeeEntitledForSpecialHolidayPay:=employeeEntitledForSpecialHolidayPay,
            employeeEntitledForRegularHolidayPay:=employeeEntitledForRegularHolidayPay,
            employeeEntitledForRestDayPay:=employeeEntitledForRestDayPay,
            allowanceForOvertimePolicy:=allowanceForOvertimePolicy,
            allowanceForNightDiffPolicy:=allowanceForNightDiffPolicy,
            allowanceForNightDiffOTPolicy:=allowanceForNightDiffOTPolicy,
            allowanceForHolidayPolicy:=allowanceForHolidayPolicy,
            allowanceForRestDayPolicy:=allowanceForRestDayPolicy,
            allowanceForRestDayOTPolicy:=allowanceForRestDayOTPolicy,
            regularHours:=regularHours,
            overtimeHours:=overtimeHours,
            nightDiffHours:=nightDiffHours,
            nightDiffOvertimeHours:=nightDiffOvertimeHours,
            restDayHours:=restDayHours,
            restDayOTHours:=restDayOTHours,
            specialHolidayHours:=specialHolidayHours,
            specialHolidayOTHours:=specialHolidayOTHours,
            regularHolidayHours:=regularHolidayHours,
            regularHolidayOTHours:=regularHolidayOTHours,
            leaveHours:=leaveHours,
            lateHours:=lateHours,
            undertimeHours:=undertimeHours,
            absentHours:=absentHours,
            restDayNightDiffHours:=restDayNightDiffHours,
            restDayNightDiffOTHours:=restDayNightDiffOTHours,
            specialHolidayNightDiffHours:=specialHolidayNightDiffHours,
            specialHolidayNightDiffOTHours:=specialHolidayNightDiffOTHours,
            specialHolidayRestDayHours:=specialHolidayRestDayHours,
            specialHolidayRestDayOTHours:=specialHolidayRestDayOTHours,
            specialHolidayRestDayNightDiffHours:=specialHolidayRestDayNightDiffHours,
            specialHolidayRestDayNightDiffOTHours:=specialHolidayRestDayNightDiffOTHours,
            regularHolidayNightDiffHours:=regularHolidayNightDiffHours,
            regularHolidayNightDiffOTHours:=regularHolidayNightDiffOTHours,
            regularHolidayRestDayHours:=regularHolidayRestDayHours,
            regularHolidayRestDayOTHours:=regularHolidayRestDayOTHours,
            regularHolidayRestDayNightDiffHours:=regularHolidayRestDayNightDiffHours,
            regularHolidayRestDayNightDiffOTHours:=regularHolidayRestDayNightDiffOTHours)

        Assert.AreEqual(regularHours, _dailyBenchmarkPaystubRate.RegularHours)
        Assert.AreEqual(overtimeHours, _dailyBenchmarkPaystubRate.OvertimeHours)

        Assert.AreEqual(nightDiffHours, _dailyBenchmarkPaystubRate.NightDiffHours)
        Assert.AreEqual(nightDiffOvertimeHours, _dailyBenchmarkPaystubRate.NightDiffOvertimeHours)
        Assert.AreEqual(restDayHours, _dailyBenchmarkPaystubRate.RestDayHours)
        Assert.AreEqual(restDayOTHours, _dailyBenchmarkPaystubRate.RestDayOTHours)
        Assert.AreEqual(specialHolidayHours, _dailyBenchmarkPaystubRate.SpecialHolidayHours)
        Assert.AreEqual(specialHolidayOTHours, _dailyBenchmarkPaystubRate.SpecialHolidayOTHours)
        Assert.AreEqual(regularHolidayHours, _dailyBenchmarkPaystubRate.RegularHolidayHours)
        Assert.AreEqual(regularHolidayOTHours, _dailyBenchmarkPaystubRate.RegularHolidayOTHours)

        Assert.AreEqual(leaveHours, _dailyBenchmarkPaystubRate.LeaveHours)
        Assert.AreEqual(lateHours, _dailyBenchmarkPaystubRate.LateHours)
        Assert.AreEqual(undertimeHours, _dailyBenchmarkPaystubRate.UndertimeHours)
        Assert.AreEqual(absentHours, _dailyBenchmarkPaystubRate.AbsentHours)

        Assert.AreEqual(restDayNightDiffHours, _dailyBenchmarkPaystubRate.RestDayNightDiffHours)
        Assert.AreEqual(restDayNightDiffOTHours, _dailyBenchmarkPaystubRate.RestDayNightDiffOTHours)
        Assert.AreEqual(specialHolidayNightDiffHours, _dailyBenchmarkPaystubRate.SpecialHolidayNightDiffHours)
        Assert.AreEqual(specialHolidayNightDiffOTHours, _dailyBenchmarkPaystubRate.SpecialHolidayNightDiffOTHours)
        Assert.AreEqual(specialHolidayRestDayHours, _dailyBenchmarkPaystubRate.SpecialHolidayRestDayHours)
        Assert.AreEqual(specialHolidayRestDayOTHours, _dailyBenchmarkPaystubRate.SpecialHolidayRestDayOTHours)
        Assert.AreEqual(specialHolidayRestDayNightDiffHours, _dailyBenchmarkPaystubRate.SpecialHolidayRestDayNightDiffHours)
        Assert.AreEqual(specialHolidayRestDayNightDiffOTHours, _dailyBenchmarkPaystubRate.SpecialHolidayRestDayNightDiffOTHours)
        Assert.AreEqual(regularHolidayNightDiffHours, _dailyBenchmarkPaystubRate.RegularHolidayNightDiffHours)
        Assert.AreEqual(regularHolidayNightDiffOTHours, _dailyBenchmarkPaystubRate.RegularHolidayNightDiffOTHours)
        Assert.AreEqual(regularHolidayRestDayHours, _dailyBenchmarkPaystubRate.RegularHolidayRestDayHours)
        Assert.AreEqual(regularHolidayRestDayOTHours, _dailyBenchmarkPaystubRate.RegularHolidayRestDayOTHours)
        Assert.AreEqual(regularHolidayRestDayNightDiffHours, _dailyBenchmarkPaystubRate.RegularHolidayRestDayNightDiffHours)
        Assert.AreEqual(regularHolidayRestDayNightDiffOTHours, _dailyBenchmarkPaystubRate.RegularHolidayRestDayNightDiffOTHours)

        'Monthly
        _monthlyBenchmarkPaystubRate.Compute(
            _overtimeRate,
            employeeEntitledForNightDifferentialPay:=employeeEntitledForNightDifferentialPay,
            employeeEntitledForSpecialHolidayPay:=employeeEntitledForSpecialHolidayPay,
            employeeEntitledForRegularHolidayPay:=employeeEntitledForRegularHolidayPay,
            employeeEntitledForRestDayPay:=employeeEntitledForRestDayPay,
            allowanceForOvertimePolicy:=allowanceForOvertimePolicy,
            allowanceForNightDiffPolicy:=allowanceForNightDiffPolicy,
            allowanceForNightDiffOTPolicy:=allowanceForNightDiffOTPolicy,
            allowanceForHolidayPolicy:=allowanceForHolidayPolicy,
            allowanceForRestDayPolicy:=allowanceForRestDayPolicy,
            allowanceForRestDayOTPolicy:=allowanceForRestDayOTPolicy,
            regularHours:=regularHours,
            overtimeHours:=overtimeHours,
            nightDiffHours:=nightDiffHours,
            nightDiffOvertimeHours:=nightDiffOvertimeHours,
            restDayHours:=restDayHours,
            restDayOTHours:=restDayOTHours,
            specialHolidayHours:=specialHolidayHours,
            specialHolidayOTHours:=specialHolidayOTHours,
            regularHolidayHours:=regularHolidayHours,
            regularHolidayOTHours:=regularHolidayOTHours,
            leaveHours:=leaveHours,
            lateHours:=lateHours,
            undertimeHours:=undertimeHours,
            absentHours:=absentHours,
            restDayNightDiffHours:=restDayNightDiffHours,
            restDayNightDiffOTHours:=restDayNightDiffOTHours,
            specialHolidayNightDiffHours:=specialHolidayNightDiffHours,
            specialHolidayNightDiffOTHours:=specialHolidayNightDiffOTHours,
            specialHolidayRestDayHours:=specialHolidayRestDayHours,
            specialHolidayRestDayOTHours:=specialHolidayRestDayOTHours,
            specialHolidayRestDayNightDiffHours:=specialHolidayRestDayNightDiffHours,
            specialHolidayRestDayNightDiffOTHours:=specialHolidayRestDayNightDiffOTHours,
            regularHolidayNightDiffHours:=regularHolidayNightDiffHours,
            regularHolidayNightDiffOTHours:=regularHolidayNightDiffOTHours,
            regularHolidayRestDayHours:=regularHolidayRestDayHours,
            regularHolidayRestDayOTHours:=regularHolidayRestDayOTHours,
            regularHolidayRestDayNightDiffHours:=regularHolidayRestDayNightDiffHours,
            regularHolidayRestDayNightDiffOTHours:=regularHolidayRestDayNightDiffOTHours)

        Assert.AreEqual(regularHours, _monthlyBenchmarkPaystubRate.RegularHours)
        Assert.AreEqual(overtimeHours, _monthlyBenchmarkPaystubRate.OvertimeHours)

        Assert.AreEqual(nightDiffHours, _monthlyBenchmarkPaystubRate.NightDiffHours)
        Assert.AreEqual(nightDiffOvertimeHours, _monthlyBenchmarkPaystubRate.NightDiffOvertimeHours)
        Assert.AreEqual(restDayHours, _monthlyBenchmarkPaystubRate.RestDayHours)
        Assert.AreEqual(restDayOTHours, _monthlyBenchmarkPaystubRate.RestDayOTHours)
        Assert.AreEqual(specialHolidayHours, _monthlyBenchmarkPaystubRate.SpecialHolidayHours)
        Assert.AreEqual(specialHolidayOTHours, _monthlyBenchmarkPaystubRate.SpecialHolidayOTHours)
        Assert.AreEqual(regularHolidayHours, _monthlyBenchmarkPaystubRate.RegularHolidayHours)
        Assert.AreEqual(regularHolidayOTHours, _monthlyBenchmarkPaystubRate.RegularHolidayOTHours)

        Assert.AreEqual(leaveHours, _monthlyBenchmarkPaystubRate.LeaveHours)
        Assert.AreEqual(lateHours, _monthlyBenchmarkPaystubRate.LateHours)
        Assert.AreEqual(undertimeHours, _monthlyBenchmarkPaystubRate.UndertimeHours)
        Assert.AreEqual(absentHours, _monthlyBenchmarkPaystubRate.AbsentHours)

        Assert.AreEqual(restDayNightDiffHours, _monthlyBenchmarkPaystubRate.RestDayNightDiffHours)
        Assert.AreEqual(restDayNightDiffOTHours, _monthlyBenchmarkPaystubRate.RestDayNightDiffOTHours)
        Assert.AreEqual(specialHolidayNightDiffHours, _monthlyBenchmarkPaystubRate.SpecialHolidayNightDiffHours)
        Assert.AreEqual(specialHolidayNightDiffOTHours, _monthlyBenchmarkPaystubRate.SpecialHolidayNightDiffOTHours)
        Assert.AreEqual(specialHolidayRestDayHours, _monthlyBenchmarkPaystubRate.SpecialHolidayRestDayHours)
        Assert.AreEqual(specialHolidayRestDayOTHours, _monthlyBenchmarkPaystubRate.SpecialHolidayRestDayOTHours)
        Assert.AreEqual(specialHolidayRestDayNightDiffHours, _monthlyBenchmarkPaystubRate.SpecialHolidayRestDayNightDiffHours)
        Assert.AreEqual(specialHolidayRestDayNightDiffOTHours, _monthlyBenchmarkPaystubRate.SpecialHolidayRestDayNightDiffOTHours)
        Assert.AreEqual(regularHolidayNightDiffHours, _monthlyBenchmarkPaystubRate.RegularHolidayNightDiffHours)
        Assert.AreEqual(regularHolidayNightDiffOTHours, _monthlyBenchmarkPaystubRate.RegularHolidayNightDiffOTHours)
        Assert.AreEqual(regularHolidayRestDayHours, _monthlyBenchmarkPaystubRate.RegularHolidayRestDayHours)
        Assert.AreEqual(regularHolidayRestDayOTHours, _monthlyBenchmarkPaystubRate.RegularHolidayRestDayOTHours)
        Assert.AreEqual(regularHolidayRestDayNightDiffHours, _monthlyBenchmarkPaystubRate.RegularHolidayRestDayNightDiffHours)
        Assert.AreEqual(regularHolidayRestDayNightDiffOTHours, _monthlyBenchmarkPaystubRate.RegularHolidayRestDayNightDiffOTHours)
    End Sub

    <Test>
    Public Sub ShouldComputeRegularPays()
        DefaultOvertimeInitialization()

        Dim hourlyRate = 67.125
        Dim actualHourlyRate = 75

        Dim overtimeRate = 1.25

        'Daily
        Assert.AreEqual(_dailyBenchmarkPaystubRate.RegularPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.RegularHours * hourlyRate, 4))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualRegularPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.RegularHours * actualHourlyRate, 4))

        Assert.AreEqual(_dailyBenchmarkPaystubRate.OvertimePay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.OvertimeHours * hourlyRate * overtimeRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualOvertimePay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.OvertimeHours * actualHourlyRate * overtimeRate))

        Assert.AreEqual(_dailyBenchmarkPaystubRate.LeavePay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.LeaveHours * hourlyRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualLeavePay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.LeaveHours * actualHourlyRate))

        'Monthly
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.RegularPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.RegularHours * hourlyRate, 4))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualRegularPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.RegularHours * actualHourlyRate, 4))

        Assert.AreEqual(_monthlyBenchmarkPaystubRate.OvertimePay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.OvertimeHours * hourlyRate * overtimeRate))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualOvertimePay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.OvertimeHours * actualHourlyRate * overtimeRate))

        Assert.AreEqual(_monthlyBenchmarkPaystubRate.LeavePay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.LeaveHours * hourlyRate))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualLeavePay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.LeaveHours * actualHourlyRate))

    End Sub

    <Test>
    Public Sub ShouldComputeRegularPays_WithFalseAllowanceForOvertimePolicy()
        DefaultOvertimeInitialization(allowanceForOvertimePolicy:=False)

        Dim hourlyRate = 67.125
        Dim actualHourlyRate = 75

        Dim overtimeRate = 1.25

        'Dai;y
        Assert.AreEqual(_dailyBenchmarkPaystubRate.RegularPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.RegularHours * hourlyRate, 4))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualRegularPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.RegularHours * actualHourlyRate, 4))

        Assert.AreEqual(_dailyBenchmarkPaystubRate.OvertimePay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.OvertimeHours * hourlyRate * overtimeRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualOvertimePay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.OvertimePay))

        Assert.AreEqual(_dailyBenchmarkPaystubRate.LeavePay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.LeaveHours * hourlyRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualLeavePay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.LeaveHours * actualHourlyRate))

        'Monthly
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.RegularPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.RegularHours * hourlyRate, 4))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualRegularPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.RegularHours * actualHourlyRate, 4))

        Assert.AreEqual(_monthlyBenchmarkPaystubRate.OvertimePay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.OvertimeHours * hourlyRate * overtimeRate))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualOvertimePay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.OvertimePay))

        Assert.AreEqual(_monthlyBenchmarkPaystubRate.LeavePay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.LeaveHours * hourlyRate))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualLeavePay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.LeaveHours * actualHourlyRate))
    End Sub

    <Test>
    Public Sub ShouldComputeDeductions()
        DefaultOvertimeInitialization()

        Dim hourlyRate = 67.125
        Dim actualHourlyRate = 75

        Assert.AreEqual(_dailyBenchmarkPaystubRate.LateDeduction, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.LateHours * hourlyRate), 4)
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualLateDeduction, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.LateHours * actualHourlyRate))

        Assert.AreEqual(_dailyBenchmarkPaystubRate.UndertimeDeduction, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.UndertimeHours * hourlyRate), 4)
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualUndertimeDeduction, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.UndertimeHours * actualHourlyRate))

        Assert.AreEqual(_dailyBenchmarkPaystubRate.AbsenceDeduction, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.AbsentHours * hourlyRate), 4)
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualAbsenceDeduction, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.AbsentHours * actualHourlyRate))

        'Monthly
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.LateDeduction, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.LateHours * hourlyRate), 4)
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualLateDeduction, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.LateHours * actualHourlyRate))

        Assert.AreEqual(_monthlyBenchmarkPaystubRate.UndertimeDeduction, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.UndertimeHours * hourlyRate), 4)
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualUndertimeDeduction, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.UndertimeHours * actualHourlyRate))

        Assert.AreEqual(_monthlyBenchmarkPaystubRate.AbsenceDeduction, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.AbsentHours * hourlyRate), 4)
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualAbsenceDeduction, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.AbsentHours * actualHourlyRate))

    End Sub

    <Test>
    Public Sub ShouldComputeNightDifferentialPays()
        DefaultOvertimeInitialization()

        Dim restDayRate = 1.3
        Dim specialHolidayRate = 1.3
        Dim specialHolidayRestDayRate = 1.5

        Dim regularHolidayRate = 2
        Dim regularHolidayRestDayRate = 2.6

        Dim nightDifferentialRate = 0.1

        Dim hourlyRate = 67.125
        Dim actualHourlyRate = 75

        Assert.AreEqual(_dailyBenchmarkPaystubRate.NightDiffPay, AccuMath.CommercialRound(hourlyRate * nightDifferentialRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualNightDiffPay, AccuMath.CommercialRound(actualHourlyRate * nightDifferentialRate))

        Assert.AreEqual(_dailyBenchmarkPaystubRate.RestDayNightDiffPay, AccuMath.CommercialRound(hourlyRate * restDayRate * nightDifferentialRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualRestDayNightDiffPay, AccuMath.CommercialRound(actualHourlyRate * restDayRate * nightDifferentialRate))

        Assert.AreEqual(_dailyBenchmarkPaystubRate.SpecialHolidayNightDiffPay, AccuMath.CommercialRound(hourlyRate * specialHolidayRate * nightDifferentialRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualSpecialHolidayNightDiffPay, AccuMath.CommercialRound(actualHourlyRate * specialHolidayRate * nightDifferentialRate))

        Assert.AreEqual(_dailyBenchmarkPaystubRate.SpecialHolidayRestDayNightDiffPay, AccuMath.CommercialRound(hourlyRate * specialHolidayRestDayRate * nightDifferentialRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualSpecialHolidayRestDayNightDiffPay, AccuMath.CommercialRound(actualHourlyRate * specialHolidayRestDayRate * nightDifferentialRate))

        Assert.AreEqual(_dailyBenchmarkPaystubRate.RegularHolidayNightDiffPay, AccuMath.CommercialRound(hourlyRate * regularHolidayRate * nightDifferentialRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualRegularHolidayNightDiffPay, AccuMath.CommercialRound(actualHourlyRate * regularHolidayRate * nightDifferentialRate))

        Assert.AreEqual(_dailyBenchmarkPaystubRate.RegularHolidayRestDayNightDiffPay, AccuMath.CommercialRound(hourlyRate * regularHolidayRestDayRate * nightDifferentialRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualRegularHolidayRestDayNightDiffPay, AccuMath.CommercialRound(actualHourlyRate * regularHolidayRestDayRate * nightDifferentialRate))

        'Monthly
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.NightDiffPay, AccuMath.CommercialRound(hourlyRate * nightDifferentialRate))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualNightDiffPay, AccuMath.CommercialRound(actualHourlyRate * nightDifferentialRate))

        Assert.AreEqual(_monthlyBenchmarkPaystubRate.RestDayNightDiffPay, AccuMath.CommercialRound(hourlyRate * restDayRate * nightDifferentialRate))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualRestDayNightDiffPay, AccuMath.CommercialRound(actualHourlyRate * restDayRate * nightDifferentialRate))

        Assert.AreEqual(_monthlyBenchmarkPaystubRate.SpecialHolidayNightDiffPay, AccuMath.CommercialRound(hourlyRate * specialHolidayRate * nightDifferentialRate))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualSpecialHolidayNightDiffPay, AccuMath.CommercialRound(actualHourlyRate * specialHolidayRate * nightDifferentialRate))

        Assert.AreEqual(_monthlyBenchmarkPaystubRate.SpecialHolidayRestDayNightDiffPay, AccuMath.CommercialRound(hourlyRate * specialHolidayRestDayRate * nightDifferentialRate))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualSpecialHolidayRestDayNightDiffPay, AccuMath.CommercialRound(actualHourlyRate * specialHolidayRestDayRate * nightDifferentialRate))

        Assert.AreEqual(_monthlyBenchmarkPaystubRate.RegularHolidayNightDiffPay, AccuMath.CommercialRound(hourlyRate * regularHolidayRate * nightDifferentialRate))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualRegularHolidayNightDiffPay, AccuMath.CommercialRound(actualHourlyRate * regularHolidayRate * nightDifferentialRate))

        Assert.AreEqual(_monthlyBenchmarkPaystubRate.RegularHolidayRestDayNightDiffPay, AccuMath.CommercialRound(hourlyRate * regularHolidayRestDayRate * nightDifferentialRate))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualRegularHolidayRestDayNightDiffPay, AccuMath.CommercialRound(actualHourlyRate * regularHolidayRestDayRate * nightDifferentialRate))

    End Sub

    <Test>
    Public Sub ShouldComputeNightDifferentialPays_WithFalseAllowanceForNightDiffPolicy()
        DefaultOvertimeInitialization(allowanceForNightDiffPolicy:=False)

        Dim restDayRate = 1.3
        Dim specialHolidayRate = 1.3
        Dim specialHolidayRestDayRate = 1.5

        Dim regularHolidayRate = 2
        Dim regularHolidayRestDayRate = 2.6

        Dim nightDifferentialRate = 0.1

        Dim hourlyRate = 67.125

        Assert.AreEqual(_dailyBenchmarkPaystubRate.NightDiffPay, AccuMath.CommercialRound(hourlyRate * nightDifferentialRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualNightDiffPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.NightDiffPay))

        Assert.AreEqual(_dailyBenchmarkPaystubRate.RestDayNightDiffPay, AccuMath.CommercialRound(hourlyRate * restDayRate * nightDifferentialRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualRestDayNightDiffPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.RestDayNightDiffPay))

        Assert.AreEqual(_dailyBenchmarkPaystubRate.SpecialHolidayNightDiffPay, AccuMath.CommercialRound(hourlyRate * specialHolidayRate * nightDifferentialRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualSpecialHolidayNightDiffPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.SpecialHolidayNightDiffPay))

        Assert.AreEqual(_dailyBenchmarkPaystubRate.SpecialHolidayRestDayNightDiffPay, AccuMath.CommercialRound(hourlyRate * specialHolidayRestDayRate * nightDifferentialRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualSpecialHolidayRestDayNightDiffPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.SpecialHolidayRestDayNightDiffPay))

        Assert.AreEqual(_dailyBenchmarkPaystubRate.RegularHolidayNightDiffPay, AccuMath.CommercialRound(hourlyRate * regularHolidayRate * nightDifferentialRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualRegularHolidayNightDiffPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.RegularHolidayNightDiffPay))

        Assert.AreEqual(_dailyBenchmarkPaystubRate.RegularHolidayRestDayNightDiffPay, AccuMath.CommercialRound(hourlyRate * regularHolidayRestDayRate * nightDifferentialRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualRegularHolidayRestDayNightDiffPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.RegularHolidayRestDayNightDiffPay))

        'Monthly
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.NightDiffPay, AccuMath.CommercialRound(hourlyRate * nightDifferentialRate))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualNightDiffPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.NightDiffPay))

        Assert.AreEqual(_monthlyBenchmarkPaystubRate.RestDayNightDiffPay, AccuMath.CommercialRound(hourlyRate * restDayRate * nightDifferentialRate))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualRestDayNightDiffPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.RestDayNightDiffPay))

        Assert.AreEqual(_monthlyBenchmarkPaystubRate.SpecialHolidayNightDiffPay, AccuMath.CommercialRound(hourlyRate * specialHolidayRate * nightDifferentialRate))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualSpecialHolidayNightDiffPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.SpecialHolidayNightDiffPay))

        Assert.AreEqual(_monthlyBenchmarkPaystubRate.SpecialHolidayRestDayNightDiffPay, AccuMath.CommercialRound(hourlyRate * specialHolidayRestDayRate * nightDifferentialRate))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualSpecialHolidayRestDayNightDiffPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.SpecialHolidayRestDayNightDiffPay))

        Assert.AreEqual(_monthlyBenchmarkPaystubRate.RegularHolidayNightDiffPay, AccuMath.CommercialRound(hourlyRate * regularHolidayRate * nightDifferentialRate))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualRegularHolidayNightDiffPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.RegularHolidayNightDiffPay))

        Assert.AreEqual(_monthlyBenchmarkPaystubRate.RegularHolidayRestDayNightDiffPay, AccuMath.CommercialRound(hourlyRate * regularHolidayRestDayRate * nightDifferentialRate))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualRegularHolidayRestDayNightDiffPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.RegularHolidayRestDayNightDiffPay))
    End Sub

    <Test>
    Public Sub ShouldComputeNightDifferentialOvertimePays()
        DefaultOvertimeInitialization()

        'night diff OT adds the base OT that is why we are using 1.1 for ND rate

        Dim hourlyRate = 67.125
        Dim actualHourlyRate = 75
        Dim nightDifferentialRate = 1.1

        Dim overtimeRate = 1.25
        Dim restDayOvertimeRate = 1.69
        Dim specialHolidayOvertimeRate = 1.69
        Dim specialHolidayRestDayOvertimeRate = 1.95
        Dim regularHolidayOvertimeRate = 2.6
        Dim regularHolidayRestDayOvertimeRate = 3.38

        Assert.AreEqual(_dailyBenchmarkPaystubRate.NightDiffOvertimePay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.NightDiffOvertimeHours * hourlyRate * overtimeRate * nightDifferentialRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualNightDiffOvertimePay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.NightDiffOvertimeHours * actualHourlyRate * overtimeRate * nightDifferentialRate))

        Assert.AreEqual(_dailyBenchmarkPaystubRate.RestDayNightDiffOTPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.RestDayNightDiffOTHours * hourlyRate * restDayOvertimeRate * nightDifferentialRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualRestDayNightDiffOTPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.RestDayNightDiffOTHours * actualHourlyRate * restDayOvertimeRate * nightDifferentialRate))

        Assert.AreEqual(_dailyBenchmarkPaystubRate.SpecialHolidayNightDiffOTPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.SpecialHolidayNightDiffOTHours * hourlyRate * specialHolidayOvertimeRate * nightDifferentialRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualSpecialHolidayNightDiffOTPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.SpecialHolidayNightDiffOTHours * actualHourlyRate * specialHolidayOvertimeRate * nightDifferentialRate))

        Assert.AreEqual(_dailyBenchmarkPaystubRate.SpecialHolidayRestDayNightDiffOTPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.SpecialHolidayRestDayNightDiffOTHours * hourlyRate * specialHolidayRestDayOvertimeRate * nightDifferentialRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualSpecialHolidayRestDayNightDiffOTPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.SpecialHolidayRestDayNightDiffOTHours * actualHourlyRate * specialHolidayRestDayOvertimeRate * nightDifferentialRate))

        Assert.AreEqual(_dailyBenchmarkPaystubRate.RegularHolidayNightDiffOTPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.RegularHolidayNightDiffOTHours * hourlyRate * regularHolidayOvertimeRate * nightDifferentialRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualRegularHolidayNightDiffOTPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.RegularHolidayNightDiffOTHours * actualHourlyRate * regularHolidayOvertimeRate * nightDifferentialRate))

        Assert.AreEqual(_dailyBenchmarkPaystubRate.RegularHolidayRestDayNightDiffOTPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.RegularHolidayRestDayNightDiffOTHours * hourlyRate * regularHolidayRestDayOvertimeRate * nightDifferentialRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualRegularHolidayRestDayNightDiffOTPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.RegularHolidayRestDayNightDiffOTHours * actualHourlyRate * regularHolidayRestDayOvertimeRate * nightDifferentialRate))

        'Monthly
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.NightDiffOvertimePay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.NightDiffOvertimeHours * hourlyRate * overtimeRate * nightDifferentialRate))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualNightDiffOvertimePay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.NightDiffOvertimeHours * actualHourlyRate * overtimeRate * nightDifferentialRate))

        Assert.AreEqual(_monthlyBenchmarkPaystubRate.RestDayNightDiffOTPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.RestDayNightDiffOTHours * hourlyRate * restDayOvertimeRate * nightDifferentialRate))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualRestDayNightDiffOTPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.RestDayNightDiffOTHours * actualHourlyRate * restDayOvertimeRate * nightDifferentialRate))

        Assert.AreEqual(_monthlyBenchmarkPaystubRate.SpecialHolidayNightDiffOTPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.SpecialHolidayNightDiffOTHours * hourlyRate * specialHolidayOvertimeRate * nightDifferentialRate))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualSpecialHolidayNightDiffOTPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.SpecialHolidayNightDiffOTHours * actualHourlyRate * specialHolidayOvertimeRate * nightDifferentialRate))

        Assert.AreEqual(_monthlyBenchmarkPaystubRate.SpecialHolidayRestDayNightDiffOTPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.SpecialHolidayRestDayNightDiffOTHours * hourlyRate * specialHolidayRestDayOvertimeRate * nightDifferentialRate))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualSpecialHolidayRestDayNightDiffOTPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.SpecialHolidayRestDayNightDiffOTHours * actualHourlyRate * specialHolidayRestDayOvertimeRate * nightDifferentialRate))

        Assert.AreEqual(_monthlyBenchmarkPaystubRate.RegularHolidayNightDiffOTPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.RegularHolidayNightDiffOTHours * hourlyRate * regularHolidayOvertimeRate * nightDifferentialRate))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualRegularHolidayNightDiffOTPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.RegularHolidayNightDiffOTHours * actualHourlyRate * regularHolidayOvertimeRate * nightDifferentialRate))

        Assert.AreEqual(_monthlyBenchmarkPaystubRate.RegularHolidayRestDayNightDiffOTPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.RegularHolidayRestDayNightDiffOTHours * hourlyRate * regularHolidayRestDayOvertimeRate * nightDifferentialRate))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualRegularHolidayRestDayNightDiffOTPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.RegularHolidayRestDayNightDiffOTHours * actualHourlyRate * regularHolidayRestDayOvertimeRate * nightDifferentialRate))
    End Sub

    <Test>
    Public Sub ShouldComputeNightDifferentialOvertimePays_WithFalseEmployeeEntitledForNightDifferentialPay()
        DefaultOvertimeInitialization(employeeEntitledForNightDifferentialPay:=False)

        Assert.AreEqual(_dailyBenchmarkPaystubRate.NightDiffOvertimePay, 0)
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualNightDiffOvertimePay, 0)

        Assert.AreEqual(_dailyBenchmarkPaystubRate.RestDayNightDiffOTPay, 0)
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualRestDayNightDiffOTPay, 0)

        Assert.AreEqual(_dailyBenchmarkPaystubRate.SpecialHolidayNightDiffOTPay, 0)
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualSpecialHolidayNightDiffOTPay, 0)

        Assert.AreEqual(_dailyBenchmarkPaystubRate.SpecialHolidayRestDayNightDiffOTPay, 0)
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualSpecialHolidayRestDayNightDiffOTPay, 0)

        Assert.AreEqual(_dailyBenchmarkPaystubRate.RegularHolidayNightDiffOTPay, 0)
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualRegularHolidayNightDiffOTPay, 0)

        Assert.AreEqual(_dailyBenchmarkPaystubRate.RegularHolidayRestDayNightDiffOTPay, 0)
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualRegularHolidayRestDayNightDiffOTPay, 0)

        'Monthly
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.NightDiffOvertimePay, 0)
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualNightDiffOvertimePay, 0)

        Assert.AreEqual(_monthlyBenchmarkPaystubRate.RestDayNightDiffOTPay, 0)
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualRestDayNightDiffOTPay, 0)

        Assert.AreEqual(_monthlyBenchmarkPaystubRate.SpecialHolidayNightDiffOTPay, 0)
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualSpecialHolidayNightDiffOTPay, 0)

        Assert.AreEqual(_monthlyBenchmarkPaystubRate.SpecialHolidayRestDayNightDiffOTPay, 0)
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualSpecialHolidayRestDayNightDiffOTPay, 0)

        Assert.AreEqual(_monthlyBenchmarkPaystubRate.RegularHolidayNightDiffOTPay, 0)
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualRegularHolidayNightDiffOTPay, 0)

        Assert.AreEqual(_monthlyBenchmarkPaystubRate.RegularHolidayRestDayNightDiffOTPay, 0)
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualRegularHolidayRestDayNightDiffOTPay, 0)
    End Sub

    <Test>
    Public Sub ShouldComputeNightDifferentialOvertimePays_WithFalseAllowanceForNightDiffOTPolicy()
        DefaultOvertimeInitialization(allowanceForNightDiffOTPolicy:=False)

        'night diff OT adds the base OT that is why we are using 1.1 for ND rate

        Dim hourlyRate = 67.125
        Dim nightDifferentialRate = 1.1

        Dim overtimeRate = 1.25
        Dim restDayOvertimeRate = 1.69
        Dim specialHolidayOvertimeRate = 1.69
        Dim specialHolidayRestDayOvertimeRate = 1.95
        Dim regularHolidayOvertimeRate = 2.6
        Dim regularHolidayRestDayOvertimeRate = 3.38

        Assert.AreEqual(_dailyBenchmarkPaystubRate.NightDiffOvertimePay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.NightDiffOvertimeHours * hourlyRate * overtimeRate * nightDifferentialRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualNightDiffOvertimePay, _dailyBenchmarkPaystubRate.NightDiffOvertimePay)

        Assert.AreEqual(_dailyBenchmarkPaystubRate.RestDayNightDiffOTPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.RestDayNightDiffOTHours * hourlyRate * restDayOvertimeRate * nightDifferentialRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualRestDayNightDiffOTPay, _dailyBenchmarkPaystubRate.RestDayNightDiffOTPay)

        Assert.AreEqual(_dailyBenchmarkPaystubRate.SpecialHolidayNightDiffOTPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.SpecialHolidayNightDiffOTHours * hourlyRate * specialHolidayOvertimeRate * nightDifferentialRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualSpecialHolidayNightDiffOTPay, _dailyBenchmarkPaystubRate.SpecialHolidayNightDiffOTPay)

        Assert.AreEqual(_dailyBenchmarkPaystubRate.SpecialHolidayRestDayNightDiffOTPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.SpecialHolidayRestDayNightDiffOTHours * hourlyRate * specialHolidayRestDayOvertimeRate * nightDifferentialRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualSpecialHolidayRestDayNightDiffOTPay, _dailyBenchmarkPaystubRate.SpecialHolidayRestDayNightDiffOTPay)

        Assert.AreEqual(_dailyBenchmarkPaystubRate.RegularHolidayNightDiffOTPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.RegularHolidayNightDiffOTHours * hourlyRate * regularHolidayOvertimeRate * nightDifferentialRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualRegularHolidayNightDiffOTPay, _dailyBenchmarkPaystubRate.RegularHolidayNightDiffOTPay)

        Assert.AreEqual(_dailyBenchmarkPaystubRate.RegularHolidayRestDayNightDiffOTPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.RegularHolidayRestDayNightDiffOTHours * hourlyRate * regularHolidayRestDayOvertimeRate * nightDifferentialRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualRegularHolidayRestDayNightDiffOTPay, _dailyBenchmarkPaystubRate.RegularHolidayRestDayNightDiffOTPay)

        'Monthly
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.NightDiffOvertimePay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.NightDiffOvertimeHours * hourlyRate * overtimeRate * nightDifferentialRate))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualNightDiffOvertimePay, _monthlyBenchmarkPaystubRate.NightDiffOvertimePay)

        Assert.AreEqual(_monthlyBenchmarkPaystubRate.RestDayNightDiffOTPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.RestDayNightDiffOTHours * hourlyRate * restDayOvertimeRate * nightDifferentialRate))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualRestDayNightDiffOTPay, _monthlyBenchmarkPaystubRate.RestDayNightDiffOTPay)

        Assert.AreEqual(_monthlyBenchmarkPaystubRate.SpecialHolidayNightDiffOTPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.SpecialHolidayNightDiffOTHours * hourlyRate * specialHolidayOvertimeRate * nightDifferentialRate))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualSpecialHolidayNightDiffOTPay, _monthlyBenchmarkPaystubRate.SpecialHolidayNightDiffOTPay)

        Assert.AreEqual(_monthlyBenchmarkPaystubRate.SpecialHolidayRestDayNightDiffOTPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.SpecialHolidayRestDayNightDiffOTHours * hourlyRate * specialHolidayRestDayOvertimeRate * nightDifferentialRate))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualSpecialHolidayRestDayNightDiffOTPay, _monthlyBenchmarkPaystubRate.SpecialHolidayRestDayNightDiffOTPay)

        Assert.AreEqual(_monthlyBenchmarkPaystubRate.RegularHolidayNightDiffOTPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.RegularHolidayNightDiffOTHours * hourlyRate * regularHolidayOvertimeRate * nightDifferentialRate))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualRegularHolidayNightDiffOTPay, _monthlyBenchmarkPaystubRate.RegularHolidayNightDiffOTPay)

        Assert.AreEqual(_monthlyBenchmarkPaystubRate.RegularHolidayRestDayNightDiffOTPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.RegularHolidayRestDayNightDiffOTHours * hourlyRate * regularHolidayRestDayOvertimeRate * nightDifferentialRate))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualRegularHolidayRestDayNightDiffOTPay, _monthlyBenchmarkPaystubRate.RegularHolidayRestDayNightDiffOTPay)
    End Sub

#Region "ComputeHolidayPays"

    <Test>
    Public Sub ShouldComputeHolidayPays()
        DefaultOvertimeInitialization()

        Dim specialHolidayOvertimeRate = 1.69
        Dim specialHolidayRestDayOvertimeRate = 1.95
        Dim regularHolidayOvertimeRate = 2.6
        Dim regularHolidayRestDayOvertimeRate = 3.38

        Dim specialHolidayRate = 1.3
        Dim specialHolidayRestDayRate = 1.5
        Dim regularHolidayRate = 2
        Dim regularHolidayRestDayRate = 2.6

        Dim hourlyRate = 67.125
        Dim actualHourlyRate = 75

        Assert.AreEqual(_dailyBenchmarkPaystubRate.SpecialHolidayPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.SpecialHolidayHours * hourlyRate * specialHolidayRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualSpecialHolidayPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.SpecialHolidayHours * actualHourlyRate * specialHolidayRate))

        Assert.AreEqual(_dailyBenchmarkPaystubRate.SpecialHolidayOTPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.SpecialHolidayOTHours * hourlyRate * specialHolidayOvertimeRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualSpecialHolidayOTPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.SpecialHolidayOTHours * actualHourlyRate * specialHolidayOvertimeRate))

        Assert.AreEqual(_dailyBenchmarkPaystubRate.SpecialHolidayRestDayPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.SpecialHolidayRestDayHours * hourlyRate * specialHolidayRestDayRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualSpecialHolidayRestDayPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.SpecialHolidayRestDayHours * actualHourlyRate * specialHolidayRestDayRate))

        Assert.AreEqual(_dailyBenchmarkPaystubRate.SpecialHolidayRestDayOTPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.SpecialHolidayRestDayOTHours * hourlyRate * specialHolidayRestDayOvertimeRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualSpecialHolidayRestDayOTPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.SpecialHolidayRestDayOTHours * actualHourlyRate * specialHolidayRestDayOvertimeRate))

        Assert.AreEqual(_dailyBenchmarkPaystubRate.RegularHolidayPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.RegularHolidayHours * hourlyRate * regularHolidayRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualRegularHolidayPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.RegularHolidayHours * actualHourlyRate * regularHolidayRate))

        Assert.AreEqual(_dailyBenchmarkPaystubRate.RegularHolidayOTPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.RegularHolidayOTHours * hourlyRate * regularHolidayOvertimeRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualRegularHolidayOTPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.RegularHolidayOTHours * actualHourlyRate * regularHolidayOvertimeRate))

        Assert.AreEqual(_dailyBenchmarkPaystubRate.RegularHolidayRestDayPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.RegularHolidayRestDayHours * hourlyRate * regularHolidayRestDayRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualRegularHolidayRestDayPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.RegularHolidayRestDayHours * actualHourlyRate * regularHolidayRestDayRate))

        Assert.AreEqual(_dailyBenchmarkPaystubRate.RegularHolidayRestDayOTPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.RegularHolidayRestDayOTHours * hourlyRate * regularHolidayRestDayOvertimeRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualRegularHolidayRestDayOTPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.RegularHolidayRestDayOTHours * actualHourlyRate * regularHolidayRestDayOvertimeRate))

        'Monthly
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.SpecialHolidayPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.SpecialHolidayHours * hourlyRate * (specialHolidayRate - 1)))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualSpecialHolidayPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.SpecialHolidayHours * actualHourlyRate * (specialHolidayRate - 1)))

        Assert.AreEqual(_monthlyBenchmarkPaystubRate.SpecialHolidayOTPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.SpecialHolidayOTHours * hourlyRate * specialHolidayOvertimeRate))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualSpecialHolidayOTPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.SpecialHolidayOTHours * actualHourlyRate * specialHolidayOvertimeRate))

        Assert.AreEqual(_monthlyBenchmarkPaystubRate.SpecialHolidayRestDayPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.SpecialHolidayRestDayHours * hourlyRate * specialHolidayRestDayRate))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualSpecialHolidayRestDayPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.SpecialHolidayRestDayHours * actualHourlyRate * specialHolidayRestDayRate))

        Assert.AreEqual(_monthlyBenchmarkPaystubRate.SpecialHolidayRestDayOTPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.SpecialHolidayRestDayOTHours * hourlyRate * specialHolidayRestDayOvertimeRate))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualSpecialHolidayRestDayOTPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.SpecialHolidayRestDayOTHours * actualHourlyRate * specialHolidayRestDayOvertimeRate))

        Assert.AreEqual(_monthlyBenchmarkPaystubRate.RegularHolidayPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.RegularHolidayHours * hourlyRate * (regularHolidayRate - 1)))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualRegularHolidayPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.RegularHolidayHours * actualHourlyRate * (regularHolidayRate - 1)))

        Assert.AreEqual(_monthlyBenchmarkPaystubRate.RegularHolidayOTPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.RegularHolidayOTHours * hourlyRate * regularHolidayOvertimeRate))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualRegularHolidayOTPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.RegularHolidayOTHours * actualHourlyRate * regularHolidayOvertimeRate))

        Assert.AreEqual(_monthlyBenchmarkPaystubRate.RegularHolidayRestDayPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.RegularHolidayRestDayHours * hourlyRate * regularHolidayRestDayRate))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualRegularHolidayRestDayPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.RegularHolidayRestDayHours * actualHourlyRate * regularHolidayRestDayRate))

        Assert.AreEqual(_monthlyBenchmarkPaystubRate.RegularHolidayRestDayOTPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.RegularHolidayRestDayOTHours * hourlyRate * regularHolidayRestDayOvertimeRate))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualRegularHolidayRestDayOTPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.RegularHolidayRestDayOTHours * actualHourlyRate * regularHolidayRestDayOvertimeRate))
    End Sub

    <Test>
    Public Sub ShouldComputeHolidayPays_WithFalseEmployeeEntitledForSpecialHolidayPay()
        DefaultOvertimeInitialization(employeeEntitledForSpecialHolidayPay:=False)

        Dim regularHolidayRate = 2
        Dim regularHolidayRestDayRate = 2.6

        Dim regularHolidayOvertimeRate = 2.6
        Dim regularHolidayRestDayOvertimeRate = 3.38

        Dim overtimeRate = 1.25
        Dim restDayRate = 1.3
        Dim restDayOvertimeRate = 1.69

        Dim hourlyRate = 67.125
        Dim actualHourlyRate = 75

        Assert.AreEqual(_dailyBenchmarkPaystubRate.SpecialHolidayPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.SpecialHolidayHours * hourlyRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualSpecialHolidayPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.SpecialHolidayHours * actualHourlyRate))

        Assert.AreEqual(_dailyBenchmarkPaystubRate.SpecialHolidayOTPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.SpecialHolidayOTHours * hourlyRate * overtimeRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualSpecialHolidayOTPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.SpecialHolidayOTHours * actualHourlyRate * overtimeRate))

        Assert.AreEqual(_dailyBenchmarkPaystubRate.SpecialHolidayRestDayPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.SpecialHolidayRestDayHours * hourlyRate * restDayRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualSpecialHolidayRestDayPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.SpecialHolidayRestDayHours * actualHourlyRate * restDayRate))

        Assert.AreEqual(_dailyBenchmarkPaystubRate.SpecialHolidayRestDayOTPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.SpecialHolidayRestDayOTHours * hourlyRate * restDayOvertimeRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualSpecialHolidayRestDayOTPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.SpecialHolidayRestDayOTHours * actualHourlyRate * restDayOvertimeRate))

        Assert.AreEqual(_dailyBenchmarkPaystubRate.RegularHolidayPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.RegularHolidayHours * hourlyRate * regularHolidayRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualRegularHolidayPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.RegularHolidayHours * actualHourlyRate * regularHolidayRate))

        Assert.AreEqual(_dailyBenchmarkPaystubRate.RegularHolidayOTPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.RegularHolidayOTHours * hourlyRate * regularHolidayOvertimeRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualRegularHolidayOTPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.RegularHolidayOTHours * actualHourlyRate * regularHolidayOvertimeRate))

        Assert.AreEqual(_dailyBenchmarkPaystubRate.RegularHolidayRestDayPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.RegularHolidayRestDayHours * hourlyRate * regularHolidayRestDayRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualRegularHolidayRestDayPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.RegularHolidayRestDayHours * actualHourlyRate * regularHolidayRestDayRate))

        Assert.AreEqual(_dailyBenchmarkPaystubRate.RegularHolidayRestDayOTPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.RegularHolidayRestDayOTHours * hourlyRate * regularHolidayRestDayOvertimeRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualRegularHolidayRestDayOTPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.RegularHolidayRestDayOTHours * actualHourlyRate * regularHolidayRestDayOvertimeRate))

        'Monthly
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.SpecialHolidayPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.SpecialHolidayHours * hourlyRate))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualSpecialHolidayPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.SpecialHolidayHours * actualHourlyRate))

        Assert.AreEqual(_monthlyBenchmarkPaystubRate.SpecialHolidayOTPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.SpecialHolidayOTHours * hourlyRate * overtimeRate))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualSpecialHolidayOTPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.SpecialHolidayOTHours * actualHourlyRate * overtimeRate))

        Assert.AreEqual(_monthlyBenchmarkPaystubRate.SpecialHolidayRestDayPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.SpecialHolidayRestDayHours * hourlyRate * restDayRate))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualSpecialHolidayRestDayPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.SpecialHolidayRestDayHours * actualHourlyRate * restDayRate))

        Assert.AreEqual(_monthlyBenchmarkPaystubRate.SpecialHolidayRestDayOTPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.SpecialHolidayRestDayOTHours * hourlyRate * restDayOvertimeRate))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualSpecialHolidayRestDayOTPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.SpecialHolidayRestDayOTHours * actualHourlyRate * restDayOvertimeRate))

        Assert.AreEqual(_monthlyBenchmarkPaystubRate.RegularHolidayPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.RegularHolidayHours * hourlyRate * (regularHolidayRate - 1)))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualRegularHolidayPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.RegularHolidayHours * actualHourlyRate * (regularHolidayRate - 1)))

        Assert.AreEqual(_monthlyBenchmarkPaystubRate.RegularHolidayOTPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.RegularHolidayOTHours * hourlyRate * regularHolidayOvertimeRate))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualRegularHolidayOTPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.RegularHolidayOTHours * actualHourlyRate * regularHolidayOvertimeRate))

        Assert.AreEqual(_monthlyBenchmarkPaystubRate.RegularHolidayRestDayPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.RegularHolidayRestDayHours * hourlyRate * regularHolidayRestDayRate))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualRegularHolidayRestDayPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.RegularHolidayRestDayHours * actualHourlyRate * regularHolidayRestDayRate))

        Assert.AreEqual(_monthlyBenchmarkPaystubRate.RegularHolidayRestDayOTPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.RegularHolidayRestDayOTHours * hourlyRate * regularHolidayRestDayOvertimeRate))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualRegularHolidayRestDayOTPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.RegularHolidayRestDayOTHours * actualHourlyRate * regularHolidayRestDayOvertimeRate))
    End Sub

    <Test>
    Public Sub ShouldComputeHolidayPays_WithFalseEmployeeEntitledForRegularHolidayPay()
        DefaultOvertimeInitialization(employeeEntitledForRegularHolidayPay:=False)

        Dim specialHolidayRate = 1.3
        Dim specialHolidayRestDayRate = 1.5
        Dim specialHolidayOvertimeRate = 1.69
        Dim specialHolidayRestDayOvertimeRate = 1.95

        Dim overtimeRate = 1.25
        Dim restDayRate = 1.3
        Dim restDayOvertimeRate = 1.69

        Dim hourlyRate = 67.125
        Dim actualHourlyRate = 75

        Assert.AreEqual(_dailyBenchmarkPaystubRate.SpecialHolidayPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.SpecialHolidayHours * hourlyRate * specialHolidayRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualSpecialHolidayPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.SpecialHolidayHours * actualHourlyRate * specialHolidayRate))

        Assert.AreEqual(_dailyBenchmarkPaystubRate.SpecialHolidayOTPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.SpecialHolidayOTHours * hourlyRate * specialHolidayOvertimeRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualSpecialHolidayOTPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.SpecialHolidayOTHours * actualHourlyRate * specialHolidayOvertimeRate))

        Assert.AreEqual(_dailyBenchmarkPaystubRate.SpecialHolidayRestDayPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.SpecialHolidayRestDayHours * hourlyRate * specialHolidayRestDayRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualSpecialHolidayRestDayPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.SpecialHolidayRestDayHours * actualHourlyRate * specialHolidayRestDayRate))

        Assert.AreEqual(_dailyBenchmarkPaystubRate.SpecialHolidayRestDayOTPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.SpecialHolidayRestDayOTHours * hourlyRate * specialHolidayRestDayOvertimeRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualSpecialHolidayRestDayOTPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.SpecialHolidayRestDayOTHours * actualHourlyRate * specialHolidayRestDayOvertimeRate))

        Assert.AreEqual(_dailyBenchmarkPaystubRate.RegularHolidayPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.RegularHolidayHours * hourlyRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualRegularHolidayPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.RegularHolidayHours * actualHourlyRate))

        Assert.AreEqual(_dailyBenchmarkPaystubRate.RegularHolidayOTPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.RegularHolidayOTHours * hourlyRate * overtimeRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualRegularHolidayOTPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.RegularHolidayOTHours * actualHourlyRate * overtimeRate))

        Assert.AreEqual(_dailyBenchmarkPaystubRate.RegularHolidayRestDayPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.RegularHolidayRestDayHours * hourlyRate * restDayRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualRegularHolidayRestDayPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.RegularHolidayRestDayHours * actualHourlyRate * restDayRate))

        Assert.AreEqual(_dailyBenchmarkPaystubRate.RegularHolidayRestDayOTPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.RegularHolidayRestDayOTHours * hourlyRate * restDayOvertimeRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualRegularHolidayRestDayOTPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.RegularHolidayRestDayOTHours * actualHourlyRate * restDayOvertimeRate))

        'Monthly
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.SpecialHolidayPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.SpecialHolidayHours * hourlyRate * (specialHolidayRate - 1)))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualSpecialHolidayPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.SpecialHolidayHours * actualHourlyRate * (specialHolidayRate - 1)))

        Assert.AreEqual(_monthlyBenchmarkPaystubRate.SpecialHolidayOTPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.SpecialHolidayOTHours * hourlyRate * specialHolidayOvertimeRate))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualSpecialHolidayOTPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.SpecialHolidayOTHours * actualHourlyRate * specialHolidayOvertimeRate))

        Assert.AreEqual(_monthlyBenchmarkPaystubRate.SpecialHolidayRestDayPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.SpecialHolidayRestDayHours * hourlyRate * specialHolidayRestDayRate))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualSpecialHolidayRestDayPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.SpecialHolidayRestDayHours * actualHourlyRate * specialHolidayRestDayRate))

        Assert.AreEqual(_monthlyBenchmarkPaystubRate.SpecialHolidayRestDayOTPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.SpecialHolidayRestDayOTHours * hourlyRate * specialHolidayRestDayOvertimeRate))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualSpecialHolidayRestDayOTPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.SpecialHolidayRestDayOTHours * actualHourlyRate * specialHolidayRestDayOvertimeRate))

        Assert.AreEqual(_monthlyBenchmarkPaystubRate.RegularHolidayPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.RegularHolidayHours * hourlyRate))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualRegularHolidayPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.RegularHolidayHours * actualHourlyRate))

        Assert.AreEqual(_monthlyBenchmarkPaystubRate.RegularHolidayOTPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.RegularHolidayOTHours * hourlyRate * overtimeRate))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualRegularHolidayOTPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.RegularHolidayOTHours * actualHourlyRate * overtimeRate))

        Assert.AreEqual(_monthlyBenchmarkPaystubRate.RegularHolidayRestDayPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.RegularHolidayRestDayHours * hourlyRate * restDayRate))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualRegularHolidayRestDayPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.RegularHolidayRestDayHours * actualHourlyRate * restDayRate))

        Assert.AreEqual(_monthlyBenchmarkPaystubRate.RegularHolidayRestDayOTPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.RegularHolidayRestDayOTHours * hourlyRate * restDayOvertimeRate))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualRegularHolidayRestDayOTPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.RegularHolidayRestDayOTHours * actualHourlyRate * restDayOvertimeRate))
    End Sub

    <Test>
    Public Sub ShouldComputeHolidayPays_WithFalseAllowanceForHolidayPolicy()
        DefaultOvertimeInitialization(allowanceForHolidayPolicy:=False)

        Dim specialHolidayRate = 1.3
        Dim specialHolidayRestDayRate = 1.5
        Dim regularHolidayRate = 2
        Dim regularHolidayRestDayRate = 2.6

        Dim specialHolidayOvertimeRate = 1.69
        Dim specialHolidayRestDayOvertimeRate = 1.95
        Dim regularHolidayOvertimeRate = 2.6
        Dim regularHolidayRestDayOvertimeRate = 3.38

        Dim hourlyRate = 67.125

        Assert.AreEqual(_dailyBenchmarkPaystubRate.SpecialHolidayPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.SpecialHolidayHours * hourlyRate * specialHolidayRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualSpecialHolidayPay, _dailyBenchmarkPaystubRate.SpecialHolidayPay)

        Assert.AreEqual(_dailyBenchmarkPaystubRate.SpecialHolidayOTPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.SpecialHolidayOTHours * hourlyRate * specialHolidayOvertimeRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualSpecialHolidayOTPay, _dailyBenchmarkPaystubRate.SpecialHolidayOTPay)

        Assert.AreEqual(_dailyBenchmarkPaystubRate.SpecialHolidayRestDayPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.SpecialHolidayRestDayHours * hourlyRate * specialHolidayRestDayRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualSpecialHolidayRestDayPay, _dailyBenchmarkPaystubRate.SpecialHolidayRestDayPay)

        Assert.AreEqual(_dailyBenchmarkPaystubRate.SpecialHolidayRestDayOTPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.SpecialHolidayRestDayOTHours * hourlyRate * specialHolidayRestDayOvertimeRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualSpecialHolidayRestDayOTPay, _dailyBenchmarkPaystubRate.SpecialHolidayRestDayOTPay)

        Assert.AreEqual(_dailyBenchmarkPaystubRate.RegularHolidayPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.RegularHolidayHours * hourlyRate * regularHolidayRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualRegularHolidayPay, _dailyBenchmarkPaystubRate.RegularHolidayPay)

        Assert.AreEqual(_dailyBenchmarkPaystubRate.RegularHolidayOTPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.RegularHolidayOTHours * hourlyRate * regularHolidayOvertimeRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualRegularHolidayOTPay, _dailyBenchmarkPaystubRate.RegularHolidayOTPay)

        Assert.AreEqual(_dailyBenchmarkPaystubRate.RegularHolidayRestDayPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.RegularHolidayRestDayHours * hourlyRate * regularHolidayRestDayRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualRegularHolidayRestDayPay, _dailyBenchmarkPaystubRate.RegularHolidayRestDayPay)

        Assert.AreEqual(_dailyBenchmarkPaystubRate.RegularHolidayRestDayOTPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.RegularHolidayRestDayOTHours * hourlyRate * regularHolidayRestDayOvertimeRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualRegularHolidayRestDayOTPay, _dailyBenchmarkPaystubRate.RegularHolidayRestDayOTPay)

        'Monthly
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.SpecialHolidayPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.SpecialHolidayHours * hourlyRate * (specialHolidayRate - 1)))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualSpecialHolidayPay, _monthlyBenchmarkPaystubRate.SpecialHolidayPay)

        Assert.AreEqual(_monthlyBenchmarkPaystubRate.SpecialHolidayOTPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.SpecialHolidayOTHours * hourlyRate * specialHolidayOvertimeRate))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualSpecialHolidayOTPay, _monthlyBenchmarkPaystubRate.SpecialHolidayOTPay)

        Assert.AreEqual(_monthlyBenchmarkPaystubRate.SpecialHolidayRestDayPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.SpecialHolidayRestDayHours * hourlyRate * specialHolidayRestDayRate))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualSpecialHolidayRestDayPay, _monthlyBenchmarkPaystubRate.SpecialHolidayRestDayPay)

        Assert.AreEqual(_monthlyBenchmarkPaystubRate.SpecialHolidayRestDayOTPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.SpecialHolidayRestDayOTHours * hourlyRate * specialHolidayRestDayOvertimeRate))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualSpecialHolidayRestDayOTPay, _monthlyBenchmarkPaystubRate.SpecialHolidayRestDayOTPay)

        Assert.AreEqual(_monthlyBenchmarkPaystubRate.RegularHolidayPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.RegularHolidayHours * hourlyRate * (regularHolidayRate - 1)))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualRegularHolidayPay, _monthlyBenchmarkPaystubRate.RegularHolidayPay)

        Assert.AreEqual(_monthlyBenchmarkPaystubRate.RegularHolidayOTPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.RegularHolidayOTHours * hourlyRate * regularHolidayOvertimeRate))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualRegularHolidayOTPay, _monthlyBenchmarkPaystubRate.RegularHolidayOTPay)

        Assert.AreEqual(_monthlyBenchmarkPaystubRate.RegularHolidayRestDayPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.RegularHolidayRestDayHours * hourlyRate * regularHolidayRestDayRate))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualRegularHolidayRestDayPay, _monthlyBenchmarkPaystubRate.RegularHolidayRestDayPay)

        Assert.AreEqual(_monthlyBenchmarkPaystubRate.RegularHolidayRestDayOTPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.RegularHolidayRestDayOTHours * hourlyRate * regularHolidayRestDayOvertimeRate))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualRegularHolidayRestDayOTPay, _monthlyBenchmarkPaystubRate.RegularHolidayRestDayOTPay)
    End Sub

    <Test>
    Public Sub ShouldComputeHolidayPays_WithFalseEmployeeEntitledForSpecialHolidayPayAndAllowanceForHolidayPolicy()
        DefaultOvertimeInitialization(employeeEntitledForSpecialHolidayPay:=False, allowanceForHolidayPolicy:=False)

        Dim regularHolidayRate = 2
        Dim regularHolidayRestDayRate = 2.6

        Dim regularHolidayOvertimeRate = 2.6
        Dim regularHolidayRestDayOvertimeRate = 3.38

        Dim overtimeRate = 1.25
        Dim restDayRate = 1.3
        Dim restDayOvertimeRate = 1.69

        Dim hourlyRate = 67.125
        Dim actualHourlyRate = 75

        Assert.AreEqual(_dailyBenchmarkPaystubRate.SpecialHolidayPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.SpecialHolidayHours * hourlyRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualSpecialHolidayPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.SpecialHolidayHours * actualHourlyRate))

        Assert.AreEqual(_dailyBenchmarkPaystubRate.SpecialHolidayOTPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.SpecialHolidayOTHours * hourlyRate * overtimeRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualSpecialHolidayOTPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.SpecialHolidayOTHours * actualHourlyRate * overtimeRate))

        Assert.AreEqual(_dailyBenchmarkPaystubRate.SpecialHolidayRestDayPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.SpecialHolidayRestDayHours * hourlyRate * restDayRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualSpecialHolidayRestDayPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.SpecialHolidayRestDayHours * actualHourlyRate * restDayRate))

        Assert.AreEqual(_dailyBenchmarkPaystubRate.SpecialHolidayRestDayOTPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.SpecialHolidayRestDayOTHours * hourlyRate * restDayOvertimeRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualSpecialHolidayRestDayOTPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.SpecialHolidayRestDayOTHours * actualHourlyRate * restDayOvertimeRate))

        Assert.AreEqual(_dailyBenchmarkPaystubRate.RegularHolidayPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.RegularHolidayHours * hourlyRate * regularHolidayRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualRegularHolidayPay, _dailyBenchmarkPaystubRate.RegularHolidayPay)

        Assert.AreEqual(_dailyBenchmarkPaystubRate.RegularHolidayOTPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.RegularHolidayOTHours * hourlyRate * regularHolidayOvertimeRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualRegularHolidayOTPay, _dailyBenchmarkPaystubRate.RegularHolidayOTPay)

        Assert.AreEqual(_dailyBenchmarkPaystubRate.RegularHolidayRestDayPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.RegularHolidayRestDayHours * hourlyRate * regularHolidayRestDayRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualRegularHolidayRestDayPay, _dailyBenchmarkPaystubRate.RegularHolidayRestDayPay)

        Assert.AreEqual(_dailyBenchmarkPaystubRate.RegularHolidayRestDayOTPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.RegularHolidayRestDayOTHours * hourlyRate * regularHolidayRestDayOvertimeRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualRegularHolidayRestDayOTPay, _dailyBenchmarkPaystubRate.RegularHolidayRestDayOTPay)

        'Monthly
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.SpecialHolidayPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.SpecialHolidayHours * hourlyRate))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualSpecialHolidayPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.SpecialHolidayHours * actualHourlyRate))

        Assert.AreEqual(_monthlyBenchmarkPaystubRate.SpecialHolidayOTPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.SpecialHolidayOTHours * hourlyRate * overtimeRate))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualSpecialHolidayOTPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.SpecialHolidayOTHours * actualHourlyRate * overtimeRate))

        Assert.AreEqual(_monthlyBenchmarkPaystubRate.SpecialHolidayRestDayPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.SpecialHolidayRestDayHours * hourlyRate * restDayRate))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualSpecialHolidayRestDayPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.SpecialHolidayRestDayHours * actualHourlyRate * restDayRate))

        Assert.AreEqual(_monthlyBenchmarkPaystubRate.SpecialHolidayRestDayOTPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.SpecialHolidayRestDayOTHours * hourlyRate * restDayOvertimeRate))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualSpecialHolidayRestDayOTPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.SpecialHolidayRestDayOTHours * actualHourlyRate * restDayOvertimeRate))

        Assert.AreEqual(_monthlyBenchmarkPaystubRate.RegularHolidayPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.RegularHolidayHours * hourlyRate * (regularHolidayRate - 1)))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualRegularHolidayPay, _monthlyBenchmarkPaystubRate.RegularHolidayPay)

        Assert.AreEqual(_monthlyBenchmarkPaystubRate.RegularHolidayOTPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.RegularHolidayOTHours * hourlyRate * regularHolidayOvertimeRate))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualRegularHolidayOTPay, _monthlyBenchmarkPaystubRate.RegularHolidayOTPay)

        Assert.AreEqual(_monthlyBenchmarkPaystubRate.RegularHolidayRestDayPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.RegularHolidayRestDayHours * hourlyRate * regularHolidayRestDayRate))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualRegularHolidayRestDayPay, _monthlyBenchmarkPaystubRate.RegularHolidayRestDayPay)

        Assert.AreEqual(_monthlyBenchmarkPaystubRate.RegularHolidayRestDayOTPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.RegularHolidayRestDayOTHours * hourlyRate * regularHolidayRestDayOvertimeRate))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualRegularHolidayRestDayOTPay, _monthlyBenchmarkPaystubRate.RegularHolidayRestDayOTPay)
    End Sub

    <Test>
    Public Sub ShouldComputeHolidayPays_WithFalseEmployeeEntitledForSpecialHolidayPayAndOtherPolicyInvolved()
        DefaultOvertimeInitialization(
            employeeEntitledForSpecialHolidayPay:=False,
            allowanceForHolidayPolicy:=False,
            allowanceForRestDayPolicy:=False,
            allowanceForRestDayOTPolicy:=False,
            allowanceForOvertimePolicy:=False)

        Dim regularHolidayRate = 2
        Dim regularHolidayRestDayRate = 2.6

        Dim regularHolidayOvertimeRate = 2.6
        Dim regularHolidayRestDayOvertimeRate = 3.38

        Dim overtimeRate = 1.25
        Dim restDayRate = 1.3
        Dim restDayOvertimeRate = 1.69

        Dim hourlyRate = 67.125
        Dim actualHourlyRate = 75

        Assert.AreEqual(_dailyBenchmarkPaystubRate.SpecialHolidayPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.SpecialHolidayHours * hourlyRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualSpecialHolidayPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.SpecialHolidayHours * actualHourlyRate))

        Assert.AreEqual(_dailyBenchmarkPaystubRate.SpecialHolidayOTPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.SpecialHolidayOTHours * hourlyRate * overtimeRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualSpecialHolidayOTPay, _dailyBenchmarkPaystubRate.SpecialHolidayOTPay)

        Assert.AreEqual(_dailyBenchmarkPaystubRate.SpecialHolidayRestDayPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.SpecialHolidayRestDayHours * hourlyRate * restDayRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualSpecialHolidayRestDayPay, _dailyBenchmarkPaystubRate.SpecialHolidayRestDayPay)

        Assert.AreEqual(_dailyBenchmarkPaystubRate.SpecialHolidayRestDayOTPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.SpecialHolidayRestDayOTHours * hourlyRate * restDayOvertimeRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualSpecialHolidayRestDayOTPay, _dailyBenchmarkPaystubRate.SpecialHolidayRestDayOTPay)

        Assert.AreEqual(_dailyBenchmarkPaystubRate.RegularHolidayPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.RegularHolidayHours * hourlyRate * regularHolidayRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualRegularHolidayPay, _dailyBenchmarkPaystubRate.RegularHolidayPay)

        Assert.AreEqual(_dailyBenchmarkPaystubRate.RegularHolidayOTPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.RegularHolidayOTHours * hourlyRate * regularHolidayOvertimeRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualRegularHolidayOTPay, _dailyBenchmarkPaystubRate.RegularHolidayOTPay)

        Assert.AreEqual(_dailyBenchmarkPaystubRate.RegularHolidayRestDayPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.RegularHolidayRestDayHours * hourlyRate * regularHolidayRestDayRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualRegularHolidayRestDayPay, _dailyBenchmarkPaystubRate.RegularHolidayRestDayPay)

        Assert.AreEqual(_dailyBenchmarkPaystubRate.RegularHolidayRestDayOTPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.RegularHolidayRestDayOTHours * hourlyRate * regularHolidayRestDayOvertimeRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualRegularHolidayRestDayOTPay, _dailyBenchmarkPaystubRate.RegularHolidayRestDayOTPay)

        'Monthly
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.SpecialHolidayPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.SpecialHolidayHours * hourlyRate))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualSpecialHolidayPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.SpecialHolidayHours * actualHourlyRate))

        Assert.AreEqual(_monthlyBenchmarkPaystubRate.SpecialHolidayOTPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.SpecialHolidayOTHours * hourlyRate * overtimeRate))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualSpecialHolidayOTPay, _monthlyBenchmarkPaystubRate.SpecialHolidayOTPay)

        Assert.AreEqual(_monthlyBenchmarkPaystubRate.SpecialHolidayRestDayPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.SpecialHolidayRestDayHours * hourlyRate * restDayRate))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualSpecialHolidayRestDayPay, _monthlyBenchmarkPaystubRate.SpecialHolidayRestDayPay)

        Assert.AreEqual(_monthlyBenchmarkPaystubRate.SpecialHolidayRestDayOTPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.SpecialHolidayRestDayOTHours * hourlyRate * restDayOvertimeRate))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualSpecialHolidayRestDayOTPay, _monthlyBenchmarkPaystubRate.SpecialHolidayRestDayOTPay)

        Assert.AreEqual(_monthlyBenchmarkPaystubRate.RegularHolidayPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.RegularHolidayHours * hourlyRate * (regularHolidayRate - 1)))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualRegularHolidayPay, _monthlyBenchmarkPaystubRate.RegularHolidayPay)

        Assert.AreEqual(_monthlyBenchmarkPaystubRate.RegularHolidayOTPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.RegularHolidayOTHours * hourlyRate * regularHolidayOvertimeRate))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualRegularHolidayOTPay, _monthlyBenchmarkPaystubRate.RegularHolidayOTPay)

        Assert.AreEqual(_monthlyBenchmarkPaystubRate.RegularHolidayRestDayPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.RegularHolidayRestDayHours * hourlyRate * regularHolidayRestDayRate))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualRegularHolidayRestDayPay, _monthlyBenchmarkPaystubRate.RegularHolidayRestDayPay)

        Assert.AreEqual(_monthlyBenchmarkPaystubRate.RegularHolidayRestDayOTPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.RegularHolidayRestDayOTHours * hourlyRate * regularHolidayRestDayOvertimeRate))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualRegularHolidayRestDayOTPay, _monthlyBenchmarkPaystubRate.RegularHolidayRestDayOTPay)
    End Sub

    <Test>
    Public Sub ShouldComputeHolidayPays_WithFalseEmployeeEntitledForRegularHolidayPayAndAllowanceForHolidayPay()
        DefaultOvertimeInitialization(employeeEntitledForRegularHolidayPay:=False, allowanceForHolidayPolicy:=False)

        Dim specialHolidayRate = 1.3
        Dim specialHolidayRestDayRate = 1.5
        Dim specialHolidayOvertimeRate = 1.69
        Dim specialHolidayRestDayOvertimeRate = 1.95

        Dim overtimeRate = 1.25
        Dim restDayRate = 1.3
        Dim restDayOvertimeRate = 1.69

        Dim hourlyRate = 67.125
        Dim actualHourlyRate = 75

        Assert.AreEqual(_dailyBenchmarkPaystubRate.SpecialHolidayPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.SpecialHolidayHours * hourlyRate * specialHolidayRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualSpecialHolidayPay, _dailyBenchmarkPaystubRate.SpecialHolidayPay)

        Assert.AreEqual(_dailyBenchmarkPaystubRate.SpecialHolidayOTPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.SpecialHolidayOTHours * hourlyRate * specialHolidayOvertimeRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualSpecialHolidayOTPay, _dailyBenchmarkPaystubRate.SpecialHolidayOTPay)

        Assert.AreEqual(_dailyBenchmarkPaystubRate.SpecialHolidayRestDayPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.SpecialHolidayRestDayHours * hourlyRate * specialHolidayRestDayRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualSpecialHolidayRestDayPay, _dailyBenchmarkPaystubRate.SpecialHolidayRestDayPay)

        Assert.AreEqual(_dailyBenchmarkPaystubRate.SpecialHolidayRestDayOTPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.SpecialHolidayRestDayOTHours * hourlyRate * specialHolidayRestDayOvertimeRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualSpecialHolidayRestDayOTPay, _dailyBenchmarkPaystubRate.SpecialHolidayRestDayOTPay)

        Assert.AreEqual(_dailyBenchmarkPaystubRate.RegularHolidayPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.RegularHolidayHours * hourlyRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualRegularHolidayPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.RegularHolidayHours * actualHourlyRate))

        Assert.AreEqual(_dailyBenchmarkPaystubRate.RegularHolidayOTPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.RegularHolidayOTHours * hourlyRate * overtimeRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualRegularHolidayOTPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.RegularHolidayOTHours * actualHourlyRate * overtimeRate))

        Assert.AreEqual(_dailyBenchmarkPaystubRate.RegularHolidayRestDayPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.RegularHolidayRestDayHours * hourlyRate * restDayRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualRegularHolidayRestDayPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.RegularHolidayRestDayHours * actualHourlyRate * restDayRate))

        Assert.AreEqual(_dailyBenchmarkPaystubRate.RegularHolidayRestDayOTPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.RegularHolidayRestDayOTHours * hourlyRate * restDayOvertimeRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualRegularHolidayRestDayOTPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.RegularHolidayRestDayOTHours * actualHourlyRate * restDayOvertimeRate))

        'Monthly
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.SpecialHolidayPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.SpecialHolidayHours * hourlyRate * (specialHolidayRate - 1)))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualSpecialHolidayPay, _monthlyBenchmarkPaystubRate.SpecialHolidayPay)

        Assert.AreEqual(_monthlyBenchmarkPaystubRate.SpecialHolidayOTPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.SpecialHolidayOTHours * hourlyRate * specialHolidayOvertimeRate))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualSpecialHolidayOTPay, _monthlyBenchmarkPaystubRate.SpecialHolidayOTPay)

        Assert.AreEqual(_monthlyBenchmarkPaystubRate.SpecialHolidayRestDayPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.SpecialHolidayRestDayHours * hourlyRate * specialHolidayRestDayRate))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualSpecialHolidayRestDayPay, _monthlyBenchmarkPaystubRate.SpecialHolidayRestDayPay)

        Assert.AreEqual(_monthlyBenchmarkPaystubRate.SpecialHolidayRestDayOTPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.SpecialHolidayRestDayOTHours * hourlyRate * specialHolidayRestDayOvertimeRate))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualSpecialHolidayRestDayOTPay, _monthlyBenchmarkPaystubRate.SpecialHolidayRestDayOTPay)

        Assert.AreEqual(_monthlyBenchmarkPaystubRate.RegularHolidayPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.RegularHolidayHours * hourlyRate))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualRegularHolidayPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.RegularHolidayHours * actualHourlyRate))

        Assert.AreEqual(_monthlyBenchmarkPaystubRate.RegularHolidayOTPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.RegularHolidayOTHours * hourlyRate * overtimeRate))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualRegularHolidayOTPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.RegularHolidayOTHours * actualHourlyRate * overtimeRate))

        Assert.AreEqual(_monthlyBenchmarkPaystubRate.RegularHolidayRestDayPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.RegularHolidayRestDayHours * hourlyRate * restDayRate))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualRegularHolidayRestDayPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.RegularHolidayRestDayHours * actualHourlyRate * restDayRate))

        Assert.AreEqual(_monthlyBenchmarkPaystubRate.RegularHolidayRestDayOTPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.RegularHolidayRestDayOTHours * hourlyRate * restDayOvertimeRate))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualRegularHolidayRestDayOTPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.RegularHolidayRestDayOTHours * actualHourlyRate * restDayOvertimeRate))
    End Sub

    <Test>
    Public Sub ShouldComputeHolidayPays_WithFalseEmployeeEntitledForRegularHolidayPayAndOtherPolicyInvolved()
        DefaultOvertimeInitialization(
            employeeEntitledForRegularHolidayPay:=False,
            allowanceForHolidayPolicy:=False,
            allowanceForRestDayPolicy:=False,
            allowanceForRestDayOTPolicy:=False,
            allowanceForOvertimePolicy:=False)

        Dim specialHolidayRate = 1.3
        Dim specialHolidayRestDayRate = 1.5
        Dim specialHolidayOvertimeRate = 1.69
        Dim specialHolidayRestDayOvertimeRate = 1.95

        Dim overtimeRate = 1.25
        Dim restDayRate = 1.3
        Dim restDayOvertimeRate = 1.69

        Dim hourlyRate = 67.125
        Dim actualHourlyRate = 75

        Assert.AreEqual(_dailyBenchmarkPaystubRate.SpecialHolidayPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.SpecialHolidayHours * hourlyRate * specialHolidayRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualSpecialHolidayPay, _dailyBenchmarkPaystubRate.SpecialHolidayPay)

        Assert.AreEqual(_dailyBenchmarkPaystubRate.SpecialHolidayOTPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.SpecialHolidayOTHours * hourlyRate * specialHolidayOvertimeRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualSpecialHolidayOTPay, _dailyBenchmarkPaystubRate.SpecialHolidayOTPay)

        Assert.AreEqual(_dailyBenchmarkPaystubRate.SpecialHolidayRestDayPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.SpecialHolidayRestDayHours * hourlyRate * specialHolidayRestDayRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualSpecialHolidayRestDayPay, _dailyBenchmarkPaystubRate.SpecialHolidayRestDayPay)

        Assert.AreEqual(_dailyBenchmarkPaystubRate.SpecialHolidayRestDayOTPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.SpecialHolidayRestDayOTHours * hourlyRate * specialHolidayRestDayOvertimeRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualSpecialHolidayRestDayOTPay, _dailyBenchmarkPaystubRate.SpecialHolidayRestDayOTPay)

        Assert.AreEqual(_dailyBenchmarkPaystubRate.RegularHolidayPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.RegularHolidayHours * hourlyRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualRegularHolidayPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.RegularHolidayHours * actualHourlyRate))

        Assert.AreEqual(_dailyBenchmarkPaystubRate.RegularHolidayOTPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.RegularHolidayOTHours * hourlyRate * overtimeRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualRegularHolidayOTPay, _dailyBenchmarkPaystubRate.RegularHolidayOTPay)

        Assert.AreEqual(_dailyBenchmarkPaystubRate.RegularHolidayRestDayPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.RegularHolidayRestDayHours * hourlyRate * restDayRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualRegularHolidayRestDayPay, _dailyBenchmarkPaystubRate.RegularHolidayRestDayPay)

        Assert.AreEqual(_dailyBenchmarkPaystubRate.RegularHolidayRestDayOTPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.RegularHolidayRestDayOTHours * hourlyRate * restDayOvertimeRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualRegularHolidayRestDayOTPay, _dailyBenchmarkPaystubRate.RegularHolidayRestDayOTPay)

        'Monthly
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.SpecialHolidayPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.SpecialHolidayHours * hourlyRate * (specialHolidayRate - 1)))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualSpecialHolidayPay, _monthlyBenchmarkPaystubRate.SpecialHolidayPay)

        Assert.AreEqual(_monthlyBenchmarkPaystubRate.SpecialHolidayOTPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.SpecialHolidayOTHours * hourlyRate * specialHolidayOvertimeRate))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualSpecialHolidayOTPay, _monthlyBenchmarkPaystubRate.SpecialHolidayOTPay)

        Assert.AreEqual(_monthlyBenchmarkPaystubRate.SpecialHolidayRestDayPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.SpecialHolidayRestDayHours * hourlyRate * specialHolidayRestDayRate))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualSpecialHolidayRestDayPay, _monthlyBenchmarkPaystubRate.SpecialHolidayRestDayPay)

        Assert.AreEqual(_monthlyBenchmarkPaystubRate.SpecialHolidayRestDayOTPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.SpecialHolidayRestDayOTHours * hourlyRate * specialHolidayRestDayOvertimeRate))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualSpecialHolidayRestDayOTPay, _monthlyBenchmarkPaystubRate.SpecialHolidayRestDayOTPay)

        Assert.AreEqual(_monthlyBenchmarkPaystubRate.RegularHolidayPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.RegularHolidayHours * hourlyRate))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualRegularHolidayPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.RegularHolidayHours * actualHourlyRate))

        Assert.AreEqual(_monthlyBenchmarkPaystubRate.RegularHolidayOTPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.RegularHolidayOTHours * hourlyRate * overtimeRate))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualRegularHolidayOTPay, _monthlyBenchmarkPaystubRate.RegularHolidayOTPay)

        Assert.AreEqual(_monthlyBenchmarkPaystubRate.RegularHolidayRestDayPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.RegularHolidayRestDayHours * hourlyRate * restDayRate))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualRegularHolidayRestDayPay, _monthlyBenchmarkPaystubRate.RegularHolidayRestDayPay)

        Assert.AreEqual(_monthlyBenchmarkPaystubRate.RegularHolidayRestDayOTPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.RegularHolidayRestDayOTHours * hourlyRate * restDayOvertimeRate))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualRegularHolidayRestDayOTPay, _monthlyBenchmarkPaystubRate.RegularHolidayRestDayOTPay)
    End Sub

#End Region

#Region "ComputeRestDayPays"

    <Test>
    Public Sub ShouldComputeRestDayPays()
        DefaultOvertimeInitialization()

        Dim hourlyRate = 67.125
        Dim actualHourlyRate = 75

        Dim restDayRate = 1.3
        Dim restDayOvertimeRate = 1.69

        Assert.AreEqual(_dailyBenchmarkPaystubRate.RestDayPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.RestDayHours * hourlyRate * restDayRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualRestDayPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.RestDayHours * actualHourlyRate * restDayRate))

        Assert.AreEqual(_dailyBenchmarkPaystubRate.RestDayOTPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.RestDayOTHours * hourlyRate * restDayOvertimeRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualRestDayOTPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.RestDayOTHours * actualHourlyRate * restDayOvertimeRate))

        'Monthly
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.RestDayPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.RestDayHours * hourlyRate * restDayRate))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualRestDayPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.RestDayHours * actualHourlyRate * restDayRate))

        Assert.AreEqual(_monthlyBenchmarkPaystubRate.RestDayOTPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.RestDayOTHours * hourlyRate * restDayOvertimeRate))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualRestDayOTPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.RestDayOTHours * actualHourlyRate * restDayOvertimeRate))
    End Sub

    <Test>
    Public Sub ShouldComputeRestDayPays_WithFalseEmployeeEntitledForRestDayPay()
        DefaultOvertimeInitialization(employeeEntitledForRestDayPay:=False)

        Assert.AreEqual(_dailyBenchmarkPaystubRate.RestDayPay, 0)
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualRestDayPay, 0)

        Assert.AreEqual(_dailyBenchmarkPaystubRate.RestDayOTPay, 0)
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualRestDayOTPay, 0)

        'Monthly
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.RestDayPay, 0)
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualRestDayPay, 0)

        Assert.AreEqual(_monthlyBenchmarkPaystubRate.RestDayOTPay, 0)
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualRestDayOTPay, 0)

    End Sub

    <Test>
    Public Sub ShouldComputeRestDayPays_WithAllowanceForRestDay()
        DefaultOvertimeInitialization(allowanceForRestDayPolicy:=False, allowanceForRestDayOTPolicy:=False)

        Dim hourlyRate = 67.125

        Dim restDayRate = 1.3
        Dim restDayOvertimeRate = 1.69

        Assert.AreEqual(_dailyBenchmarkPaystubRate.RestDayPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.RestDayHours * hourlyRate * restDayRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualRestDayPay, _dailyBenchmarkPaystubRate.RestDayPay)

        Assert.AreEqual(_dailyBenchmarkPaystubRate.RestDayOTPay, AccuMath.CommercialRound(_dailyBenchmarkPaystubRate.RestDayOTHours * hourlyRate * restDayOvertimeRate))
        Assert.AreEqual(_dailyBenchmarkPaystubRate.ActualRestDayOTPay, _dailyBenchmarkPaystubRate.RestDayOTPay)

        'Monthly
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.RestDayPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.RestDayHours * hourlyRate * restDayRate))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualRestDayPay, _monthlyBenchmarkPaystubRate.RestDayPay)

        Assert.AreEqual(_monthlyBenchmarkPaystubRate.RestDayOTPay, AccuMath.CommercialRound(_monthlyBenchmarkPaystubRate.RestDayOTHours * hourlyRate * restDayOvertimeRate))
        Assert.AreEqual(_monthlyBenchmarkPaystubRate.ActualRestDayOTPay, _monthlyBenchmarkPaystubRate.RestDayOTPay)
    End Sub

#End Region

    Private Sub DefaultOvertimeInitialization(
                Optional overridePayRate As Boolean = False,
                Optional payRate As OvertimeRate = Nothing,
                Optional employeeEntitledForNightDifferentialPay As Boolean = True,
                Optional employeeEntitledForSpecialHolidayPay As Boolean = True,
                Optional employeeEntitledForRegularHolidayPay As Boolean = True,
                Optional employeeEntitledForRestDayPay As Boolean = True,
                Optional allowanceForOvertimePolicy As Boolean = True,
                Optional allowanceForNightDiffPolicy As Boolean = True,
                Optional allowanceForNightDiffOTPolicy As Boolean = True,
                Optional allowanceForHolidayPolicy As Boolean = True,
                Optional allowanceForRestDayPolicy As Boolean = True,
                Optional allowanceForRestDayOTPolicy As Boolean = True,
                Optional regularHours As Decimal = 1,
                Optional overtimeHours As Decimal = 1,
                Optional nightDiffHours As Decimal = 1,
                Optional nightDiffOvertimeHours As Decimal = 1,
                Optional restDayHours As Decimal = 1,
                Optional restDayOTHours As Decimal = 1,
                Optional specialHolidayHours As Decimal = 1,
                Optional specialHolidayOTHours As Decimal = 1,
                Optional regularHolidayHours As Decimal = 1,
                Optional regularHolidayOTHours As Decimal = 1,
                Optional leaveHours As Decimal = 1,
                Optional lateHours As Decimal = 1,
                Optional undertimeHours As Decimal = 1,
                Optional absentHours As Decimal = 1,
                Optional restDayNightDiffHours As Decimal = 1,
                Optional restDayNightDiffOTHours As Decimal = 1,
                Optional specialHolidayNightDiffHours As Decimal = 1,
                Optional specialHolidayNightDiffOTHours As Decimal = 1,
                Optional specialHolidayRestDayHours As Decimal = 1,
                Optional specialHolidayRestDayOTHours As Decimal = 1,
                Optional specialHolidayRestDayNightDiffHours As Decimal = 1,
                Optional specialHolidayRestDayNightDiffOTHours As Decimal = 1,
                Optional regularHolidayNightDiffHours As Decimal = 1,
                Optional regularHolidayNightDiffOTHours As Decimal = 1,
                Optional regularHolidayRestDayHours As Decimal = 1,
                Optional regularHolidayRestDayOTHours As Decimal = 1,
                Optional regularHolidayRestDayNightDiffHours As Decimal = 1,
                Optional regularHolidayRestDayNightDiffOTHours As Decimal = 1)

        If payRate Is Nothing AndAlso overridePayRate = False Then

            payRate = _overtimeRate

        End If

        _dailyBenchmarkPaystubRate.Compute(
            payRate,
            employeeEntitledForNightDifferentialPay:=employeeEntitledForNightDifferentialPay,
            employeeEntitledForSpecialHolidayPay:=employeeEntitledForSpecialHolidayPay,
            employeeEntitledForRegularHolidayPay:=employeeEntitledForRegularHolidayPay,
            employeeEntitledForRestDayPay:=employeeEntitledForRestDayPay,
            allowanceForOvertimePolicy:=allowanceForOvertimePolicy,
            allowanceForNightDiffPolicy:=allowanceForNightDiffPolicy,
            allowanceForNightDiffOTPolicy:=allowanceForNightDiffOTPolicy,
            allowanceForHolidayPolicy:=allowanceForHolidayPolicy,
            allowanceForRestDayPolicy:=allowanceForRestDayPolicy,
            allowanceForRestDayOTPolicy:=allowanceForRestDayOTPolicy,
            regularHours:=regularHours,
            overtimeHours:=overtimeHours,
            nightDiffHours:=nightDiffHours,
            nightDiffOvertimeHours:=nightDiffOvertimeHours,
            restDayHours:=restDayHours,
            restDayOTHours:=restDayOTHours,
            specialHolidayHours:=specialHolidayHours,
            specialHolidayOTHours:=specialHolidayOTHours,
            regularHolidayHours:=regularHolidayHours,
            regularHolidayOTHours:=regularHolidayOTHours,
            leaveHours:=leaveHours,
            lateHours:=lateHours,
            undertimeHours:=undertimeHours,
            absentHours:=absentHours,
            restDayNightDiffHours:=restDayNightDiffHours,
            restDayNightDiffOTHours:=restDayNightDiffOTHours,
            specialHolidayNightDiffHours:=specialHolidayNightDiffHours,
            specialHolidayNightDiffOTHours:=specialHolidayNightDiffOTHours,
            specialHolidayRestDayHours:=specialHolidayRestDayHours,
            specialHolidayRestDayOTHours:=specialHolidayRestDayOTHours,
            specialHolidayRestDayNightDiffHours:=specialHolidayRestDayNightDiffHours,
            specialHolidayRestDayNightDiffOTHours:=specialHolidayRestDayNightDiffOTHours,
            regularHolidayNightDiffHours:=regularHolidayNightDiffHours,
            regularHolidayNightDiffOTHours:=regularHolidayNightDiffOTHours,
            regularHolidayRestDayHours:=regularHolidayRestDayHours,
            regularHolidayRestDayOTHours:=regularHolidayRestDayOTHours,
            regularHolidayRestDayNightDiffHours:=regularHolidayRestDayNightDiffHours,
            regularHolidayRestDayNightDiffOTHours:=regularHolidayRestDayNightDiffOTHours)

        _monthlyBenchmarkPaystubRate.Compute(
            payRate,
            employeeEntitledForNightDifferentialPay:=employeeEntitledForNightDifferentialPay,
            employeeEntitledForSpecialHolidayPay:=employeeEntitledForSpecialHolidayPay,
            employeeEntitledForRegularHolidayPay:=employeeEntitledForRegularHolidayPay,
            employeeEntitledForRestDayPay:=employeeEntitledForRestDayPay,
            allowanceForOvertimePolicy:=allowanceForOvertimePolicy,
            allowanceForNightDiffPolicy:=allowanceForNightDiffPolicy,
            allowanceForNightDiffOTPolicy:=allowanceForNightDiffOTPolicy,
            allowanceForHolidayPolicy:=allowanceForHolidayPolicy,
            allowanceForRestDayPolicy:=allowanceForRestDayPolicy,
            allowanceForRestDayOTPolicy:=allowanceForRestDayOTPolicy,
            regularHours:=regularHours,
            overtimeHours:=overtimeHours,
            nightDiffHours:=nightDiffHours,
            nightDiffOvertimeHours:=nightDiffOvertimeHours,
            restDayHours:=restDayHours,
            restDayOTHours:=restDayOTHours,
            specialHolidayHours:=specialHolidayHours,
            specialHolidayOTHours:=specialHolidayOTHours,
            regularHolidayHours:=regularHolidayHours,
            regularHolidayOTHours:=regularHolidayOTHours,
            leaveHours:=leaveHours,
            lateHours:=lateHours,
            undertimeHours:=undertimeHours,
            absentHours:=absentHours,
            restDayNightDiffHours:=restDayNightDiffHours,
            restDayNightDiffOTHours:=restDayNightDiffOTHours,
            specialHolidayNightDiffHours:=specialHolidayNightDiffHours,
            specialHolidayNightDiffOTHours:=specialHolidayNightDiffOTHours,
            specialHolidayRestDayHours:=specialHolidayRestDayHours,
            specialHolidayRestDayOTHours:=specialHolidayRestDayOTHours,
            specialHolidayRestDayNightDiffHours:=specialHolidayRestDayNightDiffHours,
            specialHolidayRestDayNightDiffOTHours:=specialHolidayRestDayNightDiffOTHours,
            regularHolidayNightDiffHours:=regularHolidayNightDiffHours,
            regularHolidayNightDiffOTHours:=regularHolidayNightDiffOTHours,
            regularHolidayRestDayHours:=regularHolidayRestDayHours,
            regularHolidayRestDayOTHours:=regularHolidayRestDayOTHours,
            regularHolidayRestDayNightDiffHours:=regularHolidayRestDayNightDiffHours,
            regularHolidayRestDayNightDiffOTHours:=regularHolidayRestDayNightDiffOTHours,
            isHolidayInclusive:=True)
    End Sub

End Class