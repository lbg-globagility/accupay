Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Data.Entities
Imports AccuPay.Data.Repositories
Imports AccuPay.Desktop.Enums
Imports AccuPay.Desktop.Utilities
Imports AccuPay.Utilities.Extensions
Imports Microsoft.Extensions.DependencyInjection

Public Class BonusTab

    Private Const FormEntityName As String = "Bonus"

    Private _employee As Employee

    Private _bonuses As IEnumerable(Of Bonus)

    Private _products As IEnumerable(Of Product)

    Private _currentBonus As New Bonus

    Private _mode As FormMode = FormMode.Empty

    Private _frequencies As List(Of String)

    Private ReadOnly _productRepo As ProductRepository

    Private ReadOnly _userActivityRepo As UserActivityRepository

    Public Sub New()

        InitializeComponent()

        dgvempbon.AutoGenerateColumns = False

        If MainServiceProvider IsNot Nothing Then

            _productRepo = MainServiceProvider.GetRequiredService(Of ProductRepository)

            _userActivityRepo = MainServiceProvider.GetRequiredService(Of UserActivityRepository)
        End If

    End Sub

    Private Sub BonusTab_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        dtpbonenddate.Enabled = False
    End Sub

    Public Async Function SetEmployee(employee As Employee) As Task
        Me.cbobontype.Focus()
        _employee = employee

        txtFNameBon.Text = _employee.FullNameWithMiddleInitialLastNameFirst
        txtEmpIDBon.Text = _employee.EmployeeIdWithPositionAndEmployeeType
        pbEmpPicBon.Image = ConvByteToImage(_employee.Image)

        ChangeMode(FormMode.Empty)
        Await LoadBonuses()
    End Function

    Private Async Function LoadBonuses() As Task
        If _employee Is Nothing OrElse _employee.RowID Is Nothing Then
            Return
        End If

        Dim bonusRepo = MainServiceProvider.GetRequiredService(Of BonusRepository)
        _bonuses = Await bonusRepo.GetByEmployeeAsync(_employee.RowID.Value)
        _frequencies = bonusRepo.GetFrequencyList

        _products = Await _productRepo.GetBonusTypesAsync(z_OrganizationID)

        RemoveHandler dgvempbon.SelectionChanged, AddressOf dgvempbon_SelectionChanged
        BindDataSource()

        If _bonuses.Count > 0 Then
            SelectBonus(DirectCast(dgvempbon.CurrentRow?.DataBoundItem, Bonus))
            ChangeMode(FormMode.Editing)
            FormToolsControl(True)
        Else
            SelectBonus(Nothing)
            _currentBonus = New Bonus
            ChangeMode(FormMode.Empty)
            FormToolsControl(False)
        End If

        AddHandler dgvempbon.SelectionChanged, AddressOf dgvempbon_SelectionChanged
    End Function

    Private Sub FormToolsControl(control As Boolean)
        cbobontype.Enabled = control
        cbobonfreq.Enabled = control
        dtpbonstartdate.Enabled = control
        dtpbonenddate.Enabled = control
        txtbonamt.Enabled = control
    End Sub

    Private Sub BindDataSource()

        cbobonfreq.DataSource = _frequencies
        cbobontype.DisplayMember = "Name"
        cbobontype.DataSource = _products
        dgvempbon.DataSource = _bonuses

    End Sub

    Private Sub ToolStripButton11_Click(sender As Object, e As EventArgs) Handles ToolStripButton11.Click
        EmployeeForm.Close()
    End Sub

    Private Async Sub tsbtnCancelBon_Click(sender As Object, e As EventArgs) Handles tsbtnCancelBon.Click
        If _mode = FormMode.Creating Then
            SelectBonus(Nothing)
            EnableBonusGrid()
        ElseIf _mode = FormMode.Editing Then
            Await LoadBonuses()
        End If

        If _currentBonus Is Nothing Then
            ChangeMode(FormMode.Empty)
        Else
            ChangeMode(FormMode.Editing)
        End If
    End Sub

    Private Sub dgvempbon_SelectionChanged(sender As Object, e As EventArgs) Handles dgvempbon.SelectionChanged
        If _bonuses.Count > 0 Then
            Dim bonus = DirectCast(dgvempbon.CurrentRow?.DataBoundItem, Bonus)
            SelectBonus(bonus)

        End If

    End Sub

    Private Sub SelectBonus(bonus As Bonus)
        If bonus IsNot Nothing Then
            _currentBonus = bonus

            cbobontype.Text = _currentBonus.Product.Name
            cbobonfreq.Text = _currentBonus.AllowanceFrequency
            dtpbonstartdate.Value = _currentBonus.EffectiveStartDate
            dtpbonenddate.Value = _currentBonus.EffectiveEndDate
            txtbonamt.Text = _currentBonus.BonusAmount.ToString

            If _currentBonus.AllowanceFrequency = Bonus.FREQUENCY_ONE_TIME Then
                dtpbonenddate.Enabled = False
            End If
        Else
            ClearForm()
        End If

    End Sub

    Private Sub ClearForm()
        cbobontype.SelectedItem = Nothing
        cbobonfreq.SelectedItem = Nothing
        dtpbonstartdate.Value = Today
        dtpbonenddate.Value = Today
        txtbonamt.Text = ""
    End Sub

    Private Async Sub tsbtnDelBon_Click(sender As Object, e As EventArgs) Handles tsbtnDelBon.Click
        If _bonuses.Count > 0 Then
            Dim result = MsgBox("Are you sure you want to delete this Bonus?", MsgBoxStyle.YesNo, "Delete Bonus")

            If result = MsgBoxResult.Yes Then
                Await FunctionUtils.TryCatchFunctionAsync("Delete Bonus",
                Async Function()
                    Dim bonusRepo = MainServiceProvider.GetRequiredService(Of BonusRepository)
                    Await bonusRepo.DeleteAsync(_currentBonus)

                    _userActivityRepo.RecordDelete(z_User, FormEntityName, CInt(_currentBonus.RowID), z_OrganizationID)

                    Await LoadBonuses()
                End Function)
            End If
        End If
    End Sub

    Private Async Sub tsbtnSaveBon_Click(sender As Object, e As EventArgs) Handles tsbtnSaveBon.Click
        pbEmpPicBon.Focus()
        If Await SaveBonus() Then
            Await LoadBonuses()
        End If
    End Sub

    Private Async Sub tsbtnNewBon_Click(sender As Object, e As EventArgs) Handles tsbtnNewBon.Click

        If _employee Is Nothing Then
            MessageBoxHelper.ErrorMessage("Please select an employee first.")
            Return
        End If

        Dim form As New AddBonusForm(_employee)
        form.ShowDialog()

        If form.isSaved Then
            Await LoadBonuses()

            If form.showBalloon Then
                ShowBalloonInfo("Bonus successfuly added.", "Saved")
            End If

        End If
    End Sub

    Private Async Function SaveBonus() As Task(Of Boolean)

        Dim messageTitle = ""
        Dim succeed As Boolean = True

        If cbobontype.SelectedItem Is Nothing Then
            messageTitle = "Invalid Bonus Type"
            succeed = False
        ElseIf cbobonfreq.SelectedItem Is Nothing Then
            messageTitle = "Invalid Bonus Frequency"
            succeed = False
        ElseIf txtbonamt.Text = "" OrElse IsNumeric(txtbonamt.Text) = False Then
            messageTitle = "Invalid Amount"
            succeed = False
        End If

        If Not succeed Then
            ShowBalloonInfo("Error Input.", messageTitle)
            Return False
        End If

        succeed = False

        Dim product = _products.Where(Function(x) x.Name = cbobontype.Text).
                                 FirstOrDefault
        Await FunctionUtils.TryCatchFunctionAsync("Save Bonus",
            Async Function()
                If IsChanged(product) Then
                    Dim oldBonus = _currentBonus.CloneJson()

                    With _currentBonus
                        .ProductID = product.RowID
                        .Product = product
                        .AllowanceFrequency = cbobonfreq.SelectedItem.ToString
                        .EffectiveStartDate = dtpbonstartdate.Value
                        .EffectiveEndDate = dtpbonenddate.Value
                        .BonusAmount = CType(txtbonamt.Text, Decimal?)
                        .EmployeeID = _employee.RowID
                        .OrganizationID = z_OrganizationID
                        .TaxableFlag = product.Status
                        .LastUpdBy = z_User
                    End With

                    Dim repo = MainServiceProvider.GetRequiredService(Of BonusRepository)

                    _currentBonus.LastUpdBy = z_User
                    Await repo.UpdateAsync(_currentBonus)

                    RecordUpdateBonus(oldBonus)

                    messageTitle = "Update Bonus"
                    succeed = True
                Else
                    MessageBoxHelper.Warning("No value changed")
                End If
            End Function)

        If succeed Then
            ShowBalloonInfo("Bonus successfuly saved.", messageTitle)
            Return True
        End If
        Return False

    End Function

    Private Function IsChanged(product As Product) As Boolean
        If _currentBonus.ProductID <> product.RowID Or
            _currentBonus.Product.Name <> cbobontype.Text.ToString Or
            _currentBonus.AllowanceFrequency <> cbobonfreq.SelectedItem.ToString Or
            _currentBonus.BonusAmount <> CType(txtbonamt.Text, Decimal?) Or
            _currentBonus.EffectiveEndDate <> dtpbonenddate.Value Or
            _currentBonus.EffectiveStartDate <> dtpbonstartdate.Value Then
            Return True
        End If
        Return False
    End Function

    Private Sub dtpbonstartdate_ValueChanged(sender As Object, e As EventArgs) Handles dtpbonstartdate.ValueChanged
        If Not dtpbonenddate.Enabled Or dtpbonenddate.Value < dtpbonstartdate.Value Then
            dtpbonenddate.Value = dtpbonstartdate.Value
        End If

    End Sub

    Private Sub ChangeMode(mode As FormMode)
        _mode = mode

        Select Case _mode
            Case FormMode.Disabled
                tsbtnNewBon.Enabled = False
                tsbtnSaveBon.Enabled = False
                tsbtnDelBon.Enabled = False
                tsbtnCancelBon.Enabled = False
            Case FormMode.Empty
                tsbtnNewBon.Enabled = True
                tsbtnSaveBon.Enabled = False
                tsbtnDelBon.Enabled = False
                tsbtnCancelBon.Enabled = False
            Case FormMode.Creating
                tsbtnNewBon.Enabled = False
                tsbtnSaveBon.Enabled = True
                tsbtnDelBon.Enabled = False
                tsbtnCancelBon.Enabled = True
            Case FormMode.Editing
                tsbtnNewBon.Enabled = True
                tsbtnSaveBon.Enabled = True
                tsbtnDelBon.Enabled = True
                tsbtnCancelBon.Enabled = True
        End Select
    End Sub

    Private Async Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        With newProdBonus
            .ShowDialog()
        End With
        Await LoadBonuses()
    End Sub

    Private Sub ShowBalloonInfo(content As String, title As String)
        myBalloon(content, title, pbEmpPicBon, 100, -110)
    End Sub

    Private Sub DisableBonusGrid()
        RemoveHandler dgvempbon.SelectionChanged, AddressOf dgvempbon_SelectionChanged
        dgvempbon.ClearSelection()
        dgvempbon.CurrentCell = Nothing
    End Sub

    Private Sub EnableBonusGrid()
        AddHandler dgvempbon.SelectionChanged, AddressOf dgvempbon_SelectionChanged

        If dgvempbon.Rows.Count > 0 Then
            dgvempbon.Item(1, 0).Selected = True
            SelectBonus(DirectCast(dgvempbon.CurrentRow.DataBoundItem, Bonus))
        End If
    End Sub

    Private Sub cbobonfreq_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbobonfreq.SelectedIndexChanged
        If cbobonfreq.SelectedItem Is Nothing Then Return

        If cbobonfreq.SelectedItem.ToString = Bonus.FREQUENCY_ONE_TIME Then
            dtpbonenddate.Value = dtpbonstartdate.Value
            dtpbonenddate.Enabled = False
        Else
            dtpbonenddate.Enabled = True
        End If
    End Sub

    Private Sub RecordUpdateBonus(oldBonus As Bonus)
        Dim changes As New List(Of UserActivityItem)

        Dim entityName = FormEntityName.ToLower()

        If _currentBonus.ProductID <> oldBonus.ProductID Then
            changes.Add(New UserActivityItem() With
                        {
                        .EntityId = CInt(oldBonus.RowID),
                        .Description = $"Updated {entityName} type from '{oldBonus.Product.Name}' to '{_currentBonus.Product.Name}'."
                        })
        End If
        If _currentBonus.AllowanceFrequency <> oldBonus.AllowanceFrequency Then
            changes.Add(New UserActivityItem() With
                        {
                        .EntityId = CInt(oldBonus.RowID),
                        .Description = $"Updated {entityName} frequency from '{oldBonus.AllowanceFrequency}' to '{_currentBonus.AllowanceFrequency}'."
                        })
        End If
        If _currentBonus.EffectiveStartDate <> oldBonus.EffectiveStartDate Then
            changes.Add(New UserActivityItem() With
                        {
                        .EntityId = CInt(oldBonus.RowID),
                        .Description = $"Updated {entityName} start date from '{oldBonus.EffectiveStartDate.ToShortDateString}' to '{_currentBonus.EffectiveStartDate.ToShortDateString}'."
                        })
        End If
        If _currentBonus.EffectiveEndDate <> oldBonus.EffectiveEndDate Then
            changes.Add(New UserActivityItem() With
                        {
                        .EntityId = CInt(oldBonus.RowID),
                        .Description = $"Updated {entityName} end date from '{oldBonus.EffectiveEndDate.ToShortDateString}' to '{_currentBonus.EffectiveEndDate.ToShortDateString}'."
                        })
        End If
        If _currentBonus.BonusAmount <> oldBonus.BonusAmount Then
            changes.Add(New UserActivityItem() With
                        {
                        .EntityId = CInt(oldBonus.RowID),
                        .Description = $"Updated {entityName} amount from '{oldBonus.BonusAmount}' to '{_currentBonus.BonusAmount}'."
                        })
        End If

        _userActivityRepo.CreateRecord(z_User, FormEntityName, z_OrganizationID, UserActivityRepository.RecordTypeEdit, changes)

    End Sub

    Private Sub dtpbonenddate_ValueChanged(sender As Object, e As EventArgs) Handles dtpbonenddate.ValueChanged
        If dtpbonenddate.Value < dtpbonstartdate.Value Then
            dtpbonenddate.Value = dtpbonstartdate.Value
        End If
    End Sub

    Private Sub ToolStripButton1_Click_1(sender As Object, e As EventArgs) Handles UserActivity.Click
        Dim userActivity As New UserActivityForm(FormEntityName)
        userActivity.ShowDialog()
    End Sub

End Class