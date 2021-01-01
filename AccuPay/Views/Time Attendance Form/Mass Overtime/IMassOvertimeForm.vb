Imports AccuPay.Core.Entities

Public Interface IMassOvertimeForm
    Sub RefreshDataGrid()
    Sub ShowEmployees(divisions As IEnumerable(Of Division), employees As IEnumerable(Of Employee))
    Sub ShowOvertimes(overtimes As DataTable)
    Sub ShowOvertimes(overtimes As List(Of OvertimeModel))
    Function GetActiveEmployees() As IList(Of Employee)
End Interface
