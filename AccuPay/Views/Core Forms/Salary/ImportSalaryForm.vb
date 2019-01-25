Option Strict On

Imports PayrollSys
Imports Globagility.AccuPay.Salaries
Imports AccuPay.Entity
Imports Globagility.AccuPay.Government
Imports System.Data.Entity
Imports Microsoft.EntityFrameworkCore

Public Class ImportSalaryForm

    Private _salaries As IList(Of Salary)

    Private Sub BrowseButton_Click(sender As Object, e As EventArgs) Handles BrowseButton.Click
        Dim browseFile = New OpenFileDialog With {
            .Filter = "Microsoft Excel Workbook Documents 2007-13 (*.xlsx)|*.xlsx|" &
                      "Microsoft Excel Documents 97-2003 (*.xls)|*.xls"
        }

        If browseFile.ShowDialog() <> DialogResult.OK Then
            Return
        End If

        Dim fileName = browseFile.FileName

        Dim parser = New ExcelParser(Of SalaryRowRecord)("Employee Salary")
        Dim records = parser.Read(fileName)

        Dim salaryViewModels = New List(Of SalaryViewModel)
        _salaries = New List(Of Salary)

        Using context = New PayrollContext()
            For Each record In records
                Dim employee = context.Employees.
                    FirstOrDefault(Function(t) CBool(t.EmployeeNo = record.EmployeeNo AndAlso t.OrganizationID = z_OrganizationID))

                If employee Is Nothing Then
                    Continue For
                End If

                Dim salary = New Salary With {
                    .OrganizationID = z_OrganizationID,
                    .CreatedBy = z_User,
                    .EmployeeID = employee.RowID,
                    .PositionID = employee.PositionID,
                    .EffectiveFrom = record.EffectiveFrom,
                    .EffectiveTo = record.EffectiveTo,
                    .BasicSalary = record.BasicSalary,
                    .AllowanceSalary = record.AllowanceSalary
                }

                _salaries.Add(salary)
                salaryViewModels.Add(New SalaryViewModel(salary, employee))
            Next
        End Using

        SalaryDataGrid.DataSource = salaryViewModels
    End Sub

    Private Async Sub SaveButton_Click(sender As Object, e As EventArgs) Handles SaveButton.Click
        If _salaries Is Nothing Then
            Return
        End If

        Using context = New PayrollContext()
            Dim philHealthBrackets = Await context.PhilHealthBrackets.ToListAsync()
            Dim philHealthConfig = Await context.ListOfValues.Where(Function(t) t.Type = "PhilHealth").ToListAsync()
            Dim philHealthCalculator = New PhilHealthCalculator(philHealthConfig, philHealthBrackets)

            Dim socialSecurityBrackets = Await context.SocialSecurityBrackets.ToListAsync()

            For Each salary In _salaries
                Dim employee = Await context.Employees.
                    FirstOrDefaultAsync(Function(emp) Nullable.Equals(emp.RowID, salary.EmployeeID))

                salary.TotalSalary = salary.BasicSalary + salary.AllowanceSalary

                Dim monthlyRate = 0D
                If employee.EmployeeType = "Monthly" Or employee.EmployeeType = "Fixed" Then
                    monthlyRate = salary.BasicSalary
                ElseIf employee.EmployeeType = "Daily" Then
                    Dim workDaysPerMonth = PayrollTools.GetWorkDaysPerMonth(employee.WorkDaysPerYear)

                    monthlyRate = salary.BasicSalary * workDaysPerMonth
                End If

                If employee.EmployeeType = "Monthly" Or employee.EmployeeType = "Fixed" Then
                    salary.BasicPay = monthlyRate / 2 ' Replace later
                ElseIf employee.EmployeeType = "Daily" Then
                    salary.BasicPay = salary.BasicSalary
                End If

                Dim socialSecurityBracket = socialSecurityBrackets.FirstOrDefault(
                    Function(s) s.RangeFromAmount <= monthlyRate And monthlyRate <= s.RangeToAmount)

                salary.SocialSecurityBracket = socialSecurityBracket

                salary.PhilHealthDeduction = philHealthCalculator.Calculate(monthlyRate)

                ' Set the pagibig amount to the default
                salary.HDMFAmount = 100D

                context.Salaries.Add(salary)
            Next

            Await context.SaveChangesAsync()
        End Using

        Close()
    End Sub

    Private Sub CancelButton_Click(sender As Object, e As EventArgs) Handles CancelButton.Click
        Close()
    End Sub

    ''' <summary>
    ''' ViewModel that bounds the salary entites to the data grid view
    ''' </summary>
    Private Class SalaryViewModel

        Private ReadOnly _salary As Salary

        Private ReadOnly _employee As Employee

        Public Sub New(salary As Salary, employee As Employee)
            _salary = salary
            _employee = employee
        End Sub

        Public ReadOnly Property FullName As String
            Get
                Return _employee.Fullname
            End Get
        End Property

        Public ReadOnly Property EffectiveFrom As Date
            Get
                Return _salary.EffectiveFrom
            End Get
        End Property

        Public ReadOnly Property EffectiveTo As Date?
            Get
                Return _salary.EffectiveTo
            End Get
        End Property

        Public ReadOnly Property BasicSalary As Decimal
            Get
                Return _salary.BasicSalary
            End Get
        End Property

        Public ReadOnly Property AllowanceSalary As Decimal
            Get
                Return _salary.AllowanceSalary
            End Get
        End Property

    End Class

End Class
