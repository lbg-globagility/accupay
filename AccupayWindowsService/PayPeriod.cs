using Accupay.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AccupayWindowsService
{
    internal class PayPeriod : IPayPeriod
    {
        public int? RowID { get; set; }
        public DateTime PayFromDate { get; set; }
        public DateTime PayToDate { get; set; }
    }
}