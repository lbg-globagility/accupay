Option Strict On
Imports AccuPay.Core.Entities
Imports AccuPay.Core.Interfaces
Imports AccuPay.Desktop.Utilities

Public Class ContractorSaveChangesForm
    Private ReadOnly _userId As Integer
    Private ReadOnly _contractor As Contractor
    Private ReadOnly _originContractor As Contractor

    Public Sub New(userId As Integer, contractor As Contractor)
        _userId = userId
        _contractor = contractor

        _originContractor = Contractor.CloneFrom(userId, contractor)

        InitializeComponent()

    End Sub

    Private Sub ContractorSaveChangesForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ClearDataBindings(Panel1)

        InitDataBindings()

    End Sub

    Private Sub InitDataBindings()
        Dim isUntouchable = False
        Dim updateMode = If(isUntouchable, DataSourceUpdateMode.Never, DataSourceUpdateMode.OnPropertyChanged)
        Const PropertyName As String = "Text"

        Dim nameBinding = New Binding(PropertyName, _contractor, NameOf(_contractor.Name), False, updateMode)
        txName.DataBindings.Add(nameBinding)
        AddHandler nameBinding.Parse,
            Sub(sender As Object, e As ConvertEventArgs)
                If Not e.DesiredType Is GetType(String) Then Return

                Dim value = CStr(e.Value)

                btnSave.Enabled = Not _originContractor.Name = value AndAlso Not String.IsNullOrEmpty(value)

            End Sub

        Dim addressBinding = New Binding(PropertyName, _contractor, NameOf(_contractor.Address), False, updateMode)
        txAddress.DataBindings.Add(addressBinding)
        AddHandler addressBinding.Parse,
            Sub(sender As Object, e As ConvertEventArgs)
                If Not e.DesiredType Is GetType(String) Then Return

                Dim value = CStr(e.Value)

                btnSave.Enabled = Not _originContractor.Address = value

            End Sub

        Dim tinBinding = New Binding(PropertyName, _contractor, NameOf(_contractor.TIN), False, updateMode)
        txTIN.DataBindings.Add(tinBinding)
        AddHandler tinBinding.Parse,
            Sub(sender As Object, e As ConvertEventArgs)
                If Not e.DesiredType Is GetType(String) Then Return

                Dim value = CStr(e.Value)

                btnSave.Enabled = Not _originContractor.TIN = value

            End Sub

        Dim contactBinding = New Binding(PropertyName, _contractor, NameOf(_contractor.ContactInfo), False, updateMode)
        txContact.DataBindings.Add(contactBinding)
        AddHandler contactBinding.Parse,
            Sub(sender As Object, e As ConvertEventArgs)
                If Not e.DesiredType Is GetType(String) Then Return

                Dim value = CStr(e.Value)

                btnSave.Enabled = Not _originContractor.ContactInfo = value

            End Sub

        Dim descriptionBinding = New Binding(PropertyName, _contractor, NameOf(_contractor.Description), False, updateMode)
        txDescription.DataBindings.Add(descriptionBinding)
        AddHandler descriptionBinding.Parse,
            Sub(sender As Object, e As ConvertEventArgs)
                If Not e.DesiredType Is GetType(String) Then Return

                Dim value = CStr(e.Value)

                btnSave.Enabled = Not _originContractor.Description = value

            End Sub

    End Sub

    Private Async Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        btnSave.Enabled = False

        Await FunctionUtils.TryCatchFunctionAsync("Contractor Save Changes",
            Async Function()
                Dim contractorDataService = GetRequiredService(Of IContractorDataService)()

                If _contractor.IsNewEntity Then Await contractorDataService.SaveAsync(_contractor, _userId)

                If Not _contractor.IsNewEntity Then Await contractorDataService.SaveManyAsync(currentlyLoggedInUserId:=_userId, updated:=New List(Of Contractor) From {_contractor})

                DialogResult = DialogResult.OK

                btnSave.Enabled = True

            End Function)

    End Sub

    Private Sub btnClose_Click(sender As Object, e As EventArgs) Handles btnClose.Click

    End Sub

End Class
