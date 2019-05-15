Option Strict On

Imports System.Threading.Tasks
Imports AccuPay
Imports AccuPay.Extensions
Imports AccuPay.Helpers
Imports AccuPay.Repository
Imports AccuPay.Utils
Imports CrystalDecisions.CrystalReports.Engine
Imports Microsoft.EntityFrameworkCore

Public Class LeaveLedgerReportProvider
    Implements IReportProvider

    Public Property Name As String = "Leave Ledger" Implements IReportProvider.Name

    Private _employeeRepository As New EmployeeRepository

    Public Async Sub Run() Implements IReportProvider.Run
        Dim dateSelector As New PayrollSummaDateSelection()

        If Not dateSelector.ShowDialog = Windows.Forms.DialogResult.OK Then
            Return
        End If

        Dim dateFrom = dateSelector.DateFrom
        Dim dateTo = dateSelector.DateTo

        If dateFrom Is Nothing OrElse dateTo Is Nothing Then

            MessageBoxHelper.ErrorMessage("Please select a start date and end date.")
            Return
        End If

        'OldReport(dateSelector, dateFrom, dateTo)
        Await NewReport(dateFrom, dateTo)

    End Sub

    Private Async Function NewReport(dateFrom As Date?, dateTo As Date?) As Task

        If dateFrom Is Nothing Or dateTo Is Nothing Then Throw New ArgumentException("Date From or Date To cannot be null")


        Dim startDate = dateFrom.Value
        Dim endDate = dateTo.Value
        Dim dayBeforeReport = startDate.AddDays(-1).ToMaximumHourValue

        Dim employees = Await _employeeRepository.GetAllActiveAsync()
        Dim employeeIds = employees.Select(Function(e) e.RowID).ToArray()

        Using context As New PayrollContext

            Dim leaveTransactions = Await context.LeaveTransactions.
                                                Include(Function(l) l.LeaveLedger).
                                                Include(Function(l) l.LeaveLedger.Product).
                                                Where(Function(l) l.TransactionDate <= dayBeforeReport).
                                                Where(Function(l) l.LeaveLedger.Product.IsVacationOrSickLeave).
                                                Where(Function(l) employeeIds.Contains(l.EmployeeID)).
                                                ToListAsync

            Dim timeEntries = Await context.TimeEntries.
                                        Where(Function(t) t.Date >= startDate).
                                        Where(Function(t) t.Date <= endDate).
                                        Where(Function(t) employeeIds.Contains(t.EmployeeID)).
                                        ToListAsync

            For Each employee In employees

                Dim vacationLeaveBeginningTransaction = leaveTransactions.
                                                        Where(Function(l) Nullable.Equals(l.EmployeeID, employee.RowID)).
                                                        Where(Function(l) If(l.LeaveLedger?.Product?.IsVacationLeave, False)).
                                                        LastOrDefault

                Dim vacationLeaveBeginningBalance = If(vacationLeaveBeginningTransaction?.Amount, 0)

                Dim sickLeaveBeginningTransaction = leaveTransactions.
                                                        Where(Function(l) Nullable.Equals(l.EmployeeID, employee.RowID)).
                                                        Where(Function(l) If(l.LeaveLedger?.Product?.IsSickLeave, False)).
                                                        LastOrDefault

                Dim sickLeaveBeginningBalance = If(sickLeaveBeginningTransaction?.Amount, 0)

                Dim vacationLeave As New LeaveLedgerReportModel With {
                    .EmployeeNumber = employee.EmployeeNo,
                    .FullName = NameHelper.FullNameWithMiddleNameInitialLastNameFirst(employee.FirstName, employee.MiddleName, employee.LastName),
                    
                }

            Next

        End Using


    End Function

    Private Shared Sub OldReport(dateSelector As PayrollSummaDateSelection, dateFrom As Date?, dateTo As Date?)
        Dim params = New Object(,) {
                    {"OrganizID", orgztnID},
                    {"paramDateFrom", dateFrom},
                    {"paramDateTo", dateTo},
                    {"PayPeriodDateFromID", dateSelector.PayPeriodFromID},
                    {"PayPeriodDateToID", dateSelector.PayPeriodToID}
                }

        Dim data = DirectCast(callProcAsDatTab(params, "RPT_leave_ledger"), DataTable)

        Dim report = New Employee_Leave_Ledger()
        report.SetDataSource(data)

        Dim title = DirectCast(report.ReportDefinition.Sections(1).ReportObjects("txtCutoffDate"), TextObject)
        Dim dateFromTitle = dateFrom?.ToString("MMM dd, yyyy")
        Dim dateToTitle = dateTo?.ToString("MMM dd, yyyy")
        title.Text = $"From {dateFromTitle} to {dateToTitle}"

        Dim viewer As New CrysRepForm()
        viewer.crysrepvwr.ReportSource = report
        viewer.Show()
    End Sub


End Class
