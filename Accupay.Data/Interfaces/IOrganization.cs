using System;

namespace AccuPay.Data
{
    public interface IOrganization
    {
        DateTime Created { get; set; }
        int? CreatedBy { get; set; }
        bool IsAgency { get; set; }
        DateTime? LastUpd { get; set; }
        int? LastUpdBy { get; set; }
        string Name { get; set; }
        TimeSpan NightDifferentialTimeFrom { get; set; }
        TimeSpan NightDifferentialTimeTo { get; set; }
        int? RowID { get; set; }
    }
}