Option Strict On
Imports AccuPay.Entity

Public Class TimeAttendanceAnalyzer

    Public Function Analyze(logs As IList(Of TimeAttendanceLog), employeeshifts As IList(Of ShiftSchedule)) As IList(Of TimeLog)
        Dim list = New List(Of TimeLog)

        For Each employeeshift In employeeshifts
            Dim item = New TimeLog
            Dim result = logs
            Dim a = result.FirstOrDefault
            Dim b = result.LastOrDefault
            item.LogDate = employeeshift.EffectiveFrom
            item.TimeIn = a.DateTime.TimeOfDay
            item.TimeOut = b.DateTime.TimeOfDay
            list.Add(item)
        Next

        Return list
    End Function



    Public Function AnalyzeLogs(logs As IList(Of TimeAttendanceLog), employeeshifts As IList(Of ShiftSchedule)) As IList(Of TimeLog)
        Dim list = New List(Of TimeLog)
        
        For Each employeeshift In employeeshifts
            Dim item = New TimeLog
            Dim result = logs
            Dim a = result.FirstOrDefault
            Dim b = result.LastOrDefault
            item.LogDate = employeeshift.EffectiveFrom
            item.TimeIn = a.DateTime.TimeOfDay
            item.TimeOut = b.DateTime.TimeOfDay
            list.Add(item)
        Next

        Return list
    End Function

End Class
