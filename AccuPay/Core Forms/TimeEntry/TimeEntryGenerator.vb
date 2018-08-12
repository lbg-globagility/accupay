Option Strict On

Imports System.Data.Entity
Imports AccuPay.Entity
Imports AccuPay.Tools
Imports Microsoft.EntityFrameworkCore

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

        End Using

        For Each employee In employees
            CalculateEmployee(employee, organization)
        Next
    End Sub

    Private Function CalculateEmployee(employee As Employee, organization As Organization) As IList(Of TimeEntry)
        Dim dayCalculator = New DayCalculator()

        Dim timeEntries = New List(Of TimeEntry)

        For Each day In Calendar.EachDay(_cutoffStart, _cutoffEnd)
            Dim timeEntry = dayCalculator.Compute(employee, day, organization)
            timeEntries.Add(timeEntry)
        Next

        Return timeEntries
    End Function

End Class
