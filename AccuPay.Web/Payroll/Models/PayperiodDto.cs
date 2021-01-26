using AccuPay.Core.Entities;
using AccuPay.Web.Core.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace AccuPay.Web.Payroll
{
    public class PayPeriodDto
    {
        public int? Id { get; set; }

        [DataType(DataType.Date)]
        [JsonConverter(typeof(DateConverter))]
        public DateTime CutoffStart { get; set; }

        [DataType(DataType.Date)]
        [JsonConverter(typeof(DateConverter))]
        public DateTime CutoffEnd { get; set; }

        public string Status { get; set; }

        public int Month { get; set; }

        public int Year { get; set; }

        public bool IsFirstHalf { get; set; }

        public void ApplyData(PayPeriod payPeriod)
        {
            Id = payPeriod.RowID;
            CutoffStart = payPeriod.PayFromDate;
            CutoffEnd = payPeriod.PayToDate;
            Status = payPeriod.Status.ToString();
            Month = payPeriod.Month;
            Year = payPeriod.Year;
            IsFirstHalf = payPeriod.IsFirstHalf;
        }
    }
}
