Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Core.Entities
Imports AccuPay.Core.Helpers
Imports AccuPay.Core.Interfaces
Imports AccuPay.Desktop.Utilities
Imports Microsoft.Extensions.DependencyInjection

Public Class OrganizationListDialog

    Private Const UnauthorizedOrganizationMessage As String = "You are not authorized to access this organization."
    Private ReadOnly _currentOrganizationId As Integer

    Public SelectedOrganizationId As Integer
    Public SelectedOrganizationName As String
    Public UserRoleForSelectedOrganization As AspNetRole
    Private ReadOnly _organizationRepository As IOrganizationRepository
    Private ReadOnly _roleRepository As IRoleRepository
    Private ReadOnly _policyHelper As IPolicyHelper

    Public Sub New(currentOrganizationId As Integer)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        _currentOrganizationId = currentOrganizationId

        SelectedOrganizationId = _currentOrganizationId
        SelectedOrganizationName = z_CompanyName
        UserRoleForSelectedOrganization = USER_ROLE

        _organizationRepository = MainServiceProvider.GetRequiredService(Of IOrganizationRepository)
        _roleRepository = MainServiceProvider.GetRequiredService(Of IRoleRepository)
        _policyHelper = MainServiceProvider.GetRequiredService(Of IPolicyHelper)
    End Sub

    Private Async Sub OrganizationListDialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        OrganizationComboBox.DisplayMember = "Name"

        Await ReloadOrganizationAsync()

    End Sub

    Async Function ReloadOrganizationAsync() As Task
        Dim list = Await _organizationRepository.List(OrganizationPageOptions.AllData, 1)

        If list.organizations IsNot Nothing Then

            OrganizationComboBox.DataSource = list.organizations.
                Where(Function(o) Not o.RowID.Value = _currentOrganizationId).
                OrderBy(Function(o) o.Name.Replace("&", "\&")).
                ToList()

            SetDropDownWidth()
        End If
    End Function

    Private Sub OrganizationComboBox_SelectedIndexChanged(sender As Object, e As EventArgs)

    End Sub

    Private Sub SetDropDownWidth()

        Dim organizations = CType(OrganizationComboBox.DataSource, List(Of Organization))

        If Not organizations.Any() Then Return

        Static font As Font = OrganizationComboBox.Font
        Dim grp As Graphics = OrganizationComboBox.CreateGraphics()

        Dim vertScrollBarWidth As Integer = If(OrganizationComboBox.Items.Count > OrganizationComboBox.MaxDropDownItems, SystemInformation.VerticalScrollBarWidth, 0)

        Dim width As Integer = 0

        Dim longestWord = organizations.
            OrderByDescending(Function(o) o.Name.Length).
            Select(Function(o) o.Name).
            FirstOrDefault()

        width = CInt(grp.MeasureString(longestWord, font).Width) + vertScrollBarWidth

        OrganizationComboBox.DropDownWidth = width
    End Sub

    Private Async Sub ButtonOK_Click(sender As Object, e As EventArgs) Handles ButtonOK.Click
        Dim cancelDialogResult =
            Sub()
                DialogResult = DialogResult.Cancel
            End Sub

        Dim selectedOrganization = CType(OrganizationComboBox.SelectedValue, Organization)
        If selectedOrganization IsNot Nothing Then

            Dim userRole = Await _roleRepository.GetByUserAndOrganizationAsync(userId:=z_User, organizationId:=selectedOrganization.RowID.Value)

            If Not _policyHelper.UseUserLevel AndAlso userRole Is Nothing Then
                MessageBoxHelper.ErrorMessage(UnauthorizedOrganizationMessage)
                cancelDialogResult()
                Return
            End If

            SelectedOrganizationId = selectedOrganization.RowID.Value
            SelectedOrganizationName = selectedOrganization.Name
            UserRoleForSelectedOrganization = userRole

            DialogResult = DialogResult.OK
        Else
            cancelDialogResult()
        End If
    End Sub

    Private Sub OrganizationComboBox_SelectedIndexChanged_1(sender As Object, e As EventArgs) Handles OrganizationComboBox.SelectedIndexChanged

    End Sub

    Private Sub OrganizationComboBox_KeyDown(sender As Object, e As KeyEventArgs) Handles OrganizationComboBox.KeyDown
        If e.KeyCode = Keys.Enter Then
            ButtonOK.PerformClick()
        End If
    End Sub

End Class
