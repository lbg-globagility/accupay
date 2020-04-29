Option Strict On

Imports AccuPay.Data.Entities
Imports AccuPay.Data.Repositories

Public Class UserActivityForm
    Public Property type As String

    Public Sub New(type As String)
        InitializeComponent()
        Me.type = type
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
        Me.Text = type + " " + Me.Text
        populateDataGrid()

    End Sub

    Public Sub populateDataGrid()

        Dim repo As New UserActivityRepository
        Dim userActivities = repo.GetAll(z_OrganizationID, type)
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