Option Strict On

Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace Global.AccuPay.Entity

    <Table("employeeleave")>
    Public Class Leave

        <Key>
        Public Property RowID As Integer?

        Public Property OrganizationID As Integer?

        Public Property Created As DateTime?

        Public Property CreatedBy As Integer?

        Public Property LastUpd As DateTime?

        Public Property LastUpdBy As Integer?

        Public Property EmployeeID As Integer?

        Public Property LeaveType As String

        Public Property LeaveStartDate As Date

        Public Property LeaveEndDate As Date

        Public Property LeaveStartTime As TimeSpan?

        Public Property LeaveEndTime As TimeSpan?

        Public Property Reason As String

        Public Property Comments As String

        Public Property Image As Byte()

        Public Property Status As String

    End Class

End Namespace