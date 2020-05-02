using AccuPay.Data.Entities;
using AccuPay.Utilities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace AccuPay.Data.Services
{
    public class ReleaseThirteenthMonthPay
    {
        private readonly int _organizationId;
        private readonly int _userId;

        private const string thirteenthMonthPayname = "13th Month Pay";
        private const string adjustmentCategoryName = "Adjustment Type";

        public ReleaseThirteenthMonthPay(DateTime dateFrom, DateTime dateTo, int? currentPeriodID, int organizationId, int userId)
        {
            var data = GetThirteenthMonthPays(dateFrom, dateTo);

            using (var context = new PayrollContext())
            {
                var paystubs = (from p in context.Paystubs.Include(p => p.ActualAdjustments)
                                where p.PayPeriodID.Value == currentPeriodID.Value
                                select p).ToList();

                var product = (from p in context.Products
                               where p.OrganizationID.Value == _organizationId &&
                                    p.PartNo == thirteenthMonthPayname &&
                                    p.Category == adjustmentCategoryName
                               select p).FirstOrDefault();

                var adjustmentCategoryId = context.Products.
                                            Where(p => p.OrganizationID.Value == _organizationId).
                                            Where(p => p.Category == adjustmentCategoryName).
                                            FirstOrDefault().CategoryID;

                // Provide this kind of Product if doesn't exists
                if (product == null)
                {
                    var thirteentMonthPayProduct = new Product()
                    {
                        OrganizationID = _organizationId,
                        Created = DateTime.Now.Date,
                        CreatedBy = _userId,
                        LastUpd = DateTime.Now.Date,
                        LastUpdBy = _userId,
                        PartNo = thirteenthMonthPayname,
                        Name = thirteenthMonthPayname,
                        Description = string.Empty,
                        Category = adjustmentCategoryName,
                        CategoryID = adjustmentCategoryId.Value
                    };
                    var dfsdf = context.Products.Add(thirteentMonthPayProduct);
                    context.SaveChanges();

                    product = thirteentMonthPayProduct;
                }

                foreach (var paystub in paystubs)
                {
                    // Preserve the existing ActualAdjustments, except the 13th month pay
                    var non13thMonthAdjs = paystub.ActualAdjustments.Where(aa => aa.ProductID.Value != product.RowID.Value);

                    // Reset the ActualAdjustments
                    paystub.ActualAdjustments.Clear();

                    // Restore the preserved ActualAdjustments
                    if (non13thMonthAdjs.Any())
                    {
                        foreach (var aa in non13thMonthAdjs)
                            paystub.ActualAdjustments.Add(aa);
                    }

                    // Get first the total thirteenth month pay that the employee will receive.
                    var row = data.Select($"EmployeeID = '{paystub.EmployeeID}'").FirstOrDefault();
                    var thirteenthMonthPay = Convert.ToDecimal(row["ThirteenthMonthPay"]);

                    // Add the new thirteenth pay as an adjustment to the paystub
                    paystub.ActualAdjustments.Add(new ActualAdjustment()
                    {
                        ProductID = product.RowID,
                        OrganizationID = _organizationId,
                        CreatedBy = _userId,
                        Amount = AccuMath.CommercialRound(thirteenthMonthPay)
                    });
                }

                context.SaveChanges();
            }

            this._organizationId = organizationId;
            this._userId = userId;
        }

        private DataTable GetThirteenthMonthPays(DateTime dateFrom, DateTime dateTo)
        {
            var thirteenthMonthPay = $@"
            SELECT pst.EmployeeID, SUM(tmp.Amount) AS ThirteenthMonthPay
            FROM thirteenthmonthpay tmp
            INNER JOIN paystub pst
                    ON pst.RowID = tmp.PaystubID
            WHERE @PayFromDate <= pst.PayFromDate AND
                  pst.PayToDate <= @PayToDate AND
                  pst.OrganizationID = @OrganizationID
            GROUP BY pst.EmployeeID;";

            var @params = new Dictionary<string, object>()
            {
                {"@OrganizationID",_organizationId},
                {"@PayFromDate",dateFrom},
                {"@PayToDate",dateTo}
            };

            //var query = new SqlToDataTable(thirteenthMonthPay, @params);
            //return query.Read();
            return null;
        }
    }
}