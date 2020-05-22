using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using AccuPay.Web.Shared;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Web.Loans
{
    public class LoanService
    {
        private readonly LoanScheduleRepository _loanRepository;
        private readonly ProductRepository _productRepository;
        private readonly ListOfValueRepository _listOfValueRepository;

        public LoanService(LoanScheduleRepository loanRepository,
                            ProductRepository productRepository,
                            ListOfValueRepository listOfValueRepository)
        {
            _loanRepository = loanRepository;
            _productRepository = productRepository;
            _listOfValueRepository = listOfValueRepository;
        }

        public async Task<PaginatedList<LoanDto>> PaginatedList(PageOptions options, string searchTerm)
        {
            // TODO: sort and desc in repository

            int organizationId = 2;
            var paginatedList = await _loanRepository.GetPaginatedListAsync(options, organizationId, searchTerm);

            var dtos = paginatedList.List.Select(x => ConvertToDto(x));

            return new PaginatedList<LoanDto>(dtos, paginatedList.TotalCount, ++options.PageIndex, options.PageSize);
        }

        public async Task<LoanDto> GetById(int id)
        {
            var officialBusiness = await _loanRepository.GetByIdWithEmployeeAndProductAsync(id);

            return ConvertToDto(officialBusiness);
        }

        public async Task<LoanDto> Create(CreateLoanDto dto)
        {
            // TODO: validations
            // validations on what to edit on Create
            // in progress and on hold status only

            int organizationId = 2;
            int userId = 1;
            var loanSchedule = new LoanSchedule()
            {
                EmployeeID = dto.EmployeeId,
                CreatedBy = userId,
                OrganizationID = organizationId,
                TotalBalanceLeft = dto.TotalLoanAmount
            };
            ApplyChanges(dto, loanSchedule);

            await _loanRepository.SaveAsync(loanSchedule);

            return ConvertToDto(loanSchedule);
        }

        public async Task<LoanDto> Update(int id, UpdateLoanDto dto)
        {
            // TODO: validations
            // validations on what to edit on Update

            var loanSchedule = await _loanRepository.GetByIdAsync(id);
            if (loanSchedule == null) return null;

            int userId = 1;
            loanSchedule.LastUpdBy = userId;

            ApplyChanges(dto, loanSchedule);
            //loanSchedule.TotalBalanceLeft = dto.TotalBalanceLeft;

            await _loanRepository.SaveAsync(loanSchedule);

            return ConvertToDto(loanSchedule);
        }

        public async Task<List<DropDownItem>> GetLoanTypes()
        {
            int organizationId = 2;
            var leaveTypes = await _productRepository.GetLoanTypesAsync(organizationId);

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
