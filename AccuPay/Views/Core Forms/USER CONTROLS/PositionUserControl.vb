Option Strict On

Imports AccuPay.Entity
Imports AccuPay.JobLevels

Public Class PositionUserControl

    Dim _position As New Position
    Dim _childDivisions As New List(Of Division)
    Dim _jobLevels As New List(Of JobLevel)

    Private Sub PositionUserControl_Load(sender As Object, e As EventArgs) Handles Me.Load

        ToggleJobLevelVisiblity()

        PrepareForm()

    End Sub

    Public Sub SetPosition(
                position As Position,
                childDivisions As List(Of Division),
                jobLevels As List(Of JobLevel))

        _childDivisions = childDivisions
        _jobLevels = jobLevels

        _position = position

        DivisionComboBox.DataSource = _childDivisions.OrderBy(Function(d) d.FullDivisionName).ToList
        JobLevelComboBox.DataSource = _jobLevels

        PrepareForm()


    End Sub

#Region "Private Functions"

    Private Sub PrepareForm()

        PrepareComboBoxes()

        CreateFieldDataBindings()

    End Sub

    Private Sub ToggleJobLevelVisiblity()

        Dim showJobLevel = FeatureListChecker.Instance.HasAccess(Feature.JobLevel)

        If showJobLevel = False Then

            JobLevelLabel.Visible = False
            JobLevelComboBox.Visible = False

        End If

    End Sub

    Private Sub PrepareComboBoxes()

        DivisionComboBox.DisplayMember = "FullDivisionName"
        DivisionComboBox.ValueMember = "RowID"

        JobLevelComboBox.DisplayMember = "Name"
        JobLevelComboBox.ValueMember = "RowID"

    End Sub

    Private Sub CreateFieldDataBindings()

        DivisionComboBox.DataBindings.Clear()
        DivisionComboBox.DataBindings.Add("SelectedValue", Me._position, "DivisionID")

        PositionNameTextBox.DataBindings.Clear()
        PositionNameTextBox.DataBindings.Add("Text", Me._position, "Name")

        JobLevelComboBox.DataBindings.Clear()
        JobLevelComboBox.DataBindings.Add("SelectedValue", Me._position, "JobLevelID")

    End Sub

    Public Sub ShowError(ColumnName As String, ErrorMessage As String)

        If ColumnName = "DivisionID" Then

            ShowBalloonInfo(ErrorMessage, "Division", DivisionComboBox)

        ElseIf ColumnName = "Name" Then

            ShowBalloonInfo(ErrorMessage, "Name", PositionNameTextBox)

        End If


    End Sub

    Private Sub ShowBalloonInfo(content As String, title As String, control As Control)

        Dim win32Window = CType(control, IWin32Window)

        myBalloon(content, title, win32Window)

        control.Focus()

    End Sub

    Private Sub DivisionComboBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DivisionComboBox.SelectedIndexChanged

    End Sub

#End Region

End Class
