Option Strict On

Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace Global.AccuPay.Entity

    <Table("organization")>
    Public Class Organization

        <Key>
        <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
        Public Property RowID As Integer?

        <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
        Public Property Created As Date

        Public Property CreatedBy As Integer?

        <DatabaseGenerated(DatabaseGeneratedOption.Computed)>
        Public Property LastUpd As Date?

        Public Property LastUpdBy As Integer?

        Public Property Name As String

        Public Property NightDifferentialTimeFrom As TimeSpan

        Public Property NightDifferentialTimeTo As TimeSpan

    End Class

End Namespace
