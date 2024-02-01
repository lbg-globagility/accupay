Imports System.Collections.Concurrent
Imports System.Threading.Tasks
Imports AccuPay.Core.Entities
Imports AccuPay.Core.Entities.LeaveReset
Imports AccuPay.Core.Helpers
Imports AccuPay.Core.Interfaces
Imports AccuPay.Core.Services.LeaveBalanceReset
Imports Microsoft.Extensions.DependencyInjection

Public Class LeaveBalanceResetGeneration
    Inherits ProgressGenerator

    Private ReadOnly _employees As IEnumerable(Of Employee)
    Private _results As BlockingCollection(Of LeaveResetResult)

    Public Sub New(employees As IEnumerable(Of Employee), Optional additionalProgressCount As Integer = 0)

        MyBase.New(employees.Where(Function(e) e IsNot Nothing AndAlso e.RowID IsNot Nothing).Count() + additionalProgressCount)

        _employees = employees.
            Where(Function(e) e IsNot Nothing AndAlso e.RowID IsNot Nothing).
            OrderBy(Function(e) e.FullNameWithMiddleInitialLastNameFirst)

        _results = New BlockingCollection(Of LeaveResetResult)()
    End Sub

    Public Async Function Start(leaveReset As LeaveReset,
            resources As ILeaveResetResources) As Task
        If resources Is Nothing AndAlso resources Is Nothing Then
            Throw New ArgumentNullException("Resources cannot be null.")
        End If

        For Each employee In _employees
            Try
                Dim leaveCalculator = MainServiceProvider.GetRequiredService(Of ILeaveBalanceResetCalculator)
                Dim result = Await leaveCalculator.Start(
                    organizationId:=z_OrganizationID,
                    userId:=z_User,
                    employeeId:=employee.RowID.Value,
                    leaveReset:=leaveReset,
                    resources:=resources)

                _results.Add(result)

                If result.IsSuccess Then

                    SetCurrentMessage($"Finished leave recompute [{employee.EmployeeNo}] {employee.FullNameWithMiddleInitialLastNameFirst}.")
                Else

                    SetCurrentMessage($"Failure generating [{employee.EmployeeNo}] {employee.FullNameWithMiddleInitialLastNameFirst}.")
                End If
            Catch ex As Exception

                SetCurrentMessage($"Failure generating [{employee.EmployeeNo}] {employee.FullNameWithMiddleInitialLastNameFirst}.")

                _results.Add(LeaveResetResult.Error(employee, $"Failure to recompute leave for employee {employee.EmployeeNo} {ex.Message}."))

            End Try

            IncreaseProgress()
        Next

        SetResults(_results.ToList())
    End Function
End Class
