using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace AccuPay.Data.Entities
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

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public DateTime Created { get; set; }

        public int CreatedBy { get; set; }

        public int PaystubID { get; set; }
        public DateTime? ProcessingStarted { get; set; }
        public string ErrorLogMessage { get; set; }
        public string Status { get; set; }

        [ForeignKey("PaystubID")]
        public virtual Paystub Paystub { get; set; }

        // TODO: codes using PayrollContext should be in a repository or a service
        public void SetStatusToFailed(string errorLogMessage)
        {
            using (var context = new PayrollContext())
            {
                var paystubEmail = context.PaystubEmails.
                                        FirstOrDefault(x => x.RowID == this.RowID);

                if (paystubEmail != null)
                {
                    paystubEmail.Status = StatusFailed;
                    paystubEmail.ErrorLogMessage = errorLogMessage;
                    context.SaveChanges();
                }
            }
        }

        // TODO: codes using PayrollContext should be in a repository or a service
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

        // TODO: codes using PayrollContext should be in a repository or a service
        public void Finish(string fileName, string emailAddress)
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
                    paystubEmailHistory.EmailAddress = emailAddress;

                    context.PaystubEmailHistories.Add(paystubEmailHistory);
                    context.PaystubEmails.Remove(paystubEmail);
                    context.SaveChanges();
                }
            }
        }
    }
}