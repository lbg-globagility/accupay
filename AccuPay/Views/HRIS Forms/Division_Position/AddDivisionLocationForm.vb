Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Data.Entities
Imports AccuPay.Data.Services
Imports AccuPay.Desktop.Utilities
Imports Microsoft.Extensions.DependencyInjection

Public Class AddDivisionLocationForm

    Public Property NewDivision As Division

    Public Property IsSaved As Boolean

    Private Sub AddDivisionLocationForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Me.NewDivision = Division.NewDivision(organizationId:=z_OrganizationID, userId:=z_User)

        Me.IsSaved = False

        txtDivisionName.Focus()

    End Sub

    Private Async Sub btnAdd_Click(sender As Object, e As EventArgs) Handles btnAdd.Click

        If String.IsNullOrWhiteSpace(txtDivisionName.Text) Then

            MessageBoxHelper.ErrorMessage("Please provide a division location name.")

            Return

        End If

        Const messageTitle As String = "New Division Location"

        Await FunctionUtils.TryCatchFunctionAsync(messageTitle,
            Async Function()
                Await SaveDivisionLocation()
            End Function)

    End Sub

    Private Async Function SaveDivisionLocation() As Task
        Me.NewDivision.Name = txtDivisionName.Text.Trim

        Dim divisionService = MainServiceProvider.GetRequiredService(Of DivisionDataService)

        Await divisionService.SaveAsync(Me.NewDivision)

        Me.IsSaved = True

        Me.Close()
    End Function

End Class
