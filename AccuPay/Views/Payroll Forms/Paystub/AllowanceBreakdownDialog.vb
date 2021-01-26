Option Strict On

Imports AccuPay.Core.Entities
Imports AccuPay.Core.Interfaces
Imports Microsoft.Extensions.DependencyInjection

Public Class AllowanceBreakdownDialog

    Private ReadOnly _paystubId As Integer
    Private ReadOnly _isTaxable As Boolean
    Private ReadOnly _paystubRepository As IPaystubRepository

    Private _allAllowanceTransactions As List(Of AllowanceItemViewModel)

    Private _boldFont As Font

    Private _normalFont As Font

    Sub New(paystubId As Integer, isTaxable As Boolean)

        InitializeComponent()

        _paystubRepository = MainServiceProvider.GetRequiredService(Of IPaystubRepository)

        _paystubId = paystubId
        _isTaxable = isTaxable
    End Sub

    Private Async Sub AllowanceBreakdownDialog_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        Dim currentFormFont = Me.Font
        _boldFont = New Font(currentFormFont.FontFamily, currentFormFont.Size, FontStyle.Bold)
        _normalFont = New Font(currentFormFont.FontFamily, currentFormFont.Size)

        AllowanceGridView.AutoGenerateColumns = False
        _allAllowanceTransactions = Await GetAllowanceTransactions()

        SetGridViewDataSource(_allAllowanceTransactions)
        AllowanceGridView.ClearSelection()

    End Sub

    Private Sub SetGridViewDataSource(allAllowanceTransactions As List(Of AllowanceItemViewModel))
        AllowanceGridView.DataSource = allAllowanceTransactions
    End Sub

    Private Sub AllowanceGridView_CellFormatting(sender As Object, e As DataGridViewCellFormattingEventArgs) Handles AllowanceGridView.CellFormatting

        Dim currentItem As AllowanceItemViewModel = GetCurrentTimeLogByGridRowIndex(e.RowIndex)

        If currentItem Is Nothing Then Return

        Dim row = AllowanceGridView.Rows(e.RowIndex)

        Select Case currentItem.RowType
            Case AllowanceItemViewModel.RowTypeEnum.Header
                row.DefaultCellStyle.BackColor = Color.Gray
                row.DefaultCellStyle.Font = _normalFont

            Case AllowanceItemViewModel.RowTypeEnum.Details
                row.DefaultCellStyle.BackColor = Color.White
                row.DefaultCellStyle.Font = _normalFont

            Case AllowanceItemViewModel.RowTypeEnum.Footer
                row.DefaultCellStyle.BackColor = Color.Yellow
                row.DefaultCellStyle.Font = _boldFont
        End Select

    End Sub

    Private Function GetCurrentTimeLogByGridRowIndex(rowIndex As Integer) As AllowanceItemViewModel

        If rowIndex < 0 Then Return Nothing

        Dim items = CType(AllowanceGridView.DataSource, List(Of AllowanceItemViewModel))

        If rowIndex > items.Count - 1 Then Return Nothing

        Return items(rowIndex)

    End Function

    Private Async Function GetAllowanceTransactions() As Threading.Tasks.Task(Of List(Of AllowanceItemViewModel))

        Dim list As New List(Of AllowanceItemViewModel)

        Dim allowanceItems = (Await _paystubRepository.GetAllowanceItemsAsync(_paystubId))

        If _isTaxable Then

            allowanceItems = allowanceItems.Where(Function(a) a.Allowance.Product.IsTaxable).ToList()
        Else
            allowanceItems = allowanceItems.Where(Function(a) Not a.Allowance.Product.IsTaxable).ToList()
        End If

        If Not allowanceItems.Any() Then Return list

        Dim allowanceWithBreakdowns = allowanceItems.
            Where(Function(i) i.AllowancesPerDay.Any()).
            OrderBy(Function(i) i.Allowance.AllowanceFrequency).
            ThenBy(Function(i) i.Allowance.Product.PartNo)

        Dim allowanceWithOutBreakdowns = allowanceItems.
            Where(Function(i) Not i.AllowancesPerDay.Any()).
            OrderBy(Function(i) i.Allowance.AllowanceFrequency).
            ThenBy(Function(i) i.Allowance.Product.PartNo)

        Dim arrangedAllowanceItems = allowanceWithOutBreakdowns.ToList()
        arrangedAllowanceItems.AddRange(allowanceWithBreakdowns)

        For Each item In arrangedAllowanceItems

            If item.Amount = 0 Then Continue For

            Dim transactions = item.AllowancesPerDay.OrderBy(Function(d) d.Date)

            Dim startingAmount As Decimal = 0

            If item.Allowance.IsSemiMonthly Then

                startingAmount = item.Allowance.Amount
            End If

            CreateRows(list, item, transactions, startingAmount)
        Next

        Return list
    End Function

    Private Shared Sub CreateRows(list As List(Of AllowanceItemViewModel), item As AllowanceItem, transactions As IOrderedEnumerable(Of AllowancePerDay), startingAmount As Decimal)

        Dim oneRowOnly =
            item.Allowance.IsOneTime OrElse
            item.Allowance.IsMonthly OrElse
            (item.Allowance.Product.Fixed AndAlso Not item.Allowance.IsDaily)

        Dim runningTotal = startingAmount

        If oneRowOnly Then

            runningTotal = item.Amount
        Else

            'header row
            list.Add(New AllowanceItemViewModel(item.Allowance, item, startingAmount, AllowanceItemViewModel.RowTypeEnum.Header))

            For Each transaction In transactions

                runningTotal += transaction.Amount
                list.Add(New AllowanceItemViewModel(item.Allowance, item, transaction, runningTotal))

            Next
        End If

        'footer row
        list.Add(New AllowanceItemViewModel(item.Allowance, item, runningTotal, AllowanceItemViewModel.RowTypeEnum.Footer))
    End Sub

    Private Sub dgvempallowance_KeyDown(sender As Object, e As KeyEventArgs) Handles AllowanceGridView.KeyDown
        If e.KeyCode = Keys.Escape Then
            Me.Close()
        End If
    End Sub

    Private Class AllowanceItemViewModel

        Private ReadOnly _allowance As Allowance

        Private ReadOnly _allowanceItem As AllowanceItem

        Private ReadOnly _allowancePerDay As AllowancePerDay

        Private ReadOnly _runningTotal As Decimal

        Private ReadOnly _transactionDate As Date?

        Public Enum RowTypeEnum
            Header
            Details
            Footer
        End Enum

        Public Sub New(allowance As Allowance, allowanceItem As AllowanceItem, allowancePerDay As AllowancePerDay, runningTotal As Decimal)
            _allowance = allowance
            _allowanceItem = allowanceItem
            _allowancePerDay = allowancePerDay

            _runningTotal = runningTotal
            _transactionDate = _allowancePerDay.Date
            Me.RowType = RowTypeEnum.Details
        End Sub

        Public Sub New(allowance As Allowance, allowanceItem As AllowanceItem, runningTotal As Decimal, rowType As RowTypeEnum)
            _allowance = allowance
            _allowanceItem = allowanceItem
            _runningTotal = runningTotal

            _transactionDate = Nothing
            Me.RowType = rowType
        End Sub

        Public ReadOnly Property RowType As RowTypeEnum

        Public ReadOnly Property TransactionDate As String
            Get
                If RowType = RowTypeEnum.Details Then

                    Return _transactionDate.Value.ToShortDateString()
                Else

                    Return _allowance.AllowanceFrequency
                End If
            End Get
        End Property

        Public ReadOnly Property AllowanceItemId As Integer?
            Get
                Return _allowanceItem?.RowID
            End Get
        End Property

        Public ReadOnly Property AllowanceName As String
            Get
                Return _allowanceItem?.Allowance?.Product?.PartNo
            End Get
        End Property

        Public ReadOnly Property Amount As Decimal?
            Get
                Return _allowancePerDay?.Amount
            End Get
        End Property

        Public ReadOnly Property RunningTotal As Decimal?
            Get
                Return _runningTotal
            End Get
        End Property

    End Class

End Class
