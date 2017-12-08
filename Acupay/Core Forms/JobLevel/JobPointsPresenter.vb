Option Strict On

Imports System.Data.Entity
Imports Acupay
Imports PayrollSys

Namespace Global.AccuPay.JobLevels

    Public Class JobPointsPresenter

        Private WithEvents _view As JobPointsView

        Private _context As PayrollContext

        Private _currentEmployee As EmployeeModel

        Private _jobPoints As ICollection(Of JobPoint)

        Private _jobLevels As ICollection(Of JobLevel)

        Public Sub New(view As JobPointsView)
            _view = view
        End Sub

        Private Sub OnLoad() Handles _view.OnLoad
            _context?.Dispose()
            _context = New PayrollContext()
            _jobLevels = GetJobLevels()

            Dim models = GetEmployeeModels()
            For Each model In models
                SetRecommendedLevel(model)
            Next

            _view.LoadEmployees(models)
        End Sub

        Private Sub OnEmployeeSelected(employee As EmployeeModel) Handles _view.EmployeeSelected
            _currentEmployee = employee
        End Sub

        Private Sub OnEmployeeChanged(model As EmployeeModel) Handles _view.EmployeeChanged
            SetRecommendedLevel(model)
        End Sub

        Private Sub SetRecommendedLevel(model As EmployeeModel)
            Dim employee = model.Employee
            Dim currentLevel = employee.Position?.JobLevel

            If currentLevel Is Nothing Then
                Return
            End If

            If employee.AdvancementPoints > currentLevel.Points Then
                Dim category = currentLevel.JobCategory

                Dim recommendedLevel = _jobLevels.
                    Where(Function(j) CBool(j.JobCategory.RowID = category.RowID)).
                    OrderByDescending(Function(j) j.Points).
                    First(Function(j) employee.AdvancementPoints >= j.Points)

                model.SetRecommendedLevel(recommendedLevel)
            End If
        End Sub

        Private Sub SaveEmployees() Handles _view.SaveEmployees
            _context.SaveChanges()
            _view.ShowSavedMessage()
        End Sub

        Private Function GetJobLevels() As ICollection(Of JobLevel)
            Return _context.JobLevels.
                Where(Function(j) CBool(j.OrganizationID = z_OrganizationID)).
                ToList()
        End Function

        Private Function GetEmployeeModels() As ICollection(Of EmployeeModel)
            Dim employees =
                (From e In _context.Employees
                 From s In _context.Salaries.Where(Function(s) CBool(e.RowID = s.EmployeeID)).DefaultIfEmpty()
                 From s2 In _context.Salaries.Where(Function(s2) CBool(
                     s2.EmployeeID = s.EmployeeID And
                     s.EffectiveDateFrom < s2.EffectiveDateFrom
                 )).DefaultIfEmpty()
                 Where e.OrganizationID = z_OrganizationID And
                     s2.RowID Is Nothing
                 Order By e.LastName, e.FirstName
                 Select New With {.Employee = e, .Salary = s}).
                    ToList()

            Return employees.Select(Function(e) New EmployeeModel(e.Employee, e.Salary)).ToList()
        End Function

    End Class

    Public Class EmployeeModel

        Private _employee As Employee

        Private _salary As Salary

        Private _recommendedLevel As JobLevel

        Public Sub New(employee As Employee, salary As Salary)
            _employee = employee
            _salary = salary
        End Sub

        Public ReadOnly Property Employee As Employee
            Get
                Return _employee
            End Get
        End Property

        Public ReadOnly Property EmployeeNo As String
            Get
                Return _employee.EmployeeNo
            End Get
        End Property

        Public ReadOnly Property LastName As String
            Get
                Return _employee.LastName
            End Get
        End Property

        Public ReadOnly Property FirstName As String
            Get
                Return _employee.FirstName
            End Get
        End Property

        Public Property Points As Integer
            Get
                Return _employee.AdvancementPoints
            End Get
            Set(value As Integer)
                _employee.AdvancementPoints = value
            End Set
        End Property

        Public ReadOnly Property CurrentPosition As String
            Get
                Return _employee.Position?.Name
            End Get
        End Property

        Public ReadOnly Property Division As String
            Get
                Return _employee.Position?.Division.Name
            End Get
        End Property

        Public ReadOnly Property CurrentLevel As String
            Get
                Return _employee.Position?.JobLevel?.Name
            End Get
        End Property

        Public Sub SetRecommendedLevel(level As JobLevel)
            _recommendedLevel = level
        End Sub

        Public ReadOnly Property RecommendedLevel As String
            Get
                Return _recommendedLevel?.Name
            End Get
        End Property

        Public ReadOnly Property CurrentSalary As Decimal?
            Get
                Return _salary?.TrueSalary
            End Get
        End Property

        Public ReadOnly Property RecommendedSalary As String
            Get
                Dim s1 = _recommendedLevel?.SalaryRangeFrom.ToString("N2")
                Dim s2 = _recommendedLevel?.SalaryRangeTo.ToString("N2")

                Return $"{s1} - {s2}"
            End Get
        End Property

    End Class

End Namespace