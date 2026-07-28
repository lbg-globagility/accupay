Option Strict On
Imports AccuPay.Core.Entities
Imports AccuPay.Core.Interfaces
Imports AccuPay.Desktop.Utilities

Public Class ContractorProjectSaveChangesForm
    Private ReadOnly _userId As Integer
    Private ReadOnly _contractorProject As ContractorProject
    Private ReadOnly _origincontractorProject As ContractorProject

    Public Sub New(userId As Integer, contractorProject As ContractorProject)
        _userId = userId
        _contractorProject = contractorProject

        _origincontractorProject = contractorProject.CloneFrom(userId, contractorProject)

        InitializeComponent()

    End Sub

    Private Async Sub ContractorProjectSaveChangesForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ClearDataBindings(Panel1)

        InitDataBindings()

        If String.IsNullOrEmpty(_contractorProject.ContractorName) Then
            Dim contractorDataService = GetRequiredService(Of IContractorDataService)()
            Dim contractor = (Await contractorDataService.GetAllAsync()).FirstOrDefault(Function(c) Integer.Equals(c.RowID, _contractorProject.ContractorId))
            txContractorName.Text = contractor.Name

        Else
            txContractorName.Text = _contractorProject.ContractorName

        End If

        AddHandler dtpEnd.CheckedChanged, AddressOf dtpEnd_CheckedChanged

    End Sub

    Private Sub InitDataBindings()
        Dim isUntouchable = False
        Dim updateMode = If(isUntouchable, DataSourceUpdateMode.Never, DataSourceUpdateMode.OnPropertyChanged)
        Const PropertyName As String = "Text"

        Dim nameBinding = New Binding(PropertyName, _contractorProject, NameOf(_contractorProject.Name), False, updateMode)
        txName.DataBindings.Add(nameBinding)
        AddHandler nameBinding.Parse,
            Sub(sender As Object, e As ConvertEventArgs)
                If Not e.DesiredType Is GetType(String) Then Return

                Dim value = CStr(e.Value)

                btnSave.Enabled = Not _origincontractorProject.Name = value AndAlso Not String.IsNullOrEmpty(value)

            End Sub

        Dim descriptionBinding = New Binding(PropertyName, _contractorProject, NameOf(_contractorProject.Description), False, updateMode)
        txDescription.DataBindings.Add(descriptionBinding)
        AddHandler descriptionBinding.Parse,
            Sub(sender As Object, e As ConvertEventArgs)
                If Not e.DesiredType Is GetType(String) Then Return

                Dim value = CStr(e.Value)

                btnSave.Enabled = Not _origincontractorProject.Description = value

            End Sub

        Dim nullDateValue = New Date?() 'DBNull.Value

        Dim beginDateBinding = New Binding("Value",
            _contractorProject,
            NameOf(_contractorProject.BeginDate),
            formattingEnabled:=True,
            dataSourceUpdateMode:=updateMode,
            nullValue:=nullDateValue)
        dtpBegin.DataBindings.Add(beginDateBinding)
        AddHandler beginDateBinding.Parse,
            Sub(sender As Object, e As ConvertEventArgs)

                Dim value = CType(e.Value, Date?)

                btnSave.Enabled = Not Date.Equals(_origincontractorProject.BeginDate, value)

            End Sub

        Dim endDateBinding = New Binding("Value",
            _contractorProject,
            NameOf(_contractorProject.EndDate),
            formattingEnabled:=True,
            dataSourceUpdateMode:=updateMode,
            nullValue:=nullDateValue)
        dtpEnd.DataBindings.Add(endDateBinding)
        AddHandler endDateBinding.Parse,
            Sub(sender As Object, e As ConvertEventArgs)

                Dim value = CType(If(dtpEnd.Checked, e.Value, endDateBinding.NullValue), Date?)

                btnSave.Enabled = Not Date.Equals(_origincontractorProject.EndDate, If(value Is Nothing, New Date?(), value))

                dtpBegin.Focus()

            End Sub

        AddHandler endDateBinding.Format,
            Sub(sender As Object, e As ConvertEventArgs)
                Dim dtp = CType(CType(sender, Binding).Control, NullableDatePicker)

                dtp.Checked = Not (Date.Equals(e.Value, New Date?()) OrElse e.Value Is Nothing OrElse IsDBNull(e.Value))

            End Sub

    End Sub

    Private Async Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        btnSave.Enabled = False

        Await FunctionUtils.TryCatchFunctionAsync("Contractor Project Save Changes",
            Async Function()
                Dim contractorProjectDataService = GetRequiredService(Of IContractorProjectDataService)()

                If _contractorProject.IsNewEntity Then Await contractorProjectDataService.SaveAsync(_contractorProject, _userId)

                If Not _contractorProject.IsNewEntity Then Await contractorProjectDataService.SaveManyAsync(currentlyLoggedInUserId:=_userId, updated:=New List(Of ContractorProject) From {_contractorProject})

                DialogResult = DialogResult.OK

                btnSave.Enabled = True

            End Function)

    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click

    End Sub

    Private Sub dtpEnd_CheckedChanged(sender As Object, e As EventArgs)
        Dim dp = DirectCast(sender, NullableDatePicker)

        dp.FocusToNext()

    End Sub

End Class
