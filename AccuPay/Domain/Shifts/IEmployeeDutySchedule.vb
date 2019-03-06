Public Interface IEmployeeDutySchedule
    Property OrganizationID As Integer?
    Property EmployeeID As Integer?
    Property DateSched As Date?
    Property StartTime As TimeSpan?
    Property EndTime As TimeSpan?
    Property BreakStartTime As TimeSpan?
    Property BreakEndTime As TimeSpan?
    Property IsRestDay As Boolean
End Interface
