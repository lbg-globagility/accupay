Option Strict On

Imports AccuPay.Data.Entities
Imports AccuPay.Data.Services
Imports AccuPay.Utils

Public Class BpiInsurancePaymentReportProvider
    Implements ILaGlobalEmployeeReport

    Private _selectedDate As Date
    Public Property Employee As Employee Implements ILaGlobalEmployeeReport.Employee

    Private _reportDocument As BpiInsuranceAmountReport

    Private ReadOnly _dataService As BpiInsuranceAmountReportDataService

    Sub New(dataService As BpiInsuranceAmountReportDataService)

        _reportDocument = New BpiInsuranceAmountReport()

        _dataService = dataService
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

        ' Any thrown exceptions from this function will not be handled by the calling
        ' function above because of the way these report providers were poorly structured.
        ' The one who coded these report providers could have just use the pattern on how the
        ' main report providers were created but chose not to for some reason. (*scratches head)
        Dim source As New List(Of BpiInsuranceAmountReportDataService.BpiInsuranceDataSource)
        source = (Await _dataService.GetData(z_OrganizationID,
                                            z_User,
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