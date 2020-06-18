using AccuPay.Data.Entities;
using System;
using System.Collections.Generic;

namespace AccuPay.Data.Services
{
    public class TimeEntryData
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public DateTime Date { get; set; }
        public TimeEntry TimeEntry { get; set; }
        public TimeLog TimeLog { get; set; }
        public EmployeeDutySchedule Shift { get; set; }
        public List<Overtime> Overtimes { get; set; }
        public OfficialBusiness OfficialBusiness { get; set; }
        public Leave Leave { get; set; }
        public Branch Branch { get; set; }
    }
}