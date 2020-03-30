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

            Dim hourlyRate = PayrollTools.GetHourlyRateByDailyRate(dailyRate)

            Dim allowanceAmount = 0D
            Dim payrate = _payrateCalendar.Find(timeEntry.Date)

            If payrate.IsRegularDay Then
                Dim isRestDay = timeEntry.RestDayHours > 0

                If isRestDay Then
                    allowanceAmount = dailyRate
                ElseIf allowance.Product.Fixed Then
                    allowanceAmount = dailyRate
                Else
                    allowanceAmount = (timeEntry.RegularHours + timeEntry.TotalLeaveHours) * hourlyRate
                End If

            ElseIf payrate.IsSpecialNonWorkingHoliday Then
                Dim countableHours = timeEntry.RegularHours + timeEntry.SpecialHolidayHours + timeEntry.TotalLeaveHours

                allowanceAmount = If(countableHours > 0, dailyRate, 0D)
            ElseIf payrate.IsRegularHoliday Then
                allowanceAmount = (timeEntry.RegularHours + timeEntry.RegularHolidayHours) * hourlyRate

                Dim exemption = _settings.GetBoolean("AllowancePolicy.HolidayAllowanceForMonthly")

                Dim giveAllowance =
                    PayrollTools.HasWorkedLastWorkingDay(timeEntry.Date, _previousTimeEntries, _payrateCalendar) Or
                    ((employee.IsFixed Or employee.IsMonthly) And exemption)

                If giveAllowance Then
                    Dim basicHolidayPay As Decimal

                    If _settings.GetString("AllowancePolicy.CalculationType") = "Hourly" Then
                        Dim workHours = If(timeEntry.HasShift, timeEntry.WorkHours, PayrollTools.WorkHoursPerDay)

                        basicHolidayPay = {workHours * hourlyRate, dailyRate}.Max()
                    Else
                        basicHolidayPay = dailyRate
                    End If

                    If payrate.IsDoubleHoliday Then
                        ' If it's a double holiday, then give the basic holiday pay twice
                        allowanceAmount += basicHolidayPay * 2
                    Else
                        allowanceAmount += basicHolidayPay
                    End If
                End If
            End If

            allowanceItem.AddPerDay(timeEntry.Date, allowanceAmount)
        Next

        Return allowanceItem
    End Function

End Class
