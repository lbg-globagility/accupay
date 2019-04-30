Option Strict On

Namespace Global.AccuPay.Helpers

    Public Enum ExcelTemplates

        Allowance
        Employee
        OldShift
        Leave
        Loan
        OfficialBusiness
        Overtime
        Salary
        NewShift

    End Enum

    Public Class TemplatesHelper

        Public Shared ReadOnly ALLOWANCE As String = "accupay-allowance-template.xlsx"
        Public Shared ReadOnly EMPLOYEE As String = "accupay-employeelist-template.xlsx"
        Public Shared ReadOnly OLD_SHIFT As String = "accupay-employeeshift-template.xlsx"
        Public Shared ReadOnly LEAVE As String = "accupay-leave-template.xlsx"
        Public Shared ReadOnly LOAN As String = "accupay-loan-template.xlsx"
        Public Shared ReadOnly OFFICIAL_BUSINESS As String = "accupay-officialbus-template.xlsx"
        Public Shared ReadOnly OVERTIME As String = "accupay-overtime-template.xlsx"
        Public Shared ReadOnly SALARY As String = "accupay-salary-template.xlsx"
        Public Shared ReadOnly NEW_SHIFT As String = "accupay-shiftschedule-template.xlsx"

        Private Const FILE_PATH As String = "ImportTemplates/"

        Public Shared Function GetFileName(excelTemplate As ExcelTemplates) As String

            Select Case excelTemplate

                Case ExcelTemplates.Allowance
                    Return ALLOWANCE

                Case ExcelTemplates.Employee
                    Return EMPLOYEE

                Case ExcelTemplates.OldShift
                    Return OLD_SHIFT

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

                Case ExcelTemplates.NewShift
                    Return NEW_SHIFT

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

                Case ExcelTemplates.OldShift
                    fileName = OLD_SHIFT

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

                Case ExcelTemplates.NewShift
                    fileName = NEW_SHIFT

                Case Else

                    Return Nothing

            End Select

            Return FILE_PATH & fileName

        End Function

    End Class

End Namespace
