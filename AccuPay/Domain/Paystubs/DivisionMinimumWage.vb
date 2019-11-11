Option Strict On

Imports System.ComponentModel.DataAnnotations
Imports System.ComponentModel.DataAnnotations.Schema

Namespace Global.AccuPay.Entity

    <Table("divisionminimumwage")>
    Public Class DivisionMinimumWage

        <Key>
        <DatabaseGenerated(DatabaseGeneratedOption.Identity)>
        Public Property RowID As Integer?

        Public Property OrganizationID As Integer?

        Public Property DivisionID As Integer?

        Public Property Amount As Decimal

        Public Property EffectiveDateFrom As Date

        Public Property EffectiveDateTo As Date

    End Class

End Namespace
