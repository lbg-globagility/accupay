Option Strict On

Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace Global.AccuPay.Entity

    <Table("payperiod")>
    Public Class PayPeriod

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

        <Column("TotalGrossSalary")>
        Public Property PayFrequencyID As Integer?

        Public Property PayFromDate As Date

        Public Property PayToDate As Date

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

    End Class

End Namespace