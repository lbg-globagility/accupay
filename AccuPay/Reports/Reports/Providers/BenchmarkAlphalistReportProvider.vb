Option Strict On
Option Explicit On

Imports System.Threading.Tasks
Imports AccuPay.Entity
Imports AccuPay.Enums
Imports AccuPay.Utilities.Extensions
Imports CrystalDecisions.CrystalReports.Engine
Imports Microsoft.EntityFrameworkCore
Imports PayrollSys

Public Class BenchmarkAlphalistReportProvider
    Implements IReportProvider

    Public Property Name As String = "Alphalist" Implements IReportProvider.Name
    Public Property IsHidden As Boolean = False Implements IReportProvider.IsHidden

    Public Async Sub Run() Implements IReportProvider.Run

        Dim report = New BenchmarkAlphalist

        Dim year = 2020

        Dim data = Await GetData(year)

        Dim objText As TextObject = DirectCast(report.ReportDefinition.Sections(2).ReportObjects("YearHeader"), TextObject)
        objText.Text = year.ToString()

        report.SetDataSource(data)

        Dim crvwr As New CrysRepForm
        crvwr.crysrepvwr.ReportSource = report
        crvwr.Show()

    End Sub

    Private Async Function GetData(year As Integer) As Task(Of DataTable)

        Dim data As New DataTable
        data.Columns.Add("DatCol1") ' TIN
        data.Columns.Add("DatCol2") ' Employee Name
        data.Columns.Add("DatCol3") ' Basic
        data.Columns.Add("DatCol4") ' 13th month
        data.Columns.Add("DatCol5") ' Overtime
        data.Columns.Add("DatCol6") ' Allowance
        data.Columns.Add("DatCol7") ' Bonus
        data.Columns.Add("DatCol8") ' Gross Pay
        data.Columns.Add("DatCol9") ' SSS
        data.Columns.Add("DatCol10") ' PhilHealth
        data.Columns.Add("DatCol11") ' HDMF
        data.Columns.Add("DatCol12") ' Total Deduction
        data.Columns.Add("DatCol13") ' Net Pay

        Dim allPaystubs = Await GetPaystubs(year)
        Dim allPayPeriodSalaries = Await PayPeriodSalary.Fetch(year)

        Dim employeePaystubs = allPaystubs.
            GroupBy(Function(p) p.Employee).
            OrderBy(Function(e) e.Key.LastName).
            ThenBy(Function(e) e.Key.FirstName)

        For Each item In employeePaystubs

            Dim employee = item.Key
            Dim paystubs = item.ToList()

            Dim paystubPayPeriods = paystubs.GroupBy(Function(p) p.PayPeriod)

            Dim basicPay As Decimal = 0

            For Each paystub In paystubs

                Dim payPeriodSalaries = allPayPeriodSalaries.
                    Where(Function(p) p.PayPeriod.RowID.Value = paystub.PayPeriod.RowID.Value).
                    FirstOrDefault()

                Dim salary = payPeriodSalaries.Salaries.
                    FirstOrDefault(Function(s) s.EmployeeID.Value = employee.RowID.Value)

                basicPay += ComputeBasicPay(employee, salary.BasicSalary, paystub.BasicPay)
            Next

            Dim alphalistData = New BenchmarkAlphalistData(
                tinNumber:=employee.TinNo,
                employeeName:=$"{employee.LastName}, {employee.FirstName}",
                basicPay:=basicPay,
                thirteenthMonthAmount:=paystubs.Sum(Function(p) p.ThirteenthMonthPay.Amount),
                overtime:=paystubs.Sum(Function(p) p.AdditionalPay),
                grossPay:=paystubs.Sum(Function(p) p.GrossPay),
                sSSAmount:=paystubs.Sum(Function(p) p.SssEmployeeShare),
                philhealthAmount:=paystubs.Sum(Function(p) p.PhilHealthEmployeeShare),
                hDMFAmount:=paystubs.Sum(Function(p) p.HdmfEmployeeShare),
                netpay:=paystubs.Sum(Function(p) p.NetPay))

            data.Rows.Add(alphalistData.ToDataRow(data))
        Next

        Return data
    End Function

    Private Shared Async Function GetPaystubs(year As Integer) As Task(Of List(Of Paystub))
        Using context As New PayrollContext

            Return Await context.Paystubs.
                Include(Function(p) p.PayPeriod).
                Include(Function(p) p.Employee).
                Include(Function(p) p.ThirteenthMonthPay).
                Where(Function(p) p.PayPeriod.Year = year).
                Where(Function(p) p.OrganizationID.Value = z_OrganizationID).
                ToListAsync()

        End Using
    End Function

    ''' <summary>
    ''' This is same as PaystubPayslipModel's ComputeBasicPay. They should be merged.
    ''' </summary>
    ''' <param name="salary"></param>
    ''' <param name="workHours"></param>
    ''' <returns></returns>

    Public Function ComputeBasicPay(employee As Employee, salary As Decimal, workHours As Decimal) As Decimal

        If employee.IsMonthly OrElse employee.IsFixed Then

            If employee.PayFrequencyID.Value = PayFrequencyType.Monthly Then

                Return salary

            ElseIf employee.PayFrequencyID.Value = PayFrequencyType.SemiMonthly Then

                Return salary / PayrollTools.SemiMonthlyPayPeriodsPerMonth
            Else

                Throw New Exception("GetBasicPay is implemented on monthly and semimonthly only")

            End If

        ElseIf employee.IsDaily Then

            Return workHours * (salary / PayrollTools.WorkHoursPerDay)

        End If

        Return 0

    End Function

    Public Class BenchmarkAlphalistData

        Public Sub New(tinNumber As String, employeeName As String, basicPay As Decimal, thirteenthMonthAmount As Decimal, overtime As Decimal, grossPay As Decimal, sSSAmount As Decimal, philhealthAmount As Decimal, hDMFAmount As Decimal, netpay As Decimal)

            Me.TinNumber = tinNumber
            Me.EmployeeName = employeeName
            Me.BasicPay = basicPay
            Me.ThirteenthMonthAmount = thirteenthMonthAmount
            Me.Overtime = overtime
            Me.GrossPay = grossPay
            Me.SSSAmount = sSSAmount
            Me.PhilhealthAmount = philhealthAmount
            Me.HDMFAmount = hDMFAmount
            Me.Netpay = netpay
        End Sub

        Public Property TinNumber As String
        Public Property EmployeeName As String
        Public Property BasicPay As Decimal
        Public Property ThirteenthMonthAmount As Decimal

        Public Property Overtime As Decimal

        Public ReadOnly Property Allowance As Decimal
            Get
                Return 0 'Will be added manually by maam Mely
            End Get
        End Property

        Public ReadOnly Property Bonus As Decimal
            Get
                Return 0 'Will be added manually by maam Mely
            End Get
        End Property

        Public Property GrossPay As Decimal
        Public Property SSSAmount As Decimal
        Public Property PhilhealthAmount As Decimal
        Public Property HDMFAmount As Decimal

        Public ReadOnly Property TotalDeduction As Decimal
            Get
                Return SSSAmount + PhilhealthAmount + HDMFAmount
            End Get
        End Property

        Public Property Netpay As Decimal

        Public Function ToDataRow(data As DataTable) As DataRow

            Dim newRow = data.NewRow()

            newRow("DatCol1") = Me.TinNumber ' TIN
            newRow("DatCol2") = Me.EmployeeName ' Employee Name
            newRow("DatCol3") = FormatNumber(Me.BasicPay) ' Basic
            newRow("DatCol4") = FormatNumber(Me.ThirteenthMonthAmount) ' 13th month
            newRow("DatCol5") = FormatNumber(Me.Overtime) ' Overtime
            newRow("DatCol6") = FormatNumber(Me.Allowance) ' Allowance
            newRow("DatCol7") = FormatNumber(Me.Bonus) ' Bonus
            newRow("DatCol8") = FormatNumber(Me.GrossPay) ' Gross Pay
            newRow("DatCol9") = FormatNumber(Me.SSSAmount) ' SSS
            newRow("DatCol10") = FormatNumber(Me.PhilhealthAmount) ' PhilHealth
            newRow("DatCol11") = FormatNumber(Me.HDMFAmount) ' HDMF
            newRow("DatCol12") = FormatNumber(Me.TotalDeduction) ' Total Deduction
            newRow("DatCol13") = FormatNumber(Me.Netpay) ' Net Pay

            Return newRow

        End Function

        Public Function FormatNumber(number As Decimal) As String

            If number = 0 Then
                Return String.Empty
            End If

            Return number.RoundToString(3)

        End Function

    End Class

    Public Class PayPeriodSalary

        Public Sub New(payPeriod As PayPeriod, salaries As List(Of Salary))
            Me.PayPeriod = payPeriod
            Me.Salaries = salaries
        End Sub

        Public Property PayPeriod As PayPeriod

        Public Property Salaries As List(Of Salary)

        Public Shared Async Function Fetch(year As Integer) As Task(Of List(Of PayPeriodSalary))

            Dim payPeriods As List(Of PayPeriod)

            Using context As New PayrollContext

                payPeriods = Await context.PayPeriods.
                    Where(Function(p) p.Year = year).
                    Where(Function(p) p.OrganizationID.Value = z_OrganizationID).
                    Where(Function(p) p.IsSemiMonthly).
                    ToListAsync()

            End Using

            Dim payPeriodSalaries = New List(Of PayPeriodSalary)

            For Each period In payPeriods

                Dim salaries = Await New SalaryRepository().GetAllByCutOff(period.PayFromDate)

                payPeriodSalaries.Add(New PayPeriodSalary(period, salaries))

            Next

            Return payPeriodSalaries

        End Function

    End Class

End Class
