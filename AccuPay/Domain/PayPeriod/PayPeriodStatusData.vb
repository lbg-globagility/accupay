Public Class PayPeriodStatusData

    Public Enum PayPeriodStatus
        Open
        Processing
        Closed
    End Enum

    Public Class PayPeriodStatusReference
        Public Property PayPeriodStatus As PayPeriodStatus

        Public Sub New(ByVal payPeriodStatus As PayPeriodStatus)
            Me.PayPeriodStatus = payPeriodStatus
        End Sub

    End Class

    Public Property Index As Integer
    Public Property Status As PayPeriodStatus

    Public Sub New()
    End Sub

End Class