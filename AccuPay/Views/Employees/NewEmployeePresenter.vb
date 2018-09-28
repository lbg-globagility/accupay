Option Strict On

Imports AccuPay.Entity
Imports Microsoft.EntityFrameworkCore
Imports System.Threading.Tasks

Public Class NewEmployeePresenter

    Private _employees As IList(Of Employee)

    Private WithEvents _view As NewEmployeeForm

    Public Sub New(view As NewEmployeeForm)
        _view = view
    End Sub

    Private Async Sub OnInit() Handles _view.Init
        _employees = Await GetEmployees()

        Dim employees = Await FilterEmployees(_view.Term)
        _view.SetEmployees(employees)
    End Sub

    Private Async Sub OnActiveChanged() Handles _view.ActiveChanged
        _employees = Await GetEmployees()

        Dim employees = Await FilterEmployees(_view.Term)
        _view.SetEmployees(employees)
    End Sub

    Private Async Sub OnSearch() Handles _view.Search
        Dim employees = Await FilterEmployees(_view.Term)
        _view.SetEmployees(employees)
    End Sub

    Private Async Sub OnRefresh() Handles _view.EmployeeRefresh
        _employees = Await GetEmployees()

        Dim employees = Await FilterEmployees(_view.Term)
        _view.SetEmployees(employees)
    End Sub

    Private Async Sub OnEmployeeSelected(employeeId As Integer?) Handles _view.EmployeeSelected
        Dim employee As Employee = Nothing

        Using context = New PayrollContext()
            employee = Await context.Employees.FindAsync(employeeId)
        End Using

        If employee IsNot Nothing Then
            _view.SetEmployee(employee)
        End If
    End Sub

    Private Async Function FilterEmployees(term As String) As Task(Of IList(Of Employee))
        term = term.ToLower()

        Dim matchCriteria =
            Function(employee As Employee) As Boolean
                Dim containsEmployeeId = employee.EmployeeNo.ToLower().Contains(term)
                Dim containsFullName = employee.Fullname.ToLower().Contains(term)

                Dim reverseFullName = employee.LastName.ToLower() + " " + employee.FirstName.ToLower()
                Dim containsFullNameInReverse = reverseFullName.Contains(term)

                Return containsEmployeeId Or containsFullName Or containsFullNameInReverse
            End Function

        Dim filterTask = Task.Factory.StartNew(
            Function() New List(Of Employee)(
                _employees.Where(matchCriteria).ToList()))

        Return Await filterTask
    End Function

    Public Async Function GetEmployees() As Task(Of IList(Of Employee))
        Return Await Task.Run(
            Function()
                Using context = New PayrollContext()
                    Dim query = context.Employees.
                        Where(Function(e) Nullable.Equals(e.OrganizationID, z_OrganizationID))

                    If _view.IsActive Then
                        query = query.Where(Function(e) e.EmploymentStatus <> "Resigned" And e.EmploymentStatus <> "Terminated")
                    End If

                    Return query.
                        OrderBy(Function(e) e.LastName).
                        ThenBy(Function(e) e.FirstName).ToList()
                End Using
            End Function)
    End Function

End Class
