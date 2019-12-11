Option Strict On

Imports AccuPay.Entity
Imports CrystalDecisions.CrystalReports.Engine
Imports Microsoft.EntityFrameworkCore

Public Class FiledLeaveReportProvider
    Implements IReportProvider

    Public Property Name As String = "Filed Leave" Implements IReportProvider.Name

    Private orgNameAddressSql As String =
        "SELECT
        CONCAT_WS('\n'
        , UCASE(og.`Name`)
        , CONCAT_WS(', '
                , a.StreetAddress1
                , a.StreetAddress2
                , a.Barangay
                , a.CityTown
                , a.Country
                , a.State
                , a.ZipCode)
		        )
        `FullAddress`
        FROM organization og
        LEFT JOIN (SELECT RowID
			        , IF(LENGTH(TRIM(StreetAddress1)) = 0, NULL, TRIM(StreetAddress1)) `StreetAddress1`
			        , IF(LENGTH(TRIM(StreetAddress2)) = 0, NULL, TRIM(StreetAddress2)) `StreetAddress2`
			        , IF(LENGTH(TRIM(Barangay)) = 0, NULL, TRIM(Barangay)) `Barangay`
			        , IF(LENGTH(TRIM(CityTown)) = 0, NULL, TRIM(CityTown)) `CityTown`
			        , IF(LENGTH(TRIM(Country)) = 0, NULL, TRIM(Country)) `Country`
			        , IF(LENGTH(TRIM(State)) = 0, NULL, TRIM(State)) `State`
			        , IF(LENGTH(TRIM(ZipCode)) = 0, NULL, TRIM(ZipCode)) `ZipCode`

			        FROM address) a ON a.RowID=og.PrimaryAddressID
        WHERE og.RowID = ?ogId;"

    Public Async Sub Run() Implements IReportProvider.Run
        Dim dateSelector As New PayrollSummaDateSelection()

        If Not dateSelector.ShowDialog = Windows.Forms.DialogResult.OK Then
            Return
        End If

        Dim dateFrom = dateSelector.DateFrom.Value
        Dim dateTo = dateSelector.DateTo.Value

        Dim data = Await CreateFiledLeaveDataTable(dateFrom, dateTo)

        Dim report = New FiledLeaveReport()
        report.SetDataSource(data)

        Dim title = DirectCast(report.ReportDefinition.Sections(1).ReportObjects("txtCutoffDate"), TextObject)
        Dim dateFromTitle = dateFrom.ToString("MMMM dd, yyyy")
        Dim dateToTitle = dateTo.ToString("MMMM dd, yyyy")
        title.Text = $"From {dateFromTitle} to {dateToTitle}"

        Dim sql As New SQL(orgNameAddressSql,
                           New Object() {orgztnID})
        Dim result = Convert.ToString(sql.GetFoundRow)
        Dim splitValues = Split(result, Chr(10))

        Dim txtCompanyName = DirectCast(report.ReportDefinition.Sections(1).ReportObjects("txtCompanyName"), TextObject)
        txtCompanyName.Text = splitValues.First

        Dim txtAddress = DirectCast(report.ReportDefinition.Sections(1).ReportObjects("txtAddress"), TextObject)
        txtAddress.Text = splitValues.Last

        Dim viewer As New CrysRepForm()
        viewer.crysrepvwr.ReportSource = report
        viewer.Show()
    End Sub

    Public Async Function CreateFiledLeaveDataTable(startDate As Date, endDate As Date) As Threading.Tasks.Task(Of DataTable)

        Dim datatable As New DataTable

        datatable.Columns.Add("DatCol1")
        datatable.Columns.Add("DatCol2")
        datatable.Columns.Add("DatCol3")
        datatable.Columns.Add("DatCol11")
        datatable.Columns.Add("DatCol12")
        datatable.Columns.Add("DatCol13")
        datatable.Columns.Add("DatCol14")
        datatable.Columns.Add("DatCol15")
        datatable.Columns.Add("DatCol16")

        Using context As New PayrollContext

            Dim leaveTransactions = Await context.LeaveTransactions.
                                    Include(Function(l) l.Employee).
                                    Include(Function(l) l.LeaveLedger.Product).
                                    Include(Function(l) l.Leave).
                                    Where(Function(l) l.TransactionDate >= startDate).
                                    Where(Function(l) l.TransactionDate <= endDate).
                                    Where(Function(l) l.OrganizationID.Value = z_OrganizationID).
                                    Where(Function(l) l.Type = LeaveTransactionType.Debit).
                                    Where(Function(l) l.Amount <> 0).
            OrderBy(Function(l) l.TransactionDate).
                                    ToListAsync

            For Each transaction In leaveTransactions

                Dim newRow = datatable.NewRow()

                newRow("DatCol1") = transaction.EmployeeID
                newRow("DatCol2") = transaction.Employee.EmployeeNo
                newRow("DatCol3") = transaction.Employee.FullNameWithMiddleInitialLastNameFirst
                newRow("DatCol11") = transaction.TransactionDate.ToString("dddd")
                newRow("DatCol12") = transaction.TransactionDate.ToString("MM/dd/yyyy")
                newRow("DatCol13") = transaction.LeaveLedger.Product.PartNo
                newRow("DatCol14") = transaction.Amount
                newRow("DatCol15") = transaction.Amount / PayrollTools.WorkHoursPerDay
                newRow("DatCol16") = transaction.Leave?.Reason

                datatable.Rows.Add(newRow)
            Next

        End Using

        Return datatable

    End Function

End Class