Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Data.Entities
Imports AccuPay.Data.Repositories
Imports AccuPay.Data.Services
Imports AccuPay.Desktop.Utilities
Imports AccuPay.Utilities
Imports Microsoft.Extensions.DependencyInjection

Public Class AddLoanForm

    Private _currentEmployee As Employee

    Private _newLoanSchedule As LoanModel

    Public Property IsSaved As Boolean
    Private ReadOnly _policy As IPolicyHelper
    Public Property ShowBalloonSuccess As Boolean

    Sub New(employee As Employee)

        InitializeComponent()

        _currentEmployee = employee

        Me.IsSaved = False

        _policy = MainServiceProvider.GetRequiredService(Of IPolicyHelper)

    End Sub

    Private Sub AddLoanScheduleForm_Load(sender As Object, e As EventArgs) Handles Me.Load

        PopulateEmployeeData()

        ResetForm()

    End Sub

    Private Sub ResetForm()
        Dim newLoanSchedule As New LoanSchedule With {
            .EmployeeID = _currentEmployee.RowID,
            .OrganizationID = z_OrganizationID,
            .DedEffectiveDateFrom = Date.Now,
            .Status = LoanSchedule.STATUS_IN_PROGRESS
        }

        _newLoanSchedule = LoanModel.Create(newLoanSchedule)

        LoanUserControl1.SetLoan(_newLoanSchedule, isNew:=True)
    End Sub

    Private Sub PopulateEmployeeData()

        txtEmployeeFirstName.Text = _currentEmployee?.FullNameWithMiddleInitial

        txtEmployeeNumber.Text = _currentEmployee?.EmployeeIdWithPositionAndEmployeeType

        pbEmployeePicture.Image = ConvByteToImage(_currentEmployee.Image)

    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.Close()
    End Sub

    Private Async Sub AddLoanScheduleButtonClicked(sender As Object, e As EventArgs) _
        Handles btnAddAndNew.Click, btnAddAndClose.Click

        Dim messageTitle = "New Loan"

        Dim loan = LoanUserControl1.GetLoan()

        Await FunctionUtils.TryCatchFunctionAsync(messageTitle,
            Async Function()

                Dim dataService = MainServiceProvider.GetRequiredService(Of LoanDataService)

                Me._newLoanSchedule = LoanModel.Create(Await dataService.SaveAsync(loan, z_User))

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
