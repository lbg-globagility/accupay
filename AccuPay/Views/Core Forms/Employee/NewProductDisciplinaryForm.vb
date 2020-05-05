Option Strict On
Imports System.Threading.Tasks
Imports AccuPay.Data.Entities
Imports AccuPay.Data.Repositories
Imports AccuPay.Enums
Imports AccuPay.Utils

Public Class NewProductDisciplinaryForm

    Private _findings As IEnumerable(Of Product)

    Private _currentFinding As Product

    Private _mode As FormMode = FormMode.Empty

    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()
        dgvFindings.AutoGenerateColumns = False

        ' Add any initialization after the InitializeComponent() call.

    End Sub
    Private Async Sub NewProductEmployeeDisciplinary_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Await LoadFindings()
    End Sub

    Private Async Function LoadFindings() As Task

        RemoveHandler dgvFindings.SelectionChanged, AddressOf dgvFindings_SelectionChanged

        Dim productRepo = New ProductRepository
        _findings = Await productRepo.GetDisciplinaryTypesAsync(z_OrganizationID)
        dgvFindings.DataSource = _findings

        If _findings.Count > 0 Then
            SelectFinding(DirectCast(dgvFindings.CurrentRow?.DataBoundItem, Product))
            ChangeMode(FormMode.Editing)
        Else
            SelectFinding(Nothing)
            _currentFinding = New Product
            ChangeMode(FormMode.Empty)
        End If

        AddHandler dgvFindings.SelectionChanged, AddressOf dgvFindings_SelectionChanged
    End Function

    Private Sub SelectFinding(finding As Product)
        If finding IsNot Nothing Then
            _currentFinding = finding

            With _currentFinding
                txtName.Text = .PartNo
                txtDescription.Text = .Description
            End With

        Else
            ClearForm()
        End If
    End Sub

    Private Sub ClearForm()
        txtName.Text = ""
        txtDescription.Text = ""
    End Sub

    Private Sub dgvFindings_SelectionChanged(sender As Object, e As EventArgs) Handles dgvFindings.SelectionChanged
        If _findings.Count > 0 Then
            SelectFinding(DirectCast(dgvFindings.CurrentRow?.DataBoundItem, Product))
        End If
    End Sub

    Private Async Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        Dim result = MsgBox("Are you sure you want to delete this Finding?", MsgBoxStyle.YesNo, "Delete Finding")

        If result = MsgBoxResult.Yes Then
            Await FunctionUtils.TryCatchFunctionAsync("Delete Disciplinary Finding",
                Async Function()
                    Dim repo = New ProductRepository
                    Await repo.DeleteAsync(_currentFinding.RowID.Value)

                    Await LoadFindings()
                End Function)

        End If
    End Sub

    Private Sub ChangeMode(mode As FormMode)
        _mode = mode

        Select Case _mode
            Case FormMode.Disabled
                btnNew.Enabled = False
                btnSave.Enabled = False
                btnDelete.Enabled = False
                btnCancel.Enabled = False
            Case FormMode.Empty
                btnNew.Enabled = True
                btnSave.Enabled = False
                btnDelete.Enabled = False
                btnCancel.Enabled = False
            Case FormMode.Creating
                btnNew.Enabled = False
                btnSave.Enabled = True
                btnDelete.Enabled = False
                btnCancel.Enabled = True
            Case FormMode.Editing
                btnNew.Enabled = True
                btnSave.Enabled = True
                btnDelete.Enabled = True
                btnCancel.Enabled = True
        End Select
    End Sub

    Private Async Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If Await SaveFinding() Then
            Await LoadFindings()
        End If
    End Sub

    Private Async Function SaveFinding() As Task(Of Boolean)
        Dim messageTitle = ""
        Dim succeed As Boolean = False

        If String.IsNullOrWhiteSpace(txtName.Text) Then
            ShowBalloonInfo("Finding Name is empty.", "Invalid input.")
            Return False
        End If

        Await FunctionUtils.TryCatchFunctionAsync("Save Disciplinary Action Finding",
            Async Function()
                If isChanged() Then

                    If _mode = FormMode.Creating Then
                        Dim productRepo = New ProductRepository
                        Await productRepo.AddDecipilinaryTypeAsync(txtName.Text,
                                                                   z_OrganizationID,
                                                                   z_User,
                                                                   txtDescription.Text)

                        messageTitle = "New Disciplinary Action Finding"
                    Else
                        Dim productRepo = New ProductRepository
                        Await productRepo.UpdateDisciplinaryTypeAsync(_currentFinding.RowID.Value,
                                                                      z_User,
                                                                      txtName.Text,
                                                                      txtDescription.Text)

                        messageTitle = "Update Disciplinary Action Finding"

                    End If
                    succeed = True

                Else
                    MessageBoxHelper.Warning("No value changed")
                End If
            End Function)

        If succeed Then
            ShowBalloonInfo("Disciplinary Finding successfuly saved.", messageTitle)
            Return True
        End If
        Return False
    End Function

    Private Function isChanged() As Boolean
        With _currentFinding
            If .PartNo <> txtName.Text OrElse
                    .Description <> txtDescription.Text Then
                Return True
            End If
        End With
        Return False
    End Function

    Private Sub ShowBalloonInfo(content As String, title As String)
        myBalloon(content, title, lblActionName, 150, -115)
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click

        RemoveHandler dgvFindings.SelectionChanged, AddressOf dgvFindings_SelectionChanged      'prevents from adding too many handlers
        AddHandler dgvFindings.SelectionChanged, AddressOf dgvFindings_SelectionChanged

        If _findings.Count > 0 Then
            ChangeMode(FormMode.Editing)
            SelectFinding(DirectCast(dgvFindings.CurrentRow?.DataBoundItem, Product))
        Else
            ChangeMode(FormMode.Empty)
            SelectFinding(Nothing)
        End If
    End Sub

    Private Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        ChangeMode(FormMode.Creating)

        RemoveHandler dgvFindings.SelectionChanged, AddressOf dgvFindings_SelectionChanged
        dgvFindings.ClearSelection()

        SelectFinding(Nothing)
    End Sub
End Class