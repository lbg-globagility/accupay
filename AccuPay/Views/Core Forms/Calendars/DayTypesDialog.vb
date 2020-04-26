Option Strict On

Imports AccuPay.Data.Entities
Imports AccuPay.Data.Repositories

Public Class DayTypesDialog

    Private ReadOnly _repository As DayTypeRepository

    Private _dayTypes As ICollection(Of DayType)

    Public Sub New()
        _repository = New DayTypeRepository()

        InitializeComponent()
        InitializeView()
    End Sub

    Private Sub InitializeView()
        DayTypesGridView.AutoGenerateColumns = False
    End Sub

    Private Async Sub DayTypesDialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        _dayTypes = Await _repository.GetAll()
        DayTypesGridView.DataSource = _dayTypes
    End Sub

    Private Sub DayTypesGridView_SelectionChanged(sender As Object, e As EventArgs) Handles DayTypesGridView.SelectionChanged
        Dim a = DirectCast(DayTypesGridView.CurrentRow.DataBoundItem, DayType)

        DayTypeControl.DayType = a
    End Sub

    Private Async Sub SaveButton_Click(sender As Object, e As EventArgs) Handles SaveButton.Click
        Dim dayType = DayTypeControl.DayType
        Await _repository.Save(dayType)
    End Sub

End Class