Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Data.Entities
Imports AccuPay.Data.Repositories
Imports AccuPay.Data.Services
Imports AccuPay.Desktop.Utilities
Imports AccuPay.Utilities
Imports Microsoft.Extensions.DependencyInjection

Public Class AddLoanScheduleForm

    Private _currentEmployee As Employee

    Private _newLoanSchedule As New LoanSchedule()

    Public Property IsSaved As Boolean

    Public Property ShowBalloonSuccess As Boolean

    Sub New(employee As Employee)

        InitializeComponent()

        _currentEmployee = employee

        Me.IsSaved = False

    End Sub

    Private Sub AddLoanScheduleForm_Load(sender As Object, e As EventArgs) Handles Me.Load

        PopulateEmployeeData()

        ResetForm()

    End Sub

    Private Sub ResetForm()
        Me._newLoanSchedule = New LoanSchedule
        Me._newLoanSchedule.EmployeeID = _currentEmployee.RowID
        Me._newLoanSchedule.OrganizationID = z_OrganizationID

        Me._newLoanSchedule.DedEffectiveDateFrom = Date.Now
        Me._newLoanSchedule.Status = LoanSchedule.STATUS_IN_PROGRESS

        LoanUserControl1.SetLoan(Me._newLoanSchedule, isNew:=True)
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

        Dim confirmMessage = ""
        Dim messageTitle = "New Loan"

        If Me._newLoanSchedule.TotalLoanAmount = 0 AndAlso Me._newLoanSchedule.DeductionAmount = 0 Then
            confirmMessage = "You did not enter a value for Total Loan Amount and Deduction Amount. Do you want to save the new loan?"

        ElseIf Me._newLoanSchedule.TotalLoanAmount = 0 Then
            confirmMessage = "You did not enter a value for Total Loan Amount. Do you want to save the new loan?"

        ElseIf Me._newLoanSchedule.DeductionAmount = 0 Then
            confirmMessage = "You did not enter a value for Deduction Amount. Do you want to save the new loan?"

        End If

        If String.IsNullOrWhiteSpace(confirmMessage) = False Then

            If MessageBoxHelper.Confirm(Of Boolean) _
                (confirmMessage, messageTitle, messageBoxIcon:=MessageBoxIcon.Warning) = False Then Return

        End If

        Dim loan = LoanUserControl1.GetLoan()

        Await FunctionUtils.TryCatchFunctionAsync(messageTitle,
            Async Function()

                Dim dataService = MainServiceProvider.GetRequiredService(Of LoanDataService)

                Me._newLoanSchedule = Await dataService.SaveAsync(loan, z_User)

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
