Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.ModelData
Imports AccuPay.Entity
Imports AccuPay.Helpers
Imports Microsoft.EntityFrameworkCore
Imports AccuPay.Extensions

Namespace Global.AccuPay.Repository

    Public Class LeaveRepository

        Private Const STATUS_APPROVED As String = "Approved"

        Private VALIDATABLE_TYPES As New List(Of String) From {
                    ProductConstant.SICK_LEAVE_PART_NO,
                    ProductConstant.VACATION_LEAVE_PART_NO
            }

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

                        Dim unusedApprovedLeaves = GetUnusedApprovedLeavesByType(context, employee.RowID, leave.LeaveType)

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

        Private Async Function ValidateLeaveBalance(policy As PolicyHelper, employeeShifts As List(Of ShiftSchedule), shiftSchedules As List(Of EmployeeDutySchedule), unusedApprovedLeaves As List(Of Leave), employee As Employee, leave As Leave) As Task
            If leave.Status.Trim.ToLower = STATUS_APPROVED.ToLower AndAlso
                                    policy.ValidateLeaveBalance AndAlso
                                    VALIDATABLE_TYPES.Contains(leave.LeaveType) Then


                Dim totalLeaveHours = ComputeTotalLeaveHours(unusedApprovedLeaves, policy, employeeShifts, shiftSchedules, employee)


                If leave.LeaveType = ProductConstant.SICK_LEAVE_PART_NO Then

                    Dim sickLeaveBalance = Await EmployeeData.GetSickLeaveBalance(employee.RowID)

                    If totalLeaveHours > sickLeaveBalance Then

                        Throw New ArgumentException("Employee will exceed the allowable sick leave hours.")

                    End If

                ElseIf leave.LeaveType = ProductConstant.VACATION_LEAVE_PART_NO Then

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

        Private Shared Function GetUnusedApprovedLeavesByType(
                                    context As PayrollContext,
                                    employeeId As Integer?,
                                    leaveType As String) As List(Of Leave)

            Return context.Leaves.Local.
                        Where(Function(l) Nullable.Equals(l.EmployeeID, employeeId)).
                        Where(Function(l) l.Status.Trim.ToUpper = STATUS_APPROVED.ToUpper).
                        Where(Function(l) l.LeaveType = leaveType).
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
            If leave.RowID Is Nothing Then

                context.Leaves.Add(leave)

            Else
                Await Me.UpdateAsync(leave, context)
            End If
        End Function

        Private Async Function UpdateAsync(leave As Leave, context As PayrollContext) As Task

            Dim currentLeave = Await context.Leaves.
                FirstOrDefaultAsync(Function(l) Nullable.Equals(l.RowID, leave.RowID))

            If currentLeave Is Nothing Then Return

            currentLeave.LastUpdBy = leave.LastUpdBy
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


