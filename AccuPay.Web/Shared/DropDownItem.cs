using AccuPay.Data.Entities;

namespace AccuPay.Web.Shared
{
    public class DropDownItem
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public static DropDownItem FromProduct(Product product)
        {
            return new DropDownItem()
            {
                Id = product.RowID.Value,
                Name = product.PartNo,
            };
        }
    }
}
