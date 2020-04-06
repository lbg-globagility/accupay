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
            .PayToDate = New Date(2020, 1, 15),
            .Month = 3,
            .Year = 2020
        }

        Dim service = New LeaveAccrualService()

        ' Act
        service.ComputeAccrual(employee, payperiod)

        ' Assert
        ' No assertions yet
    End Sub

End Class
