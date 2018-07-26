Option Strict On

Imports System.Data.Entity
Imports AccuPay.Entity
Imports Microsoft.EntityFrameworkCore

Public Class TimeEntryGenerator

    Private _context As PayrollContext

    Private _cutOffBegin As Date

    Private _cutOffEnd As Date

    Public Sub Start()
        Dim employees =
            (From e In _context.Employees
             Where e.OrganizationID = z_OrganizationID).
            ToList()

        Dim payRates =
            (From p In _context.PayRates
             Where p.OrganizationID = z_OrganizationID And
                 p.RateDate >= _cutOffBegin And
                 p.RateDate <= _cutOffEnd).
            ToList()

        For Each employee In employees
            CalculateEmployee(employee)
        Next
    End Sub

    Private Sub CalculateEmployee(employee As Employee)
        Dim shiftSchedules =
            (From s In _context.ShiftSchedules.
                 Include(Function(s) s.Shift)
             Where s.EmployeeID = employee.RowID And
                 s.EffectiveFrom <= _cutOffEnd And
                 s.EffectiveTo >= _cutOffBegin).
            ToList()
    End Sub

End Class
