Option Strict On

Imports System.Collections.ObjectModel
Imports System.IO
Imports AccuPay.Data.Helpers
Imports AccuPay.Data.Repositories
Imports AccuPay.Entity
Imports AccuPay.ExcelReportColumn
Imports AccuPay.Helpers
Imports AccuPay.Loans
Imports AccuPay.Utilities
Imports AccuPay.Utilities.Extensions
Imports AccuPay.Utils
Imports Microsoft.EntityFrameworkCore
Imports OfficeOpenXml
Imports OfficeOpenXml.Style
Imports PayrollSys

Public Class CostCenterReportProvider
    Inherits ExcelFormatReport
    Implements IReportProvider

    Public Property Name As String = "Cost Center Report" Implements IReportProvider.Name

    Public Property IsHidden As Boolean = False Implements IReportProvider.IsHidden

    Private ReadOnly _reportColumns As IReadOnlyCollection(Of ExcelReportColumn) = GetReportColumns()

    Private Const EmployeeIdKey As String = "EmployeeId"
    Private Const EmployeeNameKey As String = "EmployeeName"
    Private Const TotalDaysKey As String = "TotalDays"
    Private Const TotalHoursKey As String = "TotalHours"
    Private Const DailyRateKey As String = "DailyRate"
    Private Const HoulyRateKey As String = "HoulyRate"
    Private Const BasicPayKey As String = "BasicPay"
    Private Const OvertimeHoursKey As String = "OvertimeHours"
    Private Const OvertimePayKey As String = "OvertimePay"
    Private Const NightDiffHoursKey As String = "NightDiffHours"
    Private Const NightDiffPayKey As String = "NightDiffPay"
    Private Const NightDiffOvertimeHoursKey As String = "NightDiffOvertimeHours"
    Private Const NightDiffOvertimePayKey As String = "NightDiffOvertimePay"
    Private Const SpecialHolidayHoursKey As String = "SpecialHolidayHours"
    Private Const SpecialHolidayPayKey As String = "SpecialHolidayPay"
    Private Const SpecialHolidayOTHoursKey As String = "SpecialHolidayOTHours"
    Private Const SpecialHolidayOTPayKey As String = "SpecialHolidayOTPay"
    Private Const RegularHolidayHoursKey As String = "RegularHolidayHours"
    Private Const RegularHolidayPayKey As String = "RegularHolidayPay"
    Private Const RegularHolidayOTHoursKey As String = "RegularHolidayOTHours"
    Private Const RegularHolidayOTPayKey As String = "RegularHolidayOTPay"
    Private Const GrossPayKey As String = "GrossPay"
    Private Const SSSAmountKey As String = "SSSAmount"
    Private Const ECAmountKey As String = "ECAmount"
    Private Const HDMFAmountKey As String = "HDMFAmount"
    Private Const PhilHealthAmountKey As String = "PhilHealthAmount"
    Private Const HMOAmountKey As String = "HMOAmount"
    Private Const ThirteenthMonthPayKey As String = "ThirteenthMonthPay"
    Private Const FiveDaySilpAmountKey As String = "LeaveAmount"
    Private Const NetPayKey As String = "NetPay"

    Private Shared Function GetReportColumns() As ReadOnlyCollection(Of ExcelReportColumn)

        Dim reportColumns = New List(Of ExcelReportColumn)({
                New ExcelReportColumn("NAME OF EMPLOYEES", EmployeeNameKey, ColumnType.Text),
                New ExcelReportColumn("NO. OF DAYS", TotalDaysKey),
                New ExcelReportColumn("NO. OF HOURS", TotalHoursKey),
                New ExcelReportColumn("RATE", DailyRateKey),
                New ExcelReportColumn("HOURLY", HoulyRateKey),
                New ExcelReportColumn("GROSS PAY", BasicPayKey),
                New ExcelReportColumn("NO. OF OT HOURS", OvertimeHoursKey),
                New ExcelReportColumn("OT PAY", OvertimePayKey),
                New ExcelReportColumn("NO. OF ND HOURS", NightDiffHoursKey),
                New ExcelReportColumn("ND PAY", NightDiffPayKey),
                New ExcelReportColumn("NO. OF NDOT HOURS", NightDiffOvertimeHoursKey),
                New ExcelReportColumn("NDOT PAY", NightDiffOvertimePayKey),
                New ExcelReportColumn("SP HOLIDAY HOURS", SpecialHolidayHoursKey),
                New ExcelReportColumn("SP HOLIDAY PAY", SpecialHolidayPayKey),
                New ExcelReportColumn("SP HOLIDAY OT HOURS", SpecialHolidayOTHoursKey),
                New ExcelReportColumn("SP HOLIDAY OT PAY", SpecialHolidayOTPayKey),
                New ExcelReportColumn("LEGAL HOLIDAY HOURS", RegularHolidayHoursKey),
                New ExcelReportColumn("LH HOLIDAY PAY", RegularHolidayPayKey),
                New ExcelReportColumn("LEGAL HOLIDAY OT HOURS", RegularHolidayOTHoursKey),
                New ExcelReportColumn("LH HOLIDAY OT PAY", RegularHolidayOTPayKey),
                New ExcelReportColumn("TOTAL GROSS PAY", GrossPayKey),
                New ExcelReportColumn("SSS", SSSAmountKey),
                New ExcelReportColumn("EREC", ECAmountKey),
                New ExcelReportColumn("PAG-IBIG", HDMFAmountKey),
                New ExcelReportColumn("PHILHEALTH", PhilHealthAmountKey),
                New ExcelReportColumn("HMO", HMOAmountKey),
                New ExcelReportColumn("13TH MONTH PAY", ThirteenthMonthPayKey),
                New ExcelReportColumn("5 DAY SILP", FiveDaySilpAmountKey),
                New ExcelReportColumn("NET PAY", NetPayKey)
            })

        Return New ReadOnlyCollection(Of ExcelReportColumn)(reportColumns)
    End Function

    Public Sub Run() Implements IReportProvider.Run

        Try
            Dim selectMonthForm As New selectMonth

            If Not selectMonthForm.ShowDialog = Windows.Forms.DialogResult.OK Then
                Return
            End If
            Dim selectedMonth As Date = CDate(selectMonthForm.MonthValue).ToMinimumDateValue

            Dim selectedBranch = GetSelectedBranch()
            If selectedBranch Is Nothing Then Return

            Dim defaultFileName = GetDefaultFileName("Cost Center Report", selectedBranch, selectedMonth)

            Dim saveFileDialogHelperOutPut = SaveFileDialogHelper.BrowseFile(defaultFileName, ".xlsx")

            If saveFileDialogHelperOutPut.IsSuccess = False Then
                Return
            End If

            Dim newFile = saveFileDialogHelperOutPut.FileInfo

            Dim payPeriodModels = GeneratePayPeriodModels(selectedMonth,
                                                          selectedBranch)

            If payPeriodModels.Any = False OrElse payPeriodModels.Sum(Function(p) p.Paystubs.Count) = 0 Then

                MessageBoxHelper.ErrorMessage("No paystubs to show.")
                Return

            End If

            GenerateExcel(payPeriodModels, newFile, selectedBranch)

            Process.Start(newFile.FullName)
        Catch ex As IOException

            MessageBoxHelper.ErrorMessage(ex.Message)
        Catch ex As Exception

            Debugger.Break()
            MessageBoxHelper.DefaultErrorMessage()

        End Try

    End Sub

    Protected Shared Function GetDefaultFileName(reportName As String,
                                                 selectedBranch As Data.Entities.Branch,
                                                 selectedMonth As Date) As String
        Return String.Concat(selectedBranch.Name, " ",
                            reportName, " ",
                            "- ",
                            selectedMonth.ToString("MMMM"),
                            ".xlsx")
    End Function

    Private Sub GenerateExcel(payPeriodModels As List(Of PayPeriodModel),
                              newFile As IO.FileInfo,
                              selectedBranch As Data.Entities.Branch)

        Using excel = New ExcelPackage(newFile)
            Dim subTotalRows = New List(Of Integer)

            Dim worksheet = excel.Workbook.Worksheets.Add("Sheet1")

            RenderWorksheet(worksheet, payPeriodModels, _reportColumns, selectedBranch)

            excel.Save()
        End Using
    End Sub

    Private Sub RenderWorksheet(worksheet As ExcelWorksheet,
                                payPeriods As ICollection(Of PayPeriodModel),
                                viewableReportColumns As IReadOnlyCollection(Of ExcelReportColumn),
                                selectedBranch As Data.Entities.Branch)
        Dim subTotalRows = New List(Of Integer)

        worksheet.Cells.Style.Font.Size = FontSize

        Dim sys_ownr As New SystemOwner

        If sys_ownr.CurrentSystemOwner = SystemOwner.Benchmark Then
            worksheet.Cells.Style.Font.Name = "Book Antiqua"
        End If

        Dim rowIndex = 1
        Dim organizationCell = worksheet.Cells(rowIndex, 1)
        organizationCell.Value = orgNam.ToUpper()
        organizationCell.Style.Font.Bold = True
        rowIndex += 1

        ' space after the title
        rowIndex += 1
        Dim lastCell = String.Empty

        For Each payPeriodModel In payPeriods
            Dim branchNameCell = worksheet.Cells(rowIndex, 1)
            branchNameCell.Value = selectedBranch.Name.ToUpper()
            branchNameCell.Style.Font.Bold = True
            rowIndex += 1
            Dim payPeriodDateCell = worksheet.Cells(rowIndex, 1)
            payPeriodDateCell.Value = GetPayPeriodDescription(payPeriodModel.PayPeriod)
            payPeriodDateCell.Style.Font.Bold = True
            rowIndex += 1

            RenderColumnHeaders(worksheet, rowIndex, viewableReportColumns)
            rowIndex += 1

            Dim employeesStartIndex = rowIndex
            Dim employeesLastIndex = 0

            If payPeriodModel.Paystubs.Count > 0 Then
                RenderGroupedRows(worksheet,
                                  viewableReportColumns,
                                  subTotalRows,
                                  rowIndex,
                                  lastCell,
                                  payPeriodModel,
                                  employeesStartIndex,
                                  employeesLastIndex)

            End If

        Next

        worksheet.Cells.AutoFitColumns()
        worksheet.Cells("A1").AutoFitColumns(4.9, 5.3)

        rowIndex += 1

        If payPeriods.Count > 1 Then
            RenderGrandTotal(worksheet, rowIndex, lastCell, subTotalRows, "B"c)
        End If

        rowIndex += 1

        SetDefaultPrinterSettings(worksheet.PrinterSettings)
    End Sub

    Private Sub RenderGroupedRows(worksheet As ExcelWorksheet,
                                  viewableReportColumns As IReadOnlyCollection(Of ExcelReportColumn),
                                  subTotalRows As List(Of Integer),
                                  ByRef rowIndex As Integer,
                                  ByRef lastCell As String,
                                  payPeriodModel As PayPeriodModel,
                                  employeesStartIndex As Integer,
                                  ByRef employeesLastIndex As Integer)

        For Each paystub In payPeriodModel.Paystubs
            Dim letters = GenerateAlphabet.GetEnumerator()
            Dim propertyLookUp = paystub.LookUp

            For Each reportColumn In viewableReportColumns
                letters.MoveNext()
                Dim alphabet = letters.Current

                Dim column = $"{alphabet}{rowIndex}"

                Dim cell = worksheet.Cells(column)

                Dim value = propertyLookUp(reportColumn.Source)
                If reportColumn.Type = ColumnType.Numeric Then
                    cell.Value = CDec(value)
                Else
                    cell.Value = value

                End If

                If reportColumn.Type = ColumnType.Numeric Then
                    cell.Style.Numberformat.Format = "_(* #,##0.00_);_(* (#,##0.00);_(* ""-""??_);_(@_)"
                    cell.Style.HorizontalAlignment = ExcelHorizontalAlignment.Right
                End If
            Next

            lastCell = letters.Current
            employeesLastIndex = rowIndex
            rowIndex += 1
        Next

        Dim subTotalCellRange = $"B{rowIndex}:{lastCell}{rowIndex}"

        subTotalRows.Add(rowIndex)

        RenderSubTotal(worksheet,
                       subTotalCellRange,
                       employeesStartIndex,
                       employeesLastIndex,
                       formulaColumnStart:=2)

        rowIndex += 2
    End Sub

