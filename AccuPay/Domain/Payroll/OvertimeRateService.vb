Option Strict On

Imports System.Threading.Tasks
Imports AccuPay.Utilities.Extensions
Imports Microsoft.EntityFrameworkCore

Public Class OvertimeRateService

    Public Shared Async Function GetOvertimeRates() As Task(Of OvertimeRate)

        Dim listOfValuesType = "Pay rate"
        Dim regularDayLIC = "Regular Day"
        Dim specialHolidayLIC = "Special Non-Working Holiday"
        Dim regularHolidayLIC = "Regular Holiday"
        Dim doubleHolidayLIC = "Double Holiday"

        Using context As New PayrollContext

            Dim basePay As Decimal
            Dim overtime As Decimal
            Dim nightDifferential As Decimal
            Dim nightDifferentialOvertime As Decimal
            Dim restDay As Decimal
            Dim restDayOvertime As Decimal
            Dim restDayNightDifferential As Decimal
            Dim restDayNightDifferentialOvertime As Decimal
            Dim specialHoliday As Decimal
            Dim specialHolidayOvertime As Decimal
            Dim specialHolidayNightDifferential As Decimal
            Dim specialHolidayNightDifferentialOvertime As Decimal
            Dim specialHolidayRestDay As Decimal
            Dim specialHolidayRestDayOvertime As Decimal
            Dim specialHolidayRestDayNightDifferential As Decimal
            Dim specialHolidayRestDayNightDifferentialOvertime As Decimal
            Dim regularHoliday As Decimal
            Dim regularHolidayOvertime As Decimal
            Dim regularHolidayNightDifferential As Decimal
            Dim regularHolidayNightDifferentialOvertime As Decimal
            Dim regularHolidayRestDay As Decimal
            Dim regularHolidayRestDayOvertime As Decimal
            Dim regularHolidayRestDayNightDifferential As Decimal
            Dim regularHolidayRestDayNightDifferentialOvertime As Decimal
            Dim doubleHoliday As Decimal
            Dim doubleHolidayOvertime As Decimal
            Dim doubleHolidayNightDifferential As Decimal
            Dim doubleHolidayNightDifferentialOvertime As Decimal
            Dim doubleHolidayRestDay As Decimal
            Dim doubleHolidayRestDayOvertime As Decimal
            Dim doubleHolidayRestDayNightDifferential As Decimal
            Dim doubleHolidayRestDayNightDifferentialOvertime As Decimal

            Dim payRates = Await context.ListOfValues.
                            Where(Function(l) l.Type = listOfValuesType).
                            ToListAsync

            Dim regularDayRate = payRates.
                                    Where(Function(l) l.ParentLIC = regularDayLIC).
                                    FirstOrDefault?.DisplayValue

            If String.IsNullOrWhiteSpace(regularDayRate) = False Then

                Dim regularDayRates = Split(regularDayRate, ",")

                basePay = ConvertToRate(regularDayRates(0))
                overtime = ConvertToRate(regularDayRates(1))
                nightDifferential = ConvertToRate(regularDayRates(2))
                nightDifferentialOvertime = ConvertToRate(regularDayRates(3))
                restDay = ConvertToRate(regularDayRates(4))
                restDayOvertime = ConvertToRate(regularDayRates(5))
                restDayNightDifferential = ConvertToRate(regularDayRates(6))
                restDayNightDifferentialOvertime = ConvertToRate(regularDayRates(7))

            End If

            Dim specialHolidayRate = payRates.
                                    Where(Function(l) l.ParentLIC = specialHolidayLIC).
                                    FirstOrDefault?.DisplayValue

            If String.IsNullOrWhiteSpace(specialHolidayRate) = False Then

                Dim specialHolidayRates = Split(specialHolidayRate, ",")

                specialHoliday = ConvertToRate(specialHolidayRates(0))
                specialHolidayOvertime = ConvertToRate(specialHolidayRates(1))
                specialHolidayNightDifferential = ConvertToRate(specialHolidayRates(2))
                specialHolidayNightDifferentialOvertime = ConvertToRate(specialHolidayRates(3))
                specialHolidayRestDay = ConvertToRate(specialHolidayRates(4))
                specialHolidayRestDayOvertime = ConvertToRate(specialHolidayRates(5))
                specialHolidayRestDayNightDifferential = ConvertToRate(specialHolidayRates(6))
                specialHolidayRestDayNightDifferentialOvertime = ConvertToRate(specialHolidayRates(7))

            End If

            Dim regularHolidayRate = payRates.
                                    Where(Function(l) l.ParentLIC = regularHolidayLIC).
                                    FirstOrDefault?.DisplayValue

            If String.IsNullOrWhiteSpace(regularHolidayRate) = False Then

                Dim regularHolidayRates = Split(regularHolidayRate, ",")

                regularHoliday = ConvertToRate(regularHolidayRates(0))
                regularHolidayOvertime = ConvertToRate(regularHolidayRates(1))
                regularHolidayNightDifferential = ConvertToRate(regularHolidayRates(2))
                regularHolidayNightDifferentialOvertime = ConvertToRate(regularHolidayRates(3))
                regularHolidayRestDay = ConvertToRate(regularHolidayRates(4))
                regularHolidayRestDayOvertime = ConvertToRate(regularHolidayRates(5))
                regularHolidayRestDayNightDifferential = ConvertToRate(regularHolidayRates(6))
                regularHolidayRestDayNightDifferentialOvertime = ConvertToRate(regularHolidayRates(7))

            End If

            Dim doubleHolidayRate = payRates.
                                    Where(Function(l) l.ParentLIC = doubleHolidayLIC).
                                    FirstOrDefault?.DisplayValue

            If String.IsNullOrWhiteSpace(doubleHolidayRate) = False Then

                Dim doubleHolidayRates = Split(doubleHolidayRate, ",")

                doubleHoliday = ConvertToRate(doubleHolidayRates(0))
                doubleHolidayOvertime = ConvertToRate(doubleHolidayRates(1))
                doubleHolidayNightDifferential = ConvertToRate(doubleHolidayRates(2))
                doubleHolidayNightDifferentialOvertime = ConvertToRate(doubleHolidayRates(3))
                doubleHolidayRestDay = ConvertToRate(doubleHolidayRates(4))
                doubleHolidayRestDayOvertime = ConvertToRate(doubleHolidayRates(5))
                doubleHolidayRestDayNightDifferential = ConvertToRate(doubleHolidayRates(6))
                doubleHolidayRestDayNightDifferentialOvertime = ConvertToRate(doubleHolidayRates(7))

            End If

            Return New OvertimeRate(
                basePay:=basePay,
                overtime:=overtime,
                nightDifferential:=nightDifferential,
                nightDifferentialOvertime:=nightDifferentialOvertime,
                restDay:=restDay,
                restDayOvertime:=restDayOvertime,
                restDayNightDifferential:=restDayNightDifferential,
                restDayNightDifferentialOvertime:=restDayNightDifferentialOvertime,
                specialHoliday:=specialHoliday,
                specialHolidayOvertime:=specialHolidayOvertime,
                specialHolidayNightDifferential:=specialHolidayNightDifferential,
                specialHolidayNightDifferentialOvertime:=specialHolidayNightDifferentialOvertime,
                specialHolidayRestDay:=specialHolidayRestDay,
                specialHolidayRestDayOvertime:=specialHolidayRestDayOvertime,
                specialHolidayRestDayNightDifferential:=specialHolidayRestDayNightDifferential,
                specialHolidayRestDayNightDifferentialOvertime:=specialHolidayRestDayNightDifferentialOvertime,
                regularHoliday:=regularHoliday,
                regularHolidayOvertime:=regularHolidayOvertime,
                regularHolidayNightDifferential:=regularHolidayNightDifferential,
                regularHolidayNightDifferentialOvertime:=regularHolidayNightDifferentialOvertime,
                regularHolidayRestDay:=regularHolidayRestDay,
                regularHolidayRestDayOvertime:=regularHolidayRestDayOvertime,
                regularHolidayRestDayNightDifferential:=regularHolidayRestDayNightDifferential,
                regularHolidayRestDayNightDifferentialOvertime:=regularHolidayRestDayNightDifferentialOvertime,
                doubleHoliday:=doubleHoliday,
                doubleHolidayOvertime:=doubleHolidayOvertime,
                doubleHolidayNightDifferential:=doubleHolidayNightDifferential,
                doubleHolidayNightDifferentialOvertime:=doubleHolidayNightDifferentialOvertime,
                doubleHolidayRestDay:=doubleHolidayRestDay,
                doubleHolidayRestDayOvertime:=doubleHolidayRestDayOvertime,
                doubleHolidayRestDayNightDifferential:=doubleHolidayRestDayNightDifferential,
                doubleHolidayRestDayNightDifferentialOvertime:=doubleHolidayRestDayNightDifferentialOvertime)

        End Using

    End Function

    Private Shared Function ConvertToRate(input As String) As Decimal

        If input.Length > 2 Then

            'add a decimal to convert it properly
            'since the inputs are either
            '286 which is equals 2.86 or
            '1375 which is equals 1.375
            input = input.Insert(1, ".")

        End If

        Return input.ToDecimal

    End Function

End Class
