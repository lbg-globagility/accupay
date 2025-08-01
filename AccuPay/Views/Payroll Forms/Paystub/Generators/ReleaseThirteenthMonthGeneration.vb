Option Strict On

Imports System.Collections.Concurrent
Imports System.Threading.Tasks
Imports AccuPay.Core.Entities
Imports AccuPay.Core.Exceptions
Imports AccuPay.Core.Helpers
Imports AccuPay.Core.Interfaces
Imports AccuPay.Core.Services
Imports Microsoft.Extensions.DependencyInjection

Public Class ReleaseThirteenthMonthGeneration
    Inherits ProgressGenerator

    Private _employeeModels As IEnumerable(Of ThirteenthMonthEmployeeModel)
    Private ReadOnly _selectedAdjustmentTypeId As Integer

    Private _results As BlockingCollection(Of PaystubEmployeeResult)

    Public Sub New(employeeModels As IEnumerable(Of ThirteenthMonthEmployeeModel), selectedAdjustmentTypeId As Integer)

        MyBase.New(employeeModels.Where(Function(e) e IsNot Nothing).Count)

        _employeeModels = employeeModels.
            Where(Function(e) e IsNot Nothing).
            OrderBy(Function(e) e.EmployeeObject.FullNameWithMiddleInitialLastNameFirst)

        _selectedAdjustmentTypeId = selectedAdjustmentTypeId

        _results = New BlockingCollection(Of PaystubEmployeeResult)()

    End Sub

    Public Async Function Start() As Task

        For Each employee In _employeeModels

            _results.Add(Await SaveAdjustments(employee))
        Next

        SetResults(_results.ToList())
    End Function

    Private Async Function SaveAdjustments(employee As ThirteenthMonthEmployeeModel) As Task(Of PaystubEmployeeResult)
        Try
            Dim dataService = MainServiceProvider.GetRequiredService(Of IPaystubDataService)

            Await dataService.UpdateAdjustmentsAsync(
                employee.PaystubObject.RowID.Value,
                GetPaystubAdjustments(employee, employee.ThirteenthMonthAmount),
                z_User)

            SetCurrentMessage($"Finished generating 13th month pay for [{employee.EmployeeObject.EmployeeNo}] {employee.EmployeeObject.FullNameWithMiddleInitialLastNameFirst}.")

            Return PaystubEmployeeResult.Success(employee.EmployeeObject, employee.PaystubObject)
        Catch ex As BusinessLogicException

            SetCurrentMessage($"Failure generating 13th month pay for [{employee.EmployeeObject.EmployeeNo}] {employee.EmployeeObject.FullNameWithMiddleInitialLastNameFirst}.")

            Return PaystubEmployeeResult.Error(employee.EmployeeObject, ex.Message)
        Catch ex As Exception

            Return PaystubEmployeeResult.Error(employee.EmployeeObject, "Failure saving the 13th month pay adjustments.")
        Finally

            IncreaseProgress()
        End Try
    End Function

    Private Function GetPaystubAdjustments(employee As ThirteenthMonthEmployeeModel, amount As Decimal) As ICollection(Of Adjustment)

        Dim allAdjustments As New List(Of Adjustment)

        employee.Adjustments?.ForEach(
            Sub(a)
                allAdjustments.Add(a)
            End Sub)

        For Each adj In allAdjustments
            adj.Is13thMonthPay = False
        Next

        Dim thirteenthMonthAdjustment = allAdjustments.
            Where(Function(a) a.ProductID.Value = _selectedAdjustmentTypeId).
            Where(Function(a) Not a.IsActual).
            FirstOrDefault()

        If thirteenthMonthAdjustment IsNot Nothing Then

            thirteenthMonthAdjustment.Amount = amount
            thirteenthMonthAdjustment.Is13thMonthPay = True
        Else
            Dim adjustment As New Adjustment() With
            {
                .Comment = "13th Month Pay",
                .IsActual = False,
                .OrganizationID = z_OrganizationID,
                .Amount = amount,
                .PaystubID = employee.PaystubObject.RowID.Value,
                .ProductID = _selectedAdjustmentTypeId,
                .Is13thMonthPay = True
            }

            allAdjustments.Add(adjustment)

        End If

        Return allAdjustments

    End Function

End Class
