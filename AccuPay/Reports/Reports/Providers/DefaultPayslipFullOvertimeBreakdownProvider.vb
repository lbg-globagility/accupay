Imports System.Threading.Tasks
Imports AccuPay.Entity
Imports CrystalDecisions.CrystalReports.Engine

Public Class DefaultPayslipFullOvertimeBreakdownProvider
    Implements IReportProvider

    Public Property Name As String = "Payslip" Implements IReportProvider.Name

    Public Async Sub Run() Implements IReportProvider.Run

        Dim payPeriod As New PayPeriod With
        {
        .PayFromDate = New Date(2019, 7, 16),
        .PayToDate = New Date(2019, 7, 30)
        }

        Dim paystubModels = Await GeneratePaystubModels()

        Dim report As New DefaulltPayslipFullOvertimeBreakdown
        report.SetDataSource(paystubModels)

        Dim detailsSection = report.ReportDefinition.Sections(1)
        Dim txtOrganizationName As TextObject = detailsSection.ReportObjects("txtOrganizationName")
        Dim txtPayPeriod As TextObject = detailsSection.ReportObjects("txtPayPeriod")

        txtOrganizationName.Text = orgNam.ToUpper
        txtPayPeriod.Text = $"Payslip for the period of {payPeriod.PayFromDate.ToShortDateString} to {payPeriod.PayToDate.ToShortDateString}"

        Dim viewer As New CrysRepForm()
        viewer.crysrepvwr.ReportSource = report
        viewer.Show()

    End Sub

    Private Function GeneratePaystubModels() As Task(Of List(Of PaystubPayslipModel))

        Dim paystubPayslipModels As New List(Of PaystubPayslipModel)

        paystubPayslipModels.Add(
            New PaystubPayslipModel() With {
                .EmployeeNumber = "169899",
                .EmployeeName = "Josh Santos",
                .RegularPay = 20000,
                .BasicHours = 96,
                .BasicPay = 18200,
                .Allowance = 2000,
                .Ecola = 0.6,
                .AbsentHours = 16,
                .AbsentAmount = 1400,
                .LateAndUndertimeHours = 4,
                .LateAndUndertimeAmount = 350,
                .GrossPay = 18000,
                .SSSAmount = 480,
                .PhilHealthAmount = 187.5,
                .PagibigAmount = 100,
                .TaxWithheldAmount = 0.5,
                .NetPay = 17500,
                .OvertimeHours = 2,
                .OvertimePay = 671,
                .RegularHolidayHours = 8,
                .RegularHolidayPay = 1074
            }.CreateOvertimeSummaryColumns())

        paystubPayslipModels.Add(
            New PaystubPayslipModel() With {
                .EmployeeNumber = "167724",
                .EmployeeName = "Vincent Dayagbil",
                .RegularPay = 30000,
                .BasicHours = 103,
                .BasicPay = 29900,
                .Allowance = 5000,
                .Ecola = 1000,
                .AbsentHours = 8,
                .AbsentAmount = 800,
                .LateAndUndertimeHours = 2,
                .LateAndUndertimeAmount = 200,
                .GrossPay = 29000,
                .SSSAmount = 560,
                .PhilHealthAmount = 216.75,
                .PagibigAmount = 200,
                .TaxWithheldAmount = 1200,
                .NetPay = 17500,
                .SpecialHolidayRestDayNightDiffOTHours = 3,
                .SpecialHolidayRestDayNightDiffOTPay = 210,
                .RegularHolidayRestDayNightDiffOTHours = 7,
                .RegularHolidayRestDayNightDiffOTPay = 480
        }.CreateOvertimeSummaryColumns())

        paystubPayslipModels.Add(
            New PaystubPayslipModel() With {
                .EmployeeNumber = "169908",
                .EmployeeName = "Mark Galolo",
                .RegularPay = 30000,
                .BasicHours = 103,
                .BasicPay = 29900,
                .Allowance = 5000,
                .Ecola = 1000,
                .AbsentHours = 8,
                .AbsentAmount = 800,
                .LateAndUndertimeHours = 2,
                .LateAndUndertimeAmount = 200,
                .GrossPay = 29000,
                .SSSAmount = 560,
                .PhilHealthAmount = 216.75,
                .PagibigAmount = 200,
                .TaxWithheldAmount = 1200,
                .NetPay = 17500,
                .OvertimeHours = 10,
                .OvertimePay = 5000.789,
                .NightDiffHours = 10,
                .NightDiffPay = 5000.789,
                .NightDiffOvertimeHours = 10,
                .NightDiffOvertimePay = 5000.789,
                .RestDayHours = 10,
                .RestDayPay = 5000.789,
                .RestDayOTHours = 10,
                .RestDayOTPay = 5000.789,
                .SpecialHolidayHours = 10,
                .SpecialHolidayPay = 5000.789,
                .SpecialHolidayOTHours = 10,
                .SpecialHolidayOTPay = 5000.789,
                .RegularHolidayHours = 10,
                .RegularHolidayPay = 5000.789,
                .RegularHolidayOTHours = 10,
                .RegularHolidayOTPay = 5000.789,
                .RestDayNightDiffHours = 10,
                .RestDayNightDiffPay = 5000.789,
                .RestDayNightDiffOTHours = 10,
                .RestDayNightDiffOTPay = 5000.789,
                .SpecialHolidayNightDiffHours = 10,
                .SpecialHolidayNightDiffPay = 5000.789,
                .SpecialHolidayNightDiffOTHours = 10,
                .SpecialHolidayNightDiffOTPay = 5000.789,
                .SpecialHolidayRestDayHours = 10,
                .SpecialHolidayRestDayPay = 5000.789,
                .SpecialHolidayRestDayOTHours = 10,
                .SpecialHolidayRestDayOTPay = 5000.789,
                .SpecialHolidayRestDayNightDiffHours = 10,
                .SpecialHolidayRestDayNightDiffPay = 5000.789,
                .SpecialHolidayRestDayNightDiffOTHours = 10,
                .SpecialHolidayRestDayNightDiffOTPay = 5000.789,
                .RegularHolidayNightDiffHours = 10,
                .RegularHolidayNightDiffPay = 5000.789,
                .RegularHolidayNightDiffOTHours = 10,
                .RegularHolidayNightDiffOTPay = 5000.789,
                .RegularHolidayRestDayHours = 10,
                .RegularHolidayRestDayPay = 5000.789,
                .RegularHolidayRestDayOTHours = 10,
                .RegularHolidayRestDayOTPay = 5000.789,
                .RegularHolidayRestDayNightDiffHours = 10,
                .RegularHolidayRestDayNightDiffPay = 5000.789,
                .RegularHolidayRestDayNightDiffOTHours = 10,
                .RegularHolidayRestDayNightDiffOTPay = 5000.789
        }.CreateOvertimeSummaryColumns())

        paystubPayslipModels.Add(
            New PaystubPayslipModel() With {
                .EmployeeNumber = "168360",
                .EmployeeName = "Noel Santos",
                .RegularPay = 30000,
                .BasicHours = 103,
                .BasicPay = 29900,
                .Allowance = 5000,
                .Ecola = 1000,
                .AbsentHours = 8,
                .AbsentAmount = 800,
                .LateAndUndertimeHours = 2,
                .LateAndUndertimeAmount = 200,
                .GrossPay = 29000,
                .SSSAmount = 560,
                .PhilHealthAmount = 216.75,
                .PagibigAmount = 200,
                .TaxWithheldAmount = 1200,
                .NetPay = 17500,
                .OvertimeHours = 10,
                .OvertimePay = 5000.789,
                .NightDiffHours = 10,
                .NightDiffPay = 5000.789,
                .NightDiffOvertimeHours = 10,
                .NightDiffOvertimePay = 5000.789,
                .RestDayHours = 10,
                .RestDayPay = 5000.789,
                .RestDayOTHours = 10,
                .RestDayOTPay = 5000.789,
                .SpecialHolidayHours = 10,
                .SpecialHolidayPay = 5000.789,
                .SpecialHolidayOTHours = 10,
                .SpecialHolidayOTPay = 5000.789,
                .RegularHolidayHours = 10,
                .RegularHolidayPay = 5000.789,
                .RegularHolidayOTHours = 10,
                .RegularHolidayOTPay = 5000.789,
                .RestDayNightDiffHours = 10,
                .RestDayNightDiffPay = 5000.789,
                .RestDayNightDiffOTHours = 10,
                .RestDayNightDiffOTPay = 5000.789,
                .SpecialHolidayNightDiffHours = 10,
                .SpecialHolidayNightDiffPay = 5000.789,
                .SpecialHolidayNightDiffOTHours = 10,
                .SpecialHolidayNightDiffOTPay = 5000.789,
                .SpecialHolidayRestDayHours = 10,
                .SpecialHolidayRestDayPay = 5000.789,
                .SpecialHolidayRestDayOTHours = 10,
                .SpecialHolidayRestDayOTPay = 5000.789,
                .SpecialHolidayRestDayNightDiffHours = 10,
                .SpecialHolidayRestDayNightDiffPay = 5000.789,
                .SpecialHolidayRestDayNightDiffOTHours = 10,
                .SpecialHolidayRestDayNightDiffOTPay = 5000.789,
                .RegularHolidayNightDiffHours = 10,
                .RegularHolidayNightDiffPay = 5000.789,
                .RegularHolidayNightDiffOTHours = 10,
                .RegularHolidayNightDiffOTPay = 5000.789,
                .RegularHolidayRestDayHours = 10,
                .RegularHolidayRestDayPay = 5000.789,
                .RegularHolidayRestDayOTHours = 10,
                .RegularHolidayRestDayOTPay = 5000.789,
                .RegularHolidayRestDayNightDiffHours = 10,
                .RegularHolidayRestDayNightDiffPay = 5000.789,
                .RegularHolidayRestDayNightDiffOTHours = 10,
                .RegularHolidayRestDayNightDiffOTPay = 5000.789
        }.CreateOvertimeSummaryColumns())

        Return Task.Run(Function()
                            Return paystubPayslipModels
                        End Function)
    End Function

End Class