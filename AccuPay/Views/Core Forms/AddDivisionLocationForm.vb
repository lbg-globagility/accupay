Option Strict On

Imports AccuPay.Data.Entities
Imports AccuPay.Data.Repositories
Imports AccuPay.Data.Services
Imports AccuPay.Utils
Imports Microsoft.Extensions.DependencyInjection

Public Class AddDivisionLocationForm

    Private Const FormEntityName As String = "Division Location"

    Public Property NewDivision As Division

    Public Property IsSaved As Boolean

    Private _userActivityRepository As UserActivityRepository

    Sub New()

        InitializeComponent()

        _userActivityRepository = MainServiceProvider.GetRequiredService(Of UserActivityRepository)

    End Sub

    Private Sub AddDivisionLocationForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Me.NewDivision = Division.CreateEmptyDivision(organizationId:=z_OrganizationID, userId:=z_User)

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

            Dim divisionService = MainServiceProvider.GetRequiredService(Of DivisionDataService)

            Await divisionService.SaveAsync(Me.NewDivision)

            _userActivityRepository.RecordAdd(z_User, FormEntityName, Me.NewDivision.RowID.Value, z_OrganizationID)

            Me.IsSaved = True

            Me.Close()
        Catch ex As ArgumentException

            MessageBoxHelper.ErrorMessage(ex.Message, messageTitle)
        Catch ex As Exception

            MessageBoxHelper.DefaultErrorMessage(messageTitle, ex)

        End Try

    End Sub

End Class