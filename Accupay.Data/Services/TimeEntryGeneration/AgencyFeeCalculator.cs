using AccuPay.Data.Entities;
using AccuPay.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace AccuPay.Data.Services
{
    public class AgencyFeeCalculator
    {
        private const decimal HoursPerDay = 8;

        private readonly Employee _employee;
        private readonly Agency _agency;
        private readonly IList<AgencyFee> _agencyFees;

        public AgencyFeeCalculator(Employee employee, Agency agency, IList<AgencyFee> agencyFees)
        {
            _employee = employee;
            _agency = agency;
            _agencyFees = agencyFees;
        }

        public IList<AgencyFee> Compute(IList<TimeEntry> timeEntries)
        {
            var agencyFees = new List<AgencyFee>();

            foreach (var timeEntry in timeEntries)
            {
                var agencyFee = _agencyFees.SingleOrDefault(a => a.Date == timeEntry.Date);

                if (agencyFee == null)
                {
                    agencyFee = new AgencyFee()
                    {
                        OrganizationID = _employee.OrganizationID,
                        EmployeeID = _employee.RowID,
                        AgencyID = _agency.RowID,
                        Date = timeEntry.Date
                    };
                }

                var basicHours = AccuMath.CommercialRound(timeEntry.BasicHours);
                var hourlyFee = _agency.Fee / HoursPerDay;
                agencyFee.Amount = basicHours * hourlyFee;

                agencyFees.Add(agencyFee);
            }

            return agencyFees;
        }
    }
}