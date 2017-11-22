Option Strict On

Imports AccuPay.JobLevels
Imports System.Data.Entity

Public Class JobPointsPresenter

    Private WithEvents _view As JobPointsView

    Private _currentEmployee As Employee

    Private _jobPoints As ICollection(Of JobPoint)

    Public Sub New(view As JobPointsView)
        _view = view
    End Sub

    Private Sub OnLoad() Handles _view.OnLoad
        Dim employees = GetEmployeees()
        _view.LoadEmployees(employees)
    End Sub

    Private Sub OnEmployeeSelected(employee As Employee) Handles _view.EmployeeSelected
        _currentEmployee = employee
    End Sub

    'Private Function GetJobPoints(employee As Employee) As ICollection(Of JobPoint)
    '    Using context = New PayrollContext()
    '        Return context.JobPoints.
    '            Where(Function(j) CBool(j.EmployeeID = employee.RowID)).
    '            ToList()
    '    End Using
    'End Function

    Private Function GetEmployeees() As ICollection(Of Employee)
        Using context = New PayrollContext()
            Return context.Employees.
                Include(Function(e) e.Position).
                Where(Function(e) CBool(e.OrganizationID = z_OrganizationID)).
                OrderBy(Function(e) e.LastName).
                ToList()
        End Using
    End Function

End Class
