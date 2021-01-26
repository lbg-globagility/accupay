namespace AccuPay.Core.Interfaces
{
    public interface ICinemaTardinessReportModel
    {
        decimal Days { get; }
        int EmployeeId { get; }
        string EmployeeName { get; }
        decimal Hours { get; }
        int NumberOfOffense { get; }
        string NumberOfOffenseOrdinal { get; }
        string Sanction { get; }
    }
}
