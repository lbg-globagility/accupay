Option Strict On

Imports System.Collections.Concurrent
Imports System.Threading.Tasks
Imports AccuPay.Core.Entities
Imports AccuPay.Core.Helpers
Imports AccuPay.Core.Services
Imports AccuPay.Core.ValueObjects
Imports Microsoft.Extensions.DependencyInjection

Public Class PayrollGeneration
    Inherits ProgressGenerator

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
                    currentlyLoggedInUserId:=z_User,
                    employeeId:=employee.RowID.Value,
                    resources:=resources)

                _results.Add(result)

                If result.IsSuccess Then

                    SetCurrentMessage($"Finished generating [{employee.EmployeeNo}] {employee.FullNameWithMiddleInitialLastNameFirst}.")
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

End Class
