Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Data.Helpers
Imports AccuPay.Data.Repositories
Imports Microsoft.Extensions.DependencyInjection

Public Class UserActivityForm

    Private ReadOnly _changedType As ChangedType

    Private ReadOnly _entityName As String

    Private ReadOnly _userActivityRepository As UserActivityRepository

    Private _currentPageIndex As Integer

    Private _totalPages As Integer

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

        _currentPageIndex = 0
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

        Await PaginateList(_currentPageIndex)

    End Function

    Private Async Function PaginateList(pageIndex As Integer) As Task
        Dim pageSize = 100
        Dim options As New PageOptions(pageIndex, pageSize, "Created", "DESC")

        Dim list = Await _userActivityRepository.GetPaginatedListAsync(options, z_OrganizationID, _entityName)
        Dim acitivityItems As New List(Of ActivityItem)

        For Each item In list.Items

            Dim changedEntity = String.Empty

            Select Case _changedType
                Case ChangedType.Employee
                    changedEntity = item.ChangedEmployee?.FullNameLastNameFirst
                Case ChangedType.User
                    changedEntity = item.ChangedUser?.FullNameLastNameFirst

            End Select

            acitivityItems.Add(New ActivityItem With {
                .ChangedBy = item.Activity.User.LastName + ", " + item.Activity.User.FirstName,
                .ChangedEntity = changedEntity,
                .Description = item.Description,
                .Date = item.Created
            })

        Next

        DataGridView1.DataSource = acitivityItems

        _currentPageIndex = pageIndex
        _totalPages = list.TotalPages

        DisablePaginateButtons()

    End Function

    Private Sub DisablePaginateButtons()
        FirstLinkLabel.Enabled = True
        PreviousLinkLabel.Enabled = True
        NextLinkLabel.Enabled = True
        LastLinkLabel.Enabled = True

        If _currentPageIndex = 0 Then

            FirstLinkLabel.Enabled = False
            PreviousLinkLabel.Enabled = False
        End If

        If (_currentPageIndex + 1) >= _totalPages Then

            NextLinkLabel.Enabled = False
            LastLinkLabel.Enabled = False
        End If
    End Sub

    Private Async Sub Pagination_Click(sender As Object, e As EventArgs) Handles PreviousLinkLabel.Click, NextLinkLabel.Click, LastLinkLabel.Click, FirstLinkLabel.Click

        Dim updatedPageIndex = GetFirstPageIndex()

        If sender Is FirstLinkLabel Then

            updatedPageIndex = GetFirstPageIndex()

        ElseIf sender Is PreviousLinkLabel Then

            updatedPageIndex = _currentPageIndex - 1

        ElseIf sender Is NextLinkLabel Then

            updatedPageIndex = _currentPageIndex + 1

        ElseIf sender Is LastLinkLabel Then

            updatedPageIndex = GetLastPageIndex()
        End If

        If updatedPageIndex < 0 Then updatedPageIndex = GetFirstPageIndex()
        If updatedPageIndex > _totalPages Then updatedPageIndex = GetLastPageIndex()

        FirstLinkLabel.Enabled = False
        PreviousLinkLabel.Enabled = False
        NextLinkLabel.Enabled = False
        LastLinkLabel.Enabled = False

        Await PaginateList(updatedPageIndex)

    End Sub

    Private Shared Function GetFirstPageIndex() As Integer
        Return 0
    End Function

    Private Function GetLastPageIndex() As Integer
        Return _totalPages - 1
    End Function

    Private Sub CloseButton_Click(sender As Object, e As EventArgs) Handles CloseButton.Click
        Me.Close()
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