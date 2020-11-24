using AccuPay.Data.Entities;
using AccuPay.Data.Exceptions;
using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using AccuPay.Data.Services.Imports.OfficialBusiness;
using AccuPay.Utilities.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Services
{
    public class OfficialBusinessDataService : BaseDailyPayrollDataService<OfficialBusiness>
    {
        private readonly OfficialBusinessRepository _officialBusinessRepository;

        public OfficialBusinessDataService(
            OfficialBusinessRepository officialBusinessRepository,
            PayPeriodRepository payPeriodRepository,
            PayrollContext context,
            PolicyHelper policy) :

            base(officialBusinessRepository,
                payPeriodRepository,
                context,
                policy,
                entityName: "Official Business",
                entityNamePlural: "Official Businesses")
        {
            _officialBusinessRepository = officialBusinessRepository;
        }

        protected override async Task SanitizeEntity(OfficialBusiness officialBusiness, OfficialBusiness oldOfficialBusiness)
        {
            if (officialBusiness.OrganizationID == null)
                throw new BusinessLogicException("Organization is required.");

            if (officialBusiness.EmployeeID == null)
                throw new BusinessLogicException("Employee is required.");

            if (officialBusiness.StartDate == null)
                throw new BusinessLogicException("Start Date is required.");

            if (officialBusiness.StartDate < PayrollTools.SqlServerMinimumDate)
                throw new BusinessLogicException("Date cannot be earlier than January 1, 1753");

            if (officialBusiness.StartTime == null)
                throw new BusinessLogicException("Start Time is required.");

            if (officialBusiness.EndTime == null)
                throw new BusinessLogicException("End Time is required.");

            if (new string[] { OfficialBusiness.StatusPending, OfficialBusiness.StatusApproved }
                            .Contains(officialBusiness.Status) == false)
            {
                throw new BusinessLogicException("Status is not valid.");
            }

            var doesExistQuery = _context.OfficialBusinesses
                .Where(l => l.EmployeeID == officialBusiness.EmployeeID)
                .Where(l => l.StartDate.Value.Date == officialBusiness.StartDate.Value.Date);

            if (officialBusiness.IsNewEntity == false)
            {
                doesExistQuery = doesExistQuery.Where(l => officialBusiness.RowID != l.RowID);
            }

            if (await doesExistQuery.AnyAsync())
                throw new BusinessLogicException($"Employee already has an OB for {officialBusiness.StartDate.Value.ToShortDateString()}");

            officialBusiness.StartTime = officialBusiness.StartTime.Value.StripSeconds();
            officialBusiness.EndTime = officialBusiness.EndTime.Value.StripSeconds();

            if (officialBusiness.StartTime == officialBusiness.EndTime)
                throw new BusinessLogicException("End Time cannot be equal to Start Time");

            officialBusiness.UpdateEndDate();
        }

        public async Task<List<OfficialBusiness>> BatchApply(IReadOnlyCollection<OfficialBusinessImportModel> validRecords, int organizationId, int userId)
        {
            List<OfficialBusiness> officialBusinesses = new List<OfficialBusiness>();

            foreach (var ob in validRecords)
            {
                officialBusinesses.Add(new OfficialBusiness()
                {
                    CreatedBy = userId,
                    EmployeeID = ob.EmployeeID,
                    OrganizationID = organizationId,
                    EndTimeFull = ob.EndTime.Value,
                    StartDate = ob.StartDate.Value,
                    StartTimeFull = ob.StartTime.Value,
                    Status = Overtime.StatusPending
                });
            }

            await _officialBusinessRepository.SaveManyAsync(officialBusinesses);

            return officialBusinesses;
        }
    }
}
