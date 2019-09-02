Option Strict On

Namespace Global.AccuPay.Payroll

    Public Class PhilHealthPolicy

        Private ReadOnly _settings As ListOfValueCollection

        Public Sub New(settings As ListOfValueCollection)
            _settings = settings
        End Sub

        Public ReadOnly Property OddCentDifference As Boolean
            Get
                Return _settings.GetBoolean("PhilHealth.Remainder", True)
            End Get
        End Property

        Public ReadOnly Property CalculationBasis As PhilHealthCalculationBasis
            Get
                Dim policyByOrganization = _settings.GetBoolean("Policy.ByOrganization", False)

                Return _settings.GetEnum(
                    "PhilHealth.CalculationBasis",
                    PhilHealthCalculationBasis.BasicSalary,
                    policyByOrganization)
            End Get
        End Property

        Public ReadOnly Property MinimumContribution As Decimal
            Get
                Return _settings.GetDecimal("PhilHealth.MinimumContribution")
            End Get
        End Property

        Public ReadOnly Property MaximumContribution As Decimal
            Get
                Return _settings.GetDecimal("PhilHealth.MaximumContribution")
            End Get
        End Property

        Public ReadOnly Property Rate As Decimal
            Get
                Return _settings.GetDecimal("PhilHealth.Rate")
            End Get
        End Property

    End Class

End Namespace