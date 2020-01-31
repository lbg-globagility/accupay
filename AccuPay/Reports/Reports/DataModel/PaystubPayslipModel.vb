Imports System.Text
Imports AccuPay.Utilities.Extensions
Imports AccuPay.Entity
Imports AccuPay.Enums
Imports AccuPay.Loans

Public Class PaystubPayslipModel

    Private Const MoneyFormat As String = "#,##0.00"

    Public Property EmployeeId As Integer
    Public Property EmployeeNumber As String
    Public Property EmployeeName As String
    Public Property RegularPay As Decimal

    Public Property BasicHours As Decimal

    Private _basicPay As Decimal

    Public ReadOnly Property BasicPay As Decimal
        Get
            Return _basicPay
        End Get
    End Property

    Public Property Allowance As Decimal
    Public Property Ecola As Decimal
    Public Property AbsentHours As Decimal

    Private _absentAmount As Decimal

    Public Property AbsentAmount As Decimal
        Set(value As Decimal)

            _absentAmount = Negative(value)
        End Set
        Get
            Return _absentAmount
        End Get
    End Property

    Public Property LateAndUndertimeHours As Decimal

    Private _lateAndUndertimeAmount As Decimal

    Public Property LateAndUndertimeAmount As Decimal
        Set(value As Decimal)

            _lateAndUndertimeAmount = Negative(value)
        End Set
        Get
            Return _lateAndUndertimeAmount
        End Get
    End Property

    Public Property GrossPay As Decimal

    Private _SSSAmount As Decimal

    Public Property SSSAmount As Decimal
        Set(value As Decimal)

            _SSSAmount = Negative(value)
        End Set
        Get
            Return _SSSAmount
        End Get
    End Property

    Private _philHealthAmount As Decimal

    Public Property PhilHealthAmount As Decimal
        Set(value As Decimal)

            _philHealthAmount = Negative(value)
        End Set
        Get
            Return _philHealthAmount
        End Get
    End Property

    Private _pagibigAmount As Decimal

    Public Property PagibigAmount As Decimal
        Set(value As Decimal)

            _pagibigAmount = Negative(value)
        End Set
        Get
            Return _pagibigAmount
        End Get
    End Property

    Private _taxWithheldAmount As Decimal

    Public Property TaxWithheldAmount As Decimal
        Set(value As Decimal)

            _taxWithheldAmount = Negative(value)
        End Set
        Get
            Return _taxWithheldAmount
        End Get
    End Property

    Public Property LeaveHours As Decimal
    Public Property LeavePay As Decimal

    Public Property NetPay As Decimal

    Public ReadOnly Property Employee As Employee

    Sub New(employee As Employee)

        Me.Employee = employee

    End Sub

    Private Function Negative(num As Decimal) As Decimal

        If num > 0 Then

            Return num * -1

        End If

        Return num

    End Function

    Public Function ComputeBasicPay(salary As Decimal, workHours As Decimal) As Decimal

        If Employee.IsMonthly OrElse Employee.IsFixed Then

            If Employee.PayFrequencyID.Value = PayFrequencyType.Monthly Then

                Return salary

            ElseIf Employee.PayFrequencyID.Value = PayFrequencyType.SemiMonthly Then

                Return salary / PayrollTools.SemiMonthlyPayPeriodsPerMonth
            Else

                Throw New Exception("GetBasicPay is implemented on monthly and semimonthly only")

            End If

        ElseIf Employee.IsDaily Then

            Return workHours * (salary / PayrollTools.WorkHoursPerDay)

        End If

        Return 0

    End Function

    Public Function CreateSummaries(salary As Decimal, workHours As Decimal) As PaystubPayslipModel

        _basicPay = ComputeBasicPay(salary, workHours)

        Return Me.CreateOvertimeSummaryColumns().
                    CreateLoanSummaryColumns().
                    CreateAdjustmentSummaryColumns()

    End Function

    Public ReadOnly Property TotalDeductions As Decimal
        Get
            Return Negative(SSSAmount + PhilHealthAmount + PagibigAmount + TaxWithheldAmount + TotalLoans)
        End Get
    End Property

