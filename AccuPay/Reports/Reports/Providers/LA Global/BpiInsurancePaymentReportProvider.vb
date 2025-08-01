Option Strict On

Imports AccuPay.Core.Entities
Imports AccuPay.Core.Interfaces
Imports AccuPay.Core.Services
Imports AccuPay.Desktop.Utilities
Imports Microsoft.Extensions.DependencyInjection

Public Class BpiInsurancePaymentReportProvider
    Implements ILaGlobalEmployeeReport

    Private _selectedDate As Date
    Public Property Employee As Employee Implements ILaGlobalEmployeeReport.Employee

    Private _reportDocument As BpiInsuranceAmountReport

    Sub New()
        _reportDocument = New BpiInsuranceAmountReport()
    End Sub

    Public Function Output() As Boolean Implements ILaGlobalEmployeeReport.Output
        Dim monthSelector = New selectMonth()

        Dim succeed = monthSelector.ShowDialog = DialogResult.OK
        If Not succeed Then Return False

        _selectedDate = CDate(monthSelector.MonthFirstDate)

        Try

            SetDataSource()

            succeed = True
        Catch ex As Exception
            Throw ex
        End Try
        Return succeed
    End Function

    Private Async Sub SetDataSource()

        ' Any thrown exceptions from this function will not be
        ' handled by the calling function above.
        Dim dataService = MainServiceProvider.GetRequiredService(Of IBpiInsuranceAmountReportDataService)

        Dim source As New List(Of BpiInsuranceDataSource)
        source = (Await dataService.GetData(
                organizationId:=z_OrganizationID,
                userId:=z_User,
                _selectedDate)).
            ToList()

        If source.Any() = False Then

            MessageBoxHelper.Warning("No record found.")
            Return
        End If

        _reportDocument.SetDataSource(source)

        Dim parameterSetter = New CrystalReportParameterValueSetter(_reportDocument)
        With parameterSetter
            .SetParameter("organizationName", z_CompanyName)

        End With

        Dim form = New LaGlobalEmployeeReportForm
        form.SetReportSource(_reportDocument)
        form.Show()
    End Sub

End Class
