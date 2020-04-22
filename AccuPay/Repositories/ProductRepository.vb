Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Entity
Imports AccuPay.Enums
Imports Microsoft.EntityFrameworkCore

Namespace Global.AccuPay.Repository

    Public Class ProductRepository

        Public Async Function GetBonusTypes() _
            As Task(Of IEnumerable(Of Product))

            Dim categoryName = ProductConstant.BONUS_TYPE_CATEGORY

            Dim category = Await GetOrCreateCategoryByName(categoryName)
            Return Await GetProductsByCategory(category.RowID)

        End Function
        Public Async Function GetAllowanceTypes() _
            As Task(Of IEnumerable(Of Product))

            Dim categoryName = ProductConstant.ALLOWANCE_TYPE_CATEGORY

            Dim category = Await GetOrCreateCategoryByName(categoryName)
            Return Await GetProductsByCategory(category.RowID)

        End Function

        Public Async Function GetLeaveTypes() _
            As Task(Of IEnumerable(Of Product))

            Dim categoryName = ProductConstant.LEAVE_TYPE_CATEGORY

            Dim category = Await GetOrCreateCategoryByName(categoryName)
            Return Await GetProductsByCategory(category.RowID)

        End Function

        Public Async Function GetLoanTypes() _
            As Task(Of IEnumerable(Of Product))

            Using context As New PayrollContext

                Return Await (Await GetLoanTypesBaseQuery(context)).ToListAsync

            End Using

        End Function

        Public Async Function GetGovernmentLoanTypes() _
            As Task(Of IEnumerable(Of Product))

            Dim governmentLoans = {ProductConstant.PAG_IBIG_LOAN, ProductConstant.SSS_LOAN}

            Using context As New PayrollContext

                Return Await (Await GetLoanTypesBaseQuery(context)).
                            Where(Function(p) governmentLoans.Contains(p.PartNo)).
                            ToListAsync

            End Using

        End Function

        Public Async Function GetAdjustmentTypes() _
            As Task(Of IEnumerable(Of Product))

            Using context As New PayrollContext

                Return Await (Await GetAdjustmentTypesBaseQuery(context)).ToListAsync

            End Using

        End Function

        Public Async Function GetDeductionAdjustmentTypes() _
            As Task(Of IEnumerable(Of Product))

            Using context As New PayrollContext

                Return Await (Await GetAdjustmentTypesBaseQuery(context)).
                            Where(Function(p) p.Description = ProductConstant.ADJUSTMENT_TYPE_DEDUCTION).
                            ToListAsync

            End Using

        End Function

        Public Async Function GetAdditionAdjustmentTypes() _
            As Task(Of IEnumerable(Of Product))

            Using context As New PayrollContext

                Return Await (Await GetAdjustmentTypesBaseQuery(context)).
                            Where(Function(p) p.Description = ProductConstant.ADJUSTMENT_TYPE_ADDITION).
                            ToListAsync

            End Using

        End Function

        Public Async Function GetOrCreateLoanType(loanTypeName As String) As Task(Of Product)

            Using context = New PayrollContext()

                Dim loanType = Await context.Products.
                                Where(Function(p) Nullable.Equals(p.OrganizationID, z_OrganizationID)).
                                Where(Function(p) p.PartNo.ToLower = loanTypeName.ToLower).
                                FirstOrDefaultAsync

                If loanType Is Nothing Then
                    loanType = Await AddLoanType(loanTypeName)
                End If

                Return loanType

            End Using

        End Function

        Public Async Function GetOrCreateAllowanceType(allowanceTypeName As String) As Task(Of Product)

            Using context = New PayrollContext()

                Dim allowanceType = Await context.Products.
                                    Where(Function(p) Nullable.Equals(p.OrganizationID, z_OrganizationID)).
                                    Where(Function(p) p.PartNo.ToLower = allowanceTypeName.ToLower).
                                    FirstOrDefaultAsync

                If allowanceType Is Nothing Then
                    allowanceType = Await AddAllowanceType(allowanceTypeName)
                End If

                Return allowanceType

            End Using

        End Function

        Public Async Function GetOrCreateAdjustmentType(adjustmentTypeName As String) As Task(Of Product)

            Using context = New PayrollContext()

                Dim adjustmentType = Await context.Products.
                                    Where(Function(p) Nullable.Equals(p.OrganizationID, z_OrganizationID)).
                                    Where(Function(p) p.PartNo.ToLower = adjustmentTypeName.ToLower).
                                    FirstOrDefaultAsync

                If adjustmentType Is Nothing Then
                    adjustmentType = Await AddAdjustmentType(adjustmentTypeName)
                End If

                Return adjustmentType

            End Using

        End Function

        Public Async Function GetOrCreateBonusType(bonusTypeName As String) As Task(Of Product)

            Using context = New PayrollContext()

                Dim bonusType = Await context.Products.
                                Where(Function(p) Nullable.Equals(p.OrganizationID, z_OrganizationID)).
                                Where(Function(p) p.PartNo.ToLower = bonusTypeName.ToLower).
                                FirstOrDefaultAsync

                If bonusType Is Nothing Then
                    bonusType = Await AddBonusType(bonusTypeName)
                End If

                Return bonusType

            End Using

        End Function

        Public Async Function AddBonusType(loanName As String, Optional isTaxable As Boolean = False, Optional throwError As Boolean = True) _
            As Task(Of Product)

            Dim product As New Product

            product.Category = ProductConstant.BONUS_TYPE_CATEGORY

            Return Await AddProduct(loanName, throwError, product, isTaxable)

        End Function

        Public Async Function AddLoanType(loanName As String, Optional throwError As Boolean = True) _
            As Task(Of Product)

            Dim product As New Product

            product.Category = ProductConstant.LOAN_TYPE_CATEGORY

            Return Await AddProduct(loanName, throwError, product)

        End Function

        Public Async Function AddAllowanceType(allowanceName As String, Optional throwError As Boolean = True) _
            As Task(Of Product)

            Dim product As New Product

            product.Category = ProductConstant.ALLOWANCE_TYPE_CATEGORY

            Return Await AddProduct(allowanceName, throwError, product)

        End Function

        Public Async Function AddAdjustmentType(
                                    adjustmentName As String,
                                    Optional adjustmentType As AdjustmentType = AdjustmentType.Blank,
                                    Optional comments As String = "",
                                    Optional throwError As Boolean = True) _
            As Task(Of Product)

            Dim product As New Product

            product.Comments = comments
            product.Description = DetermineAdjustmentTypeString(adjustmentType)

            product.Category = ProductConstant.ADJUSTMENT_TYPE_CATEGORY

            Return Await AddProduct(adjustmentName, throwError, product)

        End Function

        Public Async Function Delete(id As Integer, Optional throwError As Boolean = True) _
            As Task(Of Boolean)

            Using context = New PayrollContext()

                Dim product = Await context.Products.FirstOrDefaultAsync(Function(p) p.RowID.Value = id)

                If product Is Nothing Then
                    If throwError Then
                        Throw New ArgumentException("The data that you want to delete is already gone. Please reopen the form to refresh the page.")
                    Else
                        Return False
                    End If

                End If

                context.Products.Remove(product)

                Await context.SaveChangesAsync()

                Return True
            End Using

        End Function

        Public Async Function UpdateAdjustmentType(id As Integer, adjustmentName As String, code As String, Optional throwError As Boolean = True) _
            As Task(Of Product)

            Using context = New PayrollContext()

                Dim product = Await context.Products.FirstOrDefaultAsync(Function(p) p.RowID.Value = id)

                If product Is Nothing AndAlso throwError Then
                    Throw New ArgumentException("There was a problem in updating the adjustment type. Please reopen the form and try again.")
                End If

                product.PartNo = adjustmentName.Trim()
                product.Name = adjustmentName.Trim()

                product.Comments = code
                product.LastUpdBy = z_User

                Await context.SaveChangesAsync()

                Dim newProduct = Await context.Products.
                    FirstOrDefaultAsync(Function(p) Nullable.Equals(p.RowID, product.RowID))

                If newProduct Is Nothing AndAlso throwError Then
                    Throw New ArgumentException("There was a problem inserting the new adjustment type. Please try again.")
                End If

                Return newProduct
            End Using

        End Function

        Public Async Function CheckIfProductExists(productName As String, categoryId As Integer?) As Task(Of Boolean)

            Using context As New PayrollContext

                Return Await context.Products.
                                Where(Function(p) p.PartNo.Trim = productName.Trim).
                                Where(Function(p) p.CategoryID.Value = categoryId.Value).
                                Where(Function(p) p.OrganizationID.Value = z_OrganizationID).
                                AnyAsync
            End Using

        End Function

        Public Function ConvertToStringList(products As IEnumerable(Of Product), Optional columnName As String = "PartNo") _
            As List(Of String)

            Dim stringList As List(Of String)
            stringList = New List(Of String)

            For Each product In products

                Select Case columnName
                    Case "Name"
                        stringList.Add(product.PartNo)

                    Case Else
                        stringList.Add(product.Name)
                End Select
            Next

            Return stringList.OrderBy(Function(s) s).ToList

        End Function

