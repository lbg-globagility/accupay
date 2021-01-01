using AccuPay.Utilities;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace AccuPay.Core.Entities
{
    [Table("employmentpolicy")]
    public class EmploymentPolicy : IEmploymentPolicy
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<EmploymentPolicyItem> Items { get; private set; }

        public decimal WorkDaysPerYear => ObjectUtils.ToDecimal(Find("WorkDaysPerYear")?.Value);

        public int GracePeriod => ObjectUtils.ToInteger(Find("GracePeriod")?.Value);

        public bool ComputeNightDiff => ObjectUtils.ToBoolean(Find("ComputeNightDiff")?.Value);

        public bool ComputeNightDiffOT => ObjectUtils.ToBoolean(Find("ComputeNightDiffOT")?.Value);

        public bool ComputeRestDay => ObjectUtils.ToBoolean(Find("ComputeRestDay")?.Value);

        public bool ComputeRestDayOT => ObjectUtils.ToBoolean(Find("ComputeRestDayOT")?.Value);

        public bool ComputeSpecialHoliday => ObjectUtils.ToBoolean(Find("ComputeSpecialHoliday")?.Value);

        public bool ComputeRegularHoliday => ObjectUtils.ToBoolean(Find("ComputeRegularHoliday")?.Value);

        public EmploymentPolicy(string name)
            : this()
        {
            Name = name;
        }

        private EmploymentPolicy()
        {
            Items = new Collection<EmploymentPolicyItem>();
        }

        public void SetItem(EmploymentPolicyType type, string value)
        {
            if (type is null) return;

            var existingItem = Items.FirstOrDefault(t => t.EmploymentPolicyTypeId == type.Id);
            if (existingItem is null)
            {
                var newItem = new EmploymentPolicyItem(type)
                {
                    Value = value
                };

                Items.Add(newItem);
            }
            else
            {
                existingItem.Value = value;
            }
        }

        public void SetItem(EmploymentPolicyType type, decimal value)
        {
            SetItem(type, value.ToString());
        }

        public void SetItem(EmploymentPolicyType type, bool value)
        {
            SetItem(type, value.ToString());
        }

        private EmploymentPolicyItem Find(string employmentPolicyTypeName)
        {
            var item = Items.FirstOrDefault(t => t.Type.Name == employmentPolicyTypeName);
            return item;
        }
    }
}