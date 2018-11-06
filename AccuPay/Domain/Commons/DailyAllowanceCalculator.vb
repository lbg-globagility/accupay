Option Strict On

Imports AccuPay.Entity

Public Class DailyAllowanceCalculator

    Private _settings As ListOfValueCollection

    Private _previousTimeEntries2 As ICollection(Of TimeEntry)

    Private _payrates As IDictionary(Of Date, PayRate)

    Public Sub New(settings As ListOfValueCollection, payrates As IDictionary(Of Date, PayRate), previousTimeEntries2 As ICollection(Of TimeEntry))
        _settings = settings
        _payrates = payrates
        _previousTimeEntries2 = previousTimeEntries2
    End Sub

    Public Function Compute(payperiod As PayPeriod, allowance As Allowance, employee As Employee, paystub As Paystub, timeEntries As IList(Of TimeEntry)) As AllowanceItem
        Dim dailyRate = allowance.Amount

        Dim allowanceItem = New AllowanceItem() With {
            .OrganizationID = z_OrganizationID,
            .CreatedBy = z_User,
            .LastUpdBy = z_User,
            .Paystub = paystub,
            .PayPeriodID = payperiod.RowID,
            .AllowanceID = allowance.RowID
        }

        For Each timeEntry In timeEntries
            If Not (allowance.EffectiveStartDate <= timeEntry.Date And timeEntry.Date <= allowance.EffectiveEndDate) Then
                Continue For
            End If

            Dim divisor = If(timeEntry.ShiftSchedule?.Shift?.DivisorToDailyRate, 8D)
            Dim hourlyRate = dailyRate / divisor

            Dim amount = 0D
            Dim payRate = _payrates(timeEntry.Date)
            If payRate.IsRegularDay Then
                Dim isRestDay = timeEntry.RestDayHours > 0

                If isRestDay Then
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
                    HasWorkedLastWorkingDay(timeEntry) Or
                    ((employee.IsFixed Or employee.IsMonthly) And exemption)

                If giveAllowance Then
                    If _settings.GetString("AllowancePolicy.CalculationType") = "Hourly" Then
                        Dim workHours = If(timeEntry.ShiftSchedule?.Shift?.WorkHours, 8D)

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

    Private Function HasWorkedLastWorkingDay(current As TimeEntry) As Boolean
        Dim lastPotentialEntry = current.Date.AddDays(-3)

        Dim lastTimeEntries = _previousTimeEntries2.
            Where(Function(t) lastPotentialEntry <= t.Date And t.Date <= current.Date).
            Reverse().
            ToList()

        For Each lastTimeEntry In lastTimeEntries
            ' If employee has no shift set for the day, it's not a working day.
            If lastTimeEntry?.ShiftSchedule?.Shift Is Nothing Then
                Continue For
            End If

            If lastTimeEntry?.ShiftSchedule.IsRestDay Then
                If lastTimeEntry.TotalDayPay > 0 Then
                    Return True
                End If

                Continue For
            End If

            Dim payRate = _payrates(lastTimeEntry.Date)
            If payRate.IsHoliday Then
                If lastTimeEntry.TotalDayPay > 0 Then
                    Return True
                End If

                Continue For
            End If

            Return lastTimeEntry.RegularHours > 0 Or lastTimeEntry.TotalLeaveHours > 0
        Next

        Return False
    End Function

End Class
