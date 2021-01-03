using AccuPay.Core.Entities;
using AccuPay.Core.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Web.Users
{
    public class PermissionService
    {
        private readonly IPermissionRepository _repository;

        public PermissionService(IPermissionRepository repository)
        {
            _repository = repository;
        }

        public async Task<ICollection<PermissionDto>> GetAll()
        {
            var permissions = await _repository.GetAll();
            var dtos = permissions.Select(t => ConvertToDto(t)).ToList();

            return dtos;
        }

        public PermissionDto ConvertToDto(Permission permission)
        {
            var dto = new PermissionDto()
            {
                Id = permission.Id,
                Name = permission.Name
            };

            return dto;
        }
    }
}
