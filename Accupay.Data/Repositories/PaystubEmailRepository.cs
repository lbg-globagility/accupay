using AccuPay.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class PaystubEmailRepository
    {
        private readonly PayrollContext _context;

        public PaystubEmailRepository(PayrollContext context)
        {
            _context = context;
        }

        // Use domain methods like paystubEmail.Failed()
        // then just call service.Update(paystubEmail)
        // Methods like this should not exist on the repository
        public void SetStatusToFailed(int paystubEmailId, string errorLogMessage)
        {
            var paystubEmail = _context.PaystubEmails
                .FirstOrDefault(x => x.RowID == paystubEmailId);

            if (paystubEmail != null)
            {
                paystubEmail.Status = PaystubEmail.StatusFailed;
                paystubEmail.ErrorLogMessage = errorLogMessage;
                _context.SaveChanges();
            }
        }

        public void SetStatusToProcessing(int paystubEmailId)
        {
            var paystubEmail = _context.PaystubEmails
                .FirstOrDefault(x => x.RowID == paystubEmailId);

            if (paystubEmail != null)
            {
                paystubEmail.Status = PaystubEmail.StatusProcessing;
                paystubEmail.ProcessingStarted = DateTime.Now;
                _context.SaveChanges();
            }
        }

        public void Finish(int id, string fileName, string emailAddress)
        {
            var paystubEmail = _context.PaystubEmails
                .FirstOrDefault(x => x.RowID == id);

            if (paystubEmail != null)
            {
                var paystubEmailHistory = new PaystubEmailHistory();
                paystubEmailHistory.PaystubID = paystubEmail.PaystubID;
                paystubEmailHistory.ReferenceNumber = fileName;
                paystubEmailHistory.SentDateTime = DateTime.Now;
                paystubEmailHistory.SentBy = paystubEmail.CreatedBy;
                paystubEmailHistory.EmailAddress = emailAddress;
                paystubEmailHistory.IsActual = paystubEmail.IsActual;

                _context.PaystubEmailHistories.Add(paystubEmailHistory);
                _context.PaystubEmails.Remove(paystubEmail);
                _context.SaveChanges();
            }
        }

        public async Task CreateManyAsync(IEnumerable<PaystubEmail> paystubEmails)
        {
            _context.PaystubEmails.AddRange(paystubEmails);
            await _context.SaveChangesAsync();
        }

        public PaystubEmail FirstOnQueueWithPaystubDetails()
        {
            return _context.PaystubEmails
                .Where(x => x.Status == PaystubEmail.StatusWaiting)
                .Include(x => x.Paystub.PayPeriod)
                .Include(x => x.Paystub.Employee)
                .FirstOrDefault();
        }

        public async Task<IEnumerable<PaystubEmail>> GetByPaystubIdsAsync(int[] paystubIds)
        {
            return await _context.PaystubEmails
                .Where(x => paystubIds.Contains(x.PaystubID))
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task DeleteByEmployeeAndPayPeriodAsync(int employeeId, int payPeriodId)
        {
            var emails = await _context.PaystubEmails
                .Where(x => x.Paystub.EmployeeID == employeeId)
                .Where(x => x.Paystub.PayPeriodID == payPeriodId)
                .ToListAsync();

            if (!emails.Any()) return;

            _context.PaystubEmails.RemoveRange(emails);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteByPayPeriodAsync(int payPeriodId)
        {
            var emails = await _context.PaystubEmails
                .Where(x => x.Paystub.PayPeriodID == payPeriodId)
                .ToListAsync();

            if (!emails.Any()) return;

            _context.PaystubEmails.RemoveRange(emails);
            await _context.SaveChangesAsync();
        }
    }
}