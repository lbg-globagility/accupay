using AccuPay.Data.Entities;
using AccuPay.Data.Repositories;
using AccuPay.Web.Core.Auth;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Web.Branches
{
    public class BranchService
    {
        private readonly BranchRepository _branchRepository;
        private readonly ICurrentUser _currentUser;

        public BranchService(BranchRepository branchRepository, ICurrentUser currentUser)
        {
            _branchRepository = branchRepository;
            _currentUser = currentUser;
        }

        public async Task<BranchDto> GetById(int branchId)
        {
            var branch = await _branchRepository.GetById(branchId);

            return ConvertToDto(branch);
        }

        public async Task<BranchDto> Create(CreateBranchDto dto)
        {
            var branch = new Branch();
            branch.Name = dto.Name;
            branch.Code = dto.Code;
            branch.CreatedBy = _currentUser.UserId;

            await _branchRepository.CreateAsync(branch);

            return ConvertToDto(branch);
        }

        public async Task<BranchDto> Update(int id, UpdateBranchDto dto)
        {
            var branch = await _branchRepository.GetById(id);
            branch.Name = dto.Name;
            branch.Code = dto.Code;
            branch.LastUpdBy = _currentUser.UserId;

            await _branchRepository.UpdateAsync(branch);

            return ConvertToDto(branch);
        }

        public async Task<ICollection<BranchDto>> List()
        {
            var branches = await _branchRepository.GetAllAsync();
            var dtos = branches.Select(b => ConvertToDto(b)).ToList();

            return dtos;
        }

        private BranchDto ConvertToDto(Branch branch)
        {
            var dto = new BranchDto()
            {
                Id = branch.RowID,
                Name = branch.Name,
                Code = branch.Code
            };

            return dto;
        }
    }
}
