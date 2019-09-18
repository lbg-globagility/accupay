Imports AccuPay.Entity
Imports AccuPay.Utils

Public Class PayPeriodStatusData

    Public Enum PayPeriodStatus

        Open
        Processing
        Closed

    End Enum

    Public Class PayPeriodStatusReference

        Public Property PayPeriodStatus As PayPeriodStatus

        Sub New(payPeriodStatus As PayPeriodStatus)

            Me.PayPeriodStatus = payPeriodStatus

        End Sub

    End Class

    Public Property Index As Integer
    Public Property Status As PayPeriodStatus

    Public Shared Function GetPeriodsWithPaystubCount(Optional payFreqType As String = "SEMI-MONTHLY") As List(Of PayPeriod)

        Using context As New PayrollContext

            Dim payFrequencyId = If(payFreqType = "WEEKLY",
                                        PayrollTools.PayFrequencyWeeklyId,
                                        PayrollTools.PayFrequencyMonthlyId)

            Return context.PayPeriods.
                    Where(Function(p) p.OrganizationID.Value = ObjectUtils.ToNullableInteger(orgztnID)).
                    Where(Function(p) p.PayFrequencyID = payFrequencyId).
                    Where(Function(p) Not p.IsClosed).
                    Where(Function(p) context.
                                        Paystubs.
                                        Where(Function(s) s.PayPeriodID.Value = p.RowID.Value).
                                        Any()).
                    ToList

        End Using

    End Function

End Class