#Region "Loans and Adjustments"

    Public Property Loans As List(Of Loan)
    Public Property Adjustments As List(Of Adjustment)

    Public ReadOnly Property TotalLoans As Decimal
        Get
            Return Negative(Loans.Sum(Function(l) l.Amount))
        End Get
    End Property

    Public ReadOnly Property TotalAdjustments As Decimal
        Get
            Return Adjustments.Sum(Function(l) l.Amount)
        End Get
    End Property

    Private _loanNamesSummary As String

    Public ReadOnly Property LoanNamesSummary As String
        Get
            Return _loanNamesSummary
        End Get
    End Property

    Private _loanAmountsSummary As String

    Public ReadOnly Property LoanAmountsSummary As String
        Get
            Return _loanAmountsSummary
        End Get
    End Property

    Private _loanBalancesSummary As String

    Public ReadOnly Property LoanBalancesSummary As String
        Get
            Return _loanBalancesSummary
        End Get
    End Property

    Private _adjustmentNamesSummary As String

    Public ReadOnly Property AdjustmentNamesSummary As String
        Get
            Return _adjustmentNamesSummary
        End Get
    End Property

    Private _adjustmentAmountsSummary As String

    Public ReadOnly Property AdjustmentAmountsSummary As String
        Get
            Return _adjustmentAmountsSummary
        End Get
    End Property

    Public Function CreateLoanSummaryColumns() As PaystubPayslipModel

        _loanNamesSummary = ""
        _loanAmountsSummary = ""
        _loanBalancesSummary = ""

        Dim loanNamesSummaryBuilder As New StringBuilder
        Dim loanAmountsSummaryBuilder As New StringBuilder
        Dim loanBalancesSummaryBuilder As New StringBuilder

        Dim rightSideSummaryMaxCharacters = 15

        For Each loan In Loans

            If loan.Amount <> 0 Then

                loanNamesSummaryBuilder.AppendLine(loan.Name.Ellipsis(rightSideSummaryMaxCharacters))
                loanAmountsSummaryBuilder.AppendLine(loan.Amount.ToString(MoneyFormat))
                loanBalancesSummaryBuilder.AppendLine(loan.Balance.ToString(MoneyFormat))

            End If

        Next

        _loanNamesSummary = loanNamesSummaryBuilder.ToString
        _loanAmountsSummary = loanAmountsSummaryBuilder.ToString
        _loanBalancesSummary = loanBalancesSummaryBuilder.ToString

        Return Me
    End Function

    Public Function CreateAdjustmentSummaryColumns() As PaystubPayslipModel

        _adjustmentNamesSummary = ""
        _adjustmentAmountsSummary = ""

        Dim adjustmentNamesSummaryBuilder As New StringBuilder
        Dim adjustmentAmountsSummaryBuilder As New StringBuilder

        Dim rightSideSummaryMaxCharacters = 25

        For Each adjustment In Adjustments

            If adjustment.Amount <> 0 Then

                adjustmentNamesSummaryBuilder.AppendLine(adjustment.Name.Ellipsis(rightSideSummaryMaxCharacters))
                adjustmentAmountsSummaryBuilder.AppendLine(adjustment.Amount.ToString(MoneyFormat))

            End If

        Next

        _adjustmentNamesSummary = adjustmentNamesSummaryBuilder.ToString
        _adjustmentAmountsSummary = adjustmentAmountsSummaryBuilder.ToString

        Return Me
    End Function

#End Region

