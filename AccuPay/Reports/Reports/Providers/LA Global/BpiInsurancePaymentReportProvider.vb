Imports AccuPay.Entity
Imports CrystalDecisions.CrystalReports.Engine
Imports Microsoft.EntityFrameworkCore

Public Class BpiInsurancePaymentReportProvider
    Implements ILaGlobalEmployeeReport

    Private _selectedDate As Date
    Public Property Employee As Employee Implements ILaGlobalEmployeeReport.Employee

    Public Function Output() As Boolean Implements ILaGlobalEmployeeReport.Output
        Dim monthSelector = New selectMonth()

        Dim succeed = monthSelector.ShowDialog = DialogResult.OK
        If Not succeed Then Return False

        _selectedDate = CDate(monthSelector.MonthFirstDate)

        Try
            Dim reportDocument As New BpiInsuranceAmountReport

            Dim parameterSetter = New CrystalReportParameterValueSetter(reportDocument)
            With parameterSetter
                .SetParameter("selectedDate", _selectedDate)
                .SetParameter("noRecordFound", True)

            End With

            Dim form = New LaGlobalEmployeeReportForm
            form.reportViewer.ReportSource = reportDocument
            form.Show()

            succeed = True
        Catch ex As Exception
            Throw ex
        End Try
        Return succeed
    End Function

    Private Async Sub SetDataSource(reportClass As ReportClass, parameterSetter As CrystalReportParameterValueSetter)

        Using context = New PayrollContext
            Dim periods = Await context.PayPeriods.Where(Function(p) p.OrganizationID.Value = z_OrganizationID).
                Where(Function(p) p.Year = _selectedDate.Year).
                Where(Function(p) p.Month = _selectedDate.Month).
                Where(Function(p) p.PayFrequencyID = 1).
                ToListAsync()

            Dim periodIDs = periods.Select(Function(p) p.RowID.Value).ToArray()


        End Using
        'reportClass.SetDataSource()
    End Sub

End Class