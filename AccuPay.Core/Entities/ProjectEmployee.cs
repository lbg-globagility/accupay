using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AccuPay.Core.Entities
{
    [Table("project_employees")]
    public partial class ProjectEmployee : OrganizationalEntity
    {
        public int? ProjectId { get; set; }

        public int? EmployeeId { get; set; }

        public DateTime? BeginDate { get; set; }

        public DateTime? EndDate { get; set; }

        [ForeignKey(nameof(ProjectId))]
        public virtual ContractorProject Project { get; set; }

        [ForeignKey(nameof(EmployeeId))]
        public virtual Employee Employee { get; set; }
    }

    public partial class ProjectEmployee
    {
        private ProjectEmployee()
        {
        }

        public ProjectEmployee(int userId,
            int? projectId,
            int? employeeId,
            DateTime? beginDate,
            DateTime? endDate)
        {
            ProjectId = projectId;
            EmployeeId = employeeId;
            BeginDate = beginDate;
            EndDate = endDate;

            OrganizationID = 0;

            if (IsNewEntity) CreatedBy = userId;

            LastUpdBy = userId;
        }

        public static ProjectEmployee Create(int userId,
            int? projectId = null,
            int? employeeId = null,
            DateTime? beginDate = null,
            DateTime? endDate = null)
            => new ProjectEmployee(userId: userId,
                projectId: projectId,
                employeeId: employeeId,
                beginDate: beginDate,
                endDate: endDate);

        public static ProjectEmployee CloneFrom(int userId,
            ProjectEmployee project)
            => Create(userId: userId,
                projectId: project?.ProjectId,
                employeeId: project?.EmployeeId,
                beginDate: project?.BeginDate,
                endDate: project?.EndDate);

        public string ProjectName => Project?.Name;

        public string OrganizationName => Employee?.OrganizationName ?? string.Empty;

        public string EmployeeName => Employee?.FullNameLastNameFirst ?? string.Empty;

        public string EmployeeNo => Employee?.EmployeeNo ?? string.Empty;

        [NotMapped]
        public bool IsSelected { get; set; }
    }
}
