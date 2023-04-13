using AccuPay.Utilities.Attributes;

namespace AccuPay.Core.Interfaces.Excel
{
    public abstract class ExcelOrganizationRowRecord
    {
        [ColumnName("Company Name")]
        public string CompanyName { get; set; }

        [ColumnName("OrganizationRowId")]
        public int? OrganizationRowId { get; set; }
    }
}
