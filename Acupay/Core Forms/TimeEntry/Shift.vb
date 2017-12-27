Option Strict On

Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace Global.AccuPay.Entity

    <Table("shift")>
    Public Class Shift

        <Key>
        Public Property RowID As Integer?

        Public Property OrganizationID As Integer?

        Public Property Created As Date

        Public Property CreatedBy As Integer?

        Public Property LastUpd As Date?

        Public Property LastUpdBy As Integer?

        Public Property TimeFrom As TimeSpan?

        Public Property TimeTo As TimeSpan?

        Public Property BreaktimeFrom As TimeSpan?

        Public Property BreaktimeTo As TimeSpan?

        Public Property DivisorToDailyRate As Decimal

        Public ReadOnly Property HasBreaktime As Boolean
            Get
                Return BreaktimeFrom.HasValue And BreaktimeTo.HasValue
            End Get
        End Property

    End Class

End Namespace
