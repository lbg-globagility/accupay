Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Data.Entities
Imports AccuPay.Data.Repositories
Imports AccuPay.Data.Services
Imports AccuPay.Data.ValueObjects
Imports log4net
Imports Microsoft.Extensions.DependencyInjection

Namespace Benchmark

    Public Class BenchmarkPayrollHelper

        Private _loanService As LoanDataService
        Private _productRepository As ProductRepository

        Private _pagibigLoanId As Integer
        Private _sssLoanId As Integer

#Region "Read-only Properties"

        Private _deductionList As List(Of Product)

        Public ReadOnly Property DeductionList() As List(Of Product)
            Get
                Return _deductionList
            End Get
        End Property

        Private _incomeList As List(Of Product)

        Public ReadOnly Property IncomeList() As List(Of Product)
            Get
                Return _incomeList
            End Get
        End Property

#End Region

        Private Sub New()

            _loanService = MainServiceProvider.GetRequiredService(Of LoanDataService)

            _productRepository = MainServiceProvider.GetRequiredService(Of ProductRepository)

        End Sub

        Public Shared Async Function GetEcola(
            employeeId As Integer,
            payDateFrom As Date,
            payDateTo As Date) As Task(Of Allowance)

            'TODO: check if the effectivity date matters in benchmark payroll generation
            'better if there is a change in ECOLA value, a new allowance should be created.
            Dim timePeriod = New TimePeriod(payDateFrom, payDateTo)

            Dim productService = MainServiceProvider.GetRequiredService(Of ProductDataService)
            Return Await productService.GetOrCreateEmployeeEcola(
                employeeId:=employeeId,
                organizationId:=z_OrganizationID,
                userId:=z_User,
                timePeriod:=timePeriod,
                allowanceFrequency:=Allowance.FREQUENCY_DAILY,
                amount:=0)

        End Function

        Public Shared Async Function GetInstance(logger As ILog) As Task(Of BenchmarkPayrollHelper)

            Dim helper = New BenchmarkPayrollHelper()

            If Await helper.Initialize(logger) = False Then

                Return Nothing

            End If

            Return helper

        End Function

        Public Shared Function GetTotalOvertimePay(paystub As Paystub) As Decimal

            Return paystub.OvertimePay +
                paystub.NightDiffPay +
                paystub.NightDiffOvertimePay +
                paystub.RestDayPay +
                paystub.RestDayOTPay +
                paystub.RestDayNightDiffPay +
                paystub.RestDayNightDiffOTPay +
                paystub.SpecialHolidayPay +
                paystub.SpecialHolidayOTPay +
                paystub.SpecialHolidayNightDiffPay +
                paystub.SpecialHolidayNightDiffOTPay +
                paystub.SpecialHolidayRestDayPay +
                paystub.SpecialHolidayRestDayOTPay +
                paystub.SpecialHolidayRestDayNightDiffPay +
                paystub.SpecialHolidayRestDayNightDiffOTPay +
                paystub.RegularHolidayPay +
                paystub.RegularHolidayOTPay +
                paystub.RegularHolidayNightDiffPay +
                paystub.RegularHolidayNightDiffOTPay +
                paystub.RegularHolidayRestDayPay +
                paystub.RegularHolidayRestDayOTPay +
                paystub.RegularHolidayRestDayNightDiffPay +
                paystub.RegularHolidayRestDayNightDiffOTPay

        End Function

        Public Shared Function ConvertHoursToDays(hours As Decimal) As Decimal

            Return hours / BenchmarkPaystubRate.WorkHoursPerDay

        End Function

        ''' <summary>
        ''' Makes sure that Accupay does not have unneccesary data for benchmark.
        ''' </summary>
        Public Async Function CleanEmployee(employeeId As Integer) As Task

            Await _loanService.DeleteAllLoansExceptGovernmentLoansAsync(
                    employeeId:=employeeId,
                    pagibigLoanId:=_pagibigLoanId,
                    ssLoanId:=_sssLoanId)

        End Function

        Public Async Function Initialize(logger As ILog) As Task(Of Boolean)

            If Await InitializeLoanIds(logger) = False OrElse
                Await InitializeAdjustmentsLists() = False Then

                Return False

            End If

            Return True

        End Function

        Private Async Function InitializeLoanIds(logger As ILog) As Task(Of Boolean)

            Dim govermentLoans = Await _productRepository.GetGovernmentLoanTypesAsync(z_OrganizationID)

            Dim pagibigLoanId = govermentLoans.FirstOrDefault(Function(l) l.IsPagibigLoan)?.RowID
            Dim sssLoanId = govermentLoans.FirstOrDefault(Function(l) l.IsSssLoan)?.RowID

            If pagibigLoanId Is Nothing OrElse sssLoanId Is Nothing Then

                logger.Error("Pagibig or SSS loan Id were not found in the database.")

                Return False

            End If

            _pagibigLoanId = pagibigLoanId.Value
            _sssLoanId = sssLoanId.Value
            Return True

        End Function

        Private Async Function InitializeAdjustmentsLists() As Task(Of Boolean)

            _deductionList = New List(Of Product)(Await _productRepository.GetDeductionAdjustmentTypesAsync(z_OrganizationID))
            _incomeList = New List(Of Product)(Await _productRepository.GetAdditionAdjustmentTypesAsync(z_OrganizationID))

            If _deductionList Is Nothing OrElse _incomeList Is Nothing Then Return False

            Return True

        End Function

    End Class

End Namespace
