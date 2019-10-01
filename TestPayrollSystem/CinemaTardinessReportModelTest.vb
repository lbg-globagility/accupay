Option Strict On

Imports AccuPay

<TestFixture>
Public Class CinemaTardinessReportModelTest

    Public Property _cinemaTardinessReportModel As CinemaTardinessReportModel

    <SetUp>
    Public Sub Setup()
        _cinemaTardinessReportModel = New CinemaTardinessReportModel() With {
            .EmployeeName = "Santos, Joshua Noel, C.",
            .Days = 1,
            .Hours = 2,
            .NumberOfOffense = 3
        }
    End Sub

    <Test>
    Public Sub ShouldOutputRightData()

        Assert.AreEqual(_cinemaTardinessReportModel.EmployeeName, "Santos, Joshua Noel, C.")
        Assert.AreEqual(_cinemaTardinessReportModel.Days, 1)
        Assert.AreEqual(_cinemaTardinessReportModel.Hours, 2)
        Assert.AreEqual(_cinemaTardinessReportModel.NumberOfOffense, 3)
        Assert.AreEqual(_cinemaTardinessReportModel.NumberOfOffenseOrdinal, "3rd")
        Assert.AreEqual(_cinemaTardinessReportModel.Sanction, "4 days Suspension")

    End Sub

    <Test>
    Public Sub ShouldReturnProperNumberOfOffenseOrdinal()

        _cinemaTardinessReportModel.NumberOfOffense = (New System.Random()).Next(-1001, 1)
        Assert.AreEqual(_cinemaTardinessReportModel.NumberOfOffenseOrdinal, "-")

        _cinemaTardinessReportModel.NumberOfOffense = -10
        Assert.AreEqual(_cinemaTardinessReportModel.NumberOfOffenseOrdinal, "-")

        _cinemaTardinessReportModel.NumberOfOffense = -1
        Assert.AreEqual(_cinemaTardinessReportModel.NumberOfOffenseOrdinal, "-")

        _cinemaTardinessReportModel.NumberOfOffense = 0
        Assert.AreEqual(_cinemaTardinessReportModel.NumberOfOffenseOrdinal, "-")

        _cinemaTardinessReportModel.NumberOfOffense = 1
        Assert.AreEqual(_cinemaTardinessReportModel.NumberOfOffenseOrdinal, "1st")

        _cinemaTardinessReportModel.NumberOfOffense = 2
        Assert.AreEqual(_cinemaTardinessReportModel.NumberOfOffenseOrdinal, "2nd")

        _cinemaTardinessReportModel.NumberOfOffense = 3
        Assert.AreEqual(_cinemaTardinessReportModel.NumberOfOffenseOrdinal, "3rd")

        _cinemaTardinessReportModel.NumberOfOffense = 4
        Assert.AreEqual(_cinemaTardinessReportModel.NumberOfOffenseOrdinal, "4th")

        _cinemaTardinessReportModel.NumberOfOffense = 5
        Assert.AreEqual(_cinemaTardinessReportModel.NumberOfOffenseOrdinal, "5th")

        _cinemaTardinessReportModel.NumberOfOffense = 6
        Assert.AreEqual(_cinemaTardinessReportModel.NumberOfOffenseOrdinal, "6th")

        _cinemaTardinessReportModel.NumberOfOffense = 7
        Assert.AreEqual(_cinemaTardinessReportModel.NumberOfOffenseOrdinal, "7th")

        _cinemaTardinessReportModel.NumberOfOffense = 8
        Assert.AreEqual(_cinemaTardinessReportModel.NumberOfOffenseOrdinal, "8th")

        _cinemaTardinessReportModel.NumberOfOffense = 9
        Assert.AreEqual(_cinemaTardinessReportModel.NumberOfOffenseOrdinal, "9th")

        _cinemaTardinessReportModel.NumberOfOffense = 10
        Assert.AreEqual(_cinemaTardinessReportModel.NumberOfOffenseOrdinal, "10th")

        _cinemaTardinessReportModel.NumberOfOffense = 11
        Assert.AreEqual(_cinemaTardinessReportModel.NumberOfOffenseOrdinal, "11th")

        _cinemaTardinessReportModel.NumberOfOffense = 12
        Assert.AreEqual(_cinemaTardinessReportModel.NumberOfOffenseOrdinal, "12th")

        _cinemaTardinessReportModel.NumberOfOffense = 13
        Assert.AreEqual(_cinemaTardinessReportModel.NumberOfOffenseOrdinal, "13th")

        _cinemaTardinessReportModel.NumberOfOffense = 14
        Assert.AreEqual(_cinemaTardinessReportModel.NumberOfOffenseOrdinal, "14th")

        _cinemaTardinessReportModel.NumberOfOffense = 15
        Assert.AreEqual(_cinemaTardinessReportModel.NumberOfOffenseOrdinal, "15th")

        _cinemaTardinessReportModel.NumberOfOffense = 16
        Assert.AreEqual(_cinemaTardinessReportModel.NumberOfOffenseOrdinal, "16th")

        _cinemaTardinessReportModel.NumberOfOffense = 17
        Assert.AreEqual(_cinemaTardinessReportModel.NumberOfOffenseOrdinal, "17th")

        _cinemaTardinessReportModel.NumberOfOffense = 18
        Assert.AreEqual(_cinemaTardinessReportModel.NumberOfOffenseOrdinal, "18th")

        _cinemaTardinessReportModel.NumberOfOffense = 19
        Assert.AreEqual(_cinemaTardinessReportModel.NumberOfOffenseOrdinal, "19th")

        _cinemaTardinessReportModel.NumberOfOffense = 20
        Assert.AreEqual(_cinemaTardinessReportModel.NumberOfOffenseOrdinal, "20th")

        _cinemaTardinessReportModel.NumberOfOffense = 21
        Assert.AreEqual(_cinemaTardinessReportModel.NumberOfOffenseOrdinal, "21st")

        _cinemaTardinessReportModel.NumberOfOffense = 22
        Assert.AreEqual(_cinemaTardinessReportModel.NumberOfOffenseOrdinal, "22nd")

        _cinemaTardinessReportModel.NumberOfOffense = 23
        Assert.AreEqual(_cinemaTardinessReportModel.NumberOfOffenseOrdinal, "23rd")

        _cinemaTardinessReportModel.NumberOfOffense = 24
        Assert.AreEqual(_cinemaTardinessReportModel.NumberOfOffenseOrdinal, "24th")

        _cinemaTardinessReportModel.NumberOfOffense = 25
        Assert.AreEqual(_cinemaTardinessReportModel.NumberOfOffenseOrdinal, "25th")

        _cinemaTardinessReportModel.NumberOfOffense = 26
        Assert.AreEqual(_cinemaTardinessReportModel.NumberOfOffenseOrdinal, "26th")

        _cinemaTardinessReportModel.NumberOfOffense = 27
        Assert.AreEqual(_cinemaTardinessReportModel.NumberOfOffenseOrdinal, "27th")

        _cinemaTardinessReportModel.NumberOfOffense = 28
        Assert.AreEqual(_cinemaTardinessReportModel.NumberOfOffenseOrdinal, "28th")

        _cinemaTardinessReportModel.NumberOfOffense = 29
        Assert.AreEqual(_cinemaTardinessReportModel.NumberOfOffenseOrdinal, "29th")

        _cinemaTardinessReportModel.NumberOfOffense = 30
        Assert.AreEqual(_cinemaTardinessReportModel.NumberOfOffenseOrdinal, "30th")

    End Sub

    <Test>
    Public Sub ShouldReturnProperSanction()

        _cinemaTardinessReportModel.NumberOfOffense = (New System.Random()).Next(-1001, 1)
        Assert.AreEqual(_cinemaTardinessReportModel.Sanction, "-")

        _cinemaTardinessReportModel.NumberOfOffense = (New System.Random()).Next(-1001, 1)
        Assert.AreEqual(_cinemaTardinessReportModel.Sanction, "-")

        _cinemaTardinessReportModel.NumberOfOffense = -1
        Assert.AreEqual(_cinemaTardinessReportModel.Sanction, "-")

        _cinemaTardinessReportModel.NumberOfOffense = 0
        Assert.AreEqual(_cinemaTardinessReportModel.Sanction, "-")

        _cinemaTardinessReportModel.NumberOfOffense = 1
        Assert.AreEqual(_cinemaTardinessReportModel.Sanction, "Written Reprimand")

        _cinemaTardinessReportModel.NumberOfOffense = 2
        Assert.AreEqual(_cinemaTardinessReportModel.Sanction, "2 day Suspension")

        _cinemaTardinessReportModel.NumberOfOffense = 3
        Assert.AreEqual(_cinemaTardinessReportModel.Sanction, "4 days Suspension")

        _cinemaTardinessReportModel.NumberOfOffense = 4
        Assert.AreEqual(_cinemaTardinessReportModel.Sanction, "10 days Suspension")

        _cinemaTardinessReportModel.NumberOfOffense = 5
        Assert.AreEqual(_cinemaTardinessReportModel.Sanction, "Dismissal with Due Process")

        _cinemaTardinessReportModel.NumberOfOffense = 6
        Assert.AreEqual(_cinemaTardinessReportModel.Sanction, "HR: FOR IMMEDIATE ACTION")

        _cinemaTardinessReportModel.NumberOfOffense = 7
        Assert.AreEqual(_cinemaTardinessReportModel.Sanction, "HR: FOR IMMEDIATE ACTION")

        _cinemaTardinessReportModel.NumberOfOffense = 8
        Assert.AreEqual(_cinemaTardinessReportModel.Sanction, "HR: FOR IMMEDIATE ACTION")

        _cinemaTardinessReportModel.NumberOfOffense = 9
        Assert.AreEqual(_cinemaTardinessReportModel.Sanction, "HR: FOR IMMEDIATE ACTION")

        _cinemaTardinessReportModel.NumberOfOffense = 10
        Assert.AreEqual(_cinemaTardinessReportModel.Sanction, "HR: FOR IMMEDIATE ACTION")

        _cinemaTardinessReportModel.NumberOfOffense = (New System.Random()).Next(10, 10001)
        Assert.AreEqual(_cinemaTardinessReportModel.Sanction, "HR: FOR IMMEDIATE ACTION")

        _cinemaTardinessReportModel.NumberOfOffense = (New System.Random()).Next(10, 10001)
        Assert.AreEqual(_cinemaTardinessReportModel.Sanction, "HR: FOR IMMEDIATE ACTION")

    End Sub

End Class