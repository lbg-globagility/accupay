Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Core.Entities
Imports AccuPay.Core.Interfaces
Imports AccuPay.Utilities.Extensions
Imports Microsoft.Extensions.DependencyInjection

Namespace AccuPay.Desktop.Helpers

    Public Class EcolaHelper

        Public Shared Async Function CreateEcola(
            employeeId As Integer,
            ecolaAmount As Decimal,
            startDate As Date) As Task(Of Allowance)

            Dim repository = MainServiceProvider.GetRequiredService(Of IAllowanceRepository)
            Dim dataService = MainServiceProvider.GetRequiredService(Of IAllowanceDataService)

            Dim latestEcola = Await repository.GetEmployeeEcolaAsync(employeeId, z_OrganizationID, startDate)

            If latestEcola IsNot Nothing AndAlso latestEcola.EffectiveEndDate Is Nothing Then

                latestEcola.EffectiveEndDate = startDate.AddDays(-1).ToMinimumHourValue()

                Dim dataServiceUpdate = MainServiceProvider.GetRequiredService(Of IAllowanceDataService)
                Await dataServiceUpdate.SaveAsync(latestEcola, z_User)

            End If

            Return Await dataService.CreateEcola(
                employeeId:=employeeId,
                organizationId:=z_OrganizationID,
                currentlyLoggedInUserId:=z_User,
                startDate:=startDate,
                allowanceFrequency:=Allowance.FREQUENCY_DAILY,
                amount:=ecolaAmount)

        End Function

        Public Shared Async Function UpdateEcola(
            ecolaAllowanceId As Integer,
            startDate As Date,
            ecolaAmount As Decimal) As Task

            Dim repository = MainServiceProvider.GetRequiredService(Of IAllowanceRepository)
            Dim dataService = MainServiceProvider.GetRequiredService(Of IAllowanceDataService)

            Dim ecolaAllowance = Await repository.GetByIdAsync(ecolaAllowanceId)

            ecolaAllowance.EffectiveStartDate = startDate
            ecolaAllowance.Amount = ecolaAmount

            Await dataService.SaveAsync(ecolaAllowance, z_User)

        End Function

    End Class

End Namespace
