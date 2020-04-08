Option Strict On

Imports System.Collections.ObjectModel
Imports AccuPay.Entity
Imports AccuPay.ExcelReportColumn
Imports AccuPay.Helpers
Imports AccuPay.Utilities
Imports AccuPay.Utils
Imports Microsoft.EntityFrameworkCore
Imports OfficeOpenXml
Imports OfficeOpenXml.Style
Imports PayrollSys

Public Class PayrollSummaryByBranchProvider
    Inherits ExcelFormatReport
    Implements IReportProvider

    Public Property Name As String = "Payroll Summary by Branch" Implements IReportProvider.Name

    Public Property IsHidden As Boolean = False Implements IReportProvider.IsHidden

    Private ReadOnly _reportColumns As IReadOnlyCollection(Of ExcelReportColumn) = GetReportColumns()

    Private Const EmployeeIdDescription As String = "EmployeeId"
    Private Const EmployeeNameDescription As String = "EmployeeName"
    Private Const TotalDaysDescription As String = "TotalDays"
    Private Const TotalHoursDescription As String = "TotalHours"
    Private Const DailyRateDescription As String = "DailyRate"
    Private Const HoulyRateDescription As String = "HoulyRate"
    Private Const BasicPayDescription As String = "BasicPay"
    Private Const TotalOvertimeHoursDescription As String = "TotalOvertimeHours"
    Private Const TotalOvertimePayDescription As String = "TotalOvertimePay"
    Private Const SpecialHolidayHoursDescription As String = "SpecialHolidayHours"
    Private Const SpecialHolidayPayDescription As String = "SpecialHolidayPay"
    Private Const LegalHolidayHoursDescription As String = "LegalHolidayHours"
    Private Const LegalHolidayPayDescription As String = "LegalHolidayPay"
    Private Const GrossPayDescription As String = "GrossPay"
    Private Const SSSAmountDescription As String = "SSSAmount"
    Private Const ECAmountDescription As String = "ECAmount"
    Private Const HDMFAmountDescription As String = "HDMFAmount"
    Private Const PhilHealthAmountDescription As String = "PhilHealthAmount"
    Private Const HMOAmountDescription As String = "HMOAmount"
    Private Const ThirteenthMonthPayDescription As String = "ThirteenthMonthPay"
    Private Const LeaveAmountDescription As String = "LeaveAmount"
    Private Const NetPayDescription As String = "NetPay"

    Private Shared Function GetReportColumns() As ReadOnlyCollection(Of ExcelReportColumn)

        Dim reportColumns = New List(Of ExcelReportColumn)({
                New ExcelReportColumn("NAME OF EMPLOYEES", EmployeeNameDescription, ColumnType.Text),
                New ExcelReportColumn("NO. OF DAYS", TotalDaysDescription),
                New ExcelReportColumn("NO. OF HOURS", TotalHoursDescription),
                New ExcelReportColumn("RATE", DailyRateDescription),
                New ExcelReportColumn("HOURLY", HoulyRateDescription),
                New ExcelReportColumn("GROSS PAY", BasicPayDescription),
                New ExcelReportColumn("NO. OF OT HOURS", TotalOvertimeHoursDescription),
                New ExcelReportColumn("OT PAY", TotalOvertimePayDescription),
                New ExcelReportColumn("SP HOLIDAY HOURS", SpecialHolidayHoursDescription),
                New ExcelReportColumn("SP HOLIDAY PAY", SpecialHolidayPayDescription),
                New ExcelReportColumn("LEGAL HOLIDAY HOURS", LegalHolidayHoursDescription),
                New ExcelReportColumn("LH HOLIDAY PAY", LegalHolidayPayDescription),
                New ExcelReportColumn("TOTAL GROSS PAY", GrossPayDescription),
                New ExcelReportColumn("SSS", SSSAmountDescription),
                New ExcelReportColumn("EREC", ECAmountDescription),
                New ExcelReportColumn("PAG-IBIG", HDMFAmountDescription),
                New ExcelReportColumn("PHILHEALTH", PhilHealthAmountDescription),
                New ExcelReportColumn("HMO", HMOAmountDescription),
                New ExcelReportColumn("13TH MONTH PAY", ThirteenthMonthPayDescription),
                New ExcelReportColumn("5 DAY SILP", LeaveAmountDescription),
                New ExcelReportColumn("NET PAY", NetPayDescription)
            })

        Return New ReadOnlyCollection(Of ExcelReportColumn)(reportColumns)
    End Function

    Public Sub Run() Implements IReportProvider.Run

        Dim payrollSelector = GetPayrollSelector()
        If payrollSelector Is Nothing Then Return

        Dim selectedBranch = GetSelectedBranch()
        If selectedBranch Is Nothing Then Return

        MsgBox($"({payrollSelector.PayPeriodFromID} - {payrollSelector.PayPeriodToID}) / {selectedBranch.Name}")

        Dim defaultFileName = GetDefaultFileName("PayrollSummaryByBranch", payrollSelector)

        Dim saveFileDialogHelperOutPut = SaveFileDialogHelper.BrowseFile(defaultFileName, ".xlsx")

        If saveFileDialogHelperOutPut.IsSuccess = False Then
            Return
        End If

        Dim newFile = saveFileDialogHelperOutPut.FileInfo

        Dim payPeriodModels = GeneratePayPeriodModels(payrollSelector.DateFrom.Value,
                                                      payrollSelector.DateTo.Value,
                                                      selectedBranch)

        If payPeriodModels.Any = False Then

            MessageBoxHelper.ErrorMessage("No paystubs to show.")
            Return

        End If

        GenerateExcel(payPeriodModels, newFile)

        Process.Start(newFile.FullName)

    End Sub

    Private Sub GenerateExcel(payPeriodModels As List(Of PayPeriodModel),
                              newFile As IO.FileInfo)

        Using excel = New ExcelPackage(newFile)
            Dim subTotalRows = New List(Of Integer)

            Dim worksheet = excel.Workbook.Worksheets.Add("Sheet1")

            RenderWorksheet(worksheet, payPeriodModels, _reportColumns)

            excel.Save()
        End Using
    End Sub

    Private Function GeneratePayPeriodModels(startDate As Date,
                                             endDate As Date,
                                             selectedBranch As Branch) As _
                                             List(Of PayPeriodModel)

        Dim payPeriodModels As New List(Of PayPeriodModel)

        Using context As New PayrollContext

            Dim payPeriods = context.PayPeriods.
                                        Where(Function(p) p.OrganizationID.Value = z_OrganizationID).
                                        Where(Function(p) p.IsSemiMonthly).
                                        Where(Function(p) p.PayFromDate >= startDate).
                                        Where(Function(p) p.PayToDate <= endDate).
                                        ToList

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

            payPeriodModels = CreatePayPeriodModels(payPeriods, timeEntries, employees, salaries)

        End Using

        Return payPeriodModels

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
                                                  salaries As List(Of Salary)) As _
                                                  List(Of PayPeriodModel)

        Dim payPeriodModels As New List(Of PayPeriodModel)
        For Each payPeriod In payPeriods

            Dim paystubs = CreatePaystubModels(allTimeEntries, employees, salaries, payPeriod)

            payPeriodModels.Add(New PayPeriodModel With
            {
                   .PayPeriod = payPeriod,
                   .Paystubs = paystubs
            })

        Next

        Return payPeriodModels
    End Function

    Private Shared Function CreatePaystubModels(allTimeEntries As List(Of TimeEntry),
                                                employees As List(Of Employee),
                                                salaries As List(Of Salary),
                                                payPeriod As PayPeriod) _
                                                As List(Of PaystubModel)

        Dim paystubs As New List(Of PaystubModel)

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

            Dim paystub = PaystubModel.CreateModel(employee, salary, timeEntries)

            If paystub IsNot Nothing AndAlso paystub.GrossPay > 0 Then
                paystubs.Add(paystub)
            End If

        Next

        Return paystubs
    End Function

    Private Sub RenderWorksheet(worksheet As ExcelWorksheet,
                                payPeriods As ICollection(Of PayPeriodModel),
                                viewableReportColumns As IReadOnlyCollection(Of ExcelReportColumn))
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
            Dim divisionCell = worksheet.Cells(rowIndex, 1)
            divisionCell.Value = GetPayPeriodDescription(payPeriodModel.PayPeriod)
            divisionCell.Style.Font.Bold = True
            rowIndex += 1

            RenderColumnHeaders(worksheet, rowIndex, viewableReportColumns)
            rowIndex += 1

            Dim employeesStartIndex = rowIndex
            Dim employeesLastIndex = 0

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

            Dim subTotalCellRange = $"C{rowIndex}:{lastCell}{rowIndex}"

            subTotalRows.Add(rowIndex)

            RenderSubTotal(worksheet, subTotalCellRange, employeesStartIndex, employeesLastIndex)

            rowIndex += 2
        Next

        worksheet.Cells.AutoFitColumns()
        worksheet.Cells("A1").AutoFitColumns(4.9, 5.3)

        rowIndex += 1

        If payPeriods.Count > 1 Then
            RenderGrandTotal(worksheet, rowIndex, lastCell, subTotalRows)
        End If

        rowIndex += 1

        SetDefaultPrinterSettings(worksheet.PrinterSettings)
    End Sub

    Private Function GetPayPeriodDescription(payPeriod As PayPeriod) As String

        If payPeriod Is Nothing Then Return String.Empty

        If payPeriod.PayFromDate.Month = payPeriod.PayToDate.Month Then

            Return $"{payPeriod.PayFromDate.ToString("MMMM").ToUpper} {payPeriod.PayFromDate.Day} - {payPeriod.PayToDate.Day}"
        Else
            Return $"{payPeriod.PayFromDate.ToString("MMMM").ToUpper} {payPeriod.PayFromDate.Day} - {payPeriod.PayToDate.ToString("MMMM").ToUpper} {payPeriod.PayToDate.Day}"

        End If

    End Function

    Private Shared Function GetSelectedBranch() As Branch

        Dim selectBranchDialog As New SelectBranchForm

        If Not selectBranchDialog.ShowDialog <> DialogResult.OK Then
            Return Nothing
        End If

        Return selectBranchDialog.SelectedBranch

    End Function

    Private Class PayPeriodModel

        Public Property PayPeriod As PayPeriod

        Public Property Paystubs As List(Of PaystubModel)

    End Class

    Private Class PaystubModel

        Public Shared Function CreateModel(employee As Employee,
                                           salary As Salary,
                                           timeEntries As List(Of TimeEntry)) As PaystubModel

            If employee Is Nothing Then Return Nothing
            If timeEntries Is Nothing OrElse timeEntries.Any = False Then Return Nothing

            Dim paystubModel As New PaystubModel
            paystubModel.EmployeeId = employee.RowID.Value
            paystubModel.EmployeeName = employee.FullNameWithMiddleInitialLastNameFirst.ToUpper + " " + employee.EmployeeNo

            Dim totalTimeEntries = TotalTimeEntry.Calculate(timeEntries, salary, employee, New List(Of ActualTimeEntry))

            paystubModel.RegularHours = totalTimeEntries.RegularHours
            paystubModel.HourlyRate = totalTimeEntries.HourlyRate

            paystubModel.TotalOvertimeHours = AccuMath.CommercialRound(totalTimeEntries.OvertimeHours)
            paystubModel.TotalOvertimePay = totalTimeEntries.OvertimePay

            paystubModel.SpecialHolidayHours = AccuMath.CommercialRound(totalTimeEntries.SpecialHolidayHours)
            paystubModel.SpecialHolidayPay = totalTimeEntries.SpecialHolidayPay

            paystubModel.LegalHolidayHours = AccuMath.CommercialRound(totalTimeEntries.RegularHolidayHours)
            paystubModel.LegalHolidayPay = totalTimeEntries.RegularHolidayPay

            'paystubModel.LeaveHours = totalTimeEntries.LeaveHours
            'paystubModel.LeavePay = totalTimeEntries.LeavePay
            Return paystubModel
        End Function

        Private Sub New()

        End Sub

        Private Property EmployeeId As Integer
        Private Property EmployeeName As String

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

        Private Property TotalOvertimeHours As Decimal
        Private Property TotalOvertimePay As Decimal
        Private Property SpecialHolidayHours As Decimal
        Private Property SpecialHolidayPay As Decimal
        Private Property LegalHolidayHours As Decimal
        Private Property LegalHolidayPay As Decimal

        Public ReadOnly Property GrossPay As Decimal
            Get
                Return AccuMath.CommercialRound(RegularPay + TotalOvertimePay +
                                                SpecialHolidayPay + LegalHolidayPay)
            End Get
        End Property

        Private Property SSSAmount As Decimal
        Private Property ECAmount As Decimal
        Private Property HDMFAmount As Decimal
        Private Property PhilHealthAmount As Decimal
        Private Property HMOAmount As Decimal
        Private Property ThirteenthMonthPay As Decimal
        Private Property LeaveAmount As Decimal
        Private Property NetPay As Decimal

        Private _lookUp As Dictionary(Of String, String)

        Public ReadOnly Property LookUp As Dictionary(Of String, String)
            Get
                If _lookUp IsNot Nothing Then Return _lookUp

                _lookUp = New Dictionary(Of String, String)
                _lookUp("EmployeeId") = Me.EmployeeId.ToString
                _lookUp("EmployeeName") = Me.EmployeeName
                _lookUp("TotalDays") = Me.RegularDays.ToString
                _lookUp("TotalHours") = Me.RegularHours.ToString
                _lookUp("DailyRate") = Me.DailyRate.ToString
                _lookUp("HoulyRate") = Me.HourlyRate.ToString
                _lookUp("BasicPay") = Me.RegularPay.ToString
                _lookUp("TotalOvertimeHours") = Me.TotalOvertimeHours.ToString
                _lookUp("TotalOvertimePay") = Me.TotalOvertimePay.ToString
                _lookUp("SpecialHolidayHours") = Me.SpecialHolidayHours.ToString
                _lookUp("SpecialHolidayPay") = Me.SpecialHolidayPay.ToString
                _lookUp("LegalHolidayHours") = Me.LegalHolidayHours.ToString
                _lookUp("LegalHolidayPay") = Me.LegalHolidayPay.ToString
                _lookUp("GrossPay") = Me.GrossPay.ToString
                _lookUp("SSSAmount") = Me.SSSAmount.ToString
                _lookUp("ECAmount") = Me.ECAmount.ToString
                _lookUp("HDMFAmount") = Me.HDMFAmount.ToString
                _lookUp("PhilHealthAmount") = Me.PhilHealthAmount.ToString
                _lookUp("HMOAmount") = Me.HMOAmount.ToString
                _lookUp("ThirteenthMonthPay") = Me.ThirteenthMonthPay.ToString
                _lookUp("LeaveAmount") = Me.LeaveAmount.ToString
                _lookUp("NetPay") = Me.NetPay.ToString

                Return _lookUp
            End Get
        End Property

    End Class

End Class