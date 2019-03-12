Option Strict On

Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace Global.AccuPay.Entity

    <Table("breaktimebracket")>
    Public Class BreakTimeBracket

        <Key>
        <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
        Public Property RowID As Integer

        <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
        Public Property Created As Date

        Public Property CreatedBy As Integer?

        <DatabaseGenerated(DatabaseGeneratedOption.Computed)>
        Public Property LastUpd As Date?

        Public Property LastUpdBy As Integer?

        Public Property ShiftDuration As Decimal

        Public Property BreakDuration As Decimal

        Public Property DivisionID As Integer

        <ForeignKey("DivisionID")>
        Public Overridable Property Division As Division

    End Class

End Namespace