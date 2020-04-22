Option Strict On

Imports AccuPay
Imports AccuPay.Attributes

Namespace Global.Globagility.AccuPay.Loans

    Public Class LoanRowRecord
        Implements IExcelRowRecord

        <ColumnName("Employee Name")>
        Public Property EmployeeFullName As String

        <ColumnName("Employee ID")>
        Public Property EmployeeNumber As String

        Public Property LoanName As String

        <ColumnName("Loan number/code")>
        Public Property LoanNumber As String

        <ColumnName("Start date")>
        Public Property StartDate As Date?

        Public Property TotalLoanAmount As Decimal?

        <ColumnName("Loan balance")>
        Public Property TotalBalanceLeft As Decimal?

        Public Property DeductionAmount As Decimal?

        <ColumnName("Deduction frequency(First half, End of the month, Per pay period)")>
        Public Property DeductionSchedule As String

        Public Property Comments As String

        <Ignore>
        Public Property ErrorMessage As String

        <Ignore>
        Public Property LineNumber As Integer Implements IExcelRowRecord.LineNumber

    End Class

End Namespace