Imports System.Threading.Tasks
Imports AccuPay.Data.Helpers
Imports AccuPay.Entity
Imports AccuPay.Loans
Imports CrystalDecisions.CrystalReports.Engine
Imports Microsoft.EntityFrameworkCore

Public Class DefaultPayslipFullOvertimeBreakdownProvider
    Implements IReportProvider

    Public Property Name As String = "Payslip" Implements IReportProvider.Name

    Public Property IsHidden As Boolean = False Implements IReportProvider.IsHidden

    Public Async Sub Run() Implements IReportProvider.Run

        Dim form As New selectPayPeriod()
        form.GeneratePayroll = False
        form.ShowDialog()

        Dim payPeriod As PayPeriod = form.PayPeriod

        If payPeriod Is Nothing Then Return

        Dim paystubModels = Await GeneratePaystubModels(payPeriod)

        Dim report As New Payslip.DefaulltPayslipFullOvertimeBreakdown
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

    Private Async Function GeneratePaystubModels(payPeriod As PayPeriod, Optional isActual As Boolean = False) As Task(Of List(Of PaystubPayslipModel))

        Dim paystubPayslipModels As New List(Of PaystubPayslipModel)

        'TODO Create PaystubPayslipModel from database THEN check if equal to new payslip
        Using context As New PayrollContext

            Dim paystubs = Await context.Paystubs.
                                    Include(Function(p) p.Employee).
                                    Include(Function(p) p.Actual).
                                    Where(Function(p) p.PayPeriodID.Value = payPeriod.RowID.Value).
                                    Where(Function(p) p.OrganizationID.Value = z_OrganizationID).
                                    ToListAsync

            paystubs = paystubs.
                        OrderBy(Function(p) p.Employee.FullNameWithMiddleInitialLastNameFirst).
                        ToList

            Dim loans = Await context.LoanTransactions.
                                        Include(Function(l) l.LoanSchedule).
                                        Include(Function(l) l.LoanSchedule.LoanType).
                                        Include(Function(l) l.Paystub).
                                        Where(Function(l) l.Paystub.PayPeriodID = payPeriod.RowID.Value).
                                        ToListAsync

            Dim adjustments = Await context.Adjustments.
                                        Include(Function(a) a.Product).
                                        Include(Function(a) a.Paystub).
                                        Where(Function(a) a.Paystub.PayPeriodID = payPeriod.RowID.Value).
                                        ToListAsync

            Dim actualAdjustments = Await context.ActualAdjustments.
                                        Include(Function(a) a.Product).
                                        Include(Function(a) a.Paystub).
                                        Where(Function(a) a.Paystub.PayPeriodID = payPeriod.RowID.Value).
                                        ToListAsync

            Dim employeeSalaries = Await context.Salaries.
                                        Where(Function(s) s.OrganizationID.Value = z_OrganizationID).
                                        Where(Function(s) s.EffectiveFrom <= payPeriod.PayToDate).
                                        Where(Function(s) payPeriod.PayFromDate <= If(s.EffectiveTo, payPeriod.PayFromDate)).
                                        ToListAsync

            Dim ecolas = Await context.AllowanceItems.
                                        Include(Function(p) p.Allowance).
                                        Include(Function(p) p.Allowance.Product).
                                        Where(Function(p) p.Allowance.Product.PartNo.ToUpper = ProductConstant.ECOLA.ToUpper).
                                        ToListAsync

            For Each paystub In paystubs

                Dim employeeId = paystub.EmployeeID.Value

                Dim employeeSalary = employeeSalaries.
                                        FirstOrDefault(Function(s) s.EmployeeID.Value = employeeId)

                paystub.Ecola = If(ecolas.
                                Where(Function(e) e.PaystubID.Value = paystub.RowID.Value).
                                FirstOrDefault?.Amount, 0)

                Dim salary = If(isActual, employeeSalary.TotalSalary, employeeSalary.BasicSalary)

                Dim allAdjustments As List(Of PaystubPayslipModel.Adjustment) = GetEmployeeAdjustments(actualAdjustments, employeeId)
                allAdjustments.AddRange(GetEmployeeAdjustments(adjustments, employeeId))

                Dim paystubPayslipModel As New PaystubPayslipModel(paystub.Employee)

                paystubPayslipModel.EmployeeId = employeeId
                paystubPayslipModel.EmployeeNumber = paystub.Employee?.EmployeeNo
                paystubPayslipModel.EmployeeName = paystub.Employee?.FullNameWithMiddleInitialLastNameFirst
                paystubPayslipModel.RegularPay = salary
                paystubPayslipModel.BasicHours = paystub.BasicHours
                paystubPayslipModel.Allowance = paystub.TotalAllowance - paystub.Ecola
                paystubPayslipModel.Ecola = paystub.Ecola
                paystubPayslipModel.AbsentHours = paystub.AbsentHours + If(paystub.Employee.IsMonthly, paystub.LeaveHours, 0)
                paystubPayslipModel.AbsentAmount = If(isActual,
                            paystub.Actual.AbsenceDeduction + If(paystub.Employee.IsMonthly, paystub.Actual.LeavePay, 0),
                            paystub.AbsenceDeduction + If(paystub.Employee.IsMonthly, paystub.LeavePay, 0))
                paystubPayslipModel.LateAndUndertimeHours = paystub.LateHours + paystub.UndertimeHours
                paystubPayslipModel.LateAndUndertimeAmount = If(isActual, paystub.Actual.LateDeduction + paystub.UndertimeDeduction, paystub.LateDeduction + paystub.UndertimeDeduction)
                paystubPayslipModel.LeaveHours = paystub.LeaveHours
                paystubPayslipModel.LeavePay = If(isActual, paystub.Actual.LeavePay, paystub.LeavePay)
                paystubPayslipModel.GrossPay = If(isActual, paystub.Actual.GrossPay, paystub.GrossPay)
                paystubPayslipModel.SSSAmount = paystub.SssEmployeeShare
                paystubPayslipModel.PhilHealthAmount = paystub.PhilHealthEmployeeShare
                paystubPayslipModel.PagibigAmount = paystub.HdmfEmployeeShare
                paystubPayslipModel.NetPay = If(isActual, paystub.Actual.NetPay, paystub.NetPay)
                paystubPayslipModel.TaxWithheldAmount = paystub.WithholdingTax
                paystubPayslipModel.Loans = GetEmployeeLoans(loans, employeeId)
                paystubPayslipModel.Adjustments = allAdjustments

                'overtimes
                paystubPayslipModel.OvertimeHours = paystub.OvertimeHours
                paystubPayslipModel.OvertimePay = If(isActual, paystub.Actual.OvertimePay, paystub.OvertimePay)
                paystubPayslipModel.NightDiffHours = paystub.NightDiffHours
                paystubPayslipModel.NightDiffPay = If(isActual, paystub.Actual.NightDiffPay, paystub.NightDiffPay)
                paystubPayslipModel.NightDiffOvertimeHours = paystub.NightDiffOvertimeHours
                paystubPayslipModel.NightDiffOvertimePay = If(isActual, paystub.Actual.NightDiffOvertimePay, paystub.NightDiffOvertimePay)
                paystubPayslipModel.RestDayHours = paystub.RestDayHours
                paystubPayslipModel.RestDayPay = If(isActual, paystub.Actual.RestDayPay, paystub.RestDayPay)
                paystubPayslipModel.RestDayOTHours = paystub.RestDayOTHours
                paystubPayslipModel.RestDayOTPay = If(isActual, paystub.Actual.RestDayOTPay, paystub.RestDayOTPay)
                paystubPayslipModel.SpecialHolidayHours = paystub.SpecialHolidayHours
                paystubPayslipModel.SpecialHolidayPay = If(isActual, paystub.Actual.SpecialHolidayPay, paystub.SpecialHolidayPay)
                paystubPayslipModel.SpecialHolidayOTHours = paystub.SpecialHolidayOTHours
                paystubPayslipModel.SpecialHolidayOTPay = If(isActual, paystub.Actual.SpecialHolidayOTPay, paystub.SpecialHolidayOTPay)
                paystubPayslipModel.RegularHolidayHours = paystub.RegularHolidayHours
                paystubPayslipModel.RegularHolidayPay = If(isActual, paystub.Actual.RegularHolidayPay, paystub.RegularHolidayPay)
                paystubPayslipModel.RegularHolidayOTHours = paystub.RegularHolidayOTHours
                paystubPayslipModel.RegularHolidayOTPay = If(isActual, paystub.Actual.RegularHolidayOTPay, paystub.RegularHolidayOTPay)
                paystubPayslipModel.RestDayNightDiffHours = paystub.RestDayNightDiffHours
                paystubPayslipModel.RestDayNightDiffPay = If(isActual, paystub.Actual.RestDayNightDiffPay, paystub.RestDayNightDiffPay)
                paystubPayslipModel.RestDayNightDiffOTHours = paystub.RestDayNightDiffOTHours
                paystubPayslipModel.RestDayNightDiffOTPay = If(isActual, paystub.Actual.RestDayNightDiffOTPay, paystub.RestDayNightDiffOTPay)
                paystubPayslipModel.SpecialHolidayNightDiffHours = paystub.SpecialHolidayNightDiffHours
                paystubPayslipModel.SpecialHolidayNightDiffPay = If(isActual, paystub.Actual.SpecialHolidayNightDiffPay, paystub.SpecialHolidayNightDiffPay)
                paystubPayslipModel.SpecialHolidayNightDiffOTHours = paystub.SpecialHolidayNightDiffOTHours
                paystubPayslipModel.SpecialHolidayNightDiffOTPay = If(isActual, paystub.Actual.SpecialHolidayNightDiffOTPay, paystub.SpecialHolidayNightDiffOTPay)
                paystubPayslipModel.SpecialHolidayRestDayHours = paystub.SpecialHolidayRestDayHours
                paystubPayslipModel.SpecialHolidayRestDayPay = If(isActual, paystub.Actual.SpecialHolidayRestDayPay, paystub.SpecialHolidayRestDayPay)
                paystubPayslipModel.SpecialHolidayRestDayOTHours = paystub.SpecialHolidayRestDayOTHours
                paystubPayslipModel.SpecialHolidayRestDayOTPay = If(isActual, paystub.Actual.SpecialHolidayRestDayOTPay, paystub.SpecialHolidayRestDayOTPay)
                paystubPayslipModel.SpecialHolidayRestDayNightDiffHours = paystub.SpecialHolidayRestDayNightDiffHours
                paystubPayslipModel.SpecialHolidayRestDayNightDiffPay = If(isActual, paystub.Actual.SpecialHolidayRestDayNightDiffPay, paystub.SpecialHolidayRestDayNightDiffPay)
                paystubPayslipModel.SpecialHolidayRestDayNightDiffOTHours = paystub.SpecialHolidayRestDayNightDiffOTHours
                paystubPayslipModel.SpecialHolidayRestDayNightDiffOTPay = If(isActual, paystub.Actual.SpecialHolidayRestDayNightDiffOTPay, paystub.SpecialHolidayRestDayNightDiffOTPay)
                paystubPayslipModel.RegularHolidayNightDiffHours = paystub.RegularHolidayNightDiffHours
                paystubPayslipModel.RegularHolidayNightDiffPay = If(isActual, paystub.Actual.RegularHolidayNightDiffPay, paystub.RegularHolidayNightDiffPay)
                paystubPayslipModel.RegularHolidayNightDiffOTHours = paystub.RegularHolidayNightDiffOTHours
                paystubPayslipModel.RegularHolidayNightDiffOTPay = If(isActual, paystub.Actual.RegularHolidayNightDiffOTPay, paystub.RegularHolidayNightDiffOTPay)
                paystubPayslipModel.RegularHolidayRestDayHours = paystub.RegularHolidayRestDayHours
                paystubPayslipModel.RegularHolidayRestDayPay = If(isActual, paystub.Actual.RegularHolidayRestDayPay, paystub.RegularHolidayRestDayPay)
                paystubPayslipModel.RegularHolidayRestDayOTHours = paystub.RegularHolidayRestDayOTHours
                paystubPayslipModel.RegularHolidayRestDayOTPay = If(isActual, paystub.Actual.RegularHolidayRestDayOTPay, paystub.RegularHolidayRestDayOTPay)
                paystubPayslipModel.RegularHolidayRestDayNightDiffHours = paystub.RegularHolidayRestDayNightDiffHours
                paystubPayslipModel.RegularHolidayRestDayNightDiffPay = If(isActual, paystub.Actual.RegularHolidayRestDayNightDiffPay, paystub.RegularHolidayRestDayNightDiffPay)
                paystubPayslipModel.RegularHolidayRestDayNightDiffOTHours = paystub.RegularHolidayRestDayNightDiffOTHours
                paystubPayslipModel.RegularHolidayRestDayNightDiffOTPay = If(isActual, paystub.Actual.RegularHolidayRestDayNightDiffOTPay, paystub.RegularHolidayRestDayNightDiffOTPay)

                paystubPayslipModels.Add(paystubPayslipModel.CreateSummaries(salary, paystub.BasicHours))

            Next

        End Using

        Return paystubPayslipModels

        'CreateSampleModels(paystubPayslipModels)

        'Return Await Task.Run(Function()
        '                          Return paystubPayslipModels
        '                      End Function)
    End Function

    Private Function GetEmployeeLoans(loans As List(Of LoanTransaction), employeeId As Integer) As List(Of PaystubPayslipModel.Loan)

        Dim employeeLoans = loans.
                                Where(Function(l) l.EmployeeID.Value = employeeId).
                                ToList

        Dim loanModels As New List(Of PaystubPayslipModel.Loan)

        For Each loan In employeeLoans

            loanModels.Add(New PaystubPayslipModel.Loan(loan))

        Next

        Return loanModels

    End Function

    Private Function GetEmployeeAdjustments(adjustments As IEnumerable(Of IAdjustment), employeeId As Integer) As List(Of PaystubPayslipModel.Adjustment)

        Dim employeeAdjustments = adjustments.
                                    Where(Function(l) l.Paystub?.EmployeeID.Value = employeeId).
                                    ToList

        Dim adjustmentModels As New List(Of PaystubPayslipModel.Adjustment)

        For Each adjustment In employeeAdjustments

            adjustmentModels.Add(New PaystubPayslipModel.Adjustment(adjustment))

        Next

        Return adjustmentModels

    End Function

    'Private Shared Sub CreateSampleModels(paystubPayslipModels As List(Of PaystubPayslipModel))
    '    paystubPayslipModels.Add(
    '                New PaystubPayslipModel() With {
    '                    .EmployeeNumber = "169899",
    '                    .EmployeeName = "Josh Santos",
    '                    .RegularPay = 20000,
    '                    .BasicHours = 96,
    '                    .BasicPay = 18200,
    '                    .Allowance = 2000,
    '                    .Ecola = 0.6,
    '                    .AbsentHours = 16,
    '                    .AbsentAmount = 1400,
    '                    .LateAndUndertimeHours = 4,
    '                    .LateAndUndertimeAmount = 350,
    '                    .GrossPay = 18000,
    '                    .SSSAmount = 480,
    '                    .PhilHealthAmount = 187.5,
    '                    .PagibigAmount = 100,
    '                    .TaxWithheldAmount = 0.5,
    '                    .NetPay = 17500,
    '                    .OvertimeHours = 2,
    '                    .OvertimePay = 671,
    '                    .RegularHolidayHours = 8,
    '                    .RegularHolidayPay = 1074,
    '                    .Loans = New List(Of PaystubPayslipModel.Loan) From {
    '                        New PaystubPayslipModel.Loan("SSS Loan", 888888, 88888888),
    '                        New PaystubPayslipModel.Loan("123456789012345678901234567890", 888881, 88888881),
    '                        New PaystubPayslipModel.Loan("Car Loan", 888883, 88888882)
    '                    },
    '                    .Adjustments = New List(Of PaystubPayslipModel.Adjustment) From {
    '                        New PaystubPayslipModel.Adjustment("sOT Adjustment", 50),
    '                        New PaystubPayslipModel.Adjustment("sUniform", -10000),
    '                        New PaystubPayslipModel.Adjustment("sSalary Adjustment", 88888),
    '                        New PaystubPayslipModel.Adjustment("sCellphone", -8888888)
    '                    }
    '                }.CreateSummaries())

    '    paystubPayslipModels.Add(
    '        New PaystubPayslipModel() With {
    '            .EmployeeNumber = "167724",
    '            .EmployeeName = "Vincent Dayagbil",
    '            .RegularPay = 30000,
    '            .BasicHours = 103,
    '            .BasicPay = 29900,
    '            .Allowance = 5000,
    '            .Ecola = 1000,
    '            .AbsentHours = 8,
    '            .AbsentAmount = 800,
    '            .LateAndUndertimeHours = 2,
    '            .LateAndUndertimeAmount = 200,
    '            .GrossPay = 29000,
    '            .SSSAmount = 560,
    '            .PhilHealthAmount = 216.75,
    '            .PagibigAmount = 200,
    '            .TaxWithheldAmount = 1200,
    '            .NetPay = 17500,
    '            .SpecialHolidayRestDayNightDiffOTHours = 3,
    '            .SpecialHolidayRestDayNightDiffOTPay = 210,
    '            .RegularHolidayRestDayNightDiffOTHours = 7,
    '            .RegularHolidayRestDayNightDiffOTPay = 480,
    '            .Loans = New List(Of PaystubPayslipModel.Loan) From {
    '                New PaystubPayslipModel.Loan("SSS Loan", 888888, 88888888),
    '                New PaystubPayslipModel.Loan("Pagibig Loan", 888888, 88888888),
    '                New PaystubPayslipModel.Loan("123456789012345678901234567890", 888888, 88888888)
    '            },
    '            .Adjustments = New List(Of PaystubPayslipModel.Adjustment) From {
    '                New PaystubPayslipModel.Adjustment("OT Adjustments", 500),
    '                New PaystubPayslipModel.Adjustment("Uniforms", -100500),
    '                New PaystubPayslipModel.Adjustment("Salary Adjustments", 888883),
    '                New PaystubPayslipModel.Adjustment("123456789012345678901234567890", -88888885)
    '            }
    '    }.CreateSummaries())

    '    paystubPayslipModels.Add(
    '        New PaystubPayslipModel() With {
    '            .EmployeeNumber = "169908",
    '            .EmployeeName = "Mark Galolo",
    '            .RegularPay = 30000,
    '            .BasicHours = 103,
    '            .BasicPay = 29900,
    '            .Allowance = 5000,
    '            .Ecola = 1000,
    '            .AbsentHours = 8,
    '            .AbsentAmount = 800,
    '            .LateAndUndertimeHours = 2,
    '            .LateAndUndertimeAmount = 200,
    '            .GrossPay = 29000,
    '            .SSSAmount = 560,
    '            .PhilHealthAmount = 216.75,
    '            .PagibigAmount = 200,
    '            .TaxWithheldAmount = 1200,
    '            .NetPay = 17500,
    '            .OvertimeHours = 10,
    '            .OvertimePay = 5000.789,
    '            .NightDiffHours = 10,
    '            .NightDiffPay = 5000.789,
    '            .NightDiffOvertimeHours = 10,
    '            .NightDiffOvertimePay = 5000.789,
    '            .RestDayHours = 10,
    '            .RestDayPay = 5000.789,
    '            .RestDayOTHours = 10,
    '            .RestDayOTPay = 5000.789,
    '            .SpecialHolidayHours = 10,
    '            .SpecialHolidayPay = 5000.789,
    '            .SpecialHolidayOTHours = 10,
    '            .SpecialHolidayOTPay = 5000.789,
    '            .RegularHolidayHours = 10,
    '            .RegularHolidayPay = 5000.789,
    '            .RegularHolidayOTHours = 10,
    '            .RegularHolidayOTPay = 5000.789,
    '            .RestDayNightDiffHours = 10,
    '            .RestDayNightDiffPay = 5000.789,
    '            .RestDayNightDiffOTHours = 10,
    '            .RestDayNightDiffOTPay = 5000.789,
    '            .SpecialHolidayNightDiffHours = 10,
    '            .SpecialHolidayNightDiffPay = 5000.789,
    '            .SpecialHolidayNightDiffOTHours = 10,
    '            .SpecialHolidayNightDiffOTPay = 5000.789,
    '            .SpecialHolidayRestDayHours = 10,
    '            .SpecialHolidayRestDayPay = 5000.789,
    '            .SpecialHolidayRestDayOTHours = 10,
    '            .SpecialHolidayRestDayOTPay = 5000.789,
    '            .SpecialHolidayRestDayNightDiffHours = 10,
    '            .SpecialHolidayRestDayNightDiffPay = 5000.789,
    '            .SpecialHolidayRestDayNightDiffOTHours = 10,
    '            .SpecialHolidayRestDayNightDiffOTPay = 5000.789,
    '            .RegularHolidayNightDiffHours = 10,
    '            .RegularHolidayNightDiffPay = 5000.789,
    '            .RegularHolidayNightDiffOTHours = 10,
    '            .RegularHolidayNightDiffOTPay = 5000.789,
    '            .RegularHolidayRestDayHours = 10,
    '            .RegularHolidayRestDayPay = 5000.789,
    '            .RegularHolidayRestDayOTHours = 10,
    '            .RegularHolidayRestDayOTPay = 5000.789,
    '            .RegularHolidayRestDayNightDiffHours = 10,
    '            .RegularHolidayRestDayNightDiffPay = 5000.789,
    '            .RegularHolidayRestDayNightDiffOTHours = 10,
    '            .RegularHolidayRestDayNightDiffOTPay = 5000.789,
    '            .Loans = New List(Of PaystubPayslipModel.Loan) From {
    '                New PaystubPayslipModel.Loan("SSS Loan", 888888, 88888888),
    '                New PaystubPayslipModel.Loan("Pagibig Loan", 888881, 88888881),
    '                New PaystubPayslipModel.Loan("Car Loan", 888883, 88888882),
    '                New PaystubPayslipModel.Loan("Car Loan", 888883, 88888882)
    '            },
    '            .Adjustments = New List(Of PaystubPayslipModel.Adjustment) From {
    '                New PaystubPayslipModel.Adjustment("OT Adjustment", 50),
    '                New PaystubPayslipModel.Adjustment("Uniform", -10050),
    '                New PaystubPayslipModel.Adjustment("Salary Adjustment", 88883),
    '                New PaystubPayslipModel.Adjustment("Cellphone", -8888885)
    '            }
    '    }.CreateSummaries())

    '    paystubPayslipModels.Add(
    '        New PaystubPayslipModel() With {
    '            .EmployeeNumber = "168360",
    '            .EmployeeName = "Noel Santos",
    '            .RegularPay = 30000,
    '            .BasicHours = 103,
    '            .BasicPay = 29900,
    '            .Allowance = 5000,
    '            .Ecola = 1000,
    '            .AbsentHours = 8,
    '            .AbsentAmount = 800,
    '            .LateAndUndertimeHours = 2,
    '            .LateAndUndertimeAmount = 200,
    '            .GrossPay = 29000,
    '            .SSSAmount = 560,
    '            .PhilHealthAmount = 216.75,
    '            .PagibigAmount = 200,
    '            .TaxWithheldAmount = 1200,
    '            .NetPay = 17500,
    '            .OvertimeHours = 10,
    '            .OvertimePay = 5000.789,
    '            .NightDiffHours = 10,
    '            .NightDiffPay = 5000.789,
    '            .NightDiffOvertimeHours = 10,
    '            .NightDiffOvertimePay = 5000.789,
    '            .RestDayHours = 10,
    '            .RestDayPay = 5000.789,
    '            .RestDayOTHours = 10,
    '            .RestDayOTPay = 5000.789,
    '            .SpecialHolidayHours = 10,
    '            .SpecialHolidayPay = 5000.789,
    '            .SpecialHolidayOTHours = 10,
    '            .SpecialHolidayOTPay = 5000.789,
    '            .RegularHolidayHours = 10,
    '            .RegularHolidayPay = 5000.789,
    '            .RegularHolidayOTHours = 10,
    '            .RegularHolidayOTPay = 5000.789,
    '            .RestDayNightDiffHours = 10,
    '            .RestDayNightDiffPay = 5000.789,
    '            .RestDayNightDiffOTHours = 10,
    '            .RestDayNightDiffOTPay = 5000.789,
    '            .SpecialHolidayNightDiffHours = 10,
    '            .SpecialHolidayNightDiffPay = 5000.789,
    '            .SpecialHolidayNightDiffOTHours = 10,
    '            .SpecialHolidayNightDiffOTPay = 5000.789,
    '            .SpecialHolidayRestDayHours = 10,
    '            .SpecialHolidayRestDayPay = 5000.789,
    '            .SpecialHolidayRestDayOTHours = 10,
    '            .SpecialHolidayRestDayOTPay = 5000.789,
    '            .SpecialHolidayRestDayNightDiffHours = 10,
    '            .SpecialHolidayRestDayNightDiffPay = 5000.789,
    '            .SpecialHolidayRestDayNightDiffOTHours = 10,
    '            .SpecialHolidayRestDayNightDiffOTPay = 5000.789,
    '            .RegularHolidayNightDiffHours = 10,
    '            .RegularHolidayNightDiffPay = 5000.789,
    '            .RegularHolidayNightDiffOTHours = 10,
    '            .RegularHolidayNightDiffOTPay = 5000.789,
    '            .RegularHolidayRestDayHours = 10,
    '            .RegularHolidayRestDayPay = 5000.789,
    '            .RegularHolidayRestDayOTHours = 10,
    '            .RegularHolidayRestDayOTPay = 5000.789,
    '            .RegularHolidayRestDayNightDiffHours = 10,
    '            .RegularHolidayRestDayNightDiffPay = 5000.789,
    '            .RegularHolidayRestDayNightDiffOTHours = 10,
    '            .RegularHolidayRestDayNightDiffOTPay = 5000.789,
    '            .Loans = New List(Of PaystubPayslipModel.Loan) From {
    '                New PaystubPayslipModel.Loan("SSS Loan", 500, 10000),
    '                New PaystubPayslipModel.Loan("Pagibig Loan", 3000, 100500),
    '                New PaystubPayslipModel.Loan("Car Loan", 888883, 88888882),
    '                New PaystubPayslipModel.Loan("Battery Loan", 888884, 88888885)
    '            },
    '            .Adjustments = New List(Of PaystubPayslipModel.Adjustment) From {
    '                New PaystubPayslipModel.Adjustment("OT Adjustment", 500),
    '                New PaystubPayslipModel.Adjustment("Uniform", -100500),
    '                New PaystubPayslipModel.Adjustment("Salary Adjustment", 888883),
    '                New PaystubPayslipModel.Adjustment("Cellphone", -88888885)
    '            }
    '    }.CreateSummaries())
    'End Sub

End Class