Option Strict On
Imports AccuPay.Attributes

Namespace Global.Globagility.AccuPay.Salaries

    Public Class SalaryRowRecord

        <ColumnName("Employee No")>
        Public Property EmployeeNo As String

        <ColumnName("Effective From")>
        Public Property EffectiveFrom As Date

        <ColumnName("Effective To")>
        Public Property EffectiveTo As Date?

        <ColumnName("Basic Salary")>
        Public Property BasicSalary As Decimal

        <ColumnName("Allowance Salary")>
        Public Property AllowanceSalary As Decimal

        <Ignore>
        Public Property ErrorMessage As String

        <Ignore>
        Public Property LineNumber As Integer

    End Class

End Namespace
