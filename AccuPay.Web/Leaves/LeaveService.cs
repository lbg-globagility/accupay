using AccuPay.Core.Entities;
using AccuPay.Core.Exceptions;
using AccuPay.Core.Helpers;
using AccuPay.Core.Interfaces;
using AccuPay.Web.Core.Auth;
using AccuPay.Web.Leaves.Models;
using Microsoft.VisualBasic;
using OfficeOpenXml.FormulaParsing.Excel.Functions.DateTime;
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
            if (leave == null) return null;

            var dto = ConvertToDto(leave);
            var filingGroup = await GetFilingGroupAsync(leave);
            dto.DateTimes = filingGroup.Select(x => x.StartDate).ToList();

            return dto;
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
            ValidateSelfServiceDto(dto);

            var leaves = new List<Leave>();
            var filingGroupDate = DateTime.Now;
            if (dto.LeaveTiming == SelfServiceCreateLeaveDto.TimingHour)
            {
              var leave = NewSelfServiceLeave(dto, dto.StartDate, dto.StartTime.Value.TimeOfDay, dto.EndTime.Value.TimeOfDay, filingGroupDate);
              leaves.Add(leave);
              await _dataService.SaveAsync(leave, _currentUser.UserId);
            }
            else if(dto.LeaveTiming == SelfServiceCreateLeaveDto.TimingDay)
            {
                var startDate = dto.DateTimes;
                

                foreach (var date in dto.DateTimes)
                {
                  
                    var leave = NewSelfServiceLeave(dto, date, null, null, filingGroupDate);

                    leaves.Add(leave);
                }

                await _dataService.SaveManyAsync(leaves, _currentUser.UserId);


            }

            var dateTimes = leaves.Select(x => x.StartDate).ToList();
            return leaves.Select(x =>
            {
                var dto = ConvertToDto(x);
                dto.DateTimes = dateTimes;
                return dto;
            }).ToList();
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

        public async Task<List<LeaveDto>> UpdateSelfService(int id, SelfServiceCreateLeaveDto dto)
        {
            var leave = await _leaveRepository.GetByIdWithEmployeeAsync(id);
            if (leave == null || leave.EmployeeID != _currentUser.EmployeeId) return null;

            if (leave.Status != Leave.StatusPending)
                throw new Exception("Only pending leave filings can be edited.");
            if (leave.IsNotifyEmail)
                throw new Exception("Emailed filings can no longer be edited.");

            ValidateSelfServiceDto(dto);

            var filingGroupDate = leave.FilingGroupDate ?? DateTime.Now;
            var filingGroup = await GetFilingGroupAsync(leave);

            var leaves = new List<Leave>();
            if (dto.LeaveTiming == SelfServiceCreateLeaveDto.TimingHour)
            {
                var newLeave = NewSelfServiceLeave(dto, dto.StartDate, dto.StartTime.Value.TimeOfDay, dto.EndTime.Value.TimeOfDay, filingGroupDate);
                leaves.Add(newLeave);
            }
            else if (dto.LeaveTiming == SelfServiceCreateLeaveDto.TimingDay)
            {
                foreach (var date in dto.DateTimes)
                {
                    var newLeave = NewSelfServiceLeave(dto, date, null, null, filingGroupDate);
                    leaves.Add(newLeave);
                }
            }

            // The old filing group is deleted and the replacement is created in a single
            // transaction: the old rows still hold these dates, so create-then-delete would
            // trip SanitizeEntity's duplicate-date check, and delete-then-create without a
            // transaction risks losing the request entirely if the create half fails.
            await _dataService.ReplaceSelfServiceFilingGroupAsync(filingGroup, leaves, _currentUser.UserId);

            var dateTimes = leaves.Select(x => x.StartDate).ToList();
            return leaves.Select(x =>
            {
                var dto = ConvertToDto(x);
                dto.DateTimes = dateTimes;
                return dto;
            }).ToList();
        }

        private static void ValidateSelfServiceDto(SelfServiceCreateLeaveDto dto)
        {
            if (dto.LeaveTiming != SelfServiceCreateLeaveDto.TimingDay &&
                dto.LeaveTiming != SelfServiceCreateLeaveDto.TimingHour)
                throw new BusinessLogicException("Leave timing must be 'Day' or 'Hour'.");

            if (dto.LeaveTiming == SelfServiceCreateLeaveDto.TimingDay &&
                (dto.DateTimes == null || !dto.DateTimes.Any()))
                throw new BusinessLogicException("At least one date is required.");
        }

        public async Task<bool> DeleteSelfService(int id)
        {
            var leave = await _leaveRepository.GetByIdWithEmployeeAsync(id);
            if (leave == null || leave.EmployeeID != _currentUser.EmployeeId) return false;

            if (leave.Status != Leave.StatusPending)
                throw new Exception("Only pending leave filings can be deleted.");
            if (leave.IsNotifyEmail)
                throw new Exception("Emailed filings can no longer be deleted.");

            var filingGroupDate = leave.FilingGroupDate ?? DateTime.Now;
            var filingGroup = await GetFilingGroupAsync(leave);
            foreach (var groupLeave in filingGroup)
            {
                await _dataService.DeleteAsync(
                    id: groupLeave.RowID.Value,
                    currentlyLoggedInUserId: _currentUser.UserId);
            }

            return true;
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

            // Multi-day self-service filings are saved as one Leave row per day, all sharing
            // the same FilingGroupDate. Approving/rejecting one row should approve/reject the
            // whole request.
            var filingGroup = await GetFilingGroupAsync(leave);
            var pendingInGroup = filingGroup.Where(l => l.Status == Leave.StatusPending).ToList();

            foreach (var groupLeave in pendingInGroup)
            {
                groupLeave.Status = status;

                if (status == Leave.StatusApproved)
                    groupLeave.ApproverEmail = approverEmail;
            }

            if (_currentUser.UserId > 0)
            {
                // Authenticated approval/rejection: go through the normal audited save
                // (stamps LastUpdBy and records a UserActivity entry).
                await _dataService.SaveManyAsync(pendingInGroup, _currentUser.UserId);
            }
            else
            {
                // Anonymous email-link approval/rejection: there is no attributable user,
                // so bypass the audited pipeline and leave LastUpdBy null (the column/FK
                // support null; ApproverEmail already records who approved it).
                pendingInGroup.ForEach(l => l.LastUpdBy = null);
                await _leaveRepository.UpdateApprovalAsync(pendingInGroup);
            }

            return ConvertToDto(leave);
        }

        private async Task<List<Leave>> GetFilingGroupAsync(Leave leave)
        {
            if (!leave.FilingGroupDate.HasValue || leave.EmployeeID == null)
            {
                return new List<Leave> { leave };
            }

            var groupLeaves = await _leaveRepository.GetByFilingGroupDateAsync(
                leave.FilingGroupDate.Value, leave.EmployeeID.Value);

            return groupLeaves.Any() ? groupLeaves.ToList() : new List<Leave> { leave };
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
