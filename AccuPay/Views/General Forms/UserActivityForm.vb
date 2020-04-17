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
        Public Property DateAndTime As String

    End Class

    Private Sub UserActivityForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        DataGridView1.AutoGenerateColumns = False
        Me.Text = type + " " + Me.Text
        populateDataGrid()

    End Sub

    Public Sub populateDataGrid()

        Dim repo As New UserActivityRepository
        Dim userActivities = repo.List(z_OrganizationID, type)
        Dim list As New List(Of ActivityItem)

        For Each activity In userActivities
            For Each item In activity.ActivityItems
                list.Add(New ActivityItem With {
                    .Name = activity.User.LastName + ", " + activity.User.FirstName,
                    .Description = item.Description,
                    .DateAndTime = item.Created.ToString("MMM dd, yyyy hh:mm tt")
                })
            Next
        Next

        DataGridView1.DataSource = list.OrderByDescending(Function(a) a.DateAndTime).ToList

    End Sub

    Private Sub CloseButton_Click(sender As Object, e As EventArgs) Handles CloseButton.Click
        Close()
    End Sub

End Class