Option Strict On

Public Class PayrollConstants

    Public Const SssCalculationEarnings = "Earnings"

    Public Const SssCalculationGross = "GrossPay"

End Class

Public Enum SssCalculationBasis
    Earnings
    GrossPay
    BasicSalary
    BasicMinusDeductions
    BasicMinusDeductionsWithoutPremium
End Enum

Public Enum PhilHealthCalculationBasis
    Earnings
    GrossPay
    BasicSalary
    BasicAndEcola
    BasicMinusDeductions
    BasicMinusDeductionsWithoutPremium
End Enum

Public Enum ThirteenthMonthCalculationBasis
    RegularPayAndAllowance
    DailyRate
End Enum

Public Enum WithholdingTaxCalculationBasis
    Earnings
    GrossPay
    BasicSalary
End Enum

Public Enum PayrollSummaryAdjustmentBreakdownPolicy
    TotalOnly
    BreakdownOnly
    Both
End Enum

Public Enum PayRateCalculationBasis
    Organization
    Branch
End Enum