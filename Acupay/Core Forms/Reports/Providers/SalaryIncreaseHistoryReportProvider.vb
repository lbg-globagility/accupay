Option Strict On

Imports CrystalDecisions.CrystalReports.Engine

Public Class SalaryIncreaseHistoryReportProvider
    Implements IReportProvider

    Public Property Name As String = "Salary Increase History" Implements IReportProvider.Name

    Public Sub Run() Implements IReportProvider.Run
        Dim payPeriodSelector As New PayrollSummaDateSelection()

        If Not payPeriodSelector.ShowDialog = Windows.Forms.DialogResult.OK Then
            Return
        End If

        Dim dateFrom = payPeriodSelector.DateFrom
        Dim dateTo = payPeriodSelector.DateTo

        Dim params = New Object(,) {
            {"OrganizID", orgztnID},
            {"PayPerDate1", dateFrom},
            {"PayPerDate2", dateTo}
        }

        Dim data = DirectCast(callProcAsDatTab(params, "RPT_salary_increase_histo"), DataTable)

        LinkSalaryToPreviousSalary(data)
        Dim report = New Employees_History_of_Salary_Increase()
        report.SetDataSource(data)

        Dim crvwr As New CrysRepForm
        crvwr.crysrepvwr.ReportSource = report
        crvwr.Show()
    End Sub

    Private Sub LinkSalaryToPreviousSalary(data As DataTable)
        Dim column = New DataColumn("DatCol10", System.Type.GetType("System.Decimal"))
        data.Columns.Add(column)

        Dim previousRow As DataRow = Nothing

        For Each currentRow As DataRow In data.Rows
            Dim currentEmployeeID = ConvertToType(Of Integer?)(currentRow.Item("DatCol1"))
            Dim previousEmployeeID = ConvertToType(Of Integer?)(previousRow?.Item("DatCol1"))

            If currentEmployeeID = previousEmployeeID Then
                Dim currentSalary = ConvertToType(Of Decimal)(currentRow("DatCol9"))
                Dim previousSalary = ConvertToType(Of Decimal)(previousRow("DatCol9"))

                currentRow.Item("DatCol10") = currentSalary - previousSalary
            End If

            previousRow = currentRow
        Next
    End Sub

End Class
