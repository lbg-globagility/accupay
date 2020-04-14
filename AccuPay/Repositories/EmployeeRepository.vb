Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Entity
Imports Microsoft.EntityFrameworkCore

Namespace Global.AccuPay.Repository

    Public Class EmployeeRepository

        Public Async Function GetAllAsync() As Task(Of List(Of Employee))

            Using context = New PayrollContext()

                Return Await GetAllEmployeeBaseQuery(context).ToListAsync

            End Using

        End Function

        Public Async Function GetAllActiveAsync() As Task(Of List(Of Employee))

            Using context = New PayrollContext()

                Dim query = GetAllEmployeeBaseQuery(context)

                Return Await query.
                    Where(Function(l) l.IsActive).
                    ToListAsync()

            End Using

        End Function

        Public Async Function GetAllActiveWithoutPayrollAsync(payPeriodId As Integer?) As Task(Of List(Of Employee))

            Using context = New PayrollContext(PayrollContext.DbCommandConsoleLoggerFactory)

                Dim query = GetAllEmployeeBaseQuery(context).
                                Where(Function(l) l.IsActive)

                Return Await query.
                                Where(Function(e) context.Paystubs.
                                                    Any(Function(p) p.EmployeeID.Value = e.RowID.Value AndAlso p.PayPeriodID.Value = payPeriodId.Value) = False).
                                ToListAsync

            End Using

        End Function

        Public Async Function GetAllWithPayrollAsync(payPeriodId As Integer) As Task(Of List(Of Employee))

            Using context = New PayrollContext(PayrollContext.DbCommandConsoleLoggerFactory)

                Return Await GetAllEmployeeBaseQuery(context).
                                Where(Function(e) context.Paystubs.
                                                    Any(Function(p) p.EmployeeID.Value = e.RowID.Value AndAlso p.PayPeriodID.Value = payPeriodId)).
                                ToListAsync

            End Using

        End Function

        Public Async Function GetByEmployeeNumberAsync(employeeNumber As String) As Task(Of Employee)

            Using context = New PayrollContext()

                Dim query = GetAllEmployeeBaseQuery(context)

                Return Await query.
                    Where(Function(l) l.EmployeeNo = employeeNumber).
                    FirstOrDefaultAsync()

            End Using

        End Function

        Public Async Function GetAllWithPositionAsync() As Task(Of List(Of Employee))

            Using context = New PayrollContext()

                Dim query = GetAllEmployeeBaseQuery(context)

                Return Await query.
                                Include(Function(e) e.Position).
                                ToListAsync

            End Using

        End Function

        Public Async Function GetAllActiveWithPositionAsync() As Task(Of List(Of Employee))

            Using context = New PayrollContext()

                Dim query = GetAllEmployeeBaseQuery(context)

                Return Await query.
                                Include(Function(e) e.Position).
                                Where(Function(l) l.IsActive).
                                ToListAsync

            End Using

        End Function

        Public Async Function GetEmployeeWithDivisionAsync(id As Integer?) As Task(Of Employee)

            Using context = New PayrollContext()

                Dim query = GetAllEmployeeBaseQuery(context)

                Return Await query.
                                Include(Function(e) e.Position.Division).
                                Where(Function(l) l.RowID.Value = id.Value).
                                Where(Function(l) l.IsActive).
                                FirstOrDefaultAsync

            End Using

        End Function

        Public Async Function SearchSimpleLocal _
            (employees As IEnumerable(Of Employee), searchValue As String) As _
            Task(Of IEnumerable(Of Employee))

            If employees Is Nothing OrElse employees.Count = 0 Then Return employees

            Dim matchCriteria =
            Function(employee As Employee) As Boolean
                Dim containsEmployeeId = employee.EmployeeNo.ToLower().Contains(searchValue)
                Dim containsFullName = (employee.FirstName.ToLower() + " " + employee.LastName.ToLower()).
                                        Contains(searchValue)

                Dim reverseFullName = employee.LastName.ToLower() + " " + employee.FirstName.ToLower()
                Dim containsFullNameInReverse = reverseFullName.Contains(searchValue)

                Return containsEmployeeId Or containsFullName Or containsFullNameInReverse
            End Function

            Return Await Task.Run(Function() employees.Where(matchCriteria).ToList())

        End Function

#Region "Private Functions"

        Private Function GetAllEmployeeBaseQuery(context As PayrollContext) As IQueryable(Of Entity.Employee)
            Return context.Employees.
                Include(Function(e) e.Position).
                Include(Function(e) e.Salaries).
                Include(Function(e) e.Branch).
                Where(Function(e) Nullable.Equals(e.OrganizationID, z_OrganizationID))
        End Function

#End Region

    End Class

End Namespace