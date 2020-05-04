using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Data.Entities
{
    [Table("tardinessrecord")]
    public class TardinessRecord
    {
        public int Year { get; set; }
        public int EmployeeId { get; set; }
        public DateTime FirstOffenseDate { get; set; }
    }
}