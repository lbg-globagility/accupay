Option Strict On

Imports AccuPay
Imports AccuPay.Entity

<TestFixture>
Public Class LeaveAccrualServiceTest

    <Test>
    Public Sub ShouldGiveLeaves()
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
        Dim result = sut.Calculate(payperiod, employee.VacationLeaveAllowance, firstPayperiod, lastPayperiod)

        ' Assert
        Assert.AreEqual(1.64D, result)
    End Sub

End Class
