Option Strict On

Imports System.Collections.Concurrent
Imports AccuPay.Data.Entities
Imports AccuPay.Data.Helpers
Imports AccuPay.Data.Services
Imports AccuPay.Data.ValueObjects
Imports Microsoft.Extensions.DependencyInjection

Public Class TimeEntryGeneration
    Inherits ProgressGenerator

    Private ReadOnly _employees As IEnumerable(Of Employee)

    Private _results As BlockingCollection(Of EmployeeResult)

    Public Sub New(employees As IEnumerable(Of Employee), Optional additionalProgressCount As Integer = 0)

        MyBase.New(employees.Where(Function(e) e IsNot Nothing AndAlso e.RowID IsNot Nothing).Count() + additionalProgressCount)

        _employees = employees.
            Where(Function(e) e IsNot Nothing AndAlso e.RowID IsNot Nothing).
            OrderBy(Function(e) e.FullNameWithMiddleInitialLastNameFirst)

        _results = New BlockingCollection(Of EmployeeResult)()
    End Sub

    Friend Sub Start(resources As TimeEntryResources, payPeriod As TimePeriod)

        If resources Is Nothing Then Return

        For Each employee In _employees

            Dim generator = MainServiceProvider.GetRequiredService(Of TimeEntryGenerator)
            Dim result = generator.Start(
                userId:=z_User,
                employeeId:=employee.RowID.Value,
                resources:=resources,
                payPeriod:=payPeriod)

            _results.Add(result)

            If result.IsSuccess Then

                SetCurrentMessage($"Finished generating [{employee.EmployeeNo}] {employee.FullNameWithMiddleInitialLastNameFirst}.")
            Else

                SetCurrentMessage($"An error occurred while generating [{employee.EmployeeNo}] {employee.FullNameWithMiddleInitialLastNameFirst}.")
            End If

            IncreaseProgress()
        Next

        SetResults(_results.ToList())

    End Sub

End Class
