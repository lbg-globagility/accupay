Option Strict On

Imports AccuPay.Core.Entities
Imports AccuPay.Core.Interfaces.Excel
Imports AccuPay.Utilities.Attributes

Public Class AllowanceRowRecord
    Inherits ExcelEmployeeRowRecord
    Implements IExcelRowRecord

    <ColumnName("EmployeeID")>
    Public Property EmployeeNumber As String

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

    Public Property Remarks As String

    Public Property EmployeeFullName As String

    <Ignore>
    Public Property ErrorMessage As String

    <Ignore>
    Public Property LineNumber As Integer Implements IExcelRowRecord.LineNumber

    <Ignore>
    Public Property AllowanceType As Product

    Friend Function ToAllowance(employeeId As Integer, organizationId As Integer) As Allowance

        If EffectiveStartDate Is Nothing Then Return Nothing

        Return New Allowance With {
            .RowID = Nothing,
            .OrganizationID = organizationId,
            .EmployeeID = employeeId,
            .AllowanceFrequency = AllowanceFrequency,
            .Amount = Amount.Value,
            .EffectiveStartDate = EffectiveStartDate.Value,
            .EffectiveEndDate = EffectiveEndDate,
            .ProductID = AllowanceType.RowID,
            .Remarks = Remarks
        }

    End Function

End Class
