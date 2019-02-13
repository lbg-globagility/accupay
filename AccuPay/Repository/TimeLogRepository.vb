Namespace Global.AccuPay.Repository

    Public Class TimeLogRepository
        Private _context As PayrollContext

        Public Sub New(context As PayrollContext)

            _context = context

        End Sub

        Friend Sub Add(timelog As TimeLog)

            Dim duplicateTimeLogs = _context.TimeLogs.
                                    Where(Function(t) t.OrganizationID.Value = timelog.OrganizationID.Value).
                                    Where(Function(t) t.EmployeeID.Value = timelog.EmployeeID.Value).
                                    Where(Function(t) t.LogDate = timelog.LogDate).
                                    ToList

            For Each duplicateTimeLog In duplicateTimeLogs
                '_context.TimeLogs
                'duplicateTimeLog.
            Next

            _context.TimeLogs.Add(timelog)
        End Sub
    End Class

End Namespace
