Option Strict On

Imports System.Collections.ObjectModel
Imports System.Threading
Imports System.Threading.Tasks
Imports AccuPay.Entity
Imports AccuPay.Tools
Imports Microsoft.EntityFrameworkCore
Imports PayrollSys

Public Class TimeEntryGenerator

    Private ReadOnly _cutoffStart As Date

    Private ReadOnly _cutoffEnd As Date

    Private _total As Integer

    Private _finished As Integer

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
        Dim payrateCalendar As PayratesCalendar = Nothing
        Dim settings As ListOfValueCollection = Nothing
        Dim agencies As IList(Of Agency) = Nothing

        Using context = New PayrollContext()
            employees = context.Employees.
                Where(Function(e) Nullable.Equals(e.OrganizationID, z_OrganizationID)).
                ToList()

            agencies = context.Agencies.
                Where(Function(a) Nullable.Equals(a.OrganizationID, z_OrganizationID)).
                ToList()

            organization = context.Organizations.
                FirstOrDefault(Function(o) Nullable.Equals(o.RowID, z_OrganizationID))

            Dim previousCutoff = _cutoffStart.AddDays(-3)

            Dim payRates =
                (From p In context.PayRates
                 Where p.OrganizationID = z_OrganizationID And
                     p.Date >= previousCutoff And
                     p.Date <= _cutoffEnd).
                ToList()

            payrateCalendar = New PayratesCalendar(payRates)

            settings = New ListOfValueCollection(context.ListOfValues.ToList())
        End Using

        Dim progress = New ObservableCollection(Of Integer)

        _total = employees.Count

        Parallel.ForEach(employees,
            Sub(employee)
                CalculateEmployee(employee, organization, payrateCalendar, settings, agencies)

                Interlocked.Increment(_finished)
            End Sub)
    End Sub

    Private Sub CalculateEmployee(employee As Employee, organization As Organization, payrateCalendar As PayratesCalendar, settings As ListOfValueCollection, agencies As IList(Of Agency))
        Dim previousTimeEntries As IList(Of TimeEntry) = Nothing
        Dim actualTimeEntries As IList(Of ActualTimeEntry) = Nothing
        Dim salary As Salary = Nothing
        Dim timeLogs As IList(Of TimeLog) = Nothing
        Dim shiftSchedules As IList(Of ShiftSchedule) = Nothing
        Dim overtimesInCutoff As IList(Of Overtime) = Nothing
        Dim officialBusinesses As IList(Of OfficialBusiness) = Nothing
        Dim leavesInCutoff As IList(Of Leave) = Nothing
        Dim agencyFees As IList(Of AgencyFee) = Nothing

        Using context = New PayrollContext()
            salary = GetSalary(context, employee)
            previousTimeEntries = GetTimeEntries(context, employee)
            actualTimeEntries = GetActualTimeEntries(context, employee)
            timeLogs = GetTimeLogs(context, employee)
            shiftSchedules = GetShifts(context, employee)
            overtimesInCutoff = GetOvertimes(context, employee)
            officialBusinesses = GetOfficialBusinesses(context, employee)
            leavesInCutoff = GetLeaves(context, employee)
            agencyFees = GetAgencyFees(context, employee)
        End Using

        If employee.EmploymentStatus = "Resigned" OrElse employee.EmploymentStatus = "Terminated" Then
            Dim currentTimeEntries = previousTimeEntries.
                Where(Function(t) _cutoffStart <= t.Date And t.Date <= _cutoffEnd)

            If Not currentTimeEntries.Any() Then
                Return
            End If
        End If

        Dim dayCalculator = New DayCalculator(organization, settings, payrateCalendar, employee)

        Dim timeEntries = New List(Of TimeEntry)
        For Each currentDate In Calendar.EachDay(_cutoffStart, _cutoffEnd)
            Dim timelog = timeLogs.OrderByDescending(Function(t) t.LastUpd).FirstOrDefault(Function(t) t.LogDate = currentDate)
            Dim shiftSchedule = shiftSchedules.FirstOrDefault(Function(s) s.EffectiveFrom <= currentDate And currentDate <= s.EffectiveTo)
            Dim overtimes = overtimesInCutoff.Where(Function(o) o.OTStartDate <= currentDate And currentDate <= o.OTEndDate).ToList()
            Dim leaves = leavesInCutoff.Where(Function(l) l.StartDate = currentDate).ToList()
            Dim officialBusiness = officialBusinesses.FirstOrDefault(Function(o) o.StartDate = currentDate)

            Dim timeEntry = dayCalculator.Compute(
                currentDate,
                salary,
                previousTimeEntries,
                shiftSchedule,
                timelog,
                overtimes,
                officialBusiness,
                leaves)

            timeEntries.Add(timeEntry)
        Next

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
            Include(Function(t) t.ShiftSchedule.Shift).
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
            Where(Function(o) o.StartDate <= _cutoffEnd And _cutoffStart <= o.EndDate).
            Where(Function(o) o.Status = OfficialBusiness.StatusApproved).
            ToList()
    End Function

    Private Function GetLeaves(context As PayrollContext, employee As Employee) As IList(Of Leave)
        Return context.Leaves.
            Where(Function(l) Nullable.Equals(l.EmployeeID, employee.RowID)).
            Where(Function(l) _cutoffStart <= l.StartDate And l.StartDate <= _cutoffEnd).
            Where(Function(l) l.Status = "Approved").
            ToList()
    End Function

End Class
