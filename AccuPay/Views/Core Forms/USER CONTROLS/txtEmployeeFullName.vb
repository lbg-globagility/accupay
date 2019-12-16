
Public Class txtEmployeeFullName

    'Inherits TextBox
    Inherits Femiani.Forms.UI.Input.CoolTextBox

    Private organization_RowID As String = String.Empty

    Sub New()
        organization_RowID = orgztnID
    End Sub

    Dim dbcol_employee As String = String.Empty

    Public Property EmployeeTableColumnName As String

        Get
            Return dbcol_employee

        End Get

        Set(value As String)
            dbcol_employee = value

        End Set

    End Property

    Dim n_Exists As Boolean = False

    Public ReadOnly Property Exists As Boolean

        Get

            Return n_Exists

        End Get

    End Property

    Dim n_RowIDValue As String = String.Empty

    Public ReadOnly Property RowIDValue As String

        Get

            Return n_RowIDValue

        End Get

    End Property

    Protected Overrides Sub OnTextChanged(e As EventArgs)
        MyBase.OnTextChanged(e)
    End Sub

    Protected Overrides Sub OnLeave(e As EventArgs)

        Dim isExistCount =
            EXECQUER("SELECT" &
                        " COUNT(RowID)" &
                        " FROM employee" &
                        " WHERE " & dbcol_employee & "='" & MyBase.Text & "'" &
                        " AND EmploymentStatus NOT IN ('Resigned','Terminated');")

        Static once As String = String.Empty

        If once <> MyBase.Text Then

            once = MyBase.Text

            If ValNoComma(isExistCount) > 1 Then

                Dim n_OrganizationPrompt As New OrganizationPrompt

                n_OrganizationPrompt.EmployeeRowIDValue = MyBase.Text
                'n_OrganizationPrompt.OrganizationTableColumnName = "CONCAT(e.LastName,', ',e.FirstName,IF(e.MiddleName = '', '', CONCAT(', ',e.MiddleName)))"
                n_OrganizationPrompt.OrganizationTableColumnName = "CONCAT_WS(', ', e.LastName, e.FirstName, IF(LENGTH(TRIM(e.MiddleName)) = 0, NULL, e.MiddleName))"

                If n_OrganizationPrompt.ShowDialog = DialogResult.OK Then

                    organization_RowID = n_OrganizationPrompt.RowIDValue

                    If organization_RowID = String.Empty Then

                        Me.Focus()

                    End If

                Else

                End If

            Else

                organization_RowID =
                    EXECQUER("SELECT" &
                            " OrganizationID" &
                            " FROM employee" &
                            " WHERE " & dbcol_employee & "='" & MyBase.Text & "'" &
                            " AND EmploymentStatus NOT IN ('Resigned','Terminated');")

            End If

            Dim isExists =
                EXECQUER("SELECT EXISTS(SELECT" &
                            " RowID" &
                            " FROM employee" &
                            " WHERE " & dbcol_employee & "='" & MyBase.Text & "'" &
                            " AND OrganizationID='" & organization_RowID & "'" &
                            " AND EmploymentStatus NOT IN ('Resigned','Terminated'));")

            n_Exists = CBool(isExists)

            If n_Exists Then

                n_RowIDValue = EXECQUER("SELECT" &
                                        " RowID" &
                                        " FROM employee" &
                                        " WHERE " & dbcol_employee & "='" & MyBase.Text & "'" &
                                        " AND OrganizationID='" & organization_RowID & "'" &
                                        " AND EmploymentStatus NOT IN ('Resigned','Terminated');")

            Else
                n_RowIDValue = String.Empty

            End If

        Else

        End If

        MyBase.OnLeave(e)

    End Sub

End Class