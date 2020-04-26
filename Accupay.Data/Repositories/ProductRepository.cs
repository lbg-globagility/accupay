using AccuPay.Data.Entities;
using AccuPay.Data.Enums;
using AccuPay.Data.Helpers;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AccuPay.Data.Repositories
{
    public class ProductRepository
    {
        public async Task<IEnumerable<Product>> GetBonusTypes(int organizationID)
        {
            var categoryName = ProductConstant.BONUS_TYPE_CATEGORY;

            var category = await GetOrCreateCategoryByName(categoryName, organizationID);
            return await GetProductsByCategory(category.RowID, organizationID);
        }

        public async Task<IEnumerable<Product>> GetAllowanceTypes(int organizationID)
        {
            var categoryName = ProductConstant.ALLOWANCE_TYPE_CATEGORY;

            var category = await GetOrCreateCategoryByName(categoryName, organizationID);
            return await GetProductsByCategory(category.RowID, organizationID);
        }

        public async Task<IEnumerable<Product>> GetLeaveTypes(int organizationID)
        {
            var categoryName = ProductConstant.LEAVE_TYPE_CATEGORY;

            var category = await GetOrCreateCategoryByName(categoryName, organizationID);
            return await GetProductsByCategory(category.RowID, organizationID);
        }

        public async Task<IEnumerable<Product>> GetLoanTypes(int organizationID)
        {
            using (PayrollContext context = new PayrollContext())
            {
                return await (await GetLoanTypesBaseQuery(context, organizationID)).ToListAsync();
            }
        }

        public async Task<IEnumerable<Product>> GetGovernmentLoanTypes(int organizationID)
        {
            string[] governmentLoans = {
                ProductConstant.PAG_IBIG_LOAN,
                ProductConstant.SSS_LOAN
            };

            using (PayrollContext context = new PayrollContext())
            {
                return await (await GetLoanTypesBaseQuery(context, organizationID)).Where(p => governmentLoans.Contains(p.PartNo)).ToListAsync();
            }
        }

        public async Task<IEnumerable<Product>> GetAdjustmentTypes(int organizationID)
        {
            using (PayrollContext context = new PayrollContext())
            {
                return await (await GetAdjustmentTypesBaseQuery(context, organizationID)).ToListAsync();
            }
        }

        public async Task<IEnumerable<Product>> GetDeductionAdjustmentTypes(int organizationID)
        {
            using (PayrollContext context = new PayrollContext())
            {
                return await (await GetAdjustmentTypesBaseQuery(context, organizationID)).Where(p => p.Description == ProductConstant.ADJUSTMENT_TYPE_DEDUCTION).ToListAsync();
            }
        }

        public async Task<IEnumerable<Product>> GetAdditionAdjustmentTypes(int organizationID)
        {
            using (PayrollContext context = new PayrollContext())
            {
                return await (await GetAdjustmentTypesBaseQuery(context, organizationID)).Where(p => p.Description == ProductConstant.ADJUSTMENT_TYPE_ADDITION).ToListAsync();
            }
        }

        public async Task<Product> GetOrCreateLoanType(string loanTypeName, int organizationID, int userID)
        {
            using (var context = new PayrollContext())
            {
                var loanType = await context.Products.Where(p => p.OrganizationID == organizationID).Where(p => p.PartNo.ToLower() == loanTypeName.ToLower()).FirstOrDefaultAsync();

                if (loanType == null)
                    loanType = await AddLoanType(loanTypeName, organizationID, userID);

                return loanType;
            }
        }

        public async Task<Product> GetOrCreateAllowanceType(string allowanceTypeName, int organizationID, int userID)
        {
            using (var context = new PayrollContext())
            {
                var allowanceType = await context.Products.Where(p => p.OrganizationID == organizationID).Where(p => p.PartNo.ToLower() == allowanceTypeName.ToLower()).FirstOrDefaultAsync();

                if (allowanceType == null)
                    allowanceType = await AddAllowanceType(allowanceTypeName, organizationID, userID);

                return allowanceType;
            }
        }

        public async Task<Product> GetOrCreateAdjustmentType(string adjustmentTypeName, int organizationID, int userID)
        {
            using (var context = new PayrollContext())
            {
                var adjustmentType = await context.Products.Where(p => p.OrganizationID == organizationID).Where(p => p.PartNo.ToLower() == adjustmentTypeName.ToLower()).FirstOrDefaultAsync();

                if (adjustmentType == null)
                    adjustmentType = await AddAdjustmentType(adjustmentTypeName, organizationID, userID);

                return adjustmentType;
            }
        }

        public async Task<Product> GetOrCreateBonusType(string bonusTypeName, int organizationID, int userID, bool isTaxable = false)
        {
            using (var context = new PayrollContext())
            {
                var adjustmentType = await context.Products.Where(p => p.OrganizationID == organizationID).Where(p => p.PartNo.ToLower() == bonusTypeName.ToLower()).FirstOrDefaultAsync();

                if (adjustmentType == null)
                    adjustmentType = await AddBonusType(bonusTypeName, organizationID, userID, isTaxable);

                return adjustmentType;
            }
        }

        public async Task<Product> AddBonusType(string loanName, int organizationID, int userID, bool isTaxable = false, bool throwError = true)
        {
            Product product = new Product();

            product.Category = ProductConstant.BONUS_TYPE_CATEGORY;

            return await AddProduct(loanName, throwError, product, organizationID, userID, isTaxable);
        }

        public async Task<Product> AddLoanType(string loanName, int organizationID, int userID, bool throwError = true)
        {
            Product product = new Product();

            product.Category = ProductConstant.LOAN_TYPE_CATEGORY;

            return await AddProduct(loanName, throwError, product, organizationID, userID);
        }

        public async Task<Product> AddAllowanceType(string allowanceName, int organizationID, int userID, bool throwError = true)
        {
            Product product = new Product();

            product.Category = ProductConstant.ALLOWANCE_TYPE_CATEGORY;

            return await AddProduct(allowanceName, throwError, product, organizationID, userID);
        }

        public async Task<Product> AddAdjustmentType(string adjustmentName, int organizationID, int userID, AdjustmentType adjustmentType = AdjustmentType.Blank, string comments = "", bool throwError = true)
        {
            Product product = new Product();

            product.Comments = comments;
            product.Description = DetermineAdjustmentTypeString(adjustmentType);

            product.Category = ProductConstant.ADJUSTMENT_TYPE_CATEGORY;

            return await AddProduct(adjustmentName, throwError, product, organizationID, userID);
        }

        public async Task<bool> Delete(int id, bool throwError = true)
        {
            using (var context = new PayrollContext())
            {
                var product = await context.Products.FirstOrDefaultAsync(p => p.RowID.Value == id);

                if (product == null)
                {
                    if (throwError)
                        throw new ArgumentException("The data that you want to delete is already gone. Please reopen the form to refresh the page.");
                    else
                        return false;
                }

                context.Products.Remove(product);

                await context.SaveChangesAsync();

                return true;
            }
        }

        public async Task<Product> UpdateAdjustmentType(int id, int userID, string adjustmentName, string code, bool throwError = true)
        {
            using (var context = new PayrollContext())
            {
                var product = await context.Products.FirstOrDefaultAsync(p => p.RowID.Value == id);

                if (product == null && throwError)
                    throw new ArgumentException("There was a problem in updating the adjustment type. Please reopen the form and try again.");

                product.PartNo = adjustmentName.Trim();
                product.Name = adjustmentName.Trim();

                product.Comments = code;
                product.LastUpdBy = userID;

                await context.SaveChangesAsync();

                var newProduct = await context.Products.FirstOrDefaultAsync(p => p.RowID == product.RowID);

                if (newProduct == null && throwError)
                    throw new ArgumentException("There was a problem inserting the new adjustment type. Please try again.");

                return newProduct;
            }
        }

        public async Task<bool> CheckIfProductExists(string productName, int? categoryId, int organizationID)
        {
            using (PayrollContext context = new PayrollContext())
            {
                return await context.Products.
                                Where(p => p.PartNo.Trim() == productName.Trim()).
                                Where(p => p.CategoryID.Value == categoryId.Value).
                                Where(p => p.OrganizationID.Value == organizationID).
                                AnyAsync();
            }
        }

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

        // this may only apply to "Loan Type" use with caution
        private static async Task<Category> GetOrCreateCategoryByName(string categoryName, int organizationID)
        {
            using (var context = new PayrollContext())
            {
                var categoryProduct = await context.Categories.Where(c => c.OrganizationID == organizationID).Where(c => c.CategoryName == categoryName).FirstOrDefaultAsync();

                if (categoryProduct == null)
                {
                    // get the existing category with same name to use as CategoryID
                    var existingCategoryProduct = await context.Categories.Where(c => c.CategoryName == categoryName).FirstOrDefaultAsync();

                    var existingCategoryProductId = existingCategoryProduct?.RowID;

                    categoryProduct = new Category();
                    categoryProduct.CategoryID = existingCategoryProductId;
                    categoryProduct.CategoryName = categoryName;
                    categoryProduct.OrganizationID = organizationID;
                    categoryProduct.CatalogID = null;
                    categoryProduct.LastUpd = DateTime.Now;

                    context.Categories.Add(categoryProduct);
                    context.SaveChanges();

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
                            context.SaveChanges();

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

        private async Task<IEnumerable<Product>> GetProductsByCategory(int? categoryId, int organizationID)
        {
            using (var context = new PayrollContext())
            {
                var listOfValues = await GetProductsByCategoryBaseQuery(categoryId, context, organizationID).
                                            ToListAsync();

                return listOfValues;
            }
        }

        private IQueryable<Product> GetProductsByCategoryBaseQuery(int? categoryId, PayrollContext context, int organizationID)
        {
            return context.Products.
                            Where(p => p.OrganizationID == organizationID).
                            Where(p => p.CategoryID == categoryId);
        }

        private async Task<IQueryable<Product>> GetAdjustmentTypesBaseQuery(PayrollContext context, int organizationID)
        {
            var categoryName = ProductConstant.ADJUSTMENT_TYPE_CATEGORY;

            var category = await GetOrCreateCategoryByName(categoryName, organizationID);
            return GetProductsByCategoryBaseQuery(category.RowID, context, organizationID);
        }

        private async Task<IQueryable<Product>> GetLoanTypesBaseQuery(PayrollContext context, int organizationID)
        {
            var categoryName = ProductConstant.LOAN_TYPE_CATEGORY;

            var category = await GetOrCreateCategoryByName(categoryName, organizationID);
            return GetProductsByCategoryBaseQuery(category.RowID, context, organizationID);
        }

        private async Task<Product> AddProduct(string productName, bool throwError, Product product, int organizationID, int userID, bool isTaxable = false)
        {
            var categoryId = (await GetOrCreateCategoryByName(product.Category, organizationID))?.RowID;

            if (categoryId == null)
            {
                if (throwError)
                    throw new ArgumentException("There was a problem on saving the data. Please try again.");
                else
                    return null/* TODO Change to default(_) if this is not a reference type */;
            }

            if (await CheckIfProductExists(productName, categoryId, organizationID))
            {
                if (throwError)
                    throw new ArgumentException("Product already exists.");
                else
                    return null/* TODO Change to default(_) if this is not a reference type */;
            }

            product.CategoryID = categoryId;

            product.PartNo = productName.Trim();
            product.Name = productName.Trim();
            product.Status = isTaxable ? "1" : "0";

            product.Created = DateTime.Now;
            product.CreatedBy = userID;
            product.OrganizationID = organizationID;

            using (var context = new PayrollContext())
            {
                context.Products.Add(product);

                await context.SaveChangesAsync();

                var newProduct = await context.Products.FirstOrDefaultAsync(p => p.RowID == product.RowID);

                if (newProduct == null && throwError)
                    throw new ArgumentException("There was a problem on saving the data. Please try again.");

                return newProduct;
            }
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
    }
}