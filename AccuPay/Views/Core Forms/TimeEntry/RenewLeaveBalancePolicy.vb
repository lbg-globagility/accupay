Public Class RenewLeaveBalancePolicy

    Private _isProrateOnFirstAnniv As Boolean = False

    Public Property ProrateOnFirstAnniversary As Boolean
        Get
            Return _isProrateOnFirstAnniv
        End Get
        Set(value As Boolean)
            _isProrateOnFirstAnniv = value
        End Set
    End Property

    Public Property LeaveAllowanceAmount As LeaveAllowanceAmountBasis

    Public Enum LeaveAllowanceAmountBasis
        [Default]
        NumberOfService
    End Enum

End Class