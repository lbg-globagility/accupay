using AccuPay.Data.Entities;
using AccuPay.Data.Exceptions;
using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using AccuPay.Data.Services.Imports.Employees;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Services
{
    public class EmployeeDataService
    {
        private readonly EmployeeRepository _employeeRepository;
        private readonly LeaveLedgerRepository _leaveLedgerRepository;
        private readonly ProductRepository _productRepository;

        public EmployeeDataService(EmployeeRepository employeeRepository, LeaveLedgerRepository leaveLedgerRepository, ProductRepository productRepository)
        {
            _employeeRepository = employeeRepository;
            _leaveLedgerRepository = leaveLedgerRepository;
            _productRepository = productRepository;
        }

        public async Task ImportAsync(ICollection<EmployeeWithLeaveBalanceData> employeeWithLeaveBalanceModels, int organizationId, int userId)
        {
            var vacationLeaveProduct = await _productRepository.
                    GetOrCreateLeaveTypeAsync(ProductConstant.VACATION_LEAVE,
                                                organizationId: organizationId,
                                                userId: userId);

            var sickLeaveProduct = await _productRepository.
                    GetOrCreateLeaveTypeAsync(ProductConstant.SICK_LEAVE,
                                                organizationId: organizationId,
                                                userId: userId);

            if (vacationLeaveProduct?.RowID == null || sickLeaveProduct?.RowID == null)
                throw new BusinessLogicException("Error accessing leave type data.");

            var employees = employeeWithLeaveBalanceModels.Select(x => x.Employee).ToList();

            await _employeeRepository.SaveManyAsync(employees);

            foreach (var model in employeeWithLeaveBalanceModels)
            {
                // vacation leave balance
                await _leaveLedgerRepository.CreateBeginningBalanceAsync(
                    employeeId: model.Employee.RowID.Value,
                    leaveTypeId: vacationLeaveProduct.RowID.Value,
                    userId: userId,
                    organizationId: organizationId,
                    balance: model.VacationLeaveBalance);

                // sick leave balance
                await _leaveLedgerRepository.CreateBeginningBalanceAsync(
                    employeeId: model.Employee.RowID.Value,
                    leaveTypeId: sickLeaveProduct.RowID.Value,
                    userId: userId,
                    organizationId: organizationId,
                    balance: model.SickLeaveBalance);
            }
        }

        public async Task<List<Employee>> BatchApply(IReadOnlyCollection<EmployeeImportModel> validRecords, int organizationId, int userId)
        {
            var added = validRecords.Where(e => !e.Employee.RowID.HasValue).Select(e => e.Employee).ToList();
            added.ForEach(e =>
            {
                e.OrganizationID = organizationId;
                e.CreatedBy = userId;
            });

            var updated = validRecords.Where(e => e.Employee.RowID.HasValue).Select(e => e.Employee).ToList();
            updated.ForEach(e =>
            {
                e.LastUpdBy = userId;
            });

            await _employeeRepository.ChangeManyAsync(added: added, updated: updated);

            List<Employee> employees = new List<Employee>();
            if (added.Any()) employees.AddRange(added);
            if (updated.Any()) employees.AddRange(updated);

            return employees;
        }
    }
}