using AccuPay.Core.Entities;
using AccuPay.Core.Helpers;
using AccuPay.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Core.Services
{
    public class BpiInsuranceAmountReportDataService
    {
        private readonly PayrollContext _context;
        private readonly IPayPeriodRepository _payPeriodRepository;
        private readonly IProductRepository _productRepository;

        public BpiInsuranceAmountReportDataService(
            PayrollContext context,
            IPayPeriodRepository payPeriodRepository,
            IProductRepository productRepository)
        {
            _context = context;
            _payPeriodRepository = payPeriodRepository;
            _productRepository = productRepository;
        }

        public async Task<IEnumerable<BpiInsuranceDataSource>> GetData(int organizationId, int userId, DateTime selectedDate)
        {
            var bpiInsuranceProductID = (await (_productRepository.
                                                GetOrCreateAdjustmentTypeAsync(
                                                        ProductConstant.BPI_INSURANCE_ADJUSTMENT,
                                                        organizationId: organizationId,
                                                        userId: userId)))?.RowID;

            if (bpiInsuranceProductID.HasValue == false)
                throw new Exception("Cannot get BPI Insurance data.");

            var periods = (await _payPeriodRepository.GetByMonthYearAndPayPrequencyAsync(
                                    organizationId: organizationId,
                                    month: selectedDate.Month,
                                    year: selectedDate.Year,
                                    payFrequencyId: PayrollTools.PayFrequencySemiMonthlyId)).
                            ToList();

            var periodIDs = periods.Select(p => p.RowID.Value).ToArray();

            // TODO: move this to repository
            var adjustments = _context.Adjustments.
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
                    Select(a => ConvertToDataSource(a, selectedDate)).
                    OrderBy(a => a.Column2).
                    ToList();
        }

        private BpiInsuranceDataSource ConvertToDataSource(IGrouping<int?, Adjustment> bpiInsuranceAdjustments, DateTime selectedDate)
        {
            var employee = bpiInsuranceAdjustments.FirstOrDefault().Paystub.Employee;

            BpiInsuranceDataSource result = new BpiInsuranceDataSource()
            {
                Column1 = employee.EmployeeNo,
                Column2 = employee.FullNameWithMiddleInitialLastNameFirst,
                Column3 = bpiInsuranceAdjustments.Sum(adj => adj.Amount).ToString(),
                Column4 = selectedDate.ToShortDateString()
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
