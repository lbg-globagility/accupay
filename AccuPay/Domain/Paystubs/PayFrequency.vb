Option Strict On

Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace Global.AccuPay.Entity

    <Table("payfrequency")>
    Public Class PayFrequency

        Public Const WeeklyType As String = "WEEKLY"

        Public Const SemiMonthlyType As String = "SEMI-MONTHLY"

        <Key>
        <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
        Public Property RowID As Integer?

        <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
        Public Property Created As Date

        Public Property CreatedBy As Integer?

        <DatabaseGenerated(DatabaseGeneratedOption.Computed)>
        Public Property LastUpd As Date?

        Public Property LastUpdBy As Integer?

        <Column("PayFrequencyType")>
        Public Property Type As String

        Public ReadOnly Property IsWeekly As Boolean
            Get
                Return Type.ToUpper() = WeeklyType
            End Get
        End Property

        Public ReadOnly Property IsSemiMonthly As Boolean
            Get
                Return Type.ToUpper() = SemiMonthlyType
            End Get
        End Property

    End Class

End Namespace
