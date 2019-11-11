Public Class SetEmployeeEndDate

    Dim n_EmployementStatus As String = String.Empty

    Sub New(ByVal EmpRowID As Object,
            Optional EmployementStatus As String = "")

        If EmployementStatus.Length > 0 Then

            n_EmployementStatus = EmployementStatus.Trim

        End If

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Sub SetEmployeeEndDate()

    End Sub

    Protected Overrides Sub OnLoad(e As EventArgs)

        If n_EmployementStatus.Length > 0 Then
            
            Label1.Text = n_EmployementStatus & Space(1) & Label1.Text

            Label1.Text = StrConv(Label1.Text, VbStrConv.ProperCase)

        End If

        MyBase.OnLoad(e)

    End Sub

    Dim isShowAsDialog As Boolean = False

    Public Overloads Function ShowDialog(ByVal someValue As String) As DialogResult

        With Me

            isShowAsDialog = True

            .Text = someValue

        End With

        Return Me.ShowDialog

    End Function


    '                MessageBoxManager.OK = "Actual"

    '                MessageBoxManager.Cancel = "Declared"

    '                MessageBoxManager.Register()

    'Dim custom_prompt = _
    '    MessageBox.Show("Choose the payroll summary to be printed.", "Payroll Summary Data Option", MessageBoxButtons.OKCancel, MessageBoxIcon.Information)

    '                If custom_prompt = Windows.Forms.DialogResult.OK Then

    '                    params(3, 1) = "1"

    '                    PayrollSummaChosenData = " (ACTUAL)"

    '                Else

    '                    params(3, 1) = "0"

    '                    PayrollSummaChosenData = " (DECLARED)"

    '                End If

    '                params(4, 1) = n_PayrollSummaDateSelection.
    '                               cboStringParameter.
    '                               Text

    '                MessageBoxManager.Unregister()

    Private Sub btnOk_Click(sender As Object, e As EventArgs) Handles btnOk.Click
        n_ReturnDateValue = CustomDatePicker1.Tag

        Me.DialogResult = Windows.Forms.DialogResult.OK

    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        n_ReturnDateValue = Nothing

        'Me.DialogResult = Windows.Forms.DialogResult.None

        Me.Close()

    End Sub

    Private Sub SetEmployeeEndDate_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Protected Overrides Function ProcessCmdKey(ByRef msg As Message, keyData As Keys) As Boolean

        If keyData = Keys.Escape Then

            Me.Close()

            Return True

        Else

            Return MyBase.ProcessCmdKey(msg, keyData)

        End If

    End Function

    Dim n_ReturnDateValue As Object = Nothing

    Property ReturnDateValue As Object

        Get
            Return n_ReturnDateValue
        End Get
        Set(value As Object)
            n_ReturnDateValue = value
        End Set
    End Property

    Private Sub CustomDatePicker1_ValueChanged(sender As Object, e As EventArgs) Handles CustomDatePicker1.ValueChanged



    End Sub

End Class