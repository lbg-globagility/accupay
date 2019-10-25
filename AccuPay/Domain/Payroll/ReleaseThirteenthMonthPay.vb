Option Strict On

Imports AccuPay.DB
Imports AccuPay.Entity
Imports AccuPay.Utils
Imports Microsoft.EntityFrameworkCore

Public Class ReleaseThirteenthMonthPay

    Private thirteenthMonthPayname As String = "13th Month Pay"
    Private adjustmentCategoryName As String = "Adjustment Type"

    Public Sub New(dateFrom As Date, dateTo As Date, currentPeriodID As Integer?)
        Dim data = GetThirteenthMonthPays(dateFrom, dateTo)

        Using context = New PayrollContext()
            Dim paystubs =
                (From p In context.Paystubs.Include(Function(p) p.ActualAdjustments)
                 Where p.PayPeriodID.Value = currentPeriodID.Value).
                 ToList()

            Dim product =
                (From p In context.Products
                 Where p.OrganizationID.Value = z_OrganizationID And
                     p.PartNo = thirteenthMonthPayname And
                     p.Category = adjustmentCategoryName).
                 FirstOrDefault()

            Dim adjustmentCategoryId =
                context.Products.
                    Where(Function(p) CBool(p.OrganizationID.Value = z_OrganizationID _
                            And p.Category = adjustmentCategoryName)).
                        FirstOrDefault.
                            CategoryID

            'Provide this kind of Product if doesn't exists
            If product Is Nothing Then
                Dim thirteentMonthPayProduct = New Product() With {
                .OrganizationID = z_OrganizationID,
                .Created = Now.Date,
                .CreatedBy = z_User,
                .LastUpd = Now.Date,
                .LastUpdBy = z_User,
                .PartNo = thirteenthMonthPayname,
                .Name = thirteenthMonthPayname,
                .Description = String.Empty,
                .Category = adjustmentCategoryName,
                .CategoryID = adjustmentCategoryId.Value
                }
                Dim dfsdf = context.Products.Add(thirteentMonthPayProduct)
                context.SaveChanges()

                product = thirteentMonthPayProduct
            End If

            For Each paystub In paystubs
                'Preserve the existing ActualAdjustments, except the 13th month pay
                Dim non13thMonthAdjs = paystub.ActualAdjustments.Where(Function(aa) aa.ProductID.Value <> product.RowID.Value)

                'Reset the ActualAdjustments
                paystub.ActualAdjustments.Clear()

                'Restore the preserved ActualAdjustments
                If non13thMonthAdjs.Any() Then
                    For Each aa In non13thMonthAdjs
                        paystub.ActualAdjustments.Add(aa)
                    Next
                End If

                ' Get first the total thirteenth month pay that the employee will receive.
                Dim row = data.
                    Select($"EmployeeID = '{paystub.EmployeeID}'").
                    FirstOrDefault()
                Dim thirteenthMonthPay = CDec(row("ThirteenthMonthPay"))

                ' Add the new thirteenth pay as an adjustment to the paystub
                paystub.ActualAdjustments.Add(New ActualAdjustment() With {
                    .ProductID = product.RowID,
                    .OrganizationID = z_OrganizationID,
                    .CreatedBy = z_User,
                    .PayAmount = AccuMath.CommercialRound(thirteenthMonthPay)
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