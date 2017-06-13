Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared
Imports MySql.Data.MySqlClient
Imports MySql.Data

Public Class Josh_CrysRepForm

    Public employeeName As String
    Public disciplinaryAction As String
    Public infraction As String
    Public comments As String

    Private Sub Josh_CrysRepForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim rpt As New ReportDocument

        Dim paramFields As New ParameterFields

        Dim paramField As New ParameterField
        Dim paramField2 As New ParameterField
        Dim paramField3 As New ParameterField
        Dim paramField4 As New ParameterField
        Dim paramDiscreteValue As New ParameterDiscreteValue
        Dim paramDiscreteValue2 As New ParameterDiscreteValue
        Dim paramDiscreteValue3 As New ParameterDiscreteValue
        Dim paramDiscreteValue4 As New ParameterDiscreteValue


        paramField.Name = "EmployeeName"
        paramDiscreteValue.Value = Me.employeeName
        paramField.CurrentValues.Add(paramDiscreteValue)
        paramFields.Add(paramField)

        paramField2.Name = "DisciplinaryAction"
        paramDiscreteValue2.Value = Me.disciplinaryAction.ToLower
        paramField2.CurrentValues.Add(paramDiscreteValue2)
        paramFields.Add(paramField2)

        paramField3.Name = "Infraction"
        paramDiscreteValue3.Value = Me.infraction.ToLower
        paramField3.CurrentValues.Add(paramDiscreteValue3)
        paramFields.Add(paramField3)

        If Me.comments.Trim.Length > 0 Then Me.comments &= "."

        paramField4.Name = "Comments"
        paramDiscreteValue4.Value = Me.comments
        paramField4.CurrentValues.Add(paramDiscreteValue4)
        paramFields.Add(paramField4)

        Dim adapter As New MySqlDataAdapter()
        Dim dt As New DataTable

        CrystalReportViewer1.ParameterFieldInfo = paramFields
        '\Lambert Form
        rpt.Load(Application.StartupPath + "\rpt\discplinaryActions.rpt")

        adapter.SelectCommand = New MySqlCommand("SELECT * FROM VW_DisciplinaryMemoUserDetails WHERE UserID = " & z_User)

        adapter.SelectCommand.Connection = getConn()
        adapter.Fill(dt)

        rpt.SetDataSource(dt)

        CrystalReportViewer1.ReportSource = rpt
        CrystalReportViewer1.Refresh()

    End Sub

End Class