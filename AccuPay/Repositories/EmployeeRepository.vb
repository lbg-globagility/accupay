Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Entity
Imports AccuPay.SimplifiedEntities
Imports Microsoft.EntityFrameworkCore

Namespace Global.AccuPay.Repository

    Public Class EmployeeRepository

        Public Async Function GetAll(Of T As {New, IEmployeeBase})() As Task(Of IEnumerable(Of IEmployeeBase))

            Using context = New PayrollContext()
                Dim query = GetAllEmployeeBaseQuery(context)

                Select Case GetType(T)
                    Case GetType(GridView.Employee)

                        Dim list As New List(Of GridView.Employee)

                        list = Await query.
                                    Select(Function(e) New GridView.Employee With {
                                        .RowID = e.RowID,
                                        .EmployeeNo = e.EmployeeNo,
                                        .FirstName = e.FirstName,
                                        .MiddleName = e.MiddleName,
                                        .LastName = e.LastName,
                                        .Image = e.Image
                                    }).
                                    ToListAsync

                        Return list


                    Case Else

                        Dim list As New List(Of Entity.Employee)

                        list = Await query.ToListAsync

                        Return list

                End Select
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

        Public Async Function GetAllWithPosition() As Task(Of List(Of Employee))

            Using context = New PayrollContext()

                Dim query As IQueryable(Of Entity.Employee) = GetAllEmployeeBaseQuery(context)

                Dim list As New List(Of Entity.Employee)

                list = Await query.
                                Include(Function(e) e.Position).
                                ToListAsync

                Return list

            End Using

        End Function

        Public Async Function SearchSimpleLocal _
            (employees As IEnumerable(Of IEmployeeBase), searchValue As String) As _
            Task(Of IEnumerable(Of IEmployeeBase))

            Dim matchCriteria =
            Function(employee As IEmployeeBase) As Boolean
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
                            Where(Function(e) Nullable.Equals(e.OrganizationID, z_OrganizationID))
        End Function
#End Region

    End Class

End Namespace
