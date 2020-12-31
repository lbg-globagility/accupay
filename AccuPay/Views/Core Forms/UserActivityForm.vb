Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.AccuPay.Desktop.Helpers
Imports AccuPay.Data.Entities.UserActivity
Imports AccuPay.Data.Helpers
Imports AccuPay.Data.Repositories
Imports Microsoft.Extensions.DependencyInjection

Public Class UserActivityForm

    Private ReadOnly _changedType As ChangedType

    Private ReadOnly _entityNames As String()
    Private ReadOnly _formName As String
    Private ReadOnly _employeeRepository As EmployeeRepository
    Private ReadOnly _userActivityRepository As UserActivityRepository
    Private ReadOnly _userRepository As AspNetUserRepository

    Private _currentPageIndex As Integer

    Private _totalPages As Integer

    Public Sub New(entityName As String, Optional changedType As ChangedType = ChangedType.Employee)

        Me.New(New String() {entityName}, entityName, changedType)

    End Sub

    Public Sub New(entityNames As String(), formName As String, Optional changedType As ChangedType = ChangedType.Employee)

        InitializeComponent()

        _entityNames = entityNames
        _formName = formName
        _changedType = changedType

        _employeeRepository = MainServiceProvider.GetRequiredService(Of EmployeeRepository)
        _userActivityRepository = MainServiceProvider.GetRequiredService(Of UserActivityRepository)
        _userRepository = MainServiceProvider.GetRequiredService(Of AspNetUserRepository)

        _currentPageIndex = 0
    End Sub

    Private Async Sub UserActivityForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        UserActivityGrid.AutoGenerateColumns = False
        Me.Text = _formName + " " + Me.Text

        Select Case _changedType
            Case ChangedType.Employee
                ChangedEntityColumn.HeaderText = "Employee"
                ChangedEntityLabel.Text = "Employee"
            Case ChangedType.User
                ChangedEntityColumn.HeaderText = "User"
                ChangedEntityLabel.Text = "User"
            Case Else
                ChangedEntityColumn.HeaderText = String.Empty
                ChangedEntityColumn.Visible = False

                ChangedEntityLabel.Visible = False
                ChangedEntityComboBox.Visible = False

                Dim originalChangedEntityLabelHorizontalLocation = ChangedEntityLabel.Location.X
                Dim originalDescriptionLabelHorizontalLocation = DescriptionLabel.Location.X

                Dim locationDifference = originalDescriptionLabelHorizontalLocation - originalChangedEntityLabelHorizontalLocation

                DescriptionLabel.Location = MoveLocation(DescriptionLabel, locationDifference)
                DescriptionTextBox.Location = MoveLocation(DescriptionTextBox, locationDifference)

                FromLabel.Location = MoveLocation(FromLabel, locationDifference)
                FromDatePicker.Location = MoveLocation(FromDatePicker, locationDifference)

                ToLabel.Location = MoveLocation(ToLabel, locationDifference)
                ToDatePicker.Location = MoveLocation(ToDatePicker, locationDifference)

        End Select

        FromDatePicker.Value = Nothing
        ToDatePicker.Value = Nothing

        Await PopulateComboBoxes()

        Await PopulateDataGrid()

    End Sub

    Private Async Function PopulateComboBoxes() As Task

        Dim users = Await _userRepository.List(PageOptions.AllData, Z_Client, includeDeleted:=True)

        Dim userLookUpItems = LookUpItem.Convert(
            users.users,
            idPropertyName:="Id",
            displayMemberPropertyName:="FullNameLastNameFirst",
            hasDefaultItem:=True,
            nullDefaultItem:="<Any>")

        ChangedByComboBox.ValueMember = "Id"
        ChangedByComboBox.DisplayMember = "DisplayMember"
        ChangedByComboBox.DataSource = userLookUpItems

        If _changedType = ChangedType.User Then

            ChangedEntityComboBox.ValueMember = "Id"
            ChangedEntityComboBox.DisplayMember = "DisplayMember"
            ChangedEntityComboBox.DataSource = userLookUpItems

        ElseIf _changedType = ChangedType.Employee Then

            Dim employee = Await _employeeRepository.
                GetPaginatedListAsync(New EmployeePageOptions() With {.All = True}, z_OrganizationID)

            Dim employeeLookUpItems = LookUpItem.Convert(
                employee.Items,
                idPropertyName:="RowID",
                displayMemberPropertyName:="FullNameLastNameFirst",
                hasDefaultItem:=True,
                nullDefaultItem:="<Any>")

            ChangedEntityComboBox.ValueMember = "Id"
            ChangedEntityComboBox.DisplayMember = "DisplayMember"
            ChangedEntityComboBox.DataSource = employeeLookUpItems
        Else

            ChangedEntityComboBox.DataSource = Nothing

        End If

    End Function

    Private Function MoveLocation(control As Control, locationDifference As Integer) As Point
        Return New Point(control.Location.X - locationDifference, control.Location.Y)
    End Function

    Private Async Sub FilterButton_Click(sender As Object, e As EventArgs) Handles FilterButton.Click

        Await PaginateList(GetFirstPageIndex())
    End Sub

    Public Async Function PopulateDataGrid() As Task

        Await PaginateList(_currentPageIndex)

    End Function

    Private Async Function PaginateList(pageIndex As Integer) As Task
        Dim pageSize = 100
        Dim options As New PageOptions(pageIndex, pageSize, "Created", "DESC")

        Dim list = Await _userActivityRepository.GetPaginatedListAsync(
            options,
            z_OrganizationID,
            entityNames:=_entityNames,
            changedByUserId:=CType(ChangedByComboBox.SelectedValue, Integer?),
            changedType:=_changedType,
            changedEntityId:=CType(ChangedEntityComboBox.SelectedValue, Integer?),
            description:=DescriptionTextBox.Text,
            dateFrom:=FromDatePicker.Value,
            dateTo:=ToDatePicker.Value)

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

        UserActivityGrid.DataSource = acitivityItems

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
