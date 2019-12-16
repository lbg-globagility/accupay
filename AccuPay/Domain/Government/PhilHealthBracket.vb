Imports System.ComponentModel.DataAnnotations.Schema
Imports System.ComponentModel.DataAnnotations

Namespace Global.PayrollSys

    <Table("payphilhealth")>
    Public Class PhilHealthBracket

        <Key>
        Public Property RowID As Integer?

        Public Property Created As Date

        Public Property CreatedBy As Integer?

        Public Property LastUpd As Date

        Public Property LastUpdBy As Integer?

        Public Property SalaryBracket As Integer

        Public Property SalaryRangeFrom As Decimal

        Public Property SalaryRangeTo As Decimal

        Public Property SalaryBase As Decimal

        Public Property TotalMonthlyPremium As Decimal

        Public Property EmployeeShare As Decimal

        Public Property EmployerShare As Decimal

        Public Property HiddenData As Boolean

    End Class

End Namespace
