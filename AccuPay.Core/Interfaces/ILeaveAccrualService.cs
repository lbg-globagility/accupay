using AccuPay.Core.Entities;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface ILeaveAccrualService
    {
        Task CheckAccruals(int organizationId, int userId);

        Task ComputeAccrual(Employee employee, PayPeriod payperiod, int z_OrganizationID, int z_User);

        Task ComputeAccrual2(Employee employee, int z_OrganizationID, int z_User);
    }
}
