using AccuPay.Core.Entities;
using AccuPay.Core.Helpers;
using AccuPay.Core.Interfaces;
using AccuPay.Web.Core.Auth;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Web.Leaves
{
    public class LeaveService
    {
        private readonly ILeaveRepository _leaveRepository;
        private readonly IProductRepository _productRepository;
        private readonly ILeaveLedgerRepository _leaveLedgerRepository;
        private readonly ILeaveDataService _dataService;
        private readonly ICurrentUser _currentUser;

        public LeaveService(
            ILeaveRepository leaveRepository,
            IProductRepository productRepository,
            ILeaveLedgerRepository leaveLedgerRepository,
            ILeaveDataService dataService,
            ICurrentUser currentUser)
        {
            _leaveRepository = leaveRepository;
            _productRepository = productRepository;
            _leaveLedgerRepository = leaveLedgerRepository;
            _dataService = dataService;
            _currentUser = currentUser;
        }

        public async Task<PaginatedList<LeaveDto>> PaginatedList(LeavePageOptions options)
        {
            // TODO: sort and desc in repository
            var paginatedList = await _leaveRepository.GetPaginatedListAsync(
                options,
                _currentUser.OrganizationId);

            return paginatedList.Select(x => ConvertToDto(x));
        }

        public async Task<PaginatedList<LeaveBalanceDto>> GetLeaveBalance(PageOptions options, string searchTerm)
        {
            var paginatedList = await _dataService.GetLeaveBalancesAsync(
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
            var paginatedList = await _leaveLedgerRepository.ListTransactionsAsync(
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
                OrganizationID = _currentUser.OrganizationId,
            };
            ApplyChanges(dto, leave);

            await _dataService.SaveAsync(leave, _currentUser.UserId);

            return ConvertToDto(leave);
        }

        public async Task<LeaveDto> Create(SelfServiceCreateLeaveDto dto)
        {
            var leave = new Leave()
            {
                EmployeeID = _currentUser.EmployeeId,
                OrganizationID = _currentUser.OrganizationId,
            };

            leave.LeaveType = dto.LeaveType;
            leave.StartDate = dto.StartDate;
            leave.StartTime = dto.StartTime?.TimeOfDay;
            leave.EndTime = dto.EndTime?.TimeOfDay;
            leave.Reason = dto.Reason;
            leave.Status = Leave.StatusPending;

            await _dataService.SaveAsync(leave, _currentUser.UserId);

            return ConvertToDto(leave);
        }

        public async Task<LeaveDto> Update(int id, UpdateLeaveDto dto)
        {
            var leave = await _leaveRepository.GetByIdAsync(id);
            if (leave == null) return null;

            ApplyChanges(dto, leave);

            await _dataService.SaveAsync(leave, _currentUser.UserId);

            return ConvertToDto(leave);
        }

        public async Task Delete(int id)
        {
            await _dataService.DeleteAsync(
                id: id,
                currentlyLoggedInUserId: _currentUser.UserId);
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
