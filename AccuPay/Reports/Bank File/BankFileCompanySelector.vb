Option Strict On

Public Class BankFileCompanySelector
    Private ReadOnly _companyNames As String()
    Private Const ALL As String = "All"

    Private _selectedCompanyName As String

    Public Sub New(companyNames() As String)
        Dim fasdfasd = New String() {ALL}
        Dim sdfsdfsd = fasdfasd.Concat(companyNames)
        _companyNames = sdfsdfsd.ToArray()

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        cboCompanyNames.Items.AddRange(_companyNames)
        cboCompanyNames.SelectedItem = ALL
    End Sub

    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        DialogResult = DialogResult.OK
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        DialogResult = DialogResult.Cancel
    End Sub

    Public ReadOnly Property SelectedCompanyName As String
        Get
            Return _selectedCompanyName
        End Get
    End Property

    Public ReadOnly Property IsAll As Boolean
        Get
            Return _selectedCompanyName = ALL
        End Get
    End Property

    Private Sub cboCompanyNames_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboCompanyNames.SelectedIndexChanged
        _selectedCompanyName = CStr(cboCompanyNames.SelectedItem)
    End Sub

End Class
