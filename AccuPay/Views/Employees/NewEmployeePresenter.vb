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
        _view.SetEmployees(_employees)
    End Sub

    Private Async Sub OnSearch(term As String) Handles _view.Search
        Dim searchValue = term.ToLower()

        Dim matchCriteria =
            Function(employee As Employee) As Boolean
                Dim containsEmployeeId = employee.EmployeeNo.ToLower().Contains(searchValue)
                Dim containsFullName = employee.Fullname.ToLower().Contains(searchValue)

                Dim reverseFullName = employee.LastName.ToLower() + " " + employee.FirstName.ToLower()
                Dim containsFullNameInReverse = reverseFullName.Contains(searchValue)

                Return containsEmployeeId Or containsFullName Or containsFullNameInReverse
            End Function

        Dim filteredTask = Task.Factory.StartNew(
            Function() New List(Of Employee)(
                _employees.Where(matchCriteria).ToList()))

        Dim filteredEmployees = Await filteredTask

        _view.SetEmployees(filteredEmployees)
    End Sub

    Public Async Function GetEmployees() As Task(Of IList(Of Employee))
        Return Await Task.Run(
            Function()
                Using context = New PayrollContext()
                    Return context.Employees.
                        Where(Function(e) Nullable.Equals(e.OrganizationID, z_OrganizationID)).
                        OrderBy(Function(e) e.LastName).
                        ThenBy(Function(e) e.FirstName).
                        ToList()
                End Using
            End Function)
    End Function

End Class
