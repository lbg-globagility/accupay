Option Strict On

Imports AccuPay
Imports AccuPay.Benchmark
Imports AccuPay.Entity
Imports PayrollSys

<TestFixture>
Public Class BenchmarkPaystubRateTest

    Private _benchmarkPaystubRate As BenchmarkPaystubRate
    Private _overtimeRate As OvertimeRate

    Private _employee As Employee
    Private _salary As Salary

    <SetUp>
    Public Sub SetUp()

        _employee = New Employee() With {
            .RowID = 1,
            .EmployeeType = "Daily",
            .WorkDaysPerYear = 312
        }

        _salary = New Salary() With {
            .EmployeeID = 1,
            .BasicSalary = 537,
            .AllowanceSalary = 63
        }

        _benchmarkPaystubRate = New BenchmarkPaystubRate(_employee, New List(Of Salary)({_salary}))

        InitializeOvertimeRate()

    End Sub

    Private Sub InitializeOvertimeRate()
        Dim basePay As Decimal = 1
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
        Dim regularHoliday As Decimal = 2
        Dim regularHolidayOvertime As Decimal = 2.6D
        Dim regularHolidayNightDifferential As Decimal = 2.2D
        Dim regularHolidayNightDifferentialOvertime As Decimal = 2.86D
        Dim regularHolidayRestDay As Decimal = 2.6D
        Dim regularHolidayRestDayOvertime As Decimal = 3.38D
        Dim regularHolidayRestDayNightDifferential As Decimal = 2.86D
        Dim regularHolidayRestDayNightDifferentialOvertime As Decimal = 3.718D

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
                    regularHolidayRestDayNightDifferentialOvertime:=regularHolidayRestDayNightDifferentialOvertime)
    End Sub

    <Test>
    Public Sub ShouldReadEmployeeAndSalary()
        DefaultOvertimeInitialization()

        Assert.AreEqual(_employee, _benchmarkPaystubRate.Employee)
        Assert.AreEqual(_salary, _benchmarkPaystubRate.Salary)

        Assert.AreEqual(False, _benchmarkPaystubRate.IsInvalid)

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

        Assert.AreEqual(dailyRate, _benchmarkPaystubRate.DailyRate)
        Assert.AreEqual(monthlyRate, _benchmarkPaystubRate.MonthlyRate)
        Assert.AreEqual(hourlyRate, _benchmarkPaystubRate.HourlyRate)
        Assert.AreEqual(actualDailyRate, _benchmarkPaystubRate.ActualDailyRate)
        Assert.AreEqual(actualMonthlyRate, _benchmarkPaystubRate.ActualMonthlyRate)
        Assert.AreEqual(actualHourlyRate, _benchmarkPaystubRate.ActualHourlyRate)

        Assert.AreEqual(_salary.AllowanceSalary, _benchmarkPaystubRate.AllowanceSalary)

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

        _benchmarkPaystubRate.Compute(
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

        Assert.AreEqual(regularHours, _benchmarkPaystubRate.RegularHours)
        Assert.AreEqual(overtimeHours, _benchmarkPaystubRate.OvertimeHours)

        Assert.AreEqual(nightDiffHours, _benchmarkPaystubRate.NightDiffHours)
        Assert.AreEqual(nightDiffOvertimeHours, _benchmarkPaystubRate.NightDiffOvertimeHours)
        Assert.AreEqual(restDayHours, _benchmarkPaystubRate.RestDayHours)
        Assert.AreEqual(restDayOTHours, _benchmarkPaystubRate.RestDayOTHours)
        Assert.AreEqual(specialHolidayHours, _benchmarkPaystubRate.SpecialHolidayHours)
        Assert.AreEqual(specialHolidayOTHours, _benchmarkPaystubRate.SpecialHolidayOTHours)
        Assert.AreEqual(regularHolidayHours, _benchmarkPaystubRate.RegularHolidayHours)
        Assert.AreEqual(regularHolidayOTHours, _benchmarkPaystubRate.RegularHolidayOTHours)

        Assert.AreEqual(leaveHours, _benchmarkPaystubRate.LeaveHours)
        Assert.AreEqual(lateHours, _benchmarkPaystubRate.LateHours)
        Assert.AreEqual(undertimeHours, _benchmarkPaystubRate.UndertimeHours)
        Assert.AreEqual(absentHours, _benchmarkPaystubRate.AbsentHours)

        Assert.AreEqual(restDayNightDiffHours, _benchmarkPaystubRate.RestDayNightDiffHours)
        Assert.AreEqual(restDayNightDiffOTHours, _benchmarkPaystubRate.RestDayNightDiffOTHours)
        Assert.AreEqual(specialHolidayNightDiffHours, _benchmarkPaystubRate.SpecialHolidayNightDiffHours)
        Assert.AreEqual(specialHolidayNightDiffOTHours, _benchmarkPaystubRate.SpecialHolidayNightDiffOTHours)
        Assert.AreEqual(specialHolidayRestDayHours, _benchmarkPaystubRate.SpecialHolidayRestDayHours)
        Assert.AreEqual(specialHolidayRestDayOTHours, _benchmarkPaystubRate.SpecialHolidayRestDayOTHours)
        Assert.AreEqual(specialHolidayRestDayNightDiffHours, _benchmarkPaystubRate.SpecialHolidayRestDayNightDiffHours)
        Assert.AreEqual(specialHolidayRestDayNightDiffOTHours, _benchmarkPaystubRate.SpecialHolidayRestDayNightDiffOTHours)
        Assert.AreEqual(regularHolidayNightDiffHours, _benchmarkPaystubRate.RegularHolidayNightDiffHours)
        Assert.AreEqual(regularHolidayNightDiffOTHours, _benchmarkPaystubRate.RegularHolidayNightDiffOTHours)
        Assert.AreEqual(regularHolidayRestDayHours, _benchmarkPaystubRate.RegularHolidayRestDayHours)
        Assert.AreEqual(regularHolidayRestDayOTHours, _benchmarkPaystubRate.RegularHolidayRestDayOTHours)
        Assert.AreEqual(regularHolidayRestDayNightDiffHours, _benchmarkPaystubRate.RegularHolidayRestDayNightDiffHours)
        Assert.AreEqual(regularHolidayRestDayNightDiffOTHours, _benchmarkPaystubRate.RegularHolidayRestDayNightDiffOTHours)

    End Sub

    <Test>
    Public Sub ShouldComputeRegularPays()
        DefaultOvertimeInitialization()

        Dim hourlyRate = 67.125
        Dim actualHourlyRate = 75

        Dim overtimeRate = 1.25

        Assert.AreEqual(_benchmarkPaystubRate.RegularPay, AccuMath.CommercialRound(_benchmarkPaystubRate.RegularHours * hourlyRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualRegularPay, AccuMath.CommercialRound(_benchmarkPaystubRate.RegularHours * actualHourlyRate))

        Assert.AreEqual(_benchmarkPaystubRate.OvertimePay, AccuMath.CommercialRound(_benchmarkPaystubRate.OvertimeHours * hourlyRate * overtimeRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualOvertimePay, AccuMath.CommercialRound(_benchmarkPaystubRate.OvertimeHours * actualHourlyRate * overtimeRate))

        Assert.AreEqual(_benchmarkPaystubRate.LeavePay, AccuMath.CommercialRound(_benchmarkPaystubRate.LeaveHours * hourlyRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualLeavePay, AccuMath.CommercialRound(_benchmarkPaystubRate.LeaveHours * actualHourlyRate))

    End Sub

    <Test>
    Public Sub ShouldComputeRegularPays_WithFalseAllowanceForOvertimePolicy()
        DefaultOvertimeInitialization(allowanceForOvertimePolicy:=False)

        Dim hourlyRate = 67.125
        Dim actualHourlyRate = 75

        Dim overtimeRate = 1.25

        Assert.AreEqual(_benchmarkPaystubRate.RegularPay, AccuMath.CommercialRound(_benchmarkPaystubRate.RegularHours * hourlyRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualRegularPay, AccuMath.CommercialRound(_benchmarkPaystubRate.RegularHours * actualHourlyRate))

        Assert.AreEqual(_benchmarkPaystubRate.OvertimePay, AccuMath.CommercialRound(_benchmarkPaystubRate.OvertimeHours * hourlyRate * overtimeRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualOvertimePay, AccuMath.CommercialRound(_benchmarkPaystubRate.OvertimePay))

        Assert.AreEqual(_benchmarkPaystubRate.LeavePay, AccuMath.CommercialRound(_benchmarkPaystubRate.LeaveHours * hourlyRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualLeavePay, AccuMath.CommercialRound(_benchmarkPaystubRate.LeaveHours * actualHourlyRate))

    End Sub

    <Test>
    Public Sub ShouldComputeDeductions()
        DefaultOvertimeInitialization()

        Dim hourlyRate = 67.125
        Dim actualHourlyRate = 75

        Assert.AreEqual(_benchmarkPaystubRate.LateDeduction, AccuMath.CommercialRound(_benchmarkPaystubRate.LateHours * hourlyRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualLateDeduction, AccuMath.CommercialRound(_benchmarkPaystubRate.LateHours * actualHourlyRate))

        Assert.AreEqual(_benchmarkPaystubRate.UndertimeDeduction, AccuMath.CommercialRound(_benchmarkPaystubRate.UndertimeHours * hourlyRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualUndertimeDeduction, AccuMath.CommercialRound(_benchmarkPaystubRate.UndertimeHours * actualHourlyRate))

        Assert.AreEqual(_benchmarkPaystubRate.AbsenceDeduction, AccuMath.CommercialRound(_benchmarkPaystubRate.AbsentHours * hourlyRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualAbsenceDeduction, AccuMath.CommercialRound(_benchmarkPaystubRate.AbsentHours * actualHourlyRate))

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

        Assert.AreEqual(_benchmarkPaystubRate.NightDiffPay, AccuMath.CommercialRound(hourlyRate * nightDifferentialRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualNightDiffPay, AccuMath.CommercialRound(actualHourlyRate * nightDifferentialRate))

        Assert.AreEqual(_benchmarkPaystubRate.RestDayNightDiffPay, AccuMath.CommercialRound(hourlyRate * restDayRate * nightDifferentialRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualRestDayNightDiffPay, AccuMath.CommercialRound(actualHourlyRate * restDayRate * nightDifferentialRate))

        Assert.AreEqual(_benchmarkPaystubRate.SpecialHolidayNightDiffPay, AccuMath.CommercialRound(hourlyRate * specialHolidayRate * nightDifferentialRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualSpecialHolidayNightDiffPay, AccuMath.CommercialRound(actualHourlyRate * specialHolidayRate * nightDifferentialRate))

        Assert.AreEqual(_benchmarkPaystubRate.SpecialHolidayRestDayNightDiffPay, AccuMath.CommercialRound(hourlyRate * specialHolidayRestDayRate * nightDifferentialRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualSpecialHolidayRestDayNightDiffPay, AccuMath.CommercialRound(actualHourlyRate * specialHolidayRestDayRate * nightDifferentialRate))

        Assert.AreEqual(_benchmarkPaystubRate.RegularHolidayNightDiffPay, AccuMath.CommercialRound(hourlyRate * regularHolidayRate * nightDifferentialRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualRegularHolidayNightDiffPay, AccuMath.CommercialRound(actualHourlyRate * regularHolidayRate * nightDifferentialRate))

        Assert.AreEqual(_benchmarkPaystubRate.RegularHolidayRestDayNightDiffPay, AccuMath.CommercialRound(hourlyRate * regularHolidayRestDayRate * nightDifferentialRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualRegularHolidayRestDayNightDiffPay, AccuMath.CommercialRound(actualHourlyRate * regularHolidayRestDayRate * nightDifferentialRate))

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

        Assert.AreEqual(_benchmarkPaystubRate.NightDiffPay, AccuMath.CommercialRound(hourlyRate * nightDifferentialRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualNightDiffPay, AccuMath.CommercialRound(_benchmarkPaystubRate.NightDiffPay))

        Assert.AreEqual(_benchmarkPaystubRate.RestDayNightDiffPay, AccuMath.CommercialRound(hourlyRate * restDayRate * nightDifferentialRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualRestDayNightDiffPay, AccuMath.CommercialRound(_benchmarkPaystubRate.RestDayNightDiffPay))

        Assert.AreEqual(_benchmarkPaystubRate.SpecialHolidayNightDiffPay, AccuMath.CommercialRound(hourlyRate * specialHolidayRate * nightDifferentialRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualSpecialHolidayNightDiffPay, AccuMath.CommercialRound(_benchmarkPaystubRate.SpecialHolidayNightDiffPay))

        Assert.AreEqual(_benchmarkPaystubRate.SpecialHolidayRestDayNightDiffPay, AccuMath.CommercialRound(hourlyRate * specialHolidayRestDayRate * nightDifferentialRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualSpecialHolidayRestDayNightDiffPay, AccuMath.CommercialRound(_benchmarkPaystubRate.SpecialHolidayRestDayNightDiffPay))

        Assert.AreEqual(_benchmarkPaystubRate.RegularHolidayNightDiffPay, AccuMath.CommercialRound(hourlyRate * regularHolidayRate * nightDifferentialRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualRegularHolidayNightDiffPay, AccuMath.CommercialRound(_benchmarkPaystubRate.RegularHolidayNightDiffPay))

        Assert.AreEqual(_benchmarkPaystubRate.RegularHolidayRestDayNightDiffPay, AccuMath.CommercialRound(hourlyRate * regularHolidayRestDayRate * nightDifferentialRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualRegularHolidayRestDayNightDiffPay, AccuMath.CommercialRound(_benchmarkPaystubRate.RegularHolidayRestDayNightDiffPay))

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

        Assert.AreEqual(_benchmarkPaystubRate.NightDiffOvertimePay, AccuMath.CommercialRound(_benchmarkPaystubRate.NightDiffOvertimeHours * hourlyRate * overtimeRate * nightDifferentialRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualNightDiffOvertimePay, AccuMath.CommercialRound(_benchmarkPaystubRate.NightDiffOvertimeHours * actualHourlyRate * overtimeRate * nightDifferentialRate))

        Assert.AreEqual(_benchmarkPaystubRate.RestDayNightDiffOTPay, AccuMath.CommercialRound(_benchmarkPaystubRate.RestDayNightDiffOTHours * hourlyRate * restDayOvertimeRate * nightDifferentialRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualRestDayNightDiffOTPay, AccuMath.CommercialRound(_benchmarkPaystubRate.RestDayNightDiffOTHours * actualHourlyRate * restDayOvertimeRate * nightDifferentialRate))

        Assert.AreEqual(_benchmarkPaystubRate.SpecialHolidayNightDiffOTPay, AccuMath.CommercialRound(_benchmarkPaystubRate.SpecialHolidayNightDiffOTHours * hourlyRate * specialHolidayOvertimeRate * nightDifferentialRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualSpecialHolidayNightDiffOTPay, AccuMath.CommercialRound(_benchmarkPaystubRate.SpecialHolidayNightDiffOTHours * actualHourlyRate * specialHolidayOvertimeRate * nightDifferentialRate))

        Assert.AreEqual(_benchmarkPaystubRate.SpecialHolidayRestDayNightDiffOTPay, AccuMath.CommercialRound(_benchmarkPaystubRate.SpecialHolidayRestDayNightDiffOTHours * hourlyRate * specialHolidayRestDayOvertimeRate * nightDifferentialRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualSpecialHolidayRestDayNightDiffOTPay, AccuMath.CommercialRound(_benchmarkPaystubRate.SpecialHolidayRestDayNightDiffOTHours * actualHourlyRate * specialHolidayRestDayOvertimeRate * nightDifferentialRate))

        Assert.AreEqual(_benchmarkPaystubRate.RegularHolidayNightDiffOTPay, AccuMath.CommercialRound(_benchmarkPaystubRate.RegularHolidayNightDiffOTHours * hourlyRate * regularHolidayOvertimeRate * nightDifferentialRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualRegularHolidayNightDiffOTPay, AccuMath.CommercialRound(_benchmarkPaystubRate.RegularHolidayNightDiffOTHours * actualHourlyRate * regularHolidayOvertimeRate * nightDifferentialRate))

        Assert.AreEqual(_benchmarkPaystubRate.RegularHolidayRestDayNightDiffOTPay, AccuMath.CommercialRound(_benchmarkPaystubRate.RegularHolidayRestDayNightDiffOTHours * hourlyRate * regularHolidayRestDayOvertimeRate * nightDifferentialRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualRegularHolidayRestDayNightDiffOTPay, AccuMath.CommercialRound(_benchmarkPaystubRate.RegularHolidayRestDayNightDiffOTHours * actualHourlyRate * regularHolidayRestDayOvertimeRate * nightDifferentialRate))

    End Sub

    <Test>
    Public Sub ShouldComputeNightDifferentialOvertimePays_WithFalseEmployeeEntitledForNightDifferentialPay()
        DefaultOvertimeInitialization(employeeEntitledForNightDifferentialPay:=False)

        Assert.AreEqual(_benchmarkPaystubRate.NightDiffOvertimePay, 0)
        Assert.AreEqual(_benchmarkPaystubRate.ActualNightDiffOvertimePay, 0)

        Assert.AreEqual(_benchmarkPaystubRate.RestDayNightDiffOTPay, 0)
        Assert.AreEqual(_benchmarkPaystubRate.ActualRestDayNightDiffOTPay, 0)

        Assert.AreEqual(_benchmarkPaystubRate.SpecialHolidayNightDiffOTPay, 0)
        Assert.AreEqual(_benchmarkPaystubRate.ActualSpecialHolidayNightDiffOTPay, 0)

        Assert.AreEqual(_benchmarkPaystubRate.SpecialHolidayRestDayNightDiffOTPay, 0)
        Assert.AreEqual(_benchmarkPaystubRate.ActualSpecialHolidayRestDayNightDiffOTPay, 0)

        Assert.AreEqual(_benchmarkPaystubRate.RegularHolidayNightDiffOTPay, 0)
        Assert.AreEqual(_benchmarkPaystubRate.ActualRegularHolidayNightDiffOTPay, 0)

        Assert.AreEqual(_benchmarkPaystubRate.RegularHolidayRestDayNightDiffOTPay, 0)
        Assert.AreEqual(_benchmarkPaystubRate.ActualRegularHolidayRestDayNightDiffOTPay, 0)

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

        Assert.AreEqual(_benchmarkPaystubRate.NightDiffOvertimePay, AccuMath.CommercialRound(_benchmarkPaystubRate.NightDiffOvertimeHours * hourlyRate * overtimeRate * nightDifferentialRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualNightDiffOvertimePay, _benchmarkPaystubRate.NightDiffOvertimePay)

        Assert.AreEqual(_benchmarkPaystubRate.RestDayNightDiffOTPay, AccuMath.CommercialRound(_benchmarkPaystubRate.RestDayNightDiffOTHours * hourlyRate * restDayOvertimeRate * nightDifferentialRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualRestDayNightDiffOTPay, _benchmarkPaystubRate.RestDayNightDiffOTPay)

        Assert.AreEqual(_benchmarkPaystubRate.SpecialHolidayNightDiffOTPay, AccuMath.CommercialRound(_benchmarkPaystubRate.SpecialHolidayNightDiffOTHours * hourlyRate * specialHolidayOvertimeRate * nightDifferentialRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualSpecialHolidayNightDiffOTPay, _benchmarkPaystubRate.SpecialHolidayNightDiffOTPay)

        Assert.AreEqual(_benchmarkPaystubRate.SpecialHolidayRestDayNightDiffOTPay, AccuMath.CommercialRound(_benchmarkPaystubRate.SpecialHolidayRestDayNightDiffOTHours * hourlyRate * specialHolidayRestDayOvertimeRate * nightDifferentialRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualSpecialHolidayRestDayNightDiffOTPay, _benchmarkPaystubRate.SpecialHolidayRestDayNightDiffOTPay)

        Assert.AreEqual(_benchmarkPaystubRate.RegularHolidayNightDiffOTPay, AccuMath.CommercialRound(_benchmarkPaystubRate.RegularHolidayNightDiffOTHours * hourlyRate * regularHolidayOvertimeRate * nightDifferentialRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualRegularHolidayNightDiffOTPay, _benchmarkPaystubRate.RegularHolidayNightDiffOTPay)

        Assert.AreEqual(_benchmarkPaystubRate.RegularHolidayRestDayNightDiffOTPay, AccuMath.CommercialRound(_benchmarkPaystubRate.RegularHolidayRestDayNightDiffOTHours * hourlyRate * regularHolidayRestDayOvertimeRate * nightDifferentialRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualRegularHolidayRestDayNightDiffOTPay, _benchmarkPaystubRate.RegularHolidayRestDayNightDiffOTPay)

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

        Assert.AreEqual(_benchmarkPaystubRate.SpecialHolidayPay, AccuMath.CommercialRound(_benchmarkPaystubRate.SpecialHolidayHours * hourlyRate * specialHolidayRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualSpecialHolidayPay, AccuMath.CommercialRound(_benchmarkPaystubRate.SpecialHolidayHours * actualHourlyRate * specialHolidayRate))

        Assert.AreEqual(_benchmarkPaystubRate.SpecialHolidayOTPay, AccuMath.CommercialRound(_benchmarkPaystubRate.SpecialHolidayOTHours * hourlyRate * specialHolidayOvertimeRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualSpecialHolidayOTPay, AccuMath.CommercialRound(_benchmarkPaystubRate.SpecialHolidayOTHours * actualHourlyRate * specialHolidayOvertimeRate))

        Assert.AreEqual(_benchmarkPaystubRate.SpecialHolidayRestDayPay, AccuMath.CommercialRound(_benchmarkPaystubRate.SpecialHolidayRestDayHours * hourlyRate * specialHolidayRestDayRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualSpecialHolidayRestDayPay, AccuMath.CommercialRound(_benchmarkPaystubRate.SpecialHolidayRestDayHours * actualHourlyRate * specialHolidayRestDayRate))

        Assert.AreEqual(_benchmarkPaystubRate.SpecialHolidayRestDayOTPay, AccuMath.CommercialRound(_benchmarkPaystubRate.SpecialHolidayRestDayOTHours * hourlyRate * specialHolidayRestDayOvertimeRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualSpecialHolidayRestDayOTPay, AccuMath.CommercialRound(_benchmarkPaystubRate.SpecialHolidayRestDayOTHours * actualHourlyRate * specialHolidayRestDayOvertimeRate))

        Assert.AreEqual(_benchmarkPaystubRate.RegularHolidayPay, AccuMath.CommercialRound(_benchmarkPaystubRate.RegularHolidayHours * hourlyRate * regularHolidayRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualRegularHolidayPay, AccuMath.CommercialRound(_benchmarkPaystubRate.RegularHolidayHours * actualHourlyRate * regularHolidayRate))

        Assert.AreEqual(_benchmarkPaystubRate.RegularHolidayOTPay, AccuMath.CommercialRound(_benchmarkPaystubRate.RegularHolidayOTHours * hourlyRate * regularHolidayOvertimeRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualRegularHolidayOTPay, AccuMath.CommercialRound(_benchmarkPaystubRate.RegularHolidayOTHours * actualHourlyRate * regularHolidayOvertimeRate))

        Assert.AreEqual(_benchmarkPaystubRate.RegularHolidayRestDayPay, AccuMath.CommercialRound(_benchmarkPaystubRate.RegularHolidayRestDayHours * hourlyRate * regularHolidayRestDayRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualRegularHolidayRestDayPay, AccuMath.CommercialRound(_benchmarkPaystubRate.RegularHolidayRestDayHours * actualHourlyRate * regularHolidayRestDayRate))

        Assert.AreEqual(_benchmarkPaystubRate.RegularHolidayRestDayOTPay, AccuMath.CommercialRound(_benchmarkPaystubRate.RegularHolidayRestDayOTHours * hourlyRate * regularHolidayRestDayOvertimeRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualRegularHolidayRestDayOTPay, AccuMath.CommercialRound(_benchmarkPaystubRate.RegularHolidayRestDayOTHours * actualHourlyRate * regularHolidayRestDayOvertimeRate))
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

        Assert.AreEqual(_benchmarkPaystubRate.SpecialHolidayPay, AccuMath.CommercialRound(_benchmarkPaystubRate.SpecialHolidayHours * hourlyRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualSpecialHolidayPay, AccuMath.CommercialRound(_benchmarkPaystubRate.SpecialHolidayHours * actualHourlyRate))

        Assert.AreEqual(_benchmarkPaystubRate.SpecialHolidayOTPay, AccuMath.CommercialRound(_benchmarkPaystubRate.SpecialHolidayOTHours * hourlyRate * overtimeRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualSpecialHolidayOTPay, AccuMath.CommercialRound(_benchmarkPaystubRate.SpecialHolidayOTHours * actualHourlyRate * overtimeRate))

        Assert.AreEqual(_benchmarkPaystubRate.SpecialHolidayRestDayPay, AccuMath.CommercialRound(_benchmarkPaystubRate.SpecialHolidayRestDayHours * hourlyRate * restDayRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualSpecialHolidayRestDayPay, AccuMath.CommercialRound(_benchmarkPaystubRate.SpecialHolidayRestDayHours * actualHourlyRate * restDayRate))

        Assert.AreEqual(_benchmarkPaystubRate.SpecialHolidayRestDayOTPay, AccuMath.CommercialRound(_benchmarkPaystubRate.SpecialHolidayRestDayOTHours * hourlyRate * restDayOvertimeRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualSpecialHolidayRestDayOTPay, AccuMath.CommercialRound(_benchmarkPaystubRate.SpecialHolidayRestDayOTHours * actualHourlyRate * restDayOvertimeRate))

        Assert.AreEqual(_benchmarkPaystubRate.RegularHolidayPay, AccuMath.CommercialRound(_benchmarkPaystubRate.RegularHolidayHours * hourlyRate * regularHolidayRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualRegularHolidayPay, AccuMath.CommercialRound(_benchmarkPaystubRate.RegularHolidayHours * actualHourlyRate * regularHolidayRate))

        Assert.AreEqual(_benchmarkPaystubRate.RegularHolidayOTPay, AccuMath.CommercialRound(_benchmarkPaystubRate.RegularHolidayOTHours * hourlyRate * regularHolidayOvertimeRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualRegularHolidayOTPay, AccuMath.CommercialRound(_benchmarkPaystubRate.RegularHolidayOTHours * actualHourlyRate * regularHolidayOvertimeRate))

        Assert.AreEqual(_benchmarkPaystubRate.RegularHolidayRestDayPay, AccuMath.CommercialRound(_benchmarkPaystubRate.RegularHolidayRestDayHours * hourlyRate * regularHolidayRestDayRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualRegularHolidayRestDayPay, AccuMath.CommercialRound(_benchmarkPaystubRate.RegularHolidayRestDayHours * actualHourlyRate * regularHolidayRestDayRate))

        Assert.AreEqual(_benchmarkPaystubRate.RegularHolidayRestDayOTPay, AccuMath.CommercialRound(_benchmarkPaystubRate.RegularHolidayRestDayOTHours * hourlyRate * regularHolidayRestDayOvertimeRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualRegularHolidayRestDayOTPay, AccuMath.CommercialRound(_benchmarkPaystubRate.RegularHolidayRestDayOTHours * actualHourlyRate * regularHolidayRestDayOvertimeRate))
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

        Assert.AreEqual(_benchmarkPaystubRate.SpecialHolidayPay, AccuMath.CommercialRound(_benchmarkPaystubRate.SpecialHolidayHours * hourlyRate * specialHolidayRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualSpecialHolidayPay, AccuMath.CommercialRound(_benchmarkPaystubRate.SpecialHolidayHours * actualHourlyRate * specialHolidayRate))

        Assert.AreEqual(_benchmarkPaystubRate.SpecialHolidayOTPay, AccuMath.CommercialRound(_benchmarkPaystubRate.SpecialHolidayOTHours * hourlyRate * specialHolidayOvertimeRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualSpecialHolidayOTPay, AccuMath.CommercialRound(_benchmarkPaystubRate.SpecialHolidayOTHours * actualHourlyRate * specialHolidayOvertimeRate))

        Assert.AreEqual(_benchmarkPaystubRate.SpecialHolidayRestDayPay, AccuMath.CommercialRound(_benchmarkPaystubRate.SpecialHolidayRestDayHours * hourlyRate * specialHolidayRestDayRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualSpecialHolidayRestDayPay, AccuMath.CommercialRound(_benchmarkPaystubRate.SpecialHolidayRestDayHours * actualHourlyRate * specialHolidayRestDayRate))

        Assert.AreEqual(_benchmarkPaystubRate.SpecialHolidayRestDayOTPay, AccuMath.CommercialRound(_benchmarkPaystubRate.SpecialHolidayRestDayOTHours * hourlyRate * specialHolidayRestDayOvertimeRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualSpecialHolidayRestDayOTPay, AccuMath.CommercialRound(_benchmarkPaystubRate.SpecialHolidayRestDayOTHours * actualHourlyRate * specialHolidayRestDayOvertimeRate))

        Assert.AreEqual(_benchmarkPaystubRate.RegularHolidayPay, AccuMath.CommercialRound(_benchmarkPaystubRate.RegularHolidayHours * hourlyRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualRegularHolidayPay, AccuMath.CommercialRound(_benchmarkPaystubRate.RegularHolidayHours * actualHourlyRate))

        Assert.AreEqual(_benchmarkPaystubRate.RegularHolidayOTPay, AccuMath.CommercialRound(_benchmarkPaystubRate.RegularHolidayOTHours * hourlyRate * overtimeRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualRegularHolidayOTPay, AccuMath.CommercialRound(_benchmarkPaystubRate.RegularHolidayOTHours * actualHourlyRate * overtimeRate))

        Assert.AreEqual(_benchmarkPaystubRate.RegularHolidayRestDayPay, AccuMath.CommercialRound(_benchmarkPaystubRate.RegularHolidayRestDayHours * hourlyRate * restDayRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualRegularHolidayRestDayPay, AccuMath.CommercialRound(_benchmarkPaystubRate.RegularHolidayRestDayHours * actualHourlyRate * restDayRate))

        Assert.AreEqual(_benchmarkPaystubRate.RegularHolidayRestDayOTPay, AccuMath.CommercialRound(_benchmarkPaystubRate.RegularHolidayRestDayOTHours * hourlyRate * restDayOvertimeRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualRegularHolidayRestDayOTPay, AccuMath.CommercialRound(_benchmarkPaystubRate.RegularHolidayRestDayOTHours * actualHourlyRate * restDayOvertimeRate))
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

        Assert.AreEqual(_benchmarkPaystubRate.SpecialHolidayPay, AccuMath.CommercialRound(_benchmarkPaystubRate.SpecialHolidayHours * hourlyRate * specialHolidayRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualSpecialHolidayPay, _benchmarkPaystubRate.SpecialHolidayPay)

        Assert.AreEqual(_benchmarkPaystubRate.SpecialHolidayOTPay, AccuMath.CommercialRound(_benchmarkPaystubRate.SpecialHolidayOTHours * hourlyRate * specialHolidayOvertimeRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualSpecialHolidayOTPay, _benchmarkPaystubRate.SpecialHolidayOTPay)

        Assert.AreEqual(_benchmarkPaystubRate.SpecialHolidayRestDayPay, AccuMath.CommercialRound(_benchmarkPaystubRate.SpecialHolidayRestDayHours * hourlyRate * specialHolidayRestDayRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualSpecialHolidayRestDayPay, _benchmarkPaystubRate.SpecialHolidayRestDayPay)

        Assert.AreEqual(_benchmarkPaystubRate.SpecialHolidayRestDayOTPay, AccuMath.CommercialRound(_benchmarkPaystubRate.SpecialHolidayRestDayOTHours * hourlyRate * specialHolidayRestDayOvertimeRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualSpecialHolidayRestDayOTPay, _benchmarkPaystubRate.SpecialHolidayRestDayOTPay)

        Assert.AreEqual(_benchmarkPaystubRate.RegularHolidayPay, AccuMath.CommercialRound(_benchmarkPaystubRate.RegularHolidayHours * hourlyRate * regularHolidayRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualRegularHolidayPay, _benchmarkPaystubRate.RegularHolidayPay)

        Assert.AreEqual(_benchmarkPaystubRate.RegularHolidayOTPay, AccuMath.CommercialRound(_benchmarkPaystubRate.RegularHolidayOTHours * hourlyRate * regularHolidayOvertimeRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualRegularHolidayOTPay, _benchmarkPaystubRate.RegularHolidayOTPay)

        Assert.AreEqual(_benchmarkPaystubRate.RegularHolidayRestDayPay, AccuMath.CommercialRound(_benchmarkPaystubRate.RegularHolidayRestDayHours * hourlyRate * regularHolidayRestDayRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualRegularHolidayRestDayPay, _benchmarkPaystubRate.RegularHolidayRestDayPay)

        Assert.AreEqual(_benchmarkPaystubRate.RegularHolidayRestDayOTPay, AccuMath.CommercialRound(_benchmarkPaystubRate.RegularHolidayRestDayOTHours * hourlyRate * regularHolidayRestDayOvertimeRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualRegularHolidayRestDayOTPay, _benchmarkPaystubRate.RegularHolidayRestDayOTPay)
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

        Assert.AreEqual(_benchmarkPaystubRate.SpecialHolidayPay, AccuMath.CommercialRound(_benchmarkPaystubRate.SpecialHolidayHours * hourlyRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualSpecialHolidayPay, AccuMath.CommercialRound(_benchmarkPaystubRate.SpecialHolidayHours * actualHourlyRate))

        Assert.AreEqual(_benchmarkPaystubRate.SpecialHolidayOTPay, AccuMath.CommercialRound(_benchmarkPaystubRate.SpecialHolidayOTHours * hourlyRate * overtimeRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualSpecialHolidayOTPay, AccuMath.CommercialRound(_benchmarkPaystubRate.SpecialHolidayOTHours * actualHourlyRate * overtimeRate))

        Assert.AreEqual(_benchmarkPaystubRate.SpecialHolidayRestDayPay, AccuMath.CommercialRound(_benchmarkPaystubRate.SpecialHolidayRestDayHours * hourlyRate * restDayRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualSpecialHolidayRestDayPay, AccuMath.CommercialRound(_benchmarkPaystubRate.SpecialHolidayRestDayHours * actualHourlyRate * restDayRate))

        Assert.AreEqual(_benchmarkPaystubRate.SpecialHolidayRestDayOTPay, AccuMath.CommercialRound(_benchmarkPaystubRate.SpecialHolidayRestDayOTHours * hourlyRate * restDayOvertimeRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualSpecialHolidayRestDayOTPay, AccuMath.CommercialRound(_benchmarkPaystubRate.SpecialHolidayRestDayOTHours * actualHourlyRate * restDayOvertimeRate))

        Assert.AreEqual(_benchmarkPaystubRate.RegularHolidayPay, AccuMath.CommercialRound(_benchmarkPaystubRate.RegularHolidayHours * hourlyRate * regularHolidayRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualRegularHolidayPay, _benchmarkPaystubRate.RegularHolidayPay)

        Assert.AreEqual(_benchmarkPaystubRate.RegularHolidayOTPay, AccuMath.CommercialRound(_benchmarkPaystubRate.RegularHolidayOTHours * hourlyRate * regularHolidayOvertimeRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualRegularHolidayOTPay, _benchmarkPaystubRate.RegularHolidayOTPay)

        Assert.AreEqual(_benchmarkPaystubRate.RegularHolidayRestDayPay, AccuMath.CommercialRound(_benchmarkPaystubRate.RegularHolidayRestDayHours * hourlyRate * regularHolidayRestDayRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualRegularHolidayRestDayPay, _benchmarkPaystubRate.RegularHolidayRestDayPay)

        Assert.AreEqual(_benchmarkPaystubRate.RegularHolidayRestDayOTPay, AccuMath.CommercialRound(_benchmarkPaystubRate.RegularHolidayRestDayOTHours * hourlyRate * regularHolidayRestDayOvertimeRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualRegularHolidayRestDayOTPay, _benchmarkPaystubRate.RegularHolidayRestDayOTPay)
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

        Assert.AreEqual(_benchmarkPaystubRate.SpecialHolidayPay, AccuMath.CommercialRound(_benchmarkPaystubRate.SpecialHolidayHours * hourlyRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualSpecialHolidayPay, AccuMath.CommercialRound(_benchmarkPaystubRate.SpecialHolidayHours * actualHourlyRate))

        Assert.AreEqual(_benchmarkPaystubRate.SpecialHolidayOTPay, AccuMath.CommercialRound(_benchmarkPaystubRate.SpecialHolidayOTHours * hourlyRate * overtimeRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualSpecialHolidayOTPay, _benchmarkPaystubRate.SpecialHolidayOTPay)

        Assert.AreEqual(_benchmarkPaystubRate.SpecialHolidayRestDayPay, AccuMath.CommercialRound(_benchmarkPaystubRate.SpecialHolidayRestDayHours * hourlyRate * restDayRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualSpecialHolidayRestDayPay, _benchmarkPaystubRate.SpecialHolidayRestDayPay)

        Assert.AreEqual(_benchmarkPaystubRate.SpecialHolidayRestDayOTPay, AccuMath.CommercialRound(_benchmarkPaystubRate.SpecialHolidayRestDayOTHours * hourlyRate * restDayOvertimeRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualSpecialHolidayRestDayOTPay, _benchmarkPaystubRate.SpecialHolidayRestDayOTPay)

        Assert.AreEqual(_benchmarkPaystubRate.RegularHolidayPay, AccuMath.CommercialRound(_benchmarkPaystubRate.RegularHolidayHours * hourlyRate * regularHolidayRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualRegularHolidayPay, _benchmarkPaystubRate.RegularHolidayPay)

        Assert.AreEqual(_benchmarkPaystubRate.RegularHolidayOTPay, AccuMath.CommercialRound(_benchmarkPaystubRate.RegularHolidayOTHours * hourlyRate * regularHolidayOvertimeRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualRegularHolidayOTPay, _benchmarkPaystubRate.RegularHolidayOTPay)

        Assert.AreEqual(_benchmarkPaystubRate.RegularHolidayRestDayPay, AccuMath.CommercialRound(_benchmarkPaystubRate.RegularHolidayRestDayHours * hourlyRate * regularHolidayRestDayRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualRegularHolidayRestDayPay, _benchmarkPaystubRate.RegularHolidayRestDayPay)

        Assert.AreEqual(_benchmarkPaystubRate.RegularHolidayRestDayOTPay, AccuMath.CommercialRound(_benchmarkPaystubRate.RegularHolidayRestDayOTHours * hourlyRate * regularHolidayRestDayOvertimeRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualRegularHolidayRestDayOTPay, _benchmarkPaystubRate.RegularHolidayRestDayOTPay)
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

        Assert.AreEqual(_benchmarkPaystubRate.SpecialHolidayPay, AccuMath.CommercialRound(_benchmarkPaystubRate.SpecialHolidayHours * hourlyRate * specialHolidayRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualSpecialHolidayPay, _benchmarkPaystubRate.SpecialHolidayPay)

        Assert.AreEqual(_benchmarkPaystubRate.SpecialHolidayOTPay, AccuMath.CommercialRound(_benchmarkPaystubRate.SpecialHolidayOTHours * hourlyRate * specialHolidayOvertimeRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualSpecialHolidayOTPay, _benchmarkPaystubRate.SpecialHolidayOTPay)

        Assert.AreEqual(_benchmarkPaystubRate.SpecialHolidayRestDayPay, AccuMath.CommercialRound(_benchmarkPaystubRate.SpecialHolidayRestDayHours * hourlyRate * specialHolidayRestDayRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualSpecialHolidayRestDayPay, _benchmarkPaystubRate.SpecialHolidayRestDayPay)

        Assert.AreEqual(_benchmarkPaystubRate.SpecialHolidayRestDayOTPay, AccuMath.CommercialRound(_benchmarkPaystubRate.SpecialHolidayRestDayOTHours * hourlyRate * specialHolidayRestDayOvertimeRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualSpecialHolidayRestDayOTPay, _benchmarkPaystubRate.SpecialHolidayRestDayOTPay)

        Assert.AreEqual(_benchmarkPaystubRate.RegularHolidayPay, AccuMath.CommercialRound(_benchmarkPaystubRate.RegularHolidayHours * hourlyRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualRegularHolidayPay, AccuMath.CommercialRound(_benchmarkPaystubRate.RegularHolidayHours * actualHourlyRate))

        Assert.AreEqual(_benchmarkPaystubRate.RegularHolidayOTPay, AccuMath.CommercialRound(_benchmarkPaystubRate.RegularHolidayOTHours * hourlyRate * overtimeRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualRegularHolidayOTPay, AccuMath.CommercialRound(_benchmarkPaystubRate.RegularHolidayOTHours * actualHourlyRate * overtimeRate))

        Assert.AreEqual(_benchmarkPaystubRate.RegularHolidayRestDayPay, AccuMath.CommercialRound(_benchmarkPaystubRate.RegularHolidayRestDayHours * hourlyRate * restDayRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualRegularHolidayRestDayPay, AccuMath.CommercialRound(_benchmarkPaystubRate.RegularHolidayRestDayHours * actualHourlyRate * restDayRate))

        Assert.AreEqual(_benchmarkPaystubRate.RegularHolidayRestDayOTPay, AccuMath.CommercialRound(_benchmarkPaystubRate.RegularHolidayRestDayOTHours * hourlyRate * restDayOvertimeRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualRegularHolidayRestDayOTPay, AccuMath.CommercialRound(_benchmarkPaystubRate.RegularHolidayRestDayOTHours * actualHourlyRate * restDayOvertimeRate))
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

        Assert.AreEqual(_benchmarkPaystubRate.SpecialHolidayPay, AccuMath.CommercialRound(_benchmarkPaystubRate.SpecialHolidayHours * hourlyRate * specialHolidayRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualSpecialHolidayPay, _benchmarkPaystubRate.SpecialHolidayPay)

        Assert.AreEqual(_benchmarkPaystubRate.SpecialHolidayOTPay, AccuMath.CommercialRound(_benchmarkPaystubRate.SpecialHolidayOTHours * hourlyRate * specialHolidayOvertimeRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualSpecialHolidayOTPay, _benchmarkPaystubRate.SpecialHolidayOTPay)

        Assert.AreEqual(_benchmarkPaystubRate.SpecialHolidayRestDayPay, AccuMath.CommercialRound(_benchmarkPaystubRate.SpecialHolidayRestDayHours * hourlyRate * specialHolidayRestDayRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualSpecialHolidayRestDayPay, _benchmarkPaystubRate.SpecialHolidayRestDayPay)

        Assert.AreEqual(_benchmarkPaystubRate.SpecialHolidayRestDayOTPay, AccuMath.CommercialRound(_benchmarkPaystubRate.SpecialHolidayRestDayOTHours * hourlyRate * specialHolidayRestDayOvertimeRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualSpecialHolidayRestDayOTPay, _benchmarkPaystubRate.SpecialHolidayRestDayOTPay)

        Assert.AreEqual(_benchmarkPaystubRate.RegularHolidayPay, AccuMath.CommercialRound(_benchmarkPaystubRate.RegularHolidayHours * hourlyRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualRegularHolidayPay, AccuMath.CommercialRound(_benchmarkPaystubRate.RegularHolidayHours * actualHourlyRate))

        Assert.AreEqual(_benchmarkPaystubRate.RegularHolidayOTPay, AccuMath.CommercialRound(_benchmarkPaystubRate.RegularHolidayOTHours * hourlyRate * overtimeRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualRegularHolidayOTPay, _benchmarkPaystubRate.RegularHolidayOTPay)

        Assert.AreEqual(_benchmarkPaystubRate.RegularHolidayRestDayPay, AccuMath.CommercialRound(_benchmarkPaystubRate.RegularHolidayRestDayHours * hourlyRate * restDayRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualRegularHolidayRestDayPay, _benchmarkPaystubRate.RegularHolidayRestDayPay)

        Assert.AreEqual(_benchmarkPaystubRate.RegularHolidayRestDayOTPay, AccuMath.CommercialRound(_benchmarkPaystubRate.RegularHolidayRestDayOTHours * hourlyRate * restDayOvertimeRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualRegularHolidayRestDayOTPay, _benchmarkPaystubRate.RegularHolidayRestDayOTPay)
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

        Assert.AreEqual(_benchmarkPaystubRate.RestDayPay, AccuMath.CommercialRound(_benchmarkPaystubRate.RestDayHours * hourlyRate * restDayRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualRestDayPay, AccuMath.CommercialRound(_benchmarkPaystubRate.RestDayHours * actualHourlyRate * restDayRate))

        Assert.AreEqual(_benchmarkPaystubRate.RestDayOTPay, AccuMath.CommercialRound(_benchmarkPaystubRate.RestDayOTHours * hourlyRate * restDayOvertimeRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualRestDayOTPay, AccuMath.CommercialRound(_benchmarkPaystubRate.RestDayOTHours * actualHourlyRate * restDayOvertimeRate))

    End Sub

    <Test>
    Public Sub ShouldComputeRestDayPays_WithFalseEmployeeEntitledForRestDayPay()
        DefaultOvertimeInitialization(employeeEntitledForRestDayPay:=False)

        Assert.AreEqual(_benchmarkPaystubRate.RestDayPay, 0)
        Assert.AreEqual(_benchmarkPaystubRate.ActualRestDayPay, 0)

        Assert.AreEqual(_benchmarkPaystubRate.RestDayOTPay, 0)
        Assert.AreEqual(_benchmarkPaystubRate.ActualRestDayOTPay, 0)

    End Sub

    <Test>
    Public Sub ShouldComputeRestDayPays_WithAllowanceForRestDay()
        DefaultOvertimeInitialization(allowanceForRestDayPolicy:=False, allowanceForRestDayOTPolicy:=False)

        Dim hourlyRate = 67.125

        Dim restDayRate = 1.3
        Dim restDayOvertimeRate = 1.69

        Assert.AreEqual(_benchmarkPaystubRate.RestDayPay, AccuMath.CommercialRound(_benchmarkPaystubRate.RestDayHours * hourlyRate * restDayRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualRestDayPay, _benchmarkPaystubRate.RestDayPay)

        Assert.AreEqual(_benchmarkPaystubRate.RestDayOTPay, AccuMath.CommercialRound(_benchmarkPaystubRate.RestDayOTHours * hourlyRate * restDayOvertimeRate))
        Assert.AreEqual(_benchmarkPaystubRate.ActualRestDayOTPay, _benchmarkPaystubRate.RestDayOTPay)

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

        _benchmarkPaystubRate.Compute(
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
    End Sub

End Class