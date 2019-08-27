Option Strict On

Imports AccuPay.Entity

Public Interface IAdjustment

    Property RowID As Integer?

    Property OrganizationID As Integer?

    Property Created As Date

    Property CreatedBy As Integer?

    Property LastUpd As Date?

    Property LastUpdBy As Integer?

    Property PayStubID As Integer?

    Property ProductID As Integer?

    Property PayAmount As Decimal

    Property Comment As String

    Property IsActual As Boolean

    Property Paystub As Paystub

    Property Product As Product

End Interface