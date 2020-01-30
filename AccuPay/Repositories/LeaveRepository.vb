Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Entity
Imports AccuPay.Extensions
Imports AccuPay.Helpers
Imports AccuPay.ModelData
Imports Microsoft.EntityFrameworkCore

Namespace Global.AccuPay.Repository

    Public Class LeaveRepository

        Private VALIDATABLE_TYPES As New List(Of String) From {
                    ProductConstant.SICK_LEAVE,
                    ProductConstant.VACATION_LEAVE
            }

        Public Function GetStatusList() As List(Of String)
            Return New List(Of String) From {
                    Leave.StatusPending,
                    Leave.StatusApproved
            }
        End Function

        Public Async Function GetByEmployeeAsync(
            employeeId As Integer?) As _
            Task(Of IEnumerable(Of Leave))

            Using context = New PayrollContext()

                Return Await context.Leaves.
                        Where(Function(l) l.EmployeeID.Value = employeeId.Value).
                        ToListAsync

            End Using

        End Function

        Public Async Function SaveManyAsync(leaves As List(Of Leave)) As Task

            If leaves.Any = False Then Return

            Dim policy As New PolicyHelper

            Dim employeeShifts As New List(Of ShiftSchedule)
            Dim shiftSchedules As New List(Of EmployeeDutySchedule)
            Dim employees As New List(Of Employee)

            Dim orderedLeaves = leaves.OrderBy(Function(l) l.StartDate).ToList

            Dim firstLeave = leaves.FirstOrDefault.StartDate
            Dim lastLeave = leaves.LastOrDefault.StartDate

            Using context As New PayrollContext

                If policy.ValidateLeaveBalance Then
                    Dim employeeIds = leaves.Select(Function(l) l.EmployeeID).Distinct

                    employeeShifts = Await GetEmployeeShifts(employeeIds, firstLeave, lastLeave, context)
                    shiftSchedules = Await GetShiftSchedules(employeeIds, firstLeave, lastLeave, context)
                    employees = Await GetEmployees(employeeIds, context)

                End If

                Await context.Leaves.LoadAsync()

                For Each leave In leaves

                    Await Me.SaveAsync(leave, context)

                    If policy.ValidateLeaveBalance Then

                        Dim employee = employees.FirstOrDefault(Function(e) Nullable.Equals(e.RowID, leave.EmployeeID))

                        Dim unusedApprovedLeaves = Await GetUnusedApprovedLeavesByType(context, employee.RowID, leave)

                        Dim earliestUnusedApprovedLeave = unusedApprovedLeaves.OrderBy(Function(l) l.StartDate).FirstOrDefault

                        'if the earliest unused approved leave is earlier than the first leave, get its shifts
                        If earliestUnusedApprovedLeave IsNot Nothing AndAlso
                            earliestUnusedApprovedLeave.StartDate.ToMinimumHourValue < firstLeave.ToMinimumHourValue Then

                            Dim firstShiftDate = earliestUnusedApprovedLeave.StartDate.ToMinimumHourValue
                            Dim lastShiftDate = firstLeave.ToMinimumHourValue.AddSeconds(-1)

                            Dim earlierEmployeeShifts = Await GetEmployeeShifts({leave.EmployeeID}, firstShiftDate, lastShiftDate, context)
                            Dim earlierShiftSchedules = Await GetShiftSchedules({leave.EmployeeID}, firstShiftDate, lastShiftDate, context)

                            employeeShifts.InsertRange(0, earlierEmployeeShifts)
                            shiftSchedules.InsertRange(0, earlierShiftSchedules)

                            employeeShifts = employeeShifts.OrderBy(Function(s) s.EffectiveFrom).ToList
                            shiftSchedules = shiftSchedules.OrderBy(Function(s) s.DateSched).ToList

                        End If

                        Await ValidateLeaveBalance(policy, employeeShifts, shiftSchedules, unusedApprovedLeaves, employee, leave)

                    End If

                Next

                Await context.SaveChangesAsync()

            End Using

        End Function

        Public Async Function ForceUpdateLeaveAllowance(employeeId As Integer,
                                                        selectedLeaveType As LeaveType.LeaveType,
                                                        newAllowance As Decimal) As Task(Of Decimal)

            Dim newBalance As Decimal = newAllowance

            Dim currentPayPeriod = Await PayrollTools.GetCurrentlyWorkedOnPayPeriodByCurrentYear()

            Dim firstPayPeriodOfTheYear = Await PayrollTools.GetFirstPayPeriodOfTheYear(context:=Nothing,
                                                        currentPayPeriod:=CType(currentPayPeriod, PayPeriod))

            Dim firstDayOfTheWorkingYear = firstPayPeriodOfTheYear?.PayFromDate

            If currentPayPeriod Is Nothing OrElse
                firstPayPeriodOfTheYear?.RowID Is Nothing OrElse
                firstDayOfTheWorkingYear Is Nothing Then

                Throw New Exception("Cannot retrieve current pay period or the first days of the working year.")

            End If

            Using context As New PayrollContext

                '#1. Update employee's leave allowance
                '#2. Update employee's leave transactions

                Dim employee = Await context.Employees.
                                FirstOrDefaultAsync(Function(e) e.RowID.Value = employeeId)

                Dim leaveLedgerQuery = context.LeaveLedgers.
                                        Include(Function(l) l.Product).
                                        Where(Function(l) l.EmployeeID.Value = employeeId)

                '#1
                UpdateEmployeeLeaveAllowanceAndUpdateLeaveLedgerQuery(selectedLeaveType, newAllowance, employee, leaveLedgerQuery)

                Dim leaveLedger = Await leaveLedgerQuery.FirstOrDefaultAsync()
                Dim leaveLedgerId = leaveLedger?.RowID

                If leaveLedgerId Is Nothing Then

                    Throw New Exception("Cannot retrieve the leave ledger ID.")

                End If

                Console.WriteLine($"Leave ledger ID: {leaveLedgerId}")

                Dim leaveTransactions = Await context.LeaveTransactions.
                                                Where(Function(l) l.EmployeeID.Value = employeeId).
                                                Where(Function(l) l.TransactionDate >= firstDayOfTheWorkingYear.Value).
                                                Where(Function(l) l.LeaveLedgerID.Value = leaveLedgerId.Value).
                                                OrderBy(Function(l) l.TransactionDate).
                                                ToListAsync()

                '2.1. Add the first credit (the beginning balance).
                '2.2. Remove existing credits from database. It should only have one credit, on the first cutoff, then all debits.
                '2.3. Save all debits but their balance should be recalculated to adjust to the new allowance.
                '2.4. Update the leaveledger's last transaction ID.
                '2.5. Check if there is a last transaction ID saved for the leaveledger.
                '     If no last transaction ID, this means that the employee did not have a leave for the current pay period.
                '     Use the newly added first credit (the beginning balance) as the last transaction ID for the leaveledger.

                Dim lastTransactionId As Integer? = Nothing

                '#2.1
                Dim beginningTransaction As New LeaveTransaction
                beginningTransaction.OrganizationID = z_OrganizationID
                beginningTransaction.CreatedBy = z_User
                beginningTransaction.EmployeeID = employeeId
                beginningTransaction.ReferenceID = Nothing
                beginningTransaction.LeaveLedgerID = leaveLedgerId
                beginningTransaction.PayPeriodID = firstPayPeriodOfTheYear.RowID
                beginningTransaction.TransactionDate = firstDayOfTheWorkingYear.Value
                beginningTransaction.Type = LeaveTransactionType.Credit
                beginningTransaction.Amount = newAllowance
                beginningTransaction.Balance = newAllowance

                context.LeaveTransactions.Add(beginningTransaction)

                '-
                For Each leaveTransaction In leaveTransactions

                    If leaveTransaction.IsCredit Then

                        '#2.2
                        context.Remove(leaveTransaction)
                    Else

                        '#2.3
                        newBalance = newBalance - leaveTransaction.Amount
                        leaveTransaction.Balance = newBalance
                        leaveTransaction.LastUpdBy = z_User

                        lastTransactionId = leaveTransaction.RowID

                    End If

                Next

                '#2.4
                'lastTransactionId will be null if the employee did not use at least one leave for the
                'whole year. This will be managed in section 2.5
                leaveLedger.LastTransactionID = lastTransactionId
                leaveLedger.LastUpdBy = z_User

                Await context.SaveChangesAsync()

                '#2.5
                Await ProcessIfEmployeeHasNotTakenAleaveThisYear(firstPayPeriodOfTheYear, context, leaveLedgerId)

            End Using

            Return newBalance

        End Function

        Private Shared Async Function ProcessIfEmployeeHasNotTakenAleaveThisYear(firstPayPeriodOfTheYear As PayPeriod, context As PayrollContext, leaveLedgerId As Integer?) As Task
            Dim updatedLeaveLedger = Await context.LeaveLedgers.
                                                    FirstOrDefaultAsync(Function(l) l.RowID.Value = leaveLedgerId.Value)

            If updatedLeaveLedger Is Nothing Then

                Throw New ArgumentException("Cannot find leave ledger.")

            ElseIf updatedLeaveLedger.LastTransactionID Is Nothing Then

                'get the beginning balance transaction geting it from the first payperiod of the year
                'that we added earlier [using GUID as rowids would have made this easier]
                Dim updatedBeginningTransaction = Await context.LeaveTransactions.
                                Where(Function(t) t.PayPeriodID.Value = firstPayPeriodOfTheYear.RowID.Value).
                                Where(Function(t) t.LeaveLedgerID.Value = updatedLeaveLedger.RowID.Value).
                                Where(Function(t) t.IsCredit).
                                OrderByDescending(Function(t) t.Amount).
                                FirstOrDefaultAsync()

                If updatedBeginningTransaction Is Nothing Then

                    Throw New ArgumentException("Was not able to create a beginning transaction")

                End If

                updatedLeaveLedger.LastTransactionID = updatedBeginningTransaction.RowID
                updatedLeaveLedger.LastUpdBy = z_User
                Await context.SaveChangesAsync()

            End If
        End Function

        Private Shared Sub UpdateEmployeeLeaveAllowanceAndUpdateLeaveLedgerQuery(selectedLeaveType As LeaveType.LeaveType, newAllowance As Decimal, ByRef employee As Employee, ByRef leaveLedgerQuery As IQueryable(Of LeaveLedger))
            Select Case selectedLeaveType
                Case LeaveType.LeaveType.Sick
                    leaveLedgerQuery = leaveLedgerQuery.Where(Function(l) l.Product.IsSickLeave)
                    employee.SickLeaveAllowance = newAllowance

                Case LeaveType.LeaveType.Vacation
                    leaveLedgerQuery = leaveLedgerQuery.Where(Function(l) l.Product.IsVacationLeave)
                    employee.VacationLeaveAllowance = newAllowance

                Case LeaveType.LeaveType.Others
                    leaveLedgerQuery = leaveLedgerQuery.Where(Function(l) l.Product.IsOthersLeave)
                    employee.OtherLeaveAllowance = newAllowance

                Case LeaveType.LeaveType.Maternity
                    leaveLedgerQuery = leaveLedgerQuery.Where(Function(l) l.Product.IsMaternityLeave)
                    employee.MaternityLeaveAllowance = newAllowance

                Case LeaveType.LeaveType.Parental
                    leaveLedgerQuery = leaveLedgerQuery.Where(Function(l) l.Product.IsParentalLeave)
                    'THIS DOES NOT HAVE AN ALLOWANCE COLUMN
                    Throw New Exception("No column for Parental Leave Allowance on employee table.")

            End Select
        End Sub

        Private Async Function ValidateLeaveBalance(policy As PolicyHelper, employeeShifts As List(Of ShiftSchedule), shiftSchedules As List(Of EmployeeDutySchedule), unusedApprovedLeaves As List(Of Leave), employee As Employee, leave As Leave) As Task
            If leave.Status.Trim.ToLower = Leave.StatusApproved.ToLower AndAlso
                                    policy.ValidateLeaveBalance AndAlso
                                    VALIDATABLE_TYPES.Contains(leave.LeaveType) Then

                Dim totalLeaveHours = ComputeTotalLeaveHours(unusedApprovedLeaves, policy, employeeShifts, shiftSchedules, employee)

                If leave.LeaveType = ProductConstant.SICK_LEAVE Then

                    Dim sickLeaveBalance = Await EmployeeData.GetSickLeaveBalance(employee.RowID)

                    If totalLeaveHours > sickLeaveBalance Then

                        Throw New ArgumentException("Employee will exceed the allowable sick leave hours.")

                    End If

                ElseIf leave.LeaveType = ProductConstant.VACATION_LEAVE Then

                    Dim vacationLeaveBalance = Await EmployeeData.GetVacationLeaveBalance(employee.RowID)

                    If totalLeaveHours > vacationLeaveBalance Then

                        Throw New ArgumentException("Employee will exceed the allowable vacation leave hours.")

                    End If

                End If
            End If
        End Function

        Private Shared Async Function GetEmployees(employeeIds As IEnumerable(Of Integer?), context As PayrollContext) As Task(Of List(Of Employee))
            Return Await context.Employees.
                            Where(Function(e) employeeIds.Contains(e.RowID)).
                            ToListAsync
        End Function

        Private Shared Async Function GetShiftSchedules(employeeIds As IEnumerable(Of Integer?), firstLeave As Date, lastLeave As Date, context As PayrollContext) As Task(Of List(Of EmployeeDutySchedule))
            Return Await context.EmployeeDutySchedules.
                            Where(Function(es) es.OrganizationID.Value = z_OrganizationID).
                            Where(Function(es) firstLeave <= es.DateSched AndAlso es.DateSched <= lastLeave).
                            Where(Function(es) employeeIds.Contains(es.EmployeeID)).
                            ToListAsync()
        End Function

        Private Shared Async Function GetEmployeeShifts(employeeIds As IEnumerable(Of Integer?), firstLeave As Date, lastLeave As Date, context As PayrollContext) As Task(Of List(Of ShiftSchedule))
            Return Await context.ShiftSchedules.
                            Include(Function(s) s.Shift).
                            Where(Function(s) s.OrganizationID = z_OrganizationID).
                            Where(Function(s) s.EffectiveFrom <= lastLeave AndAlso firstLeave <= s.EffectiveTo).
                            Where(Function(s) employeeIds.Contains(s.EmployeeID)).
                            ToListAsync()
        End Function

        Private Function ComputeTotalLeaveHours(
                            leaves As List(Of Leave),
                            policy As PolicyHelper,
                            employeeShifts As List(Of ShiftSchedule),
                            shiftSchedules As List(Of EmployeeDutySchedule),
                            employee As Employee) As Decimal

            If leaves.Any = False Then Return 0

            Dim computeBreakTimeLate = policy.ComputeBreakTimeLate

            Dim totalHours As Decimal = 0

            For Each leave In leaves

                Dim currentDate = leave.StartDate

                Dim employeeShift = employeeShifts.
                    FirstOrDefault(Function(s) s.EffectiveFrom <= currentDate And currentDate <= s.EffectiveTo)

                Dim dutyShiftSched = shiftSchedules.
                    FirstOrDefault(Function(es) es.DateSched = currentDate)

                Dim currentShift = DayCalculator.GetCurrentShift(
                                currentDate,
                                employeeShift,
                                dutyShiftSched,
                                policy.UseShiftSchedule,
                                policy.RespectDefaultRestDay,
                                employee.DayOfRest)

                totalHours += DayCalculator.
                        ComputeLeaveHoursWithoutTimelog(currentShift, leave, computeBreakTimeLate)

            Next

            Return totalHours

        End Function

        Private Shared Async Function GetUnusedApprovedLeavesByType(
                                    context As PayrollContext,
                                    employeeId As Integer?,
                                    leave As Leave) As Task(Of List(Of Leave))

            Dim currentPayPeriod = context.PayPeriods.
                                                    Where(Function(p) Nullable.Equals(p.OrganizationID, z_OrganizationID)).
                                                    Where(Function(p) p.IsSemiMonthly).
                                                    Where(Function(p) p.IsBetween(leave.StartDate)).
                                                    FirstOrDefault

            Dim firstDayOfTheYear As Date? = Await PayrollTools.GetFirstDayOfTheYear(context, currentPayPeriod)

            Dim lastDayOfTheYear = context.PayPeriods.
                                                    Where(Function(p) Nullable.Equals(p.OrganizationID, z_OrganizationID)).
                                                    Where(Function(p) p.IsSemiMonthly).
                                                    Where(Function(p) p.Year = currentPayPeriod.Year).
                                                    Where(Function(p) p.IsLastPayPeriodOfTheYear).
                                                    FirstOrDefault?.PayToDate

            If firstDayOfTheYear Is Nothing OrElse lastDayOfTheYear Is Nothing Then Return New List(Of Leave)

            Return context.Leaves.Local.
                        Where(Function(l) Nullable.Equals(l.EmployeeID, employeeId)).
                        Where(Function(l) l.Status.Trim.ToUpper = Leave.StatusApproved.ToUpper).
                        Where(Function(l) l.LeaveType = leave.LeaveType).
                        Where(Function(l) l.StartDate >= firstDayOfTheYear.Value).
                        Where(Function(l) l.StartDate <= lastDayOfTheYear.Value).
                        Where(Function(l) context.LeaveTransactions.
                                            Where(Function(t) Nullable.Equals(t.ReferenceID, l.RowID)).
                                            Count = 0).
                        ToList
        End Function

        ''' <summary>
        ''' This inserts or updates a leave depending if its RowID is null.
        ''' </summary>
        ''' <param name="leave">The leave that will be inserted/updated.</param>
        ''' <param name="context">If there is no context provided, SaveAsync will save the changes to database automatically.</param>
        Public Async Function SaveAsync(leave As Leave, Optional context As PayrollContext = Nothing) As Task

            leave.OrganizationID = z_OrganizationID
            If leave.StartTime.HasValue Then leave.StartTime = leave.StartTime.Value.StripSeconds
            If leave.EndTime.HasValue Then leave.EndTime = leave.EndTime.Value.StripSeconds

            If context Is Nothing Then

                context = New PayrollContext

                Using context

                    Await SaveAsyncFunction(leave, context)

                    Await context.SaveChangesAsync

                End Using
            Else
                Await SaveAsyncFunction(leave, context)
            End If

        End Function

        Private Async Function SaveAsyncFunction(leave As Leave, context As PayrollContext) As Task

            If context.Leaves.
                Where(Function(l) If(leave.RowID Is Nothing, True, Nullable.Equals(leave.RowID, l.RowID) = False)).
                Where(Function(l) l.EmployeeID.Value = leave.EmployeeID.Value).
                Where(Function(l) (leave.StartDate.Date >= l.StartDate.Date AndAlso leave.StartDate.Date <= l.EndDate.Value.Date) OrElse
                                    (leave.EndDate.Value.Date >= l.StartDate.Date AndAlso leave.EndDate.Value.Date <= l.EndDate.Value.Date)).
                Any() Then

                Throw New ArgumentException($"Employee already has a leave for {leave.StartDate.ToShortDateString()}")
            End If

            If leave.RowID Is Nothing Then

                leave.CreatedBy = z_User
                context.Leaves.Add(leave)
            Else
                Await Me.UpdateAsync(leave, context)
            End If
        End Function

        Private Async Function UpdateAsync(leave As Leave, context As PayrollContext) As Task

            Dim currentLeave = Await context.Leaves.
                FirstOrDefaultAsync(Function(l) Nullable.Equals(l.RowID, leave.RowID))

            If currentLeave Is Nothing Then Return

            currentLeave.LastUpdBy = z_User
            currentLeave.StartTime = leave.StartTime
            currentLeave.EndTime = leave.EndTime
            currentLeave.LeaveType = leave.LeaveType
            currentLeave.StartDate = leave.StartDate
            currentLeave.EndDate = leave.EndDate
            currentLeave.Reason = leave.Reason
            currentLeave.Comments = leave.Comments
            currentLeave.Status = leave.Status

        End Function

    End Class

End Namespace