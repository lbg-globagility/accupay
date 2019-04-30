Imports AccuPay.Entity
Imports AccuPay.Utils
Imports Microsoft.EntityFrameworkCore

Public Class AddAllowanceTypeForm
    Public Property NewAllowanceType As Product

    Public Property IsSaved As Boolean


    Private Sub AddLoanTypeForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Me.NewAllowanceType = New Product

        Me.IsSaved = False

        txtAllowanceType.Focus()

    End Sub

    Private Async Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click
        If String.IsNullOrWhiteSpace(txtAllowanceType.Text) Then

            MessageBoxHelper.ErrorMessage("Please provide a allowance type.")

            Return

        End If

        Const messageTitle As String = "New Allowance Type"


        Try
            Using context As New PayrollContext
                Dim product As New Product
                Dim allowanceName = (txtAllowanceType.Text).Trim()

                product.PartNo = allowanceName.Trim()
                product.Name = allowanceName.Trim()

                product.Category = ProductConstant.ALLOWANCE_TYPE_CATEGORY

                product.Created = Date.Now
                product.CreatedBy = z_User
                product.OrganizationID = z_OrganizationID

                context.Products.Add(product)

                Await context.SaveChangesAsync()

                Dim newProduct = Await context.Products.FirstOrDefaultAsync(Function(p) Nullable.Equals(p.RowID, product.RowID))

                If newProduct Is Nothing Then
                    Throw New ArgumentException("There was a problem inserting the new allowance type. Please try again.")
                End If

                Me.NewAllowanceType = newProduct
            End Using


            Me.IsSaved = True

            Me.Close()

        Catch ex As ArgumentException

            MessageBoxHelper.ErrorMessage(ex.Message, messageTitle)

        Catch ex As Exception

            MessageBoxHelper.DefaultErrorMessage(messageTitle, ex)

        End Try
    End Sub
End Class