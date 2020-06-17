using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AccuPay.Web.TimeLogs
{
    public class EmployeeTimeLogsDto
    {
        public int? EmployeeId { get; set; }

        public string EmployeeNo { get; set; }

        public string FullName { get; set; }

        public ICollection<EmployeeTimeLogDto> TimeLogs { get; set; }

        public EmployeeTimeLogsDto()
        {
            TimeLogs = new Collection<EmployeeTimeLogDto>();
        }

        public class EmployeeTimeLogDto
        {
            public int? Id { get; set; }

            public DateTime Date { get; set; }

            public DateTime? StartTime { get; set; }

            public DateTime? EndTime { get; set; }

            public int? BranchId { get; set; }

            public string BranchName { get; set; }
        }
    }
}
