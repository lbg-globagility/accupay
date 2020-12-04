Imports AccuPay.Data.Entities
Imports AccuPay.Data.ValueObjects

Public Class LoanPaymentFromBonusModel
    Public ReadOnly Property Id As Integer
    Public ReadOnly Property LoanId As Integer
    Public ReadOnly Property BonusId As Integer
    Public ReadOnly Property BonusAmount As Decimal
    Public ReadOnly Property EffectiveStartDate As Date
    Public ReadOnly Property EffectiveEndDate As Date
    Public ReadOnly Property Frequency As String
    Public ReadOnly Property IsEditable As Boolean
    Public ReadOnly Property IsNew As Boolean
    Public ReadOnly Property DeductionAmount As Decimal
    Public ReadOnly Property TotalPayment As Decimal
    Public ReadOnly Property LoanSchedule As LoanSchedule

    Public Overridable Property LoanPaymentFromBonus As LoanPaymentFromBonus
End Class

Partial Public Class LoanPaymentFromBonusModel
    Private ReadOnly _originalAmountPayment As Decimal
    Private _amountPayment As Decimal
    Private _isFullPayment As Boolean

    Public Sub New(bonus As Bonus, loanSchedule As LoanSchedule)
        BonusAmount = bonus.BonusAmount
        EffectiveStartDate = bonus.EffectiveStartDate
        EffectiveEndDate = bonus.EffectiveEndDate
        Frequency = bonus.AllowanceFrequency
        BonusId = bonus.RowID.Value

        _LoanSchedule = loanSchedule
        DeductionAmount = loanSchedule.DeductionAmount

        LoanId = loanSchedule.RowID.Value
        Dim loanPaymentFromBonus = bonus.LoanPaymentFromBonuses.FirstOrDefault(Function(l) l.LoanId = LoanId)

        Dim hasItems As Boolean

        If loanPaymentFromBonus IsNot Nothing Then
            _LoanPaymentFromBonus = loanPaymentFromBonus

            Id = loanPaymentFromBonus.Id

            AmountPayment = loanPaymentFromBonus.AmountPayment
            _originalAmountPayment = AmountPayment

            hasItems = If(loanPaymentFromBonus.Items?.Any(), False)
        End If

        _isFullPayment = If(AmountPayment = 0, False, AmountPayment >= MaxAvailablePayment)

        IsEditable = Not hasItems

        IsNew = Id = 0

        _TotalPayment = If(bonus.LoanPaymentFromBonuses?.
            Where(Function(l) l.BonusId = BonusId).
            Where(Function(l) l.LoanId <> LoanId).
            Sum(Function(l) l.AmountPayment), 0)
    End Sub

    Public ReadOnly Property IsFulfilled As Boolean
        Get
            Return LoanSchedule.TotalBalanceLeft <= TotalPayment + AmountPayment
        End Get
    End Property

    Public Property AmountPayment As Decimal
        Get
            Return _amountPayment
        End Get
        Set(value As Decimal)
            _amountPayment = value

            Dim overPaysLoan = _amountPayment > LoanSchedule.TotalBalanceLeft

            Dim overUseBonus = _amountPayment > ExclusiveCurrentBonusAmount

            'Dim overPaysDeductionAmount = _amountPayment > DeductionAmount

            If overPaysLoan OrElse overUseBonus Then
                SetPaymentToMaxAvailable()
            End If
        End Set
    End Property

    Public ReadOnly Property ValidPayment As Decimal
        Get
            Return ModDivision(AmountPayment, DeductionAmount)
        End Get
    End Property

    Public Property IsFullPayment As Boolean
        Get
            Return _isFullPayment
        End Get
        Set(value As Boolean)
            _isFullPayment = value

            If value Then
                SetPaymentToMaxAvailable()
            Else
                _amountPayment = 0
            End If
        End Set
    End Property

    Private Sub SetPaymentToMaxAvailable()
        'Dim remainder = MaxAvailablePayment Mod DeductionAmount
        '_amountPayment = MaxAvailablePayment - remainder
        'If MaxAvailablePayment - remainder = 0 Then
        '    _amountPayment = MaxAvailablePayment
        'End If
        _amountPayment = ModDivision(MaxAvailablePayment, DeductionAmount)
        _isFullPayment = True
    End Sub

    Private Function ModDivision(dividend As Decimal, divisor As Decimal) As Decimal
        Dim remainder = dividend Mod divisor
        If dividend - remainder = 0 Then
            Return dividend
        Else
            Return dividend - remainder
        End If
    End Function

    Public ReadOnly Property CurrentBonusAmount As Decimal
        Get
            Dim totalSharedLoanPayment = ExclusiveCurrentBonusAmount + AmountPayment
            Return BonusAmount - totalSharedLoanPayment
        End Get
    End Property

    Public ReadOnly Property ExclusiveCurrentBonusAmount As Decimal
        Get
            Return BonusAmount - TotalPayment
        End Get
    End Property

    Public ReadOnly Property InclusiveCurrentBonusAmount As Decimal
        Get
            Return BonusAmount - (TotalPayment + AmountPayment)
        End Get
    End Property

    Public ReadOnly Property MaxAvailablePayment As Decimal
        Get
            Dim totalBalanceLeft = LoanSchedule.TotalBalanceLeft

            Dim insufficientToPayMinimumDeductionAmount = ExclusiveCurrentBonusAmount < DeductionAmount
            If insufficientToPayMinimumDeductionAmount Then
                Return ExclusiveCurrentBonusAmount
            End If

            Dim sufficientToPayBalance = ExclusiveCurrentBonusAmount - totalBalanceLeft > -1
            If sufficientToPayBalance Then
                Return totalBalanceLeft
            Else
                Return ExclusiveCurrentBonusAmount
            End If
        End Get
    End Property

    Public ReadOnly Property HasChanged As Boolean
        Get
            Return AmountPayment <> _originalAmountPayment
        End Get
    End Property

    Public Function Export() As LoanPaymentFromBonus
        If Not IsNew Then
            LoanPaymentFromBonus.AmountPayment = AmountPayment
            LoanPaymentFromBonus.LastUpdBy = z_User
            Return LoanPaymentFromBonus
        Else
            Return New LoanPaymentFromBonus() With {
                .OrganizationID = z_OrganizationID,
                .CreatedBy = z_User,
                .AmountPayment = AmountPayment,
                .BonusId = BonusId,
                .LoanId = LoanId
            }
        End If
    End Function

End Class
