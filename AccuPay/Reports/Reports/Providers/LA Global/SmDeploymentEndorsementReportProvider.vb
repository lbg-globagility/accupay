Imports AccuPay.Data.Entities

Public Class SmDeploymentEndorsementReportProvider
    Implements ILaGlobalEmployeeReport

    Public Property Employee As Employee Implements ILaGlobalEmployeeReport.Employee

    Public Function Output() As Boolean Implements ILaGlobalEmployeeReport.Output
        Dim succeed = False
        Dim e = Employee
        Try
            Dim reportDocument As New SMDeploymentEndorsement

            Dim parameterSetter = New CrystalReportParameterValueSetter(reportDocument)
            With parameterSetter
                .SetParameter("employeeMiddleName", e.MiddleName)
                .SetParameter("employeeLastName", e.LastName)
                .SetParameter("employeeFirstName", e.FirstName)
                .SetParameter("employeeSssNum", e.SssNo)
                .SetParameter("gender", e.Gender)

                Dim branchName = e.Branch?.Name
                .SetParameter("storeLocation", If(String.IsNullOrWhiteSpace(branchName), String.Empty, branchName))

                Dim dateNow = Date.UtcNow
                .SetParameter("startDate", dateNow.Date)
                .SetParameter("endDate", dateNow.Date)

                Dim mrMrs = If(e.Gender = "M", "Mr.", "Ms.")
                .SetParameter("salutation", $"{mrMrs} {e.LastName}")

            End With

            Dim form = New LaGlobalEmployeeReportForm
            form.SetReportSource(reportDocument)
            form.Show()

            succeed = True
        Catch ex As Exception
            Throw ex
        End Try
        Return succeed
    End Function

End Class