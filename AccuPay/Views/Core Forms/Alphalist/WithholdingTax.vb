Imports AccuPay.AlphalistDataset
Imports AccuPay.DB

Public Class WithholdingTax

    Private Const BASIC_TAX_EXEMPTION As Decimal = 50000

    Private Alphalist As Alphalistv2 'Alphalist
    Private SelectedYear As Integer

    Private batchId As UInteger?

    Private withholdingTaxReports As WithholdingTaxReportDataTable
    Private selectedReport As WithholdingTaxReportRow

    Private WAGE_RATE As Decimal = 466

    Private categoryItems As New Dictionary(Of String, String) From
        {{"", ""},
         {"Schedule 7.1", "Schedule 7.1 (Terminated before December 31)"},
         {"Schedule 7.3", "Schedule 7.3 (Taxable employees)"},
         {"Schedule 7.4", "Schedule 7.4 (Has a previous employer)"},
         {"Schedule 7.5", "Schedule 7.5 (Minimum wage employees)"}}

    Private Sub LoadWithholdingTaxBatches()
        Dim sql = "SELECT * FROM withholdingtaxbatch WHERE OrganizationID=" & orgztnID & ";"
        Dim withholdingTaxBatches As DataTable = getDataTableForSQL(sql)

        dgvYear.Rows.Clear()

        For Each row As DataRow In withholdingTaxBatches.Rows
            Dim idx = dgvYear.Rows.Add()
            dgvYear.Rows(idx).Cells("Year").Value = row("Year")
            dgvYear.Rows(idx).Cells("colBatchID").Value = row("RowID")
        Next
    End Sub

    Private Sub LoadWithholdingTaxReports(ByVal batchId As Integer, ByVal year As Integer)
        Dim params = {
            {"BatchID", batchId},
            {"OrganizationID", orgztnID}
        }

        Dim source As DataTable = callProcAsDatTab(params, "LoadWithholdingTaxReports")

        withholdingTaxReports = LoadWithholdingTaxData(year)

        For Each row As WithholdingTaxReportRow In withholdingTaxReports.Rows
            Dim employeeId = row.EmployeeID

            Dim sourceRow = source.Select("EmployeeID = " & employeeId) '.First()

            For Each srdata In sourceRow
                row.PreviousTaxableIncome = srdata("PreviousTaxableIncome")
                row.PremiumPaidOnHealth = srdata("PremiumPaidOnHealth")
                row.PreviousTaxWithheld = srdata("PreviousTaxWithheld")
                row.HazardPay = srdata("HazardPay")
                row.DeMinimisBenefits = srdata("DeMinimisBenefits")
                row.SalariesAndOtherCompensation = srdata("SalariesAndOtherCompensation")
                row.Representation = srdata("Representation")
                row.Transportation = srdata("Transportation")
                row.CostOfLivingAllowance = srdata("CostOfLivingAllowance")
                row.FixedHousingAllowance = srdata("FixedHousingAllowance")
                row.OthersAName = srdata("OthersAName")
                row.OthersAAmount = srdata("OthersAAmount")
                row.OthersBName = srdata("OthersBName")
                row.OthersBAmount = srdata("OthersBAmount")
                row.Commission = srdata("Commission")
                row.ProfitSharing = srdata("ProfitSharing")
                row.FeesInclDirectorsFees = srdata("FeesInclDirectorsFee")
                row.Taxable13thMonthPay = srdata("Taxable13thMonthPay")
                row.TaxableHazardPay = srdata("TaxableHazardPay")
                row.TaxableOvertimePay = srdata("TaxableOvertimePay")
                row.SupplementaryAAmount = srdata("SupplementaryAAmount")
                row.SupplementaryBAmount = srdata("SupplementaryBAmount")
                Exit For
            Next
        Next
    End Sub

    Private Function Initialize(ByVal annualizedWithholdingTax As DataTable, ByVal employees As DataTable, ByVal grossCompensation As DataTable) As WithholdingTaxReportDataTable
        Dim withholdingTaxReports = New WithholdingTaxReportDataTable()

        Dim firstDayOfYear = New Date(SelectedYear, 1, 1)
        Dim lastDayOfYear = New Date(SelectedYear, 12, 31)

        For Each employee As DataRow In employees.Rows
            Dim row = withholdingTaxReports.NewWithholdingTaxReportRow()
            Dim employeeId = employee("RowID")

            row.EmployeeID = employee("RowID")
            row.TinNo = employee("TINNo")
            row.FirstName = employee("FirstName")
            row.MiddleName = employee("MiddleName")
            row.LastName = employee("LastName")
            row.ExemptionStatus = employee("ExemptionStatus")

            row.StartDate = employee("StartDate")
            row.EndDate = employee("TerminationDate").ToString()

            row.WorkDaysPerYear = employee("WorkDaysPerYear")

            row.IsMinimumWageEarner = CDec(employee("BasicPay")) <= WAGE_RATE

            ' Get the thirteenth month pay of the employee
            Dim grossCompensationRow = grossCompensation.Select("EmployeeID = " & employeeId)
            row._13thMonthPay = If(grossCompensationRow.Count = 0, 0, grossCompensationRow.First()("ThirteenthMonthPay"))

            row.HolidayPay = annualizedWithholdingTax.Compute("SUM(HolidayPay)", "EmployeeID = " & employeeId).ToString()
            row.OvertimePay = annualizedWithholdingTax.Compute("SUM(OvertimePay)", "EmployeeID = " & employeeId).ToString()
            row.NightDiffPay = annualizedWithholdingTax.Compute("SUM(NightDiffPay)", "EmployeeID = " & employeeId).ToString()
            row.TaxDue = ParseDecimal(annualizedWithholdingTax.Compute("SUM(TotalEmpWithholdingTax)", "EmployeeID = " & employeeId).ToString())

            Dim sssContribution = ParseDecimal(annualizedWithholdingTax.Compute("SUM(TotalEmpSSS)", "EmployeeID = " & employeeId).ToString())
            Dim philHealthContribution = ParseDecimal(annualizedWithholdingTax.Compute("SUM(TotalEmpPhilHealth)", "EmployeeID = " & employeeId).ToString())
            Dim hdmfContribution = ParseDecimal(annualizedWithholdingTax.Compute("SUM(TotalEmpHDMF)", "EmployeeID = " & employeeId).ToString())
            Dim governmentInsurance = sssContribution + philHealthContribution + hdmfContribution
            row.GovernmentInsurance = governmentInsurance

            Dim allowance = ParseDecimal(annualizedWithholdingTax.Compute("SUM(TotalAllowance)", "EmployeeID = " & employeeId).ToString())
            Dim bonus = ParseDecimal(annualizedWithholdingTax.Compute("SUM(TotalBonus)", "EmployeeID = " & employeeId).ToString())
            Dim grossSalary = ParseDecimal(annualizedWithholdingTax.Compute("SUM(TotalGrossSalary)", "EmployeeID = " & employeeId).ToString())
            Dim basicPay = grossSalary - allowance - bonus - governmentInsurance

            If row.IsMinimumWageEarner Then
                row.BasicSalary = basicPay
            Else
                row.TaxableBasicSalary = basicPay
            End If

            Recompute(row)

            Dim startDate = DateTime.Parse(row.StartDate)

            ' Set the correct alphalist category the employee belongs to
            If startDate > firstDayOfYear Then
                row.Category = "Schedule 7.4"
            ElseIf row.EmploymentStatus = "Resigned" Or row.EmploymentStatus = "Terminated" Then
                row.Category = "Schedule 7.1"
            ElseIf row.IsMinimumWageEarner Then
                row.Category = "Schedule 7.5"
            Else
                row.Category = "Schedule 7.3"
            End If

            withholdingTaxReports.AddWithholdingTaxReportRow(row)
        Next

        Return withholdingTaxReports
    End Function

    Private Sub PopulateEmployeeList(ByVal employees As WithholdingTaxReportDataTable)
        dgvEmployees.Rows.Clear()

        For Each employee As WithholdingTaxReportRow In employees.Rows
            Dim idx = dgvEmployees.Rows.Add()

            dgvEmployees.Rows(idx).Cells(0).Value = employee.EmployeeID
            dgvEmployees.Rows(idx).Cells(1).Value = employee.LastName & ", " & employee.FirstName & " " & employee.MiddleName
        Next
    End Sub

    ''' <summary>
    ''' Load the annualized withholding tax report from the database
    ''' </summary>
    ''' <param name="Year">The tax year of the annual report</param>
    Private Function LoadWithholdingTaxData(ByVal year As Integer) As WithholdingTaxReportDataTable
        Dim taxDateFrom As Date = New Date(year, 1, 1)
        Dim taxDateTo As Date = New Date(year, 12, 31)

        Dim params(3, 1) As Object

        params(0, 0) = "OrganizID"
        params(1, 0) = "AnnualDateFrom"
        params(2, 0) = "AnnualDateTo"
        params(3, 0) = "IsActual"

        params(0, 1) = orgztnID
        params(1, 1) = Format(taxDateFrom, "yyyy-MM-dd")
        params(2, 1) = Format(taxDateTo, "yyyy-MM-dd")
        params(3, 1) = 0

        Dim annualizedWithholdingTax = callProcAsDatTab(params, "RPT_AnnualizedWithholdingTax")
        Dim employees = New SQLQueryToDatatable("CALL `RPT_AlphaListemployee`('" & orgztnID & "');").ResultTable

        Dim paramet(2, 1) As Object
        paramet(0, 0) = "OrganizID"
        paramet(1, 0) = "LastDateOfFinancialYear"
        paramet(2, 0) = "FirstDateOfFinancialYear"

        paramet(0, 1) = orgztnID
        paramet(1, 1) = Format(taxDateTo, "yyyy-MM-dd")
        paramet(2, 1) = Format(taxDateFrom, "yyyy-MM-dd")

        Dim grossCompensation = callProcAsDatTab(paramet, "RPT_getGrossCompensation")

        Dim withholdingTaxReports = Initialize(annualizedWithholdingTax, employees, grossCompensation)
        PopulateEmployeeList(withholdingTaxReports)

        Return withholdingTaxReports
    End Function

    Private Sub btnNewAlphalist_Click(sender As Object, e As EventArgs) Handles btnNewAlphalist.Click
        Dim dialog = New TaxYearDialog()
        dialog.ShowDialog()

        If dialog.DialogResult = Windows.Forms.DialogResult.OK Then
            SelectedYear = dialog.Year
            Me.withholdingTaxReports = LoadWithholdingTaxData(SelectedYear)
        End If

        dialog.Dispose()
    End Sub

    Private Sub btnPrint_Click(sender As Object, e As EventArgs) Handles btnPrint.Click
        Alphalist = New Alphalistv2() 'Alphalist()
        Alphalist.SetDataSource(DirectCast(withholdingTaxReports, DataTable))

        With Alphalist.ReportDefinition.Sections(2)
            Dim objText As CrystalDecisions.CrystalReports.Engine.TextObject = .ReportObjects("Text2")
            objText.Text = mtxtFromDate.Text.Replace("/", "")

            objText = .ReportObjects("Text3")
            objText.Text = mtxtToDate.Text.Replace("/", "")

            objText = .ReportObjects("Text1")
            If ValNoComma(dgvYear.Tag) > 0 Then
                objText.Text = dgvYear.Tag
            Else
                objText.Text = String.Empty
            End If

            objText = .ReportObjects("Text53")
            If txtAuthSign.Text.Trim.Length > 0 Then
                objText.Text = txtAuthSign.Text.Trim
            End If

        End With

        Dim viewer = New CrysRepForm()
        viewer.crysrepvwr.ReportSource = Alphalist
        viewer.Show()
    End Sub

    Private Sub dgvEmployees_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvEmployees.CellClick
        Dim employeeId = dgvEmployees.CurrentRow.Cells(colEmployeeID.Name).Value

        selectedReport = withholdingTaxReports.Select("EmployeeID = '" & employeeId & "'").First()

        DisplayWithholdingTaxReport(selectedReport)
        pnlWithholdingTaxForm.Enabled = True
    End Sub

    Private Sub RefreshForm()
        Recompute(selectedReport)
        DisplayWithholdingTaxReport(selectedReport)
    End Sub

    Private Sub Recompute(ByVal row As WithholdingTaxReportRow)
        row.TotalNonTaxableIncome =
            ParseDecimal(row.BasicSalary) +
            ParseDecimal(row.HolidayPay) +
            ParseDecimal(row.OvertimePay) +
            ParseDecimal(row.NightDiffPay) +
            ParseDecimal(row.HazardPay) +
            ParseDecimal(row._13thMonthPay) +
            ParseDecimal(row.DeMinimisBenefits) +
            ParseDecimal(row.GovernmentInsurance) +
            ParseDecimal(row.SalariesAndOtherCompensation)

        row.TotalTaxableIncome =
            ParseDecimal(row.TaxableBasicSalary) +
            ParseDecimal(row.Representation) +
            ParseDecimal(row.Transportation) +
            ParseDecimal(row.CostOfLivingAllowance) +
            ParseDecimal(row.FixedHousingAllowance) +
            ParseDecimal(row.OthersAAmount) +
            ParseDecimal(row.OthersBAmount) +
            ParseDecimal(row.Commission) +
            ParseDecimal(row.ProfitSharing) +
            ParseDecimal(row.FeesInclDirectorsFees) +
            ParseDecimal(row.Taxable13thMonthPay) +
            ParseDecimal(row.TaxableHazardPay) +
            ParseDecimal(row.TaxableOvertimePay) +
            ParseDecimal(row.SupplementaryAAmount) +
            ParseDecimal(row.SupplementaryBAmount)

        row.NonTaxableIncome = row.TotalNonTaxableIncome
        row.TaxableIncome = row.TotalTaxableIncome
        row.GrossCompensationIncome = ParseDecimal(row.NonTaxableIncome) + ParseDecimal(row.TaxableIncome)
        row.GrossTaxableIncome = ParseDecimal(row.TaxableIncome) + ParseDecimal(row.PreviousTaxableIncome)
        row.TotalExemptions = BASIC_TAX_EXEMPTION
        row.NetTaxableIncome = Math.Max(ParseDecimal(row.GrossTaxableIncome) - ParseDecimal(row.TotalExemptions), 0D)
        row.PresentTaxWithheld = row.TaxDue
        row.TotalTaxWithheld = ParseDecimal(row.PresentTaxWithheld) + ParseDecimal(row.PreviousTaxWithheld)
    End Sub

    Private Function ValueOrNothing(ByVal value As String) As String
        Return If(String.IsNullOrEmpty(value), Nothing, value)
    End Function

    Private Function ParseDecimal(ByVal value As String) As Decimal
        Return If(String.IsNullOrWhiteSpace(value), 0D, CDec(value))
    End Function

    Private Sub DisplayWithholdingTaxReport(ByVal row As WithholdingTaxReportRow)
        txtFullname.Text = row.LastName & ", " & row.FirstName & " " & row.MiddleName

        cboCategory.SelectedValue = If(row.IsCategoryNull, "", row.Category)

        chkMinimumWage.Checked = row.IsMinimumWageEarner
        txtMinimumWagePerDay.Text = row.MinimumWagePerDay
        txtMinimumWagePerMonth.Text = row.MinimumWagePerMonth

        txtGrossCompensationIncome.Text = row.GrossCompensationIncome
        txtNonTaxableIncome.Text = row.NonTaxableIncome
        txtTaxableIncome.Text = row.TaxableIncome
        txtPreviousTaxableIncome.Text = row.PreviousTaxableIncome
        txtGrossTaxableIncome.Text = row.GrossTaxableIncome
        txtTotalExemptions.Text = row.TotalExemptions
        txtPremiumPaidOnHealth.Text = row.PremiumPaidOnHealth
        txtNetTaxableIncome.Text = row.NetTaxableIncome
        txtTaxDue.Text = row.TaxDue
        txtPresentTaxWithheld.Text = row.PresentTaxWithheld
        txtPreviousTaxWithheld.Text = row.PreviousTaxWithheld
        txtTotalTaxWithheld.Text = row.TotalTaxWithheld

        txtBasicSalary.Text = row.BasicSalary
        txtHolidayPay.Text = row.HolidayPay
        txtOvertimePay.Text = row.OvertimePay
        txtNightDiffPay.Text = row.NightDiffPay
        txtHazardPay.Text = row.HazardPay
        txt13thMonthPay.Text = row._13thMonthPay
        txtDeMinimisBenefits.Text = row.DeMinimisBenefits
        txtGovernmentInsurance.Text = row.GovernmentInsurance
        txtSalariesAndOtherCompensation.Text = row.SalariesAndOtherCompensation
        txtTotalNonTaxableIncome.Text = row.TotalNonTaxableIncome

        txtTaxableBasicSalary.Text = row.TaxableBasicSalary
        txtRepresentation.Text = row.Representation
        txtTransportation.Text = row.Transportation
        txtCostOfLivingAllowance.Text = row.CostOfLivingAllowance
        txtFixedHousingAllowance.Text = row.FixedHousingAllowance
        txtOthersAAmount.Text = row.OthersAAmount
        txtOthersAName.Text = row.OthersAName
        txtOthersBAmount.Text = row.OthersBAmount
        txtOthersBName.Text = row.OthersBName
        txtCommission.Text = row.Commission
        txtProfitSharing.Text = row.ProfitSharing
        txtFeesInclDirectorsFees.Text = row.FeesInclDirectorsFees
        txtTaxable13thMonthPay.Text = row.Taxable13thMonthPay
        txtTaxableHazardPay.Text = row.TaxableHazardPay
        txtTaxableOvertimePay.Text = row.TaxableOvertimePay
        txtSupplementaryAAmount.Text = row.SupplementaryAAmount
        txtSupplementaryBAmount.Text = row.SupplementaryBAmount
        txtTotalTaxableIncome.Text = row.TotalTaxableIncome
    End Sub

    Private Function CreateWithholdingTaxBatch(ByVal year As Integer) As Integer
        Dim cmd As New MySql.Data.MySqlClient.MySqlCommand()
        cmd.Connection = conn

        cmd.CommandText = "CreateWithholdingTaxBatch"
        cmd.CommandType = CommandType.StoredProcedure

        Dim batchID As Integer

        Try
            If conn.State = ConnectionState.Closed Then
                conn.Open()
            End If

            cmd.Parameters.AddWithValue("I_OrganizationID", orgztnID)
            cmd.Parameters.AddWithValue("I_Year", year)

            batchID = cmd.ExecuteScalar()
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            conn.Close()
        End Try

        Return batchID
    End Function

    Private Sub CreateWithholdingTaxReports(ByVal batchId As Integer)
        Dim cmd As New MySql.Data.MySqlClient.MySqlCommand()
        cmd.Connection = conn

        cmd.CommandText = "CreateWithholdingTaxReport"
        cmd.CommandType = CommandType.StoredProcedure

        Try
            If conn.State = ConnectionState.Closed Then
                conn.Open()
            End If

            For Each row As WithholdingTaxReportRow In withholdingTaxReports.Rows
                cmd.Parameters.Clear()
                cmd.Parameters.AddWithValue("I_EmployeeID", ValueOrNothing(row.EmployeeID))
                cmd.Parameters.AddWithValue("I_OrganizationID", orgztnID)
                cmd.Parameters.AddWithValue("I_BatchID", batchId)
                cmd.Parameters.AddWithValue("I_Category", row.Category)
                cmd.Parameters.AddWithValue("I_IsMinimumWageEarner", row.IsMinimumWageEarner)
                cmd.Parameters.AddWithValue("I_MinimumWagePerDay", ValueOrNothing(row.MinimumWagePerDay))
                cmd.Parameters.AddWithValue("I_MinimumWagePerMonth", ValueOrNothing(row.MinimumWagePerMonth))
                cmd.Parameters.AddWithValue("I_PreviousTaxableIncome", ValueOrNothing(row.PreviousTaxableIncome))
                cmd.Parameters.AddWithValue("I_PremiumPaidOnHealth", ValueOrNothing(row.PremiumPaidOnHealth))
                cmd.Parameters.AddWithValue("I_PreviousTaxWithheld", ValueOrNothing(row.PreviousTaxWithheld))
                cmd.Parameters.AddWithValue("I_HazardPay", ValueOrNothing(row.HazardPay))
                cmd.Parameters.AddWithValue("I_DeMinimisBenefits", ValueOrNothing(row.DeMinimisBenefits))
                cmd.Parameters.AddWithValue("I_SalariesAndOtherCompensation", ValueOrNothing(row.SalariesAndOtherCompensation))
                cmd.Parameters.AddWithValue("I_Representation", ValueOrNothing(row.Representation))
                cmd.Parameters.AddWithValue("I_Transportation", ValueOrNothing(row.Transportation))
                cmd.Parameters.AddWithValue("I_CostOfLivingAllowance", ValueOrNothing(row.CostOfLivingAllowance))
                cmd.Parameters.AddWithValue("I_FixedHousingAllowance", ValueOrNothing(row.FixedHousingAllowance))
                cmd.Parameters.AddWithValue("I_OthersAAmount", ValueOrNothing(row.OthersAAmount))
                cmd.Parameters.AddWithValue("I_OthersAName", ValueOrNothing(row.OthersAName))
                cmd.Parameters.AddWithValue("I_OthersBAmount", ValueOrNothing(row.OthersBAmount))
                cmd.Parameters.AddWithValue("I_OthersBName", ValueOrNothing(row.OthersBName))
                cmd.Parameters.AddWithValue("I_Commission", ValueOrNothing(row.Commission))
                cmd.Parameters.AddWithValue("I_ProfitSharing", ValueOrNothing(row.ProfitSharing))
                cmd.Parameters.AddWithValue("I_FeesInclDirectorsFees", ValueOrNothing(row.FeesInclDirectorsFees))
                cmd.Parameters.AddWithValue("I_Taxable13thMonthPay", ValueOrNothing(row.Taxable13thMonthPay))
                cmd.Parameters.AddWithValue("I_TaxableHazardPay", ValueOrNothing(row.TaxableHazardPay))
                cmd.Parameters.AddWithValue("I_TaxableOvertimePay", ValueOrNothing(row.TaxableOvertimePay))
                cmd.Parameters.AddWithValue("I_SupplementaryAAmount", ValueOrNothing(row.SupplementaryAAmount))
                cmd.Parameters.AddWithValue("I_SupplementaryBAmount", ValueOrNothing(row.SupplementaryBAmount))
                cmd.Parameters.AddWithValue("I_SupplementaryAName", ValueOrNothing(row.SupplementaryAName))
                cmd.Parameters.AddWithValue("I_SupplementaryBName", ValueOrNothing(row.SupplementaryBName))
                cmd.ExecuteNonQuery()
            Next
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            conn.Close()
        End Try
    End Sub

    Private Sub WithholdingTax_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        cboCategory.DataSource = categoryItems.ToList()
        cboCategory.DisplayMember = "Value"
        cboCategory.ValueMember = "Key"

        btnPrint.Enabled = False
        tsbAlphalist.Enabled = False

        pnlWithholdingTaxForm.Enabled = False
        LoadWithholdingTaxBatches()
    End Sub

    Private Sub tsbAlphalist_Click(sender As Object, e As EventArgs) Handles tsbAlphalist.Click
        Dim alphalist = New AlphalistGeneration(withholdingTaxReports, SelectedYear)
        alphalist.Start()
    End Sub

    Private Sub dgvYear_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvYear.CellClick
        batchId = dgvYear.CurrentRow.Cells("colBatchID").Value
        SelectedYear = dgvYear.CurrentRow.Cells("Year").Value

        btnPrint.Enabled = True
        tsbAlphalist.Enabled = True

        LoadWithholdingTaxReports(batchId, SelectedYear)
        dgvYear.Tag = Nothing
        If e.RowIndex > -1 Then
            dgvYear.Tag = dgvYear.Item("Year", e.RowIndex).Value
        End If

    End Sub

    Private Sub tsbSave_Click(sender As Object, e As EventArgs) Handles tsbSave.Click
        If batchId Is Nothing Then
            batchId = CreateWithholdingTaxBatch(SelectedYear)
        End If

        CreateWithholdingTaxReports(batchId)

        ToolTip1.Show("Save", Label38, 3000)

        btnPrint.Enabled = True
        tsbAlphalist.Enabled = True

        LoadWithholdingTaxBatches()
    End Sub

    Private Sub txtPreviousTaxableIncome_TextChanged(sender As Object, e As EventArgs) Handles txtPreviousTaxableIncome.TextChanged
        selectedReport.PreviousTaxableIncome = txtPreviousTaxableIncome.Text
        RefreshForm()
    End Sub

    Private Sub txtPremiumPaidOnHealth_TextChanged(sender As Object, e As EventArgs) Handles txtPremiumPaidOnHealth.TextChanged
        selectedReport.PremiumPaidOnHealth = txtPremiumPaidOnHealth.Text
        RefreshForm()
    End Sub

    Private Sub txtPreviousTaxWithheld_TextChanged(sender As Object, e As EventArgs) Handles txtPreviousTaxWithheld.TextChanged
        selectedReport.PreviousTaxWithheld = txtPreviousTaxWithheld.Text
        RefreshForm()
    End Sub

    Private Sub txtHazardPay_TextChanged(sender As Object, e As EventArgs) Handles txtHazardPay.TextChanged
        selectedReport.HazardPay = txtHazardPay.Text
        RefreshForm()
    End Sub

    Private Sub txtDeMinimisBenefits_TextChanged(sender As Object, e As EventArgs) Handles txtDeMinimisBenefits.TextChanged
        selectedReport.DeMinimisBenefits = txtDeMinimisBenefits.Text
        RefreshForm()
    End Sub

    Private Sub txtSalariesAndOtherCompensation_TextChanged(sender As Object, e As EventArgs) Handles txtSalariesAndOtherCompensation.TextChanged
        selectedReport.SalariesAndOtherCompensation = txtSalariesAndOtherCompensation.Text
        RefreshForm()
    End Sub

    Private Sub txtRepresentation_TextChanged(sender As Object, e As EventArgs) Handles txtRepresentation.TextChanged
        selectedReport.Representation = txtRepresentation.Text
        RefreshForm()
    End Sub

    Private Sub txtTransportation_TextChanged(sender As Object, e As EventArgs) Handles txtTransportation.TextChanged
        selectedReport.Transportation = txtTransportation.Text
        RefreshForm()
    End Sub

    Private Sub txtCostOfLivingAllowance_TextChanged(sender As Object, e As EventArgs) Handles txtCostOfLivingAllowance.TextChanged
        selectedReport.CostOfLivingAllowance = txtCostOfLivingAllowance.Text
        RefreshForm()
    End Sub

    Private Sub txtFixedHousingAllowance_TextChanged(sender As Object, e As EventArgs) Handles txtFixedHousingAllowance.TextChanged
        selectedReport.FixedHousingAllowance = txtFixedHousingAllowance.Text
        RefreshForm()
    End Sub

    Private Sub txtOthersAName_TextChanged(sender As Object, e As EventArgs) Handles txtOthersAName.TextChanged
        selectedReport.OthersAName = txtOthersAName.Text
        RefreshForm()
    End Sub

    Private Sub txtOthersAAmount_TextChanged(sender As Object, e As EventArgs) Handles txtOthersAAmount.TextChanged
        selectedReport.OthersAAmount = txtOthersAAmount.Text
        RefreshForm()
    End Sub

    Private Sub txtOthersBName_TextChanged(sender As Object, e As EventArgs) Handles txtOthersBName.TextChanged
        selectedReport.OthersBName = txtOthersBName.Text
        RefreshForm()
    End Sub

    Private Sub txtOthersBAmount_TextChanged(sender As Object, e As EventArgs) Handles txtOthersBAmount.TextChanged
        selectedReport.OthersBAmount = txtOthersBAmount.Text
        RefreshForm()
    End Sub

    Private Sub txtCommission_TextChanged(sender As Object, e As EventArgs) Handles txtCommission.TextChanged
        selectedReport.Commission = txtCommission.Text
        RefreshForm()
    End Sub

    Private Sub txtProfitSharing_TextChanged(sender As Object, e As EventArgs) Handles txtProfitSharing.TextChanged
        selectedReport.ProfitSharing = txtProfitSharing.Text
        RefreshForm()
    End Sub

    Private Sub txtFeesInclDirectorsFees_TextChanged(sender As Object, e As EventArgs) Handles txtFeesInclDirectorsFees.TextChanged
        selectedReport.FeesInclDirectorsFees = txtFeesInclDirectorsFees.Text
        RefreshForm()
    End Sub

    Private Sub txtTaxable13thMonthPay_TextChanged(sender As Object, e As EventArgs) Handles txtTaxable13thMonthPay.TextChanged
        selectedReport.Taxable13thMonthPay = txtTaxable13thMonthPay.Text
        RefreshForm()
    End Sub

    Private Sub txtTaxableHazardPay_TextChanged(sender As Object, e As EventArgs) Handles txtTaxableHazardPay.TextChanged
        selectedReport.TaxableHazardPay = txtTaxableHazardPay.Text
        RefreshForm()
    End Sub

    Private Sub txtTaxableOvertimePay_TextChanged(sender As Object, e As EventArgs) Handles txtTaxableOvertimePay.TextChanged
        selectedReport.TaxableOvertimePay = txtTaxableOvertimePay.Text
        RefreshForm()
    End Sub

    Private Sub txtSupplementaryAAmount_TextChanged(sender As Object, e As EventArgs) Handles txtSupplementaryAAmount.TextChanged
        selectedReport.SupplementaryAAmount = txtSupplementaryAAmount.Text
        RefreshForm()
    End Sub

    Private Sub txtSupplementaryBAmount_TextChanged(sender As Object, e As EventArgs) Handles txtSupplementaryBAmount.TextChanged
        selectedReport.SupplementaryBAmount = txtSupplementaryBAmount.Text
        RefreshForm()
    End Sub

    Private Sub chkMinimumWage_CheckedChanged(sender As Object, e As EventArgs) Handles chkMinimumWage.CheckedChanged
        selectedReport.IsMinimumWageEarner = chkMinimumWage.Checked
        RefreshForm()
    End Sub

    Private Sub txtMinimumWagePerMonth_TextChanged(sender As Object, e As EventArgs) Handles txtMinimumWagePerMonth.TextChanged
        selectedReport.MinimumWagePerMonth = txtMinimumWagePerMonth.Text
        RefreshForm()
    End Sub

    Private Sub txtMinimumWagePerDay_TextChanged(sender As Object, e As EventArgs) Handles txtMinimumWagePerDay.TextChanged
        selectedReport.MinimumWagePerDay = txtMinimumWagePerDay.Text
        RefreshForm()
    End Sub

    Private Sub cboCategory_SelectedValueChanged(sender As Object, e As EventArgs) Handles cboCategory.SelectedValueChanged
        If Not selectedReport Is Nothing Then
            selectedReport.Category = If(cboCategory.SelectedValue = Nothing, "", cboCategory.SelectedValue)
            RefreshForm()
        End If
    End Sub

    Private Sub dgvEmployees_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvEmployees.CellContentClick

    End Sub

    Private Sub dgvYear_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvYear.CellContentClick

    End Sub

End Class