Option Strict On

Public Class PayrollLedgerReportProvider
    Implements IReportProvider

    Public Property Name As String = "Payroll Ledger" Implements IReportProvider.Name

    Public Sub Run() Implements IReportProvider.Run
        Dim payperiodSelector = New PayrollSummaDateSelection()

        If Not payperiodSelector.ShowDialog() = DialogResult.OK Then
            Return
        End If

        Dim startPayPeriodID = payperiodSelector.PayPeriodFromID
        Dim endPayPeriodID = payperiodSelector.PayPeriodToID

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
