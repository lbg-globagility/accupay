Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Entity
Imports Microsoft.EntityFrameworkCore

Namespace Global.AccuPay.Repository

    Public Class ProductRepository

        Public Async Function GetLoanTypes() _
            As Task(Of IEnumerable(Of Product))

            Dim categoryName = "Loan Type"

            Dim category = Await GetOrCreateCategoryByName(categoryName)
            Return Await GetProductsByCategory(category.RowID)

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

            Return stringList

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
                            context.SaveChanges()

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

                Dim listOfValues = Await context.Products.
                                Where(Function(p) Nullable.Equals(p.OrganizationID, z_OrganizationID)).
                                Where(Function(p) Nullable.Equals(p.CategoryID, categoryId)).
                                ToListAsync

                Return listOfValues

            End Using

        End Function
#End Region

    End Class

End Namespace
