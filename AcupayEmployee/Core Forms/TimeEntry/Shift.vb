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

        Public Property TimeFrom As TimeSpan

        Public Property TimeTo As TimeSpan

        Public Property BreaktimeFrom As TimeSpan?

        Public Property BreaktimeTo As TimeSpan?

        Public Property DivisorToDailyRate As Decimal

        Public ReadOnly Property HasBreaktime As Boolean
            Get
                Return BreaktimeFrom.HasValue And BreaktimeTo.HasValue
            End Get
        End Property

        Public Sub New()
        End Sub

        Public Sub New(timeFrom As TimeSpan, timeTo As TimeSpan)
            Me.TimeFrom = timeFrom
            Me.TimeTo = timeTo
        End Sub

        Public Sub New(timeFrom As TimeSpan, timeTo As TimeSpan, breaktimeFrom As TimeSpan, breaktimeTo As TimeSpan)
            Me.New(timeFrom, timeTo)
            Me.BreaktimeFrom = breaktimeFrom
            Me.BreaktimeTo = breaktimeTo
        End Sub

    End Class

End Namespace
