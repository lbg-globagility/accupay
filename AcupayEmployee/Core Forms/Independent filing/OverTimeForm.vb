Imports Femiani.Forms.UI.Input
Imports System.Threading

Public Class OverTimeForm

    Dim dbCurrentTimeStamp = EXECQUER("SELECT CAST(CURRENT_TIMESTAMP() AS CHAR) 'dbCurrentTimeStamp';")

    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        OverTimeForm()

    End Sub

    Sub OverTimeForm()

        dtpstarttime.CustomFormat = machineShortTimeFormat

        dtpendtime.CustomFormat = machineShortTimeFormat

    End Sub

    Protected Overrides Sub OnLoad(e As EventArgs)

        Dim item_count As Integer = cboOTType.Items.Count

        If item_count > 0 Then
            cboOTType.SelectedIndex = (item_count - 1)
        End If

        MyBase.OnLoad(e)

    End Sub

    Dim n_OverTimeRowID As String = String.Empty

    Public Property OverTimeRowID As String

        Get

            Return n_OverTimeRowID

        End Get

        Set(value As String)

            n_OverTimeRowID = value

        End Set

    End Property

    Private Sub OverTimeForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

        If Panel1.Enabled = True Then

            e.Cancel = False
        Else

            e.Cancel = True

        End If

    End Sub

    Private previous_orgID As String = String.Empty

    Private Sub OverTimeForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        enlistToCboBox("SELECT Name FROM organization WHERE NoPurpose='0' ORDER BY Name;",
                       cboOrganization)

        If n_OverTimeRowID <> Nothing Then

            Dim dtOverTimeRow As New DataTable

            dtOverTimeRow =
            retAsDatTbl("SELECT eo.*" &
                        ",e.EmployeeID AS EmpID" &
                        ",IFNULL(og.Name,'') AS OrgName" &
                        ",TIME_FORMAT(eo.OTStartTime,'%r') AS OTStartTime" &
                        ",TIME_FORMAT(eo.OTEndTime,'%r') AS OTEndTime" &
                        " FROM employeeovertime eo" &
                        " LEFT JOIN employee e ON e.RowID=eo.EmployeeID" &
                        " LEFT JOIN organization og ON og.RowID=eo.OrganizationID" &
                        " WHERE eo.RowID='" & n_OverTimeRowID & "';")

            If dtOverTimeRow IsNot Nothing Then

                For Each drow As DataRow In dtOverTimeRow.Rows

                    TxtEmployeeNumber1.Text = If(IsDBNull(drow("EmpID")), "", CStr(drow("EmpID")))

                    cboOTType.Text = CStr(drow("OTType"))

                    Dim ii = Format(CDate(drow("OTStartDate")), machineShortDateFormat)

                    dtpstartdateEmpOT.Text = ii

                    ii = Format(CDate(drow("OTEndDate")), machineShortDateFormat)

                    dtpendateEmpOT.Text = ii

                    ii = Format(CDate(drow("OTStartTime")), "hh:mm tt")

                    dtpstarttime.Text = dtpstarttime.MinDate & " " & ii

                    ii = Format(CDate(drow("OTEndTime")), "hh:mm tt")

                    dtpendtime.Text = dtpendtime.MinDate & " " & ii

                    cboOTStatus.Text = If(IsDBNull(drow("OTStatus")), "", CStr(drow("OTStatus")))

                    txtreasonEmpOT.Text = If(IsDBNull(drow("Reason")), "", CStr(drow("Reason")))

                    txtcommentsEmpOT.Text = If(IsDBNull(drow("Comments")), "", CStr(drow("Comments")))

                    cboOrganization.Text = CStr(drow("OrgName"))

                Next

            End If

        End If

        With TxtEmployeeFullName1

            .Text = String.Empty
            .Enabled = False
            .EmployeeTableColumnName = "CONCAT(LastName,', ',FirstName,IF(MiddleName = '', '', CONCAT(', ',MiddleName)))"
            '"CONCAT(LastName,', ',FirstName,', ',MiddleName)"

        End With

        Panel1.Focus()

        bgwEmpNames.RunWorkerAsync()

    End Sub

    Private Sub btnApply_Click(sender As Object, e As EventArgs) Handles btnApply.Click

        'TxtEmployeeNumber1.Focus()

        btnApply.Focus()

        'cboOrganization.Focus()

        'btnApply.Focus()

        If TxtEmployeeNumber1.Exists _
            And TxtEmployeeFullName1.Exists Then

            Panel1.Enabled = False
            bgwSaving.RunWorkerAsync()

        End If

    End Sub

    Private Function INSUPD_employeeOT() As Object

        Dim param(13, 2) As Object

        param(0, 0) = "eot_RowID"
        param(1, 0) = "eot_OrganizationID"
        param(2, 0) = "eot_CreatedBy"
        param(3, 0) = "eot_LastUpdBy"
        param(4, 0) = "eot_EmployeeID"
        param(5, 0) = "eot_OTType"
        param(6, 0) = "eot_OTStartTime"
        param(7, 0) = "eot_OTEndTime"
        param(8, 0) = "eot_OTStartDate"
        param(9, 0) = "eot_OTEndDate"
        param(10, 0) = "eot_Reason"
        param(11, 0) = "eot_Comments"
        param(12, 0) = "eot_Image"
        param(13, 0) = "eot_OTStatus"

        param(0, 1) = If(n_OverTimeRowID = Nothing, DBNull.Value, n_OverTimeRowID)
        param(1, 1) = EXECQUER("SELECT OrganizationID FROM employee WHERE RowID='" & e_rowid & "';") 'TxtEmployeeNumber1.RowIDValue
        param(2, 1) = If(z_User = 0, DBNull.Value, z_User)
        param(3, 1) = param(2, 1) 'z_User
        param(4, 1) = e_rowid 'TxtEmployeeNumber1.RowIDValue
        param(5, 1) = OTTypeString

        Dim iii = Format(CDate(dtpstarttime.Value), "hh:mm tt")

        param(6, 1) = MilitTime(iii)

        iii = Format(CDate(dtpendtime.Value), "hh:mm tt")

        param(7, 1) = MilitTime(iii)

        param(8, 1) = Format(CDate(dtpstartdateEmpOT.Value), "yyyy-MM-dd")

        param(9, 1) = Format(CDate(dtpendateEmpOT.Value), "yyyy-MM-dd")

        param(10, 1) = txtreasonEmpOT.Text.Trim

        param(11, 1) = txtcommentsEmpOT.Text.Trim

        Dim imageobj As Object = DBNull.Value

        param(12, 1) = imageobj

        param(13, 1) = If(OTStatusString = Nothing, "Pending", OTStatusString)

        Return _
                EXEC_INSUPD_PROCEDURE(param,
                                      "INSUPD_employeeOT_indepen",
                                      "eot_ID")

    End Function

    Function MilitTime(ByVal timeval As Object) As Object

        Dim retrnObj As Object

        retrnObj = New Object

        If timeval = Nothing Then
            retrnObj = DBNull.Value
        Else

            Dim endtime As Object = timeval

            If endtime.ToString.Contains("P") Then

                Dim hrs As String = If(Val(getStrBetween(endtime, "", ":")) = 12, 12, Val(getStrBetween(endtime, "", ":")) + 12)

                Dim mins As String = StrReverse(getStrBetween(StrReverse(endtime.ToString), "", ":"))

                mins = getStrBetween(mins, "", " ")

                retrnObj = hrs & ":" & mins

            ElseIf endtime.ToString.Contains("A") Then

                Dim i As Integer = StrReverse(endtime).ToString.IndexOf(" ")

                endtime = endtime.ToString.Replace("A", "")

                'Dim i As Integer = StrReverse("3:15 AM").ToString.IndexOf(" ")

                ''endtime = endtime.ToString.Replace("A", "")

                'MsgBox(Trim(StrReverse(StrReverse("3:15 AM").ToString.Substring(i, ("3:15 AM").ToString.Length - i))).Length)

                Dim amTime As String = Trim(StrReverse(StrReverse(endtime.ToString).Substring(i,
                                                                                  endtime.ToString.Length - i)
                                          )
                               )

                amTime = If(getStrBetween(amTime, "", ":") = "12",
                            24 & ":" & StrReverse(getStrBetween(StrReverse(amTime), "", ":")),
                            amTime)

                retrnObj = amTime

            End If

        End If

        Return retrnObj

    End Function

    Dim OTTypeString As String = String.Empty

    Private Sub cboOTType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboOTType.SelectedIndexChanged, cboOTType.SelectedValueChanged
        OTTypeString = cboOTType.Text
    End Sub

    Dim OTStatusString As String = String.Empty

    Private Sub cboOTStatus_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboOTStatus.SelectedIndexChanged, cboOTStatus.SelectedValueChanged
        OTStatusString = cboOTStatus.Text
    End Sub

    Private Sub bgwSaving_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles bgwSaving.DoWork

        If n_OverTimeRowID = Nothing Then
            n_OverTimeRowID =
                INSUPD_employeeOT()
        Else
            INSUPD_employeeOT()
        End If

    End Sub

    Private Sub bgwSaving_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles bgwSaving.ProgressChanged

    End Sub

    Private Sub bgwSaving_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bgwSaving.RunWorkerCompleted

        If e.Error IsNot Nothing Then
            MessageBox.Show("Error: " & e.Error.Message)
        ElseIf e.Cancelled Then
            MessageBox.Show("Background work cancelled.")
        Else

        End If

        Panel1.Enabled = True

        If TxtEmployeeNumber1.Exists Then
            MsgBox("Overtime successfully saved", MsgBoxStyle.Information)
            Me.Close()
        End If

    End Sub

    Private Sub cboOrganization_Leave(sender As Object, e As EventArgs) Handles cboOrganization.Leave

        previous_orgID = EXECQUER("SELECT RowID FROM organization WHERE Name='" & cboOrganization.Text & "';")

    End Sub

    Private Sub cboOrganization_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboOrganization.SelectedIndexChanged

    End Sub

    Private Sub dtpstarttime_Leave(sender As Object, e As EventArgs) Handles dtpstarttime.Leave

        'Dim starttimevalue = CDate(dtpstarttime.Value)

        'MsgBox(starttimevalue.ToString)

        'dtpendtime

    End Sub

    Dim custom_date = EXECQUER("SELECT CURDATE();")

    Dim catchdt As New DataTable

    Private Sub bgwEmpNames_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles bgwEmpNames.DoWork

        'Thread.Sleep(750)

        Dim dtempfullname As New DataTable

        'dtempfullname = retAsDatTbl("SELECT CONCAT(LastName,', ',FirstName,IF(MiddleName = '', '', CONCAT(', ',MiddleName))) 'EmpFullName'" & _
        '                            " FROM employee" & _
        '                            " GROUP BY CONCAT(LastName,', ',FirstName,IF(MiddleName = '', '', CONCAT(', ',MiddleName)));")
        'CONCAT(e.LastName,', ',e.FirstName, IF(e.MiddleName = '', '', CONCAT(', ',e.MiddleName)))
        dtempfullname = New SQLQueryToDatatable("SELECT e.RowID, CONCAT_WS(', ', e.LastName, e.FirstName, IF(LENGTH(TRIM(e.MiddleName)) = 0, NULL, e.MiddleName)) `EmpFullName`" &
                                    " FROM employee e INNER JOIN organization og ON og.RowID=e.OrganizationID" &
                                    " WHERE og.NoPurpose='0'" &
                                    " GROUP BY e.OrganizationID,e.EmployeeID;").ResultTable

        If dtempfullname IsNot Nothing Then

            For Each drow As DataRow In dtempfullname.Rows

                If CStr(drow("EmpFullName")) <> Nothing Then
                    'autcoEmpID.Items.Add(New AutoCompleteEntry(CStr(drow("EmployeeID"))))

                    TxtEmployeeFullName1.Items.Add(New AutoCompleteEntry(CStr(drow("EmpFullName")), StringToArray(CStr(drow("EmpFullName")))))

                End If

            Next

            catchdt = dtempfullname

        End If

    End Sub

    Private Sub bgwEmpNames_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles bgwEmpNames.ProgressChanged

    End Sub

    Private Sub bgwEmpNames_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bgwEmpNames.RunWorkerCompleted

        Panel1.Focus()

        If e.Error IsNot Nothing Then

            MessageBox.Show("Error: " & e.Error.Message)

        ElseIf e.Cancelled Then

            MessageBox.Show("Background work cancelled.")
        Else

            TxtEmployeeFullName1.Enabled = True

            TxtEmployeeFullName1.Text = ""

            cboxEmployees.DisplayMember = "EmpFullName"
            cboxEmployees.ValueMember = "RowID"
            cboxEmployees.DataSource = catchdt

        End If

        TxtEmployeeFullName1.Focus()

        AddHandler TxtEmployeeNumber1.GotFocus, AddressOf TxtEmployeeNumber1_GotFocus

        cboxEmployees.Text = String.Empty
    End Sub

    Private Sub TxtEmployeeNumber1_GotFocus(sender As Object, e As EventArgs) 'Handles TxtEmployeeNumber1.GotFocus

        'If TxtEmployeeFullName1.RowIDValue <> String.Empty Then

        'End If

        'Dim selectEmpID As String = EXECQUER("SELECT CAST(EmployeeID AS CHAR) FROM employee WHERE RowID='" & TxtEmployeeFullName1.RowIDValue & "' LIMIT 1;")

        'TxtEmployeeNumber1.Text = selectEmpID

    End Sub

    Private Sub TxtEmployeeNumber1_TextChanged(sender As Object, e As EventArgs) Handles TxtEmployeeNumber1.TextChanged

    End Sub

    Dim e_rowid As Integer = 0

    Private Sub cboxEmployees_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboxEmployees.SelectedIndexChanged,
                                                                                             cboxEmployees.SelectedValueChanged

        e_rowid = Convert.ToInt32(cboxEmployees.SelectedValue)

        Dim selectEmpID = EXECQUER("SELECT EmployeeID FROM employee WHERE RowID='" & e_rowid & "' LIMIT 1;")

        TxtEmployeeNumber1.Text = Convert.ToString(selectEmpID)

        TxtEmployeeNumber1.RowIDValue = e_rowid

    End Sub

    Private Sub TxtEmployeeFullName1_Leave(sender As Object, e As EventArgs) Handles TxtEmployeeFullName1.Leave

        If TxtEmployeeFullName1.Text.Trim.Length = 0 Then

            cboxEmployees.Text = String.Empty
        Else

            cboxEmployees.Text = TxtEmployeeFullName1.Text

        End If

    End Sub

End Class
