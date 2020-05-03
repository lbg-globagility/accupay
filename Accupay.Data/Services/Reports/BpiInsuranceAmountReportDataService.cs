using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Services
{
    public class BpiInsuranceAmountReportDataService
    {
        private readonly PayPeriodRepository _payPeriodRepository;
        private readonly ProductRepository _productRepository;

        private readonly int _organizationId;
        private readonly int _userId;
        private readonly DateTime _selectedDate;

        public BpiInsuranceAmountReportDataService(int organizationId, int userId, DateTime selectedDate)
        {
            _payPeriodRepository = new PayPeriodRepository();
            _productRepository = new ProductRepository();

            _organizationId = organizationId;
            _userId = userId;
            _selectedDate = selectedDate;
        }

        public async Task<IEnumerable<BpiInsuranceDataSource>> GetData()
        {
            var bpiInsuranceProductID = (await (_productRepository.
                                                GetOrCreateAdjustmentTypeAsync(
                                                        ProductConstant.BPI_INSURANCE_ADJUSTMENT,
                                                        organizationId: _organizationId,
                                                        userId: _userId)))?.RowID;

            if (bpiInsuranceProductID.HasValue == false)
                throw new Exception("Cannot get BPI Insurance data.");

            var periods = (await _payPeriodRepository.GetByMonthYearAndPayPrequencyAsync(
                                    organizationId: _organizationId,
                                    month: _selectedDate.Month,
                                    year: _selectedDate.Year,
                                    payFrequencyId: PayrollTools.PayFrequencySemiMonthlyId)).
                            ToList();

            using (var context = new PayrollContext())
            {
                var periodIDs = periods.Select(p => p.RowID.Value).ToArray();

                // TODO: move this to repository
                var adjustments = context.Adjustments.
                                            Include(a => a.Paystub.Employee).
                                            Where(a => periodIDs.Contains(a.Paystub.PayPeriodID.Value)).
                                            Where(a => bpiInsuranceProductID == a.ProductID).
                                            ToList();

                if (!adjustments.Any())
                {
                    return new List<BpiInsuranceDataSource>();
                }

                return adjustments.
                        GroupBy(a => a.Paystub.EmployeeID).
                        Select(a => ConvertToDataSource(a)).
                        OrderBy(a => a.Column2).
                        ToList();
            }
        }

        private BpiInsuranceDataSource ConvertToDataSource(IGrouping<int?, Adjustment> bpiInsuranceAdjustments)
        {
            var employee = bpiInsuranceAdjustments.FirstOrDefault().Paystub.Employee;

            BpiInsuranceDataSource result = new BpiInsuranceDataSource()
            {
                Column1 = employee.EmployeeNo,
                Column2 = employee.FullNameWithMiddleInitialLastNameFirst,
                Column3 = bpiInsuranceAdjustments.Sum(adj => adj.Amount).ToString(),
                Column4 = _selectedDate.ToShortDateString()
            };

            return result;
        }

        public class BpiInsuranceDataSource
        {
            // Employee ID
            public string Column1 { get; set; }

            // Employee Fullname
            public string Column2 { get; set; }

            // Payment/Amount
            public string Column3 { get; set; }

            // Selected Month - in a value of date
            public string Column4 { get; set; }
        }
    }
}