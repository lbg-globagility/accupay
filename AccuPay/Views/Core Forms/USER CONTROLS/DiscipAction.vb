
Public Class DiscipAction

    Dim IsNew As Integer
    Dim rowid As Integer
    Dim pID As Integer

    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean

        If keyData = Keys.Escape And btnSave.Enabled Then

            Me.Close()

            Return True

        Else

            Return MyBase.ProcessCmdKey(msg, keyData)

        End If

    End Function

    Private Sub fillfinding()
        Dim dt As New DataTable

        'dt = getDataTableForSQL("SELECT p.*" & _
        '                        " FROM product p" & _
        '                        " INNER JOIN category c ON c.CategoryName='Actions for disciplinary'" & _
        '                        " AND c.OrganizationID='" & orgztnID & "'" & _
        '                        " WHERE p.CategoryID=c.RowID;")

        dt = getDataTableForSQL("SELECT DisplayValue, Description, RowID FROM listofval WHERE `Type`='Employee Disciplinary Penalty';") 'Actions for disciplinary

        'Actions for disciplinary

        '"Select * From product Where OrganizationID = '" & z_OrganizationID & _
        '                        "' AND CategoryID='" & Employee.categDiscipID & "'" & _
        '                        " Order By RowID DESC"

        'Actions for disciplinary

        dgvFindingsList.Rows.Clear()
        For Each drow As DataRow In dt.Rows
            Dim n As Integer = dgvFindingsList.Rows.Add()
            With drow
                dgvFindingsList.Rows.Item(n).Cells(c_findingdesc.Index).Value = .Item("Description").ToString
                dgvFindingsList.Rows.Item(n).Cells(c_findingname.Index).Value = .Item("DisplayValue").ToString
                dgvFindingsList.Rows.Item(n).Cells(c_rowid.Index).Value = .Item("RowID").ToString
                rowid = .Item("RowID").ToString
            End With
        Next
    End Sub

    Private Sub fillfindingselected()
        Try
            If Not dgvFindingsList.Rows.Count = 0 Then
                Dim dt As New DataTable
                dt = getDataTableForSQL("Select * From product Where OrganizationID = '" & z_OrganizationID & "' And RowID = '" & dgvFindingsList.CurrentRow.Cells(c_rowid.Index).Value & "'")

                For Each drow As DataRow In dt.Rows
                    With drow
                        txtdesc.Text = .Item("Description").ToString
                        txtname.Text = .Item("partno").ToString
                        pID = .Item("RowID").ToString
                    End With
                Next
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

    End Sub

    Private Sub btnNew_Click(sender As Object, e As EventArgs) Handles btnNew.Click
        IsNew = 1
        txtdesc.Clear()
        txtname.Clear()
        txtdesc.Enabled = True
        txtname.Enabled = True
        dgvFindingsList.Enabled = False
        btnNew.Enabled = False
        txtname.Focus()

    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click

        btnSave.Enabled = False

        dgvFindingsList.Enabled = False

        If btnNew.Enabled = False Then

            EXECQUER("SELECT INSUPD_listofval('" & txtname.Text & "','" & txtname.Text & "','Employee Disciplinary Penalty','','Yes','" & txtname.Text & "','" & z_User & "',1);")

            btnCancel_Click(sender, e)

        Else

            EXECQUER("UPDATE listofval" & _
                     " SET DisplayValue='" & txtname.Text & "'" & _
                     ",LastUpd=CURRENT_TIMESTAMP()" & _
                     ",LastUpdBy='" & z_User & "'" & _
                     " WHERE RowID='" & dgvFindingsList.CurrentRow.Cells("c_rowid").Value & "'" & _
                     ";")

        End If

        myBalloon("Successfully Save", "Saving...", lblforballoon, , -100)

        btnSave.Enabled = True

        dgvFindingsList.Enabled = False

    End Sub

    Private Sub DiscipAction_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing

        Dim isthereanupdate = EXECQUER("SELECT EXISTS(SELECT RowID FROM listofval WHERE `Type`='Employee Disciplinary Penalty' AND DATE_FORMAT(LastUpd,'%Y-%m-%d')=CURDATE() LIMIT 1);")

        If isthereanupdate = "1" Then

            Me.DialogResult = Windows.Forms.DialogResult.OK

        Else

            Me.DialogResult = Windows.Forms.DialogResult.Cancel

        End If

    End Sub

    Private Sub FindingForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        btnCancel_Click(sender, e)

    End Sub

    Private Sub dgvFindingsList_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvFindingsList.CellClick

        btnNew.Enabled = True

        If dgvFindingsList.RowCount <> 0 Then

            With dgvFindingsList.CurrentRow

                txtname.Text = .Cells("c_findingname").Value
                txtdesc.Text = .Cells("c_findingdesc").Value

            End With

        Else
            txtname.Text = String.Empty
            txtdesc.Text = String.Empty

        End If

    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click

        fillfinding()

        If dgvFindingsList.RowCount <> 0 Then
            dgvFindingsList_CellClick(sender, New DataGridViewCellEventArgs(c_findingname.Index, 0))
        End If

    End Sub

    Private Sub dgvFindingsList_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvFindingsList.CellContentClick

    End Sub

End Class