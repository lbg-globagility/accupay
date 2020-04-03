Option Strict On

Namespace Global.AccuPay.Entity

    Public Interface IPayrate

        ReadOnly Property [Date] As Date

        ReadOnly Property RegularRate As Decimal

        ReadOnly Property OvertimeRate As Decimal

        ReadOnly Property NightDiffRate As Decimal

        ReadOnly Property NightDiffOTRate As Decimal

        ReadOnly Property RestDayRate As Decimal

        ReadOnly Property RestDayOTRate As Decimal

        ReadOnly Property RestDayNDRate As Decimal

        ReadOnly Property RestDayNDOTRate As Decimal

        ReadOnly Property IsRegularDay As Boolean

        ReadOnly Property IsRegularHoliday As Boolean

        ReadOnly Property IsSpecialNonWorkingHoliday As Boolean

        ReadOnly Property IsHoliday As Boolean

        ReadOnly Property IsDoubleHoliday As Boolean

    End Interface

End Namespace
