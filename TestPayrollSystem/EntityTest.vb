<TestFixture>
Public Class EntityTest
    Inherits ServiceProvider

    ''' <summary>
    ''' Mostly used as a sandbox in testing queries of entities.
    ''' </summary>

    <Test>
    Public Sub Test1()
        'Dim repo As New UserActivityRepository
        ''Dim userActivities = repo.List(11, "Employee")

        'Assert.IsTrue(UserActivityRepository.CheckIfFirstLetterIsVowel("employee"))

        ''Dim repo As New UserActivityRepository()

        ''repo.RecordAdd(1, "a")
        ''repo.RecordDelete(1, "a")
        ''repo.RecordAdd(1, "")
        ''repo.RecordDelete(1, "")
        ''repo.RecordAdd(1, "Employee")
        ''repo.RecordDelete(1, "Employee")
        ''repo.RecordAdd(1, "Paystub")
        ''repo.RecordDelete(1, "Paystub")

        ''Assert.AreEqual(4, repo.List.Count)

        ''Dim answer = True

        ''Assert.AreEqual(True, answer)
    End Sub

    <Test>
    Public Sub TestEntity()

        'Dim loanRepository = MainServiceProvider.GetRequiredService(Of LoanRepository)

        'Dim loans = Await loanRepository.GetLoanTransactionsWithPayPeriodAsync(648)

        'Assert.IsTrue(True)

        'Dim adjustment = New AdjustmentService().
        '                GetByMultipleEmployeeAndDatePeriodAsync(2, {1}, New TimePeriod(New Date(2020, 2, 1), New Date(2020, 2, 1))).
        '                ToList()

        'Dim date1 = New Date(2020, 5, 31)
        'Dim date2 = New Date(2020, 5, 30)

        'Dim date3 = date1.AddMonths(1)
        'Dim date4 = date2.AddMonths(1)

        'Dim repo1 As New UserActivityRepository()

        'Dim count1 = repo1.GetAll(2, "EMPLOYEE")
        'Dim count1u = count1.Where(Function(c) c.User Is Nothing).ToList

        ''for checking if we can access the AccuPay.Core repository without error
        'Dim repo4 As New EmployeeRepository()

        'Dim count4 = repo4.GetAllAsync(2)

        'Dim branchRepo As New BranchRepository()

        'Dim count2 = branchRepo.GetAll().Count

        'Assert.IsTrue(count2 > 0)
    End Sub

    <Test>
    Public Sub CheckVBNullableId()

        'Change the PayrollContext to public to use this
        'Using context = New Data.PayrollContext()

        '    Dim organization = context.Organizations.
        '                FirstOrDefault(Function(o) o.LastUpdBy IsNot Nothing)

        '    CompareQueries(context, organization)

        '    'Dim organizationNull = context.Organizations.
        '    '            FirstOrDefault(Function(o) o.LastUpdBy Is Nothing)

        '    'CompareQueries(context, organizationNull)

        '    Assert.IsTrue(True)

        'End Using
    End Sub

    'Private Shared Sub CompareQueries(context As Data.PayrollContext, organization As Organization)
    '    Dim organization2 = context.Organizations.
    '                                    FirstOrDefault(Function(o) o.LastUpdBy = organization.LastUpdBy)

    '    Dim organization3 = context.Organizations.
    '                    FirstOrDefault(Function(o) o.LastUpdBy.Value = organization.LastUpdBy)

    '    'Has problem when LastUpdBy is null (not because of DB)
    '    Dim organization4 = context.Organizations.
    '                    FirstOrDefault(Function(o) o.LastUpdBy = organization.LastUpdBy.Value)

    '    'Has problem when LastUpdBy is null (not because of DB)
    '    Dim organization5 = context.Organizations.
    '                    FirstOrDefault(Function(o) o.LastUpdBy.Value = organization.LastUpdBy.Value)

    '    Dim organization6 = context.Organizations.
    '                    FirstOrDefault(Function(o) CBool(o.LastUpdBy = organization.LastUpdBy))

    '    Dim organization7 = context.Organizations.
    '                    FirstOrDefault(Function(o) Nullable.Equals(o.LastUpdBy, organization.LastUpdBy))
    'End Sub

End Class
