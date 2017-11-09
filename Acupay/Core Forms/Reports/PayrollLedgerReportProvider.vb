Option Strict On

Public Class PayrollLedgerReportProvider
    Implements ReportProvider

    Public Property Name As String = "Payroll Ledger" Implements ReportProvider.Name

    Public Sub Run() Implements ReportProvider.Run
        Dim payperiodSelector = New PayrollSummaDateSelection()

        If Not payperiodSelector.ShowDialog() = DialogResult.OK Then
            Return
        End If

        Dim startPayPeriodID = payperiodSelector.DateFromID
        Dim endPayPeriodID = payperiodSelector.DateToID

        Dim params = New Object(,) {
            {"OrganizID", z_OrganizationID},
            {"PayPerID1", startPayPeriodID},
            {"PayPerID2", endPayPeriodID},
            {"psi_undeclared", 1}
        }

        Dim data = DirectCast(callProcAsDatTab(params, "RPT_payroll_legder"), DataTable)

        Dim payrollLedger = New Employees_Payroll_Ledger()
        payrollLedger.SetDataSource(data)

        Dim crvwr As New CrysRepForm
        crvwr.crysrepvwr.ReportSource = payrollLedger
        crvwr.Show()
    End Sub

End Class
