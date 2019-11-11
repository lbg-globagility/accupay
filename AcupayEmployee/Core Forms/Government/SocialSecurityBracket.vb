Imports System.ComponentModel.DataAnnotations.Schema
Imports System.ComponentModel.DataAnnotations

Namespace Global.PayrollSys

    <Table("paysocialsecurity")>
    Public Class SocialSecurityBracket

        <Key>
        Public Property RowID As Integer?

        Public Property Created As Date

        Public Property CreatedBy As Integer?

        Public Property LastUpd As Date

        Public Property LastUpdBy As Integer?

        Public Property RangeFromAmount As Decimal

        Public Property RangeToAmount As Decimal

        Public Property MonthlySalaryCredit As Decimal

        Public Property EmployeeContributionAmount As Decimal

        Public Property EmployerContributionAmount As Decimal

        Public Property EmployeeECAmount As Decimal

        Public Property HiddenData As Boolean

    End Class

End Namespace
