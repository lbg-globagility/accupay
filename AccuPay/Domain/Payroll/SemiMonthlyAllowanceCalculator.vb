Option Strict On

Imports AccuPay.Entity

Public Class SemiMonthlyAllowanceCalculator

    Private ReadOnly _employee As Employee

    Private ReadOnly _paystub As Paystub

    Private ReadOnly _payperiod As PayPeriod

    Private ReadOnly _calendarCollection As CalendarCollection

    Private ReadOnly _timeEntries As ICollection(Of TimeEntry)

    Private ReadOnly _allowancePolicy As AllowancePolicy

    Public Sub New(allowancePolicy As AllowancePolicy, employee As Employee, paystub As Paystub, payperiod As PayPeriod, calendarCollection As CalendarCollection, timeEntries As ICollection(Of TimeEntry))
        _employee = employee
        _paystub = paystub
        _payperiod = payperiod
        _calendarCollection = calendarCollection
        _timeEntries = timeEntries
        _allowancePolicy = allowancePolicy
    End Sub

    Public Function Calculate(allowance As Allowance) As AllowanceItem

        Dim allowanceItem = PayrollGeneration.CreateBasicAllowanceItem(
                                                paystub:=_paystub,
                                                payperiodId:=_payperiod.RowID,
                                                allowanceId:=allowance.RowID,
                                                product:=allowance.Product
                                            )
        allowanceItem.Amount = allowance.Amount

        If allowance.Product.Fixed Then
            Return allowanceItem
        End If

        Dim monthlyRate = allowance.Amount * 2
        Dim hourlyRate = PayrollTools.GetHourlyRateByMonthlyRate(monthlyRate, _employee.WorkDaysPerYear)

        For Each timeEntry In _timeEntries
            Dim deductionHours =
                timeEntry.LateHours +
                timeEntry.UndertimeHours +
                timeEntry.AbsentHours
            Dim deductionAmount = -(hourlyRate * deductionHours)

            Dim additionalAmount = 0D

            Dim payrateCalendar = _calendarCollection.GetCalendar(timeEntry.BranchID)
            Dim payrate = payrateCalendar.Find(timeEntry.Date)

            If _allowancePolicy.IsSpecialHolidayPaid Then

                If (payrate.IsSpecialNonWorkingHoliday And _employee.CalcSpecialHoliday) Then
                    additionalAmount = timeEntry.SpecialHolidayHours * hourlyRate * (payrate.RegularRate - 1D)
                End If

            End If

            If _allowancePolicy.IsRegularHolidayPaid Then

                If (payrate.IsRegularHoliday And _employee.CalcHoliday) Then
                    additionalAmount = timeEntry.RegularHolidayHours * hourlyRate * (payrate.RegularRate - 1D)
                End If

            End If

            allowanceItem.AddPerDay(timeEntry.Date, deductionAmount + additionalAmount)
        Next

        Return allowanceItem
    End Function

End Class