using AccuPay.Core.Helpers;
using AccuPay.Core.Interfaces;
using System.Threading.Tasks;

namespace AccuPay.Web.AllowanceType
{
    public class AllowanceTypeService
    {
        private readonly IAllowanceTypeRepository _repository;

        public AllowanceTypeService(IAllowanceTypeRepository repository)
        {
            _repository = repository;
        }

        public async Task<AllowanceTypeDto> CreateAsync(AllowanceTypeDto dto)
        {
            var newAllowanceType = new AccuPay.Core.Entities.AllowanceType();

            ApplyChanges(dto, newAllowanceType);

            var allowanceType = await _repository.CreateAsync(newAllowanceType);

            return ConvertToDto(allowanceType);
        }

        public async Task<AllowanceTypeDto> UpdateAsync(int id, AllowanceTypeDto dto)
        {
            var allowanceType = await _repository.GetByIdAsync(id);

            ApplyChanges(dto, allowanceType);

            await _repository.UpdateAsync(allowanceType);

            return dto;
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }

        public async Task<AccuPay.Core.Entities.AllowanceType> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<PaginatedList<AllowanceTypeDto>> GetPaginatedListAsync(
            PageOptions options,
            string searchTerm = "")
        {
            var paginatedList = await _repository.GetPaginatedListAsync(options, searchTerm);

            return paginatedList.Select(x => ConvertToDto(x));
        }

        private static void ApplyChanges(AllowanceTypeDto dto, AccuPay.Core.Entities.AllowanceType allowanceType)
        {
            allowanceType.DisplayString = dto.DisplayString;
            allowanceType.Frequency = dto.Frequency;
            allowanceType.Is13thMonthPay = dto.Is13thMonthPay;
            allowanceType.IsFixed = dto.IsFixed;
            allowanceType.IsTaxable = dto.IsTaxable;
            allowanceType.Name = dto.Name;
        }

        private static AllowanceTypeDto ConvertToDto(AccuPay.Core.Entities.AllowanceType allowanceType)
        {
            return AllowanceTypeDto.Convert(allowanceType);
        }
    }
}
