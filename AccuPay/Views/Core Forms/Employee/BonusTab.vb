Option Strict On
Imports System.Threading.Tasks
Imports AccuPay.Data.Entities
Imports AccuPay.Data.Repositories
Imports AccuPay.Enums
Imports AccuPay.Utils

Public Class BonusTab

    Private _employee As Entity.Employee

    Private _bonuses As List(Of Bonus)

    Private _products As IEnumerable(Of Entity.Product)

    Private _currentBonus As New Bonus

    Private _mode As FormMode = FormMode.Empty

    Dim category As String = "Bonus"

    Public Sub New()
        InitializeComponent()
        dgvempbon.AutoGenerateColumns = False

    End Sub
    Private Sub BonusTab_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        dtpbonenddate.Enabled = False
    End Sub

    Public Sub SetEmployee(employee As Entity.Employee)
        Me.cbobontype.Focus()
        _employee = employee

        txtFNameBon.Text = _employee.FullNameWithMiddleInitialLastNameFirst
        txtEmpIDBon.Text = _employee.EmployeeIdWithPositionAndEmployeeType
        pbEmpPicBon.Image = ConvByteToImage(_employee.Image)

        ChangeMode(FormMode.Empty)
        LoadBonuses()
    End Sub

    Private Async Sub LoadBonuses()
        If _employee Is Nothing OrElse _employee.RowID Is Nothing Then
            Return
        End If

        Dim bonusRepo = New BonusRepository
        _bonuses = bonusRepo.GetByEmployee(_employee.RowID.Value).ToList

        Dim productRepo = New Repository.ProductRepository
        _products = Await productRepo.GetBonusTypes()

        RemoveHandler dgvempbon.SelectionChanged, AddressOf dgvempbon_SelectionChanged
        Await BindDataSource()

        If _bonuses.Count > 0 Then
            SelectBonus(DirectCast(dgvempbon.CurrentRow?.DataBoundItem, Bonus))
            ChangeMode(FormMode.Editing)
        Else
            SelectBonus(Nothing)
            _currentBonus = New Bonus
            ChangeMode(FormMode.Empty)
        End If

        AddHandler dgvempbon.SelectionChanged, AddressOf dgvempbon_SelectionChanged
    End Sub

    Private Async Function BindDataSource() As Task

        Dim payFrequencyRepo = New Repository.PayFrequencyRepository
        cbobonfreq.DataSource = (Await payFrequencyRepo.GetAllAsync()).Select(Function(x) x.Type).ToList()
        cbobontype.DisplayMember = "Name"
        cbobontype.DataSource = _products
        dgvempbon.DataSource = _bonuses

    End Function

    Private Sub ToolStripButton11_Click(sender As Object, e As EventArgs) Handles ToolStripButton11.Click
        EmployeeForm.Close()
    End Sub

    Private Sub tsbtnCancelBon_Click(sender As Object, e As EventArgs) Handles tsbtnCancelBon.Click
        If _mode = FormMode.Creating Then
            SelectBonus(Nothing)
            EnableBonusGrid()
        ElseIf _mode = FormMode.Editing Then
            LoadBonuses()
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
            dtpbonenddate.Value = _currentBonus.EffectiveStartDate
            txtbonamt.Text = _currentBonus.BonusAmount.ToString
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

    Private Sub tsbtnDelBon_Click(sender As Object, e As EventArgs) Handles tsbtnDelBon.Click
        If _bonuses.Count > 0 Then
            Dim result = MsgBox("Are you sure you want to delete this Bonus?", MsgBoxStyle.YesNo, "Delete Bonus")

            If result = MsgBoxResult.Yes Then
                Dim repo = New BonusRepository
                repo.Delete(_currentBonus)
                LoadBonuses()

            End If
        End If
    End Sub

    Private Sub tsbtnSaveBon_Click(sender As Object, e As EventArgs) Handles tsbtnSaveBon.Click
        If SaveBonus() Then
            LoadBonuses()
        End If
    End Sub

    Private Sub tsbtnNewBon_Click(sender As Object, e As EventArgs) Handles tsbtnNewBon.Click
        _currentBonus = New Bonus
        ClearForm()

        ChangeMode(FormMode.Creating)
        DisableBonusGrid()
    End Sub

    Private Function SaveBonus() As Boolean

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
        Try

            With _currentBonus
                .ProductID = product.RowID
                .AllowanceFrequency = cbobonfreq.SelectedItem.ToString
                .EffectiveStartDate = dtpbonstartdate.Value
                .EffectiveEndDate = dtpbonenddate.Value
                .BonusAmount = CType(txtbonamt.Text, Decimal?)
                .EmployeeID = _employee.RowID
                .OrganizationID = z_OrganizationID
                .TaxableFlag = product.Status
            End With

            Dim repo = New BonusRepository

            If _currentBonus.RowID.HasValue Then
                If IsChanged(product) Then
                    _currentBonus.LastUpdBy = z_User
                    repo.Update(_currentBonus)
                    messageTitle = "Update Bonus"
                    succeed = True
                Else
                    MessageBoxHelper.Warning("No value changed")
                    Return False
                End If

            Else
                _currentBonus.CreatedBy = z_User
                repo.Create(_currentBonus)
                messageTitle = "New Bonus"
                succeed = True
            End If

        Catch ex As Exception
            MsgBox("Something wrong occured.", MsgBoxStyle.Exclamation)
        End Try

        If Not succeed Then
            Return False
        End If
        ShowBalloonInfo("Bonus successfuly saved.", messageTitle)
        Return True


    End Function

    Private Function IsChanged(product As Entity.Product) As Boolean
        If _currentBonus.ProductID <> product.RowID Or
            _currentBonus.Product.Name <> cbobontype.SelectedItem.ToString Or
            _currentBonus.AllowanceFrequency <> cbobonfreq.SelectedItem.ToString Or
            _currentBonus.BonusAmount <> CType(txtbonamt.Text, Decimal?) Or
            _currentBonus.EffectiveEndDate <> dtpbonenddate.Value Or
            _currentBonus.EffectiveStartDate <> dtpbonstartdate.Value Then
            Return True
        End If
        Return False
    End Function

    Private Sub dtpbonstartdate_ValueChanged(sender As Object, e As EventArgs) Handles dtpbonstartdate.ValueChanged
        dtpbonenddate.Value = dtpbonstartdate.Value
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

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        With newProdBonus
            .ShowDialog()
        End With
        LoadBonuses()
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
End Class
