Option Strict On

Public Class JobPointsForm
    Implements JobPointsView

    Public Event JobPointsForm_OnLoad() Implements JobPointsView.OnLoad

    Public Event JobPointsForm_EmployeeSelected(employee As Employee) Implements JobPointsView.EmployeeSelected

    Public Sub New()
        InitializeComponent()
        Dim presenter = New JobPointsPresenter(Me)
    End Sub

    Private Sub JobPointsForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        InitializeComponents()
        RaiseEvent JobPointsForm_OnLoad()
    End Sub

    Private Sub InitializeComponents()
        EmployeeDataGridView.AutoGenerateColumns = False
    End Sub

    Public Sub LoadEmployees(employees As ICollection(Of Employee)) Implements JobPointsView.LoadEmployees
        EmployeeDataGridView.DataSource = employees.
            Select(Function(e) New EmployeeModel(e)).
            ToList()
    End Sub

    Private Sub EmployeeDataGridView_SelectionChanged(sender As Object, e As EventArgs) Handles EmployeeDataGridView.SelectionChanged
        Dim employeeModel = DirectCast(EmployeeDataGridView.CurrentRow.DataBoundItem, EmployeeModel)

        If employeeModel Is Nothing Then
            Return
        End If

        RaiseEvent JobPointsForm_EmployeeSelected(employeeModel.Employee)
    End Sub

    Private Class EmployeeModel

        Private _employee As Employee

        Public Sub New(employee As Employee)
            _employee = employee
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

        Public ReadOnly Property Name As String
            Get
                Return $"{_employee.LastName}, {_employee.FirstName} {_employee.MiddleInitial}"
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

        Public ReadOnly Property CurrentLevel As String
            Get
                Return _employee.Position?.JobLevel?.Name
            End Get
        End Property

        Public ReadOnly Property RecommendedLevel As String
            Get
                Return String.Empty
            End Get
        End Property

    End Class

End Class

Public Interface JobPointsView

    Event OnLoad()

    Event EmployeeSelected(employee As Employee)

    Sub LoadEmployees(employees As ICollection(Of Employee))

End Interface
