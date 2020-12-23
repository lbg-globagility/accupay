Option Strict On

Imports AccuPay.Data.Entities

Public Class PayPeriodMother

    Public Shared Function StartDateOnly(payFromDate As Date) As PayPeriod

        Dim payPeriod As New PayPeriod() With
        {
            .PayFromDate = payFromDate
        }

        Return payPeriod

    End Function

End Class
