Option Strict On

Imports AccuPay
Imports AccuPay.Entity
Imports PayrollSys

Public Class ActualTimeEntryCalculator

    Private ReadOnly _salary As Salary
    Private ReadOnly _actualTimeEntries As IList(Of ActualTimeEntry)
    Private ReadOnly _policy As ActualTimeEntryPolicy

    Public Sub New(salary As Salary, actualTimeEntries As IList(Of ActualTimeEntry), actualTimeEntryPolicy As ActualTimeEntryPolicy)
        _salary = salary
        _actualTimeEntries = actualTimeEntries
        _policy = actualTimeEntryPolicy
    End Sub

    Public Function Compute(timeEntries As IList(Of TimeEntry)) As IList(Of ActualTimeEntry)
        Dim actualTimeEntries = New List(Of ActualTimeEntry)

        Dim allowanceRate = If(
            _salary.BasicSalary = 0,
            0D,
            _salary.AllowanceSalary / _salary.BasicSalary)

        For Each timeEntry In timeEntries
            Dim actualTimeEntry = _actualTimeEntries.FirstOrDefault(Function(t) t.Date = timeEntry.Date)

            If actualTimeEntry Is Nothing Then
                actualTimeEntry = New ActualTimeEntry() With {
                    .EmployeeID = timeEntry.EmployeeID,
                    .OrganizationID = timeEntry.OrganizationID,
                    .[Date] = timeEntry.Date
                }
            End If

            actualTimeEntry.BasicDayPay = timeEntry.BasicDayPay + (timeEntry.BasicDayPay * allowanceRate)

            actualTimeEntry.RegularPay = timeEntry.RegularPay + (timeEntry.RegularPay * allowanceRate)

            actualTimeEntry.LateDeduction = timeEntry.LateDeduction + (timeEntry.LateDeduction * allowanceRate)
            actualTimeEntry.UndertimeDeduction = timeEntry.UndertimeDeduction + (timeEntry.UndertimeDeduction * allowanceRate)
            actualTimeEntry.AbsentDeduction = timeEntry.AbsentDeduction + (timeEntry.AbsentDeduction * allowanceRate)
            actualTimeEntry.LeavePay = timeEntry.LeavePay + (timeEntry.LeavePay * allowanceRate)

            actualTimeEntry.OvertimePay = timeEntry.OvertimePay
            If _policy.AllowanceForOvertime Then
                actualTimeEntry.OvertimePay += timeEntry.OvertimePay * allowanceRate
            End If

            actualTimeEntry.NightDiffPay = timeEntry.NightDiffPay
            If _policy.AllowanceForNightDiff Then
                actualTimeEntry.NightDiffPay += timeEntry.NightDiffPay * allowanceRate
            End If

            actualTimeEntry.NightDiffOTPay = timeEntry.NightDiffOTPay
            If _policy.AllowanceForNightDiffOT Then
                actualTimeEntry.NightDiffOTPay += timeEntry.NightDiffOTPay * allowanceRate
            End If

            actualTimeEntry.RestDayPay = timeEntry.RestDayPay
            If _policy.AllowanceForRestDay Then
                actualTimeEntry.RestDayPay += timeEntry.RestDayPay * allowanceRate
            End If

            actualTimeEntry.RestDayOTPay = timeEntry.RestDayOTPay
            If _policy.AllowanceForRestDayOT Then
                actualTimeEntry.RestDayOTPay += timeEntry.RestDayOTPay * allowanceRate
            End If

            actualTimeEntry.SpecialHolidayPay = timeEntry.SpecialHolidayPay
            actualTimeEntry.SpecialHolidayOTPay = timeEntry.SpecialHolidayOTPay
            actualTimeEntry.RegularHolidayPay = timeEntry.RegularHolidayPay
            actualTimeEntry.RegularHolidayOTPay = timeEntry.RegularHolidayOTPay
            If _policy.AllowanceForHoliday Then
                actualTimeEntry.SpecialHolidayPay += timeEntry.SpecialHolidayPay
                actualTimeEntry.SpecialHolidayOTPay += timeEntry.SpecialHolidayOTPay
                actualTimeEntry.RegularHolidayPay += timeEntry.RegularHolidayPay
                actualTimeEntry.RegularHolidayOTPay += timeEntry.RegularHolidayOTPay
            End If

            actualTimeEntry.TotalDayPay = timeEntry.TotalDayPay + (timeEntry.TotalDayPay * allowanceRate)

            actualTimeEntries.Add(actualTimeEntry)
        Next

        Return actualTimeEntries
    End Function

End Class
