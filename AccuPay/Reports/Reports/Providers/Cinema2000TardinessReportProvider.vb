Imports CrystalDecisions.CrystalReports.Engine

Public Class Cinema2000TardinessReportProvider
    Implements IReportProvider

    Public Property Name As String = "Tardiness Report" Implements IReportProvider.Name

    Public Async Sub Run() Implements IReportProvider.Run

        Dim firstDate As New Date(2019, 10, 1)

        Try

            Dim report = New Cinemas_Tardiness_Report

            Dim txtOrgName As TextObject = DirectCast(report.ReportDefinition.Sections(2).ReportObjects("txtOrgName"), TextObject)
            txtOrgName.Text = orgNam.ToUpper

            Dim txtReportName As TextObject = DirectCast(report.ReportDefinition.Sections(1).ReportObjects("txtReportName"), TextObject)
            txtReportName.Text = $"Tadiness Report - {firstDate.ToString("MMMM yyyy")}"

            Dim txtAddress = DirectCast(report.ReportDefinition.Sections(2).ReportObjects("txtAddress"), TextObject)

            txtAddress.Text = PayrollTools.GetOrganizationAddress()

            Dim tardinessReportModels = Await GetTardinessReportModels()

            report.SetDataSource(tardinessReportModels)

            Dim crvwr As New CrysRepForm
            crvwr.crysrepvwr.ReportSource = report
            crvwr.Show()
        Catch ex As Exception
            MsgBox(getErrExcptn(ex, Me.Name))
        End Try

    End Sub

    Private Async Function GetTardinessReportModels() As Threading.Tasks.Task(Of List(Of CinemaTardinessReportModel))

        Dim tardinessReportModels As New List(Of CinemaTardinessReportModel)

        tardinessReportModels.Add(
            New CinemaTardinessReportModel() With {
                .EmployeeName = "Andal, Myrna, B.",
                .Days = 2,
                .Hours = 4,
                .NumberOfOffense = 0
        })

        tardinessReportModels.Add(
            New CinemaTardinessReportModel() With {
                .EmployeeName = "Banal, Joseph Brian, A.",
                .Days = 8,
                .Hours = 8.5,
                .NumberOfOffense = 2
        })

        tardinessReportModels.Add(
            New CinemaTardinessReportModel() With {
                .EmployeeName = "Banga, Jessa, O.",
                .Days = 9,
                .Hours = 10,
                .NumberOfOffense = 4
        })

        tardinessReportModels.Add(
            New CinemaTardinessReportModel() With {
                .EmployeeName = "Santos, Joshua Noel C.",
                .Days = 0,
                .Hours = 0,
                .NumberOfOffense = 0
        })

        Return tardinessReportModels

    End Function

End Class