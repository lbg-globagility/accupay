using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace AccuPay.Core.Entities
{
    [Table("contractor_project")]
    public partial class ContractorProject : OrganizationalEntity
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; } = null;

        [StringLength(255)]
        public string Description { get; set; }

        public int? ContractorId { get; set; }

        public DateTime? BeginDate { get; set; }

        public DateTime? EndDate { get; set; }

        [ForeignKey(nameof(ContractorId))]
        public virtual Contractor Contractor { get; set; }

        public virtual ICollection<ProjectEmployee> ProjectEmployees { get; set; } = new List<ProjectEmployee>();

        public string ContractorName => Contractor?.Name;

    }

    public partial class ContractorProject
    {
        private ContractorProject() { }

        public ContractorProject(int userId,
            int contractorId,
            string name,
            string description,
            DateTime? beginDate,
            DateTime? endDate)
        {
            Name = name;
            Description = description;
            ContractorId = contractorId;
            BeginDate = beginDate;
            EndDate = endDate;
            OrganizationID = 0;
            if (IsNewEntity) CreatedBy = userId;
            LastUpdBy = userId;
        }

        public static ContractorProject Create(int userId,
            int contractorId,
            string name = "Project ABC123",
            string description = "",
            DateTime? beginDate = null,
            DateTime? endDate = null)
            => new ContractorProject(userId: userId,
                contractorId: contractorId,
                name: name,
                description: description,
                beginDate: beginDate,
                endDate: endDate);

        public ContractorProject CloneFrom(int userId, ContractorProject contractorProject)
        {
            var clone = Create(userId: userId,
                contractorId: contractorProject.ContractorId ?? 0,
                name: contractorProject.Name,
                description: contractorProject.Description,
                beginDate: contractorProject.BeginDate,
                endDate: contractorProject.EndDate);

            clone.RowID = contractorProject.RowID;

            return clone;
        }

        public ProjectEmployee FindEmployeeById(int id)
            => ProjectEmployees?.FirstOrDefault(t => t.EmployeeId == id);
    }
}
