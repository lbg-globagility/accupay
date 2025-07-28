using AccuPay.Core.Entities;
using AccuPay.Core.Interfaces;
using System.Threading.Tasks;

namespace AccuPay.Infrastructure.Data
{
    public class BankFileHeaderDataService : BaseSavableDataService<BankFileHeader>, IBankFileHeaderDataService
    {
        private const string ENTITY_NAME = "BankFileHeader";
        private readonly IBankFileHeaderRepository _bankFileHeaderRepository;

        public BankFileHeaderDataService(IBankFileHeaderRepository bankFileHeaderRepository,
            IPayPeriodRepository payPeriodRepository,
            PayrollContext context,
            IPolicyHelper policy,
            string entityName,
            string entityNamePlural = null) : base(bankFileHeaderRepository,
                payPeriodRepository,
                context,
                policy,
                entityName: ENTITY_NAME,
                entityNamePlural: $"{ENTITY_NAME}s")
        {
            _bankFileHeaderRepository = bankFileHeaderRepository;
        }

        public async Task<BankFileHeader> GetByOrganizationOrCreateAsync(int organizationID,
            int userId,
            string companyCode = "0",
            string fundingAccountNo = "0",
            string presentingOfficeNo = "0",
            string batchNo = "0") => await _bankFileHeaderRepository.GetByOrganizationOrCreateAsync(organizationID, userId: userId);
    }
}
