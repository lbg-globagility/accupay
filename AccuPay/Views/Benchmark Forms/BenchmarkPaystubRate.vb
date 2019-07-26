Imports AccuPay.Entity
Imports PayrollSys

Public Class BenchmarkPaystubRate
    Implements IPaystubRate

    Public Const WorkHoursPerDay As Decimal = 8

    Public Property Employee As Employee
    Public Property Salary As Salary
    Public Property MonthlyRate As Decimal
    Public Property DailyRate As Decimal
    Public Property HourlyRate As Decimal
    Public Property ActualMonthlyRate As Decimal
    Public Property ActualDailyRate As Decimal
    Public Property ActualHourlyRate As Decimal

    Sub New(employee As Employee, salaries As List(Of Salary))

        Me.Employee = employee

        Me.Salary = salaries.
                            Where(Function(s) Nullable.Equals(s.EmployeeID, employee?.RowID)).
                            FirstOrDefault

        Me.MonthlyRate = AccuMath.CommercialRound(PayrollTools.GetEmployeeMonthlyRate(employee, Me.Salary), 4)
        Me.DailyRate = AccuMath.CommercialRound(PayrollTools.GetDailyRate(Me.MonthlyRate, employee.WorkDaysPerYear), 4)
        Me.HourlyRate = AccuMath.CommercialRound(PayrollTools.GetHourlyRateByDailyRate(Me.DailyRate), 4)

        Me.ActualMonthlyRate = AccuMath.CommercialRound(PayrollTools.GetEmployeeMonthlyRate(employee, Me.Salary, isActual:=True), 4)
        Me.ActualDailyRate = AccuMath.CommercialRound(PayrollTools.GetDailyRate(Me.ActualMonthlyRate, employee.WorkDaysPerYear), 4)
        Me.ActualHourlyRate = AccuMath.CommercialRound(PayrollTools.GetHourlyRateByDailyRate(Me.ActualDailyRate), 4)

    End Sub

    Public Function IsInvalid() As Boolean

        Return Me.Employee Is Nothing OrElse Me.Salary Is Nothing

    End Function

    Public Function AllowanceSalary() As Decimal

        Return Me.Salary.AllowanceSalary

    End Function

    Public Property RegularHours As Decimal Implements IPaystubRate.RegularHours
    Public Property RegularPay As Decimal Implements IPaystubRate.RegularPay
    Public Property ActualRegularPay As Decimal Implements IPaystubRate.ActualRegularPay

    Public Property OvertimeHours As Decimal Implements IPaystubRate.OvertimeHours
    Public Property OvertimePay As Decimal Implements IPaystubRate.OvertimePay
    Public Property ActualOvertimePay As Decimal Implements IPaystubRate.ActualOvertimePay

    Public Property NightDiffHours As Decimal Implements IPaystubRate.NightDiffHours
    Public Property NightDiffPay As Decimal Implements IPaystubRate.NightDiffPay
    Public Property ActualNightDiffPay As Decimal Implements IPaystubRate.ActualNightDiffPay

    Public Property NightDiffOvertimeHours As Decimal Implements IPaystubRate.NightDiffOvertimeHours
    Public Property NightDiffOvertimePay As Decimal Implements IPaystubRate.NightDiffOvertimePay
    Public Property ActualNightDiffOTPay As Decimal Implements IPaystubRate.ActualNightDiffOTPay

    Public Property RestDayHours As Decimal Implements IPaystubRate.RestDayHours
    Public Property RestDayPay As Decimal Implements IPaystubRate.RestDayPay
    Public Property ActualRestDayPay As Decimal Implements IPaystubRate.ActualRestDayPay

    Public Property RestDayOTHours As Decimal Implements IPaystubRate.RestDayOTHours
    Public Property RestDayOTPay As Decimal Implements IPaystubRate.RestDayOTPay
    Public Property ActualRestDayOTPay As Decimal Implements IPaystubRate.ActualRestDayOTPay

    Public Property SpecialHolidayHours As Decimal Implements IPaystubRate.SpecialHolidayHours
    Public Property SpecialHolidayPay As Decimal Implements IPaystubRate.SpecialHolidayPay
    Public Property ActualSpecialHolidayPay As Decimal Implements IPaystubRate.ActualSpecialHolidayPay

    Public Property SpecialHolidayOTHours As Decimal Implements IPaystubRate.SpecialHolidayOTHours
    Public Property SpecialHolidayOTPay As Decimal Implements IPaystubRate.SpecialHolidayOTPay
    Public Property ActualSpecialHolidayOTPay As Decimal Implements IPaystubRate.ActualSpecialHolidayOTPay

    Public Property RegularHolidayHours As Decimal Implements IPaystubRate.RegularHolidayHours
    Public Property RegularHolidayPay As Decimal Implements IPaystubRate.RegularHolidayPay
    Public Property ActualRegularHolidayPay As Decimal Implements IPaystubRate.ActualRegularHolidayPay

    Public Property RegularHolidayOTHours As Decimal Implements IPaystubRate.RegularHolidayOTHours
    Public Property RegularHolidayOTPay As Decimal Implements IPaystubRate.RegularHolidayOTPay
    Public Property ActualRegularHolidayOTPay As Decimal Implements IPaystubRate.ActualRegularHolidayOTPay

    Public Property HolidayPay As Decimal Implements IPaystubRate.HolidayPay

    Public Property LeaveHours As Decimal Implements IPaystubRate.LeaveHours
    Public Property LeavePay As Decimal Implements IPaystubRate.LeavePay
    Public Property ActualLeavePay As Decimal Implements IPaystubRate.ActualLeavePay

    Public Property LateHours As Decimal Implements IPaystubRate.LateHours
    Public Property LateDeduction As Decimal Implements IPaystubRate.LateDeduction
    Public Property ActualLateDeduction As Decimal Implements IPaystubRate.ActualLateDeduction

    Public Property UndertimeHours As Decimal Implements IPaystubRate.UndertimeHours
    Public Property UndertimeDeduction As Decimal Implements IPaystubRate.UndertimeDeduction
    Public Property ActualUndertimeDeduction As Decimal Implements IPaystubRate.ActualUndertimeDeduction

    Public Property AbsentHours As Decimal Implements IPaystubRate.AbsentHours
    Public Property AbsenceDeduction As Decimal Implements IPaystubRate.AbsenceDeduction
    Public Property ActualAbsenceDeduction As Decimal Implements IPaystubRate.ActualAbsenceDeduction

    'new
    Public Property RestDayNightDiffHours As Decimal

    Public Property RestDayNightDiffPay As Decimal
    Public Property ActualRestDayNightDiffPay As Decimal

    Public Property RestDayNightDiffOTHours As Decimal
    Public Property RestDayNightDiffOTPay As Decimal
    Public Property ActualRestDayNightDiffOTPay As Decimal

    Public Property SpecialHolidayNightDiffHours As Decimal
    Public Property SpecialHolidayNightDiffPay As Decimal
    Public Property ActualSpecialHolidayNightDiffPay As Decimal

    Public Property SpecialHolidayNightDiffOTHours As Decimal
    Public Property SpecialHolidayNightDiffOTPay As Decimal
    Public Property ActualSpecialHolidayNightDiffOTPay As Decimal

    Public Property SpecialHolidayRestDayHours As Decimal
    Public Property SpecialHolidayRestDayPay As Decimal
    Public Property ActualSpecialHolidayRestDayPay As Decimal

    Public Property SpecialHolidayRestDayOTHours As Decimal
    Public Property SpecialHolidayRestDayOTPay As Decimal
    Public Property ActualSpecialHolidayRestDayOTPay As Decimal

    Public Property SpecialHolidayRestDayNightDiffHours As Decimal
    Public Property SpecialHolidayRestDayNightDiffPay As Decimal
    Public Property ActualSpecialHolidayRestDayNightDiffPay As Decimal

    Public Property SpecialHolidayRestDayNightDiffOTHours As Decimal
    Public Property SpecialHolidayRestDayNightDiffOTPay As Decimal
    Public Property ActualSpecialHolidayRestDayNightDiffOTPay As Decimal

    Public Property RegularHolidayNightDiffHours As Decimal
    Public Property RegularHolidayNightDiffPay As Decimal
    Public Property ActualRegularHolidayNightDiffPay As Decimal

    Public Property RegularHolidayNightDiffOTHours As Decimal
    Public Property RegularHolidayNightDiffOTPay As Decimal
    Public Property ActualRegularHolidayNightDiffOTPay As Decimal

    Public Property RegularHolidayRestDayHours As Decimal
    Public Property RegularHolidayRestDayPay As Decimal
    Public Property ActualRegularHolidayRestDayPay As Decimal

    Public Property RegularHolidayRestDayOTHours As Decimal
    Public Property RegularHolidayRestDayOTPay As Decimal
    Public Property ActualRegularHolidayRestDayOTPay As Decimal

    Public Property RegularHolidayRestDayNightDiffHours As Decimal
    Public Property RegularHolidayRestDayNightDiffPay As Decimal
    Public Property ActualRegularHolidayRestDayNightDiffPay As Decimal

    Public Property RegularHolidayRestDayNightDiffOTHours As Decimal
    Public Property RegularHolidayRestDayNightDiffOTPay As Decimal
    Public Property ActualRegularHolidayRestDayNightDiffOTPay As Decimal

    Public Sub Compute(
                payRate As OvertimeRate,
                employeeEntitledForNightDifferentialPay As Boolean,
                employeeEntitledForSpecialHolidayPay As Boolean,
                employeeEntitledForRegularHolidayPay As Boolean,
                employeeEntitledForRestDayPay As Boolean,
                allowanceForOvertimePolicy As Boolean,
                allowanceForNightDiffPolicy As Boolean,
                allowanceForNightDiffOTPolicy As Boolean,
                allowanceForHolidayPolicy As Boolean,
                allowanceForRestDayPolicy As Boolean,
                allowanceForRestDayOTPolicy As Boolean,
                regularHours As Decimal,
                overtimeHours As Decimal,
                nightDiffHours As Decimal,
                nightDiffOvertimeHours As Decimal,
                restDayHours As Decimal,
                restDayOTHours As Decimal,
                specialHolidayHours As Decimal,
                specialHolidayOTHours As Decimal,
                regularHolidayHours As Decimal,
                regularHolidayOTHours As Decimal,
                leaveHours As Decimal,
                lateHours As Decimal,
                undertimeHours As Decimal,
                absentHours As Decimal,
                restDayNightDiffHours As Decimal,
                restDayNightDiffOTHours As Decimal,
                specialHolidayNightDiffHours As Decimal,
                specialHolidayNightDiffOTHours As Decimal,
                specialHolidayRestDayHours As Decimal,
                specialHolidayRestDayOTHours As Decimal,
                specialHolidayRestDayNightDiffHours As Decimal,
                specialHolidayRestDayNightDiffOTHours As Decimal,
                regularHolidayNightDiffHours As Decimal,
                regularHolidayNightDiffOTHours As Decimal,
                regularHolidayRestDayHours As Decimal,
                regularHolidayRestDayOTHours As Decimal,
                regularHolidayRestDayNightDiffHours As Decimal,
                regularHolidayRestDayNightDiffOTHours As Decimal)

        Dim allowanceRate = If(
            If(Me.Salary?.BasicSalary, 0) = 0,
            0D,
            If(Me.Salary?.AllowanceSalary, 0) / If(Me.Salary?.BasicSalary, 0))

        ComputeRegularPays(payRate, allowanceForOvertimePolicy, regularHours, overtimeHours, leaveHours, allowanceRate)
        ComputeDeductions(lateHours, undertimeHours, absentHours)

        If employeeEntitledForNightDifferentialPay Then

            ComputeNightDifferentialPays(
                payRate:=payRate,
                allowanceForNightDiffPolicy:=allowanceForNightDiffPolicy,
                nightDiffHours:=nightDiffHours,
                restDayNightDiffHours:=restDayNightDiffHours,
                specialHolidayNightDiffHours:=specialHolidayNightDiffHours,
                specialHolidayRestDayNightDiffHours:=specialHolidayRestDayNightDiffHours,
                regularHolidayNightDiffHours:=regularHolidayNightDiffHours,
                regularHolidayRestDayNightDiffHours:=regularHolidayRestDayNightDiffHours,
                allowanceRate:=allowanceRate)

            ComputeNightDifferentialOvertimePays(
                payRate:=payRate,
                allowanceForNightDiffOTPolicy:=allowanceForNightDiffOTPolicy,
                nightDiffOvertimeHours:=nightDiffOvertimeHours,
                restDayNightDiffOTHours:=restDayNightDiffOTHours,
                specialHolidayNightDiffOTHours:=specialHolidayNightDiffOTHours,
                specialHolidayRestDayNightDiffOTHours:=specialHolidayRestDayNightDiffOTHours,
                regularHolidayNightDiffOTHours:=regularHolidayNightDiffOTHours,
                regularHolidayRestDayNightDiffOTHours:=regularHolidayRestDayNightDiffOTHours,
                allowanceRate:=allowanceRate)

        End If

        ComputeHolidayPays(
            payRate:=payRate,
            employeeEntitledForSpecialHolidayPay:=employeeEntitledForSpecialHolidayPay,
            employeeEntitledForRegularHolidayPay:=employeeEntitledForRegularHolidayPay,
            allowanceForHolidayPolicy:=allowanceForHolidayPolicy,
            allowanceForRestDayPolicy:=allowanceForRestDayPolicy,
            allowanceForRestDayOTPolicy:=allowanceForRestDayOTPolicy,
            allowanceForOvertimePolicy:=allowanceForOvertimePolicy,
            specialHolidayHours:=specialHolidayHours,
            specialHolidayOTHours:=specialHolidayOTHours,
            regularHolidayHours:=regularHolidayHours,
            regularHolidayOTHours:=regularHolidayOTHours,
            specialHolidayRestDayHours:=specialHolidayRestDayHours,
            specialHolidayRestDayOTHours:=specialHolidayRestDayOTHours,
            regularHolidayRestDayHours:=regularHolidayRestDayHours,
            regularHolidayRestDayOTHours:=regularHolidayRestDayOTHours)

        If employeeEntitledForRestDayPay Then

            ComputeRestDayPays(
                payRate:=payRate,
                allowanceForRestDayPolicy:=allowanceForRestDayPolicy,
                allowanceForRestDayOTPolicy:=allowanceForRestDayOTPolicy,
                restDayHours:=restDayHours,
                restDayOTHours:=restDayOTHours,
                allowanceRate:=allowanceRate)

        End If

    End Sub

    Private Sub ComputeRegularPays(payRate As OvertimeRate, allowanceForOvertimePolicy As Boolean, regularHours As Decimal, overtimeHours As Decimal, leaveHours As Decimal, allowanceRate As Decimal)
        Me.RegularHours = regularHours
        Me.RegularPay = AccuMath.CommercialRound(Me.RegularHours * Me.HourlyRate)
        Me.ActualRegularPay = AccuMath.CommercialRound(Me.RegularHours * Me.ActualHourlyRate)

        Me.OvertimeHours = overtimeHours
        Me.OvertimePay = AccuMath.CommercialRound(Me.OvertimeHours * Me.HourlyRate * payRate.Overtime.Rate)
        Me.ActualOvertimePay = Me.OvertimePay
        If allowanceForOvertimePolicy Then
            Me.ActualOvertimePay += AccuMath.CommercialRound(Me.OvertimePay * allowanceRate)
        End If

        Me.LeaveHours = leaveHours
        Me.LeavePay = AccuMath.CommercialRound(Me.LeaveHours * Me.HourlyRate)
        Me.ActualLeavePay = AccuMath.CommercialRound(Me.LeaveHours * Me.ActualHourlyRate)
    End Sub

    Private Sub ComputeDeductions(lateHours As Decimal, undertimeHours As Decimal, absentHours As Decimal)
        'Deductions
        Me.LateHours = lateHours
        Me.LateDeduction = AccuMath.CommercialRound(Me.LateHours * Me.HourlyRate)
        Me.ActualLateDeduction = AccuMath.CommercialRound(Me.LateHours * Me.ActualHourlyRate)

        Me.UndertimeHours = undertimeHours
        Me.UndertimeDeduction = AccuMath.CommercialRound(Me.UndertimeHours * Me.HourlyRate)
        Me.ActualUndertimeDeduction = AccuMath.CommercialRound(Me.UndertimeHours * Me.ActualHourlyRate)

        Me.AbsentHours = absentHours
        Me.AbsenceDeduction = AccuMath.CommercialRound(Me.AbsentHours * Me.HourlyRate)
        Me.ActualAbsenceDeduction = AccuMath.CommercialRound(Me.AbsentHours * Me.ActualHourlyRate)
    End Sub

    Private Function GetNightDifferentialRate(basicRate As Decimal, nightDifferentialRate As Decimal) As Decimal

        Return nightDifferentialRate - basicRate

    End Function

    Private Sub ComputeNightDifferentialPays(payRate As OvertimeRate, allowanceForNightDiffPolicy As Boolean, nightDiffHours As Decimal, restDayNightDiffHours As Decimal, specialHolidayNightDiffHours As Decimal, specialHolidayRestDayNightDiffHours As Decimal, regularHolidayNightDiffHours As Decimal, regularHolidayRestDayNightDiffHours As Decimal, allowanceRate As Decimal)
        Me.NightDiffHours = nightDiffHours
        Me.NightDiffPay = Me.NightDiffHours * Me.HourlyRate * GetNightDifferentialRate(payRate.BasePay.Rate, payRate.NightDifferential.Rate)
        Me.ActualNightDiffPay = Me.NightDiffPay

        Me.RestDayNightDiffHours = restDayNightDiffHours
        Me.RestDayNightDiffPay = Me.RestDayNightDiffHours * Me.HourlyRate * GetNightDifferentialRate(payRate.RestDay.Rate, payRate.RestDayNightDifferential.Rate)
        Me.ActualRestDayNightDiffPay = Me.RestDayNightDiffPay

        Me.SpecialHolidayNightDiffHours = specialHolidayNightDiffHours
        Me.SpecialHolidayNightDiffPay = Me.SpecialHolidayNightDiffHours * Me.HourlyRate * GetNightDifferentialRate(payRate.SpecialHoliday.Rate, payRate.SpecialHolidayNightDifferential.Rate)
        Me.ActualSpecialHolidayNightDiffPay = Me.SpecialHolidayNightDiffPay

        Me.SpecialHolidayRestDayNightDiffHours = specialHolidayRestDayNightDiffHours
        Me.SpecialHolidayRestDayNightDiffPay = Me.SpecialHolidayRestDayNightDiffHours * Me.HourlyRate * GetNightDifferentialRate(payRate.SpecialHolidayRestDay.Rate, payRate.SpecialHolidayRestDayNightDifferential.Rate)
        Me.ActualSpecialHolidayRestDayNightDiffPay = Me.SpecialHolidayRestDayNightDiffPay

        Me.RegularHolidayNightDiffHours = regularHolidayNightDiffHours
        Me.RegularHolidayNightDiffPay = Me.RegularHolidayNightDiffHours * Me.HourlyRate * GetNightDifferentialRate(payRate.RegularHoliday.Rate, payRate.RegularHolidayNightDifferential.Rate)
        Me.ActualRegularHolidayNightDiffPay = Me.RegularHolidayNightDiffPay

        Me.RegularHolidayRestDayNightDiffHours = regularHolidayRestDayNightDiffHours
        Me.RegularHolidayRestDayNightDiffPay = Me.RegularHolidayRestDayNightDiffHours * Me.HourlyRate * GetNightDifferentialRate(payRate.RegularHolidayRestDay.Rate, payRate.RegularHolidayRestDayNightDifferential.Rate)
        Me.ActualRegularHolidayRestDayNightDiffPay = Me.RegularHolidayRestDayNightDiffPay

        If allowanceForNightDiffPolicy Then
            Me.ActualNightDiffPay += Me.NightDiffPay * allowanceRate
            Me.ActualRestDayNightDiffPay += Me.RestDayNightDiffPay * allowanceRate
            Me.ActualSpecialHolidayNightDiffPay += Me.SpecialHolidayNightDiffPay * allowanceRate
            Me.ActualSpecialHolidayRestDayNightDiffPay += Me.SpecialHolidayRestDayNightDiffPay * allowanceRate
            Me.ActualRegularHolidayNightDiffPay += Me.RegularHolidayNightDiffPay * allowanceRate
            Me.ActualRegularHolidayRestDayNightDiffPay += Me.RegularHolidayRestDayNightDiffPay * allowanceRate
        End If
    End Sub

    Private Sub ComputeNightDifferentialOvertimePays(payRate As OvertimeRate, allowanceForNightDiffOTPolicy As Boolean, nightDiffOvertimeHours As Decimal, restDayNightDiffOTHours As Decimal, specialHolidayNightDiffOTHours As Decimal, specialHolidayRestDayNightDiffOTHours As Decimal, regularHolidayNightDiffOTHours As Decimal, regularHolidayRestDayNightDiffOTHours As Decimal, allowanceRate As Decimal)
        Me.NightDiffOvertimeHours = nightDiffOvertimeHours
        Me.NightDiffOvertimePay = AccuMath.CommercialRound(Me.NightDiffOvertimeHours * Me.HourlyRate * payRate.NightDifferentialOvertime.Rate)
        Me.ActualNightDiffOTPay = Me.NightDiffOvertimePay

        Me.RestDayNightDiffOTHours = restDayNightDiffOTHours
        Me.RestDayNightDiffOTPay = AccuMath.CommercialRound(Me.RestDayNightDiffOTHours * Me.HourlyRate * payRate.RestDayNightDifferentialOvertime.Rate)
        Me.ActualRestDayNightDiffOTPay = Me.RestDayNightDiffOTPay

        Me.SpecialHolidayNightDiffOTHours = specialHolidayNightDiffOTHours
        Me.SpecialHolidayNightDiffOTPay = AccuMath.CommercialRound(Me.SpecialHolidayNightDiffOTHours * Me.HourlyRate * payRate.SpecialHolidayNightDifferentialOvertime.Rate)
        Me.ActualSpecialHolidayNightDiffOTPay = Me.SpecialHolidayNightDiffOTPay

        Me.SpecialHolidayRestDayNightDiffOTHours = specialHolidayRestDayNightDiffOTHours
        Me.SpecialHolidayRestDayNightDiffOTPay = AccuMath.CommercialRound(Me.SpecialHolidayRestDayNightDiffOTHours * Me.HourlyRate * payRate.SpecialHolidayRestDayNightDifferentialOvertime.Rate)
        Me.ActualSpecialHolidayRestDayNightDiffOTPay = Me.SpecialHolidayRestDayNightDiffOTPay

        Me.RegularHolidayNightDiffOTHours = regularHolidayNightDiffOTHours
        Me.RegularHolidayNightDiffOTPay = AccuMath.CommercialRound(Me.RegularHolidayNightDiffOTHours * Me.HourlyRate * payRate.RegularHolidayNightDifferentialOvertime.Rate)
        Me.ActualRegularHolidayNightDiffOTPay = Me.RegularHolidayNightDiffOTPay

        Me.RegularHolidayRestDayNightDiffOTHours = regularHolidayRestDayNightDiffOTHours
        Me.RegularHolidayRestDayNightDiffOTPay = AccuMath.CommercialRound(Me.RegularHolidayRestDayNightDiffOTHours * Me.HourlyRate * payRate.RegularHolidayRestDayNightDifferentialOvertime.Rate)
        Me.ActualRegularHolidayRestDayNightDiffOTPay = Me.RegularHolidayRestDayNightDiffOTPay

        If allowanceForNightDiffOTPolicy Then
            Me.ActualNightDiffOTPay += AccuMath.CommercialRound(Me.NightDiffOvertimePay * allowanceRate)
            Me.ActualRestDayNightDiffOTPay += AccuMath.CommercialRound(Me.RestDayNightDiffOTPay * allowanceRate)
            Me.ActualSpecialHolidayNightDiffOTPay += AccuMath.CommercialRound(Me.SpecialHolidayNightDiffOTPay * allowanceRate)
            Me.ActualSpecialHolidayRestDayNightDiffOTPay += AccuMath.CommercialRound(Me.SpecialHolidayRestDayNightDiffOTPay * allowanceRate)
            Me.ActualRegularHolidayNightDiffOTPay += AccuMath.CommercialRound(Me.RegularHolidayNightDiffOTPay * allowanceRate)
            Me.ActualRegularHolidayRestDayNightDiffOTPay += AccuMath.CommercialRound(Me.RegularHolidayRestDayNightDiffOTPay * allowanceRate)
        End If
    End Sub

    Private Function GetRateWithCondition(normalRate As Decimal, premiumRate As Decimal, condition As Boolean) As Decimal

        Return If(condition, premiumRate, normalRate)

    End Function

    Private Sub ComputeHolidayPays(
                    payRate As OvertimeRate,
                    employeeEntitledForSpecialHolidayPay As Boolean,
                    employeeEntitledForRegularHolidayPay As Boolean,
                    allowanceForHolidayPolicy As Boolean,
                    allowanceForRestDayPolicy As Boolean,
                    allowanceForRestDayOTPolicy As Boolean,
                    allowanceForOvertimePolicy As Boolean,
                    specialHolidayHours As Decimal,
                    specialHolidayOTHours As Decimal,
                    regularHolidayHours As Decimal,
                    regularHolidayOTHours As Decimal,
                    specialHolidayRestDayHours As Decimal,
                    specialHolidayRestDayOTHours As Decimal,
                    regularHolidayRestDayHours As Decimal,
                    regularHolidayRestDayOTHours As Decimal)

        Me.SpecialHolidayHours = specialHolidayHours
        Me.SpecialHolidayPay = ComputeSpecialHolidayPay(payRate, Me.HourlyRate, employeeEntitledForSpecialHolidayPay)
        Me.ActualSpecialHolidayPay = Me.SpecialHolidayPay

        Me.SpecialHolidayOTHours = specialHolidayOTHours
        Me.SpecialHolidayOTPay = ComputeSpecialOTPay(payRate, Me.HourlyRate, employeeEntitledForSpecialHolidayPay)
        Me.ActualSpecialHolidayOTPay = Me.SpecialHolidayOTPay

        Me.SpecialHolidayRestDayHours = specialHolidayRestDayHours
        Me.SpecialHolidayRestDayPay = ComputeSpecialHolidayRestDayPay(payRate, Me.HourlyRate, employeeEntitledForSpecialHolidayPay)
        Me.ActualSpecialHolidayRestDayPay = Me.SpecialHolidayRestDayPay

        Me.SpecialHolidayRestDayOTHours = specialHolidayRestDayOTHours
        Me.SpecialHolidayRestDayOTPay = ComputeSpecialHolidayRestDayOTPay(payRate, Me.HourlyRate, employeeEntitledForSpecialHolidayPay)
        Me.ActualSpecialHolidayRestDayOTPay = Me.SpecialHolidayRestDayOTPay

        Me.RegularHolidayHours = regularHolidayHours
        Me.RegularHolidayPay = ComputeRegularHolidayPay(payRate, Me.HourlyRate, employeeEntitledForRegularHolidayPay)
        Me.ActualRegularHolidayPay = Me.RegularHolidayPay

        Me.RegularHolidayOTHours = regularHolidayOTHours
        Me.RegularHolidayOTPay = ComputeRegularHolidayOTPay(payRate, Me.HourlyRate, employeeEntitledForRegularHolidayPay)
        Me.ActualRegularHolidayOTPay = Me.RegularHolidayOTPay

        Me.RegularHolidayRestDayHours = regularHolidayRestDayHours
        Me.RegularHolidayRestDayPay = ComputeRegularHolidayRestDayPay(payRate, Me.HourlyRate, employeeEntitledForRegularHolidayPay)
        Me.ActualRegularHolidayRestDayPay = Me.RegularHolidayRestDayPay

        Me.RegularHolidayRestDayOTHours = regularHolidayRestDayOTHours
        Me.RegularHolidayRestDayOTPay = GetRegularHolidayRestDayOTPay(payRate, Me.HourlyRate, employeeEntitledForRegularHolidayPay)
        Me.ActualRegularHolidayRestDayOTPay = Me.RegularHolidayRestDayOTPay

        ComputeActualPayForHolidays(
            payRate:=payRate,
            employeeEntitledForSpecialHolidayPay:=employeeEntitledForSpecialHolidayPay,
            employeeEntitledForRegularHolidayPay:=employeeEntitledForRegularHolidayPay,
            allowanceForHolidayPolicy:=allowanceForHolidayPolicy,
            allowanceForRestDayPolicy:=allowanceForRestDayPolicy,
            allowanceForRestDayOTPolicy:=allowanceForRestDayOTPolicy,
            allowanceForOvertimePolicy:=allowanceForOvertimePolicy)
    End Sub

    Private Function ComputeSpecialHolidayPay(payRate As OvertimeRate, hourlyRate As Decimal, employeeEntitledForSpecialHolidayPay As Boolean) As Decimal
        Return AccuMath.CommercialRound(Me.SpecialHolidayHours * hourlyRate * GetRateWithCondition(payRate.BasePay.Rate, payRate.SpecialHoliday.Rate, employeeEntitledForSpecialHolidayPay))
    End Function

    Private Function ComputeSpecialOTPay(payRate As OvertimeRate, hourlyRate As Decimal, employeeEntitledForSpecialHolidayPay As Boolean) As Decimal
        Return AccuMath.CommercialRound(Me.SpecialHolidayOTHours * hourlyRate * GetRateWithCondition(payRate.Overtime.Rate, payRate.SpecialHolidayOvertime.Rate, employeeEntitledForSpecialHolidayPay))
    End Function

    Private Function ComputeSpecialHolidayRestDayPay(payRate As OvertimeRate, hourlyRate As Decimal, employeeEntitledForSpecialHolidayPay As Boolean) As Decimal
        Return AccuMath.CommercialRound(Me.SpecialHolidayRestDayHours * hourlyRate * GetRateWithCondition(payRate.RestDay.Rate, payRate.SpecialHolidayRestDay.Rate, employeeEntitledForSpecialHolidayPay))
    End Function

    Private Function ComputeSpecialHolidayRestDayOTPay(payRate As OvertimeRate, hourlyRate As Decimal, employeeEntitledForSpecialHolidayPay As Boolean) As Decimal
        Return AccuMath.CommercialRound(Me.SpecialHolidayRestDayOTHours * hourlyRate * GetRateWithCondition(payRate.RestDayOvertime.Rate, payRate.SpecialHolidayRestDayOvertime.Rate, employeeEntitledForSpecialHolidayPay))
    End Function

    Private Function ComputeRegularHolidayPay(payRate As OvertimeRate, hourlyRate As Decimal, employeeEntitledForRegularHolidayPay As Boolean) As Decimal
        Return AccuMath.CommercialRound(Me.RegularHolidayHours * hourlyRate * GetRateWithCondition(payRate.BasePay.Rate, payRate.RegularHoliday.Rate, employeeEntitledForRegularHolidayPay))
    End Function

    Private Function ComputeRegularHolidayOTPay(payRate As OvertimeRate, hourlyRate As Decimal, employeeEntitledForRegularHolidayPay As Boolean) As Decimal
        Return AccuMath.CommercialRound(Me.RegularHolidayOTHours * hourlyRate * GetRateWithCondition(payRate.Overtime.Rate, payRate.RegularHolidayOvertime.Rate, employeeEntitledForRegularHolidayPay))
    End Function

    Private Function ComputeRegularHolidayRestDayPay(payRate As OvertimeRate, hourlyRate As Decimal, employeeEntitledForRegularHolidayPay As Boolean) As Decimal
        Return AccuMath.CommercialRound(Me.RegularHolidayRestDayHours * hourlyRate * GetRateWithCondition(payRate.RestDay.Rate, payRate.RegularHolidayRestDay.Rate, employeeEntitledForRegularHolidayPay))
    End Function

    Private Function GetRegularHolidayRestDayOTPay(payRate As OvertimeRate, hourlyRate As Decimal, employeeEntitledForRegularHolidayPay As Boolean) As Decimal
        Return AccuMath.CommercialRound(Me.RegularHolidayRestDayOTHours * hourlyRate * GetRateWithCondition(payRate.RestDayOvertime.Rate, payRate.RegularHolidayRestDayOvertime.Rate, employeeEntitledForRegularHolidayPay))
    End Function

    Private Sub ComputeActualPayForHolidays(payRate As OvertimeRate, employeeEntitledForSpecialHolidayPay As Boolean, employeeEntitledForRegularHolidayPay As Boolean, allowanceForHolidayPolicy As Boolean, allowanceForRestDayPolicy As Boolean, allowanceForRestDayOTPolicy As Boolean, allowanceForOvertimePolicy As Boolean)
        'if employeeEntitledForSpecialHolidayPay = false, treat is as normal day
        If employeeEntitledForSpecialHolidayPay = False Then

            If allowanceForRestDayPolicy Then
                Me.ActualSpecialHolidayRestDayPay = ComputeSpecialHolidayRestDayPay(payRate, Me.ActualHourlyRate, employeeEntitledForSpecialHolidayPay)
            End If

            If allowanceForRestDayOTPolicy Then
                Me.ActualSpecialHolidayRestDayOTPay = ComputeSpecialHolidayRestDayOTPay(payRate, Me.ActualHourlyRate, employeeEntitledForSpecialHolidayPay)
            End If

            If allowanceForOvertimePolicy Then
                Me.ActualSpecialHolidayOTPay = ComputeSpecialOTPay(payRate, Me.ActualHourlyRate, employeeEntitledForSpecialHolidayPay)
            End If

            Me.ActualSpecialHolidayPay = ComputeSpecialHolidayPay(payRate, Me.ActualHourlyRate, employeeEntitledForSpecialHolidayPay)
        Else
            If allowanceForHolidayPolicy Then
                Me.ActualSpecialHolidayRestDayPay = ComputeSpecialHolidayRestDayPay(payRate, Me.ActualHourlyRate, employeeEntitledForSpecialHolidayPay)
                Me.ActualSpecialHolidayRestDayOTPay = ComputeSpecialHolidayRestDayOTPay(payRate, Me.ActualHourlyRate, employeeEntitledForSpecialHolidayPay)
                Me.ActualSpecialHolidayOTPay = ComputeSpecialOTPay(payRate, Me.ActualHourlyRate, employeeEntitledForSpecialHolidayPay)
                Me.ActualSpecialHolidayPay = ComputeSpecialHolidayPay(payRate, Me.ActualHourlyRate, employeeEntitledForSpecialHolidayPay)
            End If
        End If

        'if employeeEntitledForRegularHolidayPay = false, treat is as normal day
        If employeeEntitledForRegularHolidayPay = False Then

            If allowanceForRestDayPolicy Then
                Me.ActualRegularHolidayRestDayPay = ComputeRegularHolidayRestDayPay(payRate, Me.ActualHourlyRate, employeeEntitledForRegularHolidayPay)
            End If

            If allowanceForRestDayOTPolicy Then
                Me.ActualRegularHolidayRestDayOTPay = GetRegularHolidayRestDayOTPay(payRate, Me.ActualHourlyRate, employeeEntitledForRegularHolidayPay)
            End If

            If allowanceForOvertimePolicy Then
                Me.ActualRegularHolidayOTPay = ComputeRegularHolidayOTPay(payRate, Me.ActualHourlyRate, employeeEntitledForRegularHolidayPay)
            End If

            Me.ActualRegularHolidayPay = ComputeRegularHolidayPay(payRate, Me.ActualHourlyRate, employeeEntitledForRegularHolidayPay)
        Else
            If allowanceForHolidayPolicy Then
                Me.ActualRegularHolidayRestDayPay = ComputeRegularHolidayRestDayPay(payRate, Me.ActualHourlyRate, employeeEntitledForRegularHolidayPay)
                Me.ActualRegularHolidayRestDayOTPay = GetRegularHolidayRestDayOTPay(payRate, Me.ActualHourlyRate, employeeEntitledForRegularHolidayPay)
                Me.ActualRegularHolidayOTPay = ComputeRegularHolidayOTPay(payRate, Me.ActualHourlyRate, employeeEntitledForRegularHolidayPay)
                Me.ActualRegularHolidayPay = ComputeRegularHolidayPay(payRate, Me.ActualHourlyRate, employeeEntitledForRegularHolidayPay)
            End If
        End If
    End Sub

    Private Sub ComputeRestDayPays(payRate As OvertimeRate, allowanceForRestDayPolicy As Boolean, allowanceForRestDayOTPolicy As Boolean, restDayHours As Decimal, restDayOTHours As Decimal, allowanceRate As Decimal)
        Me.RestDayHours = restDayHours
        Me.RestDayPay = Me.RestDayHours * Me.HourlyRate * payRate.RestDay.Rate
        Me.ActualRestDayPay = Me.RestDayPay

        Me.RestDayOTHours = restDayOTHours
        Me.RestDayOTPay = Me.RestDayOTHours * Me.HourlyRate * payRate.RestDayOvertime.Rate
        Me.ActualRestDayOTPay = Me.RestDayOTPay

        If allowanceForRestDayPolicy Then
            Me.ActualRestDayPay += Me.RestDayPay * allowanceRate
        End If

        If allowanceForRestDayOTPolicy Then
            Me.ActualRestDayOTPay += Me.RestDayOTPay * allowanceRate
        End If
    End Sub

End Class