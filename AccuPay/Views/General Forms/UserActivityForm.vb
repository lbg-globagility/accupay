Option Strict On

Imports AccuPay.Data.Entities
Imports AccuPay.Data.Repositories
Imports Microsoft.Extensions.DependencyInjection

Public Class UserActivityForm
    Public Property _type As String
    Public Property _userActivityRepository As UserActivityRepository

    Public Sub New(type As String)

        InitializeComponent()

        _type = type

        _userActivityRepository = MainServiceProvider.GetRequiredService(Of UserActivityRepository)

    End Sub

    Private Class ActivityItem

        Public Property Name As String
        Public Property Description As String
        Public Property [Date] As Date

        Public ReadOnly Property DateAndTime As String
            Get
                Return [Date].ToString("MMM dd, yyyy hh:mm tt")
            End Get
        End Property

    End Class

    Private Sub UserActivityForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        DataGridView1.AutoGenerateColumns = False
        Me.Text = _type + " " + Me.Text
        populateDataGrid()

    End Sub

    Public Sub populateDataGrid()

        Dim userActivities = _userActivityRepository.GetAll(z_OrganizationID, _type)
        Dim list As New List(Of ActivityItem)

        For Each activity In userActivities
            For Each item In activity.ActivityItems
                list.Add(New ActivityItem With {
                    .Name = activity.User.LastName + ", " + activity.User.FirstName,
                    .Description = item.Description,
                    .Date = item.Created
                })
            Next
        Next

        DataGridView1.DataSource = list.OrderByDescending(Function(a) a.Date).ToList

    End Sub

    Private Sub CloseButton_Click(sender As Object, e As EventArgs) Handles CloseButton.Click
        Close()
    End Sub

End Class