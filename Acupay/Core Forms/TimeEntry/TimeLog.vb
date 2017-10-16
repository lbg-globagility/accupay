Option Strict On

Imports System
Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

<Table("employeetimeentrydetails")>
Public Class TimeLog

    <Key>
    Public Property RowID As Integer?

    Public Property OrganizationID As Integer?

    Public Property EmployeeID As Integer?

    <Column("Date")>
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
            Return TimeUtility.RangeStart(LogDate, TimeIn.Value)
        End Get
    End Property

    Public ReadOnly Property FullTimeOut As Date
        Get
            Return TimeUtility.RangeEnd(LogDate, TimeIn.Value, TimeOut.Value)
        End Get
    End Property

    Public Sub New()
    End Sub

    Public Sub New(timeIn As String, timeOut As String)
        Me.TimeIn = TimeSpan.Parse(timeIn)
        Me.TimeOut = TimeSpan.Parse(timeOut)
    End Sub

    <ForeignKey("EmployeeID")>
    Public Overridable Property Employee As Employee

End Class
