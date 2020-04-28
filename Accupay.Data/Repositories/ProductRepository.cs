using AccuPay.Data.Entities;
using AccuPay.Data.Enums;
using AccuPay.Data.Helpers;
using AccuPay.Utilities.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class ProductRepository
    {
        #region CRUD

        public async Task DeleteAsync(int id)
        {
            using (var context = new PayrollContext())
            {
                var product = await context.Products.FirstOrDefaultAsync(p => p.RowID == id);

                context.Products.Remove(product);

                await context.SaveChangesAsync();
            }
        }

        public async Task<Product> AddBonusTypeAsync(string loanName, int organizationId, int userId, bool isTaxable = false)
        {
            Product product = new Product
            {
                Category = ProductConstant.BONUS_TYPE_CATEGORY
            };

            return await AddProductAsync(loanName, product, organizationId, userId, isTaxable);
        }

        public async Task<Product> AddLoanTypeAsync(string loanName, int organizationId, int userId)
        {
            Product product = new Product
            {
                Category = ProductConstant.LOAN_TYPE_CATEGORY
            };

            return await AddProductAsync(loanName, product, organizationId, userId);
        }

        public async Task<Product> AddAllowanceTypeAsync(string allowanceName, int organizationId, int userId)
        {
            Product product = new Product
            {
                Category = ProductConstant.ALLOWANCE_TYPE_CATEGORY
            };

            return await AddProductAsync(allowanceName, product, organizationId, userId);
        }

        public async Task<Product> AddAdjustmentTypeAsync(string adjustmentName, int organizationId, int userId, AdjustmentType adjustmentType = AdjustmentType.Blank, string comments = "")
        {
            Product product = new Product
            {
                Comments = comments,
                Description = DetermineAdjustmentTypeString(adjustmentType),
                Category = ProductConstant.ADJUSTMENT_TYPE_CATEGORY
            };

            return await AddProductAsync(adjustmentName, product, organizationId, userId);
        }

        private async Task<Product> AddProductAsync(string productName, Product product, int organizationId, int userId, bool isTaxable = false)
        {
            var categoryId = (await GetOrCreateCategoryByName(product.Category, organizationId))?.RowID;

            if (categoryId == null)
                throw new ArgumentException("There was a problem on saving the data. Please try again.");

            if (await CheckIfProductExists(productName, categoryId.Value, organizationId))
                throw new ArgumentException("Product already exists.");

            product.CategoryID = categoryId;

            product.PartNo = productName.Trim();
            product.Name = productName.Trim();
            product.Status = isTaxable ? "1" : "0";

            product.Created = DateTime.Now;
            product.CreatedBy = userId;
            product.OrganizationID = organizationId;

            using (var context = new PayrollContext())
            {
                context.Products.Add(product);

                await context.SaveChangesAsync();

                var newProduct = await context.Products.FirstOrDefaultAsync(p => p.RowID == product.RowID);

                return newProduct;
            }
        }

        public async Task<Product> UpdateAdjustmentTypeAsync(int id, int userId, string adjustmentName, string code)
        {
            using (var context = new PayrollContext())
            {
                var product = await context.Products.FirstOrDefaultAsync(p => p.RowID == id);

                if (product == null) return null;

                product.PartNo = adjustmentName.Trim();
                product.Name = adjustmentName.Trim();

                product.Comments = code;
                product.LastUpdBy = userId;

                await context.SaveChangesAsync();

                var newProduct = await context.Products.FirstOrDefaultAsync(p => p.RowID == product.RowID);

                return newProduct;
            }
        }

        #endregion CRUD

        #region Queries

        #region Single entity

        public async Task<Product> GetOrCreateLoanTypeAsync(string loanTypeName, int organizationId, int userId)
        {
            var categoryId = (await GetOrCreateCategoryByName(ProductConstant.LOAN_TYPE_CATEGORY, organizationId))?.RowID;

            if (categoryId == null)
                throw new ArgumentException("There was a problem on fetching the data. Please try again.");

            var loanType = await GetProductByNameAndCategory(loanTypeName, categoryId.Value, organizationId);

            if (loanType == null)
                loanType = await AddLoanTypeAsync(loanTypeName, organizationId: organizationId, userId: userId);

            return loanType;
        }

        public async Task<Product> GetOrCreateAllowanceTypeAsync(string allowanceTypeName, int organizationId, int userId)
        {
            var categoryId = (await GetOrCreateCategoryByName(ProductConstant.ALLOWANCE_TYPE_CATEGORY, organizationId))?.RowID;

            if (categoryId == null)
                throw new ArgumentException("There was a problem on fetching the data. Please try again.");

            var allowanceType = await GetProductByNameAndCategory(allowanceTypeName, categoryId.Value, organizationId);

            if (allowanceType == null)
                allowanceType = await AddAllowanceTypeAsync(allowanceTypeName, organizationId, userId);

            return allowanceType;
        }

        public async Task<Product> GetOrCreateAdjustmentTypeAsync(string adjustmentTypeName, int organizationId, int userId)
        {
            var categoryId = (await GetOrCreateCategoryByName(ProductConstant.ADJUSTMENT_TYPE_CATEGORY, organizationId))?.RowID;

            if (categoryId == null)
                throw new ArgumentException("There was a problem on fetching the data. Please try again.");

            var adjustmentType = await GetProductByNameAndCategory(adjustmentTypeName, categoryId.Value, organizationId);

            if (adjustmentType == null)
                adjustmentType = await AddAdjustmentTypeAsync(adjustmentTypeName, organizationId, userId);

            return adjustmentType;
        }

        #endregion Single entity

        #region List of entities

        public async Task<IEnumerable<Product>> GetBonusTypesAsync(int organizationId)
        {
            var categoryName = ProductConstant.BONUS_TYPE_CATEGORY;

            var category = await GetOrCreateCategoryByName(categoryName, organizationId);
            return await GetProductsByCategory(category.RowID, organizationId);
        }

        public async Task<IEnumerable<Product>> GetAllowanceTypesAsync(int organizationId)
        {
            var categoryName = ProductConstant.ALLOWANCE_TYPE_CATEGORY;

            var category = await GetOrCreateCategoryByName(categoryName, organizationId);
            return await GetProductsByCategory(category.RowID, organizationId);
        }

        public async Task<IEnumerable<Product>> GetLeaveTypesAsync(int organizationId)
        {
            var categoryName = ProductConstant.LEAVE_TYPE_CATEGORY;

            var category = await GetOrCreateCategoryByName(categoryName, organizationId);
            return await GetProductsByCategory(category.RowID, organizationId);
        }

        public async Task<IEnumerable<Product>> GetLoanTypesAsync(int organizationId)
        {
            using (PayrollContext context = new PayrollContext())
            {
                return await (await GetLoanTypesBaseQuery(context, organizationId)
                            ).ToListAsync();
            }
        }

        public async Task<IEnumerable<Product>> GetGovernmentLoanTypesAsync(int organizationId)
        {
            string[] governmentLoans = {
                ProductConstant.PAG_IBIG_LOAN,
                ProductConstant.SSS_LOAN
            };

            using (PayrollContext context = new PayrollContext())
            {
                return await (await GetLoanTypesBaseQuery(context, organizationId)).
                                    Where(p => governmentLoans.Contains(p.PartNo)
                            ).ToListAsync();
            }
        }

        public async Task<IEnumerable<Product>> GetAdjustmentTypesAsync(int organizationId)
        {
            using (PayrollContext context = new PayrollContext())
            {
                return await (await GetAdjustmentTypesBaseQuery(context, organizationId)).
                                ToListAsync();
            }
        }

        public async Task<IEnumerable<Product>> GetDeductionAdjustmentTypesAsync(int organizationId)
        {
            using (PayrollContext context = new PayrollContext())
            {
                return await (await GetAdjustmentTypesBaseQuery(context, organizationId)).Where(p => p.Description == ProductConstant.ADJUSTMENT_TYPE_DEDUCTION).ToListAsync();
            }
        }

        public async Task<IEnumerable<Product>> GetAdditionAdjustmentTypesAsync(int organizationId)
        {
            using (PayrollContext context = new PayrollContext())
            {
                return await (await GetAdjustmentTypesBaseQuery(context, organizationId)).Where(p => p.Description == ProductConstant.ADJUSTMENT_TYPE_ADDITION).ToListAsync();
            }
        }

        #endregion List of entities

        #endregion Queries

        public List<string> ConvertToStringList(IEnumerable<Product> products, string columnName = "PartNo")
        {
            List<string> stringList;
            stringList = new List<string>();

            foreach (var product in products)
            {
                switch (columnName)
                {
                    case "Name":
                        {
                            stringList.Add(product.PartNo);
                            break;
                        }

                    default:
                        {
                            stringList.Add(product.Name);
                            break;
                        }
                }
            }

            return stringList.OrderBy(s => s).ToList();
        }

        #region Private helper methods

        private async Task<Product> GetProductByNameAndCategory(string productName, int categoryId, int organizationId)
        {
            using (PayrollContext context = new PayrollContext())
            {
                return await CreateBaseQueryByCategory(categoryId, context, organizationId).
                                Where(p => p.PartNo.Trim().ToLower() == productName.ToTrimmedLowerCase()).
                                FirstOrDefaultAsync();
            }
        }

        private async Task<bool> CheckIfProductExists(string productName, int categoryId, int organizationId)
        {
            var product = await GetProductByNameAndCategory(productName,
                                                    categoryId: categoryId,
                                                    organizationId: organizationId);
            return product != null;
        }

        // this may only apply to "Loan Type" use with caution
        private async Task<Category> GetOrCreateCategoryByName(string categoryName, int organizationId)
        {
            using (var context = new PayrollContext())
            {
                var categoryProduct = await context.Categories.
                                        Where(c => c.OrganizationID == organizationId).
                                        Where(c => c.CategoryName.Trim().ToLower() == categoryName.ToTrimmedLowerCase()).
                                        FirstOrDefaultAsync();

                if (categoryProduct == null)
                {
                    // get the existing category with same name to use as CategoryID
                    // this is due to the design of the database wanting the category to have a CategoryID
                    // to maybe group them across different organization but never used them in the application ever
                    var existingCategoryProduct = await context.Categories.
                                                    Where(c => c.CategoryName == categoryName).
                                                    FirstOrDefaultAsync();

                    var existingCategoryProductId = existingCategoryProduct?.RowID;

                    categoryProduct = new Category
                    {
                        CategoryID = existingCategoryProductId,
                        CategoryName = categoryName,
                        OrganizationID = organizationId,
                        CatalogID = null,
                        LastUpd = DateTime.Now
                    };

                    context.Categories.Add(categoryProduct);
                    await context.SaveChangesAsync();

                    // if there is no existing category with same name,
                    // use the newly added category's RowID as its CategoryID

                    if (existingCategoryProductId == null)
                    {
                        try
                        {
                            categoryProduct.CategoryID = categoryProduct.RowID;
                            await context.SaveChangesAsync();
                        }
                        catch (Exception ex)
                        {
                            // if for some reason hindi na update, we can't let that row
                            // to have no CategoryID so dapat i-delete rin yung added category
                            context.Categories.Remove(categoryProduct);
                            await context.SaveChangesAsync();

                            throw ex;
                        }
                    }
                }

                if (categoryProduct == null)
                {
                    var ex = new Exception("ProductRepository->GetOrCreate: Category not found.");
                    throw ex;
                }

                return categoryProduct;
            }
        }

        private async Task<IEnumerable<Product>> GetProductsByCategory(int categoryId, int organizationId)
        {
            using (var context = new PayrollContext())
            {
                var listOfValues = await CreateBaseQueryByCategory(categoryId, context, organizationId).
                                            ToListAsync();

                return listOfValues;
            }
        }

        private async Task<IQueryable<Product>> GetAdjustmentTypesBaseQuery(PayrollContext context, int organizationId)
        {
            var categoryName = ProductConstant.ADJUSTMENT_TYPE_CATEGORY;

            var category = await GetOrCreateCategoryByName(categoryName, organizationId);
            return CreateBaseQueryByCategory(category.RowID, context, organizationId);
        }

        private async Task<IQueryable<Product>> GetLoanTypesBaseQuery(PayrollContext context, int organizationId)
        {
            var categoryName = ProductConstant.LOAN_TYPE_CATEGORY;

            var category = await GetOrCreateCategoryByName(categoryName, organizationId);
            return CreateBaseQueryByCategory(category.RowID, context, organizationId);
        }

        private IQueryable<Product> CreateBaseQueryByCategory(int categoryId, PayrollContext context, int organizationId)
        {
            return context.Products.
                            Where(p => p.OrganizationID == organizationId).
                            Where(p => p.CategoryID == categoryId);
        }

        private static string DetermineAdjustmentTypeString(AdjustmentType adjustmentType)
        {
            if (adjustmentType == AdjustmentType.Deduction)
                return ProductConstant.ADJUSTMENT_TYPE_DEDUCTION;
            else if (adjustmentType == AdjustmentType.OtherIncome)
                return ProductConstant.ADJUSTMENT_TYPE_ADDITION;
            else
                return string.Empty;
        }

        #endregion Private helper methods
    }
}