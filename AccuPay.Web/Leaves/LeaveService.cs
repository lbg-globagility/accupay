using AccuPay.Core.Entities;
using AccuPay.Core.Exceptions;
using AccuPay.Core.Helpers;
using AccuPay.Core.Interfaces;
using AccuPay.Web.Core.Auth;
using AccuPay.Web.Leaves.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static AccuPay.Core.ReportModels.PaystubPayslipModel;

namespace AccuPay.Web.Leaves
{
    public class LeaveService
    {
        // Sentinel times used for the boundary days of a multi-day self-service leave filing:
        // the first day runs from its given start time through end of day, and the last day
        // runs from start of day through its given end time. Days in between are whole-day.
        private static readonly TimeSpan StartOfDay = TimeSpan.Zero;

        private static readonly TimeSpan EndOfDay = new TimeSpan(23, 59, 59);

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

        public async Task<List<LeaveDto>> Create(SelfServiceCreateLeaveDto dto)
        {
            var startDate = dto.StartDate.Date;
            var endDate = dto.EndDate.Date;

            if (endDate < startDate)
                throw new BusinessLogicException("End Date cannot be earlier than Start Date.");

            var filingGroupDate = DateTime.Now;

            var leaves = new List<Leave>();

            for (var date = startDate; date <= endDate; date = date.AddDays(1))
            {
                TimeSpan? dayStartTime = null;
                TimeSpan? dayEndTime = null;
                if (dto.StartTime != null)
                {
                    dayStartTime = dto.StartTime.Value.TimeOfDay;
                }
                if (dto.EndTime != null)
                {
                    dayEndTime = dto.EndTime.Value.TimeOfDay;
                }



                var leave = NewSelfServiceLeave(dto, date, dayStartTime, dayEndTime, filingGroupDate);

                leaves.Add(leave);
            }

            await _dataService.SaveManyAsync(leaves, _currentUser.UserId);

            return leaves.Select(x => ConvertToDto(x)).ToList();
        }

        private Leave NewSelfServiceLeave(SelfServiceCreateLeaveDto dto, DateTime date, TimeSpan? startTime, TimeSpan? endTime, DateTime filingGroupDate)
        {
            return new Leave()
            {
                EmployeeID = _currentUser.EmployeeId,
                OrganizationID = _currentUser.OrganizationId,
                LeaveType = dto.LeaveType,
                StartDate = date,
                EndDate = date,
                StartTime = startTime,
                EndTime = endTime,
                Reason = dto.Reason,
                Status = Leave.StatusPending,
                FilingGroupDate = filingGroupDate
            };
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

        public async Task<LeaveDto> ApproveFiling(int id, string approverEmail)
        {
            return await SetFilingStatus(id, Leave.StatusApproved, approverEmail);
        }

        public async Task<LeaveDto> RejectFiling(int id)
        {
            return await SetFilingStatus(id, Leave.StatusRejected);
        }

        private async Task<LeaveDto> SetFilingStatus(int id, string status, string approverEmail = null)
        {
            var leave = await _leaveRepository.GetByIdWithEmployeeAsync(id);
            if (leave == null)
                throw new System.Exception("Leave filing not found.");

            if (leave.Status == status)
                throw new System.Exception($"Leave filing already {status.ToLowerInvariant()}.");

            if (leave.Status != Leave.StatusPending)
                throw new System.Exception($"Only pending leave filings can be {status.ToLowerInvariant()}.");

            leave.Status = status;

            if (status == Leave.StatusApproved)
                leave.ApproverEmail = approverEmail;

            if (_currentUser.UserId > 0)
            {
                // Authenticated approval/rejection: go through the normal audited save
                // (stamps LastUpdBy and records a UserActivity entry).
                await _dataService.SaveAsync(leave, _currentUser.UserId);
            }
            else
            {
                // Anonymous email-link approval/rejection: there is no attributable user,
                // so bypass the audited pipeline and leave LastUpdBy null (the column/FK
                // support null; ApproverEmail already records who approved it).
                leave.LastUpdBy = null;
                await _leaveRepository.UpdateApprovalAsync(leave);
            }

            return ConvertToDto(leave);
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
                Comments = leave.Comments,
                ApproverEmail = leave.ApproverEmail,
                CreatedBy = leave.CreatedBy,
                LastUpd = leave.LastUpd,
                Created = leave.Created,
                LastUpdBy = leave.LastUpdBy,
                IsNotifyEmail = leave.IsNotifyEmail,
                FilingGroupDate = leave.FilingGroupDate
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
        public async Task<List<EmployeeLeaveBalanceDto>> GetLeaveBalanceAsync(int employeeId)
        {
            var leaveLedgers = await _leaveLedgerRepository.GetAllByEmployeeAndLeaveType(employeeId);
            var list =new List<EmployeeLeaveBalanceDto>();
            leaveLedgers.ToList().ForEach(x =>
            {
                var dto = new EmployeeLeaveBalanceDto
                {
                    Balance =x.LastTransaction!=null ? x.LastTransaction.Balance: 0,
                    LeaveType = x.Product.PartNo
                };
                list.Add(dto);
            });
             
            return list;
        }
       
    }
}
