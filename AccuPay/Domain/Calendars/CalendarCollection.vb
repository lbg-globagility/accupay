Option Strict On

Namespace Global.AccuPay.Entity

    Public Class CalendarCollection

        Private _isUsingCalendars As Boolean

        Private _branches As ICollection(Of Branch)

        Private _calendars As IDictionary(Of Integer?, PayratesCalendar)

        Private _organizationCalendar As PayratesCalendar

        Public Sub New(payrates As ICollection(Of PayRate))
            _organizationCalendar = New PayratesCalendar(payrates)
            _isUsingCalendars = False
        End Sub

        Public Sub New(branches As ICollection(Of Branch), calendarDays As ICollection(Of CalendarDay))
            _branches = branches
            _calendars = calendarDays.
                GroupBy(Function(t) t.CalendarID).
                ToDictionary(Function(t) t.Key, Function(t) New PayratesCalendar(t))
            _isUsingCalendars = True
        End Sub

        Public Function GetCalendar(employee As Employee) As PayratesCalendar
            Dim calendar = If(
                _isUsingCalendars,
                FindCalendarByBranch(employee.BranchID),
                _organizationCalendar)

            If calendar Is Nothing Then
                Throw New Exception("No calendar was found")
            End If

            Return calendar
        End Function

        Private Function FindCalendarByBranch(branchId As Integer?) As PayratesCalendar
            Dim branch = _branches.FirstOrDefault(Function(t) t.RowID.Value = branchId.Value)

            If Not _calendars.ContainsKey(branch.CalendarID) Then
                Throw New Exception($"Calendar for branch #{branchId} doesn't exist")
            End If

            Return _calendars(branch.CalendarID)
        End Function

    End Class

End Namespace
