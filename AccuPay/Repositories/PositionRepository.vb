Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Data
Imports AccuPay.Entity
Imports Microsoft.EntityFrameworkCore

Namespace Global.AccuPay.Repository

    Public Class PositionRepository

        Private _employeeRepository As New Repositories.EmployeeRepository

        Private _categoryRepository As New Repositories.CategoryRepository

        Public Async Function GetAllAsync() As Task(Of List(Of Position))

            Using context As New PayrollContext

                Return Await context.Positions.
                                Where(Function(p) Nullable.Equals(p.OrganizationID, z_OrganizationID)).
                                ToListAsync

            End Using

        End Function

        Public Async Function GetByIdAsync(positionId As Integer?) As Task(Of Position)

            Using context As New PayrollContext

                Return Await context.Positions.
                            Where(Function(p) Nullable.Equals(p.RowID, positionId)).
                            FirstOrDefaultAsync()

            End Using

        End Function

        Public Async Function GetByNameAsync(positionName As String) As Task(Of Position)

            Using context As New PayrollContext

                Return Await context.Positions.
                            Where(Function(p) p.Name.ToLower.Trim = positionName.ToLower.Trim).
                            Where(Function(p) Nullable.Equals(p.OrganizationID, z_OrganizationID)).
                            FirstOrDefaultAsync()

            End Using

        End Function

        Public Async Function GetEmployeesAsync(positionId As Integer?) As Task(Of List(Of Entities.Employee))

            Dim employees = Await _employeeRepository.GetAllActiveWithPositionAsync(z_OrganizationID)

            Return employees.Where(Function(e) positionId.Value = e.PositionID.Value).ToList()
        End Function

        Public Async Function HasEmployeesAsync(positionId As Integer?) As Task(Of Boolean)
            Dim employees = Await GetEmployeesAsync(positionId)

            Return employees.Any
        End Function

        Public Async Function SaveAsync(position As Position) As Task(Of Position)

            Using context As New PayrollContext

                Dim existingPosition As Position = Await GetByNameAsync(position.Name)

                If position.RowID Is Nothing Then
                    Insert(position, existingPosition, context)
                Else
                    Update(position, existingPosition, context)
                End If

                Await context.SaveChangesAsync()

                Dim newPosition = Await context.Positions.
                                    FirstOrDefaultAsync(Function(p) Nullable.Equals(p.RowID, position.RowID))

                If newPosition Is Nothing Then
                    Throw New ArgumentException("There was a problem inserting the new position. Please try again.")
                End If

                Return newPosition

            End Using

        End Function

        Public Async Function DeleteAsync(positionId As Integer?) As Task

            Using context = New PayrollContext()

                If Await HasEmployeesAsync(positionId) Then

                    Throw New ArgumentException("Position already has assigned employee therefore cannot be deleted.")

                End If

                Dim position = Await GetByIdAsync(positionId)

                context.Remove(position)

                Await context.SaveChangesAsync()

            End Using

        End Function

#Region "Private Functions"

        Private Async Function GetCategoryId(categoryName As String) As Task(Of Integer?)
            Dim category = Await _categoryRepository.GetByName(z_OrganizationID, categoryName)
            Return category.RowID
        End Function

        Private Sub Insert(
                        position As Position,
                        existingPosition As Position,
                        context As PayrollContext)

            If existingPosition IsNot Nothing Then

                Throw New ArgumentException("Position name already exists!")

            End If

            position.CreatedBy = z_User

            context.Positions.Add(position)
        End Sub

        Private Sub Update(
                        position As Position,
                        existingPosition As Position,
                        context As PayrollContext)

            If existingPosition IsNot Nothing AndAlso
                Nullable.Equals(position.RowID, existingPosition.RowID) = False Then

                Throw New ArgumentException("Position name already exists!")

            End If

            position.LastUpdBy = z_User

            context.Positions.Attach(position)
            context.Entry(position).State = EntityState.Modified

        End Sub

#End Region

    End Class

End Namespace