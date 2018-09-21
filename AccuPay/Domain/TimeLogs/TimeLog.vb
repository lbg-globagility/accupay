Option Strict On

Imports System
Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema
Imports AccuPay.Entity

<Table("employeetimeentrydetails")>
Public Class TimeLog

    <Key>
    <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
    Public Property RowID As Integer?

    Public Property OrganizationID As Integer?

    Public Property Created As Date

    Public Property CreatedBy As Integer?

    <DatabaseGenerated(DatabaseGeneratedOption.Computed)>
    Public Property LastUpd As Date?

    Public Property LastUpdBy As Date?

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

    Public Sub New()
    End Sub

    Public Sub New(timeIn As String, timeOut As String)
        Me.TimeIn = TimeSpan.Parse(timeIn)
        Me.TimeOut = TimeSpan.Parse(timeOut)
    End Sub

    <ForeignKey("EmployeeID")>
    Public Overridable Property Employee As Employee

End Class
