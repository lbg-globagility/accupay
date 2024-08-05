Option Strict On

Imports AccuPay.Core.Interfaces.Excel
Imports AccuPay.Utilities.Attributes

Namespace Global.Globagility.AccuPay.Salaries

    Public Class SalaryRowRecord
        Implements IExcelRowRecord

        <ColumnName("Employee No")>
        Public Property EmployeeNo As String

        <ColumnName("Effective From")>
        Public Property EffectiveFrom As Date?

        <ColumnName("Basic Salary")>
        Public Property BasicSalary As Decimal?

        <ColumnName("Allowance Salary")>
        Public Property AllowanceSalary As Decimal

        <ColumnName("Philhealth (Auto)")>
        Public Property PhilhealthAuto As String

        <ColumnName("Philhealth")>
        Public Property Philhealth As Decimal

        <ColumnName("SSS (Auto)")>
        Public Property SSSAuto As String

        <ColumnName("HDMF (Auto)")>
        Public Property HDMF_Auto As String

        <ColumnName("HDMF")>
        Public Property HDMF As Decimal

        <Ignore>
        Public Property ErrorMessage As String

        <Ignore>
        Public Property LineNumber As Integer Implements IExcelRowRecord.LineNumber

    End Class

End Namespace
