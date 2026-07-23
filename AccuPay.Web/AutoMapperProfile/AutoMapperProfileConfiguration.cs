using AccuPay.Core.Entities;
using AccuPay.Web.Appraisers;
using AccuPay.Web.Employees.Models;
using AutoMapper;

namespace AccuPay.Web.AutoMapperProfile
{
    public class AutoMapperProfileConfiguration : Profile
    {
        public AutoMapperProfileConfiguration() : this("My Profile")
        {
        }

        private AutoMapperProfileConfiguration(string profileName) : base(profileName)
        {
            CreateMap<Approver, ApproverDto>();
            CreateMap<EmployeeApprover, ApproverDto.EmployeeApproversDto>();
            CreateMap<Employee, ApproverDto.EmployeeDto>();
        }
    }
}

