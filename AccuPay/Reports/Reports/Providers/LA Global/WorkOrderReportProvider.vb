Imports AccuPay.Core.Entities

Public Class WorkOrderReportProvider
    Implements ILaGlobalEmployeeReport

    Public Property Employee As Employee Implements ILaGlobalEmployeeReport.Employee

    Public Function Output() As Boolean Implements ILaGlobalEmployeeReport.Output
        Dim succeed = False
        Dim e = _Employee
        Try
            Dim reportDocument As New WorkOrder

            Dim parameterSetter = New CrystalReportParameterValueSetter(reportDocument)
            With parameterSetter
                Dim nameParts = {e.FirstName, e.MiddleName, e.LastName}
                Dim fullName =
                    String.Join(" ", nameParts.Where(Function(s) Not String.IsNullOrWhiteSpace(s)).ToArray())

                .SetParameter("employeeFullName", fullName)
                .SetParameter("jobName", e.Position.Name)
                .SetParameter("dateToday", Date.Now)
                .SetParameter("assignment", String.Empty)
                .SetParameter("location", String.Empty)
                .SetParameter("workDurationStart", e.StartDate)
                .SetParameter("workDurationEnd", e.StartDate.AddYears(1))
                .SetParameter("signatorySupervisor", String.Empty)

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