Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Entity
Imports Microsoft.EntityFrameworkCore

Namespace Global.AccuPay.Repository

    Public Class DivisionRepository

        Public Shared ReadOnly DIVISION_TYPE_DEPARTMENT As String = "Department"
        Public Shared ReadOnly DIVISION_TYPE_BRANCH As String = "Branch"
        Public Shared ReadOnly DIVISION_TYPE_SUB_BRANCH As String = "Sub branch"

        Public Function GetDivisionTypeList() As List(Of String)
            Return New List(Of String) From {
                        DIVISION_TYPE_DEPARTMENT,
                        DIVISION_TYPE_BRANCH,
                        DIVISION_TYPE_SUB_BRANCH
                }
        End Function

        Public Async Function GetAllAsync() As Task(Of List(Of Division))

            Using context As New PayrollContext

                Return Await context.Divisions.
                            Where(Function(d) Nullable.Equals(d.OrganizationID, z_OrganizationID)).
                            ToListAsync

            End Using

        End Function

        Public Async Function SaveAsync(division As Division) As Task

            Using context As New PayrollContext

                Dim existingDivision As Division = Await GetByNameAndParentDivisionAsync(division.Name, division.ParentDivisionID)

                If division.RowID Is Nothing Then
                    Insert(division, existingDivision, context)

                Else
                    Update(division, existingDivision, context)
                End If

                Await context.SaveChangesAsync()

            End Using

        End Function

#Region "Private Function"

        Public Async Function GetByNameAndParentDivisionAsync(positionName As String, parentDivisionId As Integer?) As Task(Of Division)

            Using context As New PayrollContext

                Return Await context.Divisions.
                            Include(Function(d) d.ParentDivision).
                            Where(Function(d) d.Name.ToLower.Trim = positionName.ToLower.Trim).
                            Where(Function(d) Nullable.Equals(d.ParentDivisionID, parentDivisionId)).
                            Where(Function(d) Nullable.Equals(d.OrganizationID, z_OrganizationID)).
                            FirstOrDefaultAsync()

            End Using

        End Function

        Private Sub Insert(
                        division As Division,
                        existingDivision As Division,
                        context As PayrollContext)

            If existingDivision IsNot Nothing Then

                Throw New ArgumentException($"Division name already exists under { existingDivision.ParentDivision.Name }!")

            End If

            division.CreatedBy = z_User

            context.Divisions.Add(division)
        End Sub

        Private Sub Update(
                        division As Division,
                        existingDivision As Division,
                        context As PayrollContext)

            If existingDivision IsNot Nothing AndAlso
                Nullable.Equals(division.RowID, existingDivision.RowID) = False Then

                Throw New ArgumentException($"Division name already exists under { existingDivision.ParentDivision.Name }!")

            End If

            division.LastUpdBy = z_User

            context.Divisions.Attach(division)
            context.Entry(division).State = EntityState.Modified

        End Sub

#End Region

    End Class

End Namespace
