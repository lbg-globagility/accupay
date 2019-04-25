Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Entity
Imports Microsoft.EntityFrameworkCore

Namespace Global.AccuPay.Repository

    Public Class PositionRepository

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

        Public Async Function GetEmployeesAsync(positionId As Integer?) As Task(Of List(Of Employee))

            Using context As New PayrollContext

                Return Await context.Employees.
                            Where(Function(e) Nullable.Equals(e.PositionID, positionId)).
                            ToListAsync()

            End Using

        End Function

        Public Async Function HasEmployeesAsync(positionId As Integer?) As Task(Of Boolean)

            Using context As New PayrollContext

                Return Await context.Employees.
                            Where(Function(e) Nullable.Equals(e.PositionID, positionId)).
                            AnyAsync()

            End Using

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

            Using context = New PayrollContext()

                Dim category = Await context.Categories.
                                Where(Function(c) Nullable.Equals(c.OrganizationID, z_OrganizationID)).
                                FirstOrDefaultAsync

                Return category?.RowID

            End Using

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
