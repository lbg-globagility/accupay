using AccuPay.Data.Entities;
using System;

namespace AccuPay.Web.Employees.Models
{
    public class EmployeeDto : BaseEmployeeDto
    {
        public int? Id { get; set; }
        public int? PayFrequencyID { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Tin { get; set; }
        public string SssNo { get; set; }
        public string PagIbigNo { get; set; }
        public string PhilHealthNo { get; set; }
        public string EmploymentStatus { get; set; }
        public string EmailAddress { get; set; }
        public string WorkPhone { get; set; }
        public string LandlineNo { get; set; }
        public string MobileNo { get; set; }
        public string Address { get; set; }
        public string Gender { get; set; }
        public string EmployeeType { get; set; }
        public string MaritalStatus { get; set; }
        public DateTime Birthdate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? TerminationDate { get; set; }
        public int? NoOfDependents { get; set; }
        public bool NewEmployeeFlag { get; set; }
        public bool AlphalistExempted { get; set; }
        public int? DayOfRest { get; set; }
        public string AtmNo { get; set; }
        public string BankName { get; set; }
        public DateTime? DateRegularized { get; set; }
        public DateTime? DateEvaluated { get; set; }
        public bool RevealInPayroll { get; set; }
        public int? AgencyID { get; set; }
        public int AdvancementPoints { get; set; }
        public decimal BPIInsurance { get; set; }
        public EmploymentPolicyDto EmploymentPolicy { get; set; }
        public PositionDto Position { get; set; }

        public static EmployeeDto Convert(Employee employee)
        {
            var dto = new EmployeeDto();
            if (employee == null) return dto;

            dto.ApplyData(employee);

            return dto;
        }

        protected override void ApplyData(Employee employee)
        {
            if (employee == null) return;

            base.ApplyData(employee);

            Id = employee.RowID;
            PayFrequencyID = employee.PayFrequencyID;
            FirstName = employee.FirstName;
            MiddleName = employee.MiddleName;
            LastName = employee.LastName;
            Tin = employee.TinNo;
            SssNo = employee.SssNo;
            PhilHealthNo = employee.PhilHealthNo;
            PagIbigNo = employee.HdmfNo;
            EmploymentStatus = employee.EmploymentStatus;
            EmailAddress = employee.EmailAddress;
            WorkPhone = employee.WorkPhone;
            LandlineNo = employee.HomePhone;
            MobileNo = employee.MobilePhone;
            Address = employee.HomeAddress;
            Gender = employee.Gender;
            EmployeeType = employee.EmployeeType;
            MaritalStatus = employee.MaritalStatus;
            Birthdate = employee.BirthDate;
            StartDate = employee.StartDate;
            TerminationDate = employee.TerminationDate;
            NoOfDependents = employee.NoOfDependents;
            AlphalistExempted = employee.AlphalistExempted;
            DayOfRest = employee.DayOfRest;
            AtmNo = employee.AtmNo;
            BankName = employee.BankName;
            DateRegularized = employee.DateRegularized;
            DateEvaluated = employee.DateEvaluated;
            AgencyID = employee.AgencyID;
            AdvancementPoints = employee.AdvancementPoints;
            BPIInsurance = employee.BPIInsurance;

            if (employee.EmploymentPolicy != null)
            {
                EmploymentPolicy = new EmploymentPolicyDto()
                {
                    Id = employee.EmploymentPolicy.Id,
                    Name = employee.EmploymentPolicy.Name,
                };
            }

            if (employee.Position != null)
            {
                Position = new PositionDto()
                {
                    Id = employee.Position.RowID.Value,
                    Name = employee.Position.Name
                };
            }
        }

        public class EmploymentPolicyDto
        {
            public int Id { get; set; }

            public string Name { get; set; }
        }

        public class PositionDto
        {
            public int Id { get; set; }

            public string Name { get; set; }
        }
    }
}
