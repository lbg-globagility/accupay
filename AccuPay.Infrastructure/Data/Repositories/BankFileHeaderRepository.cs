using AccuPay.Core.Entities;
using AccuPay.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace AccuPay.Infrastructure.Data
{
    public class BankFileHeaderRepository : SavableRepository<BankFileHeader>, IBankFileHeaderRepository
    {
        public BankFileHeaderRepository(PayrollContext context) : base(context)
        {
        }

        public async Task<BankFileHeader> GetByOrganizationOrCreateAsync(int organizationID,
            int userId,
            string companyCode = "0",
            string fundingAccountNo = "0",
            string presentingOfficeNo = "0",
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
                    fundingAccountNo: fundingAccountNo,
                    presentingOfficeNo: presentingOfficeNo,
                    batchNo: batchNo);

                await SaveAsync(newBankFileHeader);

                return newBankFileHeader;
            }

            return bankFileHeader;
        }
    }
}
