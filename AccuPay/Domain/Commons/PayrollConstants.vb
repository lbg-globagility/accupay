Option Strict On

Public Class PayrollConstants

    Public Const SssCalculationEarnings = "Earnings"

    Public Const SssCalculationGross = "GrossPay"

End Class

Public Enum SssCalculationBasis
    Earnings
    GrossPay
    BasicSalary
    BasicMinusDeduction
End Enum

Public Enum PhilHealthCalculationBasis
    Earnings
    GrossPay
    BasicSalary
    BasicAndEcola
    BasicMinusDeduction
End Enum

Public Enum WithholdingTaxCalculationBasis
    Earnings
    GrossPay
    BasicSalary
End Enum