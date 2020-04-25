Option Strict On

Imports System.IO
Imports AccuPay.Entity
Imports AccuPay.Helper.TimeLogsReader
Imports AccuPay.Tools

Public Class TimeAnalyzer

    Protected _projectPath As String

    Sub New()

        _projectPath = Directory.GetParent(
                            Directory.GetParent(
                                AppDomain.CurrentDomain.BaseDirectory
                            ).Parent.FullName
                        ).FullName

    End Sub

    Protected Function GetParsedTimeLogs() As IList(Of ImportTimeAttendanceLog)
        Dim importer = New TimeLogsReader()

        Dim filename = _projectPath & "\_timelogs_test.dat"

        Dim importOutput = importer.Import(filename)
        Dim logs = importOutput.Logs

        Return logs
    End Function

    Protected Function GetSampleEmployees() As List(Of AccuPay.Data.Entities.Employee)
        Dim employees = New List(Of AccuPay.Data.Entities.Employee)

        Dim employeeId As Integer? = 1

        Dim employee As New AccuPay.Data.Entities.Employee With {
            .RowID = employeeId,
            .EmployeeNo = "10123"
        }

        employees.Add(employee)

        Return employees
    End Function

    Protected Function GetSampleShiftSchedules() As List(Of ShiftSchedule)
        Dim employeeShifts = New List(Of ShiftSchedule)

        Dim employeeShift As ShiftSchedule

        Dim employeeId As Integer? = 1
        Dim employeeId_2 As Integer? = 2

        employeeShift = New ShiftSchedule() With {
            .EmployeeID = employeeId,
            .EffectiveFrom = Date.Parse("2018-06-01"),
            .EffectiveTo = Date.Parse("2018-06-01"),
            .Shift = New Shift() With {
                .TimeFrom = TimeSpan.Parse("08:00:00"),
                .TimeTo = TimeSpan.Parse("17:00:00")
            }
        }

        employeeShifts.Add(employeeShift)

        employeeShift = New ShiftSchedule() With {
            .EmployeeID = employeeId,
            .EffectiveFrom = Date.Parse("2018-06-02"),
            .EffectiveTo = Date.Parse("2018-06-02"),
            .Shift = New Shift() With {
                .TimeFrom = TimeSpan.Parse("08:00:00"),
                .TimeTo = TimeSpan.Parse("17:00:00")
            }
        }

        employeeShifts.Add(employeeShift)

        employeeShift = New ShiftSchedule() With {
            .EmployeeID = employeeId_2,
            .EffectiveFrom = Date.Parse("2018-06-01"),
            .EffectiveTo = Date.Parse("2018-06-01"),
            .Shift = New Shift() With {
                .TimeFrom = TimeSpan.Parse("08:00:00"),
                .TimeTo = TimeSpan.Parse("17:00:00")
            }
        }

        employeeShifts.Add(employeeShift)

        employeeShift = New ShiftSchedule() With {
            .EmployeeID = employeeId_2,
            .EffectiveFrom = Date.Parse("2018-06-02"),
            .EffectiveTo = Date.Parse("2018-06-02"),
            .Shift = New Shift() With {
                .TimeFrom = TimeSpan.Parse("08:00:00"),
                .TimeTo = TimeSpan.Parse("17:00:00")
            }
        }

        employeeShifts.Add(employeeShift)

        employeeShift = New ShiftSchedule() With {
            .EmployeeID = employeeId,
            .EffectiveFrom = Date.Parse("2018-06-03"),
            .EffectiveTo = Date.Parse("2018-06-03"),
            .Shift = New Shift() With {
                .TimeFrom = TimeSpan.Parse("08:00:00"),
                .TimeTo = TimeSpan.Parse("17:00:00")
            }
        }

        employeeShifts.Add(employeeShift)

        employeeShift = New ShiftSchedule() With {
            .EmployeeID = employeeId,
            .EffectiveFrom = Date.Parse("2018-06-04"),
            .EffectiveTo = Date.Parse("2018-06-04"),
            .Shift = New Shift() With {
                .TimeFrom = TimeSpan.Parse("08:00:00"),
                .TimeTo = TimeSpan.Parse("17:00:00")
            }
        }
        employeeShifts.Add(employeeShift)

        employeeShift = New ShiftSchedule() With {
            .EmployeeID = employeeId,
            .EffectiveFrom = Date.Parse("2018-06-05"),
            .EffectiveTo = Date.Parse("2018-06-05"),
            .Shift = New Shift() With {
                .TimeFrom = TimeSpan.Parse("08:00:00"),
                .TimeTo = TimeSpan.Parse("17:00:00")
            }
        }
        employeeShifts.Add(employeeShift)
        employeeShift = New ShiftSchedule() With {
            .EmployeeID = employeeId,
            .EffectiveFrom = Date.Parse("2018-06-06"),
            .EffectiveTo = Date.Parse("2018-06-06"),
            .Shift = New Shift() With {
                .TimeFrom = TimeSpan.Parse("08:00:00"),
                .TimeTo = TimeSpan.Parse("17:00:00")
            }
        }
        employeeShifts.Add(employeeShift)
        employeeShift = New ShiftSchedule() With {
            .EmployeeID = employeeId,
            .EffectiveFrom = Date.Parse("2018-06-07"),
            .EffectiveTo = Date.Parse("2018-06-07"),
            .Shift = New Shift() With {
                .TimeFrom = TimeSpan.Parse("08:00:00"),
                .TimeTo = TimeSpan.Parse("17:00:00")
            }
        }
        employeeShifts.Add(employeeShift)

        Return employeeShifts
    End Function

    Protected Function GetSampleShiftSchedules_WithNextShiftScheduleWithoutShift() As List(Of ShiftSchedule)
        Dim employeeShifts = New List(Of ShiftSchedule)

        Dim employeeShift As ShiftSchedule

        Dim employeeId As Integer? = 1
        Dim employeeId_2 As Integer? = 2

        employeeShift = New ShiftSchedule() With {
            .EmployeeID = employeeId,
            .EffectiveFrom = Date.Parse("2018-06-01"),
            .EffectiveTo = Date.Parse("2018-06-01"),
            .Shift = New Shift() With {
                .TimeFrom = TimeSpan.Parse("08:00:00"),
                .TimeTo = TimeSpan.Parse("17:00:00")
            }
        }

        employeeShifts.Add(employeeShift)

        employeeShift = New ShiftSchedule() With {
            .EmployeeID = employeeId,
            .EffectiveFrom = Date.Parse("2018-06-02"),
            .EffectiveTo = Date.Parse("2018-06-02"),
            .Shift = New Shift() With {
                .TimeFrom = TimeSpan.Parse("08:00:00"),
                .TimeTo = TimeSpan.Parse("17:00:00")
            }
        }

        employeeShifts.Add(employeeShift)

        employeeShift = New ShiftSchedule() With {
            .EmployeeID = employeeId_2,
            .EffectiveFrom = Date.Parse("2018-06-01"),
            .EffectiveTo = Date.Parse("2018-06-01"),
            .Shift = New Shift() With {
                .TimeFrom = TimeSpan.Parse("08:00:00"),
                .TimeTo = TimeSpan.Parse("17:00:00")
            }
        }

        employeeShifts.Add(employeeShift)

        employeeShift = New ShiftSchedule() With {
            .EmployeeID = employeeId_2,
            .EffectiveFrom = Date.Parse("2018-06-02"),
            .EffectiveTo = Date.Parse("2018-06-02"),
            .Shift = New Shift() With {
                .TimeFrom = TimeSpan.Parse("08:00:00"),
                .TimeTo = TimeSpan.Parse("17:00:00")
            }
        }

        employeeShifts.Add(employeeShift)

        employeeShift = New ShiftSchedule() With {
            .EmployeeID = employeeId,
            .EffectiveFrom = Date.Parse("2018-06-03"),
            .EffectiveTo = Date.Parse("2018-06-03"),
            .Shift = New Shift() With {
                .TimeFrom = TimeSpan.Parse("08:00:00"),
                .TimeTo = TimeSpan.Parse("17:00:00")
            }
        }

        employeeShifts.Add(employeeShift)

        employeeShift = New ShiftSchedule() With {
            .EmployeeID = employeeId,
            .EffectiveFrom = Date.Parse("2018-06-04"),
            .EffectiveTo = Date.Parse("2018-06-04"),
            .Shift = New Shift() With {
                .TimeFrom = TimeSpan.Parse("08:00:00"),
                .TimeTo = TimeSpan.Parse("17:00:00")
            }
        }
        employeeShifts.Add(employeeShift)

        employeeShift = New ShiftSchedule() With {
            .EmployeeID = employeeId,
            .EffectiveFrom = Date.Parse("2018-06-05"),
            .EffectiveTo = Date.Parse("2018-06-05"),
            .Shift = New Shift() With {
                .TimeFrom = TimeSpan.Parse("08:00:00"),
                .TimeTo = TimeSpan.Parse("17:00:00")
            }
        }
        employeeShifts.Add(employeeShift)
        employeeShift = New ShiftSchedule() With {
            .EmployeeID = employeeId,
            .EffectiveFrom = Date.Parse("2018-06-06"),
            .EffectiveTo = Date.Parse("2018-06-06"),
            .Shift = New Shift() With {
                .TimeFrom = TimeSpan.Parse("08:00:00"),
                .TimeTo = TimeSpan.Parse("17:00:00")
            }
        }
        employeeShifts.Add(employeeShift)
        employeeShift = New ShiftSchedule() With {
            .EmployeeID = employeeId,
            .EffectiveFrom = Date.Parse("2018-06-07"),
            .EffectiveTo = Date.Parse("2018-06-07"),
            .Shift = Nothing
        }
        employeeShifts.Add(employeeShift)

        Return employeeShifts
    End Function

    Protected Function GetSampleEmployeeDutySchedules() As List(Of EmployeeDutySchedule)

        Dim shiftSchedules = GetSampleShiftSchedules()

        Return ConvertShiftSchedulesListToEmployeeDutyScheduleList(shiftSchedules)

    End Function

    Protected Function GetSampleEmployeeOvertimes() As List(Of AccuPay.Data.Entities.Overtime)

        Return New List(Of AccuPay.Data.Entities.Overtime)

    End Function

    Protected Function GetSampleEmployeeDutySchedules_WithNextShiftScheduleWithoutShift() _
                            As List(Of EmployeeDutySchedule)

        Dim shiftSchedules = GetSampleShiftSchedules_WithNextShiftScheduleWithoutShift()

        Return ConvertShiftSchedulesListToEmployeeDutyScheduleList(shiftSchedules)

    End Function

    Protected Sub AssertTimeLog(time As TimeLog, correctTimeIn As String, correctTimeOut As String, correctDate As String)
        Dim timeIn As TimeSpan?
        If correctTimeIn Is Nothing Then
            timeIn = Nothing
        Else
            Try
                timeIn = TimeSpan.Parse(correctTimeIn)
            Catch ex As Exception
                timeIn = Nothing
            End Try
        End If

        Dim timeOut As TimeSpan?
        If correctTimeOut Is Nothing Then
            timeOut = Nothing
        Else
            Try
                timeOut = TimeSpan.Parse(correctTimeOut)
            Catch ex As Exception
                timeOut = Nothing
            End Try
        End If

        Assert.Multiple(
            Sub()
                If String.IsNullOrWhiteSpace(correctTimeIn) Then
                    Assert.That(time.TimeIn, [Is].Null, "Date", correctDate)
                Else
                    Assert.That(time.TimeIn, [Is].EqualTo(timeIn), correctDate)
                End If

                Assert.That(time.TimeOut, [Is].EqualTo(timeOut), correctDate)
                Assert.That(time.LogDate.Date, [Is].EqualTo(DateTime.Parse(correctDate)))
            End Sub)
    End Sub

    Private Function ConvertShiftSchedulesListToEmployeeDutyScheduleList(shiftSchedules As List(Of ShiftSchedule)) _
        As List(Of EmployeeDutySchedule)

        Dim employeeDutySchedules As New List(Of EmployeeDutySchedule)

        For Each shiftSchedule In shiftSchedules

            Dim daysSpan = Calendar.EachDay(shiftSchedule.EffectiveFrom, shiftSchedule.EffectiveTo)

            For Each currentDate In daysSpan

                employeeDutySchedules.Add(New EmployeeDutySchedule With {
                    .EmployeeID = shiftSchedule.EmployeeID,
                    .DateSched = currentDate,
                    .StartTime = shiftSchedule.Shift?.TimeFrom,
                    .EndTime = shiftSchedule.Shift?.TimeTo
                })

            Next

        Next

        Return employeeDutySchedules

    End Function

End Class