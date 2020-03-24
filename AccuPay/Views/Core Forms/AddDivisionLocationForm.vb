Imports AccuPay.Data.Repositories
Imports AccuPay.Entity
Imports AccuPay.Repository
Imports AccuPay.Utils

Public Class AddDivisionLocationForm

    Private _divisionRepository As New DivisionRepository

    Public Property NewDivision As Division

    Public Property IsSaved As Boolean

    Private Sub AddDivisionLocationForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Me.NewDivision = Division.CreateEmptyDivision()

        Me.IsSaved = False

        txtDivisionName.Focus()

    End Sub

    Private Async Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click

        If String.IsNullOrWhiteSpace(txtDivisionName.Text) Then

            MessageBoxHelper.ErrorMessage("Please provide a division location name.")

            Return

        End If

        Const messageTitle As String = "New Division Location"


        Try
            Me.NewDivision.Name = txtDivisionName.Text.Trim

            Me.NewDivision = Await _divisionRepository.SaveAsync(Me.NewDivision)

            Dim repo As New UserActivityRepository
            repo.RecordAdd(z_User, "Division Location")

            Me.IsSaved = True

            Me.Close()

        Catch ex As ArgumentException

            MessageBoxHelper.ErrorMessage(ex.Message, messageTitle)

        Catch ex As Exception

            MessageBoxHelper.DefaultErrorMessage(messageTitle, ex)

        End Try

    End Sub
End Class