Imports AccuPay.Data.Entities

Public Class LoanPaymentFromBonusModel
    Public Property Id As Integer
    Public Property LoanId As Integer
    Public Property BonusId As Integer
    Public Property PaystubId As Integer?

    'Private _amountPayment As Decimal

    Public Property AmountPayment As Decimal
    '    Get
    '        Return _amountPayment
    '    End Get
    '    Set(ByVal value As Decimal)
    '        _amountPayment = value

    '        'If _amountPayment = Bonus.BonusAmount _
    '        '    And Not isFullAmount Then isFullAmount = True

    '        'If _amountPayment < Bonus.BonusAmount Then isFullAmount = False
    '    End Set
    'End Property

    Private _isFullAmount As Boolean

    Public Property IsFullPayment() As Boolean
        Get
            Return _isFullAmount
        End Get
        Set(ByVal value As Boolean)
            _isFullAmount = value

            'If isFullAmount _
            '    And Not _amountPayment = Bonus.BonusAmount Then _amountPayment = Bonus.BonusAmount
        End Set
    End Property

    Public Property BonusAmount As Decimal

    Public Property DeductionAmount As Decimal

    Public Property EffectiveDate As Date

    Public Overridable Property Bonus As Bonus

    Public Overridable Property LoanSchedule As LoanSchedule
End Class

Partial Public Class LoanPaymentFromBonusModel
    Private Shared _loanPaymentFromBonus As LoanPaymentFromBonus

    Public Sub New()

    End Sub

    Public Sub New(loanPaymentFromBonus As LoanPaymentFromBonus)
        _loanPaymentFromBonus = loanPaymentFromBonus
        Convert(_loanPaymentFromBonus)
    End Sub

    Private Sub Convert(loanPaymentFromBonus As LoanPaymentFromBonus)
        'Dim bonus = loanPaymentFromBonus.Bonus
        'Dim model = New LoanPaymentFromBonusModel() With {}
        Bonus = loanPaymentFromBonus.Bonus
        BonusId = loanPaymentFromBonus.BonusId
        Id = loanPaymentFromBonus.Id
        LoanSchedule = loanPaymentFromBonus.LoanSchedule
        LoanId = loanPaymentFromBonus.LoanId
        AmountPayment = loanPaymentFromBonus.AmountPayment
        DeductionAmount = loanPaymentFromBonus.DeductionAmount
        PaystubId = loanPaymentFromBonus.PaystubId

        Dim difference = Bonus.BonusAmount - AmountPayment
        IsFullPayment = (AmountPayment = Bonus.BonusAmount) Or
            (difference >= 0 And difference <= DeductionAmount)

        BonusAmount = Bonus.BonusAmount
        EffectiveDate = Bonus.EffectiveEndDate

        'Return model
    End Sub

    Public Shared Function Convert(bonus As Bonus, loanSchedule As LoanSchedule)

        Dim model = New LoanPaymentFromBonusModel() With
            {.Bonus = bonus,
            .LoanSchedule = loanSchedule,
            .BonusId = bonus.RowID,
            .LoanId = loanSchedule.RowID,
            .DeductionAmount = loanSchedule.DeductionAmount}

        'model.isFullAmount = loanSchedule.TotalBalanceLeft <= bonus.BonusAmount
        model.BonusAmount = bonus.BonusAmount
        model.EffectiveDate = bonus.EffectiveEndDate

        Return model
    End Function

    Public Sub Scrutinize(newTotalBalanceLeft As Decimal)
        If _isFullAmount Then
            Dim remainder As Decimal = Bonus.BonusAmount Mod LoanSchedule.DeductionAmount
            Dim isValid = Bonus.BonusAmount - newTotalBalanceLeft > -1
            If isValid Then
                AmountPayment = newTotalBalanceLeft
            Else
                AmountPayment = Bonus.BonusAmount - remainder
            End If

            'ElseIf Not _isFullAmount Then
            '    If AmountPayment > 0 Then
            '        AmountPayment = 0
            '    End If
        End If
    End Sub

    Public ReadOnly Property IsNew() As Boolean
        Get
            Return Id = 0
        End Get
    End Property

    Public ReadOnly Property InvalidAmountPayment As Boolean
        Get
            If LoanSchedule Is Nothing Then Return False

            If AmountPayment = 0 Then Return False

            Dim deductAmount = If(Id = 0, LoanSchedule.DeductionAmount, DeductionAmount)

            Return Not (AmountPayment Mod deductAmount) = 0

            'Return AmountPayment < LoanSchedule.DeductionAmount
        End Get
    End Property

    Public ReadOnly Property HasChanged As Boolean
        Get
            Dim isFullPaymentChanged = Not IsFullPayment And AmountPayment > 0
            'Dim isAmountPaymentChanged = AmountPayment > 0
            Dim notNew = Not IsNew
            Dim existingAndChanged1 = notNew And IsFullPayment = False
            Dim existingAndChanged2 = notNew And AmountPayment > 0 And IsFullPayment
            Dim newlyAdded = IsNew And AmountPayment > 0

            Dim satisfiedList = {isFullPaymentChanged, existingAndChanged1, existingAndChanged2, newlyAdded}
            Dim satisfied = satisfiedList.Any(Function(t) t)

            Return satisfied
        End Get
    End Property

    Public Function Export() As LoanPaymentFromBonus
        If Id > 0 Then
            _loanPaymentFromBonus.AmountPayment = AmountPayment
            Return _loanPaymentFromBonus
        Else
            Return New LoanPaymentFromBonus() With {
                .AmountPayment = AmountPayment,
                .BonusId = BonusId,
                .LoanId = LoanId,
                .PaystubId = PaystubId,
                .DeductionAmount = LoanSchedule.DeductionAmount
            }

        End If
    End Function

End Class