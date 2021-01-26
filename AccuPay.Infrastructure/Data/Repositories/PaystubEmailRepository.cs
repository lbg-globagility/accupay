using AccuPay.Core.Entities;
using AccuPay.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Infrastructure.Data
{
    public class PaystubEmailRepository : IPaystubEmailRepository
    {
        private readonly PayrollContext _context;

        public PaystubEmailRepository(PayrollContext context)
        {
            _context = context;
        }

        #region Save

        public async Task SetStatusToFailed(int paystubEmailId, string errorLogMessage)
        {
            var paystubEmail = await _context.PaystubEmails
                .FirstOrDefaultAsync(x => x.RowID == paystubEmailId);

            if (paystubEmail != null)
            {
                paystubEmail.SetStatusToFailed(errorLogMessage);
                await _context.SaveChangesAsync();
            }
        }

        public async Task SetStatusToProcessing(int paystubEmailId)
        {
            var paystubEmail = await _context.PaystubEmails
                .FirstOrDefaultAsync(x => x.RowID == paystubEmailId);

            if (paystubEmail != null)
            {
                paystubEmail.SetStatusToProcessing();
                await _context.SaveChangesAsync();
            }
        }

        public async Task Finish(int id, string fileName, string emailAddress)
        {
            var paystubEmail = await _context.PaystubEmails
                .FirstOrDefaultAsync(x => x.RowID == id);

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
                await _context.SaveChangesAsync();
            }
        }

        public async Task CreateManyAsync(ICollection<PaystubEmail> paystubEmails)
        {
            _context.PaystubEmails.AddRange(paystubEmails);
            await _context.SaveChangesAsync();
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

        public async Task ResetAllProcessingAsync()
        {
            var emails = await _context.PaystubEmails
                .Where(x => x.Status == PaystubEmail.StatusProcessing)
                .ToListAsync();

            if (!emails.Any()) return;

            emails.ForEach((paystubEmail) =>
            {
                paystubEmail.ResetStatus();
            });

            _context.PaystubEmails.RemoveRange(emails);
            await _context.SaveChangesAsync();
        }

        #endregion Save

        #region Queries

        public PaystubEmail FirstOnQueueWithPaystubDetails()
        {
            return _context.PaystubEmails
                .Where(x => x.Status == PaystubEmail.StatusWaiting)
                .Include(x => x.Paystub.PayPeriod)
                .Include(x => x.Paystub.Employee)
                .FirstOrDefault();
        }

        public async Task<ICollection<PaystubEmail>> GetByPaystubIdsAsync(int[] paystubIds)
        {
            return await _context.PaystubEmails
                .Where(x => paystubIds.Contains(x.PaystubID))
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<ICollection<PaystubEmail>> GetAllOnQueueAsync()
        {
            return await _context.PaystubEmails
                .Where(x => x.Status != PaystubEmail.StatusFailed)
                .Include(x => x.Paystub.Employee)
                .AsNoTracking()
                .ToListAsync();
        }

        #endregion Queries
    }
}
