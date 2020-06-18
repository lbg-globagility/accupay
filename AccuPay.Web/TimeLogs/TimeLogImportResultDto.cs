using System;
using System.Collections.Generic;
using System.Text;

namespace AccuPay.Web.TimeLogs
{
    public class TimeLogImportResultDto
    {
        public IEnumerable<TimeLogDto> GeneratedTimeLogs { get; set; }
        public IEnumerable<TimeLogImportDetailsDto> InvalidRecords { get; set; }
    }
}
