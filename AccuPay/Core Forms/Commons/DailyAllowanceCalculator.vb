Option Strict On

Imports AccuPay.Entity

Public Class DailyAllowanceCalculator

    Public Function Compute(_payrates As IDictionary(Of Date, PayRate), payperiod As PayPeriod, allowance As Allowance, settings As ListOfValueCollection, employee As Employee, paystub As Paystub, timeEntries As IList(Of TimeEntry)) As AllowanceItem
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

                Dim exemption = settings.GetBoolean("AllowancePolicy.HolidayAllowanceForMonthly")

                Dim giveAllowance =
                    HasWorkedLastWorkingDay(timeEntry) Or
                    ((employee.IsFixed Or employee.IsMonthly) And exemption)

                If giveAllowance Then
                    If settings.GetString("AllowancePolicy.CalculationType") = "Hourly" Then
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

    Private Function HasWorkedLastWorkingDay(timeEntry As TimeEntry) As Boolean
        Return False
    End Function

End Class
