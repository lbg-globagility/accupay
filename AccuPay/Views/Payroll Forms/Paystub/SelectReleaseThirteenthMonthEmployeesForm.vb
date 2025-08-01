Option Strict On

Imports System.Threading
Imports System.Threading.Tasks
Imports AccuPay.Core.Entities
Imports AccuPay.Core.Exceptions
Imports AccuPay.Core.Helpers
Imports AccuPay.Core.Interfaces
Imports AccuPay.Core.Services
Imports AccuPay.Core.ValueObjects
Imports AccuPay.Desktop.Utilities
Imports Microsoft.Extensions.DependencyInjection

Public Class SelectReleaseThirteenthMonthEmployeesForm

    Private _employeeModels As List(Of ThirteenthMonthEmployeeModel)

    Private ReadOnly _payPeriodRepository As IPayPeriodRepository

    Private ReadOnly _paystubRepository As IPaystubRepository

    Private ReadOnly _productRepository As IProductRepository

    Private ReadOnly _currentPayPeriodId As Integer

    Private _startingPayPeriod As PayPeriod

    Private _endingPayPeriod As PayPeriod
    Public Property HasChanges As Boolean

    Sub New(currentPayPeriodId As Integer)

        InitializeComponent()

        _currentPayPeriodId = currentPayPeriodId

        Me.HasChanges = False

        _payPeriodRepository = MainServiceProvider.GetRequiredService(Of IPayPeriodRepository)

        _paystubRepository = MainServiceProvider.GetRequiredService(Of IPaystubRepository)

        _productRepository = MainServiceProvider.GetRequiredService(Of IProductRepository)

    End Sub

    Private Async Sub SelectReleaseThirteenthMonthEmployeesForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        EmployeeGridView.AutoGenerateColumns = False

        Dim currentPayPeriod = Await _payPeriodRepository.GetByIdAsync(_currentPayPeriodId)

        If currentPayPeriod Is Nothing OrElse Not currentPayPeriod.IsOpen Then

            MessageBoxHelper.Warning("Current pay period must be Open to use this form.")
            Me.Close()

        End If

        _startingPayPeriod = Await _payPeriodRepository.GetFirstPayPeriodOfTheYear(
            currentPayPeriodYear:=currentPayPeriod.Year,
            organizationId:=z_OrganizationID)

        If _startingPayPeriod Is Nothing Then

            MessageBoxHelper.Warning("Current pay period must be Open to use this form.")
            Me.Close()

        End If

        If _startingPayPeriod.RowID Is Nothing Then

            MessageBoxHelper.Warning("Selected pay period does not exists.")
            Me.Close()

        End If

        _endingPayPeriod = currentPayPeriod

        Await PrepareAdjustmentTypeComboBox()

        Await ShowEmployees()

        UpdateSaveButton()

        UpdateSelectedPayPeriodSpanLinkLabelText()

        UncheckAllButton.Enabled = True
        SelectedPayPeriodSpanLinkLabel.Enabled = True

    End Sub

    Private Sub CancelDialogButton_Click(sender As Object, e As EventArgs) Handles CancelDialogButton.Click

        Me.Close()

    End Sub

    Private Sub UncheckAllButton_Click(sender As Object, e As EventArgs) Handles UncheckAllButton.Click

        For Each model In _employeeModels
            model.IsSelected = False
        Next

        EmployeeGridView.EndEdit()
        EmployeeGridView.Refresh()
        UpdateSaveButton()
    End Sub

    Private Sub EmployeeGridView_CellMouseUp(sender As Object, e As DataGridViewCellMouseEventArgs) _
        Handles EmployeeGridView.CellMouseUp

        EmployeeGridView.EndEdit()
        EmployeeGridView.Refresh()
        UpdateSaveButton()
    End Sub

    Private Async Sub SelectedPayPeriodSpanLinkLabel_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) _
        Handles SelectedPayPeriodSpanLinkLabel.LinkClicked

        Dim form As New MultiplePayPeriodSelectionDialog(_startingPayPeriod, _endingPayPeriod)

        If Not form.ShowDialog = Windows.Forms.DialogResult.OK Then
            Return
        End If

        _startingPayPeriod = form.PayPeriodFrom
        _endingPayPeriod = form.PayPeriodTo
        UpdateSelectedPayPeriodSpanLinkLabelText()

        Await UpdateThirteenthMonthAmount()

    End Sub

    Private Async Function PrepareAdjustmentTypeComboBox() As Task

        Dim productRepository = MainServiceProvider.GetRequiredService(Of IProductRepository)

        Dim thirteenthMonthPayAdjustment = Await productRepository.GetOrCreateAdjustmentTypeAsync(
            ProductConstant.THIRTEENTH_MONTH_PAY_ADJUSTMENT,
            organizationId:=z_OrganizationID,
            userId:=z_User)

        AdjustmentTypesComboBox.ValueMember = "RowID"
        AdjustmentTypesComboBox.DisplayMember = "DisplayName"

        Await PopulateAdjustmentTypeComboBox()

        Dim adjustmentTypes = CType(AdjustmentTypesComboBox.DataSource, List(Of Product))

        If adjustmentTypes Is Nothing OrElse adjustmentTypes.Count = 0 Then
            Return
        End If

        Dim thirteenthMonthPayAdjustmentInList = adjustmentTypes.Where(Function(a) a.RowID.Value = thirteenthMonthPayAdjustment.RowID.Value).FirstOrDefault()

        If thirteenthMonthPayAdjustmentInList IsNot Nothing Then

            Dim thirteenthMonthPayAdjustmentIndex = adjustmentTypes.IndexOf(thirteenthMonthPayAdjustmentInList)

            AdjustmentTypesComboBox.SelectedValue = thirteenthMonthPayAdjustmentInList.RowID.Value

        End If

    End Function

    Private Async Function PopulateAdjustmentTypeComboBox() As Task
        AdjustmentTypesComboBox.DataSource = (Await _productRepository.GetAdjustmentTypesAsync(z_OrganizationID)).
            OrderBy(Function(a) a.PartNo).
            ToList()

    End Function

    Private Async Function ShowEmployees() As Task

        Dim paystubs = Await _paystubRepository.GetByPayPeriodFullPaystubAsync(_currentPayPeriodId)

        Dim employeeModels As New List(Of ThirteenthMonthEmployeeModel)

        For Each paystub In paystubs
            employeeModels.Add(New ThirteenthMonthEmployeeModel(paystub))
        Next

        _employeeModels = employeeModels.OrderBy(Function(e) e.FullName).ToList

        EmployeeGridView.DataSource = _employeeModels

        Await UpdateThirteenthMonthAmount()

    End Function

    Private Sub UpdateSelectedPayPeriodSpanLinkLabelText()
        SelectedPayPeriodSpanLinkLabel.Text = $"{_startingPayPeriod.PayFromDate.ToShortDateString()} - {_endingPayPeriod.PayToDate.ToShortDateString()}"
    End Sub

    Private Async Function UpdateThirteenthMonthAmount() As Task

        If _employeeModels Is Nothing Then Return

        Dim timePeriod As New TimePeriod(_startingPayPeriod.PayFromDate, _endingPayPeriod.PayToDate)

        Dim thirteenthMonthPaystubs = (Await _paystubRepository.GetByTimePeriodWithThirteenthMonthPayAndEmployeeAsync(timePeriod, z_OrganizationID)).
            Where(Function(p) p.EmployeeID IsNot Nothing).
            GroupBy(Function(p) p.EmployeeID.Value)

        For Each employee In _employeeModels

            Dim thirteenthMonthPay = thirteenthMonthPaystubs.Where(Function(t) t.Key = employee.EmployeeId).FirstOrDefault()

            Dim amount As Decimal = 0
            Dim basicPay As Decimal = 0

            If thirteenthMonthPay IsNot Nothing Then
                amount = thirteenthMonthPay.Sum(Function(t) t.ThirteenthMonthPay.Amount)
                basicPay = thirteenthMonthPay.Sum(Function(t) t.ThirteenthMonthPay.BasicPay)
            End If

            employee.UpdateThirteenthMonthPayAmount(
                amount:=amount,
                basicPay:=basicPay)

        Next

        EmployeeGridView.EndEdit()
        EmployeeGridView.Refresh()

    End Function

    Private Sub UpdateSaveButton()
        Dim selectedCount = _employeeModels.Where(Function(m) m.IsSelected).Count()
        SaveButton.Enabled = selectedCount > 0
        SaveButton.Text = $"&Release 13th Month Pay ({selectedCount})"

    End Sub

