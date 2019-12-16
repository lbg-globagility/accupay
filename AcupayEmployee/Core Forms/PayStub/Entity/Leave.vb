Option Strict On

Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace Global.AccuPay.Entity

    <Table("employeeleave")>
    Public Class Leave

        <Key>
        <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
        Public Property RowID As Integer?

        Public Property OrganizationID As Integer?

        <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
        Public Property Created As DateTime

        Public Property CreatedBy As Integer?

        <DatabaseGenerated(DatabaseGeneratedOption.Computed)>
        Public Property LastUpd As DateTime?

        Public Property LastUpdBy As Integer?

        Public Property EmployeeID As Integer?

        Public Property LeaveType As String

        Public Property LeaveHours As Decimal

        <Column("LeaveStartTime")>
        Public Property StartTime As TimeSpan?

        <Column("LeaveEndTime")>
        Public Property EndTime As TimeSpan?

        <Column("LeaveStartDate")>
        Public Property StartDate As Date

        <Column("LeaveEndDate")>
        Public Property EndDate As Date?

        Public Property Reason As String

        Public Property Comments As String

        Public Property Image As Byte()

        Public Property Status As String

        Public ReadOnly Property IsPaid As Boolean
            Get
                Return LeaveType <> "Leave w/o Pay"
            End Get
        End Property

    End Class

End Namespace