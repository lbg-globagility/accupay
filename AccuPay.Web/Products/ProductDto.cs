using AccuPay.Core.Entities;

namespace AccuPay.Web.Products
{
    public class ProductDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsFixed { get; set; }

        public bool IsTaxable { get; set; }

        public bool IsThirteenthMonthPay { get; set; }

        public static ProductDto FromProduct(Product product)
        {
            return new ProductDto()
            {
                Id = product.RowID.Value,
                Name = product.PartNo,
                IsFixed = product.Fixed,
                IsTaxable = product.IsTaxable,
                IsThirteenthMonthPay = product.IsThirteenthMonthPay
            };
        }
    }
}
