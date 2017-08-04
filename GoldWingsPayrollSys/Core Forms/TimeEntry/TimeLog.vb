Option Strict On

Public Class TimeLog

    Public Property RowID As Integer?

    Public Property OrganizationID As Integer?

    Public Property EmployeeID As Integer?

    Public Property LogDate As Date

    Public Property TimeIn As TimeSpan?

    Public Property TimeOut As TimeSpan?

    Public ReadOnly Property HasLogs As Boolean
        Get
            Return TimeIn.HasValue And TimeOut.HasValue
        End Get
    End Property

    Public ReadOnly Property FullTimeIn As Date
        Get
            Return TimeUtility.RangeStart(TimeIn.Value, LogDate)
        End Get
    End Property

    Public ReadOnly Property FullTimeOut As Date
        Get
            Return TimeUtility.RangeEnd(TimeIn.Value, TimeOut.Value, LogDate)
        End Get
    End Property

    Public Sub New(timeIn As String, timeOut As String)
        Me.TimeIn = TimeSpan.Parse(timeIn)
        Me.TimeOut = TimeSpan.Parse(timeOut)
    End Sub

    Public Sub New()
    End Sub

End Class
