Option Strict On

Imports AccuPay.Core.Entities
Imports AccuPay.Core.Interfaces
Imports Microsoft.Extensions.DependencyInjection

Namespace Global.AccuPay.JobLevels

    ''' <summary>
    ''' This form is tightly coupled to PayrolContext. PayrollContext is now transferred to another library
    ''' and encapsulated by repositories so this form will not be able to directly access it.
    ''' THIS FORM WILL NOT WORK until this is updated to use repositories.
    ''' This form is created for Hyundai which is not an Active client currently.
    ''' </summary>
    Public Class JobPointsPresenter

        Private WithEvents _view As IJobPointsView

        'Private _context As PayrollContext

        Private _currentEmployee As EmployeeModel

        Private _jobPoints As ICollection(Of JobPoint)

        Private _jobLevels As ICollection(Of JobLevel)

        Private _employeeRepo As IEmployeeRepository

        Private _salaryRepo As ISalaryRepository

        Private _jobLevelRepo As IJobLevelRepository

        Public Sub New(view As IJobPointsView)
            _view = view

            _employeeRepo = MainServiceProvider.GetRequiredService(Of IEmployeeRepository)

            _salaryRepo = MainServiceProvider.GetRequiredService(Of ISalaryRepository)

            _jobLevelRepo = MainServiceProvider.GetRequiredService(Of IJobLevelRepository)

        End Sub

        Private Async Sub OnLoad() Handles _view.OnLoad
            '_context?.Dispose()
            '_context = New PayrollContext()
            _jobLevels = GetJobLevels()

            Dim models = Await GetEmployeeModels()
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
            '_context.SaveChanges()
            _view.ShowSavedMessage()
        End Sub

        Private Function GetJobLevels() As ICollection(Of JobLevel)
            Return _jobLevelRepo.GetAll(z_OrganizationID).ToList
        End Function

        Private Async Function GetEmployeeModels() As Threading.Tasks.Task(Of ICollection(Of EmployeeModel))

            Dim fetchEmployees = Await _employeeRepo.GetAllActiveAsync(z_OrganizationID)
            Dim salaries = Await _salaryRepo.GetAllAsync(z_OrganizationID)

            Dim employees =
                (From e In fetchEmployees
                 From s In salaries.Where(Function(s) CBool(e.RowID = s.EmployeeID)).DefaultIfEmpty()
                 From s2 In salaries.Where(Function(s2) CBool(
                     s2.EmployeeID = s.EmployeeID And
                     s.EffectiveFrom < s2.EffectiveFrom
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
                Return _salary?.TotalSalary
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