#Region "Overtimes Breakdowns and Summary"

    Private _totalOvertimeHours As Decimal

    Public ReadOnly Property TotalOvertimeHours As Decimal
        Get
            Return _totalOvertimeHours
        End Get
    End Property

    Private _totalOvertimePay As Decimal

    Public ReadOnly Property TotalOvertimePay As Decimal
        Get
            Return _totalOvertimePay
        End Get
    End Property

    Private _overtimeNamesSummary As String

    Public ReadOnly Property OvertimeNamesSummary As String
        Get
            Return _overtimeNamesSummary
        End Get
    End Property

    Private _overtimeHoursSummary As String

    Public ReadOnly Property OvertimeHoursSummary As String
        Get
            Return _overtimeHoursSummary
        End Get
    End Property

    Private _overtimeAmountsSummary As String

    Public ReadOnly Property OvertimeAmountsSummary As String
        Get
            Return _overtimeAmountsSummary
        End Get
    End Property

    Public Function CreateOvertimeSummaryColumns() As PaystubPayslipModel

        _totalOvertimeHours = 0
        _totalOvertimePay = 0

        Dim overtimeNamesSummaryBuilder As New StringBuilder
        Dim overtimeHoursSummaryBuilder As New StringBuilder
        Dim overtimeAmountsSummaryBuilder As New StringBuilder

        If OvertimePay <> 0 Then

            overtimeNamesSummaryBuilder.AppendLine("Overtime")
            overtimeHoursSummaryBuilder.AppendLine(OvertimeHours.ToString(MoneyFormat))
            overtimeAmountsSummaryBuilder.AppendLine(OvertimePay.ToString(MoneyFormat))
            _totalOvertimeHours += OvertimeHours
            _totalOvertimePay += OvertimePay
        End If
        If NightDiffPay <> 0 Then

            overtimeNamesSummaryBuilder.AppendLine("Night Diff")
            overtimeHoursSummaryBuilder.AppendLine(NightDiffHours.ToString(MoneyFormat))
            overtimeAmountsSummaryBuilder.AppendLine(NightDiffPay.ToString(MoneyFormat))
            _totalOvertimeHours += NightDiffHours
            _totalOvertimePay += NightDiffPay
        End If
        If NightDiffOvertimePay <> 0 Then

            overtimeNamesSummaryBuilder.AppendLine("Night Diff OT")
            overtimeHoursSummaryBuilder.AppendLine(NightDiffOvertimeHours.ToString(MoneyFormat))
            overtimeAmountsSummaryBuilder.AppendLine(NightDiffOvertimePay.ToString(MoneyFormat))
            _totalOvertimeHours += NightDiffOvertimeHours
            _totalOvertimePay += NightDiffOvertimePay
        End If
        If RestDayPay <> 0 Then

            overtimeNamesSummaryBuilder.AppendLine("Rest Day")
            overtimeHoursSummaryBuilder.AppendLine(RestDayHours.ToString(MoneyFormat))
            overtimeAmountsSummaryBuilder.AppendLine(RestDayPay.ToString(MoneyFormat))
            _totalOvertimeHours += RestDayHours
            _totalOvertimePay += RestDayPay
        End If
        If RestDayOTPay <> 0 Then

            overtimeNamesSummaryBuilder.AppendLine("Rest Day OT")
            overtimeHoursSummaryBuilder.AppendLine(RestDayOTHours.ToString(MoneyFormat))
            overtimeAmountsSummaryBuilder.AppendLine(RestDayOTPay.ToString(MoneyFormat))
            _totalOvertimeHours += RestDayOTHours
            _totalOvertimePay += RestDayOTPay
        End If
        If SpecialHolidayPay <> 0 Then

            overtimeNamesSummaryBuilder.AppendLine("Special Holiday")
            overtimeHoursSummaryBuilder.AppendLine(SpecialHolidayHours.ToString(MoneyFormat))
            overtimeAmountsSummaryBuilder.AppendLine(SpecialHolidayPay.ToString(MoneyFormat))
            _totalOvertimeHours += SpecialHolidayHours
            _totalOvertimePay += SpecialHolidayPay
        End If
        If SpecialHolidayOTPay <> 0 Then

            overtimeNamesSummaryBuilder.AppendLine("Special Holiday OT")
            overtimeHoursSummaryBuilder.AppendLine(SpecialHolidayOTHours.ToString(MoneyFormat))
            overtimeAmountsSummaryBuilder.AppendLine(SpecialHolidayOTPay.ToString(MoneyFormat))
            _totalOvertimeHours += SpecialHolidayOTHours
            _totalOvertimePay += SpecialHolidayOTPay
        End If
        If RegularHolidayPay <> 0 Then

            overtimeNamesSummaryBuilder.AppendLine("Regular Holiday")
            overtimeHoursSummaryBuilder.AppendLine(RegularHolidayHours.ToString(MoneyFormat))
            overtimeAmountsSummaryBuilder.AppendLine(RegularHolidayPay.ToString(MoneyFormat))
            _totalOvertimeHours += RegularHolidayHours
            _totalOvertimePay += RegularHolidayPay
        End If
        If RegularHolidayOTPay <> 0 Then

            overtimeNamesSummaryBuilder.AppendLine("Regular Holiday OT")
            overtimeHoursSummaryBuilder.AppendLine(RegularHolidayOTHours.ToString(MoneyFormat))
            overtimeAmountsSummaryBuilder.AppendLine(RegularHolidayOTPay.ToString(MoneyFormat))
            _totalOvertimeHours += RegularHolidayOTHours
            _totalOvertimePay += RegularHolidayOTPay
        End If
        If RestDayNightDiffPay <> 0 Then

            overtimeNamesSummaryBuilder.AppendLine("Rest Day ND")
            overtimeHoursSummaryBuilder.AppendLine(RestDayNightDiffHours.ToString(MoneyFormat))
            overtimeAmountsSummaryBuilder.AppendLine(RestDayNightDiffPay.ToString(MoneyFormat))
            _totalOvertimeHours += RestDayNightDiffHours
            _totalOvertimePay += RestDayNightDiffPay
        End If
        If RestDayNightDiffOTPay <> 0 Then

            overtimeNamesSummaryBuilder.AppendLine("Rest Day ND OT")
            overtimeHoursSummaryBuilder.AppendLine(RestDayNightDiffOTHours.ToString(MoneyFormat))
            overtimeAmountsSummaryBuilder.AppendLine(RestDayNightDiffOTPay.ToString(MoneyFormat))
            _totalOvertimeHours += RestDayNightDiffOTHours
            _totalOvertimePay += RestDayNightDiffOTPay
        End If
        If SpecialHolidayNightDiffPay <> 0 Then

            overtimeNamesSummaryBuilder.AppendLine("S. Holi ND")
            overtimeHoursSummaryBuilder.AppendLine(SpecialHolidayNightDiffHours.ToString(MoneyFormat))
            overtimeAmountsSummaryBuilder.AppendLine(SpecialHolidayNightDiffPay.ToString(MoneyFormat))
            _totalOvertimeHours += SpecialHolidayNightDiffHours
            _totalOvertimePay += SpecialHolidayNightDiffPay
        End If
        If SpecialHolidayNightDiffOTPay <> 0 Then

            overtimeNamesSummaryBuilder.AppendLine("S. Holi ND OT")
            overtimeHoursSummaryBuilder.AppendLine(SpecialHolidayNightDiffOTHours.ToString(MoneyFormat))
            overtimeAmountsSummaryBuilder.AppendLine(SpecialHolidayNightDiffOTPay.ToString(MoneyFormat))
            _totalOvertimeHours += SpecialHolidayNightDiffOTHours
            _totalOvertimePay += SpecialHolidayNightDiffOTPay
        End If
        If SpecialHolidayRestDayPay <> 0 Then

            overtimeNamesSummaryBuilder.AppendLine("S. Holi RD")
            overtimeHoursSummaryBuilder.AppendLine(SpecialHolidayRestDayHours.ToString(MoneyFormat))
            overtimeAmountsSummaryBuilder.AppendLine(SpecialHolidayRestDayPay.ToString(MoneyFormat))
            _totalOvertimeHours += SpecialHolidayRestDayHours
            _totalOvertimePay += SpecialHolidayRestDayPay
        End If
        If SpecialHolidayRestDayOTPay <> 0 Then

            overtimeNamesSummaryBuilder.AppendLine("S. Holi RD OT")
            overtimeHoursSummaryBuilder.AppendLine(SpecialHolidayRestDayOTHours.ToString(MoneyFormat))
            overtimeAmountsSummaryBuilder.AppendLine(SpecialHolidayRestDayOTPay.ToString(MoneyFormat))
            _totalOvertimeHours += SpecialHolidayRestDayOTHours
            _totalOvertimePay += SpecialHolidayRestDayOTPay
        End If
        If SpecialHolidayRestDayNightDiffPay <> 0 Then

            overtimeNamesSummaryBuilder.AppendLine("S. Holi RD ND")
            overtimeHoursSummaryBuilder.AppendLine(SpecialHolidayRestDayNightDiffHours.ToString(MoneyFormat))
            overtimeAmountsSummaryBuilder.AppendLine(SpecialHolidayRestDayNightDiffPay.ToString(MoneyFormat))
            _totalOvertimeHours += SpecialHolidayRestDayNightDiffHours
            _totalOvertimePay += SpecialHolidayRestDayNightDiffPay
        End If
        If SpecialHolidayRestDayNightDiffOTPay <> 0 Then

            overtimeNamesSummaryBuilder.AppendLine("S. Holi RD ND OT")
            overtimeHoursSummaryBuilder.AppendLine(SpecialHolidayRestDayNightDiffOTHours.ToString(MoneyFormat))
            overtimeAmountsSummaryBuilder.AppendLine(SpecialHolidayRestDayNightDiffOTPay.ToString(MoneyFormat))
            _totalOvertimeHours += SpecialHolidayRestDayNightDiffOTHours
            _totalOvertimePay += SpecialHolidayRestDayNightDiffOTPay
        End If
        If RegularHolidayNightDiffPay <> 0 Then

            overtimeNamesSummaryBuilder.AppendLine("R. Holi. ND")
            overtimeHoursSummaryBuilder.AppendLine(RegularHolidayNightDiffHours.ToString(MoneyFormat))
            overtimeAmountsSummaryBuilder.AppendLine(RegularHolidayNightDiffPay.ToString(MoneyFormat))
            _totalOvertimeHours += RegularHolidayNightDiffHours
            _totalOvertimePay += RegularHolidayNightDiffPay
        End If
        If RegularHolidayNightDiffOTPay <> 0 Then

            overtimeNamesSummaryBuilder.AppendLine("R. Holi. ND OT")
            overtimeHoursSummaryBuilder.AppendLine(RegularHolidayNightDiffOTHours.ToString(MoneyFormat))
            overtimeAmountsSummaryBuilder.AppendLine(RegularHolidayNightDiffOTPay.ToString(MoneyFormat))
            _totalOvertimeHours += RegularHolidayNightDiffOTHours
            _totalOvertimePay += RegularHolidayNightDiffOTPay
        End If
        If RegularHolidayRestDayPay <> 0 Then

            overtimeNamesSummaryBuilder.AppendLine("R. Holi. RD")
            overtimeHoursSummaryBuilder.AppendLine(RegularHolidayRestDayHours.ToString(MoneyFormat))
            overtimeAmountsSummaryBuilder.AppendLine(RegularHolidayRestDayPay.ToString(MoneyFormat))
            _totalOvertimeHours += RegularHolidayRestDayHours
            _totalOvertimePay += RegularHolidayRestDayPay
        End If
        If RegularHolidayRestDayOTPay <> 0 Then

            overtimeNamesSummaryBuilder.AppendLine("R. Holi. RD OT")
            overtimeHoursSummaryBuilder.AppendLine(RegularHolidayRestDayOTHours.ToString(MoneyFormat))
            overtimeAmountsSummaryBuilder.AppendLine(RegularHolidayRestDayOTPay.ToString(MoneyFormat))
            _totalOvertimeHours += RegularHolidayRestDayOTHours
            _totalOvertimePay += RegularHolidayRestDayOTPay
        End If
        If RegularHolidayRestDayNightDiffPay <> 0 Then

            overtimeNamesSummaryBuilder.AppendLine("R. Holi. RD ND")
            overtimeHoursSummaryBuilder.AppendLine(RegularHolidayRestDayNightDiffHours.ToString(MoneyFormat))
            overtimeAmountsSummaryBuilder.AppendLine(RegularHolidayRestDayNightDiffPay.ToString(MoneyFormat))
            _totalOvertimeHours += RegularHolidayRestDayNightDiffHours
            _totalOvertimePay += RegularHolidayRestDayNightDiffPay
        End If
        If RegularHolidayRestDayNightDiffOTPay <> 0 Then

            overtimeNamesSummaryBuilder.AppendLine("R. Holi. RD ND OT")
            overtimeHoursSummaryBuilder.AppendLine(RegularHolidayRestDayNightDiffOTHours.ToString(MoneyFormat))
            overtimeAmountsSummaryBuilder.AppendLine(RegularHolidayRestDayNightDiffOTPay.ToString(MoneyFormat))
            _totalOvertimeHours += RegularHolidayRestDayNightDiffOTHours
            _totalOvertimePay += RegularHolidayRestDayNightDiffOTPay
        End If

        _overtimeNamesSummary = overtimeNamesSummaryBuilder.ToString
        _overtimeHoursSummary = overtimeHoursSummaryBuilder.ToString
        _overtimeAmountsSummary = overtimeAmountsSummaryBuilder.ToString

        Return Me
    End Function

    Public Property OvertimeHours As Decimal
    Public Property OvertimePay As Decimal
    Public Property NightDiffHours As Decimal
    Public Property NightDiffPay As Decimal
    Public Property NightDiffOvertimeHours As Decimal
    Public Property NightDiffOvertimePay As Decimal
    Public Property RestDayHours As Decimal
    Public Property RestDayPay As Decimal
    Public Property RestDayOTHours As Decimal
    Public Property RestDayOTPay As Decimal
    Public Property SpecialHolidayHours As Decimal
    Public Property SpecialHolidayPay As Decimal
    Public Property SpecialHolidayOTHours As Decimal
    Public Property SpecialHolidayOTPay As Decimal
    Public Property RegularHolidayHours As Decimal
    Public Property RegularHolidayPay As Decimal
    Public Property RegularHolidayOTHours As Decimal
    Public Property RegularHolidayOTPay As Decimal
    Public Property RestDayNightDiffHours As Decimal
    Public Property RestDayNightDiffPay As Decimal
    Public Property RestDayNightDiffOTHours As Decimal
    Public Property RestDayNightDiffOTPay As Decimal
    Public Property SpecialHolidayNightDiffHours As Decimal
    Public Property SpecialHolidayNightDiffPay As Decimal
    Public Property SpecialHolidayNightDiffOTHours As Decimal
    Public Property SpecialHolidayNightDiffOTPay As Decimal
    Public Property SpecialHolidayRestDayHours As Decimal
    Public Property SpecialHolidayRestDayPay As Decimal
    Public Property SpecialHolidayRestDayOTHours As Decimal
    Public Property SpecialHolidayRestDayOTPay As Decimal
    Public Property SpecialHolidayRestDayNightDiffHours As Decimal
    Public Property SpecialHolidayRestDayNightDiffPay As Decimal
    Public Property SpecialHolidayRestDayNightDiffOTHours As Decimal
    Public Property SpecialHolidayRestDayNightDiffOTPay As Decimal
    Public Property RegularHolidayNightDiffHours As Decimal
    Public Property RegularHolidayNightDiffPay As Decimal
    Public Property RegularHolidayNightDiffOTHours As Decimal
    Public Property RegularHolidayNightDiffOTPay As Decimal
    Public Property RegularHolidayRestDayHours As Decimal
    Public Property RegularHolidayRestDayPay As Decimal
    Public Property RegularHolidayRestDayOTHours As Decimal
    Public Property RegularHolidayRestDayOTPay As Decimal
    Public Property RegularHolidayRestDayNightDiffHours As Decimal
    Public Property RegularHolidayRestDayNightDiffPay As Decimal
    Public Property RegularHolidayRestDayNightDiffOTHours As Decimal
    Public Property RegularHolidayRestDayNightDiffOTPay As Decimal

#End Region

    Public Class Overtime

        Public Property Name As String
        Public Property Hours As Decimal
        Public Property Amount As Decimal

    End Class

    Public Class Loan

        Public Property Name As String

        Public Property Amount As Decimal

        Public Property Balance As Decimal

        Sub New(loan As LoanTransaction)

            Me.New(loan.LoanSchedule?.LoanType?.PartNo, loan.Amount, loan.TotalBalance)

        End Sub

        Sub New(name As String, amount As Decimal, balance As Decimal)

            Me.Name = name

            Me.Amount = amount

            Me.Balance = balance

        End Sub

    End Class

    Public Class Adjustment

        Public Property Name As String

        Public Property Amount As Decimal

        Sub New(adjustment As IAdjustment)

            Me.New(adjustment.Product?.PartNo, adjustment.PayAmount)

        End Sub

        Sub New(name As String, amount As Decimal)

            Me.Name = name

            Me.Amount = amount

        End Sub

    End Class

End Class