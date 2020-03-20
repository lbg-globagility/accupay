Option Strict On

Imports AccuPay.Entity
Imports AccuPay.Repository

Public Class DayTypesDialog

    Private _repository As DayTypeRepository

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

End Class
