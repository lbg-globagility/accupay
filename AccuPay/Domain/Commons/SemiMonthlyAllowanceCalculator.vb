Option Strict On

Imports AccuPay.Entity

Public Class SemiMonthlyAllowanceCalculator

    Private _employee As Employee

    Private _paystub As Paystub

    Private _payperiod As PayPeriod

    Private _payRates As IReadOnlyDictionary(Of Date, PayRate)

    Private _timeEntries As ICollection(Of TimeEntry)

    Private _settings As ListOfValueCollection

    Public Sub New(settings As ListOfValueCollection, employee As Employee, paystub As Paystub, payperiod As PayPeriod, payrates As IReadOnlyDictionary(Of Date, PayRate), timeEntries As ICollection(Of TimeEntry))
        _employee = employee
        _paystub = paystub
        _payperiod = payperiod
        _payRates = payrates
        _timeEntries = timeEntries
        _settings = settings
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
            Dim divisor = If(timeEntry.ShiftSchedule?.Shift?.DivisorToDailyRate, 8D)
            Dim hourlyRate = dailyRate / divisor

            Dim deductionHours =
                timeEntry.LateHours +
                timeEntry.UndertimeHours +
                timeEntry.AbsentHours
            Dim deductionAmount = -(hourlyRate * deductionHours)

            Dim additionalAmount = 0D
            Dim giveAllowanceDuringHolidays = _settings.GetBoolean("Payroll Policy", "allowances.holiday")
            If giveAllowanceDuringHolidays Then
                Dim payRate = _payRates(timeEntry.Date)

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
