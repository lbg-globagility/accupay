Imports AccuPay.Entity

Public Class EmploymentContractReportProvider
    Implements ILaGlobalEmployeeReport

    Public Property Employee As Employee Implements ILaGlobalEmployeeReport.Employee

    Public Function Output() As Boolean Implements ILaGlobalEmployeeReport.Output
        Dim succeed = False
        Dim e = Employee
        Try
            Dim reportDocument As New EmploymentContract 'HTMLTextInterpretation 'EmploymentContract

            Dim source = New List(Of DummyID)
            For i = 1 To 17
                source.Add(New DummyID() With {.RowID = i})
            Next
            reportDocument.SetDataSource(source)

            Dim form = New LaGlobalEmployeeReportForm
            form.reportViewer.ReportSource = reportDocument
            form.Show()

            succeed = True
        Catch ex As Exception
            Throw ex
        End Try
        Return succeed
    End Function

    Private Class DummyID
        Public Property RowID As Integer
    End Class
End Class
