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
        Me.ActualHourlyRate = AccuMath.CommercialRound(PayrollTools.GetHourlyRateByDailyRate(Me.ActualHourlyRate), 4)

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

    Public Sub Compute(
                payRate As OvertimeRate,
                actualSalaryPolicy As ActualTimeEntryPolicy,
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
                absentHours As Decimal)

        Dim allowanceRate = If(
            If(Me.Salary?.BasicSalary, 0) = 0,
            0D,
            If(Me.Salary?.AllowanceSalary, 0) / If(Me.Salary?.BasicSalary, 0))

        Me.RegularHours = regularHours
        Me.RegularPay = Me.RegularHours * Me.HourlyRate
        Me.ActualRegularPay = Me.RegularHours * Me.ActualHourlyRate

        Me.OvertimeHours = overtimeHours
        Me.OvertimePay = Me.OvertimeHours * Me.HourlyRate * payRate.Overtime.Rate
        Me.ActualOvertimePay = Me.OvertimePay
        If actualSalaryPolicy.AllowanceForOvertime Then
            Me.ActualOvertimePay += Me.OvertimePay * allowanceRate
        End If

        Me.NightDiffHours = nightDiffHours
        Me.NightDiffPay = Me.NightDiffHours * Me.HourlyRate * payRate.NightDifferential.Rate
        Me.ActualNightDiffPay = Me.NightDiffPay
        If actualSalaryPolicy.AllowanceForNightDiff Then
            Me.ActualNightDiffPay += Me.NightDiffPay * allowanceRate
        End If

        Me.NightDiffOvertimeHours = nightDiffOvertimeHours
        Me.NightDiffOvertimePay = Me.NightDiffOvertimeHours * Me.HourlyRate * payRate.NightDifferentialOvertime.Rate
        Me.ActualNightDiffOTPay = Me.NightDiffOvertimePay
        If actualSalaryPolicy.AllowanceForNightDiffOT Then
            Me.ActualNightDiffOTPay += Me.NightDiffOvertimePay * allowanceRate
        End If
        'TODO ND Holiday, ND Special Holiday

        Me.RestDayHours = restDayHours
        Me.RestDayPay = Me.RestDayHours * Me.HourlyRate * payRate.RestDay.Rate
        Me.ActualRestDayPay = Me.RestDayPay
        If actualSalaryPolicy.AllowanceForRestDay Then
            Me.ActualRestDayPay += Me.RestDayPay * allowanceRate
        End If
        'TODO ND Holiday OT, ND Special Holiday OT

        Me.RestDayOTHours = restDayOTHours
        Me.RestDayOTPay = Me.RestDayOTHours * Me.HourlyRate * payRate.RestDayOvertime.Rate
        Me.ActualRestDayOTPay = Me.RestDayOTPay
        If actualSalaryPolicy.AllowanceForRestDay Then
            Me.ActualRestDayOTPay += Me.RestDayOTPay * allowanceRate
        End If

        Me.RestDayOTHours = restDayOTHours
        Me.RestDayOTPay = Me.RestDayOTHours * Me.HourlyRate * payRate.RestDayOvertime.Rate
        Me.ActualRestDayOTPay = Me.RestDayOTPay

        'Holidays
        Me.SpecialHolidayHours = specialHolidayHours
        Me.SpecialHolidayPay = Me.SpecialHolidayHours * Me.HourlyRate * payRate.SpecialHoliday.Rate
        Me.ActualSpecialHolidayPay = Me.SpecialHolidayPay

        Me.SpecialHolidayOTHours = specialHolidayOTHours
        Me.SpecialHolidayOTPay = Me.SpecialHolidayOTHours * Me.HourlyRate * payRate.SpecialHolidayOvertime.Rate
        Me.ActualSpecialHolidayOTPay = Me.SpecialHolidayOTPay

        Me.RegularHolidayHours = regularHolidayHours
        Me.RegularHolidayPay = Me.RegularHolidayHours * Me.HourlyRate * payRate.RegularHoliday.Rate
        Me.ActualRegularHolidayPay = Me.RegularHolidayPay

        Me.RegularHolidayOTHours = regularHolidayOTHours
        Me.RegularHolidayOTPay = Me.RegularHolidayOTHours * Me.HourlyRate * payRate.RegularHolidayOvertime.Rate
        Me.ActualRegularHolidayOTPay = Me.RegularHolidayOTPay

        If actualSalaryPolicy.AllowanceForHoliday Then
            Me.ActualSpecialHolidayPay += Me.SpecialHolidayPay * allowanceRate
            Me.ActualSpecialHolidayOTPay += Me.SpecialHolidayOTPay * allowanceRate
            Me.ActualRegularHolidayPay += Me.RegularHolidayPay * allowanceRate
            Me.ActualRegularHolidayOTPay += Me.RegularHolidayOTPay * allowanceRate
        End If
        ''End of holidays

        Me.LeaveHours = leaveHours
        Me.LeavePay = Me.LeaveHours * Me.HourlyRate
        Me.ActualLeavePay = Me.LeaveHours * Me.ActualHourlyRate

        Me.LateHours = lateHours
        Me.LateDeduction = Me.LateHours * Me.HourlyRate
        Me.ActualLateDeduction = Me.LateHours * Me.ActualHourlyRate

        Me.UndertimeHours = undertimeHours
        Me.UndertimeDeduction = Me.UndertimeHours * Me.HourlyRate
        Me.ActualUndertimeDeduction = Me.UndertimeHours * Me.ActualHourlyRate

        Me.AbsentHours = absentHours
        Me.AbsenceDeduction = Me.AbsentHours * Me.HourlyRate
        Me.ActualAbsenceDeduction = Me.AbsentHours * Me.ActualHourlyRate

    End Sub

End Class