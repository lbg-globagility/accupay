Imports System.Threading.Tasks

Public Class DefaultPayslipFullOvertimeBreakdownProvider
    Implements IReportProvider

    Public Property Name As String = "Payslip" Implements IReportProvider.Name

    Public Async Sub Run() Implements IReportProvider.Run

        Dim paystubModels = Await GeneratePaystubModels()

        Dim report As New DefaulltPayslipFullOvertimeBreakdown
        report.SetDataSource(paystubModels)

        Dim viewer As New CrysRepForm()
        viewer.crysrepvwr.ReportSource = report
        viewer.Show()

    End Sub

    Private Function GeneratePaystubModels() As Task(Of List(Of PaystubPayslipModel))

        Dim paystubPayslipModels As New List(Of PaystubPayslipModel)

        paystubPayslipModels.Add(
            New PaystubPayslipModel() With {
                .EmployeeId = 1,
                .EmployeeName = "Josh Santos"
        })

        paystubPayslipModels.Add(
            New PaystubPayslipModel() With {
                .EmployeeId = 2,
                .EmployeeName = "Noel Santos"
        })

        Return Task.Run(Function()
                            Return paystubPayslipModels
                        End Function)
    End Function

End Class