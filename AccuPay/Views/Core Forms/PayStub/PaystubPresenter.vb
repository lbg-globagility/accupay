Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Entity
Imports Microsoft.EntityFrameworkCore

Public Class PaystubPresenter

    Private _dateFrom As Date

    Private _dateTo As Date

    Private WithEvents _view As PaystubView

    Public Sub New(view As PaystubView)
        _view = view
    End Sub

    Private Async Sub OnInit() Handles _view.Init
        Dim payperiods = Await GetPayperiods()
        _view.SetPayperiods(payperiods)
        Dim paystubs = Await GetPaystubs()
        _view.SetPaystubs(paystubs)
    End Sub

    Private Sub OnEmployeeSelected() Handles _view.EmployeeSelected

    End Sub

    Private Async Sub LoadTimeEntries(paystub As Paystub)
        Dim timeEntries As IList(Of TimeEntry) = Nothing

        Using context = New PayrollContext()
            Dim query = context.TimeEntries.
                Where(Function(t) _dateFrom <= t.Date And t.Date <= _dateTo).
                Where(Function(t) Nullable.Equals(t.EmployeeID, paystub.EmployeeID))

            timeEntries = Await query.ToListAsync()
        End Using
    End Sub

    Private Async Function GetPayperiods() As Task(Of IList(Of PayPeriod))
        Dim payPeriods As IList(Of PayPeriod) = Nothing

        Using context = New PayrollContext()
            Dim query =
                (From p In context.PayPeriods
                 Join ps In context.Paystubs On p.RowID Equals ps.PayPeriodID
                 Where p.OrganizationID = z_OrganizationID
                 Group By p.RowID Into x = Group
                 Select x.Distinct().FirstOrDefault().p)

            payPeriods = Await query.ToListAsync()
        End Using
    End Function

    Private Async Function GetPaystubs() As Task(Of IList(Of Paystub))
        Dim paystubs As IList(Of Paystub) = Nothing

        Using context = New PayrollContext()
            Dim query = context.Paystubs.Include(Function(p) p.Employee).
                Where(Function(p) p.PayFromdate = _dateFrom).
                Where(Function(p) p.PayToDate = _dateTo).
                Where(Function(p) Nullable.Equals(p.OrganizationID, z_OrganizationID)).
                OrderBy(Function(p) p.Employee.LastName).
                ThenBy(Function(p) p.Employee.FirstName)

            paystubs = Await query.ToListAsync()
        End Using

        Return paystubs
    End Function

End Class
