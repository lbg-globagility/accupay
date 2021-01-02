Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Core.Interfaces
Imports Microsoft.Extensions.DependencyInjection

Namespace AccuPay.Desktop.Helpers

    Public Class EcolaHelper

        Public Shared Async Function SaveEcola(ecolaAllowanceId As Integer, ecolaAmount As Decimal) As Task

            Dim repository = MainServiceProvider.GetRequiredService(Of IAllowanceRepository)
            Dim dataService = MainServiceProvider.GetRequiredService(Of IAllowanceDataService)

            Dim ecolaAllowance = Await repository.GetByIdAsync(ecolaAllowanceId)

            ecolaAllowance.Amount = ecolaAmount

            Await dataService.SaveAsync(ecolaAllowance, z_User)

        End Function

    End Class

End Namespace
