Public Class SelfFilingForm
    Private ReadOnly LEAVE_TYPES As String() =
        {"Additional VL", "Maternity/paternity leave", "Others", "Sick leave", "Vacation leave"}
    Private Const OVERTIME_TYPE As String = "Overtime"
    Private Const PENDING_STATUS As String = "Pending"
    Private ReadOnly _formType As FilingFormType
    Private ReadOnly _employeeModels As List(Of EmployeeModel)
    Private ReadOnly UNEMPLOYEMENT_STATUSES As String() = {"Resigned", "Terminated", "Retired"}

    Public Sub New(type As FilingFormType)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        TxtEmployeeFullName.MatchingMethod = SergeUtils.StringMatchingMethod.UseWildcards

        _formType = type

        SetForm(formType:=_formType)

        _employeeModels = GetEmployeeModels()

        PopulateSearchBox(employeeModels:=_employeeModels)
    End Sub

    Private Sub SetForm(formType As FilingFormType)
        Label36.Text = "Type"
        cboleavetypes.Items.Clear()
        Select Case formType
            Case FilingFormType.Leave
                LabelTitle.Text = "LEAVE FORM"
                Label36.Text = "Leave type"
                Label36.Visible = True
                Label195.Visible = True
                cboleavetypes.Visible = True
                cboleavetypes.Items.AddRange(items:=LEAVE_TYPES)
            Case FilingFormType.Overtime
                LabelTitle.Text = "OVERTIME FORM"
                With cboleavetypes
                    .Items.Add(OVERTIME_TYPE)
                    .SelectedIndex = 0
                End With

                dtpstarttime.ShowCheckBox = False
                dtpendtime.ShowCheckBox = False
            Case FilingFormType.OfficialBusiness
                LabelTitle.Text = "OFFICIAL BUSINESS FILING FORM"
                With cboleavetypes
                    .Items.Add("Official Business")
                    .SelectedIndex = 0
                End With
                Label196.Visible = False
                Label197.Visible = False

                dtpstarttime.ShowCheckBox = True
                dtpendtime.ShowCheckBox = True

                dtpstarttime.Checked = False
                dtpendtime.Checked = False
        End Select
    End Sub

    Private Function GetEmployeeModels() As List(Of EmployeeModel)
        Dim employees = GetEmployees()
        Dim models As New List(Of EmployeeModel)

        For Each row As DataRow In employees.Rows
            Dim id As Integer = row("RowID")
            Dim ids = Split(row("RowIds"), ",").
                Select(Function(t) CInt(t)).
                ToArray()
            Dim name As String = row("EmpFullName")
            Dim count As Integer = row("ProfileCount")
            Dim orgIds As String() = Split(row("OrganizationIds"), ",")
            Dim orgNames As String() = Split(row("OrganizationNames"), "•")
            Dim employeeIds As String() = Split(row("EmployeeIds"), ",")

            models.Add(New EmployeeModel(
                id:=id,
                ids:=ids,
                name:=name,
                count:=count,
                orgIds:=orgIds,
                orgNames:=orgNames,
                employeeIds:=employeeIds))
        Next

        Return models.ToList()
    End Function

    Private Sub PopulateSearchBox(employeeModels As List(Of EmployeeModel))
        'With TxtEmployeeFullName1
        '    .DisplayMember = "Name"
        '    .ValueMember = "Id"
        '    .DataSource = employeeModels.ToList()

        '    .AutoCompleteMode = AutoCompleteMode.Suggest
        '    .AutoCompleteSource = AutoCompleteSource.CustomSource
        '    Dim completionSource = New AutoCompleteStringCollection()
        '    completionSource.AddRange(employeeModels.Select(Function(t) t.Name).ToArray())
        '    .AutoCompleteCustomSource = completionSource
        'End With

        TxtEmployeeFullName.DisplayMember = "Name"
        TxtEmployeeFullName.ValueMember = "Id"
        'TxtEmployeeFullName1.Items.AddRange(employeeModels.ToArray())
        TxtEmployeeFullName.DataSource = employeeModels.ToList()
    End Sub

    Private Function GetEmployees() As DataTable
        Dim sql = New SQL(String.Concat("SELECT e.RowID, CONCAT_WS(', ', e.LastName, e.FirstName, IF(LENGTH(TRIM(e.MiddleName)) = 0, NULL, e.MiddleName)) `EmpFullName`",
                ", MAX(e.Created) `Created`, COUNT(og.RowID) `ProfileCount`",
                ", GROUP_CONCAT(og.RowID) `OrganizationIds`",
                ", GROUP_CONCAT(og.Name SEPARATOR '•') `OrganizationNames`",
                ", GROUP_CONCAT(e.EmployeeID) `EmployeeIds`",
                ", GROUP_CONCAT(e.RowID) `RowIds`",
                " FROM employee e",
                " INNER JOIN organization og ON og.RowID=e.OrganizationID AND og.NoPurpose='0'",
                $" WHERE FIND_IN_SET(e.EmploymentStatus, '{String.Join(",", UNEMPLOYEMENT_STATUSES)}') = 0",
                " GROUP BY CONCAT(e.LastName, e.FirstName, e.MiddleName)",
                " ORDER BY CONCAT(e.LastName, e.FirstName, e.MiddleName);"))

        Return sql.GetFoundRows.Tables.OfType(Of DataTable).FirstOrDefault()
    End Function

    Private Sub TxtEmployeeFullName_SelectedIndexChanged(sender As Object, e As EventArgs) Handles TxtEmployeeFullName.SelectedIndexChanged

    End Sub

    Private Sub TxtEmployeeFullName_Leave(sender As Object, e As EventArgs) Handles TxtEmployeeFullName.Leave,
        TxtEmployeeFullName.TextChanged

        If TxtEmployeeFullName.Text.Trim.Length = 0 Then
            Dim id As Integer = TxtEmployeeFullName.SelectedValue
            TxtEmployeeFullName_SelectedValueChanged(TxtEmployeeFullName, New EventArgs)
        End If
    End Sub

    Private Sub TxtEmployeeFullName_SelectedValueChanged(sender As Object, e As EventArgs) Handles TxtEmployeeFullName.SelectedValueChanged

        Dim id As Integer = TxtEmployeeFullName.SelectedValue
        Dim model = GetSelectedEmployeeModel()

        If model IsNot Nothing Then

            If model.Count > 1 Then
                Dim f As New EmployeeIdSelector(subInfo:=model.SubInfo)
                If f.ShowDialog = DialogResult.OK Then
                    Dim selectedSubInfo = f.GetSelectedSubInfo()
                    TxtEmployeeID.Text = selectedSubInfo.EmployeeID
                    model.SetSelectedSubInfo(selectedSubInfo:=selectedSubInfo)
                Else
                    ClearTxtEmployeeID()
                End If
            Else
                TxtEmployeeID.Text = model.DefaultEmployeeID
            End If
        Else
            ClearTxtEmployeeID()
        End If
    End Sub

    Private Sub dtpstartdate_ValueChanged(sender As Object, e As EventArgs) Handles dtpstartdate.ValueChanged
        dtpstartdate.Tag = Format(CDate(dtpstartdate.Value), "yyyy-MM-dd")
    End Sub

    Private Sub dtpendate_ValueChanged(sender As Object, e As EventArgs) Handles dtpenddate.ValueChanged
        dtpenddate.Tag = Format(CDate(dtpenddate.Value), "yyyy-MM-dd")
    End Sub

    Private Sub ClearTxtEmployeeID()
        TxtEmployeeID.Text = String.Empty
    End Sub

    Private Class EmployeeModel

        Public Sub New(id As Integer,
                ids As Integer(),
                name As String,
                count As Integer,
                orgIds As String(),
                orgNames As String(),
                employeeIds As String())
            Me.Id = id
            'Me.IDs = ids
            Me.Name = name
            Me.Count = count
            'Me.OrganizationIDs = orgIds
            'Me.OrganizationNames = orgNames
            'Me.EmployeeIDs = employeeIds

            SubInfo = New List(Of EmployeeSubInfo)
            For Each i As Integer In Enumerable.Range(0, count)
                'SubInfo.Add(New EmployeeSubInfo(orgId:=OrganizationIDs(i),
                '        orgName:=OrganizationNames(i),
                '        employeeRowId:=Me.IDs(i),
                '        employeeId:=Me.EmployeeIDs(i)))
                SubInfo.Add(New EmployeeSubInfo(orgId:=orgIds(i),
                        orgName:=orgNames(i),
                        employeeRowId:=ids(i),
                        employeeId:=employeeIds(i)))
            Next

            Dim firstSubInfo = SubInfo.FirstOrDefault()
            DefaultEmployeeRowID = firstSubInfo.EmployeeRowID
            DefaultEmployeeID = firstSubInfo.EmployeeID
            DefaultOrganizationID = firstSubInfo.OrganizationID
            DefaultOrganizationName = firstSubInfo.OrganizationName
        End Sub

        Public ReadOnly Property Id As Integer

        'Private ReadOnly Property IDs As Integer()
        Public ReadOnly Property Name As String

        Public ReadOnly Property Count As Integer

        'Private ReadOnly Property OrganizationIDs As String()
        'Private ReadOnly Property OrganizationNames As String()
        'Private ReadOnly Property EmployeeIDs As String()
        Public ReadOnly Property SubInfo As List(Of EmployeeSubInfo)

        Public ReadOnly Property DefaultEmployeeRowID As String
        Public ReadOnly Property DefaultEmployeeID As String
        Public ReadOnly Property DefaultOrganizationID As Integer
        Public ReadOnly Property DefaultOrganizationName As String

        Friend Sub SetSelectedSubInfo(selectedSubInfo As EmployeeSubInfo)
            If selectedSubInfo Is Nothing Then Return
            _DefaultEmployeeRowID = selectedSubInfo.EmployeeRowID
            _DefaultEmployeeID = selectedSubInfo.EmployeeID
            _DefaultOrganizationID = selectedSubInfo.OrganizationID
            _DefaultOrganizationName = selectedSubInfo.OrganizationName
        End Sub
    End Class

    Public Class EmployeeSubInfo

        Public Sub New(orgId As Integer,
                orgName As String,
                employeeRowId As Integer,
                employeeId As String)
            Me.OrganizationID = orgId
            Me.OrganizationName = orgName
            Me.EmployeeRowID = employeeRowId
            Me.EmployeeID = employeeId
        End Sub

        Public ReadOnly Property OrganizationID As Integer
        Public ReadOnly Property OrganizationName As String
        Public ReadOnly Property EmployeeRowID As String
        Public ReadOnly Property EmployeeID As String
    End Class

    Public Enum FilingFormType
        Leave
        Overtime
        OfficialBusiness
    End Enum

    Private Sub btnApply_Click(sender As Object, e As EventArgs) Handles btnApply.Click
        If sender IsNot Nothing Then Return

        Select Case _formType
            Case FilingFormType.Leave
                Dim identity = INSUPD_employeeleave()
                If identity IsNot Nothing Then
                    MsgBox("Leave successfully saved", MsgBoxStyle.Information)
                    Close()
                End If
            Case FilingFormType.Overtime
                Dim identity = INSUPD_employeeOT()
                If identity IsNot Nothing Then
                    MsgBox("Overtime successfully saved", MsgBoxStyle.Information)
                    Close()
                End If
            Case FilingFormType.OfficialBusiness
                Dim identity = INSUPD_employeeoffbusi()
                If identity IsNot Nothing Then
                    MsgBox("Official business successfully saved", MsgBoxStyle.Information)
                    Close()
                End If
        End Select
    End Sub

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

    Function INSUPD_employeeleave() As Object
        Dim model = GetSelectedEmployeeModel()
        If model Is Nothing Then
            MessageBox.Show("Please select an employee.", "Invalid employee", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return Nothing
        End If

        Dim selectedLeaveType = cboleavetypes.Text
        Dim hasSelectedLeaveType = LEAVE_TYPES.Contains(selectedLeaveType)
        If Not hasSelectedLeaveType Then
            MessageBox.Show("Please select a leave type.", "Invalid leave type", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return Nothing
        End If

        Dim param(14, 2) As Object

        param(0, 0) = "elv_RowID"
        param(1, 0) = "elv_OrganizationID"
        param(2, 0) = "elv_LeaveStartTime"
        param(3, 0) = "elv_LeaveType"
        param(4, 0) = "elv_CreatedBy"
        param(5, 0) = "elv_LastUpdBy"
        param(6, 0) = "elv_EmployeeID"
        param(7, 0) = "elv_LeaveEndTime"
        param(8, 0) = "elv_LeaveStartDate"
        param(9, 0) = "elv_LeaveEndDate"
        param(10, 0) = "elv_Reason"
        param(11, 0) = "elv_Comments"
        param(12, 0) = "elv_Image"
        param(13, 0) = "elv_Status"
        param(14, 0) = "elv_OverrideLeaveBal"

        param(0, 1) = DBNull.Value
        param(1, 1) = model.DefaultOrganizationID

        Dim iii = Format(CDate(dtpstarttime.Value), "hh:mm tt")

        param(2, 1) = MilitTime(iii) 'Start time

        param(3, 1) = selectedLeaveType 'Leave type
        param(4, 1) = If(z_User = 0, DBNull.Value, z_User)
        param(5, 1) = param(4, 1)
        param(6, 1) = model.DefaultEmployeeRowID

        iii = Format(CDate(dtpendtime.Value), "hh:mm tt")

        param(7, 1) = MilitTime(iii) 'End time
        param(8, 1) = Format(CDate(dtpstartdate.Value), "yyyy-MM-dd") 'Start date
        param(9, 1) = Format(CDate(dtpenddate.Value), "yyyy-MM-dd") 'End date
        param(10, 1) = Trim(txtreason.Text) 'Reason
        param(11, 1) = Trim(txtcomments.Text) 'Comments

        Dim imageobj As Object = DBNull.Value

        param(12, 1) = imageobj

        param(13, 1) = PENDING_STATUS

        param(14, 1) = 0

        Return _
                EXEC_INSUPD_PROCEDURE(param,
                                      "INSUPD_employeeleave_indepen",
                                      "empleaveID")

    End Function

    Private Function GetSelectedEmployeeModel() As EmployeeModel
        Dim id As Integer = TxtEmployeeFullName.SelectedValue
        Dim model = _employeeModels.FirstOrDefault(Function(t) t.Id = id)
        Return model
    End Function

    Private Function INSUPD_employeeOT() As Object
        Dim model = GetSelectedEmployeeModel()
        If model Is Nothing Then
            MessageBox.Show("Please select an employee.", "Invalid employee", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return Nothing
        End If

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

        param(0, 1) = DBNull.Value
        param(1, 1) = model.DefaultOrganizationID 'TxtEmployeeNumber1.RowIDValue
        param(2, 1) = If(z_User = 0, DBNull.Value, z_User)
        param(3, 1) = param(2, 1) 'z_User
        param(4, 1) = model.DefaultEmployeeRowID 'TxtEmployeeNumber1.RowIDValue
        param(5, 1) = OVERTIME_TYPE

        Dim iii = Format(CDate(dtpstarttime.Value), "hh:mm tt")

        param(6, 1) = MilitTime(iii)

        iii = Format(CDate(dtpendtime.Value), "hh:mm tt")

        param(7, 1) = MilitTime(iii)

        param(8, 1) = Format(CDate(dtpstartdate.Value), "yyyy-MM-dd")

        param(9, 1) = Format(CDate(dtpenddate.Value), "yyyy-MM-dd")

        param(10, 1) = txtreason.Text.Trim

        param(11, 1) = txtcomments.Text.Trim

        Dim imageobj As Object = DBNull.Value

        param(12, 1) = imageobj

        param(13, 1) = PENDING_STATUS

        Return _
                EXEC_INSUPD_PROCEDURE(param,
                                      "INSUPD_employeeOT_indepen",
                                      "eot_ID")

    End Function

    Private Function INSUPD_employeeoffbusi() As Object
        Dim model = GetSelectedEmployeeModel()
        If model Is Nothing Then
            MessageBox.Show("Please select an employee.", "Invalid employee", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return Nothing
        End If

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

        param(0, 1) = DBNull.Value
        param(1, 1) = model.DefaultOrganizationID 'TxtEmployeeNumber1.RowIDValue
        param(2, 1) = If(z_User = 0, DBNull.Value, z_User)
        param(3, 1) = param(2, 1) 'z_User
        param(4, 1) = model.DefaultEmployeeRowID 'TxtEmployeeNumber1.RowIDValue
        param(5, 1) = "Official Business"

        Dim iii = Format(CDate(dtpstarttime.Value), "hh:mm tt")

        param(6, 1) = If(Not dtpstarttime.Checked, DBNull.Value, MilitTime(iii))

        iii = Format(CDate(dtpendtime.Value), "hh:mm tt")

        param(7, 1) = If(Not dtpendtime.Checked, DBNull.Value, MilitTime(iii))

        param(8, 1) = Format(CDate(dtpstartdate.Value), "yyyy-MM-dd")

        param(9, 1) = Format(CDate(dtpenddate.Value), "yyyy-MM-dd")

        param(10, 1) = txtreason.Text.Trim
        param(11, 1) = txtcomments.Text.Trim
        param(12, 1) = DBNull.Value 'obf_Image
        param(13, 1) = PENDING_STATUS

        Return _
                EXEC_INSUPD_PROCEDURE(param,
                                      "INSUPD_employeeoffbusi_indepen",
                                      "obf_ID")

    End Function

End Class
