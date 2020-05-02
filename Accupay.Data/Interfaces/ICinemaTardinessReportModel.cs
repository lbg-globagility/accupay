namespace AccuPay.Data
{
    public interface ICinemaTardinessReportModel
    {
        decimal Days { get; set; }
        int EmployeeId { get; set; }
        string EmployeeName { get; set; }
        decimal Hours { get; set; }
        int NumberOfOffense { get; set; }
        string NumberOfOffenseOrdinal { get; }
        string Sanction { get; }
    }
}