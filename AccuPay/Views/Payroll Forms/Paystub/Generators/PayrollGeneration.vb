Option Strict On

Imports System.Collections.Concurrent
Imports System.Threading.Tasks
Imports AccuPay.Data.Entities
Imports AccuPay.Data.Helpers
Imports AccuPay.Data.Repositories
Imports AccuPay.Data.Services
Imports AccuPay.Data.ValueObjects
Imports Microsoft.Extensions.DependencyInjection

Public Class PayrollGeneration
    Inherits ProgressGenerator

    Private Const FormEntityName As String = "Payroll"

    Private ReadOnly _employees As IEnumerable(Of Employee)

    Private _results As BlockingCollection(Of PaystubEmployeeResult)

    Public Sub New(employees As IEnumerable(Of Employee), Optional additionalProgressCount As Integer = 0)

        MyBase.New(employees.Where(Function(e) e IsNot Nothing AndAlso e.RowID IsNot Nothing).Count() + additionalProgressCount)

        _employees = employees.
            Where(Function(e) e IsNot Nothing AndAlso e.RowID IsNot Nothing).
            OrderBy(Function(e) e.FullNameWithMiddleInitialLastNameFirst)

        _results = New BlockingCollection(Of PaystubEmployeeResult)()

    End Sub

    Public Async Function Start(resources As PayrollResources, payPeriod As TimePeriod) As Task

        If resources Is Nothing Then
            Throw New ArgumentNullException("Resources cannot be null.")
        End If

        For Each employee In _employees

            Try
                Dim generator = MainServiceProvider.GetRequiredService(Of PayrollGenerator)
                Dim result = Await generator.Start(
                    organizationId:=z_OrganizationID,
                    userId:=z_User,
                    employeeId:=employee.RowID.Value,
                    resources:=resources)

                _results.Add(result)

                If result.IsSuccess Then

                    SetCurrentMessage($"Finished generating [{employee.EmployeeNo}] {employee.FullNameWithMiddleInitialLastNameFirst}.")

                    Await RecordPaystubGenerated(result, payPeriod)
                Else

                    SetCurrentMessage($"Failure generating [{employee.EmployeeNo}] {employee.FullNameWithMiddleInitialLastNameFirst}.")
                End If
            Catch ex As Exception

                SetCurrentMessage($"Failure generating [{employee.EmployeeNo}] {employee.FullNameWithMiddleInitialLastNameFirst}.")

                _results.Add(PaystubEmployeeResult.Error(employee, $"Failure to generate paystub for employee {employee.EmployeeNo} {ex.Message}."))

            End Try

            IncreaseProgress()
        Next

        SetResults(_results.ToList())

    End Function

    Private Async Function RecordPaystubGenerated(result As PaystubEmployeeResult, payPeriod As TimePeriod) As Task

        Dim payPeriodString = $"'{payPeriod.Start.ToShortDateString()}' to '{payPeriod.End.ToShortDateString()}'"

        Dim activityItem = New List(Of UserActivityItem) From {
            New UserActivityItem() With
            {
                .EntityId = result.PaystubId.Value,
                .Description = $"Generated paystub for payroll {payPeriodString}.",
                .ChangedEmployeeId = result.EmployeeId
            }
        }

        Dim userActivityService = MainServiceProvider.GetRequiredService(Of UserActivityRepository)
        Await userActivityService.CreateRecordAsync(
          z_User,
          FormEntityName,
          z_OrganizationID,
          UserActivityRepository.RecordTypeAdd,
          activityItem)
    End Function

End Class
