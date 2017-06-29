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

    Public Async Function LoadPayPeriods() As Task(Of Collection(Of PayPeriod))
        Dim sql = <![CDATA[
            SELECT PayFromDate, PayToDate, Year, Month
            FROM payperiod
            WHERE payperiod.OrganizationID = @OrganizationID
                AND payperiod.Year = @Year
                AND payperiod.TotalGrossSalary = @SalaryType;
        ]]>.Value

        sql = sql.Replace("@OrganizationID", z_OrganizationID) _
            .Replace("@Year", 2016) _
            .Replace("@SalaryType", 1)

        Dim payPeriods = New Collection(Of PayPeriod)

        Using connection As New MySqlConnection(connectionString),
            command As New MySqlCommand(sql, connection)

            Await connection.OpenAsync()
            Dim reader = Await command.ExecuteReaderAsync()
            While Await reader.ReadAsync()
            End While
        End Using

        Return payPeriods
    End Function

    Private Sub TimeEntrySummary_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class