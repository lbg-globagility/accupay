using AccuPay.Data.Entities;
using System.Collections.Generic;
using System.Linq;

namespace AccuPay.Web.EmploymentPolicies
{
    /// <summary>
    /// Collection of employment policy types to assist with searching for type
    /// </summary>
    public class EmploymentPolicyTypesCollection
    {
        private ICollection<EmploymentPolicyType> PolicyTypes { get; set; }

        public EmploymentPolicyTypesCollection(ICollection<EmploymentPolicyType> policyType)
        {
            PolicyTypes = policyType;
        }

        public EmploymentPolicyType Find(string policyTypeName)
        {
            return PolicyTypes.FirstOrDefault(t => t.Name == policyTypeName);
        }
    }
}
