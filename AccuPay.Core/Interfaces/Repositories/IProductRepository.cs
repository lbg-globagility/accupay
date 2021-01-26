using AccuPay.Core.Entities;
using AccuPay.Core.Enums;
using AccuPay.Core.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AccuPay.Core.Interfaces
{
    public interface IProductRepository
    {
        Task<Product> AddAdjustmentTypeAsync(string adjustmentName, int organizationId, int userId, AdjustmentType adjustmentType = AdjustmentType.Blank, string comments = "");

        Task<Product> AddAllowanceTypeAsync(string allowanceName, int organizationId, int userId);

        Task<Product> AddBonusTypeAsync(string loanName, int organizationId, int userId, bool isTaxable = false);

        Task<Product> AddDisciplinaryTypeAsync(string name, int organizationId, int userId, string description);

        Task<Product> AddLeaveTypeAsync(string leaveName, int organizationId, int userId);

        Task<Product> AddLoanTypeAsync(string loanName, int organizationId, int userId);

        Task<List<Product>> AddManyLoanTypeAsync(List<string> loanNames, int organizationId, int userId);

        Task<bool> CheckIfAlreadyUsedInAllowancesAsync(string allowanceType);

        List<string> ConvertToStringList(ICollection<Product> products, string columnName = "PartNo");

        Task DeleteAsync(int id);

        Task DeleteLoanTypeAsync(int id);

        Task<ICollection<Product>> GetAdditionAdjustmentTypesAsync(int organizationId);

        Task<ICollection<Product>> GetAdjustmentTypesAsync(int organizationId);

        Task<ICollection<Product>> GetAllowanceTypesAsync(int organizationId);

        Task<ICollection<Product>> GetBonusTypesAsync(int organizationId);

        Task<Product> GetByIdAsync(int id);

        Task<ICollection<Product>> GetDeductionAdjustmentTypesAsync(int organizationId);

        Task<ICollection<Product>> GetDisciplinaryTypesAsync(int organizationId);

        Task<ICollection<Product>> GetGovernmentLoanTypesAsync(int organizationId);

        Task<ICollection<Product>> GetLeaveTypesAsync(int organizationId);

        Task<ICollection<Product>> GetLoanTypesAsync(int organizationId);

        Task<ICollection<Product>> GetManyByIdsAsync(int[] ids);

        Task<Product> GetOrCreateAdjustmentTypeAsync(string adjustmentTypeName, int organizationId, int userId);

        Task<Product> GetOrCreateAllowanceTypeAsync(string allowanceTypeName, int organizationId, int userId);

        Task<Product> GetOrCreateLeaveTypeAsync(string leaveTypeName, int organizationId, int userId);

        Task<Product> GetOrCreateLoanTypeAsync(string loanTypeName, int organizationId, int userId);

        Task<PaginatedList<Product>> GetPaginatedLoanTypeListAsync(PageOptions options, string searchTerm, int organizationId);

        Task<Product> UpdateAdjustmentTypeAsync(int id, int userId, string adjustmentName, string code);

        Task<Product> UpdateDisciplinaryTypeAsync(int id, int userId, string adjustmentName, string description);

        Task UpdateLoanTypeAsync(int id, string loanTypeName);
    }
}
