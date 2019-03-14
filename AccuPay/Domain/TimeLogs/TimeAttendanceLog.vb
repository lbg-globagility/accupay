Option Strict On
Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace Global.AccuPay.Entity

    <Table("employeetimeattendancelog")>
    Public Class TimeAttendanceLog

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

        Public Property ImportNumber As String

        Public Property TimeStamp As Date

        Public Property IsTimeIn As Boolean?

        Public Property WorkDay As Date

        Public Property EmployeeID As Integer

        <ForeignKey("EmployeeID")>
        Public Overridable Property Employee As Employee

        Public ReadOnly Property IsTimeInDescription() As String
            Get
                If IsTimeIn Is Nothing Then Return ""

                Return If(IsTimeIn = True, "IN", "OUT")
            End Get
        End Property

    End Class


End Namespace