#Region "Private Functions"

        'this may only apply to "Loan Type" use with caution
        Private Shared Async Function GetOrCreateCategoryByName(categoryName As String) As Task(Of Category)
            Using context = New PayrollContext()

                Dim categoryProduct = Await context.Categories.
                                    Where(Function(c) Nullable.Equals(c.OrganizationID, z_OrganizationID)).
                                    Where(Function(c) c.CategoryName = categoryName).
                                    FirstOrDefaultAsync

                If categoryProduct Is Nothing Then
                    'get the existing category with same name to use as CategoryID
                    Dim existingCategoryProduct = Await context.Categories.
                                    Where(Function(c) c.CategoryName = categoryName).
                                    FirstOrDefaultAsync

                    Dim existingCategoryProductId = existingCategoryProduct?.RowID

                    categoryProduct = New Category
                    categoryProduct.CategoryID = existingCategoryProductId
                    categoryProduct.CategoryName = categoryName
                    categoryProduct.OrganizationID = z_OrganizationID
                    categoryProduct.CatalogID = Nothing
                    categoryProduct.LastUpd = Date.Now

                    context.Categories.Add(categoryProduct)
                    context.SaveChanges()

                    'if there is no existing category with same name,
                    'use the newly added category's RowID as its CategoryID

                    If existingCategoryProductId Is Nothing Then

                        Try
                            categoryProduct.CategoryID = categoryProduct.RowID
                            Await context.SaveChangesAsync()
                        Catch ex As Exception
                            'if for some reason hindi na update, we can't let that row
                            'to have no CategoryID so dapat i-delete rin yung added category
                            context.Categories.Remove(categoryProduct)
                            context.SaveChanges()

                            Throw ex
                        End Try

                    End If
                End If

                If categoryProduct Is Nothing Then
                    Dim ex = New Exception("ProductRepository->GetOrCreate: Category not found.")
                    Throw ex
                End If

                Return categoryProduct

            End Using
        End Function

        Private Async Function GetProductsByCategory(categoryId As Integer?) _
            As Task(Of IEnumerable(Of Product))

            Using context = New PayrollContext()

                Dim listOfValues = Await GetProductsByCategoryBaseQuery(categoryId, context).
                                            ToListAsync

                Return listOfValues

            End Using

        End Function

        Private Function GetProductsByCategoryBaseQuery(categoryId As Integer?, context As PayrollContext) _
            As IQueryable(Of Product)

            Return context.Products.
                                Where(Function(p) Nullable.Equals(p.OrganizationID, z_OrganizationID)).
                                Where(Function(p) Nullable.Equals(p.CategoryID, categoryId))

        End Function

        Private Async Function GetAdjustmentTypesBaseQuery(context As PayrollContext) As Task(Of IQueryable(Of Product))

            Dim categoryName = ProductConstant.ADJUSTMENT_TYPE_CATEGORY

            Dim category = Await GetOrCreateCategoryByName(categoryName)
            Return GetProductsByCategoryBaseQuery(category.RowID, context)

        End Function

        Private Async Function GetLoanTypesBaseQuery(context As PayrollContext) As Task(Of IQueryable(Of Product))

            Dim categoryName = ProductConstant.LOAN_TYPE_CATEGORY

            Dim category = Await GetOrCreateCategoryByName(categoryName)
            Return GetProductsByCategoryBaseQuery(category.RowID, context)

        End Function

        Private Async Function AddProduct(productName As String, throwError As Boolean, product As Product, Optional isTaxable As Boolean = False) As Task(Of Product)

            Dim categoryId = (Await GetOrCreateCategoryByName(product.Category))?.RowID

            If categoryId Is Nothing Then

                If throwError Then
                    Throw New ArgumentException("There was a problem on saving the data. Please try again.")
                Else
                    Return Nothing
                End If

            End If

            If Await CheckIfProductExists(productName, categoryId) Then

                If throwError Then
                    Throw New ArgumentException("Product already exists.")
                Else
                    Return Nothing
                End If

            End If

            product.CategoryID = categoryId

            product.PartNo = productName.Trim()
            product.Name = productName.Trim()
            product.Status = If(isTaxable, "1", "0")
            product.Created = Date.Now
            product.CreatedBy = z_User
            product.OrganizationID = z_OrganizationID

            Using context = New PayrollContext()

                context.Products.Add(product)

                Await context.SaveChangesAsync()

                Dim newProduct = Await context.Products.
                    FirstOrDefaultAsync(Function(p) Nullable.Equals(p.RowID, product.RowID))

                If newProduct Is Nothing AndAlso throwError Then
                    Throw New ArgumentException("There was a problem on saving the data. Please try again.")
                End If

                Return newProduct
            End Using
        End Function

        Private Shared Function DetermineAdjustmentTypeString(adjustmentType As AdjustmentType) As String
            If adjustmentType = AdjustmentType.Deduction Then

                Return ProductConstant.ADJUSTMENT_TYPE_DEDUCTION

            ElseIf adjustmentType = AdjustmentType.OtherIncome Then

                Return ProductConstant.ADJUSTMENT_TYPE_ADDITION
            Else

                Return String.Empty

            End If
        End Function

#End Region

    End Class

End Namespace