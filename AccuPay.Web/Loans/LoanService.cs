using AccuPay.Data.Entities;
using AccuPay.Data.Helpers;
using AccuPay.Data.Repositories;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Web.Loans
{
    public class LoanService
    {
        private readonly LoanScheduleRepository _repository;

        public LoanService(LoanScheduleRepository repository)
        {
            _repository = repository;
        }

        public async Task<PaginatedList<LoanDto>> PaginatedList(PageOptions options, string searchTerm)
        {
            // TODO: sort and desc in repository

            int organizationId = 2;
            var paginatedList = await _repository.GetPaginatedListAsync(options, organizationId, searchTerm);

            var dtos = paginatedList.List.Select(x => ConvertToDto(x));

            return new PaginatedList<LoanDto>(dtos, paginatedList.TotalCount, ++options.PageIndex, options.PageSize);
        }

        public async Task<LoanDto> GetById(int id)
        {
            var officialBusiness = await _repository.GetByIdWithEmployeeAndProductAsync(id);

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

            await _repository.SaveAsync(loanSchedule);

            return ConvertToDto(loanSchedule);
        }

        public async Task<LoanDto> Update(int id, UpdateLoanDto dto)
        {
            // TODO: validations
            // validations on what to edit on Update

            var loanSchedule = await _repository.GetByIdAsync(id);
            if (loanSchedule == null) return null;

            int userId = 1;
            loanSchedule.LastUpdBy = userId;

            ApplyChanges(dto, loanSchedule);
            loanSchedule.TotalBalanceLeft = dto.TotalBalanceLeft;

            await _repository.SaveAsync(loanSchedule);

            return ConvertToDto(loanSchedule);
        }

        private static void ApplyChanges(ICrudLoanDto dto, LoanSchedule loanSchedule)
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
                LoanNumber = loan.LoanNumber,
                LoanType = loan.LoanType?.Name,
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
