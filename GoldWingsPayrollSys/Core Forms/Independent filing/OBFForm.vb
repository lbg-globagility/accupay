Imports Femiani.Forms.UI.Input

Public Class OBFForm

    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        OBFForm()

    End Sub

    Sub OBFForm()

        dtpOBFStartTime.CustomFormat = machineShortTimeFormat

        dtpOBFEndTime.CustomFormat = machineShortTimeFormat

    End Sub

    Dim n_OBFRowID As String = String.Empty

    Public Property OBFRowID As String

        Get

            Return n_OBFRowID

        End Get

        Set(value As String)

            n_OBFRowID = value

        End Set

    End Property

    Private Sub OBFForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

        If Panel1.Enabled = True Then

            e.Cancel = False

        Else

            e.Cancel = True

        End If

    End Sub

    Private previous_orgID As String = String.Empty

    Private Sub OBFForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        enlistToCboBox("SELECT Name FROM organization WHERE NoPurpose='0' ORDER BY Name;", _
                       cboOrganization)

        If n_OBFRowID <> Nothing Then

            Dim dtOBFRow As New DataTable

            dtOBFRow = _
            retAsDatTbl("SELECT eob.*" & _
                        ",e.EmployeeID AS EmpID" & _
                        ",IFNULL(og.Name,'') AS OrgName" & _
                        ",TIME_FORMAT(eo.OTStartTime,'%r') AS OBFStartTime" & _
                        ",TIME_FORMAT(eo.OTEndTime,'%r') AS OBFEndTime" & _
                        " FROM employeeofficialbusiness eob" & _
                        " LEFT JOIN employee e ON e.RowID=eob.EmployeeID" & _
                        " LEFT JOIN organization og ON og.RowID=eob.OrganizationID" & _
                        " WHERE eob.RowID='" & n_OBFRowID & "';")

            If dtOBFRow IsNot Nothing Then

                For Each drow As DataRow In dtOBFRow.Rows

                    TxtEmployeeNumber1.Text = If(IsDBNull(drow("EmpID")), "", CStr(drow("EmpID")))

                    cboOBFtype.Text = CStr(drow("OffBusType"))

                    Dim ii = Format(CDate(drow("OffBusStartDate")), machineShortDateFormat)

                    dtpOBFStartDate.Text = ii

                    ii = Format(CDate(drow("OffBusEndDate")), machineShortDateFormat)

                    dtpOBFEndDate.Text = ii

                    ii = Format(CDate(drow("OBFStartTime")), "hh:mm tt")

                    dtpOBFStartTime.Text = dtpOBFStartTime.MinDate & " " & ii

                    ii = Format(CDate(drow("OBFEndTime")), "hh:mm tt")

                    dtpOBFEndTime.Text = dtpOBFEndTime.MinDate & " " & ii

                    cboOBFStatus.Text = If(IsDBNull(drow("OffBusStatus")), "", CStr(drow("OffBusStatus")))

                    txtOBFReason.Text = If(IsDBNull(drow("Reason")), "", CStr(drow("Reason")))

                    txtOBFComments.Text = If(IsDBNull(drow("Comments")), "", CStr(drow("Comments")))

                    cboOrganization.Text = CStr(drow("OrgName"))

                Next

            End If

        End If

        TxtEmployeeFullName1.Enabled = False
        TxtEmployeeFullName1.EmployeeTableColumnName = "CONCAT(LastName,', ',FirstName,IF(MiddleName = '', '', CONCAT(', ',MiddleName)))"
        '"CONCAT(LastName,', ',FirstName,', ',MiddleName)"

        bgwEmpNames.RunWorkerAsync()

    End Sub

    Private Sub btnApply_Click(sender As Object, e As EventArgs) Handles btnApply.Click

        'TxtEmployeeNumber1.Focus()

        btnApply.Focus()

        'cboOrganization.Focus()

        'btnApply.Focus()

        'Dim isEmployeeExists = EXECQUER("SELECT EXISTS(SELECT RowID FROM )")

        'MsgBox(TxtEmployeeNumber1.RowIDValue & vbNewLine & TxtEmployeeNumber1.RowIDValue)

        If TxtEmployeeNumber1.Exists _
            And TxtEmployeeFullName1.Exists Then

            Panel1.Enabled = False
            bgwSaving.RunWorkerAsync()

        End If

    End Sub

    Private Function INSUPD_employeeoffbusi() As Object

        Dim param(13, 2) As Object

        param(0, 0) = "obf_RowID"
        param(1, 0) = "obf_OrganizationID"
        param(2, 0) = "obf_CreatedBy"
        param(3, 0) = "obf_LastUpdBy"
        param(4, 0) = "obf_EmployeeID"
        param(5, 0) = "obf_Type"
        param(6, 0) = "obf_StartTime"
        param(7, 0) = "obf_EndTime"
        param(8, 0) = "obf_StartDate"
        param(9, 0) = "obf_EndDate"
        param(10, 0) = "obf_Reason"
        param(11, 0) = "obf_Comments"
        param(12, 0) = "obf_Image"
        param(13, 0) = "obf_OffBusStatus"

        param(0, 1) = If(OBFRowID = Nothing, DBNull.Value, OBFRowID)
        param(1, 1) = EXECQUER("SELECT OrganizationID FROM employee WHERE RowID='" & e_rowid & "';") 'TxtEmployeeNumber1.RowIDValue
        param(2, 1) = If(z_User = 0, DBNull.Value, z_User)
        param(3, 1) = param(2, 1) 'z_User
        param(4, 1) = e_rowid 'TxtEmployeeNumber1.RowIDValue
        param(5, 1) = OBFtypeString

        Dim iii = Format(CDate(dtpOBFStartTime.Value), "hh:mm tt")

        param(6, 1) = MilitTime(iii)

        iii = Format(CDate(dtpOBFEndTime.Value), "hh:mm tt")

        param(7, 1) = MilitTime(iii)

        param(8, 1) = Format(CDate(dtpOBFStartDate.Value), "yyyy-MM-dd")

        param(9, 1) = Format(CDate(dtpOBFEndDate.Value), "yyyy-MM-dd")

        param(10, 1) = txtOBFReason.Text.Trim
        param(11, 1) = txtOBFComments.Text.Trim
        param(12, 1) = DBNull.Value 'obf_Image
        param(13, 1) = If(OBFStatusString = Nothing, "Pending", OBFStatusString)

        Return _
                EXEC_INSUPD_PROCEDURE(param, _
                                      "INSUPD_employeeoffbusi", _
                                      "obf_ID")

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

                Dim amTime As String = Trim(StrReverse(StrReverse(endtime.ToString).Substring(i, _
                                                                                  endtime.ToString.Length - i)
                                          )
                               )

                amTime = If(getStrBetween(amTime, "", ":") = "12", _
                            24 & ":" & StrReverse(getStrBetween(StrReverse(amTime), "", ":")), _
                            amTime)

                retrnObj = amTime

            End If

        End If

        Return retrnObj

    End Function

    Dim OBFtypeString As String = String.Empty

    Private Sub cboOBFtype_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboOBFtype.SelectedIndexChanged, cboOBFtype.SelectedValueChanged
        OBFtypeString = cboOBFtype.Text
    End Sub

    Dim OBFStatusString As String = String.Empty

    Private Sub cboOBFStatus_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboOBFStatus.SelectedIndexChanged, cboOBFStatus.SelectedValueChanged
        OBFStatusString = cboOBFStatus.Text
    End Sub

    Private Sub bgwSaving_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles bgwSaving.DoWork

        If n_OBFRowID = Nothing Then

            n_OBFRowID = INSUPD_employeeoffbusi()

        Else

            INSUPD_employeeoffbusi()

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
            MsgBox("Official business successfully saved", MsgBoxStyle.Information)
            Me.Close()
        End If

    End Sub

    Private Sub cboOrganization_Leave(sender As Object, e As EventArgs) Handles cboOrganization.Leave

        previous_orgID = EXECQUER("SELECT RowID FROM organization WHERE Name='" & cboOrganization.Text & "';")

    End Sub

    Private Sub cboOrganization_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboOrganization.SelectedIndexChanged

    End Sub

    Private Sub TxtEmployeeNumber1_GotFocus(sender As Object, e As EventArgs) Handles TxtEmployeeNumber1.GotFocus

        'Dim selectEmpID = EXECQUER("SELECT EmployeeID FROM employee WHERE RowID='" & TxtEmployeeFullName1.RowIDValue & "' LIMIT 1;")

        'TxtEmployeeNumber1.Text = selectEmpID

    End Sub

    Private Sub TxtEmployeeNumber1_TextChanged(sender As Object, e As EventArgs) Handles TxtEmployeeNumber1.TextChanged

    End Sub


    '*******************************LOAD ALL NAMES*************************************

    Protected Overrides Sub OnLoad(e As EventArgs)

        MyBase.OnLoad(e)

    End Sub

    Dim catchdt As New DataTable

    Private Sub bgwEmpNames_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles bgwEmpNames.DoWork

        Dim dtempfullname As New DataTable

        'dtempfullname = retAsDatTbl("SELECT CONCAT(LastName,', ',FirstName,IF(MiddleName = '', '', CONCAT(', ',MiddleName))) 'EmpFullName'" & _
        '                            " FROM employee" & _
        '                            " GROUP BY CONCAT(LastName,', ',FirstName,IF(MiddleName = '', '', CONCAT(', ',MiddleName)));")
        'CONCAT(e.LastName,', ',e.FirstName, IF(e.MiddleName = '', '', CONCAT(', ',e.MiddleName)))
        dtempfullname = New SQLQueryToDatatable("SELECT e.RowID, CONCAT_WS(', ', e.LastName, e.FirstName, IF(LENGTH(TRIM(e.MiddleName)) = 0, NULL, e.MiddleName)) `EmpFullName`" & _
                                    " FROM employee e INNER JOIN organization og ON og.RowID=e.OrganizationID" & _
                                    " WHERE og.NoPurpose='0'" & _
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

        cboxEmployees.Text = String.Empty
    End Sub

    Private Sub TxtEmployeeFullName1_Load(sender As Object, e As EventArgs) Handles TxtEmployeeFullName1.Load

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