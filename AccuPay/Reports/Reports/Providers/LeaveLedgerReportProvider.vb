Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Data
Imports AccuPay.Data.Enums
Imports AccuPay.Data.Repositories
Imports AccuPay.Entity
Imports AccuPay.Helpers
Imports AccuPay.Repository
Imports AccuPay.Utilities.Extensions
Imports AccuPay.Utils
Imports CrystalDecisions.CrystalReports.Engine
Imports Microsoft.EntityFrameworkCore

Public Class LeaveLedgerReportProvider
    Implements IReportProvider

    Public Property Name As String = "Leave Ledger" Implements IReportProvider.Name
    Public Property IsHidden As Boolean = False Implements IReportProvider.IsHidden

    Public Property IsNewReport As Boolean = True

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

        If IsNewReport Then
            Await NewReport(dateFrom, dateTo)
        Else
            OldReport(dateSelector, dateFrom, dateTo)
        End If

    End Sub

    Private Async Function NewReport(dateFrom As Date?, dateTo As Date?) As Task

        If dateFrom Is Nothing Or dateTo Is Nothing Then Throw New ArgumentException("Date From or Date To cannot be null")

        Dim leaveLedgerReportModels = Await GenerateLeaveLedgerReportModels(dateFrom, dateTo)

        Dim report As New New_Employee_Leave_Ledger()
        report.SetDataSource(leaveLedgerReportModels)

        Dim title = DirectCast(report.ReportDefinition.Sections(1).ReportObjects("txtCutoffDate"), TextObject)
        Dim dateFromTitle = dateFrom?.ToString("MMM dd, yyyy")
        Dim dateToTitle = dateTo?.ToString("MMM dd, yyyy")
        title.Text = $"From {dateFromTitle} to {dateToTitle}"

        Dim viewer As New CrysRepForm()
        viewer.crysrepvwr.ReportSource = report
        viewer.Show()

    End Function

    Private Async Function GenerateLeaveLedgerReportModels(dateFrom As Date?, dateTo As Date?) As Task(Of List(Of LeaveLedgerReportModel))

        Dim leaveLedgerReportModels As New List(Of LeaveLedgerReportModel)

        Dim startDate = dateFrom.Value.ToMinimumHourValue
        Dim endDate = dateTo.Value.ToMaximumHourValue
        Dim dayBeforeReport = startDate.AddDays(-1).ToMaximumHourValue

        Dim employees = Await _employeeRepository.GetAllActiveAsync(z_OrganizationID)
        Dim employeeIds = employees.Select(Function(e) e.RowID).ToArray()

        Using context As New PayrollContext

            Dim oldLeaveTransactions = Await context.LeaveTransactions.
                                                Include(Function(l) l.LeaveLedger).
                                                Include(Function(l) l.LeaveLedger.Product).
                                                Where(Function(l) l.TransactionDate <= dayBeforeReport).
                                                Where(Function(l) l.LeaveLedger.Product.IsVacationOrSickLeave).
                                                Where(Function(l) employeeIds.Contains(l.EmployeeID)).
                                                OrderBy(Function(l) l.TransactionDate).
                                                ToListAsync

            Dim currentLeaveTransactions = Await context.LeaveTransactions.
                                                Include(Function(l) l.LeaveLedger).
                                                Include(Function(l) l.LeaveLedger.Product).
                                                Where(Function(l) l.TransactionDate >= startDate).
                                                Where(Function(l) l.TransactionDate <= endDate).
                                                Where(Function(l) l.LeaveLedger.Product.IsVacationOrSickLeave).
                                                Where(Function(l) employeeIds.Contains(l.EmployeeID)).
                                                OrderBy(Function(l) l.TransactionDate).
                                                ToListAsync

            Dim timeEntries = Await context.TimeEntries.
                                        Where(Function(t) t.Date >= startDate).
                                        Where(Function(t) t.Date <= endDate).
                                        Where(Function(t) employeeIds.Contains(t.EmployeeID)).
                                        OrderBy(Function(l) l.Date).
                                        ToListAsync

            For Each employee In employees

                Dim vacationLeave = GetVacationLeave(oldLeaveTransactions, currentLeaveTransactions, timeEntries, employee)

                Dim sickLeave = GetSickLeave(oldLeaveTransactions, currentLeaveTransactions, timeEntries, employee)

                leaveLedgerReportModels.Add(vacationLeave)

                leaveLedgerReportModels.Add(sickLeave)
            Next

        End Using

        Return leaveLedgerReportModels
    End Function

    Private Shared Function GetVacationLeave(
                                oldLeaveTransactions As List(Of LeaveTransaction),
                                currentLeaveTransactions As List(Of LeaveTransaction),
                                timeEntries As List(Of TimeEntry),
                                employee As Entities.Employee) As _
                                LeaveLedgerReportModel

        Dim currentEmployeeLeaveTransactions = currentLeaveTransactions.
                                                Where(Function(l) Nullable.Equals(l.EmployeeID, employee.RowID)).
                                                Where(Function(l) If(l.LeaveLedger?.Product?.IsVacationLeave, False)).
                                                ToList

        Dim oldEmployeeLeaveTransactions = oldLeaveTransactions.
                                                Where(Function(l) Nullable.Equals(l.EmployeeID, employee.RowID)).
                                                Where(Function(l) If(l.LeaveLedger?.Product?.IsVacationLeave, False)).
                                                ToList

        Dim employeeTimeEntries = timeEntries.
                                    Where(Function(t) Nullable.Equals(t.EmployeeID, employee.RowID)).
                                    Where(Function(t) t.VacationLeaveHours > 0).
                                    ToList

        Return GetLeave(oldEmployeeLeaveTransactions, currentEmployeeLeaveTransactions, employeeTimeEntries, employee, LeaveType.Vacation)

    End Function

    Private Shared Function GetSickLeave(
                                oldLeaveTransactions As List(Of LeaveTransaction),
                                currentLeaveTransactions As List(Of LeaveTransaction),
                                timeEntries As List(Of TimeEntry),
                                employee As Entities.Employee) As _
                                LeaveLedgerReportModel

        Dim currentEmployeeLeaveTransactions = currentLeaveTransactions.
                                                Where(Function(l) Nullable.Equals(l.EmployeeID, employee.RowID)).
                                                Where(Function(l) If(l.LeaveLedger?.Product?.IsSickLeave, False)).
                                                ToList

        Dim oldEmployeeLeaveTransactions = oldLeaveTransactions.
                                                Where(Function(l) Nullable.Equals(l.EmployeeID, employee.RowID)).
                                                Where(Function(l) If(l.LeaveLedger?.Product?.IsSickLeave, False)).
                                                ToList

        Dim employeeTimeEntries = timeEntries.
                                    Where(Function(t) Nullable.Equals(t.EmployeeID, employee.RowID)).
                                    Where(Function(t) t.SickLeaveHours > 0).
                                    ToList

        Return GetLeave(oldEmployeeLeaveTransactions, currentEmployeeLeaveTransactions, employeeTimeEntries, employee, LeaveType.Sick)

    End Function

    Private Shared Function GetLeave(
                                oldLeaveTransactions As List(Of LeaveTransaction),
                                currentLeaveTransactions As List(Of LeaveTransaction),
                                timeEntries As List(Of TimeEntry),
                                employee As Entities.Employee,
                                leaveType As LeaveType) As _
                                LeaveLedgerReportModel

        Dim leaveBeginningTransaction As New LeaveTransaction

        'check first if leave was reset during the report period
        leaveBeginningTransaction = currentLeaveTransactions.
                                            Where(Function(l) l.IsCredit).
                                            LastOrDefault

        If leaveBeginningTransaction IsNot Nothing Then
            'if it was reset, only get the leaves that are after the reset date
            Dim resetDate = leaveBeginningTransaction.TransactionDate.ToMinimumHourValue
            timeEntries = timeEntries.
                            Where(Function(t) t.Date >= resetDate).
                            ToList
        Else
            'if it was not reset during the report period, get the last leavetransaction before the report period
            leaveBeginningTransaction = oldLeaveTransactions.LastOrDefault

        End If

        Dim leaveBeginningBalance = If(leaveBeginningTransaction?.Balance, 0)

        'get total availed leave
        Dim totalAvailedLeave As Decimal = 0

        If leaveType = LeaveType.Vacation Then

            totalAvailedLeave = timeEntries.Sum(Function(t) t.VacationLeaveHours)

        ElseIf leaveType = LeaveType.Sick Then

            totalAvailedLeave = timeEntries.Sum(Function(t) t.SickLeaveHours)

        End If

        ' return LeaveLedgerReportModel object
        Return New LeaveLedgerReportModel(
            employeeNumber:=employee.EmployeeNo,
            fullName:=NameHelper.FullNameWithMiddleNameInitialLastNameFirst(employee.FirstName, employee.MiddleName, employee.LastName),
            leaveType:=leaveType,
            beginningBalance:=leaveBeginningBalance,
            availedLeave:=totalAvailedLeave
        )

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

        Dim title = DirectCast(report.ReportDefinition.Sections(1).ReportObjects("txtCutoffDate"), TextObject)
        Dim dateFromTitle = dateFrom?.ToString("MMM dd, yyyy")
        Dim dateToTitle = dateTo?.ToString("MMM dd, yyyy")
        title.Text = $"From {dateFromTitle} to {dateToTitle}"

        Dim viewer As New CrysRepForm()
        viewer.crysrepvwr.ReportSource = report
        viewer.Show()
    End Sub

End Class