#Region "Data Methods"

    Private Function GeneratePayPeriodModels(selectedMonth As Date,
                                             selectedBranch As Data.Entities.Branch) As _
                                             List(Of PayPeriodModel)

        Dim payPeriodModels As New List(Of PayPeriodModel)

        Using context As New PayrollContext

            Dim payPeriods = context.PayPeriods.
                                        Where(Function(p) p.OrganizationID.Value = z_OrganizationID).
                                        Where(Function(p) p.IsSemiMonthly).
                                        Where(Function(p) p.Year = selectedMonth.Year).
                                        Where(Function(p) p.Month = selectedMonth.Month).
                                        ToList

            If payPeriods.Count <> 2 Then
                Throw New Exception($"Pay periods on the selected month was {payPeriods.Count} instead of 2 (First half, End of the month)")
            End If

            Dim startDate As Date = {payPeriods(0).PayFromDate, payPeriods(1).PayFromDate}.Min
            Dim endDate As Date = {payPeriods(0).PayToDate, payPeriods(1).PayToDate}.Max

            Dim timeEntries = context.TimeEntries.
                                        Include(Function(t) t.Employee).
                                        Where(Function(t) t.Employee.OrganizationID.Value = z_OrganizationID).
                                        Where(Function(t) t.Date >= startDate AndAlso t.Date <= endDate).
                                        ToList

            Dim employeesWithTimeEntriesInBranch = timeEntries.
                                                    Where(Function(t) t.BranchID IsNot Nothing).
                                                    Where(Function(t) t.BranchID.Value = selectedBranch.RowID.Value).
                                                    GroupBy(Function(t) t.EmployeeID).
                                                    Select(Function(t) t.Key).
                                                    ToArray()

            'Get all the employee in the branch
            'Also get the employees that has at least 1 timelogs on the branch
            Dim employees = context.Employees.
                Where(Function(e) e.IsDaily). 'This report is for daily employee only
                Where(Function(e) (e.BranchID.HasValue AndAlso
                                    e.BranchID.Value = selectedBranch.RowID.Value) OrElse
                                   employeesWithTimeEntriesInBranch.Contains(e.RowID.Value)).
                ToList

            'if timeEntry's BranchID is Nothing, set it to
            'employee's BranchID for easier querying
            AddBranchToTimeEntries(timeEntries, employees)

            timeEntries = timeEntries.
                            Where(Function(t) t.BranchID IsNot Nothing).
                            Where(Function(t) t.BranchID.Value = selectedBranch.RowID.Value).
                            ToList

            Dim salaries = context.Salaries.
                Where(Function(s) s.OrganizationID.Value = z_OrganizationID).
                Where(Function(s) s.EffectiveFrom <= startDate).
                ToList

            Dim employeePaystubs = context.Paystubs.
                                        Include(Function(p) p.ThirteenthMonthPay).
                                        Where(Function(p) p.PayPeriodID.Value = payPeriods(0).RowID.Value OrElse
                                                            p.PayPeriodID.Value = payPeriods(1).RowID.Value).
                                        ToList

            Dim employeeMonthlyDeductions = GenerateMonthlyDeductionList(payPeriods, employees, employeePaystubs)

            Dim hmoLoanType = New ProductRepository().
                                GetOrCreateAdjustmentType(ProductConstant.HMO_LOAN,
                                                          organizationID:=z_OrganizationID,
                                                          userID:=z_User)

            Dim hmoLoans = context.LoanTransactions.
                                    Include(Function(l) l.Paystub).
                                    Where(Function(l) l.PayPeriodID.Value = payPeriods(0).RowID.Value OrElse
                                                      l.PayPeriodID.Value = payPeriods(1).RowID.Value).
                                    ToList

            payPeriodModels = CreatePayPeriodModels(payPeriods, timeEntries, employees, salaries, employeePaystubs, employeeMonthlyDeductions, hmoLoans)

        End Using

        Return payPeriodModels

    End Function

    Private Function GenerateMonthlyDeductionList(payPeriods As List(Of PayPeriod),
                                                  employees As List(Of Employee),
                                                  allPaystubs As List(Of Paystub)) _
                                                  As List(Of MonthlyDeduction)

        Dim employeeMonthlyDeductions As New List(Of MonthlyDeduction)

        Dim sssBrackets As List(Of SocialSecurityBracket)

        Using context As New PayrollContext
            Dim taxEffectivityDate = New Date(payPeriods(0).Year, payPeriods(0).Month, 1)
            sssBrackets = context.SocialSecurityBrackets.
                                    Where(Function(s) taxEffectivityDate >= s.EffectiveDateFrom).
                                    Where(Function(s) taxEffectivityDate <= s.EffectiveDateTo).
                                    ToList()

        End Using

        For Each employee In employees

            Dim employeePaystubs = allPaystubs.
                                    Where(Function(p) p.EmployeeID.Value = employee.RowID.Value).
                                    ToList

            If employeePaystubs.Count > 2 Then
                Throw New Exception("Only up to 2 paystubs should be computed per employee. First half and end of the month paystubs.")
            End If

            Dim sssAmount As Decimal = 0
            Dim ecAmount As Decimal = 0
            Dim hdmfAmount As Decimal = 0
            Dim philhealthAmount As Decimal = 0
            Dim thirteenthMonthPay As Decimal = 0

            If employeePaystubs.Any Then

                hdmfAmount = employeePaystubs.Sum(Function(p) p.HdmfEmployerShare)
                philhealthAmount = employeePaystubs.Sum(Function(p) p.PhilHealthEmployerShare)
                thirteenthMonthPay = employeePaystubs.Sum(Function(p) p.ThirteenthMonthPay.Amount)

                Dim sssPayables = GetEmployerSSSPayables(sssBrackets,
                                                         employeePaystubs(0).SssEmployeeShare)
                sssAmount = sssPayables.EmployerShare
                ecAmount = sssPayables.ECamount

                'check if there is a 2nd paystub (could be only 1 paystub for the month)
                If employeePaystubs.Count = 2 Then
                    sssPayables = GetEmployerSSSPayables(sssBrackets,
                                                         employeePaystubs(1).SssEmployeeShare)
                    sssAmount += sssPayables.EmployerShare
                    ecAmount += sssPayables.ECamount
                End If
            End If

            employeeMonthlyDeductions.Add(MonthlyDeduction.Create(employeeId:=employee.RowID.Value,
                                                                sssAmount:=sssAmount,
                                                                ecAmount:=ecAmount,
                                                                hdmfAmount:=hdmfAmount,
                                                                philhealthAmount:=philhealthAmount,
                                                                thirteenthMonthPay:=thirteenthMonthPay))

        Next

        Return employeeMonthlyDeductions

    End Function

    Private Shared Function GetEmployerSSSPayables(sssBrackets As List(Of SocialSecurityBracket),
                                                   employeeShare As Decimal) _
                                                   As SSSEmployerShare

        If employeeShare = 0 Then Return SSSEmployerShare.Zero

        'SSS employer share and EC are not saved in the database. To get those data
        'we need to query the SSS bracket and get by employee contribution amount
        Dim sssBracket = sssBrackets.
                Where(Function(s) s.EmployeeContributionAmount = employeeShare).
                FirstOrDefault

        Dim sssAmount = If(sssBracket?.EmployerContributionAmount, 0)
        Dim ecAmount = If(sssBracket?.EmployeeECAmount, 0)

        Return New SSSEmployerShare(employerShare:=sssAmount,
                                    ECamount:=ecAmount)
    End Function

    Private Sub AddBranchToTimeEntries(timeEntries As List(Of TimeEntry),
                                       employees As List(Of Employee))
        For Each timeEntry In timeEntries

            If timeEntry.BranchID.HasValue Then Continue For

            timeEntry.BranchID = employees.
                FirstOrDefault(Function(e) e.RowID.Value = timeEntry.EmployeeID.Value)?.BranchID

        Next

    End Sub

    Private Shared Function CreatePayPeriodModels(payPeriods As List(Of PayPeriod),
                                                  allTimeEntries As List(Of TimeEntry),
                                                  employees As List(Of Employee),
                                                  salaries As List(Of Salary),
                                                  paystubs As List(Of Paystub),
                                                  monthlyDeductions As List(Of MonthlyDeduction),
                                                  hmoLoans As List(Of LoanTransaction)) As _
                                                  List(Of PayPeriodModel)

        Dim payPeriodModels As New List(Of PayPeriodModel)
        For Each payPeriod In payPeriods
            Dim payPeriodPaystubs = paystubs.
                                        Where(Function(p) p.PayPeriodID.Value = payPeriod.RowID.Value).
                                        ToList

            Dim payPeriodHmoLoans = hmoLoans.
                                        Where(Function(p) p.PayPeriodID.Value = payPeriod.RowID.Value).
                                        ToList

            Dim paystubModels = CreatePaystubModels(allTimeEntries, employees, salaries, payPeriod, payPeriodPaystubs, monthlyDeductions, payPeriodHmoLoans)

            payPeriodModels.Add(New PayPeriodModel With
            {
                   .PayPeriod = payPeriod,
                   .Paystubs = paystubModels
            })

        Next

        Return payPeriodModels
    End Function

    Private Shared Function CreatePaystubModels(allTimeEntries As List(Of TimeEntry),
                                                employees As List(Of Employee),
                                                salaries As List(Of Salary),
                                                payPeriod As PayPeriod,
                                                paystubs As List(Of Paystub),
                                                monthlyDeductions As List(Of MonthlyDeduction),
                                                hmoLoans As List(Of LoanTransaction)) _
                                                As List(Of PaystubModel)

        Dim paystubModels As New List(Of PaystubModel)

        For Each employee In employees

            Dim timeEntries = allTimeEntries.
                                Where(Function(t) t.Date >= payPeriod.PayFromDate).
                                Where(Function(t) t.Date <= payPeriod.PayToDate).
                                Where(Function(t) t.EmployeeID.Value = employee.RowID.Value).
                                ToList

            Dim salary = salaries.
                Where(Function(s) s.EmployeeID.Value = employee.RowID.Value).
                Where(Function(s) s.EffectiveFrom <= payPeriod.PayFromDate).
                OrderByDescending(Function(s) s.EffectiveFrom).
                FirstOrDefault

            Dim paystub = paystubs.Where(Function(p) p.EmployeeID.Value = employee.RowID.Value).
                        FirstOrDefault

            Dim monthlyDeduction = monthlyDeductions.
                                    Where(Function(d) d.EmployeeID = employee.RowID.Value).
                                    FirstOrDefault

            Dim hmoLoan = hmoLoans.Where(Function(h) h.EmployeeID.Value = employee.RowID.Value).
                                    FirstOrDefault
            Dim createdPaystubModel = PaystubModel.Create(employee, salary, timeEntries, paystub, monthlyDeduction, hmoLoan)

            If createdPaystubModel IsNot Nothing AndAlso createdPaystubModel.GrossPay > 0 Then
                paystubModels.Add(createdPaystubModel)
            End If

        Next

        Return paystubModels
    End Function

    Private Function GetPayPeriodDescription(payPeriod As PayPeriod) As String

        If payPeriod Is Nothing Then Return String.Empty

        If payPeriod.PayFromDate.Month = payPeriod.PayToDate.Month Then

            Return $"{payPeriod.PayFromDate.ToString("MMMM").ToUpper} {payPeriod.PayFromDate.Day} - {payPeriod.PayToDate.Day}"
        Else
            Return $"{payPeriod.PayFromDate.ToString("MMMM").ToUpper} {payPeriod.PayFromDate.Day} - {payPeriod.PayToDate.ToString("MMMM").ToUpper} {payPeriod.PayToDate.Day}"

        End If

    End Function

    Private Shared Function GetSelectedBranch() As Data.Entities.Branch

        Dim selectBranchDialog As New SelectBranchForm

        If Not selectBranchDialog.ShowDialog <> DialogResult.OK Then
            Return Nothing
        End If

        Return selectBranchDialog.SelectedBranch

    End Function

