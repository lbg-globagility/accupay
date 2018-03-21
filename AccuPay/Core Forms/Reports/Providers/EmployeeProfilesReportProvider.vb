Option Strict On
Imports System.IO
Imports CrystalDecisions.CrystalReports.Engine
Imports OfficeOpenXml

Public Class EmployeeProfilesReportProvider
    Implements IReportProvider

    Public Property Name As String = "Employee Personal Information" Implements IReportProvider.Name

    Private basic_alphabet() As String =
        New String() {"A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z"}

    Public Sub Run() Implements IReportProvider.Run

        Dim sql_print_employee_profiles As New SQL("CALL PRINT_employee_profiles(?og_rowid);",
                                                   New Object() {orgztnID})

        Static one_value As Integer = 1

        Try

            Dim dt As New DataTable

            dt = sql_print_employee_profiles.GetFoundRows.Tables(0)

            If sql_print_employee_profiles.HasError Then

                Throw sql_print_employee_profiles.ErrorException
            Else

                Static report_name As String = "EmployeeProfiles"

                Static temp_path As String = Path.GetTempPath()

                Static temp_file As String = String.Concat(temp_path, report_name, "Report.xlsx")

                Dim newFile = New FileInfo(temp_file)

                If newFile.Exists Then
                    newFile.Delete()
                    newFile = New FileInfo(temp_file)
                End If

                Using excl_pkg = New ExcelPackage(newFile)

                    Dim worksheet As ExcelWorksheet =
                                excl_pkg.Workbook.Worksheets.Add(report_name)

                    Dim row_indx As Integer = one_value

                    Dim col_index As Integer = one_value

                    For Each dtcol As DataColumn In dt.Columns
                        worksheet.Cells(row_indx, col_index).Value = dtcol.ColumnName
                        col_index += one_value
                    Next

                    row_indx += one_value

                    For Each dtrow As DataRow In dt.Rows

                        Dim row_array = dtrow.ItemArray

                        Dim i = 0

                        For Each rowval In row_array

                            Dim excl_colrow As String =
                                        String.Concat(basic_alphabet(i),
                                                      row_indx)

                            worksheet.Cells(excl_colrow).Value = rowval

                            i += one_value

                        Next

                        row_indx += one_value

                    Next

                    worksheet.Cells.AutoFitColumns(0)

                    excl_pkg.Save()

                End Using

                Process.Start(temp_file)

            End If
        Catch ex As Exception
            MsgBox(getErrExcptn(ex, Me.Name))
        End Try
    End Sub

End Class