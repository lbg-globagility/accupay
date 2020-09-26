Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Data.Repositories
Imports Microsoft.Extensions.DependencyInjection

Public Class UserActivityForm

    Private ReadOnly _changedType As ChangedType

    Private ReadOnly _entityName As String

    Private ReadOnly _userActivityRepository As UserActivityRepository

    Public Enum ChangedType
        Employee
        User
        Organization
        Division
        Position
    End Enum

    Public Sub New(entityName As String, Optional changedType As ChangedType = ChangedType.Employee)

        InitializeComponent()

        _entityName = entityName
        _changedType = changedType
        _userActivityRepository = MainServiceProvider.GetRequiredService(Of UserActivityRepository)

    End Sub

    Private Async Sub UserActivityForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        DataGridView1.AutoGenerateColumns = False
        Me.Text = _entityName + " " + Me.Text

        Select Case _changedType
            Case ChangedType.Employee
                ChangedEntityColumn.HeaderText = "Employee"
            Case ChangedType.User
                ChangedEntityColumn.HeaderText = "User"
            Case Else
                ChangedEntityColumn.HeaderText = String.Empty
                ChangedEntityColumn.Visible = False

        End Select

        Await PopulateDataGrid()

    End Sub

    Public Async Function PopulateDataGrid() As Task

        Dim userActivities = Await _userActivityRepository.GetAllAsync(z_OrganizationID, _entityName)
        Dim list As New List(Of ActivityItem)

        For Each activity In userActivities
            For Each item In activity.ActivityItems

                Dim changedEntity = String.Empty

                Select Case _changedType
                    Case ChangedType.Employee
                        changedEntity = item.ChangedEmployee?.FullNameLastNameFirst
                    Case ChangedType.User
                        changedEntity = item.ChangedUser?.FullNameLastNameFirst

                End Select

                list.Add(New ActivityItem With {
                    .ChangedBy = activity.User.LastName + ", " + activity.User.FirstName,
                    .ChangedEntity = changedEntity,
                    .Description = item.Description,
                    .Date = item.Created
                })
            Next
        Next

        DataGridView1.DataSource = list.OrderByDescending(Function(a) a.Date).ToList

    End Function

    Private Sub CloseButton_Click(sender As Object, e As EventArgs) Handles CloseButton.Click
        Close()
    End Sub

    Private Class ActivityItem

        Public Property ChangedBy As String
        Public Property ChangedEntity As String
        Public Property Description As String
        Public Property [Date] As Date

        Public ReadOnly Property DateAndTime As String
            Get
                Return [Date].ToString("MMM dd, yyyy hh:mm tt")
            End Get
        End Property

    End Class

End Class