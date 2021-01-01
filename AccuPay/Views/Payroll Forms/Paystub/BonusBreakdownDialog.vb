Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Core.Entities
Imports AccuPay.Core.Repositories
Imports AccuPay.Core.ValueObjects
Imports Microsoft.Extensions.DependencyInjection

Public Class BonusBreakdownDialog
    Private ReadOnly _bonusRepository As BonusRepository
    Private ReadOnly _employeeId As Integer
    Private ReadOnly _datePeriod As TimePeriod

    Public Sub New(employeeId As Integer, datePeriod As TimePeriod)
        InitializeComponent()

        _bonusRepository = MainServiceProvider.GetRequiredService(Of BonusRepository)

        _employeeId = employeeId
        _datePeriod = datePeriod
    End Sub

    Private Async Sub BonusBreakdownDialog_Load(sender As Object, e As EventArgs) Handles Me.Load
        BonusGridView.AutoGenerateColumns = False

        Await LoadBonus()
    End Sub

    Private Async Function LoadBonus() As Task
        Dim bonusList = Await _bonusRepository.
            GetByEmployeeAndPayPeriodAsync(
                organizationId:=z_OrganizationID,
                employeeId:=_employeeId,
                timePeriod:=_datePeriod)

        Dim bonuses = bonusList.
            Select(Function(b) New BonusBreakdownItem(b)).
            OrderBy(Function(b) b.BonusType).
            ThenBy(Function(b) b.AllowanceFrequency).
            ThenBy(Function(b) b.Amount).
            ToList()

        BonusGridView.DataSource = bonuses
    End Function

    Private Class BonusBreakdownItem

        Public Sub New(bonus As Bonus)
            _BonusType = bonus.BonusType
            _Amount = If(bonus.BonusAmount, 0)
            _AllowanceFrequency = bonus.AllowanceFrequency
        End Sub

        Public ReadOnly Property BonusType As String
        Public ReadOnly Property Amount As Decimal
        Public ReadOnly Property AllowanceFrequency As String
    End Class

End Class
