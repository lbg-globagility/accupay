using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace AccuPay.Data.Entities
{
    [Table("employmentpolicy")]
    public class EmploymentPolicy
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public ICollection<EmploymentPolicyItem> Items { get; private set; }

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
                var newItem = new EmploymentPolicyItem()
                {
                    EmploymentPolicyTypeId = type.Id,
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
    }
}
