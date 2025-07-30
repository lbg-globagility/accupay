using AccuPay.Core.Entities;
using AccuPay.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AccuPay.Infrastructure.Data.Repositories
{
    public class BankFileHeaderRepository : SavableRepository<BankFileHeader>, IBankFileHeaderRepository
    {
        public BankFileHeaderRepository(PayrollContext context) : base(context)
        {
        }

        public async Task<BankFileHeader> GetByOrganizationOrCreateAsync(int organizationID,
            int userId,
            string companyCode = "0",
            string batchNo = "0")
        {
            var bankFileHeader = await _context.BankFileHeaders
                .AsNoTracking()
                .FirstOrDefaultAsync(t => t.OrganizationID == organizationID);

            if (bankFileHeader == null)
            {
                var newBankFileHeader = BankFileHeader.NewBankFileHeader(organizationID,
                    userId: userId,
                    companyCode: companyCode,
                    batchNo: batchNo);

                await SaveAsync(newBankFileHeader);

                return newBankFileHeader;
            }

            return bankFileHeader;
        }
    }
}
