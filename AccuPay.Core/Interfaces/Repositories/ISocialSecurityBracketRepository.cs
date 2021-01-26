using AccuPay.Core.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface ISocialSecurityBracketRepository
    {
        IEnumerable<SocialSecurityBracket> GetAll();

        Task<IEnumerable<SocialSecurityBracket>> GetByTimePeriodAsync(DateTime taxEffectivityDate);
    }
}
