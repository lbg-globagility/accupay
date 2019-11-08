using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Accupay.Data.Entities
{
    [Table("paystubemail")]
    public class PaystubEmail
    {
        public const string StatusWaiting = "WAITING";
        public const string StatusProcessing = "PROCESSING";
        public const string StatusFailed = "FAILED";

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RowID { get; set; }

        public int PaystubID { get; set; }
        public string Status { get; set; }
        public DateTime? ProcessingStarted { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime Created { get; set; }

        public int CreatedBy { get; set; }

        [ForeignKey("PaystubID")]
        public virtual Paystub Paystub { get; set; }

        public void ResetStatus()
        {
            using (var context = new PayrollContext())
            {
                var paystubEmail = context.PaystubEmails.
                                        FirstOrDefault(x => x.RowID == this.RowID);

                if (paystubEmail != null)
                {
                    paystubEmail.Status = StatusWaiting;
                    context.SaveChanges();
                }
            }
        }

        public void SetStatusToProcessing()
        {
            using (var context = new PayrollContext())
            {
                var paystubEmail = context.PaystubEmails.
                                        FirstOrDefault(x => x.RowID == this.RowID);

                if (paystubEmail != null)
                {
                    paystubEmail.Status = StatusProcessing;
                    paystubEmail.ProcessingStarted = DateTime.Now;
                    context.SaveChanges();
                }
            }
        }

        public void Finish(string fileName)
        {
            using (var context = new PayrollContext())
            {
                var paystubEmail = context.PaystubEmails.
                                        FirstOrDefault(x => x.RowID == this.RowID);

                if (paystubEmail != null)
                {
                    var paystubEmailHistory = new PaystubEmailHistory();
                    paystubEmailHistory.PaystubID = this.PaystubID;
                    paystubEmailHistory.ReferenceNumber = fileName;
                    paystubEmailHistory.SentDateTime = DateTime.Now;
                    paystubEmailHistory.SentBy = this.CreatedBy;

                    context.PaystubEmailHistories.Add(paystubEmailHistory);
                    context.PaystubEmails.Remove(paystubEmail);
                    context.SaveChanges();
                }
            }
        }
    }
}