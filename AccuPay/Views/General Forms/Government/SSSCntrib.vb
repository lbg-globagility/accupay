Imports AccuPay.Core.Repositories
Imports Microsoft.Extensions.DependencyInjection

Public Class SSSCntrib

    Public q_paysocialsecurity As String = "SELECT sss.RowID," &
    "COALESCE(sss.RangeFromAmount,0) 'Range of Compensation'," &
    "COALESCE(sss.RangeToAmount,0)," &
    "COALESCE(sss.MonthlySalaryCredit,0) 'Monthly Salary Credit'," &
    "COALESCE(sss.EmployerContributionAmount,0) 'Employer Contribution Amount'," &
    "COALESCE(sss.EmployeeContributionAmount,0) 'Employee Contribution Amount'," &
    "COALESCE(sss.EmployeeECAmount,0) 'EC\/ER Amount'," &
    "COALESCE(sss.EmployerContributionAmount,0) + COALESCE(sss.EmployeeECAmount,0) 'Employer Total Contribution'," &
    "COALESCE(sss.EmployeeContributionAmount,0) 'Employee Total Contribution'," &
    "COALESCE(sss.EmployerContributionAmount,0) + COALESCE(sss.EmployeeContributionAmount,0) + COALESCE(sss.EmployeeECAmount,0) 'EC\/ER Total'," &
    "DATE_FORMAT(sss.Created,'%m-%d-%Y') 'Creation Date'," &
    "CONCAT(CONCAT(UCASE(LEFT(u.FirstName, 1)), SUBSTRING(u.FirstName, 2)),' ',CONCAT(UCASE(LEFT(u.LastName, 1)), SUBSTRING(u.LastName, 2))) 'Created by'," &
    "COALESCE(DATE_FORMAT(sss.LastUpd,'%m-%d-%Y'),'') 'Last Update'," &
    "(SELECT CONCAT(CONCAT(UCASE(LEFT(u.FirstName, 1)), SUBSTRING(u.FirstName, 2)),' ',CONCAT(UCASE(LEFT(u.LastName, 1)), SUBSTRING(u.LastName, 2)))  FROM aspnetusers WHERE Id=sss.LastUpdBy) 'LastUpdate by' " &
    "FROM paysocialsecurity sss " &
    "INNER JOIN aspnetusers u ON sss.CreatedBy=u.Id" &
    " WHERE sss.MonthlySalaryCredit!=0" &
    " AND sss.HiddenData='0'" &
    " AND EffectiveDateFrom='2019-04-01' AND EffectiveDateTo='2022-12-01'"

    Private Sub SSSCntrib_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        InfoBalloon(, , lblforballoon, , , 1)

        If previousForm IsNot Nothing Then
            If previousForm.Name = Me.Name Then
                previousForm = Nothing
            End If
        End If

        GeneralForm.listGeneralForm.Remove(Me.Name)
    End Sub

    Private Async Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        loadSSSCntrib()

        Dim userRepository = MainServiceProvider.GetRequiredService(Of AspNetUserRepository)
        Dim user = Await userRepository.GetByIdAsync(z_User)

    End Sub

    Sub loadSSSCntrib()
        dgvPaySSS.Rows.Clear()
        For Each r As DataRow In retAsDatTbl(q_paysocialsecurity & " ORDER BY sss.MonthlySalaryCredit").Rows
            dgvPaySSS.Rows.Add(r(0), r(1), r(2), r(3), r(4), r(5), r(6), r(7), r(8), r(9), r(10), r(11), r(12))
        Next
    End Sub

    Private Sub ToolStripButton4_Click(sender As Object, e As EventArgs) Handles ToolStripButton4.Click
        Me.Close()
    End Sub

End Class