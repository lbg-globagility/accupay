Option Strict On

Namespace Global.AccuPay.Desktop.Helpers

    Public Enum ExcelTemplates

        Allowance
        Employee
        Employee2
        Leave
        Loan
        OfficialBusiness
        Overtime
        Salary
        Shift
        TripTicket
        GovernmentPremium
        TimeLogsOptimize
    End Enum

    Public Class TemplatesHelper

        Public Shared ReadOnly ALLOWANCE As String = "accupay-allowance-template.xlsx"
        Public Shared ReadOnly EMPLOYEE As String = "accupay-employeelist-template.xlsx"
        Public Shared ReadOnly EMPLOYEE2 As String = "accupay-employeelist-template(2).xlsx"
        Public Shared ReadOnly LEAVE As String = "accupay-leave-template.xlsx"
        Public Shared ReadOnly LOAN As String = "accupay-loan-template.xlsx"
        Public Shared ReadOnly OFFICIAL_BUSINESS As String = "accupay-officialbus-template.xlsx"
        Public Shared ReadOnly OVERTIME As String = "accupay-overtime-template.xlsx"
        Public Shared ReadOnly SALARY As String = "accupay-salary-template.xlsx"
        Public Shared ReadOnly SHIFT As String = "accupay-shiftschedule-template.xlsx"
        Public Shared ReadOnly TRIP_TICKET As String = "accupay-trip-ticket-template.xlsx"
        Public Shared ReadOnly GOVERNMENT_PREMIUM As String = "accupay-govt-premium-template.xlsx"
        Public Shared ReadOnly TIMELOGS_OPTIMIZE_FORMAT As String = "optimize time logs format.xlsm"

        Private Const FILE_PATH As String = "ImportTemplates/"

        Public Shared Function GetFileName(excelTemplate As ExcelTemplates) As String

            Select Case excelTemplate

                Case ExcelTemplates.Allowance
                    Return ALLOWANCE

                Case ExcelTemplates.Employee
                    Return EMPLOYEE

                Case ExcelTemplates.Employee2
                    Return EMPLOYEE2

                Case ExcelTemplates.Leave
                    Return LEAVE

                Case ExcelTemplates.Loan
                    Return LOAN

                Case ExcelTemplates.OfficialBusiness
                    Return OFFICIAL_BUSINESS

                Case ExcelTemplates.Overtime
                    Return OVERTIME

                Case ExcelTemplates.Salary
                    Return SALARY

                Case ExcelTemplates.Shift
                    Return SHIFT

                Case ExcelTemplates.TripTicket
                    Return TRIP_TICKET

                Case ExcelTemplates.GovernmentPremium
                    Return GOVERNMENT_PREMIUM

                Case ExcelTemplates.TimeLogsOptimize
                    Return TIMELOGS_OPTIMIZE_FORMAT

                Case Else

                    Return Nothing

            End Select

        End Function

        Public Shared Function GetFullPath(excelTemplate As ExcelTemplates) As String

            Dim fileName As String

            Select Case excelTemplate

                Case ExcelTemplates.Allowance
                    fileName = ALLOWANCE

                Case ExcelTemplates.Employee
                    fileName = EMPLOYEE

                Case ExcelTemplates.Employee2
                    fileName = EMPLOYEE2

                Case ExcelTemplates.Leave
                    fileName = LEAVE

                Case ExcelTemplates.Loan
                    fileName = LOAN

                Case ExcelTemplates.OfficialBusiness
                    fileName = OFFICIAL_BUSINESS

                Case ExcelTemplates.Overtime
                    fileName = OVERTIME

                Case ExcelTemplates.Salary
                    fileName = SALARY

                Case ExcelTemplates.Shift
                    fileName = SHIFT

                Case ExcelTemplates.TripTicket
                    fileName = TRIP_TICKET

                Case ExcelTemplates.GovernmentPremium
                    fileName = GOVERNMENT_PREMIUM

                Case ExcelTemplates.TimeLogsOptimize
                    fileName = TIMELOGS_OPTIMIZE_FORMAT

                Case Else

                    Return Nothing

            End Select

            Return FILE_PATH & fileName

        End Function

    End Class

End Namespace
