Option Strict On

Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema
Imports AccuPay.Data

Namespace Global.AccuPay.Entity

    <Table("payperiod")>
    Public Class PayPeriod
        Implements IPayPeriod

        Private Const IsJanuary As Integer = 1
        Private Const IsDecember As Integer = 12

        <Key>
        <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
        Public Property RowID As Integer? Implements IPayPeriod.RowID

        Public Property OrganizationID As Integer?

        <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
        Public Property Created As Date

        Public Property CreatedBy As Integer?

        <DatabaseGenerated(DatabaseGeneratedOption.Computed)>
        Public Property LastUpd As Date?

        Public Property LastUpdBy As Integer?

        <Column("TotalGrossSalary")>
        Public Property PayFrequencyID As Integer?

        Public Property PayFromDate As Date Implements IPayPeriod.PayFromDate

        Public Property PayToDate As Date Implements IPayPeriod.PayToDate

        Public Property Month As Integer

        Public Property Year As Integer

        Public Property Half As Integer

        Public Property OrdinalValue As Integer

        Public Property SSSWeeklyContribSched As Boolean
        Public Property PhHWeeklyContribSched As Boolean
        Public Property HDMFWeeklyContribSched As Boolean
        Public Property WTaxWeeklyContribSched As Boolean

        Public Property SSSWeeklyAgentContribSched As Boolean
        Public Property PhHWeeklyAgentContribSched As Boolean
        Public Property HDMFWeeklyAgentContribSched As Boolean
        Public Property WTaxWeeklyAgentContribSched As Boolean
        Public Property IsClosed As Boolean

        Public ReadOnly Property IsSemiMonthly As Boolean
            Get
                Return PayFrequencyID.Value = Data.Helpers.PayrollTools.PayFrequencySemiMonthlyId
            End Get
        End Property

        Public ReadOnly Property IsWeekly As Boolean
            Get
                Return PayFrequencyID.Value = Data.Helpers.PayrollTools.PayFrequencyWeeklyId
            End Get
        End Property

        Public ReadOnly Property IsFirstHalf As Boolean
            Get
                Return Half = 1
            End Get
        End Property

        Public ReadOnly Property IsEndOfTheMonth As Boolean
            Get
                Return Half = 0
            End Get
        End Property

        Public ReadOnly Property IsBetween([date] As Date) As Boolean
            Get
                Return [date] >= PayFromDate AndAlso [date] <= PayToDate
            End Get
        End Property

        Public ReadOnly Property IsFirstPayPeriodOfTheYear As Boolean
            Get
                Return IsFirstHalf AndAlso Month = IsJanuary
            End Get
        End Property

        Public ReadOnly Property IsLastPayPeriodOfTheYear As Boolean
            Get
                Return IsEndOfTheMonth AndAlso Month = IsDecember
            End Get
        End Property

    End Class

End Namespace