#End Region

#Region "Custom Classes"

    Private Class PayPeriodModel

        Public Property PayPeriod As PayPeriod

        Public Property Paystubs As List(Of PaystubModel)

    End Class

    Private Class SSSEmployerShare

        Public Sub New(employerShare As Decimal, ECamount As Decimal)
            Me.EmployerShare = employerShare
            Me.ECamount = ECamount
        End Sub

        Public ReadOnly Property EmployerShare As Decimal
        Public ReadOnly Property ECamount As Decimal

        Friend Shared Function Zero() As SSSEmployerShare
            Return New SSSEmployerShare(0, 0)
        End Function

    End Class

    Private Class PaystubModel

        Public Shared Function Create(employee As Employee,
                                        salary As Salary,
                                        timeEntries As List(Of TimeEntry),
                                        currentPaystub As Paystub,
                                        monthlyDeduction As MonthlyDeduction,
                                        hmoLoan As LoanTransaction) As PaystubModel

            If employee Is Nothing Then Return Nothing
            If timeEntries Is Nothing OrElse timeEntries.Any = False Then Return Nothing

            Dim paystubModel As New PaystubModel
            paystubModel.Employee = employee

            If salary IsNot Nothing Then
                paystubModel = ComputeHoursAndPay(paystubModel, employee, salary, timeEntries)

            End If

            If currentPaystub IsNot Nothing Then
                'Check the percentage of work hours the employee worked in this branch
                'If employee worked for 100 hours in total, and he worked 40 hours in this branch,
                'then he worked 40% of his total worked hours in this branch.
                Dim workedPercentage = AccuMath.CommercialRound(paystubModel.RegularHours / currentPaystub.RegularHours) '40 / 100
                paystubModel = ComputeGovernmentDeductions(hmoLoan, monthlyDeduction, paystubModel, workedPercentage)
            End If

            Return paystubModel
        End Function

        Private Shared Function ComputeGovernmentDeductions(hmoLoan As LoanTransaction,
                                                            monthlyDeduction As MonthlyDeduction,
                                                            paystubModel As PaystubModel,
                                                            workedPercentage As Decimal) As PaystubModel
            'Test hmoLoan
            paystubModel.HMOAmount = MonthlyDeductionAmount.ComputeBranchPercentage(If(hmoLoan?.Amount, 0), workedPercentage)

            paystubModel.SSSAmount = monthlyDeduction.SSSAmount.GetBranchPercentage(workedPercentage)

            paystubModel.ECAmount = monthlyDeduction.ECAmount.GetBranchPercentage(workedPercentage)

            paystubModel.HDMFAmount = monthlyDeduction.HDMFAmount.GetBranchPercentage(workedPercentage)

            paystubModel.PhilHealthAmount = monthlyDeduction.PhilHealthAmount.GetBranchPercentage(workedPercentage)

            paystubModel.ThirteenthMonthPay = monthlyDeduction.ThirteenthMonthPay.GetBranchPercentage(workedPercentage)

            Return paystubModel
        End Function

        Private Sub New()
        End Sub

        Private Shared Function ComputeHoursAndPay(paystubModel As PaystubModel, employee As Employee, salary As Salary, timeEntries As List(Of TimeEntry)) As PaystubModel
            Dim totalTimeEntries = TotalTimeEntry.Calculate(timeEntries, salary, employee, New List(Of ActualTimeEntry))

            paystubModel.RegularHours = totalTimeEntries.RegularHours
            paystubModel.HourlyRate = totalTimeEntries.HourlyRate

            paystubModel.OvertimeHours = AccuMath.CommercialRound(totalTimeEntries.OvertimeHours)
            paystubModel.OvertimePay = totalTimeEntries.OvertimePay

            paystubModel.NightDiffHours = AccuMath.CommercialRound(totalTimeEntries.NightDiffHours)
            paystubModel.NightDiffPay = totalTimeEntries.NightDiffPay

            paystubModel.NightDiffOvertimeHours = AccuMath.CommercialRound(totalTimeEntries.NightDiffOvertimeHours)
            paystubModel.NightDiffOvertimePay = totalTimeEntries.NightDiffOvertimePay

            paystubModel.SpecialHolidayHours = AccuMath.CommercialRound(totalTimeEntries.SpecialHolidayHours)
            paystubModel.SpecialHolidayPay = totalTimeEntries.SpecialHolidayPay

            paystubModel.SpecialHolidayOTHours = AccuMath.CommercialRound(totalTimeEntries.SpecialHolidayOTHours)
            paystubModel.SpecialHolidayOTPay = totalTimeEntries.SpecialHolidayOTPay

            paystubModel.RegularHolidayHours = AccuMath.CommercialRound(totalTimeEntries.RegularHolidayHours)
            paystubModel.RegularHolidayPay = totalTimeEntries.RegularHolidayPay

            paystubModel.RegularHolidayOTHours = AccuMath.CommercialRound(totalTimeEntries.RegularHolidayOTHours)
            paystubModel.RegularHolidayOTPay = totalTimeEntries.RegularHolidayOTPay
            Return paystubModel
        End Function

        Private Property Employee As Employee

        Private ReadOnly Property EmployeeId As Integer
            Get
                Return Employee.RowID.Value
            End Get
        End Property

        Private ReadOnly Property EmployeeName As String
            Get
                Return Employee.FullNameWithMiddleInitialLastNameFirst.ToUpper & " " & Employee.EmployeeNo
            End Get
        End Property

        Private ReadOnly Property RegularDays As Decimal
            Get
                Return RegularHours / PayrollTools.WorkHoursPerDay
            End Get
        End Property

        Private Property RegularHours As Decimal

        Private ReadOnly Property DailyRate As Decimal
            Get
                Return AccuMath.CommercialRound(_hourlyRate * PayrollTools.WorkHoursPerDay)
            End Get
        End Property

        Private _hourlyRate As Decimal

        Private Property HourlyRate As Decimal
            Get
                Return AccuMath.CommercialRound(_hourlyRate)
            End Get
            Set(value As Decimal)
                _hourlyRate = value
            End Set
        End Property

        Private ReadOnly Property RegularPay As Decimal
            Get
                Return AccuMath.CommercialRound(_hourlyRate * RegularHours)
            End Get
        End Property

        Private Property OvertimeHours As Decimal
        Private Property OvertimePay As Decimal
        Private Property NightDiffHours As Decimal
        Private Property NightDiffPay As Decimal
        Private Property NightDiffOvertimeHours As Decimal
        Private Property NightDiffOvertimePay As Decimal
        Private Property SpecialHolidayHours As Decimal
        Private Property SpecialHolidayPay As Decimal
        Private Property SpecialHolidayOTHours As Decimal
        Private Property SpecialHolidayOTPay As Decimal
        Private Property RegularHolidayHours As Decimal
        Private Property RegularHolidayPay As Decimal
        Private Property RegularHolidayOTHours As Decimal
        Private Property RegularHolidayOTPay As Decimal

        Public ReadOnly Property GrossPay As Decimal
            Get
                Return AccuMath.CommercialRound(RegularPay +
                                                OvertimePay +
                                                NightDiffPay +
                                                NightDiffOvertimePay +
                                                SpecialHolidayPay +
                                                SpecialHolidayOTPay +
                                                RegularHolidayPay +
                                                RegularHolidayOTPay)
            End Get
        End Property

        Private Property SSSAmount As Decimal
        Private Property ECAmount As Decimal
        Private Property HDMFAmount As Decimal
        Private Property PhilHealthAmount As Decimal
        Private Property HMOAmount As Decimal
        Private Property ThirteenthMonthPay As Decimal

        Private ReadOnly Property FiveDaySilpAmount As Decimal
            Get
                Dim daysPerCutoff As Integer = 13 'this should probably be retrieved from employee.WorkDaysPerYear / 12 / 2

                Dim vacationLeavePerYearInDays = Employee.VacationLeaveAllowance / PayrollTools.WorkHoursPerDay

                Dim basicRate = DailyRate * daysPerCutoff 'this should probably be retrieved from PayrollTools.GetEmployeeMonthlyRate / 2

                Return AccuMath.CommercialRound(basicRate *
                                                vacationLeavePerYearInDays /
                                                PayrollTools.MonthsPerYear /
                                                daysPerCutoff)
            End Get
        End Property

        Private ReadOnly Property TotalDeductions As Decimal
            Get
                Return SSSAmount +
                    ECAmount +
                    HDMFAmount +
                    PhilHealthAmount +
                    HMOAmount +
                    ThirteenthMonthPay +
                    FiveDaySilpAmount
            End Get
        End Property

        Private ReadOnly Property NetPay As Decimal
            Get
                'Total deductions are added since cost center report is for
                'franchise/branch owners. They will pay the employer deductions.
                'Net pay is how much they will per employee
                Return GrossPay + TotalDeductions
            End Get
        End Property

        Private _lookUp As Dictionary(Of String, String)

        Public ReadOnly Property LookUp As Dictionary(Of String, String)
            Get
                If _lookUp IsNot Nothing Then Return _lookUp

                _lookUp = New Dictionary(Of String, String)
                _lookUp(EmployeeIdKey) = Me.EmployeeId.ToString
                _lookUp(EmployeeNameKey) = Me.EmployeeName
                _lookUp(TotalDaysKey) = Me.RegularDays.ToString
                _lookUp(TotalHoursKey) = Me.RegularHours.ToString
                _lookUp(DailyRateKey) = Me.DailyRate.ToString
                _lookUp(HoulyRateKey) = Me.HourlyRate.ToString
                _lookUp(BasicPayKey) = Me.RegularPay.ToString
                _lookUp(OvertimeHoursKey) = Me.OvertimeHours.ToString
                _lookUp(OvertimePayKey) = Me.OvertimePay.ToString
                _lookUp(NightDiffHoursKey) = Me.NightDiffHours.ToString
                _lookUp(NightDiffPayKey) = Me.NightDiffPay.ToString
                _lookUp(NightDiffOvertimeHoursKey) = Me.NightDiffOvertimeHours.ToString
                _lookUp(NightDiffOvertimePayKey) = Me.NightDiffOvertimePay.ToString
                _lookUp(SpecialHolidayHoursKey) = Me.SpecialHolidayHours.ToString
                _lookUp(SpecialHolidayPayKey) = Me.SpecialHolidayPay.ToString
                _lookUp(SpecialHolidayOTHoursKey) = Me.SpecialHolidayOTHours.ToString
                _lookUp(SpecialHolidayOTPayKey) = Me.SpecialHolidayOTPay.ToString
                _lookUp(RegularHolidayHoursKey) = Me.RegularHolidayHours.ToString
                _lookUp(RegularHolidayPayKey) = Me.RegularHolidayPay.ToString
                _lookUp(RegularHolidayOTHoursKey) = Me.RegularHolidayOTHours.ToString
                _lookUp(RegularHolidayOTPayKey) = Me.RegularHolidayOTPay.ToString
                _lookUp(GrossPayKey) = Me.GrossPay.ToString
                _lookUp(SSSAmountKey) = Me.SSSAmount.ToString
                _lookUp(ECAmountKey) = Me.ECAmount.ToString
                _lookUp(HDMFAmountKey) = Me.HDMFAmount.ToString
                _lookUp(PhilHealthAmountKey) = Me.PhilHealthAmount.ToString
                _lookUp(HMOAmountKey) = Me.HMOAmount.ToString
                _lookUp(ThirteenthMonthPayKey) = Me.ThirteenthMonthPay.ToString
                _lookUp(FiveDaySilpAmountKey) = Me.FiveDaySilpAmount.ToString
                _lookUp(NetPayKey) = Me.NetPay.ToString

                Return _lookUp
            End Get
        End Property

    End Class

    Private Class MonthlyDeduction

        Public Shared Function Create(employeeId As Integer,
                                    sssAmount As Decimal,
                                    ecAmount As Decimal,
                                    hdmfAmount As Decimal,
                                    philhealthAmount As Decimal,
                                    thirteenthMonthPay As Decimal) As MonthlyDeduction

            Return New MonthlyDeduction() With {
                .EmployeeID = employeeId,
                .SSSAmount = MonthlyDeductionAmount.Create(sssAmount),
                .ECAmount = MonthlyDeductionAmount.Create(ecAmount),
                .HDMFAmount = MonthlyDeductionAmount.Create(hdmfAmount),
                .PhilHealthAmount = MonthlyDeductionAmount.Create(philhealthAmount),
                .ThirteenthMonthPay = MonthlyDeductionAmount.Create(thirteenthMonthPay)
            }
        End Function

        Private Sub New()
        End Sub

        Public Property EmployeeID As Integer

        Public Property SSSAmount As MonthlyDeductionAmount
        Public Property ECAmount As MonthlyDeductionAmount
        Public Property HDMFAmount As MonthlyDeductionAmount
        Public Property PhilHealthAmount As MonthlyDeductionAmount
        Public Property ThirteenthMonthPay As MonthlyDeductionAmount

    End Class

    Private Class MonthlyDeductionAmount

        Private ReadOnly _amount As Decimal

        Public Shared Function Create(amount As Decimal) As MonthlyDeductionAmount

            Return New MonthlyDeductionAmount(amount)

        End Function

        Private Sub New(amount As Decimal)
            _amount = AccuMath.CommercialRound(amount)
        End Sub

        Public ReadOnly Property MonthlyAmount As Decimal
            Get
                Return _amount
            End Get
        End Property

        Public ReadOnly Property SemiMonthlyAmount As Decimal
            Get
                Return AccuMath.CommercialRound(MonthlyAmount / 2)
            End Get
        End Property

        Public Function GetBranchPercentage(branchPercentage As Decimal) As Decimal
            Return ComputeBranchPercentage(SemiMonthlyAmount, branchPercentage)
        End Function

        Public Shared Function ComputeBranchPercentage(amount As Decimal, branchPercentage As Decimal) As Decimal
            Return AccuMath.CommercialRound(amount * branchPercentage)
        End Function

    End Class

#End Region

End Class