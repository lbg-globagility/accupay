using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using AccuPay.Data.Services;
using AccuPay.Web.Core.Auth;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Web.Leaves
{
    public class LeaveService
    {
        private readonly LeaveRepository _leaveRepository;
        private readonly ProductRepository _productRepository;
        private readonly LeaveDataService _service;
        private readonly ICurrentUser _currentUser;

        public LeaveService(
            LeaveRepository leaveRepository,
            ProductRepository productRepository,
            LeaveDataService service,
            ICurrentUser currentUser)
        {
            _leaveRepository = leaveRepository;
            _productRepository = productRepository;
            _service = service;
            _currentUser = currentUser;
        }

        public async Task<PaginatedList<LeaveDto>> PaginatedList(PageOptions options, LeaveFilter filter)
        {
            // TODO: sort and desc in repository
            var paginatedList = await _leaveRepository.GetPaginatedListAsync(
                options,
                _currentUser.OrganizationId,
                filter.Term,
                filter.DateFrom,
                filter.DateTo);

            return paginatedList.Select(x => ConvertToDto(x));
        }

        public async Task<PaginatedList<LeaveBalanceDto>> GetLeaveBalance(PageOptions options, string searchTerm)
        {
            var paginatedList = await _service.GetLeaveBalances(
                options,
                _currentUser.OrganizationId,
                searchTerm);

            var dtos = paginatedList.Items.GroupBy(x => x.EmployeeID).Select(x => new LeaveBalanceDto
            {
                EmployeeId = x.Key,
                Id = x.FirstOrDefault().EmployeeID.Value,
                EmployeeName = x.FirstOrDefault().LastTransaction?.Employee?.FullNameWithMiddleInitialLastNameFirst,
                EmployeeNumber = x.FirstOrDefault().LastTransaction?.Employee?.EmployeeNo,
                EmployeeType = x.FirstOrDefault().LastTransaction?.Employee?.EmployeeType,
                SickLeave = x.FirstOrDefault(y => y.Product.PartNo == ProductConstant.SICK_LEAVE)?.LastTransaction.Balance ?? 0,
                VacationLeave = x.FirstOrDefault(y => y.Product.PartNo == ProductConstant.VACATION_LEAVE)?.LastTransaction.Balance ?? 0
            }).ToList();

            return new PaginatedList<LeaveBalanceDto>(dtos, paginatedList.TotalCount);
        }

        public async Task<PaginatedList<LeaveTransactionDto>> ListTransactions(PageOptions options, int id, string type)
        {
            var paginatedList = await _service.ListTransactions(
                options,
                _currentUser.OrganizationId,
                id,
                type);

            return paginatedList.Select(x => ConvertToLedgerDto(x));
        }

        public async Task<LeaveDto> GetById(int id)
        {
            var leave = await _leaveRepository.GetByIdWithEmployeeAsync(id);

            return ConvertToDto(leave);
        }

        public async Task<LeaveDto> Create(CreateLeaveDto dto)
        {
            var leave = new Leave()
            {
                EmployeeID = dto.EmployeeId,
                CreatedBy = _currentUser.DesktopUserId,
                OrganizationID = _currentUser.OrganizationId,
            };
            ApplyChanges(dto, leave);

            await _service.SaveAsync(leave);

            return ConvertToDto(leave);
        }

        public async Task<LeaveDto> Update(int id, UpdateLeaveDto dto)
        {
            var leave = await _leaveRepository.GetByIdAsync(id);
            if (leave == null) return null;

            leave.LastUpdBy = _currentUser.DesktopUserId;

            ApplyChanges(dto, leave);

            await _service.SaveAsync(leave);

            return ConvertToDto(leave);
        }

        public async Task Delete(int id)
        {
            await _service.DeleteAsync(id);
        }

        public async Task<List<string>> GetLeaveTypes()
        {
            var leaveTypes = await _productRepository.GetLeaveTypesAsync(_currentUser.OrganizationId);

            return leaveTypes
                .Where(x => !string.IsNullOrWhiteSpace(x.PartNo))
                .Select(x => x.PartNo)
                .ToList();
        }

        private static void ApplyChanges(CrudLeaveDto dto, Leave leave)
        {
            leave.LeaveType = dto.LeaveType;
            leave.Status = dto.Status;
            leave.StartDate = dto.StartDate;
            leave.StartTime = dto.StartTime?.TimeOfDay;
            leave.EndTime = dto.EndTime?.TimeOfDay;
            leave.Reason = dto.Reason;
            leave.Comments = dto.Comments;
        }

        private static LeaveDto ConvertToDto(Leave leave)
        {
            if (leave == null) return null;

            return new LeaveDto()
            {
                Id = leave.RowID.Value,
                EmployeeId = leave.EmployeeID,
                EmployeeNumber = leave.Employee?.EmployeeNo,
                EmployeeName = leave.Employee?.FullNameWithMiddleInitialLastNameFirst,
                EmployeeType = leave.Employee?.EmployeeType,
                LeaveType = leave.LeaveType,
                StartTime = leave.StartTimeFull,
                EndTime = leave.EndTimeFull,
                StartDate = leave.StartDate,
                EndDate = leave.ProperEndDate,
                Status = leave.Status,
                Reason = leave.Reason,
                Comments = leave.Comments
            };
        }

        private static LeaveTransactionDto ConvertToLedgerDto(LeaveTransaction transaction)
        {
            if (transaction == null) return null;

            return new LeaveTransactionDto()
            {
                Id = transaction.RowID.Value,
                EmployeeId = transaction.EmployeeID,
                EmployeeNo = transaction.Employee?.EmployeeNo,
                EmployeeName = transaction.Employee?.FullNameWithMiddleInitialLastNameFirst,
                EmployeeType = transaction.Employee?.EmployeeType,
                Description = transaction.Description,
                TransactionType = transaction.Type,
                Date = transaction.TransactionDate,
                Amount = transaction.Amount,
                Balance = transaction.Balance
            };
        }
    }
}
