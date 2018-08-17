Option Strict On

Imports System.Data.Entity
Imports AccuPay.Entity
Imports AccuPay.Tools
Imports Microsoft.EntityFrameworkCore
Imports PayrollSys

Public Class TimeEntryGenerator

    Private ReadOnly _cutoffStart As Date

    Private ReadOnly _cutoffEnd As Date

    Public Sub New(cutoffStart As Date, cutoffEnd As Date)
        _cutoffStart = cutoffStart
        _cutoffEnd = cutoffEnd
    End Sub

    Public Sub Start()
        Dim employees As IList(Of Employee) = Nothing
        Dim organization As Organization = Nothing
        Dim payrateCalendar As PayratesCalendar = Nothing
        Dim settings As ListOfValueCollection = Nothing

        Using context = New PayrollContext()
            employees =
                (From e In context.Employees
                 Where e.OrganizationID = z_OrganizationID).
                ToList()

            organization = context.Organizations.
                FirstOrDefault(Function(o) Nullable.Equals(o.RowID, z_OrganizationID))

            Dim payRates =
                (From p In context.PayRates
                 Where p.OrganizationID = z_OrganizationID And
                     p.RateDate >= _cutoffStart And
                     p.RateDate <= _cutoffEnd).
                ToList()

            payrateCalendar = New PayratesCalendar(payRates)

            settings = New ListOfValueCollection(context.ListOfValues.ToList())
        End Using

        For Each employee In employees
            CalculateEmployee(employee, organization, payrateCalendar, settings)
        Next
    End Sub

    Private Function CalculateEmployee(employee As Employee, organization As Organization, payrateCalendar As PayratesCalendar, settings As ListOfValueCollection) As IList(Of TimeEntry)
        Dim dayCalculator = New DayCalculator(settings, payrateCalendar)

        Dim timeEntries = New List(Of TimeEntry)

        Dim salary As Salary = Nothing
        Using context = New PayrollContext()
            salary = context.Salaries.
                Where(Function(s) Nullable.Equals(s.EmployeeID, employee.RowID)).
                Where(Function(s) s.EffectiveFrom <= _cutoffStart And _cutoffStart <= If(s.EffectiveTo, _cutoffStart)).
                FirstOrDefault()
        End Using

        For Each day In Calendar.EachDay(_cutoffStart, _cutoffEnd)
            Dim timeEntry = dayCalculator.Compute(employee, day, organization, salary)
            timeEntries.Add(timeEntry)
        Next

        Return timeEntries
    End Function

End Class
