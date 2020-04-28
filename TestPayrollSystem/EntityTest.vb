Imports AccuPay.Data.Repositories
Imports AccuPay
Imports AccuPay.Utilities
Imports Microsoft.EntityFrameworkCore
Imports AccuPay.Data.Entities
Imports AccuPay.Data.Services
Imports AccuPay.Data

<TestFixture>
Public Class EntityTest

    ''' <summary>
    ''' Mostly used as a sandbox in testing queries of entities.
    ''' </summary>

    <Test>
    Public Sub Test1()

        Assert.IsTrue(UserActivityRepository.CheckIfFirstLetterIsVowel("employee"))

        'Dim repo As New UserActivityRepository()

        'repo.RecordAdd(1, "a")
        'repo.RecordDelete(1, "a")
        'repo.RecordAdd(1, "")
        'repo.RecordDelete(1, "")
        'repo.RecordAdd(1, "Employee")
        'repo.RecordDelete(1, "Employee")
        'repo.RecordAdd(1, "Paystub")
        'repo.RecordDelete(1, "Paystub")

        'Assert.AreEqual(4, repo.List.Count)

        'Dim answer = True

        'Assert.AreEqual(True, answer)
    End Sub

    <Test>
    Public Sub TestEntity()
        'for checking if we can access the accupay.data repository without error
        Dim repo As New PaystubRepository()

        Dim count = repo.List.Count

        Assert.IsTrue(count > 0)
        Dim branchRepo As New BranchRepository()

        Dim count1 = branchRepo.GetAll().Count

        Assert.IsTrue(count1 > 0)
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