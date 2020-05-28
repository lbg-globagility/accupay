Option Strict On

Imports AccuPay.Infrastructure.Services.Excel

Namespace Global.Globagility.AccuPay.Loans

    Public Class AllowanceRowRecord
        Implements IExcelRowRecord

        <ColumnName("EmployeeID")>
        Public Property EmployeeID As String

        <ColumnName("Name of allowance")>
        Public Property Type As String

        <ColumnName("Effective start date")>
        Public Property EffectiveStartDate As Date?

        <ColumnName("Effective end date")>
        Public Property EffectiveEndDate As Date?

        <ColumnName("Allowance frequency(Daily, Semi-monthly)")>
        Public Property AllowanceFrequency As String

        <ColumnName("Allowance amount")>
        Public Property Amount As Decimal?

        Public Property EmployeeFullName As String

        <Ignore>
        Public Property ErrorMessage As String

        <Ignore>
        Public Property LineNumber As Integer Implements IExcelRowRecord.LineNumber

    End Class

End Namespace