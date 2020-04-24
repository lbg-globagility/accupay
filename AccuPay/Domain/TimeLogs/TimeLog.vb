Option Strict On

Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace Global.AccuPay.Entity

    <Table("employeetimeentrydetails")>
    Public Class TimeLog

        <Key>
        <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
        Public Property RowID As Integer?

        Public Property OrganizationID As Integer?

        <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
        Public Property Created As Date

        Public Property CreatedBy As Integer?

        <DatabaseGenerated(DatabaseGeneratedOption.Computed)>
        Public Property LastUpd As Date?

        Public Property LastUpdBy As Integer?

        Public Property EmployeeID As Integer?

        <Column("Date")>
        Public Property LogDate As Date

        Public Property TimeIn As TimeSpan?

        Public Property TimeOut As TimeSpan?

        Public Property TimeStampIn As Date?

        Public Property TimeStampOut As Date?

        Public Property TimeentrylogsImportID As String
        Public Property BranchID As Integer?

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

End Namespace