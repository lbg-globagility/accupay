using AccuPay.Reference;
using System.Linq;

namespace AccuPay.Data
{
    public class SystemOwnerService : BaseSystemOwner
    {
        public override string CurrentSystemOwner
        {
            get
            {
                using (var context = new PayrollContext())
                {
                    return context.SystemOwners.
                            Where(x => x.IsCurrentOwner == "1").
                            Select(x => x.Name).
                            FirstOrDefault();
                }
            }
        }
    }
}