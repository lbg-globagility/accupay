Option Strict On

Imports AccuPay.Core.Entities
Imports AccuPay.Core.Interfaces
Imports Microsoft.Extensions.DependencyInjection

Public Class EmploymentContractReportProvider
    Implements ILaGlobalEmployeeReport

    Private _reportDocument As EmploymentContract

    Public Property Employee As Employee Implements ILaGlobalEmployeeReport.Employee

    Public Function Output() As Boolean Implements ILaGlobalEmployeeReport.Output
        Dim succeed = False
        Try
            _reportDocument = New EmploymentContract

            SetDataSource()

            succeed = True
        Catch ex As Exception
            Throw ex
        End Try
        Return succeed
    End Function

    Private Async Sub SetDataSource()
        Dim e = _Employee

        Dim parameterSetter = New CrystalReportParameterValueSetter(_reportDocument)
        With parameterSetter
            .SetParameter("employeeMiddleName", e.MiddleName)
            .SetParameter("employeeLastName", e.LastName)
            .SetParameter("employeeFirstName", e.FirstName)
            .SetParameter("gender", e.Gender)
            .SetParameter("jobName", e.Position.Name)
            .SetParameter("salutation", e.Salutation)
            .SetParameter("employeeType", e.EmployeeType)
            .SetParameter("startDate", e.StartDate)

            Dim employeeRepository = MainServiceProvider.GetRequiredService(Of IEmployeeRepository)
            Dim latestSalary = Await employeeRepository.GetCurrentSalaryAsync(e.RowID.Value)

            .SetParameter("salary", If(latestSalary?.BasicSalary, 0))
            .SetParameter("companyName", orgNam)

            Dim area = If(String.IsNullOrWhiteSpace(e.Branch?.Name), String.Empty, e.Branch?.Name)
            .SetParameter("area", area)

        End With

        Dim form = New LaGlobalEmployeeReportForm
        form.SetReportSource(_reportDocument)
        form.Show()
    End Sub

End Class
