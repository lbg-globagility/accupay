Option Strict On

Imports System.Threading.Tasks
Imports Microsoft.EntityFrameworkCore
Imports AccuPay.Utilities.Extensions

Namespace Global.AccuPay.Repository

    Public Class OvertimeRepository

        Public Function GetStatusList() As List(Of String)
            Return New List(Of String) From {
                    Overtime.StatusPending,
                    Overtime.StatusApproved
            }
        End Function

        Public Async Function GetByEmployeeAsync(
            employeeId As Integer?) As _
            Task(Of IEnumerable(Of Overtime))

            Using context = New PayrollContext()

                Return Await context.Overtimes.
                        Where(Function(l) l.EmployeeID.Value = employeeId.Value).
                        ToListAsync

            End Using

        End Function

        Public Async Function SaveAsync(overtime As Overtime, Optional context As PayrollContext = Nothing) As Task

            overtime.OrganizationID = z_OrganizationID
            If overtime.OTStartTime.HasValue Then overtime.OTStartTime = overtime.OTStartTime.Value.StripSeconds
            If overtime.OTEndTime.HasValue Then overtime.OTEndTime = overtime.OTEndTime.Value.StripSeconds

            If context Is Nothing Then

                context = New PayrollContext

                Using context

                    Await SaveAsyncFunction(overtime, context)

                    Await context.SaveChangesAsync

                End Using
            Else
                Await SaveAsyncFunction(overtime, context)
            End If

        End Function

        Private Async Function SaveAsyncFunction(overtime As Overtime, context As PayrollContext) As Task

            If overtime.RowID Is Nothing Then

                overtime.CreatedBy = z_User
                context.Overtimes.Add(overtime)
            Else
                Await Me.UpdateAsync(overtime, context)
            End If
        End Function

        Private Async Function UpdateAsync(overtime As Overtime, context As PayrollContext) As Task

            Dim currentOfficialBusiness = Await context.Overtimes.
                FirstOrDefaultAsync(Function(l) Nullable.Equals(l.RowID, overtime.RowID))

            If currentOfficialBusiness Is Nothing Then Return

            currentOfficialBusiness.LastUpdBy = z_User
            currentOfficialBusiness.OTStartTime = overtime.OTStartTime
            currentOfficialBusiness.OTEndTime = overtime.OTEndTime
            currentOfficialBusiness.OTStartDate = overtime.OTStartDate
            currentOfficialBusiness.OTEndDate = overtime.OTEndDate
            currentOfficialBusiness.Reason = overtime.Reason
            currentOfficialBusiness.Comments = overtime.Comments
            currentOfficialBusiness.Status = overtime.Status

        End Function

    End Class

End Namespace