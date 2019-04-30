Option Strict On

Imports AccuPay.Entity

Public Class SemiMonthlyAllowanceCalculator

    Private _employee As Employee

    Private _paystub As Paystub

    Private _payperiod As PayPeriod


    Private ReadOnly _payrateCalendar As PayratesCalendar

    Private _timeEntries As ICollection(Of TimeEntry)

    Private ReadOnly _allowancePolicy As AllowancePolicy

    Public Sub New(allowancePolicy As AllowancePolicy, employee As Employee, paystub As Paystub, payperiod As PayPeriod, payrateCalendar As PayratesCalendar, timeEntries As ICollection(Of TimeEntry))
        _employee = employee
        _paystub = paystub
        _payperiod = payperiod
        _payrateCalendar = payrateCalendar
        _timeEntries = timeEntries
        _allowancePolicy = allowancePolicy
    End Sub

    Public Function Calculate(allowance As Allowance) As AllowanceItem
        Dim workDaysPerYear = _employee.WorkDaysPerYear
        Dim workingDays = CDec(workDaysPerYear / CalendarConstants.MonthsInAYear / CalendarConstants.SemiMonthlyPayPeriodsPerMonth)
        Dim dailyRate = allowance.Amount / workingDays

        Dim allowanceItem = New AllowanceItem() With {
            .OrganizationID = z_OrganizationID,
            .CreatedBy = z_User,
            .LastUpdBy = z_User,
            .Paystub = _paystub,
            .PayPeriodID = _payperiod.RowID,
            .AllowanceID = allowance.RowID,
            .Amount = allowance.Amount
        }

        For Each timeEntry In _timeEntries
            Dim divisor = PayrollTools.DivisorToDailyRate
            Dim hourlyRate = dailyRate / divisor

            Dim deductionHours =
                timeEntry.LateHours +
                timeEntry.UndertimeHours +
                timeEntry.AbsentHours
            Dim deductionAmount = -(hourlyRate * deductionHours)

            Dim additionalAmount = 0D

            Dim payRate = _payrateCalendar.Find(timeEntry.Date)

            If _allowancePolicy.IsSpecialHolidayPaid Then

                If (payRate.IsSpecialNonWorkingHoliday And _employee.CalcSpecialHoliday) Then
                    additionalAmount = timeEntry.SpecialHolidayHours * hourlyRate * (payRate.CommonRate - 1D)
                End If

            End If

            If _allowancePolicy.IsRegularHolidayPaid Then

                If (payRate.IsRegularHoliday And _employee.CalcHoliday) Then
                    additionalAmount = timeEntry.RegularHolidayHours * hourlyRate * (payRate.CommonRate - 1D)
                End If

            End If

            If _allowancePolicy.IsHolidayPaid Then

                If (payRate.IsSpecialNonWorkingHoliday And _employee.CalcSpecialHoliday) Or
                   (payRate.IsRegularHoliday And _employee.CalcHoliday) Then

                    additionalAmount = timeEntry.RegularHours * hourlyRate * (payRate.CommonRate - 1D)

                End If
            End If

            allowanceItem.AddPerDay(timeEntry.Date, deductionAmount + additionalAmount)
        Next

        Return allowanceItem
    End Function

End Class
