Option Strict On

Namespace Global.AccuPay.Helpers

    Public NotInheritable Class PolicyHelper

        Private _policy As TimeEntryPolicy
        Private _settings As ListOfValueCollection

        Public Sub New()

            Using context = New PayrollContext()

                _settings = New ListOfValueCollection(context.ListOfValues.ToList())

                _policy = New TimeEntryPolicy(_settings)

            End Using

        End Sub

        Public ReadOnly Property ComputeBreakTimeLate As Boolean
            Get
                Return _policy.ComputeBreakTimeLate
            End Get
        End Property

        Public ReadOnly Property UseShiftSchedule As Boolean
            Get

                Return _policy.UseShiftSchedule
            End Get
        End Property

        Public ReadOnly Property RespectDefaultRestDay As Boolean
            Get
                Return _policy.RespectDefaultRestDay
            End Get
        End Property

        Public ReadOnly Property ValidateLeaveBalance As Boolean
            Get

                Return _policy.ValidateLeaveBalance
            End Get
        End Property

        Public ReadOnly Property PayRateCalculationBasis As PayRateCalculationBasis
            Get
                Return _settings.GetEnum("Pay rate.CalculationBasis",
                                    PayRateCalculationBasis.Organization)
            End Get
        End Property

        Public ReadOnly Property ShowActual As Boolean
            Get
                Return _settings.GetBoolean("Policy.ShowActual", True)
            End Get
        End Property

        Public ReadOnly Property UseUserLevel As Boolean
            Get
                Return _settings.GetBoolean("User Policy.UseUserLevel", False)
            End Get
        End Property

        Public ReadOnly Property DefaultBPIInsurance As Decimal
            Get
                Return _settings.GetDecimal("Default.BPIInsurance")
            End Get
        End Property

        Public ReadOnly Property ShowBranch As Boolean
            Get
                Return _settings.GetBoolean("Employee Policy.ShowBranch", False)
            End Get
        End Property

        Public ReadOnly Property UseBPIInsurance As Boolean
            Get
                Return _settings.GetBoolean("Employee Policy.UseBPIInsurance", False)
            End Get
        End Property

    End Class

End Namespace