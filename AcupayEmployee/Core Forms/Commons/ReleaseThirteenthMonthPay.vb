Option Strict On

Imports System.Data.Entity
Imports AccuPay.Entity

Public Class ReleaseThirteenthMonthPay

    Public Sub New(dateFrom As Date, dateTo As Date, currentPeriodID As Integer?)
        Dim data = GetThirteenthMonthPays(dateFrom, dateTo)

        Using context = New PayrollContext()
            Dim paystubs =
                (From p In context.Paystubs.Include(Function(p) p.Adjustments)
                 Where p.PayPeriodID = currentPeriodID).
                 ToList()

            Dim product =
                (From p In context.Products
                 Where p.OrganizationID = z_OrganizationID And
                     p.PartNo = "13th Month Pay" And
                     p.Category = "Adjustment Type").
                 FirstOrDefault()

            For Each paystub In paystubs
                ' Get first the total thirteenth month pay that the employee will receive.
                Dim row = data.
                    Select($"EmployeeID = '{paystub.EmployeeID}'").
                    FirstOrDefault()
                Dim thirteenthMonthPay = CDec(row("ThirteenthMonthPay"))

                ' Add the new thirteenth pay as an adjustment to the paystub
                paystub.Adjustments.Add(New Adjustment() With {
                    .ProductID = product.RowID,
                    .OrganizationID = z_OrganizationID,
                    .CreatedBy = z_User,
                    .PayAmount = thirteenthMonthPay
                })
            Next

            context.SaveChanges()
        End Using
    End Sub

    Private Function GetThirteenthMonthPays(dateFrom As Date, dateTo As Date) As DataTable
        Dim thirteenthMonthPay = $"
            SELECT pst.EmployeeID, SUM(tmp.Amount) AS ThirteenthMonthPay
            FROM thirteenthmonthpay tmp
            INNER JOIN paystub pst
                    ON pst.RowID = tmp.PaystubID
            WHERE @PayFromDate <= pst.PayFromDate AND
                  pst.PayToDate <= @PayToDate AND
                  pst.OrganizationID = @OrganizationID
            GROUP BY pst.EmployeeID;"

        Dim params = New Dictionary(Of String, Object) From {
            {"@OrganizationID", z_OrganizationID},
            {"@PayFromDate", dateFrom},
            {"@PayToDate", dateTo}
        }

        Dim query = New SqlToDataTable(thirteenthMonthPay, params)
        Return query.Read()
    End Function

End Class
