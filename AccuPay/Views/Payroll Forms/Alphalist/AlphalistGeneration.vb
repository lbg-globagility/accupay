Imports OfficeOpenXml
Imports AccuPay.AlphalistDataset
Imports System.IO
Imports System.IO.Compression
Imports Microsoft.VisualBasic.FileIO

Public Class AlphalistGeneration

    Delegate Sub PrintProcedure(ByVal worksheet As ExcelWorksheet,
                                ByVal withholdingTaxReport As WithholdingTaxReportRow,
                                ByVal startNo As Integer,
                                ByVal sequenceNo As Integer)

    Private Const SCHEDULE_7_1 As String = "Resources/schedule-7.1.xlsx"
    Private Const SCHEDULE_7_3 As String = "Resources/schedule-7.3.xlsx"
    Private Const SCHEDULE_7_4 As String = "Resources/schedule-7.4.xlsx"
    Private Const SCHEDULE_7_5 As String = "Resources/schedule-7.5.xlsx"

    Private Schedule71Columns As String() = {"F", "G", "H", "I", "J", "K", "L", "M", "N", "O",
                                             "Q", "R", "S", "T", "U", "V", "W", "X"}

    Private Schedule73Columns As String() = {"D", "E", "F", "G", "H", "I", "J", "K", "L", "M",
                                             "O", "P", "Q", "R", "S", "T", "U", "V"}

    Private Schedule74Columns As String() = {"D", "E", "F", "G", "H", "I", "J", "K", "L", "M",
                                             "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W",
                                             "Y", "Z", "AA", "AB", "AC", "AD", "AE", "AF", "AG"}

    Private Schedule75Columns As String() = {"E", "F", "G", "H", "I", "J", "K", "L", "M", "N",
                                             "O", "P", "Q", "R", "U", "V", "W", "X", "Z", "AA",
                                             "AB", "AC", "AD", "AE", "AF", "AG", "AH", "AI", "AJ", "AK",
                                             "AM", "AN", "AO", "AP", "AQ", "AR", "AS", "AT", "AU"}

    Private withholdingTaxReports As WithholdingTaxReportDataTable
    Private year As UInteger

    Public Sub New(ByVal withholdingTaxReports As WithholdingTaxReportDataTable,
                   ByVal year As UInteger)
        Me.withholdingTaxReports = withholdingTaxReports
        Me.year = year
    End Sub

    Public Sub Start()
        Dim taxableEmployees As New List(Of WithholdingTaxReportRow)
        Dim nonTaxableEmployees As New List(Of WithholdingTaxReportRow)
        Dim previouslyEmployed As New List(Of WithholdingTaxReportRow)
        Dim terminatedEarly As New List(Of WithholdingTaxReportRow)

        For Each withholdingTaxReport As WithholdingTaxReportRow In withholdingTaxReports.Rows
            If withholdingTaxReport.Category = "Schedule 7.1" Then
                terminatedEarly.Add(withholdingTaxReport)
            ElseIf Not withholdingTaxReport.Category = "Schedule 7.3" Then
                taxableEmployees.Add(withholdingTaxReport)
            ElseIf withholdingTaxReport.Category = "Schedule 7.4" Then
                previouslyEmployed.Add(withholdingTaxReport)
            ElseIf Not withholdingTaxReport.Category = "Schedule 7.5" Then
                nonTaxableEmployees.Add(withholdingTaxReport)
            End If
        Next

        Dim randomId = Guid.NewGuid()
        Dim directoryName = "schedules"
        Dim tempPath = Path.GetTempPath() & "/" & randomId.ToString()

        Dim directory = tempPath & "/" & directoryName

        FileSystem.CreateDirectory(directory)
        CreateReport(SCHEDULE_7_1, directory & "/" & "schedule7.1", AddressOf PrintRow71, terminatedEarly, Schedule71Columns)
        CreateReport(SCHEDULE_7_3, directory & "/" & "schedule7.3", AddressOf PrintRow73, taxableEmployees, Schedule73Columns)
        CreateReport(SCHEDULE_7_4, directory & "/" & "schedule7.4", AddressOf PrintRow74, previouslyEmployed, Schedule74Columns)
        CreateReport(SCHEDULE_7_5, directory & "/" & "schedule7.5", AddressOf PrintRow75, nonTaxableEmployees, Schedule75Columns)

        Dim saveDialog = New SaveFileDialog()
        saveDialog.OverwritePrompt = True
        saveDialog.Filter = "Archive file|*.zip"
        saveDialog.Title = "Save Zip"
        saveDialog.ShowDialog()

        If saveDialog.FileName <> "" Then
            ZipFile.CreateFromDirectory(directory, saveDialog.FileName)
        End If
    End Sub

    Private Sub CreateReport(ByVal template As String,
                             ByVal name As String,
                             ByVal printCallback As PrintProcedure,
                             ByVal withholdingTaxReports As List(Of WithholdingTaxReportRow),
                             ByVal totals As String())
        Dim tempId = Guid.NewGuid()

        Dim fileName = name & ".xlsx"

        File.Copy(template, fileName) '"C:\Users\GLOBAL-D\AppData\Local\Temp\/92a51e03-1f8c-4583-90fd-e9cb876a248f/schedules/schedule7.1.xlsx"

        Dim lastDayOfYear = New Date(year, 12, 31)

        Using excel As New ExcelPackage(New FileInfo(fileName))
            Dim worksheet = excel.Workbook.Worksheets.FirstOrDefault()

            worksheet.Cells("A3").Value = "AS OF " & lastDayOfYear.ToString("MMMM dd yyyy")

            Dim startNo = 17
            Dim sequenceNo = 1
            Dim offsetNo = 0

            For Each withholdingTaxReport As WithholdingTaxReportRow In withholdingTaxReports
                printCallback(worksheet, withholdingTaxReport, startNo, sequenceNo)
                sequenceNo += 1
            Next

            Dim totalRowIndex = startNo + sequenceNo + 1
            Dim endNo = startNo + sequenceNo

            For Each c In totals
                worksheet.Cells(c & totalRowIndex).Formula = String.Format("=SUM({0}{1}:{0}{2})", c, startNo, endNo)
            Next

            excel.Save()
        End Using
    End Sub

    Private Sub PrintRow71(ByVal worksheet As ExcelWorksheet,
                           ByVal withholdingTaxReport As WithholdingTaxReportRow,
                           ByVal startNo As Integer,
                           ByVal sequenceNo As Integer)
        Dim index = startNo + (sequenceNo - 1)

        worksheet.InsertRow(index, 1, index + 1)

        worksheet.Cells("A" & index).Value = sequenceNo
        worksheet.Cells("B" & index).Value = withholdingTaxReport.TinNo
        worksheet.Cells("C" & index).Value = withholdingTaxReport.LastName & ", " & withholdingTaxReport.FirstName & " " & withholdingTaxReport.MiddleName
        worksheet.Cells("D" & index).Value = withholdingTaxReport.StartDate
        worksheet.Cells("E" & index).Value = withholdingTaxReport.EndDate
        worksheet.Cells("F" & index).Value = ParseDecimal(withholdingTaxReport.GrossCompensationIncome)
        worksheet.Cells("G" & index).Value = ParseDecimal(withholdingTaxReport._13thMonthPay)
        worksheet.Cells("H" & index).Value = ParseDecimal(withholdingTaxReport.DeMinimisBenefits)
        worksheet.Cells("I" & index).Value = ParseDecimal(withholdingTaxReport.GovernmentInsurance)
        worksheet.Cells("J" & index).Value = ParseDecimal(withholdingTaxReport.SalariesAndOtherCompensation)
        worksheet.Cells("K" & index).Value = ParseDecimal(withholdingTaxReport.TotalNonTaxableIncome)

        worksheet.Cells("L" & index).Value = ParseDecimal(withholdingTaxReport.TaxableBasicSalary)
        worksheet.Cells("M" & index).Value = ParseDecimal(withholdingTaxReport.Taxable13thMonthPay)
        worksheet.Cells("N" & index).Value = 0D
        worksheet.Cells("O" & index).Value = ParseDecimal(withholdingTaxReport.TotalTaxableIncome)
        worksheet.Cells("P" & index).Value = withholdingTaxReport.ExemptionCode
        worksheet.Cells("Q" & index).Value = ParseDecimal(withholdingTaxReport.TotalExemptions)
        worksheet.Cells("R" & index).Value = ParseDecimal(withholdingTaxReport.PremiumPaidOnHealth)
        worksheet.Cells("S" & index).Value = ParseDecimal(withholdingTaxReport.NetTaxableIncome)
        worksheet.Cells("T" & index).Value = ParseDecimal(withholdingTaxReport.TaxDue)
        worksheet.Cells("U" & index).Value = ParseDecimal(withholdingTaxReport.TotalTaxWithheld)

        Dim amountWithheld = Math.Max(
            ParseDecimal(withholdingTaxReport.TaxDue) - ParseDecimal(withholdingTaxReport.TotalTaxWithheld), 0D)
        Dim overWithheld = Math.Max(
            ParseDecimal(withholdingTaxReport.TotalTaxWithheld) - ParseDecimal(withholdingTaxReport.TaxDue), 0D)
        Dim adjustedAmountWithheld = ParseDecimal(withholdingTaxReport.TotalTaxWithheld) - overWithheld

        worksheet.Cells("V" & index).Value = amountWithheld
        worksheet.Cells("W" & index).Value = overWithheld
        worksheet.Cells("X" & index).Value = adjustedAmountWithheld
        worksheet.Cells("Y" & index).Value = Nothing
    End Sub

    Private Sub PrintRow73(ByVal worksheet As ExcelWorksheet,
                           ByVal withholdingTaxReport As WithholdingTaxReportRow,
                           ByVal startNo As Integer,
                           ByVal sequenceNo As Integer)
        Dim index = startNo + (sequenceNo - 1)

        worksheet.InsertRow(index, 1, index + 1)

        worksheet.Cells("A" & index).Value = sequenceNo
        worksheet.Cells("B" & index).Value = withholdingTaxReport.TinNo
        worksheet.Cells("C" & index).Value = withholdingTaxReport.LastName & ", " & withholdingTaxReport.FirstName & " " & withholdingTaxReport.MiddleName
        worksheet.Cells("D" & index).Value = ParseDecimal(withholdingTaxReport.GrossCompensationIncome)
        worksheet.Cells("E" & index).Value = ParseDecimal(withholdingTaxReport._13thMonthPay)
        worksheet.Cells("F" & index).Value = ParseDecimal(withholdingTaxReport.DeMinimisBenefits)
        worksheet.Cells("G" & index).Value = ParseDecimal(withholdingTaxReport.GovernmentInsurance)
        worksheet.Cells("H" & index).Value = ParseDecimal(withholdingTaxReport.SalariesAndOtherCompensation)
        worksheet.Cells("I" & index).Value = ParseDecimal(withholdingTaxReport.TotalNonTaxableIncome)

        worksheet.Cells("J" & index).Value = ParseDecimal(withholdingTaxReport.TaxableBasicSalary)
        worksheet.Cells("K" & index).Value = ParseDecimal(withholdingTaxReport.Taxable13thMonthPay)
        worksheet.Cells("L" & index).Value = 0D
        worksheet.Cells("M" & index).Value = ParseDecimal(withholdingTaxReport.TotalTaxableIncome)
        worksheet.Cells("N" & index).Value = withholdingTaxReport.ExemptionCode
        worksheet.Cells("O" & index).Value = ParseDecimal(withholdingTaxReport.TotalExemptions)
        worksheet.Cells("P" & index).Value = ParseDecimal(withholdingTaxReport.PremiumPaidOnHealth)
        worksheet.Cells("Q" & index).Value = ParseDecimal(withholdingTaxReport.NetTaxableIncome)
        worksheet.Cells("R" & index).Value = ParseDecimal(withholdingTaxReport.TaxDue)
        worksheet.Cells("S" & index).Value = ParseDecimal(withholdingTaxReport.TotalTaxWithheld)

        Dim amountWithheld = Math.Max(
            ParseDecimal(withholdingTaxReport.TaxDue) - ParseDecimal(withholdingTaxReport.TotalTaxWithheld), 0D)
        Dim overWithheld = Math.Max(
            ParseDecimal(withholdingTaxReport.TotalTaxWithheld) - ParseDecimal(withholdingTaxReport.TaxDue), 0D)
        Dim adjustedAmountWithheld = ParseDecimal(withholdingTaxReport.TotalTaxWithheld) - overWithheld

        worksheet.Cells("T" & index).Value = amountWithheld
        worksheet.Cells("U" & index).Value = overWithheld
        worksheet.Cells("V" & index).Value = adjustedAmountWithheld
        worksheet.Cells("W" & index).Value = Nothing
    End Sub

    Private Sub PrintRow74(ByVal worksheet As ExcelWorksheet,
                           ByVal withholdingTaxReport As WithholdingTaxReportRow,
                           ByVal startNo As Integer,
                           ByVal sequenceNo As Integer)
        Dim index = startNo + (sequenceNo - 1)

        worksheet.InsertRow(index, 1, index + 1)

        worksheet.Cells("A" & index).Value = sequenceNo
        worksheet.Cells("B" & index).Value = withholdingTaxReport.TinNo
        worksheet.Cells("C" & index).Value = withholdingTaxReport.LastName & ", " & withholdingTaxReport.FirstName & " " & withholdingTaxReport.MiddleName
        worksheet.Cells("D" & index).Value = 0D
        worksheet.Cells("E" & index).Value = 0D
        worksheet.Cells("F" & index).Value = 0D
        worksheet.Cells("G" & index).Value = 0D
        worksheet.Cells("H" & index).Value = 0D
        worksheet.Cells("I" & index).Value = 0D
        worksheet.Cells("J" & index).Value = 0D
        worksheet.Cells("K" & index).Value = 0D
        worksheet.Cells("L" & index).Value = 0D
        worksheet.Cells("M" & index).Value = 0D
        worksheet.Cells("N" & index).Value = ParseDecimal(withholdingTaxReport._13thMonthPay)
        worksheet.Cells("O" & index).Value = ParseDecimal(withholdingTaxReport.DeMinimisBenefits)
        worksheet.Cells("P" & index).Value = ParseDecimal(withholdingTaxReport.GovernmentInsurance)
        worksheet.Cells("Q" & index).Value = ParseDecimal(withholdingTaxReport.SalariesAndOtherCompensation)
        worksheet.Cells("R" & index).Value = ParseDecimal(withholdingTaxReport.TotalNonTaxableIncome)
        worksheet.Cells("S" & index).Value = ParseDecimal(withholdingTaxReport.TaxableBasicSalary)
        worksheet.Cells("T" & index).Value = ParseDecimal(withholdingTaxReport.Taxable13thMonthPay)
        worksheet.Cells("U" & index).Value = ParseDecimal(withholdingTaxReport.SalariesAndOtherCompensation)
        worksheet.Cells("V" & index).Formula = String.Format("=S{0} + T{0} + U{0}", index)
        worksheet.Cells("W" & index).Formula = String.Format("=M{0} + V{0}", index)
        worksheet.Cells("X" & index).Value = withholdingTaxReport.ExemptionCode
        worksheet.Cells("Y" & index).Value = ParseDecimal(withholdingTaxReport.TotalExemptions)
        worksheet.Cells("Z" & index).Value = ParseDecimal(withholdingTaxReport.PremiumPaidOnHealth)
        worksheet.Cells("AA" & index).Value = ParseDecimal(withholdingTaxReport.NetTaxableIncome)
        worksheet.Cells("AB" & index).Value = ParseDecimal(withholdingTaxReport.TaxDue)
        worksheet.Cells("AC" & index).Value = ParseDecimal(withholdingTaxReport.PreviousTaxWithheld)
        worksheet.Cells("AD" & index).Value = ParseDecimal(withholdingTaxReport.PresentTaxWithheld)

        Dim amountWithheld = Math.Max(
            ParseDecimal(withholdingTaxReport.TaxDue) - ParseDecimal(withholdingTaxReport.TotalTaxWithheld), 0D)
        Dim overWithheld = Math.Max(
            ParseDecimal(withholdingTaxReport.TotalTaxWithheld) - ParseDecimal(withholdingTaxReport.TaxDue), 0D)
        Dim adjustedAmountWithheld = ParseDecimal(withholdingTaxReport.TotalTaxWithheld) - overWithheld

        worksheet.Cells("AE" & index).Value = amountWithheld
        worksheet.Cells("AF" & index).Value = overWithheld
        worksheet.Cells("AG" & index).Value = adjustedAmountWithheld
    End Sub

    Private Sub PrintRow75(ByVal worksheet As ExcelWorksheet,
                           ByVal withholdingTaxReport As WithholdingTaxReportRow,
                           ByVal startNo As Integer,
                           ByVal sequenceNo As Integer)
        Dim index = startNo + (sequenceNo - 1)

        worksheet.InsertRow(index, 1, index + 1)

        worksheet.Cells("A" & index).Value = sequenceNo
        worksheet.Cells("B" & index).Value = withholdingTaxReport.TinNo
        worksheet.Cells("C" & index).Value = withholdingTaxReport.LastName & ", " & withholdingTaxReport.FirstName & " " & withholdingTaxReport.MiddleName
        worksheet.Cells("D" & index).Value = "REG"
        worksheet.Cells("E" & index).Value = 0D
        worksheet.Cells("F" & index).Value = 0D
        worksheet.Cells("G" & index).Value = 0D
        worksheet.Cells("H" & index).Value = 0D
        worksheet.Cells("I" & index).Value = 0D
        worksheet.Cells("J" & index).Value = 0D
        worksheet.Cells("K" & index).Value = 0D
        worksheet.Cells("L" & index).Value = 0D
        worksheet.Cells("M" & index).Value = 0D
        worksheet.Cells("N" & index).Value = 0D
        worksheet.Cells("O" & index).Value = 0D
        worksheet.Cells("P" & index).Value = 0D
        worksheet.Cells("Q" & index).Value = 0D
        worksheet.Cells("R" & index).Value = 0D
        worksheet.Cells("S" & index).Value = withholdingTaxReport.StartDate
        worksheet.Cells("T" & index).Value = withholdingTaxReport.EndDate
        worksheet.Cells("U" & index).Value = ParseDecimal(withholdingTaxReport.GrossCompensationIncome)
        worksheet.Cells("V" & index).Value = ParseDecimal(withholdingTaxReport.MinimumWagePerDay)
        worksheet.Cells("W" & index).Value = ParseDecimal(withholdingTaxReport.MinimumWagePerMonth)
        worksheet.Cells("X" & index).Value = ParseDecimal(withholdingTaxReport.MinimumWagePerMonth) * 12
        worksheet.Cells("Y" & index).Value = withholdingTaxReport.WorkDaysPerYear
        worksheet.Cells("Z" & index).Value = ParseDecimal(withholdingTaxReport.HolidayPay)
        worksheet.Cells("AA" & index).Value = ParseDecimal(withholdingTaxReport.OvertimePay)
        worksheet.Cells("AB" & index).Value = ParseDecimal(withholdingTaxReport.NightDiffPay)
        worksheet.Cells("AC" & index).Value = ParseDecimal(withholdingTaxReport.HazardPay)
        worksheet.Cells("AD" & index).Value = ParseDecimal(withholdingTaxReport._13thMonthPay)
        worksheet.Cells("AE" & index).Value = ParseDecimal(withholdingTaxReport.DeMinimisBenefits)
        worksheet.Cells("AF" & index).Value = ParseDecimal(withholdingTaxReport.GovernmentInsurance)
        worksheet.Cells("AG" & index).Value = ParseDecimal(withholdingTaxReport.SalariesAndOtherCompensation)
        worksheet.Cells("AH" & index).Value = ParseDecimal(withholdingTaxReport.Taxable13thMonthPay)
        worksheet.Cells("AI" & index).Value = ParseDecimal(withholdingTaxReport.SalariesAndOtherCompensation)
        worksheet.Cells("AJ" & index).Value = ParseDecimal(withholdingTaxReport.GrossCompensationIncome)
        worksheet.Cells("AK" & index).Value = ParseDecimal(withholdingTaxReport.GrossTaxableIncome)
        worksheet.Cells("AL" & index).Value = withholdingTaxReport.ExemptionCode
        worksheet.Cells("AM" & index).Value = ParseDecimal(withholdingTaxReport.TotalExemptions)
        worksheet.Cells("AN" & index).Value = ParseDecimal(withholdingTaxReport.PremiumPaidOnHealth)
        worksheet.Cells("AO" & index).Value = ParseDecimal(withholdingTaxReport.NetTaxableIncome)
        worksheet.Cells("AP" & index).Value = ParseDecimal(withholdingTaxReport.TaxDue)
        worksheet.Cells("AQ" & index).Value = ParseDecimal(withholdingTaxReport.PreviousTaxWithheld)
        worksheet.Cells("AR" & index).Value = ParseDecimal(withholdingTaxReport.PresentTaxWithheld)

        Dim amountWithheld = Math.Max(
            ParseDecimal(withholdingTaxReport.TaxDue) - ParseDecimal(withholdingTaxReport.TotalTaxWithheld), 0D)
        Dim overWithheld = Math.Max(
            ParseDecimal(withholdingTaxReport.TotalTaxWithheld) - ParseDecimal(withholdingTaxReport.TaxDue), 0D)
        Dim adjustedAmountWithheld = ParseDecimal(withholdingTaxReport.TotalTaxWithheld) - overWithheld

        worksheet.Cells("AS" & index).Value = amountWithheld
        worksheet.Cells("AT" & index).Value = overWithheld
        worksheet.Cells("AU" & index).Value = adjustedAmountWithheld
    End Sub

    Private Function ParseDecimal(ByVal value As String) As Decimal
        Return If(String.IsNullOrWhiteSpace(value), 0D, CDec(value))
    End Function

End Class
