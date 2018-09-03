Option Strict On

Imports AccuPay.Entity
Imports AccuPay.Tools
Imports Microsoft.EntityFrameworkCore
Imports PayrollSys

Public Class TimeEntryGenerator

    Private ReadOnly _cutoffStart As Date

    Private ReadOnly _cutoffEnd As Date

    Public Sub New(cutoffStart As Date, cutoffEnd As Date)
        _cutoffStart = cutoffStart
        _cutoffEnd = cutoffEnd
    End Sub

    Public Sub Start()
        Dim employees As IList(Of Employee) = Nothing
        Dim organization As Organization = Nothing
        Dim payrateCalendar As PayratesCalendar = Nothing
        Dim settings As ListOfValueCollection = Nothing

        Using context = New PayrollContext()
            employees =
                (From e In context.Employees
                 Where e.OrganizationID = z_OrganizationID).
                ToList()

            organization = context.Organizations.
                FirstOrDefault(Function(o) Nullable.Equals(o.RowID, z_OrganizationID))

            Dim previousCutoff = _cutoffStart.AddDays(-3)

            Dim payRates =
                (From p In context.PayRates
                 Where p.OrganizationID = z_OrganizationID And
                     p.RateDate >= previousCutoff And
                     p.RateDate <= _cutoffEnd).
                ToList()

            payrateCalendar = New PayratesCalendar(payRates)

            settings = New ListOfValueCollection(context.ListOfValues.ToList())
        End Using

        For Each employee In employees
            CalculateEmployee(employee, organization, payrateCalendar, settings)
        Next
    End Sub

    Private Sub CalculateEmployee(employee As Employee, organization As Organization, payrateCalendar As PayratesCalendar, settings As ListOfValueCollection)
        Dim previousTimeEntries As IList(Of TimeEntry) = Nothing
        Dim salary As Salary = Nothing
        Using context = New PayrollContext()
            salary = context.Salaries.
                Where(Function(s) Nullable.Equals(s.EmployeeID, employee.RowID)).
                Where(Function(s) s.EffectiveFrom <= _cutoffStart And _cutoffStart <= If(s.EffectiveTo, _cutoffStart)).
                FirstOrDefault()

            Dim previousCutoff = _cutoffStart.AddDays(-3)

            previousTimeEntries = context.TimeEntries.
                Include(Function(t) t.ShiftSchedule.Shift).
                Where(Function(t) Nullable.Equals(t.EmployeeID, employee.RowID)).
                Where(Function(t) previousCutoff <= t.Date And t.Date <= _cutoffEnd).
                ToList()
        End Using

        Dim dayCalculator = New DayCalculator(organization, settings, payrateCalendar, employee)

        Dim timeEntries = New List(Of TimeEntry)
        For Each currentDate In Calendar.EachDay(_cutoffStart, _cutoffEnd)
            Dim timeEntry = dayCalculator.Compute(currentDate, salary, previousTimeEntries)

            timeEntries.Add(timeEntry)
        Next

        Using context = New PayrollContext()
            For Each timeEntry In timeEntries
                If timeEntry.RowID.HasValue Then
                    context.Entry(timeEntry).State = EntityState.Modified
                Else
                    context.TimeEntries.Add(timeEntry)
                End If
            Next

            context.SaveChanges()
        End Using
    End Sub

End Class
