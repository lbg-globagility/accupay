Option Strict On

Imports AccuPay.Core.Entities
Imports AccuPay.Core.Helpers
Imports AccuPay.Core.Interfaces.Excel
Imports AccuPay.Utilities.Attributes

Public Class GovernmentPremiumRowRecord
    Inherits ExcelEmployeeRowRecord
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

    'SSS
    <ColumnName("SSS Employee Share")>
    Public Property SssEmployeeShare As Decimal?

    <ColumnName("SSS Employer Share")>
    Public Property SssEmployerShare As Decimal?

    <ColumnName("SSS EC Employer Share")>
    Public Property SssEcEmployerShare As Decimal?

    <ColumnName("SSS WISP Employee Share")>
    Public Property SssWispEmployeeShare As Decimal?

    <ColumnName("SSS WISP Employer Share")>
    Public Property SssWispEmployerShare As Decimal?

    'PhilHealth
    <ColumnName("PhilHealth Employee Share")>
    Public Property PhilHealthEmployeeShare As Decimal?

    <ColumnName("PhilHealth Employer Share")>
    Public Property PhilHealthEmployerShare As Decimal?

    'HDMF
    <ColumnName("HDMF Employee Share")>
    Public Property HdmfEmployeeShare As Decimal?

    <ColumnName("HDMF Employer Share")>
    Public Property HdmfEmployerShare As Decimal?

    <Ignore>
    Public ReadOnly Property SssTotal As Decimal
        Get
            Return If(SssEmployeeShare, 0) +
                If(SssEmployerShare, 0) +
                If(SssEcEmployerShare, 0) +
                If(SssWispEmployeeShare, 0) +
                If(SssWispEmployerShare, 0)
        End Get
    End Property

    <Ignore>
    Public ReadOnly Property PhilHealthTotal As Decimal
        Get
            Return If(PhilHealthEmployeeShare, 0) + If(PhilHealthEmployerShare, 0)
        End Get
    End Property

    <Ignore>
    Public ReadOnly Property HdmfTotal As Decimal
        Get
            Return If(HdmfEmployeeShare, 0) + If(HdmfEmployerShare, 0)
        End Get
    End Property

    Public Function ToLoans(employee As Employee,
        loanTypes As ICollection(Of Product),
        startDate As Date,
        payPeriods As List(Of PayPeriod)) As List(Of Loan)

        Dim employeeId = employee.RowID.Value
        Dim organizationId = employee.OrganizationID.Value

        Dim thisLoanTypes = loanTypes.Where(Function(l) l.OrganizationID.Value = organizationId)

        Const divisorForWeekly As Integer = 4

        Dim counter = Enumerable.Range(1, divisorForWeekly)

        Dim loanList = New List(Of Loan)

        Dim bothSssShareHasValue = If(SssEmployeeShare, 0) > 0 AndAlso If(SssEmployerShare, 0) > 0

        If bothSssShareHasValue AndAlso SssTotal > 0 Then
            'If(SssEcEmployerShare, 0) > 0 AndAlso
            'If(SssWispEmployeeShare, 0) > 0 AndAlso
            'If(SssWispEmployerShare, 0) > 0) AndAlso

            'SSS
            Dim sssLoanType = thisLoanTypes.FirstOrDefault(Function(l) l.IsSssLoanOfMorningSun)
            For Each count In counter
                Dim index = count - 1
                Dim deductionAmount = SssEmployeeShare.Value / divisorForWeekly
                Dim sssLoan = New Loan With {
                    .RowID = Nothing,
                    .OrganizationID = organizationId,
                    .EmployeeID = employeeId,
                    .TotalLoanAmount = deductionAmount,
                    .TotalBalanceLeft = deductionAmount,
                    .DedEffectiveDateFrom = payPeriods(index).PayFromDate,
                    .DeductionAmount = deductionAmount,
                    .DeductionPercentage = 0,
                    .LoanName = sssLoanType.PartNo,
                    .LoanTypeID = sssLoanType.RowID,
                    .Status = Loan.STATUS_IN_PROGRESS,
                    .DeductionSchedule = ContributionSchedule.PER_PAY_PERIOD,
                    .SssEmployeeShare = SssEmployeeShare,
                    .SssEmployerShare = SssEmployerShare,
                    .SssEcEmployerShare = SssEcEmployerShare,
                    .SssWispEmployeeShare = SssWispEmployeeShare,
                    .SssWispEmployerShare = SssWispEmployerShare
                }
                sssLoan.RecomputeTotalPayPeriod()
                sssLoan.RecomputePayPeriodLeft()

                loanList.Add(sssLoan)
            Next
        End If

        Dim bothPhilHealthShareHasValue = If(PhilHealthEmployeeShare, 0) > 0 AndAlso If(PhilHealthEmployerShare, 0) > 0

        If bothPhilHealthShareHasValue AndAlso PhilHealthTotal > 0 Then
            'PhilHealth
            Dim philHealthLoanType = thisLoanTypes.FirstOrDefault(Function(l) l.IsPhilHealthLoanOfMorningSun)
            For Each count In counter
                Dim index = count - 1
                Dim deductionAmount = PhilHealthEmployeeShare.Value / divisorForWeekly
                Dim philHealthLoan = New Loan With {
                    .RowID = Nothing,
                    .OrganizationID = organizationId,
                    .EmployeeID = employeeId,
                    .TotalLoanAmount = deductionAmount,
                    .TotalBalanceLeft = deductionAmount,
                    .DedEffectiveDateFrom = payPeriods(index).PayFromDate,
                    .DeductionAmount = deductionAmount,
                    .DeductionPercentage = 0,
                    .LoanName = philHealthLoanType.PartNo,
                    .LoanTypeID = philHealthLoanType.RowID,
                    .Status = Loan.STATUS_IN_PROGRESS,
                    .DeductionSchedule = ContributionSchedule.PER_PAY_PERIOD,
                    .PhilHealthEmployeeShare = PhilHealthEmployeeShare,
                    .PhilHealthEmployerShare = PhilHealthEmployerShare
                }
                philHealthLoan.RecomputeTotalPayPeriod()
                philHealthLoan.RecomputePayPeriodLeft()

                loanList.Add(philHealthLoan)
            Next

        End If

        Dim bothHdmfShareHasValue = If(HdmfEmployeeShare, 0) > 0 AndAlso If(HdmfEmployerShare, 0) > 0

        If bothHdmfShareHasValue AndAlso HdmfTotal > 0 Then
            'HDMF
            Dim hdmfLoanType = thisLoanTypes.FirstOrDefault(Function(l) l.IsHDMFLoanOfMorningSun)
            For Each count In counter
                Dim index = count - 1
                Dim deductionAmount = HdmfEmployeeShare.Value / divisorForWeekly
                Dim hdmfLoan = New Loan With {
                    .RowID = Nothing,
                    .OrganizationID = organizationId,
                    .EmployeeID = employeeId,
                    .TotalLoanAmount = deductionAmount,
                    .TotalBalanceLeft = deductionAmount,
                    .DedEffectiveDateFrom = payPeriods(index).PayFromDate,
                    .DeductionAmount = deductionAmount,
                    .DeductionPercentage = 0,
                    .LoanName = hdmfLoanType.PartNo,
                    .LoanTypeID = hdmfLoanType.RowID,
                    .Status = Loan.STATUS_IN_PROGRESS,
                    .DeductionSchedule = ContributionSchedule.PER_PAY_PERIOD,
                    .HdmfEmployeeShare = HdmfEmployeeShare,
                    .HdmfEmployerShare = HdmfEmployerShare
                }
                hdmfLoan.RecomputeTotalPayPeriod()
                hdmfLoan.RecomputePayPeriodLeft()

                loanList.Add(hdmfLoan)
            Next

        End If

        Return loanList
    End Function

End Class
