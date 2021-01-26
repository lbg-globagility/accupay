using AccuPay.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface ILoanPaymentFromBonusRepository
    {
        Task<ICollection<LoanPaymentFromBonus>> GetByBonusIdAsync(int bonusId);

        Task SaveManyAsync(List<LoanPaymentFromBonus> loanPaymentFromBonuses);
    }
}
