Option Strict On

Imports System
Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema
Imports AccuPay.Entity

<Table("employeetimeentrydetails")>
Public Class TimeLogInOut

    <Key>
    Public Property RowID As Integer?

    Public Property OrganizationID As Integer?

    Public Property EmployeeID As Integer?

    <Column("Date")>
    Public Property LogDate As Date

    Public Property DateIn As Date?

    Public Property DateOut As Date?

    Public Property TimeIn As TimeSpan?

    Public Property TimeOut As TimeSpan?

    Public ReadOnly Property HasLogs As Boolean
        Get
            Return TimeIn.HasValue And TimeOut.HasValue
        End Get
    End Property

    Public ReadOnly Property FullTimeIn As Date?
        Get
            Return Combine(DateIn.Value, TimeIn.Value)
        End Get
    End Property

    Public ReadOnly Property FullTimeOut As Date?
        Get
            Return Combine(DateOut.Value, TimeOut.Value)
        End Get
    End Property





    <ForeignKey("EmployeeID")>
    Public Overridable Property Employee As Employee


    Public Function Combine(day As Date, time As TimeSpan) As Date
        Dim timestampString = $"{day.ToString("yyyy-MM-dd")} {time.ToString("hh\:mm\:ss")}"
        If IsDBNull(day) And IsDBNull(time) Then
            Return Nothing
        Else
            Return Date.Parse(timestampString)
        End If

    End Function
End Class
