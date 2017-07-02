Option Strict On

Imports MySql.Data.MySqlClient
Imports System.Collections.ObjectModel
Imports System.Threading.Tasks

Public Class TimeEntrySummary

    Class PayPeriod
        Public Property PayFromDate As Date
        Public Property PayToDate As Date
        Public Property Year As Integer
        Public Property Month As Integer
    End Class

    Private payPeriods As Collection(Of PayPeriod)

    Public Async Sub LoadPayPeriods()

        Me.payPeriods = Await GetPayPeriods()
        payPeriodDataGridView.Rows.Add(2)
        'payPeriodDataGridView.DefaultCellStyle.WrapMode = DataGridViewTriState.True

        Dim monthCounters(11) As Integer

        'For Each payPeriod In Me.payPeriods
        '    Dim button = New Button()
        '    button.Height = CInt(button.Height * 1.3)
        '    button.Width = CInt(button.Width * 1.5)
        '    Dim payFromDate = payPeriod.PayFromDate.ToString("dd MMM yyyy")
        '    Dim payToDate = payPeriod.PayToDate.ToString("dd MMM yyyy")

        '    button.Text = payFromDate + vbNewLine + payToDate

        '    PayPeriodsPanel.Controls.Add(button)
        'Next

        For Each payperiod In Me.payPeriods
            Dim monthNo = payperiod.Month
            Dim counter = monthCounters(monthNo - 1)

            Dim payFromDate = payperiod.PayFromDate.ToString("dd MMM")
            Dim payToDate = payperiod.PayToDate.ToString("dd MMM")
            Dim label = payFromDate + " - " + payToDate

            payPeriodDataGridView.Rows(counter).Cells(monthNo - 1).Value = label

            counter += 1
            monthCounters(monthNo - 1) = counter
        Next
    End Sub

    Public Async Function GetPayPeriods() As Task(Of Collection(Of PayPeriod))
        Dim sql = <![CDATA[
            SELECT PayFromDate, PayToDate, Year, Month
            FROM payperiod
            WHERE payperiod.OrganizationID = @OrganizationID
                AND payperiod.Year = @Year
                AND payperiod.TotalGrossSalary = @SalaryType;
        ]]>.Value

        sql = sql.Replace("@OrganizationID", CStr(z_OrganizationID)) _
            .Replace("@Year", CStr(2016)) _
            .Replace("@SalaryType", CStr(1))

        Dim payPeriods = New Collection(Of PayPeriod)

        Using connection As New MySqlConnection(connectionString),
            command As New MySqlCommand(sql, connection)

            Await connection.OpenAsync()
            Dim reader = Await command.ExecuteReaderAsync()
            While Await reader.ReadAsync()
                Dim payFromDate = reader.GetValue(Of Date)("PayFromDate")
                Dim payToDate = reader.GetValue(Of Date)("PayToDate")
                Dim year = reader.GetValue(Of Integer)("Year")
                Dim month = reader.GetValue(Of Integer)("Month")

                Dim payPeriod = New PayPeriod() With {
                    .PayFromDate = payFromDate,
                    .PayToDate = payToDate,
                    .Year = year,
                    .Month = month
                }

                payPeriods.Add(payPeriod)
            End While
        End Using

        Return payPeriods
    End Function

    Private Sub TimeEntrySummary_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadPayPeriods()
    End Sub

End Class