Option Strict On

Imports AccuPay.Entity

Public Class DailyAllowanceCalculator

    Private _settings As ListOfValueCollection

    Private _previousTimeEntries As ICollection(Of TimeEntry)

    Private ReadOnly _payrateCalendar As PayratesCalendar

    Public Sub New(settings As ListOfValueCollection, payrateCalendar As PayratesCalendar, previousTimeEntries2 As ICollection(Of TimeEntry))
        _settings = settings
        _payrateCalendar = payrateCalendar
        _previousTimeEntries = previousTimeEntries2
    End Sub

    Public Function Compute(payperiod As PayPeriod, allowance As Allowance, employee As Employee, paystub As Paystub, timeEntries As ICollection(Of TimeEntry)) As AllowanceItem
        Dim dailyRate = allowance.Amount

        Dim allowanceItem = PayrollGeneration.CreateBasicAllowanceItem(
                                                paystub:=paystub,
                                                payperiodId:=payperiod.RowID,
                                                allowanceId:=allowance.RowID
                                            )

        For Each timeEntry In timeEntries
            If Not (allowance.EffectiveStartDate <= timeEntry.Date And timeEntry.Date <= allowance.EffectiveEndDate) Then
                Continue For
            End If

            Dim divisor = PayrollTools.DivisorToDailyRate
            Dim hourlyRate = dailyRate / divisor

            Dim amount = 0D
            Dim payRate = _payrateCalendar.Find(timeEntry.Date)
            If payRate.IsRegularDay Then
                Dim isRestDay = timeEntry.RestDayHours > 0

                If isRestDay Then
                    amount = dailyRate
                ElseIf allowance.Product.Fixed Then
                    amount = dailyRate
                Else
                    amount = (timeEntry.RegularHours + timeEntry.TotalLeaveHours) * hourlyRate
                End If
            ElseIf payRate.IsSpecialNonWorkingHoliday Then
                Dim countableHours = timeEntry.RegularHours + timeEntry.SpecialHolidayHours + timeEntry.TotalLeaveHours

                amount = If(countableHours > 0, dailyRate, 0D)
            ElseIf payRate.IsRegularHoliday Then
                amount = (timeEntry.RegularHours + timeEntry.RegularHolidayHours) * hourlyRate

                Dim exemption = _settings.GetBoolean("AllowancePolicy.HolidayAllowanceForMonthly")

                Dim giveAllowance =
                    PayrollTools.HasWorkedLastWorkingDay(timeEntry.Date, CType(_previousTimeEntries, IList(Of TimeEntry)), _payrateCalendar) Or
                    ((employee.IsFixed Or employee.IsMonthly) And exemption)

                If giveAllowance Then
                    If _settings.GetString("AllowancePolicy.CalculationType") = "Hourly" Then
                        Dim workHours = If(timeEntry.HasShift, timeEntry.WorkHours, PayrollTools.WorkHoursPerDay)

                        amount += {workHours * hourlyRate, dailyRate}.Max()
                    Else
                        amount += dailyRate
                    End If
                End If
            End If

            allowanceItem.AddPerDay(timeEntry.Date, amount)
        Next

        Return allowanceItem
    End Function

End Class
