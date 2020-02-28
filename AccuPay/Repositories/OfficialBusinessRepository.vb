Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Utilities.Extensions
Imports Microsoft.EntityFrameworkCore

Namespace Global.AccuPay.Repository

    Public Class OfficialBusinessRepository

        Public Function GetStatusList() As List(Of String)
            Return New List(Of String) From {
                    OfficialBusiness.StatusPending,
                    OfficialBusiness.StatusApproved
            }
        End Function

        Public Async Function GetByIdAsync(id As Integer?) As Task(Of OfficialBusiness)

            Using context = New PayrollContext()

                Return Await context.OfficialBusinesses.
                    FirstOrDefaultAsync(Function(l) l.RowID.Value = id.Value)

            End Using

        End Function

        Public Async Function GetByEmployeeAsync(
            employeeId As Integer?) As _
            Task(Of IEnumerable(Of OfficialBusiness))

            Using context = New PayrollContext()

                Return Await context.OfficialBusinesses.
                        Where(Function(l) l.EmployeeID.Value = employeeId.Value).
                        ToListAsync

            End Using

        End Function

        Public Async Function DeleteAsync(id As Integer?) As Task
            Using context = New PayrollContext()

                Dim officialBusiness = Await GetByIdAsync(id)

                context.Remove(officialBusiness)

                Await context.SaveChangesAsync()

            End Using
        End Function

        Public Async Function SaveManyAsync(currentOfficialBusinesses As List(Of OfficialBusiness)) As Task

            Using context As New PayrollContext

                For Each officialBusiness In currentOfficialBusinesses

                    Await Me.SaveAsync(officialBusiness, context)

                    Await context.SaveChangesAsync()
                Next

            End Using

        End Function

        Public Async Function SaveAsync(officialBusiness As OfficialBusiness, Optional context As PayrollContext = Nothing) As Task

            officialBusiness.OrganizationID = z_OrganizationID
            If officialBusiness.StartTime.HasValue Then officialBusiness.StartTime = officialBusiness.StartTime.Value.StripSeconds
            If officialBusiness.EndTime.HasValue Then officialBusiness.EndTime = officialBusiness.EndTime.Value.StripSeconds

            If context Is Nothing Then

                context = New PayrollContext

                Using context

                    Await SaveAsyncFunction(officialBusiness, context)

                    Await context.SaveChangesAsync

                End Using
            Else
                Await SaveAsyncFunction(officialBusiness, context)
            End If

        End Function

        Private Async Function SaveAsyncFunction(officialBusiness As OfficialBusiness, context As PayrollContext) As Task

            If context.OfficialBusinesses.
                Where(Function(l) If(officialBusiness.RowID Is Nothing, True, Nullable.Equals(officialBusiness.RowID, l.RowID) = False)).
                Where(Function(l) l.EmployeeID.Value = officialBusiness.EmployeeID.Value).
                Where(Function(l) l.StartDate = officialBusiness.StartDate).
                Any() Then

                Throw New ArgumentException($"Employee already has an official business for {officialBusiness.StartDate.ToShortDateString()}")
            End If

            If officialBusiness.RowID Is Nothing Then

                officialBusiness.CreatedBy = z_User
                context.OfficialBusinesses.Add(officialBusiness)
            Else
                Await Me.UpdateAsync(officialBusiness, context)
            End If
        End Function

        Private Async Function UpdateAsync(officialBusiness As OfficialBusiness, context As PayrollContext) As Task

            Dim currentOfficialBusiness = Await context.OfficialBusinesses.
                FirstOrDefaultAsync(Function(l) Nullable.Equals(l.RowID, officialBusiness.RowID))

            If currentOfficialBusiness Is Nothing Then Return

            currentOfficialBusiness.LastUpdBy = z_User
            currentOfficialBusiness.StartTime = officialBusiness.StartTime
            currentOfficialBusiness.EndTime = officialBusiness.EndTime
            currentOfficialBusiness.StartDate = officialBusiness.StartDate
            currentOfficialBusiness.EndDate = officialBusiness.EndDate
            currentOfficialBusiness.Reason = officialBusiness.Reason
            currentOfficialBusiness.Comments = officialBusiness.Comments
            currentOfficialBusiness.Status = officialBusiness.Status

        End Function

    End Class

End Namespace