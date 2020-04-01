Option Strict On

Imports System.Collections.ObjectModel
Imports System.Threading
Imports System.Threading.Tasks
Imports AccuPay.Entity
Imports AccuPay.Tools
Imports log4net
Imports Microsoft.EntityFrameworkCore
Imports Microsoft.Extensions.Logging.Console
Imports PayrollSys

Public Class TimeEntryGenerator
    Private Shared logger As ILog = LogManager.GetLogger("TimeEntryLogger")

    Private ReadOnly _cutoffStart As Date

    Private ReadOnly _cutoffEnd As Date
    Private ReadOnly _threeDays As Integer = 3
    Private _timeEntries As IList(Of TimeEntry)
    Private _actualTimeEntries As IList(Of ActualTimeEntry)
    Private _timeLogs As IList(Of TimeLog)
    Private _overtimes As IList(Of Overtime)
    Private _leaves As IList(Of Leave)
    Private _officialBusinesses As IList(Of OfficialBusiness)
    Private _agencyFees As IList(Of AgencyFee)
    Private _employeeShifts As IList(Of ShiftSchedule)
    Private _salaries As IList(Of Salary)
    Private _shiftSchedules As IList(Of EmployeeDutySchedule)
    Private _timeAttendanceLogs As List(Of TimeAttendanceLog)
    Private _breakTimeBrackets As List(Of BreakTimeBracket)

    Private _total As Integer

    Private _finished As Integer

    Private _errors As Integer
    Private _employees As IList(Of Employee)

    Public ReadOnly Property ErrorCount As Integer
        Get
            Return _errors
        End Get
    End Property

    Public ReadOnly Property Progress As Integer
        Get
            If _finished = 0 Then
                Return 0
            End If

            Return CInt(Math.Floor(_finished / _total * 100))
        End Get
    End Property

    Public Sub New(cutoffStart As Date, cutoffEnd As Date)
        _cutoffStart = cutoffStart
        _cutoffEnd = cutoffEnd
    End Sub

    Public Sub Start()
        Dim employees As IList(Of Employee) = Nothing
        Dim organization As Organization = Nothing
        Dim settings As ListOfValueCollection = Nothing
        Dim agencies As IList(Of Agency) = Nothing
        Dim calendarCollection As CalendarCollection

        Dim timeEntryPolicy As TimeEntryPolicy
        Using context = New PayrollContext()

            settings = New ListOfValueCollection(context.ListOfValues.ToList())
            timeEntryPolicy = New TimeEntryPolicy(settings)

            employees = context.Employees.
                Where(Function(e) e.OrganizationID.Value = z_OrganizationID).
                Include(Function(e) e.Position).
                ToList()
            _employees = employees

            agencies = context.Agencies.
                Where(Function(a) a.OrganizationID.Value = z_OrganizationID).
                ToList()

            organization = context.Organizations.
                SingleOrDefault(Function(o) o.RowID.Value = z_OrganizationID)

            _salaries = context.Salaries.
                Where(Function(s) s.OrganizationID.Value = z_OrganizationID).
                Where(Function(s) s.EffectiveFrom <= _cutoffStart).
                OrderByDescending(Function(s) s.EffectiveFrom).
                GroupBy(Function(s) s.EmployeeID).
                Select(Function(g) g.FirstOrDefault()).
                ToList()

            Dim previousCutoff = _cutoffStart.AddDays(-_threeDays)
            Dim endOfCutOff As Date = _cutoffEnd
            If timeEntryPolicy.PostLegalHolidayCheck Then endOfCutOff = _cutoffEnd.AddDays(_threeDays)
            _timeEntries = context.TimeEntries.
                Where(Function(t) t.OrganizationID.Value = z_OrganizationID).
                Where(Function(t) previousCutoff <= t.Date AndAlso t.Date <= endOfCutOff).
                ToList()

            _actualTimeEntries = context.ActualTimeEntries.
                Where(Function(a) a.OrganizationID.Value = z_OrganizationID).
                Where(Function(a) _cutoffStart <= a.Date AndAlso a.Date <= _cutoffEnd).
                ToList()

            _timeLogs = context.TimeLogs.
                Where(Function(t) t.OrganizationID.Value = z_OrganizationID).
                Where(Function(t) _cutoffStart <= t.LogDate AndAlso t.LogDate <= _cutoffEnd).
                ToList()

            _leaves = context.Leaves.
                Where(Function(l) l.OrganizationID.Value = z_OrganizationID).
                Where(Function(l) _cutoffStart <= l.StartDate AndAlso l.StartDate <= _cutoffEnd).
                Where(Function(l) l.Status = Leave.StatusApproved).
                ToList()

            _overtimes = context.Overtimes.
                Where(Function(o) o.OrganizationID.Value = z_OrganizationID).
                Where(Function(o) _cutoffStart <= o.OTStartDate AndAlso o.OTStartDate <= _cutoffEnd).
                Where(Function(o) o.Status = Overtime.StatusApproved).
                ToList()

            _officialBusinesses = context.OfficialBusinesses.
                Where(Function(o) o.OrganizationID.Value = z_OrganizationID).
                Where(Function(o) o.StartDate.Value <= _cutoffEnd AndAlso _cutoffStart <= o.EndDate.Value).
                Where(Function(o) o.Status = OfficialBusiness.StatusApproved).
                ToList()

            _agencyFees = context.AgencyFees.
                Where(Function(a) a.OrganizationID.Value = z_OrganizationID).
                Where(Function(a) _cutoffStart <= a.Date AndAlso a.Date <= _cutoffEnd).
                ToList()

            _employeeShifts = context.ShiftSchedules.
                Include(Function(s) s.Shift).
                Where(Function(s) s.OrganizationID = z_OrganizationID).
                Where(Function(s) s.EffectiveFrom <= _cutoffEnd AndAlso _cutoffStart <= s.EffectiveTo).
                ToList()

            _shiftSchedules = context.EmployeeDutySchedules.
                Where(Function(es) es.OrganizationID.Value = z_OrganizationID).
                Where(Function(es) _cutoffStart <= es.DateSched AndAlso es.DateSched <= _cutoffEnd).
                ToList()

            If timeEntryPolicy.ComputeBreakTimeLate Then
                _timeAttendanceLogs = context.TimeAttendanceLogs.
                                Where(Function(t) Nullable.Equals(t.OrganizationID, z_OrganizationID)).
                                Where(Function(t) _cutoffStart <= t.WorkDay AndAlso t.WorkDay <= _cutoffEnd).
                                ToList()

                _breakTimeBrackets = context.BreakTimeBrackets.
                                Include(Function(b) b.Division).
                                Where(Function(b) Nullable.Equals(b.Division.OrganizationID, z_OrganizationID)).
                                ToList()
            Else
                _timeAttendanceLogs = New List(Of TimeAttendanceLog)
                _breakTimeBrackets = New List(Of BreakTimeBracket)
            End If

            Dim payrateCalculationBasis = settings.GetEnum("Pay rate.CalculationBasis",
                                            AccuPay.PayRateCalculationBasis.Organization)

            If payrateCalculationBasis = AccuPay.PayRateCalculationBasis.Branch Then
                Dim branches = context.Branches.ToList()

                Dim calendarDays = context.CalendarDays.
                    Include(Function(t) t.DayType).
                    Where(Function(t) previousCutoff <= t.Date AndAlso t.Date <= _cutoffEnd).
                    ToList()

                calendarCollection = New CalendarCollection(branches, calendarDays)
            Else
                Dim payrates = context.PayRates.
                    Where(Function(p) p.OrganizationID.Value = z_OrganizationID).
                    Where(Function(p) previousCutoff <= p.Date AndAlso p.Date <= _cutoffEnd).
                    ToList()

                calendarCollection = New CalendarCollection(payrates)
            End If
        End Using

        Dim progress = New ObservableCollection(Of Integer)

        _total = employees.Count

        Parallel.ForEach(employees,
            Sub(employee)
                Try
                    CalculateEmployeeEntries(employee, organization, settings, agencies, timeEntryPolicy, calendarCollection)
                Catch ex As Exception
                    logger.Error(ex.Message, ex)
                    _errors += 1
                End Try

                Interlocked.Increment(_finished)
            End Sub)
    End Sub

    Private Sub CalculateEmployeeEntries(employee As Employee,
                                         organization As Organization,
                                         settings As ListOfValueCollection,
                                         agencies As IList(Of Agency),
                                         timeEntryPolicy As TimeEntryPolicy,
                                         calendarCollection As CalendarCollection)
        Dim previousTimeEntries As IList(Of TimeEntry) = _timeEntries.
            Where(Function(t) Nullable.Equals(t.EmployeeID, employee.RowID)).
            ToList()

        Dim actualTimeEntries As IList(Of ActualTimeEntry) = _actualTimeEntries.
            Where(Function(a) Nullable.Equals(a.EmployeeID, employee.RowID)).
            ToList()

        Dim salary = _salaries.
            FirstOrDefault(Function(s) Nullable.Equals(s.EmployeeID, employee.RowID))

        Dim timeLogs As IList(Of TimeLog) = _timeLogs.
            Where(Function(t) Nullable.Equals(t.EmployeeID, employee.RowID)).
            ToList()

        Dim shiftSchedules As IList(Of ShiftSchedule) = _employeeShifts.
            Where(Function(s) Nullable.Equals(s.EmployeeID, employee.RowID)).
            ToList()

        Dim overtimesInCutoff As IList(Of Overtime) = _overtimes.
            Where(Function(o) Nullable.Equals(o.EmployeeID, employee.RowID)).
            ToList()

        Dim officialBusinesses As IList(Of OfficialBusiness) = _officialBusinesses.
            Where(Function(o) Nullable.Equals(o.EmployeeID, employee.RowID)).
            ToList()

        Dim leavesInCutoff As IList(Of Leave) = _leaves.
            Where(Function(l) Nullable.Equals(l.EmployeeID, employee.RowID)).
            Where(Function(l) l.LeaveType <> "Leave w/o Pay").
            ToList()

        Dim agencyFees As IList(Of AgencyFee) = _agencyFees.
            Where(Function(a) Nullable.Equals(a.EmployeeID, employee.RowID)).
            ToList()

        Dim dutyShiftSchedules = _shiftSchedules.
            Where(Function(es) Nullable.Equals(es.EmployeeID, employee.RowID)).
            ToList()

        Dim timeAttendanceLogs As IList(Of TimeAttendanceLog) = _timeAttendanceLogs.
            Where(Function(t) Nullable.Equals(t.EmployeeID, employee.RowID)).
            ToList()

        Dim breakTimeBrackets As IList(Of BreakTimeBracket) = _breakTimeBrackets.
            Where(Function(b) Nullable.Equals(b.DivisionID, employee.Position?.DivisionID)).
            ToList()

        Dim payrateCalendar = calendarCollection.GetCalendar(employee)

        If employee.IsActive = False Then
            Dim currentTimeEntries = previousTimeEntries.
                Where(Function(t) _cutoffStart <= t.Date And t.Date <= _cutoffEnd)

            If Not currentTimeEntries.Any() Then
                Return
            End If
        End If

        If Not (timeLogs.Any() Or leavesInCutoff.Any() Or officialBusinesses.Any()) AndAlso (Not employee.IsFixed) Then
            Return
        End If

        Dim dayCalculator = New DayCalculator(organization, settings, payrateCalendar, employee)

        Dim timeEntries = New List(Of TimeEntry)
        For Each currentDate In Calendar.EachDay(_cutoffStart, _cutoffEnd)

            Try
                Dim timelog = timeLogs.OrderByDescending(Function(t) t.LastUpd).FirstOrDefault(Function(t) t.LogDate = currentDate)
                Dim employeeShift = shiftSchedules.FirstOrDefault(Function(s) s.EffectiveFrom <= currentDate And currentDate <= s.EffectiveTo)
                Dim overtimes = overtimesInCutoff.Where(Function(o) o.OTStartDate <= currentDate And currentDate <= o.OTEndDate).ToList()
                Dim leaves = leavesInCutoff.Where(Function(l) l.StartDate = currentDate).ToList()
                Dim officialBusiness = officialBusinesses.FirstOrDefault(Function(o) o.StartDate.Value = currentDate)
                Dim dutyShiftSched = dutyShiftSchedules.FirstOrDefault(Function(es) es.DateSched = currentDate)
                Dim currentTimeAttendanceLogs = timeAttendanceLogs.Where(Function(l) l.WorkDay = currentDate).ToList()

                Dim timeEntry = dayCalculator.Compute(
                    currentDate,
                    salary,
                    previousTimeEntries,
                    employeeShift,
                    dutyShiftSched,
                    timelog,
                    overtimes,
                    officialBusiness,
                    leaves,
                    currentTimeAttendanceLogs,
                    breakTimeBrackets)

                timeEntries.Add(timeEntry)
            Catch ex As Exception
                Throw New Exception($"{currentDate} #{employee.EmployeeNo}", ex)
            End Try

        Next

        PostLegalHolidayCheck(timeEntries, payrateCalendar, timeEntryPolicy)
        timeEntries.ForEach(Sub(t)
                                t.RegularHolidayPay += t.BasicRegularHolidayPay
                                t.TotalDayPay += t.BasicRegularHolidayPay
                            End Sub)

        If employee.IsUnderAgency Then
            Dim agency = agencies.SingleOrDefault(Function(a) Nullable.Equals(a.RowID, employee.AgencyID))

            Dim agencyCalculator = New AgencyFeeCalculator(employee, agency, agencyFees)
            agencyFees = agencyCalculator.Compute(timeEntries)
        End If

        Dim actualTimeEntryCalculator = New ActualTimeEntryCalculator(salary, actualTimeEntries, New ActualTimeEntryPolicy(settings))
        actualTimeEntries = actualTimeEntryCalculator.Compute(timeEntries)

        Using context = New PayrollContext()
            AddTimeEntriesToContext(context, timeEntries)
            AddActualTimeEntriesToContext(context, actualTimeEntries)
            AddAgencyFeesToContext(context, agencyFees)
            context.SaveChanges()
        End Using
    End Sub

    Private Sub PostLegalHolidayCheck(timeEntries As List(Of TimeEntry), payrateCalendar As PayratesCalendar, timeEntryPolicy As TimeEntryPolicy)
        If timeEntryPolicy.PostLegalHolidayCheck Then
            Dim employeeId = timeEntries.FirstOrDefault?.EmployeeID
            Dim employees = _employees.
                Where(Function(e) e.CalcHoliday).
                Where(Function(e) Equals(employeeId, e.RowID)).
                ToList()

            Dim isEntitledForLegalHolidayPay = employees.Any()
            If Not isEntitledForLegalHolidayPay Then Return

            Dim legalHolidays = payrateCalendar.LegalHolidays

            If legalHolidays.Any() Then
                Dim legalHolidayDescDates = legalHolidays.
                    Select(Function(p) p.Date).
                    OrderByDescending(Function(d) d).
                    ToList()

                Dim employee = employees.FirstOrDefault
                For Each holidayDate In legalHolidayDescDates
                    Dim presentAfterLegalHoliday = PayrollTools.HasWorkAfterLegalHoliday(
                        holidayDate, _cutoffEnd, timeEntries, payrateCalendar)

                    If Not presentAfterLegalHoliday Then
                        Dim timeEntry = timeEntries.Where(Function(t) t.Date = holidayDate).FirstOrDefault

                        timeEntry.BasicRegularHolidayPay = 0
                        'If employee.IsMonthly Then

                        'ElseIf employee.IsDaily Then

                        'End If
                    End If
                Next
            End If
        End If
    End Sub

    Private Sub AddTimeEntriesToContext(context As PayrollContext, timeEntries As IList(Of TimeEntry))
        For Each timeEntry In timeEntries
            If timeEntry.RowID.HasValue Then
                context.Entry(timeEntry).State = EntityState.Modified
            Else
                context.TimeEntries.Add(timeEntry)
            End If
        Next
    End Sub

    Private Sub AddActualTimeEntriesToContext(context As PayrollContext, actualTimeEntries As IList(Of ActualTimeEntry))
        For Each actualTimeEntry In actualTimeEntries
            If actualTimeEntry.RowID.HasValue Then
                context.Entry(actualTimeEntry).State = EntityState.Modified
            Else
                context.ActualTimeEntries.Add(actualTimeEntry)
            End If
        Next
    End Sub

    Private Sub AddAgencyFeesToContext(context As PayrollContext, agencyFees As IList(Of AgencyFee))
        For Each agencyFee In agencyFees
            If agencyFee.RowID.HasValue Then
                context.Entry(agencyFee).State = EntityState.Modified
            Else
                context.AgencyFees.Add(agencyFee)
            End If
        Next
    End Sub

    Private Function GetAgencyFees(context As PayrollContext, employee As Employee) As IList(Of AgencyFee)
        Return context.AgencyFees.
            Where(Function(a) Nullable.Equals(a.EmployeeID, employee.RowID)).
            Where(Function(a) _cutoffStart <= a.Date And a.Date <= _cutoffEnd).
            ToList()
    End Function

    Private Function GetSalary(context As PayrollContext, employee As Employee) As Salary
        Return context.Salaries.
            Where(Function(s) Nullable.Equals(s.EmployeeID, employee.RowID)).
            Where(Function(s) s.EffectiveFrom <= _cutoffStart And _cutoffStart <= If(s.EffectiveTo, _cutoffStart)).
            FirstOrDefault()
    End Function

    Private Function GetTimeEntries(context As PayrollContext, employee As Employee) As IList(Of TimeEntry)
        Dim previousCutoff = _cutoffStart.AddDays(-3)

        Return context.TimeEntries.
            Where(Function(t) Nullable.Equals(t.EmployeeID, employee.RowID)).
            Where(Function(t) previousCutoff <= t.Date And t.Date <= _cutoffEnd).
            ToList()
    End Function

    Private Function GetActualTimeEntries(context As PayrollContext, employee As Employee) As IList(Of ActualTimeEntry)
        Return context.ActualTimeEntries.
            Where(Function(t) Nullable.Equals(t.EmployeeID, employee.RowID)).
            Where(Function(t) _cutoffStart <= t.Date And t.Date <= _cutoffEnd).
            ToList()
    End Function

    Private Function GetTimeLogs(context As PayrollContext, employee As Employee) As IList(Of TimeLog)
        Return context.TimeLogs.
            Where(Function(t) Nullable.Equals(t.EmployeeID, employee.RowID)).
            Where(Function(t) _cutoffStart <= t.LogDate And t.LogDate <= _cutoffEnd).
            ToList()
    End Function

    Private Function GetShifts(context As PayrollContext, employee As Employee) As IList(Of ShiftSchedule)
        Return context.ShiftSchedules.
            Include(Function(s) s.Shift).
            Where(Function(s) Nullable.Equals(s.EmployeeID, employee.RowID)).
            Where(Function(s) s.EffectiveFrom <= _cutoffEnd And _cutoffStart <= s.EffectiveTo).
            ToList()
    End Function

    Private Function GetOvertimes(context As PayrollContext, employee As Employee) As IList(Of Overtime)
        Return context.Overtimes.
            Where(Function(o) Nullable.Equals(o.EmployeeID, employee.RowID)).
            Where(Function(o) o.OTStartDate <= _cutoffEnd And _cutoffStart <= o.OTEndDate).
            ToList()
    End Function

    Private Function GetOfficialBusinesses(context As PayrollContext, employee As Employee) As IList(Of OfficialBusiness)
        Return context.OfficialBusinesses.
            Where(Function(o) Nullable.Equals(o.EmployeeID, employee.RowID)).
            Where(Function(o) o.StartDate.Value <= _cutoffEnd And _cutoffStart <= o.EndDate.Value).
            Where(Function(o) o.Status = OfficialBusiness.StatusApproved).
            ToList()
    End Function

    Private Function GetLeaves(context As PayrollContext, employee As Employee) As IList(Of Leave)
        Return context.Leaves.
            Where(Function(l) Nullable.Equals(l.EmployeeID, employee.RowID)).
            Where(Function(l) _cutoffStart <= l.StartDate And l.StartDate <= _cutoffEnd).
            Where(Function(l) l.Status = Leave.StatusApproved).
            ToList()
    End Function

End Class
