Option Strict On

Imports AccuPay.Core.Entities
Imports AccuPay.Core.Interfaces
Imports AccuPay.Desktop.Utilities
Imports Microsoft.Extensions.DependencyInjection

Public Class AddLoanForm

    Private _currentEmployee As Employee

    Private _newLoan As LoanModel

    Public Property IsSaved As Boolean

    Public Property ShowBalloonSuccess As Boolean

    Private ReadOnly _policy As IPolicyHelper

    Sub New(employee As Employee)

        InitializeComponent()

        _currentEmployee = employee

        Me.IsSaved = False

        _policy = MainServiceProvider.GetRequiredService(Of IPolicyHelper)

    End Sub

    Private Sub AddLoanForm_Load(sender As Object, e As EventArgs) Handles Me.Load

        PopulateEmployeeData()

        ResetForm()

    End Sub

    Private Sub ResetForm()
        Dim newLoan As New Loan With {
            .EmployeeID = _currentEmployee.RowID,
            .OrganizationID = z_OrganizationID,
            .DedEffectiveDateFrom = Date.Now,
            .Status = Loan.STATUS_IN_PROGRESS
        }

        If _policy.UseGoldwingsLoanInterest Then
            newLoan.DeductionPercentage = _policy.GoldWingsLoanInterestDefault
        End If

        _newLoan = LoanModel.Create(newLoan)

        LoanUserControl1.SetLoan(_newLoan, isNew:=True)
    End Sub

    Private Sub PopulateEmployeeData()

        txtEmployeeFirstName.Text = _currentEmployee?.FullNameWithMiddleInitial

        txtEmployeeNumber.Text = _currentEmployee?.EmployeeIdWithPositionAndEmployeeType

        pbEmployeePicture.Image = ConvByteToImage(_currentEmployee.Image)

    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Async Sub AddLoanButtonClicked(sender As Object, e As EventArgs) _
        Handles btnAddAndNew.Click, btnAddAndClose.Click

        Dim messageTitle = "New Loan"

        Dim loan = LoanUserControl1.GetLoan()

        Await FunctionUtils.TryCatchFunctionAsync(messageTitle,
            Async Function()

                Dim dataService = MainServiceProvider.GetRequiredService(Of ILoanDataService)

                Me._newLoan = LoanModel.Create(Await dataService.SaveAsync(loan, z_User))

                Me.IsSaved = True

                If sender Is btnAddAndNew Then
                    ShowBalloonInfo("Loan Successfully Added", "Saved")

                    ResetForm()
                Else

                    Me.ShowBalloonSuccess = True
                    Me.Close()
                End If
            End Function)

    End Sub

#Region "Private Functions"

    Private Sub ShowBalloonInfo(content As String, title As String)
        myBalloon(content, title, EmployeeInfoTabLayout, 400)
    End Sub

#End Region

End Class
