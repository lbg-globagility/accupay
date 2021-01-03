using AccuPay.Core.Entities;
using AccuPay.Core.Helpers;
using AccuPay.Core.Interfaces;
using AccuPay.Core.Services.Imports.Loans;
using AccuPay.Infrastructure.Services.Excel;
using AccuPay.Web.Core.Auth;
using AccuPay.Web.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Web.Loans
{
    public class LoanService
    {
        private readonly ILoanDataService _loanService;
        private readonly IProductRepository _productRepository;
        private readonly IListOfValueRepository _listOfValueRepository;
        private readonly ILoanRepository _loanRepository;
        private readonly ICurrentUser _currentUser;
        private readonly ILoanImportParser _importParser;

        public LoanService(
            ILoanDataService loanService,
            IProductRepository productRepository,
            IListOfValueRepository listOfValueRepository,
            ILoanRepository loanRepository,
            ICurrentUser currentUser,
            ILoanImportParser importParser)
        {
            _loanService = loanService;
            _productRepository = productRepository;
            _listOfValueRepository = listOfValueRepository;
            _loanRepository = loanRepository;
            _currentUser = currentUser;
            _importParser = importParser;
        }

        public async Task<PaginatedList<LoanDto>> PaginatedList(LoanPageOptions options)
        {
            var paginatedList = await _loanRepository.GetPaginatedListAsync(
                options,
                _currentUser.OrganizationId);

            return paginatedList.Select(x => ConvertToDto(x));
        }

        public async Task<LoanDto> GetById(int id)
        {
            var officialBusiness = await _loanRepository.GetByIdWithEmployeeAndProductAsync(id);

            return ConvertToDto(officialBusiness);
        }

        public async Task<LoanDto> Create(CreateLoanDto dto)
        {
            var loan = new Loan()
            {
                EmployeeID = dto.EmployeeId,
                OrganizationID = _currentUser.OrganizationId,
                TotalBalanceLeft = dto.TotalLoanAmount
            };
            ApplyChanges(dto, loan);

            await _loanService.SaveAsync(loan, _currentUser.UserId);

            return ConvertToDto(loan);
        }

        public async Task<LoanDto> Update(int id, UpdateLoanDto dto)
        {
            var loan = await _loanRepository.GetByIdAsync(id);
            if (loan == null) return null;

            ApplyChanges(dto, loan);

            await _loanService.SaveAsync(loan, _currentUser.UserId);

            return ConvertToDto(loan);
        }

        public async Task<ActionResult<PaginatedList<LoanHistoryDto>>> GetLoanHistory(PageOptions options, int loanId)
        {
            var currentLoanTransactions = await _loanRepository.GetLoanTransactionsAsync(options, loanId);

            return currentLoanTransactions.Select(x => LoanHistoryDto.Convert(x));
        }

        public async Task Delete(int id)
        {
            await _loanService.DeleteAsync(
                id: id,
                currentlyLoggedInUserId: _currentUser.UserId);
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

        private static void ApplyChanges(CrudLoanDto dto, Loan loan)
        {
            loan.LoanTypeID = dto.LoanTypeId;
            loan.LoanNumber = dto.LoanNumber;
            loan.TotalLoanAmount = dto.TotalLoanAmount;
            loan.DedEffectiveDateFrom = dto.StartDate;
            loan.DeductionAmount = dto.DeductionAmount;
            loan.Status = dto.Status;
            loan.DeductionSchedule = dto.DeductionSchedule;
            loan.Comments = dto.Comments;
        }

        private static LoanDto ConvertToDto(Loan loan)
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

        internal async Task<LoanImportParserOutput> Import(IFormFile file)
        {
            if (Path.GetExtension(file.FileName) != _importParser.XlsxExtension)
                throw new InvalidFormatException();

            Stream stream = new MemoryStream();
            await file.CopyToAsync(stream);

            if (stream == null)
                throw new Exception("Unable to parse excel file.");

            int userId = _currentUser.UserId;
            var parsedResult = await _importParser.Parse(stream, _currentUser.OrganizationId);

            await _loanService.BatchApply(parsedResult.ValidRecords, organizationId: _currentUser.OrganizationId, currentlyLoggedInUserId: userId);

            return parsedResult;
        }
    }
}
