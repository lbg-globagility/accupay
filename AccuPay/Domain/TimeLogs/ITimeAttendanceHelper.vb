Imports AccuPay.Entity
Imports AccuPay.Helper.TimeLogsReader

Public Interface ITimeAttendanceHelper

    Function GenerateTimeLogs() As List(Of TimeLog)

    Function GenerateTimeAttendanceLogs() As List(Of TimeAttendanceLog)

    Function Analyze() As List(Of ImportTimeAttendanceLog)

End Interface
