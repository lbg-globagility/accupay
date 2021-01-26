Option Strict On

Imports System.Collections.ObjectModel
Imports AccuPay.Core.Entities
Imports AccuPay.Core.ValueObjects

<TestFixture>
Public Class SalaryCutoffTest

    <Test>
    Public Sub ShouldSuccess_WhenSalariesIsWithinCutoff()
        Dim salaries = New Collection(Of Salary) From {
            New Salary() With {.EffectiveFrom = Date.Parse("2020-01-01"), .BasicSalary = 100}
        }
        Dim cutoff = New TimePeriod(Date.Parse("2020-01-01"), Date.Parse("2020-01-15"))

        Dim result = GetSalary(salaries, cutoff)

        Assert.That(result?.BasicSalary, [Is].EqualTo(100))
    End Sub

    <Test>
    Public Sub ShouldReturnFirstSalary_WhenCutoffIsFirst()
        Dim salaries = New Collection(Of Salary) From {
            New Salary() With {.EffectiveFrom = Date.Parse("2020-01-01"), .BasicSalary = 1000},
            New Salary() With {.EffectiveFrom = Date.Parse("2020-02-01"), .BasicSalary = 2000},
            New Salary() With {.EffectiveFrom = Date.Parse("2020-03-01"), .BasicSalary = 3000}
        }
        Dim cutoff = New TimePeriod(Date.Parse("2020-01-01"), Date.Parse("2020-01-15"))

        Dim result = GetSalary(salaries, cutoff)

        Assert.That(result?.BasicSalary, [Is].EqualTo(1000))
    End Sub

    <Test>
    Public Sub ShouldReturnMiddleSalary_WhenCutoffIsInMiddle()
        Dim salaries = New Collection(Of Salary) From {
            New Salary() With {.EffectiveFrom = Date.Parse("2020-01-01"), .BasicSalary = 1000},
            New Salary() With {.EffectiveFrom = Date.Parse("2020-02-01"), .BasicSalary = 2000},
            New Salary() With {.EffectiveFrom = Date.Parse("2020-03-01"), .BasicSalary = 3000}
        }
        Dim cutoff = New TimePeriod(Date.Parse("2020-02-01"), Date.Parse("2020-02-15"))

        Dim result = GetSalary(salaries, cutoff)

        Assert.That(result?.BasicSalary, [Is].EqualTo(2000))
    End Sub

    <Test>
    Public Sub ShouldReturnLastSalary_WhenCutoffIsLast()
        Dim salaries = New Collection(Of Salary) From {
            New Salary() With {.EffectiveFrom = Date.Parse("2020-01-01"), .BasicSalary = 1000},
            New Salary() With {.EffectiveFrom = Date.Parse("2020-02-01"), .BasicSalary = 2000},
            New Salary() With {.EffectiveFrom = Date.Parse("2020-03-01"), .BasicSalary = 3000}
        }
        Dim cutoff = New TimePeriod(Date.Parse("2020-03-01"), Date.Parse("2020-03-15"))

        Dim result = GetSalary(salaries, cutoff)

        Assert.That(result?.BasicSalary, [Is].EqualTo(3000))
    End Sub

    <Test>
    Public Sub ShouldReturnSalary_WhenSalaryIsBetweenCutoff()
        Dim salaries = New Collection(Of Salary) From {
            New Salary() With {.EffectiveFrom = Date.Parse("2020-01-05"), .BasicSalary = 1000}
        }
        Dim cutoff = New TimePeriod(Date.Parse("2020-01-01"), Date.Parse("2020-01-15"))

        Dim result = GetSalary(salaries, cutoff)

        Assert.That(result?.BasicSalary, [Is].EqualTo(1000))
    End Sub

    <Test>
    Public Sub ShouldReturnNull_WhenSalaryIsAfterCutoff()
        Dim salaries = New Collection(Of Salary) From {
            New Salary() With {.EffectiveFrom = Date.Parse("2020-05-01"), .BasicSalary = 1000}
        }
        Dim cutoff = New TimePeriod(Date.Parse("2020-01-01"), Date.Parse("2020-01-15"))

        Dim result = GetSalary(salaries, cutoff)

        Assert.That(result, [Is].Null)
    End Sub

    <Test>
    Public Sub ShouldReturnSalary_WhenSalaryIsBeforeCutoff()
        Dim salaries = New Collection(Of Salary) From {
            New Salary() With {.EffectiveFrom = Date.Parse("2020-01-01"), .BasicSalary = 1000}
        }
        Dim cutoff = New TimePeriod(Date.Parse("2020-05-01"), Date.Parse("2020-05-15"))

        Dim result = GetSalary(salaries, cutoff)

        Assert.That(result?.BasicSalary, [Is].EqualTo(1000))
    End Sub

    Private Function GetSalary(salaries As ICollection(Of Salary), cutoff As TimePeriod) As Salary
        Return salaries.Where(Function(s) s.EffectiveFrom <= cutoff.End).
            OrderByDescending(Function(s) s.EffectiveFrom).
            FirstOrDefault()
    End Function

End Class