#Region "Save Adjustments"

    Private Sub SaveButton_Click(sender As Object, e As EventArgs) Handles SaveButton.Click

        Dim selectedEmployees = _employeeModels.Where(Function(m) m.IsSelected).ToList()
        Dim selectedCount = selectedEmployees.Count
        If MessageBoxHelper.Confirm(Of Boolean)($"Are you sure you want to release the thirteenth month pay of the {selectedCount} employee(s)?") = False Then
            Return
        End If

        Dim selectedAdjustmentId = CType(AdjustmentTypesComboBox.SelectedValue, Integer)

        Dim generator As New ReleaseThirteenthMonthGeneration(selectedEmployees, selectedAdjustmentId)
        Dim progressDialog = New ProgressDialog(generator, "Creating 13th month pay adjustments...")

        Me.Enabled = False
        progressDialog.Show()

        Dim generationTask = Task.Run(
                Async Function()
                    Await generator.Start()
                End Function
            )

        generationTask.ContinueWith(
            Sub() GenerationOnSuccess(generator.Results, progressDialog),
            CancellationToken.None,
            TaskContinuationOptions.OnlyOnRanToCompletion,
            TaskScheduler.FromCurrentSynchronizationContext
        )

        generationTask.ContinueWith(
            Sub(t As Task) GenerationOnError(t, progressDialog),
            CancellationToken.None,
            TaskContinuationOptions.OnlyOnFaulted,
            TaskScheduler.FromCurrentSynchronizationContext
        )

    End Sub

    Private Sub GenerationOnSuccess(results As IReadOnlyCollection(Of ProgressGenerator.IResult), progressDialog As ProgressDialog)

        CloseProgressDialog(progressDialog)

        Dim saveResults = results.Select(Function(r) CType(r, PaystubEmployeeResult)).ToList()

        Dim resultDialog = New EmployeeResultsDialog(
            saveResults,
            title:="13th Month Pay Results",
            generationDescription:="13th month pay generation",
            entityDescription:="adjustments") With {
            .Owner = Me
        }

        resultDialog.ShowDialog()

        Me.HasChanges = True

        Me.Close()
    End Sub

    Private Sub GenerationOnError(t As Task, progressDialog As ProgressDialog)

        CloseProgressDialog(progressDialog)

        Const MessageTitle As String = "Release 13th Month Pay"

        If t.Exception?.InnerException.GetType() Is GetType(BusinessLogicException) Then

            MessageBoxHelper.ErrorMessage(t.Exception?.InnerException.Message, MessageTitle)
        Else
            Debugger.Break()
            MessageBoxHelper.ErrorMessage("Something went wrong while saving the 13th month pay adjustments. Please contact Globagility Inc. for assistance.", MessageTitle)
        End If

    End Sub

    Private Sub CloseProgressDialog(progressDialog As ProgressDialog)

        Me.Enabled = True

        If progressDialog Is Nothing Then Return

        progressDialog.Close()
        progressDialog.Dispose()
    End Sub

#End Region

End Class
