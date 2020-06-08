Option Strict On

Imports AccuPay.Data.Entities
Imports AccuPay.Infrastructure.Services.Excel

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

    <Ignore>
    Public Property LoanType As Product

    Public Function ToLoan(employeeId As Integer) As LoanSchedule

        Dim newLoanSchedule = New LoanSchedule With {
            .RowID = Nothing,
            .OrganizationID = z_OrganizationID,
            .CreatedBy = z_User,
            .EmployeeID = employeeId,
            .LoanNumber = LoanNumber,
            .Comments = Comments,
            .TotalLoanAmount = TotalLoanAmount.Value,
            .TotalBalanceLeft = TotalBalanceLeft.Value,
            .DedEffectiveDateFrom = StartDate.Value,
            .DeductionAmount = DeductionAmount.Value,
            .DeductionPercentage = 0,
            .LoanName = LoanName,
            .LoanTypeID = LoanType.RowID,
            .Status = LoanSchedule.STATUS_IN_PROGRESS,
            .DeductionSchedule = DeductionSchedule
        }

        newLoanSchedule.RecomputeTotalPayPeriod()
        newLoanSchedule.RecomputePayPeriodLeft()

        Return newLoanSchedule

    End Function

End Class