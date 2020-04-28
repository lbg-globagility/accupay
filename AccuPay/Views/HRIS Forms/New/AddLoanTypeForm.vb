Option Strict On

Imports AccuPay.Data.Entities
Imports AccuPay.Data.Repositories
Imports AccuPay.Utils

Public Class AddLoanTypeForm

    Private _productRepository As New ProductRepository

    Public Property NewLoanType As Product

    Public Property IsSaved As Boolean

    Private Sub AddLoanTypeForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Me.NewLoanType = New Product

        Me.IsSaved = False

        txtLoanName.Focus()

    End Sub

    Private Async Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click

        If String.IsNullOrWhiteSpace(txtLoanName.Text) Then

            MessageBoxHelper.ErrorMessage("Please provide a loan name.")

            Return

        End If

        Const messageTitle As String = "New Loan Type"

        Try
            Me.NewLoanType = Await _productRepository.AddLoanType(txtLoanName.Text,
                                                                 organizationId:=z_OrganizationID,
                                                                 userId:=z_User)

            Me.IsSaved = True

            Me.Close()
        Catch ex As ArgumentException

            MessageBoxHelper.ErrorMessage(ex.Message, messageTitle)
        Catch ex As Exception

            MessageBoxHelper.DefaultErrorMessage(messageTitle, ex)

        End Try

    End Sub

End Class