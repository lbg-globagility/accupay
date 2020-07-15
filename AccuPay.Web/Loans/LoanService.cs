using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using AccuPay.Data.Services;
using AccuPay.Web.Core.Auth;
using AccuPay.Web.Shared;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Web.Loans
{
    public class LoanService
    {
        private readonly LoanDataService _loanService;
        private readonly ProductRepository _productRepository;
        private readonly ListOfValueRepository _listOfValueRepository;
        private readonly LoanRepository _loanRepository;
        private readonly ICurrentUser _currentUser;

        public LoanService(
            LoanDataService loanService,
            ProductRepository productRepository,
            ListOfValueRepository listOfValueRepository,
            LoanRepository loanRepository,
            ICurrentUser currentUser)
        {
            _loanService = loanService;
            _productRepository = productRepository;
            _listOfValueRepository = listOfValueRepository;
            _loanRepository = loanRepository;
            _currentUser = currentUser;
        }

        public async Task<PaginatedList<LoanDto>> PaginatedList(PageOptions options, string searchTerm)
        {
            // TODO: sort and desc in repository

            var paginatedList = await _loanRepository.GetPaginatedListAsync(
                options,
                _currentUser.OrganizationId,
                searchTerm);

            return paginatedList.Select(x => ConvertToDto(x));
        }

        public async Task<LoanDto> GetById(int id)
        {
            var officialBusiness = await _loanRepository.GetByIdWithEmployeeAndProductAsync(id);

            return ConvertToDto(officialBusiness);
        }

        public async Task<LoanDto> Create(CreateLoanDto dto)
        {
            var loanSchedule = new LoanSchedule()
            {
                EmployeeID = dto.EmployeeId,
                CreatedBy = _currentUser.DesktopUserId,
                OrganizationID = _currentUser.OrganizationId,
                TotalBalanceLeft = dto.TotalLoanAmount
            };
            ApplyChanges(dto, loanSchedule);

            await _loanService.SaveAsync(loanSchedule);

            return ConvertToDto(loanSchedule);
        }

        public async Task<LoanDto> Update(int id, UpdateLoanDto dto)
        {
            var loanSchedule = await _loanRepository.GetByIdAsync(id);
            if (loanSchedule == null) return null;

            loanSchedule.LastUpdBy = _currentUser.DesktopUserId;

            ApplyChanges(dto, loanSchedule);

            await _loanService.SaveAsync(loanSchedule);

            return ConvertToDto(loanSchedule);
        }

        public async Task<ActionResult<PaginatedList<LoanHistoryDto>>> GetLoanHistory(PageOptions options, int loanId)
        {
            var currentLoanTransactions = await _loanRepository.GetLoanTransactionsAsync(options, loanId);

            return currentLoanTransactions.Select(x => LoanHistoryDto.Convert(x));
        }

        public async Task Delete(int id)
        {
            await _loanService.DeleteAsync(id);
        }

        public List<string> GetStatusList()
        {
            return _loanRepository.GetStatusList();
        }

        public async Task<List<DropDownItem>> GetLoanTypes()
        {
            var leaveTypes = await _productRepository.GetLoanTypesAsync(_currentUser.OrganizationId);

            return leaveTypes
                    .Where(x => !string.IsNullOrWhiteSpace(x.PartNo))
                    .Select(x => DropDownItem.FromProduct(x))
                    .ToList();
        }

        public async Task<List<string>> GetDeductionSchedules()
        {
            return _listOfValueRepository.ConvertToStringList(
                            await _listOfValueRepository.GetDeductionSchedulesAsync());
        }

        private static void ApplyChanges(CrudLoanDto dto, LoanSchedule loanSchedule)
        {
            loanSchedule.LoanTypeID = dto.LoanTypeId;
            loanSchedule.LoanNumber = dto.LoanNumber;
            loanSchedule.TotalLoanAmount = dto.TotalLoanAmount;
            loanSchedule.DedEffectiveDateFrom = dto.StartDate;
            loanSchedule.DeductionAmount = dto.DeductionAmount;
            loanSchedule.Status = dto.Status;
            loanSchedule.DeductionPercentage = dto.DeductionPercentage;
            loanSchedule.DeductionSchedule = dto.DeductionSchedule;
            loanSchedule.Comments = dto.Comments;
        }

        private static LoanDto ConvertToDto(LoanSchedule loan)
        {
            if (loan == null) return null;

            return new LoanDto()
            {
                Id = loan.RowID.Value,
                EmployeeNumber = loan.Employee?.EmployeeNo,
                EmployeeId = loan.Employee?.RowID ?? 0,
                EmployeeName = loan.Employee?.FullNameWithMiddleInitialLastNameFirst,
                EmployeeType = loan.Employee?.EmployeeType,
                LoanTypeId = loan.LoanTypeID.Value,
                LoanType = loan.LoanType?.Name,
                LoanNumber = loan.LoanNumber,
                TotalLoanAmount = loan.TotalLoanAmount,
                TotalBalanceLeft = loan.TotalBalanceLeft,
                StartDate = loan.DedEffectiveDateFrom,
                LoanPayPeriodLeft = loan.LoanPayPeriodLeft,
                DeductionAmount = loan.DeductionAmount,
                Status = loan.Status,
                DeductionPercentage = loan.DeductionPercentage,
                DeductionSchedule = loan.DeductionSchedule,
                Comments = loan.Comments
            };
        }
    }
}
