Option Strict On

Imports System.Collections.ObjectModel
Imports AccuPay.Data.Entities
Imports AccuPay.Data.Services

<TestFixture>
Public Class CalendarCollectionTest

    Private _calendarCollection As CalendarCollection

    Private ReadOnly TestDate As Date = New Date(2020, 1, 1)

    <SetUp>
    Public Sub SetupCalendarCollection()
        Dim calendarDays = New Collection(Of CalendarDay) From {
            New CalendarDay() With {
                .RowID = 1,
                .CalendarID = 1,
                .[Date] = TestDate,
                .DayType = New DayType() With {
                    .RegularRate = 1
                }
            },
            New CalendarDay() With {
                .RowID = 2,
                .CalendarID = 2,
                .[Date] = TestDate,
                .DayType = New DayType() With {
                    .RegularRate = 2
                }
            },
            New CalendarDay() With {
                .RowID = 3,
                .CalendarID = 3,
                .[Date] = TestDate,
                .DayType = New DayType() With {
                    .RegularRate = 3
                }
            }
        }

        Dim branches = New Collection(Of Branch) From {
            New Branch() With {
                .RowID = 1,
                .CalendarID = 1
            },
            New Branch() With {
                .RowID = 2,
                .CalendarID = 2
            },
            New Branch() With {
                .RowID = 4
            }
        }

        Dim defaultCalendar = New PayCalendar() With {
            .RowID = 3,
            .IsDefault = True
        }

        Dim defaultRates = New DefaultRates()

        _calendarCollection = New CalendarCollection(
            Enumerable.Empty(Of PayRate)().ToList(),
            branches,
            calendarDays,
            1,
            defaultRates,
            defaultCalendar)
    End Sub

    <Test>
    Public Sub ShouldReturnCalendarByBranch()
        Dim calendar = _calendarCollection.GetCalendar(2)
        Dim calendarDay = calendar.Find(TestDate)

        Assert.That(calendarDay.RegularRate, [Is].EqualTo(2))
    End Sub

    <Test>
    Public Sub ShouldReturnDefaultCalendar_WhenThereIsNoBranch()
        Dim calendar = _calendarCollection.GetCalendar(Nothing)
        Dim calendarDay = calendar.Find(TestDate)

        Assert.That(calendarDay.RegularRate, [Is].EqualTo(3))
    End Sub

    <Test>
    Public Sub ShouldReturnDefaultCalendar_WhenBranchDoesntHaveCalendar()
        Dim calendar = _calendarCollection.GetCalendar(4)
        Dim calendarDay = calendar.Find(TestDate)

        Assert.That(calendarDay.RegularRate, [Is].EqualTo(3))
    End Sub

End Class