Option Strict On

Imports AccuPay.Core.Entities

Namespace Benchmark

    Public Class AdjustmentInput

        Property Adjustment As Product
        Property Amount As Decimal

        Sub New(adjustment As Product)

            Me.Adjustment = adjustment

        End Sub

        ReadOnly Property Code As String
            Get
                Return Adjustment?.Comments
            End Get
        End Property

        ReadOnly Property Description As String
            Get
                Return Adjustment?.PartNo
            End Get
        End Property

    End Class

End Namespace