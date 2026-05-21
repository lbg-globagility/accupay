Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Core.Entities
Imports AccuPay.Core.Helpers
Imports AccuPay.Core.Interfaces
Imports AccuPay.Desktop.Helpers
Imports AccuPay.Desktop.Utilities
Imports Microsoft.Extensions.DependencyInjection

Public Class EditSoloParentBenefitForm
    Private ReadOnly _userId As Integer
    Private ReadOnly _orgId As Integer
    Private ReadOnly _soloParentBeneficiaryDto As SoloParentBeneficiaryDto
    Private _soloParentBeneficiary As SoloParentBeneficiary
    Private ReadOnly _employee As Employee

    Public Sub New(userId As Integer, orgId As Integer, soloParentBeneficiaryDto As SoloParentBeneficiaryDto)
        _userId = userId
        _orgId = orgId
        _soloParentBeneficiaryDto = soloParentBeneficiaryDto
        _soloParentBeneficiary = _soloParentBeneficiaryDto.SoloParentBeneficiary
        _employee = _soloParentBeneficiaryDto.Employee

        InitializeComponent()

    End Sub

    Private Sub EditSoloParentBenefitForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Label20.Text = String.Empty
        Me.Text = $"Solo Parent Benefit Application for [{_employee.FullNameWithMiddleInitialLastNameFirst.ToUpper()}]"

        CheckBox1.DataBindings.Clear()
        CheckBox1.DataBindings.Add("Checked", _soloParentBeneficiary, "HasValidityPassed", False, DataSourceUpdateMode.OnPropertyChanged)

        Label20.DataBindings.Clear()
        Label20.Text = _soloParentBeneficiary.FileNameAndExtensionOnly

        LinkLabel1.Enabled = If(_soloParentBeneficiary.RowID, 0) > 0

    End Sub

    Private Async Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        Await FunctionUtils.TryCatchFunctionAsync(messageTitle:="Saving SoloParentBeneficiary Application",
            action:=Async Function()

                        Dim soloParentBeneficiaryDataService = MainServiceProvider.GetRequiredService(Of ISoloParentBeneficiaryDataService)()
                        Dim productRepository = MainServiceProvider.GetRequiredService(Of IProductRepository)()
                        Dim leaveLedgerRepository = MainServiceProvider.GetRequiredService(Of ILeaveLedgerRepository)()

                        Dim leaveType = Await productRepository.GetOrCreateLeaveTypeAsync(leaveTypeName:=ProductConstant.SOLO_PARENT_LEAVE,
                            organizationId:=_orgId,
                            userId:=_userId)
                        Dim leaveTypeId = If(leaveType.RowID, 0)

                        Dim tasks = Task.WhenAll(
                            soloParentBeneficiaryDataService.SaveManyAsync(
                                entities:=New List(Of SoloParentBeneficiary) From {_soloParentBeneficiary},
                                currentlyLoggedInUserId:=_userId),
                            leaveLedgerRepository.CreateBeginningBalanceAsync(
                                employeeId:=If(_employee.RowID, 0),
                                leaveTypeId:=leaveTypeId,
                                organizationId:=_orgId,
                                userId:=_userId,
                                balance:=_soloParentBeneficiary.LEAVE_HOURS)
                        )

                        Await tasks.
                            ContinueWith(Async Function(ant)
                                             If ant.IsCompleted AndAlso Not ant.IsFaulted Then DialogResult = DialogResult.OK

                                         End Function)

                    End Function)

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        DialogResult = DialogResult.OK
        Close()

    End Sub

    Private Async Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click
        Dim browseFileOut = OpenFileDialogImportHelper.BrowseFile(filter:="All Files (*.*)|*.*")

        If Not browseFileOut.IsSuccess Then Return

        Await _soloParentBeneficiary.AttachFileAsync(browseFileOut.FileName).
            ContinueWith(Sub(t)
                             If t.IsCompleted AndAlso Not t.IsFaulted Then
                                 Label20.Text = _soloParentBeneficiary.FileNameAndExtensionOnly
                                 Label20.Refresh()

                             End If

                         End Sub, TaskScheduler.FromCurrentSynchronizationContext())

    End Sub

    Private Sub LinkLabel2_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel2.LinkClicked
        _soloParentBeneficiary.DetachFile()
        Label20.Text = _soloParentBeneficiary.FileNameAndExtensionOnly

    End Sub

    Private Async Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Dim prompt = MessageBox.Show(text:="Are you sure you want to revoke this solo parent benefit?",
            caption:="Revoke Solo Parent Benefit",
            buttons:=MessageBoxButtons.YesNoCancel,
            icon:=MessageBoxIcon.Question,
            defaultButton:=MessageBoxDefaultButton.Button2)

        If Not prompt = DialogResult.Yes Then Return

        Await FunctionUtils.TryCatchFunctionAsync(messageTitle:="Revoke SoloParentBenefit Application",
            action:=Async Function()

                        Dim leaveLedgerRepository = MainServiceProvider.GetRequiredService(Of ILeaveLedgerRepository)()
                        Dim productRepository = MainServiceProvider.GetRequiredService(Of IProductRepository)()

                        Dim leaveType = Await productRepository.GetOrCreateLeaveTypeAsync(leaveTypeName:=ProductConstant.SOLO_PARENT_LEAVE,
                            organizationId:=_orgId,
                            userId:=_userId)

                        Dim leaveLedgers = Await leaveLedgerRepository.GetAllByEmployee(_employee.RowID)
                        Dim leaveLedger = leaveLedgers.FirstOrDefault(Function(l) Equals(l.ProductID, leaveType.RowID))

                        Dim lt = LeaveTransaction.NewLeaveTransaction(userId:=_userId,
                            organizationId:=_orgId,
                            employeeId:=_employee.RowID,
                            leaveLedgerId:=leaveLedger.RowID,
                            transactionDate:=Date.Now(),
                            type:=LeaveTransactionType.Credit,
                            amount:=0,
                            balance:=0,
                            payPeriodId:=Nothing,
                            paystubId:=Nothing,
                            referenceId:=Nothing,
                            description:="Revoke Solo Parent Benefit")

                        Dim soloParentBeneficiaryDataService = MainServiceProvider.GetRequiredService(Of ISoloParentBeneficiaryDataService)()

                        Await leaveLedgerRepository.CreateManyLeaveTransactionsAsync(leaveTransactions:=New List(Of LeaveTransaction) From {lt})

                        leaveLedger.LastTransactionID = lt.RowID

                        Dim tasks = Task.WhenAll(
                            soloParentBeneficiaryDataService.SaveManyAsync(deleted:=New List(Of SoloParentBeneficiary) From {_soloParentBeneficiary},
                                currentlyLoggedInUserId:=_userId),
                            leaveLedgerRepository.UpdateManyAsync(New List(Of LeaveLedger) From {leaveLedger})
                        )

                        Await tasks.
                            ContinueWith(Async Function(ant)
                                             If ant.IsCompleted AndAlso Not ant.IsFaulted Then DialogResult = DialogResult.OK

                                         End Function)

                    End Function)

    End Sub

    Private Async Sub Label20_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles Label20.LinkClicked
        Await _soloParentBeneficiary.ViewFileAsync()
    End Sub

End Class
