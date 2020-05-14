Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Data.Entities
Imports AccuPay.Data.Repositories
Imports Microsoft.Extensions.DependencyInjection

Public Class NewEmployeePresenter

    Private _currentEmployee As Employee

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

    Private Async Sub OnEmployeeSelected(employeeID As Integer?) Handles _view.EmployeeSelected

        If employeeID.HasValue = False Then

            _currentEmployee = Nothing
            Return

        End If

        Dim builder = MainServiceProvider.GetRequiredService(Of EmployeeQueryBuilder)

        _currentEmployee = Await builder.IncludePosition().
                                        IncludePayFrequency().
                                        GetByIdAsync(employeeID.Value, z_OrganizationID)

        If _currentEmployee IsNot Nothing Then
            _view.SetEmployee(_currentEmployee)
        End If
    End Sub

    Private Sub OnTabChanged() Handles _view.TabChanged
        If _currentEmployee Is Nothing Then
            Return
        End If

        _view.SetEmployee(_currentEmployee)
    End Sub

    Private Async Function FilterEmployees(term As String) As Task(Of IList(Of Employee))
        term = term.ToLower()

        Dim matchCriteria =
            Function(employee As Employee) As Boolean
                Dim containsEmployeeId = employee.EmployeeNo.ToLower().Contains(term)
                Dim containsFullName = employee.FullNameWithMiddleInitialLastNameFirst.ToLower().Contains(term)

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

                Dim builder = MainServiceProvider.GetRequiredService(Of EmployeeQueryBuilder)

                Dim employees As IEnumerable(Of Employee)

                If _view.IsActive Then
                    employees = builder.IsActive().ToList(z_OrganizationID)
                Else
                    employees = builder.ToList(z_OrganizationID)

                End If

                Return employees.
                    OrderBy(Function(e) e.LastName).
                    ThenBy(Function(e) e.FirstName).ToList()

            End Function)
    End Function

End Class