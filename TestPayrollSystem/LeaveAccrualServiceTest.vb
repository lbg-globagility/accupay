Option Strict On

Imports AccuPay.Data.Entities
Imports AccuPay.Data.Services

<TestFixture>
Public Class LeaveAccrualServiceTest

    <Test>
    Public Sub ShouldComputeCorrectHours_WhenPayperiodIsInTheMiddle()
        ' Arrange
        Dim employee = New Employee() With {
            .VacationLeaveAllowance = 40,
            .SickLeaveAllowance = 40
        }

        Dim payperiod = New PayPeriod() With {
            .PayFromDate = New Date(2020, 1, 1),
            .PayToDate = New Date(2020, 1, 15)
        }

        Dim firstPayperiod = New PayPeriod() With {
            .PayFromDate = New Date(2019, 12, 16),
            .PayToDate = New Date(2019, 12, 31)
        }

        Dim lastPayperiod = New PayPeriod() With {
            .PayFromDate = New Date(2020, 12, 1),
            .PayToDate = New Date(2020, 12, 15)
        }

        Dim sut = New LeaveAccrualCalculator()

        ' Act
        Dim result = sut.Calculate(employee, payperiod, employee.VacationLeaveAllowance, firstPayperiod, lastPayperiod)

        ' Assert
        Assert.AreEqual(1.64D, result)
    End Sub

    <Test>
    Public Sub ShouldComputeCorrectHours_WhenPayperiodIsTheFirst()
        ' Arrange
        Dim employee = New Employee() With {
            .VacationLeaveAllowance = 40,
            .SickLeaveAllowance = 40
        }

        Dim payperiod = New PayPeriod() With {
            .RowID = 1,
            .PayFromDate = New Date(2019, 12, 16),
            .PayToDate = New Date(2019, 12, 31)
        }

        Dim firstPayperiod = New PayPeriod() With {
            .RowID = 1,
            .PayFromDate = New Date(2019, 12, 16),
            .PayToDate = New Date(2019, 12, 31)
        }

        Dim lastPayperiod = New PayPeriod() With {
            .RowID = 2,
            .PayFromDate = New Date(2020, 12, 1),
            .PayToDate = New Date(2020, 12, 15)
        }

        Dim sut = New LeaveAccrualCalculator()

        ' Act
        Dim result = sut.Calculate(employee, payperiod, employee.VacationLeaveAllowance, firstPayperiod, lastPayperiod)

        ' Assert
        Assert.AreEqual(1.75D, result)
    End Sub

    <Test>
    Public Sub ShouldComputeCorrectHours_WhenEmployeeStartDateIsInTheMiddleOfTheCutoff()
        ' Arrange
        Dim employee = New Employee() With {
            .StartDate = New Date(2020, 1, 11),
            .VacationLeaveAllowance = 40,
            .SickLeaveAllowance = 40
        }

        Dim payperiod = New PayPeriod() With {
            .PayFromDate = New Date(2020, 1, 1),
            .PayToDate = New Date(2020, 1, 15)
        }

        Dim firstPayperiod = New PayPeriod() With {
            .PayFromDate = New Date(2019, 12, 16),
            .PayToDate = New Date(2019, 12, 31)
        }

        Dim lastPayperiod = New PayPeriod() With {
            .PayFromDate = New Date(2020, 12, 1),
            .PayToDate = New Date(2020, 12, 15)
        }

        Dim sut = New LeaveAccrualCalculator()

        ' Act
        Dim result = sut.Calculate(employee, payperiod, employee.VacationLeaveAllowance, firstPayperiod, lastPayperiod)

        ' Assert
        Assert.AreEqual(0.55D, result)
    End Sub

    <Test>
    Public Sub ShouldComputeCorrectHours_WhenEmployeeStartDateIsInTheLastCutoff()
        ' Arrange
        Dim employee = New Employee() With {
            .StartDate = New Date(2020, 1, 11),
            .VacationLeaveAllowance = 40,
            .SickLeaveAllowance = 40
        }

        Dim payperiod = New PayPeriod() With {
            .PayFromDate = New Date(2021, 1, 1),
            .PayToDate = New Date(2021, 1, 15)
        }

        Dim firstPayperiod = New PayPeriod() With {
            .PayFromDate = New Date(2020, 12, 16),
            .PayToDate = New Date(2020, 12, 31)
        }

        Dim lastPayperiod = New PayPeriod() With {
            .PayFromDate = New Date(2021, 12, 1),
            .PayToDate = New Date(2021, 12, 15)
        }

        Dim sut = New LeaveAccrualCalculator()

        ' Act
        Dim result = sut.Calculate(employee, payperiod, employee.VacationLeaveAllowance, firstPayperiod, lastPayperiod)

        ' Assert
        Assert.AreEqual(0D, result)
    End Sub

    <Test>
    Public Sub ShouldComputeCorrectHours_WhenFirstCutoff()
        ' Arrange
        Dim employee = New Employee() With {
            .StartDate = New Date(2020, 1, 5),
            .VacationLeaveAllowance = 40,
            .SickLeaveAllowance = 40
        }

        Dim sut = New LeaveAccrualCalculator()

        ' Act
        Dim result = sut.Calculate2(employee, New Date(2020, 2, 6), employee.VacationLeaveAllowance, Nothing)

        ' Assert
        Assert.AreEqual(3.33D, result)
    End Sub

    <Test>
    Public Sub ShouldComputeCorrectHours_WhenSecondCutoff()
        ' Arrange
        Dim employee = New Employee() With {
            .StartDate = New Date(2020, 1, 5),
            .VacationLeaveAllowance = 40,
            .SickLeaveAllowance = 40
        }

        Dim lastTransaction = New LeaveTransaction() With {
            .TransactionDate = New Date(2020, 2, 6)
        }

        Dim sut = New LeaveAccrualCalculator()

        ' Act
        Dim result = sut.Calculate2(employee, New Date(2020, 3, 6), employee.VacationLeaveAllowance, lastTransaction)

        ' Assert
        Assert.AreEqual(3.34D, result)
    End Sub

    <Test>
    Public Sub ShouldNotComputeCorrectHours_WhenDateIsTooEarly()
        ' Arrange
        Dim employee = New Employee() With {
            .StartDate = New Date(2020, 1, 5),
            .VacationLeaveAllowance = 40,
            .SickLeaveAllowance = 40
        }

        Dim sut = New LeaveAccrualCalculator()

        ' Act
        Dim result = sut.Calculate2(employee, New Date(2020, 2, 5), employee.VacationLeaveAllowance, Nothing)

        ' Assert
        Assert.AreEqual(0D, result)
    End Sub

End Class
