
Public Class OrganizationPrompt

    Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        OjbAssignNoContextMenu(cboOrganiz)

    End Sub

    Sub OrganizationPrompt()

        OjbAssignNoContextMenu(cboOrganiz)

    End Sub

    Dim n_RowIDValue As String = String.Empty

    Public ReadOnly Property RowIDValue As String

        Get
            Return n_RowIDValue

        End Get

    End Property

    Dim n_EmployeeRowID As String = String.Empty

    Public Property EmployeeRowIDValue As String

        Get

            Return n_EmployeeRowID

        End Get

        Set(value As String)

            n_EmployeeRowID = value

        End Set

    End Property

    Dim dbcol_organization As String = String.Empty

    Public Property OrganizationTableColumnName As String

        Get
            Return dbcol_organization

        End Get

        Set(value As String)
            dbcol_organization = value

        End Set

    End Property

    Private Sub OrganizationPrompt_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        enlistToCboBox("SELECT og.Name" & _
                       " FROM employee e" & _
                       " INNER JOIN organization og ON og.RowID=e.OrganizationID" & _
                       " WHERE " & dbcol_organization & "='" & n_EmployeeRowID & "'" & _
                       " GROUP BY e.OrganizationID;", _
                        cboOrganiz)

    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean

        If keyData = Keys.Escape Then

            btnCancel_Click(btnCancel, New EventArgs)

            Return True

        Else

            Return MyBase.ProcessCmdKey(msg, keyData)

        End If

    End Function

    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click

        If cboOrganiz.Items.Count <> 0 Then

            n_RowIDValue = EXECQUER("SELECT RowID FROM organization WHERE Name='" & cboOrganiz.Text & "';")

            n_RowIDValue = ValNoComma(n_RowIDValue)

            If n_RowIDValue = 0 Then

                Me.DialogResult = Windows.Forms.DialogResult.Cancel

            Else

                Me.DialogResult = Windows.Forms.DialogResult.OK

            End If

        Else

            Me.DialogResult = Windows.Forms.DialogResult.Cancel

        End If

        Me.Close()

    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click

        Me.DialogResult = Windows.Forms.DialogResult.Cancel

        Me.Close()

    End Sub

    Private Sub cboOrganiz_KeyDown(sender As Object, e As KeyEventArgs) Handles cboOrganiz.KeyDown
        If e.KeyCode = Keys.Enter Then
            btnOK_Click(sender, e)
        End If

    End Sub

    Private Sub cboOrganiz_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboOrganiz.SelectedIndexChanged

    End Sub

End Class