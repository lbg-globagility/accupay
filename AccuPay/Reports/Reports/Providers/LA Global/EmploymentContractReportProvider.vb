Imports AccuPay.Entity

Public Class EmploymentContractReportProvider
    Implements ILaGlobalEmployeeReport

    Public Property Employee As Employee Implements ILaGlobalEmployeeReport.Employee

    Public Function Output() As Boolean Implements ILaGlobalEmployeeReport.Output
        Dim succeed = False
        Dim e = _Employee
        Try
            Dim reportDocument As New EmploymentContract

            Dim parameterSetter = New CrystalReportParameterValueSetter(reportDocument)
            With parameterSetter
                .SetParameter("employeeMiddleName", e.MiddleName)
                .SetParameter("employeeLastName", e.LastName)
                .SetParameter("employeeFirstName", e.FirstName)
                .SetParameter("gender", e.Gender)
                .SetParameter("jobName", e.Position.Name)
                .SetParameter("salutation", e.Salutation)
                .SetParameter("employeeType", e.EmployeeType)
                .SetParameter("startDate", e.StartDate)

                Dim latestSalary = e.Salaries.
                    Where(Function(s) s.IsIndefinite).
                    FirstOrDefault
                If latestSalary Is Nothing Then
                    latestSalary = e.Salaries.
                        OrderByDescending(Function(s) s.EffectiveFrom).
                        ThenByDescending(Function(s) s.EffectiveTo).
                        FirstOrDefault
                End If
                .SetParameter("salary", latestSalary.BasicSalary)
                .SetParameter("companyName", orgNam)

            End With

            Dim form = New LaGlobalEmployeeReportForm
            form.reportViewer.ReportSource = reportDocument
            form.Show()

            succeed = True
        Catch ex As Exception
            Throw ex
        End Try
        Return succeed
    End Function

End Class
